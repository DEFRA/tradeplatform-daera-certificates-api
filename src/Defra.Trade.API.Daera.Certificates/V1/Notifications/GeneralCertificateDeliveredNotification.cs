// Copyright DEFRA (c). All rights reserved.
// Licensed under the Open Government License v3.0.

using MediatR;

namespace Defra.Trade.API.Daera.Certificates.V1.Notifications;

public sealed record GeneralCertificateDeliveredNotification(string GeneralCertificateId) : INotification;