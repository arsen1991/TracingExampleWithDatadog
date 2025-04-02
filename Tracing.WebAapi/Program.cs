using Datadog.Trace;
using Datadog.Trace.Configuration;
using MassTransit;
using System.Reflection;
using Tracing.Shared.Messages;
using Tracing.Shared.TraceAndLong;
using Tracing.WebAapi;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

// Add services to the container.

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

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var tracingAndLogSection = configuration.GetSection("TracingAndLogging");

builder.Services.ConfigureTracing(tracingAndLogSection);

var rabbitMqConfig = new RabbitMqConfiguration();
configuration.GetSection("RabbitMq").Bind(rabbitMqConfig);


builder.Services.AddMassTransit(x =>
{
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("localhost", "TracingExample", c =>
        {
            c.Username("guest");
            c.Password("guest");
        });

        cfg.ReceiveEndpoint("OrderCompleted", xr =>
        {
            xr.ConfigureConsumer<OrderCompletedMessageConsumer>(context);
        });
    });

    x.AddConsumer<OrderCompletedMessageConsumer>();
});

builder.Host.ConfigureLogging(logging => logging.ClearProviders().ConfigureLogging(tracingAndLogSection));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
