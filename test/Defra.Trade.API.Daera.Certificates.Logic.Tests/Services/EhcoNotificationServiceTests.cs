// Copyright DEFRA (c). All rights reserved.
// Licensed under the Open Government License v3.0.

using System.Threading;
using AutoMapper;
using Azure.Messaging.ServiceBus;
using Defra.Trade.API.Daera.Certificates.Database.Models;
using Defra.Trade.API.Daera.Certificates.Logic.Models.Ehco;
using Defra.Trade.API.Daera.Certificates.Logic.Services;
using Defra.Trade.API.Daera.Certificates.Logic.Services.Interfaces;
using Defra.Trade.API.Daera.Certificates.Repository.Interfaces;
using Defra.Trade.API.Daera.Certificates.Tests.Common;
using FluentAssertions.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;

namespace Defra.Trade.API.Daera.Certificates.Services.Tests.Services;

public class EhcoNotificationServiceTests
{
    private readonly Mock<ILogger<EhcoNotificationService>> _logger;
    private readonly Mock<IMapper> _mapper;
    private readonly EhcoNotificationOptions _options;
    private readonly Mock<IServiceBusClientFactory> _serviceBusFactory;
    private readonly Mock<ICertificatesStoreRepository> _repository;
    private readonly EhcoNotificationService _sut;

    public EhcoNotificationServiceTests()
    {
        _options = new();
        _serviceBusFactory = new(MockBehavior.Strict);
        _repository = new(MockBehavior.Strict);
        _mapper = new(MockBehavior.Strict);
        _logger = new();
        _sut = new(_serviceBusFactory.Object, _repository.Object, _mapper.Object, _logger.Object, Options.Create(_options));

        _logger.Setup(m => m.IsEnabled(It.IsAny<LogLevel>())).Returns(true);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("notANumber")]
    [InlineData("")]
    public async Task NotifyGeneralCertificateDelivered_FailsWhenTheApplicationSubmissionIdIs(string applicationSubmissionId)
    {
        // arrange
        string id = Guid.NewGuid().ToString();
        using var cts = new CancellationTokenSource();

        var dbModel = new GeneralCertificate();
        var application = new EhcoGeneralCertificateApplication
        {
            ExchangedDocument = new()
            {
                ApplicationSubmissionID = applicationSubmissionId
            }
        };

        _repository.Setup(m => m.GetGeneralCertificateSummaryAsync(id, cts.Token)).ReturnsAsync(dbModel).Verifiable();
        _mapper.Setup(m => m.Map<EhcoGeneralCertificateApplication>(dbModel)).Returns(application).Verifiable();

        // act
        await _sut.NotifyGeneralCertificateDelivered(id, cts.Token);

        // assert
        Mock.Verify(_serviceBusFactory, _repository, _mapper, _logger);
        VerifyLoggerCannotReadSubmissionId(id);
    }

    [Fact]
    public async Task NotifyGeneralCertificateDelivered_FailsWhenTheExchangeDocumentIsNull()
    {
        // arrange

        string id = Guid.NewGuid().ToString();
        using var cts = new CancellationTokenSource();

        var dbModel = new GeneralCertificate();
        var application = new EhcoGeneralCertificateApplication();

        _repository.Setup(m => m.GetGeneralCertificateSummaryAsync(id, cts.Token)).ReturnsAsync(dbModel).Verifiable();
        _mapper.Setup(m => m.Map<EhcoGeneralCertificateApplication>(dbModel)).Returns(application).Verifiable();

        // act
        await _sut.NotifyGeneralCertificateDelivered(id, cts.Token);

        // assert
        Mock.Verify(_serviceBusFactory, _repository, _mapper, _logger);
        VerifyLoggerCannotReadSubmissionId(id);
    }

    [Fact]
    public async Task NotifyGeneralCertificateDelivered_FailsWhenTheGeneralCertificateIsNotFound()
    {
        // arrange

        string id = Guid.NewGuid().ToString();
        using var cts = new CancellationTokenSource();

        var dbModel = null as GeneralCertificate;
        var application = null as EhcoGeneralCertificateApplication;

        _repository.Setup(m => m.GetGeneralCertificateSummaryAsync(id, cts.Token)).ReturnsAsync(dbModel).Verifiable();
        _mapper.Setup(m => m.Map<EhcoGeneralCertificateApplication>(dbModel)).Returns(application).Verifiable();

        // act
        await _sut.NotifyGeneralCertificateDelivered(id, cts.Token);

        // assert
        Mock.Verify(_serviceBusFactory, _repository, _mapper, _logger);
        VerifyLoggerCannotReadSubmissionId(id);
    }

    [Fact]
    public async Task NotifyGeneralCertificateDelivered_SendsTheNotificationToDaera()
    {
        // arrange
        long submissionId = Random.Shared.NextInt64(0, int.MaxValue);
        string id = Guid.NewGuid().ToString();
        string connectionString = "Endpoint=sb://not-a-real.servicebus.windows.net/;SharedAccessKeyName=MyKey;SharedAccessKey=abc=";
        string queueName = "my-queue";
        using var cts = new CancellationTokenSource();
        var serviceBus = new Mock<ServiceBusClient>(MockBehavior.Strict);
        var queue = new Mock<ServiceBusSender>(MockBehavior.Strict);
        var message = null as ServiceBusMessage;

        var dbModel = new GeneralCertificate
        {
            GeneralCertificateDocument =
            [
                new() { Retrieved = DateTime.UtcNow}
            ]
        };

        var application = new EhcoGeneralCertificateApplication
        {
            ExchangedDocument = new()
            {
                ApplicationSubmissionID = submissionId.ToString()
            }
        };

        _options.ConnectionString = connectionString;
        _options.QueueName = queueName;
        _repository.Setup(m => m.GetGeneralCertificateSummaryAsync(id, cts.Token)).ReturnsAsync(dbModel).Verifiable();
        _mapper.Setup(m => m.Map<EhcoGeneralCertificateApplication>(dbModel)).Returns(application).Verifiable();
        _serviceBusFactory.Setup(m => m.Create(connectionString, null)).Returns(serviceBus.Object).Verifiable();
        serviceBus.Setup(m => m.CreateSender(queueName)).Returns(queue.Object).Verifiable();
        queue.Setup(m => m.DisposeAsync()).Returns(ValueTask.CompletedTask).Verifiable();
        queue.Setup(m => m.SendMessageAsync(It.Is<ServiceBusMessage>(m =>
            m.ContentType == "application/json"
            && m.SessionId == submissionId.ToString()
            && m.Subject == "trade.remos.status.notification"
            && m.ApplicationProperties.Contains(new("EntityKey", submissionId))
            && m.ApplicationProperties.Contains(new("PublisherId", "TradeAPI"))
            && m.ApplicationProperties.Contains(new("SchemaVersion", "1"))
            && m.ApplicationProperties.ContainsKey("TimestampUtc")
            && m.ApplicationProperties["TimestampUtc"] is long
            && (DateTimeOffset.UtcNow.ToUnixTimeSeconds() - ((long)m.ApplicationProperties["TimestampUtc"])) < 2
            && (DateTimeOffset.UtcNow.ToUnixTimeSeconds() - ((long)m.ApplicationProperties["TimestampUtc"])) >= 0
            && m.ApplicationProperties.Count == 4
            && m.Body != null), cts.Token))
            .Callback((ServiceBusMessage m, CancellationToken _) => message = m)
            .Returns(Task.CompletedTask)
            .Verifiable();

        // act
        await _sut.NotifyGeneralCertificateDelivered(id, cts.Token);

        // assert
        Mock.Verify(_serviceBusFactory, _repository, _mapper, _logger, serviceBus, queue);
        message.Should().NotBeNull();
        var messageJson = JToken.Parse(message.Body.ToString());
        var messageJObject = messageJson.Should().BeOfType<JObject>().Subject;
        long messageId = messageJObject["messageId"].Should().BeOfType<JValue>()
            .Which.Value.Should().BeOfType<long>().Subject;
        var modifiedon = messageJObject["applicationUpdate"]?["modifiedon"].Should().BeOfType<JValue>()
            .Which.Value.Should().BeOfType<DateTime>().Subject;

        modifiedon.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(10));

        messageJObject.Should().BeEquivalentTo(JToken.Parse($$"""
            {
                "messageId": {{messageId}},
                "messageType": "GC_TP_APPLICATION_UPDATE",
                "applicationUpdate": {
                    "applicationId": {{submissionId}},
                    "status": "DeliveredToDAERA",
                    "modifiedon": "{{modifiedon:o}}"
                }
            }
            """));

        _logger.VerifyLogged($"Sending message {messageId} to EHCO to notify the successful delivery of General Certificate {id} to DAERA.", LogLevel.Information);
        _logger.VerifyLogged($"Sent message {messageId} to EHCO to notify the successful delivery of General Certificate {id} to DAERA.", LogLevel.Information);
    }

    [Fact]
    public async Task NotifyGeneralCertificateDelivered_SendsTheNotificationToDaera_UsingIDAndSecret()
    {
        // arrange
        string connectionString = "Endpoint=sb://not-a-real.servicebus.windows.net/;SharedAccessKeyName=MyKey;SharedAccessKey=abc=";
        long submissionId = Random.Shared.NextInt64(0, int.MaxValue);
        string id = Guid.NewGuid().ToString();
        string queueName = "my-queue";
        using var cts = new CancellationTokenSource();
        var serviceBus = new Mock<ServiceBusClient>(MockBehavior.Strict);
        var queue = new Mock<ServiceBusSender>(MockBehavior.Strict);
        var message = null as ServiceBusMessage;

        var dbModel = new GeneralCertificate
        {
            GeneralCertificateDocument =
            [
                new() { Retrieved = DateTime.UtcNow}
            ]
        };
        var application = new EhcoGeneralCertificateApplication
        {
            ExchangedDocument = new()
            {
                ApplicationSubmissionID = submissionId.ToString()
            }
        };

        _options.ConnectionString = connectionString;
        _options.QueueName = queueName;
        _repository.Setup(m => m.GetGeneralCertificateSummaryAsync(id, cts.Token)).ReturnsAsync(dbModel).Verifiable();
        _mapper.Setup(m => m.Map<EhcoGeneralCertificateApplication>(dbModel)).Returns(application).Verifiable();

        _serviceBusFactory.Setup(m => m.Create(_options.ConnectionString, null)).Returns(serviceBus.Object).Verifiable();

        serviceBus.Setup(m => m.CreateSender(queueName)).Returns(queue.Object).Verifiable();
        queue.Setup(m => m.DisposeAsync()).Returns(ValueTask.CompletedTask).Verifiable();
        queue.Setup(m => m.SendMessageAsync(It.Is<ServiceBusMessage>(m =>
            m.ContentType == "application/json"
            && m.SessionId == submissionId.ToString()
            && m.Subject == "trade.remos.status.notification"
            && m.ApplicationProperties.Contains(new("EntityKey", submissionId))
            && m.ApplicationProperties.Contains(new("PublisherId", "TradeAPI"))
            && m.ApplicationProperties.Contains(new("SchemaVersion", "1"))
            && m.ApplicationProperties.ContainsKey("TimestampUtc")
            && m.ApplicationProperties["TimestampUtc"] is long
            && (DateTimeOffset.UtcNow.ToUnixTimeSeconds() - ((long)m.ApplicationProperties["TimestampUtc"])) < 2
            && (DateTimeOffset.UtcNow.ToUnixTimeSeconds() - ((long)m.ApplicationProperties["TimestampUtc"])) >= 0
            && m.ApplicationProperties.Count == 4
            && m.Body != null), cts.Token))
            .Callback((ServiceBusMessage m, CancellationToken _) => message = m)
            .Returns(Task.CompletedTask)
            .Verifiable();

        // act
        await _sut.NotifyGeneralCertificateDelivered(id, cts.Token);

        // assert
        Mock.Verify(_serviceBusFactory, _repository, _mapper, _logger, serviceBus, queue);
        message.Should().NotBeNull();
        var messageJson = JToken.Parse(message.Body.ToString());
        var messageJObject = messageJson.Should().BeOfType<JObject>().Subject;
        long messageId = messageJObject["messageId"].Should().BeOfType<JValue>()
            .Which.Value.Should().BeOfType<long>().Subject;

        _logger.VerifyLogged($"Sending message {messageId} to EHCO to notify the successful delivery of General Certificate {id} to DAERA.", LogLevel.Information);
        _logger.VerifyLogged($"Sent message {messageId} to EHCO to notify the successful delivery of General Certificate {id} to DAERA.", LogLevel.Information);
    }

    private void VerifyLoggerCannotReadSubmissionId(string generalCertificateId)
    {
        _logger.VerifyLogged($"Failed to parse the submission id from general certificate {generalCertificateId}.", LogLevel.Error);
    }
}
