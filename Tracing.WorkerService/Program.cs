using Datadog.Trace;
using Datadog.Trace.Configuration;
using MassTransit;
using System.Reflection;
using Tracing.Shared.Messages;
using Tracing.Shared.TraceAndLong;
using Tracing.WorkerService;

Microsoft.Extensions.Hosting.IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        //services.AddHostedService<Worker>();

        var appName = $"{Assembly.GetExecutingAssembly().GetName().Name}-local";
        var settings = TracerSettings.FromDefaultSources();
        settings.Environment = "arsen-local";
        settings.ServiceName = appName;
        settings.ServiceVersion = "1.0.0";
        settings.TraceEnabled = true;
        settings.TracerMetricsEnabled = true;
        // In v2 of Datadog.Trace, use settings.Exporter.AgentUri
        settings.AgentUri = new Uri("http://localhost:8126/");
        Tracer.Configure(settings);

        var tracingAndLogSection = context.Configuration.GetSection("TracingAndLogging");

        services.ConfigureTracing(tracingAndLogSection);

        var rabbitMqConfig = new RabbitMqConfiguration();
        context.Configuration.GetSection("RabbitMq").Bind(rabbitMqConfig);

        services.AddMassTransit(x =>
        {
            x.AddConsumer<SubmitOrderConsumer>();

            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host("localhost", "TracingExample", c =>
                {
                    c.Username("guest");
                    c.Password("guest");
                });

                cfg.ReceiveEndpoint(OrderMessage.EndPointAddress,  xr =>
                {
                    xr.ConfigureConsumer<SubmitOrderConsumer>(context);
                });
            });
        });
    })
    .ConfigureLogging((context, x) =>
    {
        var tracingAndLogSection = context.Configuration.GetSection("TracingAndLogging");

        x.ClearProviders().ConfigureLogging(tracingAndLogSection);
    })
    .Build();

await host.RunAsync();
