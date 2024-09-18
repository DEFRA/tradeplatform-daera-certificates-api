// Copyright DEFRA (c). All rights reserved.
// Licensed under the Open Government License v3.0.

namespace Defra.Trade.API.Daera.Certificates.Ehco.BlobClient.Infrastructure;

public class GcDocumentBlobStorageOptions : AzureBlobStorageOptionsBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GcDocumentBlobStorageOptions"/> class.
    /// </summary>
    public GcDocumentBlobStorageOptions()
    {
        Containers.Add(nameof(StorageContainerType.Source), "application-forms");
    }

    public override string SectionName => "DaeraCertificatesExtApi:GcDocumentBlobStorage";

    public static string OptionsName => "DaeraCertificatesExtApi:GcDocumentBlobStorage";
}