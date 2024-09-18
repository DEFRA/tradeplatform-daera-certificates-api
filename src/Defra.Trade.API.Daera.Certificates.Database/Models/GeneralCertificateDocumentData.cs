// Copyright DEFRA (c). All rights reserved.
// Licensed under the Open Government License v3.0.

namespace Defra.Trade.API.Daera.Certificates.Database.Models;

public class GeneralCertificateDocument : BaseTable
{
    /// <summary>
    /// GC attachment id.
    /// </summary>
    public Guid Id { get; set; }

    public virtual GeneralCertificate GeneralCertificate { get; set; }

    /// <summary>
    /// The location of the packing list document
    /// </summary>
    public string Url { get; set; }

    /// <summary>
    /// blob name..
    /// </summary>
    public string FileName { get; set; }

    /// <summary>
    /// ListAgencyId.
    /// </summary>
    public int ListAgencyId { get; set; }

    /// <summary>
    /// Type Code.
    /// </summary>
    public int TypeCode { get; set; }

    /// <summary>
    /// Blob document id.
    /// </summary>
    public string DocumentId { get; set; }

    /// <summary>
    /// Retrieved datetime
    /// </summary>
    public DateTime? Retrieved { get; set; }
}