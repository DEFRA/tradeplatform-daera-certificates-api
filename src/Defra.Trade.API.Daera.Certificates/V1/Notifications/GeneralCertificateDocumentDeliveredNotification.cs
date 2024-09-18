// Copyright DEFRA (c). All rights reserved.
// Licensed under the Open Government License v3.0.

using MediatR;

namespace Defra.Trade.API.Daera.Certificates.V1.Notifications;

public sealed record GeneralCertificateDocumentDeliveredNotification(string GeneralCertificateId, string DocumentId) : INotification;