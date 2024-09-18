// Copyright DEFRA (c). All rights reserved.
// Licensed under the Open Government License v3.0.

using Azure.Core;
using Azure.Messaging.ServiceBus;
using Defra.Trade.API.Daera.Certificates.Logic.Services.Interfaces;

namespace Defra.Trade.API.Daera.Certificates.Logic.Services;

public class ServiceBusClientFactory : IServiceBusClientFactory
{
    public ServiceBusClient Create(string connectionString, ServiceBusClientOptions options = null)
    {
        return new(connectionString, options);
    }

    public ServiceBusClient Create(string fullyQualifiedNamespace, TokenCredential credential, ServiceBusClientOptions options = null)
    {
        return new(fullyQualifiedNamespace, credential, options);
    }
}