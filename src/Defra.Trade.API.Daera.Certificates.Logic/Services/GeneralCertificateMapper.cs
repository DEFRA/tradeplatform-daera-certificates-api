// Copyright DEFRA (c). All rights reserved.
// Licensed under the Open Government License v3.0.

using System.Globalization;
using Defra.Trade.API.Daera.Certificates.Database.Models;
using Defra.Trade.API.Daera.Certificates.Logic.Infrastructure;
using Defra.Trade.API.Daera.Certificates.Logic.Models.GeneralCertificate;
using Defra.Trade.API.Daera.Certificates.Logic.Services.Interfaces;
using Microsoft.Extensions.Options;
using EhcoModels = Defra.Trade.API.Daera.Certificates.Logic.Models.Ehco;

using GcModels = Defra.Trade.API.Daera.Certificates.Logic.Models.GeneralCertificate;

using IdcomsModels = Defra.Trade.API.Daera.Certificates.Logic.Models.Idcoms;

namespace Defra.Trade.API.Daera.Certificates.Logic.Services;

public class GeneralCertificateMapper(IOptions<ApimExternalApisSettings> apiSettings) : IModelMapper<Database.Models.GeneralCertificate, GcModels.GeneralCertificate>
{
    private readonly IOptions<ApimExternalApisSettings> _apiSettings = apiSettings ?? throw new ArgumentNullException(nameof(apiSettings));

    public static string GetDesiredDateFormat(DateTimeOffset datetime)
    {
        return datetime.ToString("yyyyMMddHHmmzzz");
    }

    public GcModels.GeneralCertificate Map(Database.Models.GeneralCertificate generalCertificateData)
    {
        var gcDocuments = generalCertificateData.GeneralCertificateDocument ?? [];
        var ehcoGc = JsonSerializer.Deserialize<Models.Ehco.EhcoGeneralCertificateApplication>(generalCertificateData.Data, SerializerOptions.SerializerOptions.GetSerializerOptions());
        var idcomsEnrichmentData = JsonSerializer.Deserialize<IdcomsModels.IdcomsGeneralCertificateEnrichment>(generalCertificateData.EnrichmentData.Data, SerializerOptions.SerializerOptions.GetSerializerOptions());

        var generalCertificate = new GcModels.GeneralCertificate
        {
            ExchangedDocument = MapExchangedDocument(ehcoGc.ExchangedDocument, gcDocuments.ToList(), idcomsEnrichmentData),
            SupplyChainConsignment = MapSupplyChainConsignment(ehcoGc.SupplyChainConsignment, idcomsEnrichmentData)
        };

        return generalCertificate;
    }

    private static DateTimeOffset GetDateTimeFromString(string datetime)
    {
        try
        {
            var datetimeResult = DateTimeOffset.ParseExact(
                datetime,
                "dd/MM/yyyy",
                CultureInfo.InvariantCulture,
                DateTimeStyles.AssumeUniversal);
            return datetimeResult;
        }
        catch
        {
            return DateTimeOffset.MinValue;
        }
    }

    private static List<GcModels.IDType> GetId(IdcomsModels.Establishment establishment)
    {
        return
        [
            MapIdType(establishment.RmsId, null, "RMS")
        ];
    }

    private static List<GcModels.IDType> GetIdFromOrganisation(IdcomsModels.Organisation organisation)
    {
        return
                   [
                       MapIdType(organisation.RmsId, null,"RMS")
                   ];
    }

    private static List<GcModels.TextType> GetNameFromOrganisation(IdcomsModels.Organisation organisation)
    {
        return
                   [
                       new ()
                       {
                           Content = organisation.Name,
                           LanguageId = "en"
                       }
                   ];
    }

    private static GcModels.TradeAddress MapAddress(EhcoModels.Operator operatorOperator)
    {
        return new GcModels.TradeAddress
        {
            LineOne = MapTextType(operatorOperator.LineOne),
            LineTwo = MapTextType(operatorOperator.LineTwo),
            CityName = MapTextType(operatorOperator.CityName),
            Postcode = MapModeCode(operatorOperator.Postcode, "5"),
            CountryCode = MapIdType(operatorOperator.CountryCode, "5"),
            TypeCode = MapModeCode("3")
        };
    }

    private static GcModels.TradeAddress MapAddressFromOrganisation(IdcomsModels.Address address, string typeCode)
    {
        return new()
        {
            Postcode = MapModeCode(address.PostCode),
            LineOne = MapTextType(address.AddressLine1),
            LineTwo = MapTextType(address.AddressLine2),
            LineThree = MapTextType(address.AddressLine3),
            LineFour = MapTextType(address.AddressLine4),
            LineFive = MapTextType(address.AddressLine5),
            CityName = MapTextType(address.Town),
            CountryCode = MapIdType(address.Country.Code, "5"),
            CountryName = MapTextType(address.Country.Name, "en"),
            CountrySubDivisionCode = MapIdType(address.Country.SubDivisionCode, "5"),
            CountrySubDivisionName = MapTextType(address.Country.SubDivisionName, "en"),
            TypeCode = MapModeCode(typeCode, "6")
        };
    }

    private static List<GcModels.LogisticsSeal> MapAffixedSeal(string source)
    {
        return
        [
            new()
            {
                Id = MapIdType(source)
            }
        ];
    }

    private static GcModels.UniversalCommunication MapCommunication(string source)
    {
        return new GcModels.UniversalCommunication
        {
            CompleteNumber = MapTextType(source, "en")
        };
    }

    private static GcModels.TradeCountry MapCountry(string source)
    {
        if (string.IsNullOrWhiteSpace(source))
            return null;

        return new GcModels.TradeCountry
        {
            Code = MapIdType(source, "5")
        };
    }

    private static GcModels.DocumentAuthentication MapDocumentAuthentication(EhcoModels.ExchangedDocument exchangedDocument, IdcomsModels.IdcomsGeneralCertificateEnrichment enrichment)
    {
        var result = new GcModels.DocumentAuthentication
        {
            IncludedDocumentClause =
            [
                new()
                {
                    Content = MapTextType("I, the undersigned operator responsible for the consignment detailed above, certify that to the best of my knowledge and belief the statements made in Part I of this document are true and complete, and I agree to comply with the requirements of Regulation (EU) 2017/625 on official controls, including payment for official controls, as well as for re-dispatching consignments, quarantine or isolation of animals, or costs of euthanasia and disposal where necessary.", "en"),
                    Id = MapIdType("GC Declaration Clause 873", null, "DC")
                }
            ]
        };

        if (exchangedDocument.ApplicationSubmissionDateTime != default)
        {
            result.ActualDateTime = new GcModels.DateTimeType
            {
                Content = GetDesiredDateFormat(exchangedDocument.ApplicationSubmissionDateTime),
                Format = "205"
            };
        }

        if (exchangedDocument.Applicant?.DefraCustomer is not null)
        {
            var applicantOrg = enrichment.Organisations.FirstOrDefault(
                x => x.DefraCustomerId == exchangedDocument.Applicant.DefraCustomer.OrgId);
            if (applicantOrg is null)
            {
                return null;
            }
            var applicantName = enrichment.Applicant;
            result.ProvidingTradeParty = new GcModels.TradeParty
            {
                Id =
                [
                    MapIdType(applicantOrg.RmsId, null, "RMS")
                ],
                Name =
                [
                    MapTextType(applicantOrg.Name, "en")
                ],
                RoleCode = [MapModeCode(GcConstants.RoleCodeDgp)],
                AuthoritativeSignatoryPerson = new AuthoritativeSignatoryPerson
                {
                    Name = MapTextType(applicantName.Name, "en")
                }
            };
        }

        return result;
    }

    private static GcModels.IDType MapIdType(string content, string schemaAgencyId = null, string schemaId = null)
    {
        return new()
        {
            Content = content,
            SchemeAgencyId = schemaAgencyId ?? string.Empty,
            SchemeId = schemaId ?? string.Empty
        };
    }

    private static GcModels.LogisticsLocation MapLocation(IdcomsModels.Establishment establishment, string typeCode)
    {
        if (establishment == null)
            return null;

        return new GcModels.LogisticsLocation
        {
            Id = GetId(establishment),
            Name = MapTextType(establishment.Name, "en"),
            LocationAddress = MapAddressFromOrganisation(establishment.Address, typeCode),
        };
    }

    private static GcModels.LogisticsLocation MapLogisticsEstablishmentId(Guid establishmentId, IReadOnlyCollection<IdcomsModels.Establishment> establishments)
    {
        string dispatchAndDestiLocationTypeCode = "3";
        var enrichedEstablishment = establishments.FirstOrDefault(x => x.EstablishmentId == establishmentId);

        return MapLocation(enrichedEstablishment, dispatchAndDestiLocationTypeCode);
    }

    private static GcModels.CodeType MapModeCode(string source, string listAgencyId = null)
    {
        return new GcModels.CodeType
        {
            Content = source,
            ListAgencyID = listAgencyId
        };
    }

    private static GcModels.LogisticsTransportMovement MapMovement(EhcoModels.LogisticsTransportMeans logisticsTransportMeans)
    {
        return new GcModels.LogisticsTransportMovement
        {
            ModeCode = MapModeCode(logisticsTransportMeans.ModeCode, "6"),
            UsedTransportMeans = new GcModels.LogisticsTransportMeans
            {
                Id = MapIdType(logisticsTransportMeans.ID)
            }
        };
    }

    private static GcModels.SupplyChainConsignment MapSupplyChainConsignment(Models.Ehco.SupplyChainConsignment supplyChainConsignment, IdcomsModels.IdcomsGeneralCertificateEnrichment enrichment)
    {
        var mapped = new GcModels.SupplyChainConsignment
        {
            CustomsId = [MapIdType(supplyChainConsignment.CustomsId, null, "GMR")],
            ExportExitDateTime = new GcModels.DateTimeType
            {
                Content = GetDesiredDateFormat(GetDateTimeFromString(supplyChainConsignment.ExportExitDateTime)),
                Format = "205"
            },
            Consignee = MapTradePartyOrgId(supplyChainConsignment.Consignee.DefraCustomer.OrgId, enrichment.Organisations),
            Consignor = MapTradePartyOrgId(supplyChainConsignment.Consignor.DefraCustomer.OrgId, enrichment.Organisations),
            OperatorResponsibleForConsignment = OperatorToTradePartyMapper(supplyChainConsignment.OperatorResponsibleForConsignment),
            BorderControlPostLocation = new GcModels.LogisticsLocation
            {
                Name = MapTextType(supplyChainConsignment.BorderControlPostLocation)
            },
            DispatchLocation = MapLogisticsEstablishmentId(supplyChainConsignment.DispatchLocation.Idcoms.EstablishmentId, enrichment.Establishments),
            DestinationLocation = MapLogisticsEstablishmentId(supplyChainConsignment.DestinationLocation.Idcoms.EstablishmentId, enrichment.Establishments),
            ExportCountry = MapCountry(supplyChainConsignment.ExportCountry),
            ImportCountry = MapCountry(supplyChainConsignment.ImportCountry),
            UsedTransportEquipment =
            [ new()
                  {
                    Id = MapIdType(supplyChainConsignment.UsedTransportEquipment.TrailerNumber, "5"),
                    AffixedSeal = MapAffixedSeal(supplyChainConsignment.UsedTransportEquipment.AffixedSeal),
                    TemperatureSetting = MapTemperature(supplyChainConsignment.UsedTransportEquipment.TemperatureSetting)
                }
            ],
            MainCarriageTransportMovement = [
                   MapMovement(supplyChainConsignment.UsedTransportMeans)
            ]
        };

        return mapped;
    }

    private static List<GcModels.TransportSettingTemperature> MapTemperature(string source)
    {
        if (string.IsNullOrWhiteSpace(source)
            || !decimal.TryParse(source, out decimal parsed))
        {
            return [];
        }

        return
        [
            new()
            {
                Value = new GcModels.TemperatureUnitMeasure
                {
                    Content = parsed
                },
                TypeCode = MapModeCode("2", "6")
            }
        ];
    }

    private static GcModels.TextType MapTextType(string source, string languageId = null)
    {
        return new GcModels.TextType
        {
            Content = source,
            LanguageId = languageId
        };
    }

    private static GcModels.TradeParty MapTradePartyOrgId(Guid orgId, IReadOnlyCollection<IdcomsModels.Organisation> organisations)
    {
        if (orgId == Guid.Empty)
            return null;

        var enrichedOrganisation = organisations.FirstOrDefault(x => x.DefraCustomerId == orgId);

        return enrichedOrganisation == null ? new GcModels.TradeParty() : new GcModels.TradeParty
        {
            Id = GetIdFromOrganisation(enrichedOrganisation),
            Name = GetNameFromOrganisation(enrichedOrganisation),
            RoleCode = [MapModeCode(GcConstants.RoleCodeDgp)],
            PostalAddress = MapAddressFromOrganisation(enrichedOrganisation.Address, "1"),
            EmailAddress = MapUriCommunication(enrichedOrganisation.Email),
            Telephone = MapCommunication(enrichedOrganisation.Telephone)
        };
    }

    private static GcModels.UniversalCommunication MapUriCommunication(string source)
    {
        return new GcModels.UniversalCommunication
        {
            Uri = MapIdType(source)
        };
    }

    private static GcModels.TradeParty OperatorToTradePartyMapper(EhcoModels.Operator operatorOperator)
    {
        return new GcModels.TradeParty
        {
            Name = [MapTextType(operatorOperator.Name, "en")],
            RoleCode = [MapModeCode("AG")],
            EmailAddress = MapUriCommunication(operatorOperator.Email),
            Telephone = MapCommunication(operatorOperator.Telephone),
            AuthoritativeSignatoryPerson = null,
            DefinedContactDetails = null,
            PostalAddress = MapAddress(operatorOperator),
            Id = [operatorOperator.Traces?.OperatorId != null ? MapIdType(operatorOperator.Traces.OperatorId) : new GcModels.IDType()]
        };
    }

    private GcModels.ExchangedDocument MapExchangedDocument(Models.Ehco.ExchangedDocument exchangedDocument, IReadOnlyCollection<GeneralCertificateDocument> gcDocuments, IdcomsModels.IdcomsGeneralCertificateEnrichment enrichment)
    {
        var mapped = new GcModels.ExchangedDocument
        {
            Id = new GcModels.IDType
            {
                Content = exchangedDocument.ID,
                SchemeAgencyId = string.Empty,
                SchemeId = "GC"
            },
            IssueDateTime = new GcModels.DateTimeType
            {
                Content = GetDesiredDateFormat(exchangedDocument.CertificateIssueDateTime),
                Format = "205"
            },
            Issuer = new GcModels.TradeParty
            {
                Name =
                [
                    MapTextType(GcConstants.GcIssuerName, "en")
                ]
            },
            PrimarySignatoryAuthentication = MapDocumentAuthentication(exchangedDocument, enrichment),
            ReferenceDocument = MapReferenceDocument(exchangedDocument.ID, gcDocuments),
            TraderAssignedId = new GcModels.IDType { Content = exchangedDocument.ApplicantAssignedID },
            TypeCode = MapModeCode(exchangedDocument.TypeCode, GcConstants.ExchangeDocListingAgencyId)
        };
        return mapped;
    }

    private List<GcModels.ReferencedDocument> MapReferenceDocument(string gcId, IReadOnlyCollection<GeneralCertificateDocument> documents)
    {
        return (from document in documents
                let docId = document.FileName.Contains('.')
                                                           ? document.FileName.Split('.')[0]
                                                           : document.FileName
                select new ReferencedDocument
                {
                    Id = [MapIdType(docId)],
                    TypeCode = MapModeCode(document.TypeCode.ToString()),
                    AttachedBinaryObject = [new()
                                           {
                                               Uri = $"{_apiSettings.Value.DaeraCertificatesApiUrlV1}/general-certificate/document?gcId={gcId}&documentId={document.DocumentId}",
                                               Filename = document.FileName
                                           }]
                }).ToList();
    }
}