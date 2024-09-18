// Copyright DEFRA (c). All rights reserved.
// Licensed under the Open Government License v3.0.

using Defra.Trade.API.Daera.Certificates.V1.Dtos.GeneralCertificate;
using Swashbuckle.AspNetCore.Filters;

namespace Defra.Trade.API.Daera.Certificates.V1.Examples;

public class GeneralCertificateExample : IExamplesProvider<GeneralCertificate>
{
    public GeneralCertificate GetExamples()
    {
        return new GeneralCertificate
        {
            ExchangedDocument = new ExchangedDocument
            {
                Id = new IDType
                {
                    Content = "CHEDP.XI.2023.0061020",
                    SchemeId = "GC"
                },
                TypeCode = new CodeType
                {
                    Content = "1001"
                },
                IssueDateTime = new DateTimeType
                {
                    Content = "202306190000+00",
                    Format = "205"
                },
                TraderAssignedId = new IDType
                {
                    Content = "RETAIL"
                },
                ReferenceDocument =
                [
                    new()
                    {
                        TypeCode = new CodeType
                        {
                            Content = "271",
                            ListAgencyID = "6"
                        },
                        Id =
                        [
                            new()
                            {
                                Content = "Organisation B Packing List 01234"
                            }
                        ],
                        AttachedBinaryObject =
                        [
                            new()
                            {
                                Uri = "https://www.defra.org.uk/remos/packinglists/",
                                Filename = "packing-list.pdf"
                            }
                        ]
                    }
                ],
                PrimarySignatoryAuthentication = new DocumentAuthentication()
                {
                    ActualDateTime = new DateTimeType()
                    {
                        Content = "202306190000+00",
                        Format = "205"
                    },
                    IncludedDocumentClause =
                    [
                        new()
                        {
                            Id = new IDType
                            {
                                Content = "GC Declaration Clause 873",
                                SchemeId = "DC"
                            },
                            Content = new TextType
                            {
                                Content = "I, the undersigned operator responsible for the consignment detailed above, certify that to the best of my knowledge and belief the statements made " +
                                          "in Part I of this document are true and complete, and I agree to comply with the requirements of Regulation (EU) 2017/625 on official controls, " +
                                          "including payment for official controls, as well as for re-dispatching consignments, quarantine or isolation of animals, or costs of euthanasia and " +
                                          "disposal where necessary.",
                                LanguageId = "en"
                            }
                        }
                    ],
                    ProvidingTradeParty = new TradeParty
                    {
                        Id = new[]
                        {
                            new IDType
                            {
                                Content = "RMS-NI-000001",
                                SchemeId = "RMS"
                            }
                        },
                        Name =
                        [
                            new()
                            {
                                Content = "Organisation A Ltd",
                                LanguageId = "en"
                            }
                        ],
                        RoleCode =
                        [
                            new()
                            {
                                Content = "DGP"
                            }
                        ]
                    }
                },
                Issuer = new TradeParty
                {
                    Id = new[]
                    {
                        new IDType
                        {
                            Content = "100001000100010006",
                            SchemeId = "Defra.Customer"
                        },
                        new IDType
                        {
                            Content = "RMS-GB-000001",
                            SchemeId = "RMS"
                        }
                    },
                    Name =
                    [
                        new()
                        {
                            Content = "Organisation B Ltd",
                            LanguageId = "en"
                        }
                    ],
                    RoleCode =
                    [
                        new()
                        {
                            Content = "DGP"
                        }
                    ],
                    DefinedContactDetails =
                    [
                        new()
                        {
                            Id = new IDType
                            {
                                Content = "100001000100010004",
                                SchemeId = "Defra.Customer"
                            },
                            PersonName = new TextType
                            {
                                Content = "Defra.Customer"
                            }
                        }
                    ]
                }
            },
            SupplyChainConsignment = new SupplyChainConsignment
            {
                ExportExitDateTime = new DateTimeType
                {
                    Content = "202306190000+00",
                    Format = "205"
                },
                CustomsID = new[]
                {
                    new IDType
                    {
                            Content = "GMRCQP0AAAAA",
                            SchemeId = "GMR"
                     }
                },
                Consignor = new TradeParty
                {
                    Id = new[]
                    {
                        new IDType
                        {
                            Content = "763782378123761236",
                            SchemeId = "Defra.Customer"
                        },
                        new IDType
                        {
                            Content = "RMS-GB-000002",
                            SchemeId = "RMS"
                        }
                    },
                    Name =
                    [
                        new()
                        {
                            Content = "Organisation B Ltd",
                            LanguageId = "en"
                        }
                    ],
                    RoleCode =
                    [
                        new()
                        {
                            Content = "DGP"
                        }
                    ],
                    PostalAddress = new TradeAddress
                    {
                        Postcode = new CodeType
                        {
                            Content = "AA1 1AA"
                        },
                        LineOne = new TextType
                        {
                            Content = "A road"
                        },
                        CityName = new TextType
                        {
                            Content = "Liverpool"
                        },
                        CountryCode = new IDType
                        {
                            Content = "GB",
                            SchemeAgencyId = "5"
                        },
                        TypeCode = new CodeType
                        {
                            Content = "1",
                            ListAgencyID = "6"
                        }
                    }
                },
                Consignee = new TradeParty
                {
                    Id = new[]
                    {
                        new IDType
                        {
                            Content = "RMS-GB-000003",
                            SchemeId = "RMS"
                        }
                    },
                    Name =
                    [
                        new()
                        {
                            Content = "Organisation C Ltd",
                            LanguageId = "en"
                        }
                    ],
                    RoleCode =
                    [
                        new()
                        {
                            Content = "DGP"
                        }
                    ],
                    PostalAddress = new TradeAddress
                    {
                        Postcode = new CodeType
                        {
                            Content = "B11 1HP"
                        },
                        LineOne = new TextType
                        {
                            Content = "1 ROAD"
                        },
                        CityName = new TextType
                        {
                            Content = "Belfast"
                        },
                        CountryCode = new IDType
                        {
                            Content = "XI",
                            SchemeAgencyId = "5"
                        },
                        TypeCode = new CodeType
                        {
                            Content = "1",
                            ListAgencyID = "6"
                        }
                    }
                },
                OperatorResponsibleForConsignment = new TradeParty
                {
                    Id = new[]
                    {
                        new IDType
                        {
                            Content = "RFL-ID",
                            SchemeId = "RMS"
                        }
                    },
                    Name =
                    [
                        new()
                        {
                            Content = "AG",
                            LanguageId = "en"
                        }
                    ],
                    RoleCode =
                    [
                        new()
                        {
                            Content = "AG"
                        }
                    ],
                    PostalAddress = new TradeAddress
                    {
                        Postcode = new CodeType
                        {
                            Content = "BT11 1LT"
                        },
                        LineOne = new TextType
                        {
                            Content = "1 Town Road"
                        },
                        CityName = new TextType
                        {
                            Content = "LURGAN"
                        },
                        CountryCode = new IDType
                        {
                            Content = "XI",
                            SchemeAgencyId = "5"
                        },
                        TypeCode = new CodeType
                        {
                            Content = "1",
                            ListAgencyID = "6"
                        },
                        CountrySubDivisionCode = new IDType
                        {
                            Content = "",
                            SchemeAgencyId = ""
                        },
                        CountrySubDivisionName = new TextType
                        {
                            Content = ""
                        }
                    },
                    Telephone = new UniversalCommunication
                    {
                        CompleteNumber = new TextType
                        {
                            Content = "0123456789",
                            LanguageId = ""
                        }
                    },
                    EmailAddress = new UniversalCommunication
                    {
                        Uri = new IDType
                        {
                            Content = "test@defra.gov.uk"
                        }
                    },
                    AuthoritativeSignatoryPerson = new AuthoritativeSignatoryPerson
                    {
                        Name = new TextType
                        {
                            Content = "Sample Name",
                            LanguageId = ""
                        }
                    }
                },
                DispatchLocation = new LogisticsLocation
                {
                    Id = new[]
                    {
                        new IDType
                        {
                            Content = "",
                            SchemeId = "Defra.Customer",
                        }
                    },
                    Name = new TextType
                    {
                        Content = "",
                        LanguageId = "en"
                    },
                    LocationAddress = new TradeAddress
                    {
                        TypeCode = new CodeType
                        {
                            Content = "3",
                            ListAgencyID = "6"
                        },
                        Postcode = new CodeType
                        {
                            Content = ""
                        },
                        LineOne = new TextType
                        {
                            Content = ""
                        },
                        LineTwo = new TextType
                        {
                            Content = ""
                        },
                        LineThree = new TextType
                        {
                            Content = ""
                        },
                        LineFour = new TextType
                        {
                            Content = ""
                        },
                        LineFive = new TextType
                        {
                            Content = ""
                        },
                        CityName = new TextType
                        {
                            Content = ""
                        },
                        CountryCode = new IDType
                        {
                            Content = "GB", // GB = UNITED KINGDOM OF GREAT BRITAIN AND NORTHERN IRELAND
                            SchemeAgencyId = "5" // 5 = ISO (International Organization for Standardization)
                        },
                        CountryName = new TextType
                        {
                            Content = "UNITED KINGDOM OF GREAT BRITAIN AND NORTHERN IRELAND",
                            LanguageId = "en"
                        },
                        CountrySubDivisionCode = new IDType
                        {
                            Content = "GB-GBN",
                            SchemeAgencyId = "5" // 5 = ISO (International Organization for Standardization)
                        },
                        CountrySubDivisionName = new TextType
                        {
                            Content = "Great Britain"
                        }
                    }
                },
                DestinationLocation = new LogisticsLocation
                {
                    Id = new[]
                    {
                        new IDType
                        {
                            Content = "RMS-NI-000001-001",
                            SchemeId = "RMS"
                        }
                    },
                    Name = new TextType
                    {
                        Content = "Organisation D Ltd",
                        LanguageId = "en"
                    },
                    LocationAddress = new TradeAddress
                    {
                        Postcode = new CodeType
                        {
                            Content = "B12 6HP"
                        },
                        LineOne = new TextType
                        {
                            Content = "1 Long ROAD"
                        },
                        CityName = new TextType
                        {
                            Content = "Belfast"
                        },
                        CountryCode = new IDType
                        {
                            Content = "XI",
                            SchemeAgencyId = "5"
                        },
                        TypeCode = new CodeType
                        {
                            Content = "3",
                            ListAgencyID = "6"
                        },
                        CountrySubDivisionCode = new IDType
                        {
                            Content = "",
                            SchemeAgencyId = ""
                        },
                        CountrySubDivisionName = new TextType
                        {
                            Content = ""
                        }
                    }
                },
                BorderControlPostLocation = new LogisticsLocation
                {
                    Id = new[]
                    {
                        new IDType
                        {
                            Content = "GBBEL1",
                            SchemeId = "BCP"
                        }
                    },
                    Name = new TextType
                    {
                        Content = "Belfast Port",
                        LanguageId = "en"
                    }
                },
                UsedTransportEquipment =
                [
                    new()
                    {
                        Id = new IDType
                        {
                            Content = "GCSBBB01"
                        },
                        AffixedSeal =
                        [
                            new()
                            {
                                Id = new IDType
                                {
                                    Content = "CC-001002"
                                }
                            }
                        ],
                        TemperatureSetting =
                        [
                            new()
                            {
                                Value = new TemperatureUnitMeasure
                                {
                                    Content = 2
                                },
                                TypeCode = new CodeType
                                {
                                    Content = "2",
                                    ListAgencyID = "6"
                                }
                            }
                        ]
                    }
                ],
                OriginCountry =
                [
                    new ()
                    {
                        Code = new IDType
                        {
                                Content = "GB",
                                SchemeAgencyId = "5" // 5 = ISO (International Organization for Standardization)
                        }
                    }
                ],
                MainCarriageTransportMovement =
                [
                    new()
                    {
                        ModeCode = new CodeType
                        {
                            Content = "3",
                            ListAgencyID = "6"
                        },
                        UsedTransportMeans = new LogisticsTransportMeans
                        {
                            Id = new IDType
                            {
                                Content = "GCSAAA01"
                            },
                            Name = new TextType
                            {
                                Content="",
                                LanguageId = "6"
                            }
                        }
                    }
                ],
                ImportCountry = new TradeCountry
                {
                    Code = new IDType
                    {
                        Content = "XI",
                        SchemeAgencyId = "5" // 5 = ISO (International Organization for Standardization)
                    }
                },
                ExportCountry = new TradeCountry
                {
                    Code = new IDType
                    {
                        Content = "GB",
                        SchemeAgencyId = "5" // 5 = ISO (International Organization for Standardization)
                    }
                }
            }
        };
    }
}