using System.Net;
using commercetools.Sdk.Api;
using commercetools.Sdk.ImportApi;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http.Resilience;
using Polly;

namespace Accelerator.Commercetools.Importer.Commercetools;

public static class Extensions
{
    private static Action<ResiliencePipelineBuilder<HttpResponseMessage>> CommercetoolsApiConfiguration() =>
        static builder =>
        {
            builder.AddRetry(new HttpRetryStrategyOptions
            {
                BackoffType = DelayBackoffType.Exponential,
                MaxRetryAttempts = 5,
                UseJitter = true,
                ShouldHandle = static args => ValueTask.FromResult(args is
                {
                    Outcome.Result.StatusCode:
                    HttpStatusCode.BadGateway or
                    HttpStatusCode.ServiceUnavailable or
                    HttpStatusCode.RequestTimeout or
                    HttpStatusCode.GatewayTimeout 
                })
            });

            builder.AddCircuitBreaker(new HttpCircuitBreakerStrategyOptions
            {
                SamplingDuration = TimeSpan.FromSeconds(10),
                FailureRatio = 0.2,
                MinimumThroughput = 3,
                ShouldHandle = static args => ValueTask.FromResult(args is
                {
                    Outcome.Result.StatusCode:
                    HttpStatusCode.RequestTimeout or
                    HttpStatusCode.TooManyRequests
                })
            });

            builder.AddTimeout(TimeSpan.FromSeconds(5));
        };
    
    public static IServiceCollection AddCommercetools(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .UseCommercetoolsImportApi(configuration, "AcceleratorConfiguration:CommercetoolsImportConfig")
            .AddResilienceHandler("CommercetoolsApiResiliencePipeline", CommercetoolsApiConfiguration());
        //
        // services
        //     .UseCommercetoolsApi(configuration, "AcceleratorConfiguration:CommercetoolsConfig")
        //     .AddResilienceHandler("CommercetoolsApiResiliencePipeline", CommercetoolsApiConfiguration());
        //
        return services;
    }
}