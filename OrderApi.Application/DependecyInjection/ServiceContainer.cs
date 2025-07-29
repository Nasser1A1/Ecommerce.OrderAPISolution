using eCommerce.SharedLib.Logs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrderApi.Application.Interfaces;
using OrderApi.Application.Services;
using Polly;
using Polly.Retry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderApi.Application.DependecyInjection
{
    public static class ServiceContainer
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services,IConfiguration config)
        {
    

            services.AddHttpClient<IOrderService,OrderService>(options =>
            {
                options.BaseAddress = new Uri(config["ApiGateway:BaseAddress"]!);
                options.DefaultRequestHeaders.Add("Accept", "application/json");

                options.Timeout = TimeSpan.FromSeconds(10);
            }).AddHttpMessageHandler<AuthorizedHandler>(); ;

            // create retry startegy for the http client
            var retryStrategy = new RetryStrategyOptions()
            {
                ShouldHandle = new PredicateBuilder().Handle<TaskCanceledException>(),
                BackoffType = DelayBackoffType.Constant,
                UseJitter = true,
                MaxRetryAttempts = 3,
                Delay = TimeSpan.FromMilliseconds(500),
                OnRetry = args =>
                {
                    // Log the retry attempt
                    string message = $"OnRetry, Attempt:{args.AttemptNumber} Outcome{args.Outcome}";
                    LogExceptions.LogToDebugger(message);
                    LogExceptions.LogToConsole(message);
                    return ValueTask.CompletedTask;
                }
            };

            services.AddResiliencePipeline("my-retry-pipeline", builder =>{
                builder.AddRetry(retryStrategy);
            });


            return services;
        }
    }
}
