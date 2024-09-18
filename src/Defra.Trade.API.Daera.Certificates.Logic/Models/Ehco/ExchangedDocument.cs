// Copyright DEFRA (c). All rights reserved.
// Licensed under the Open Government License v3.0.

namespace Defra.Trade.API.Daera.Certificates.Logic.Models.Ehco;

/// <summary>
/// The header document information for a use of this master message assembly.
/// </summary>
public class ExchangedDocument
{
    /// <summary>
    /// The unique identifier of this exchanged document.
    /// </summary>
    public string ID { get; set; }

    /// <summary>
    /// The code specifying the type of exchanged document.
    /// </summary>
    public string TypeCode { get; set; }

    /// <summary>
    /// A user entered ID to reference the General Certificate
    /// </summary>
    public string ApplicantAssignedID { get; set; }

    /// <summary>
    /// The EHCO application ID
    /// </summary>
    public string ApplicationSubmissionID { get; set; }

    /// <summary>
    /// Details of the applicant
    /// </summary>
    public Applicant Applicant { get; set; }

    /// <summary>
    /// The location of the packing list document
    /// </summary>
    public string PackingListFileLocation { get; set; }

    /// <summary>
    /// The location of the certificate PDF document
    /// </summary>
    public string CertificatePDFLocation { get; set; }

    /// <summary>
    /// The date, time, date time or other date time value for the application submission.
    /// </summary>
    public DateTimeOffset ApplicationSubmissionDateTime { get; set; }

    /// <summary>
    /// The date, time, date time or other date time value for the issuance of the certificate.
    /// </summary>
    public DateTimeOffset CertificateIssueDateTime { get; set; }
}