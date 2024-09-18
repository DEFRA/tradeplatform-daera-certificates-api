// Copyright DEFRA (c). All rights reserved.
// Licensed under the Open Government License v3.0.

using System.ComponentModel.DataAnnotations;

namespace Defra.Trade.API.Daera.Certificates.Ehco.BlobClient;

/// <summary>
/// Blob storage container types used in Defra.
/// </summary>
public enum StorageContainerType
{
    /// <summary>
    /// Source container to store source documents.
    /// </summary>
    [Display(Name = "Source")]
    Source
}