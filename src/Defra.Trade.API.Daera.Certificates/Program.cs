// Copyright DEFRA (c). All rights reserved.
// Licensed under the Open Government License v3.0.

using System.Diagnostics.CodeAnalysis;
using Defra.Trade.API.Daera.Certificates.Infrastructure;
using Defra.Trade.Common.AppConfig;
using Microsoft.Extensions.Hosting;

namespace Defra.Trade.API.Daera.Certificates;

/// <summary>
/// Application program file.
/// </summary>
public static class Program
{
    /// <summary>
    /// Application main class.
    /// </summary>
    /// <param name="args">Args</param>
    [ExcludeFromCodeCoverage(Justification = "Process entry point covered by end-to-end tests.")]
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    private static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration(config =>
                {
                    config.ConfigureTradeAppConfiguration(cfg =>
                    {
                        cfg.UseKeyVaultSecrets = true;
                        cfg.RefreshKeys.Add($"{ExtApiAppConfig.AppConfigSettingsName}:{ExtApiAppConfig.RefreshKey}");
                    });
                })
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });
}