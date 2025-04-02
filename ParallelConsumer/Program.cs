using Datadog.Trace;
using Datadog.Trace.Configuration;
using MassTransit;
using Microsoft.Extensions.Configuration;
using ParallelConsumer;
using System.Reflection;
using Tracing.Shared.Messages;
using static Tracing.Shared.TraceAndLong.ConfigureTracingAndLogging;

var builder = WebApplication.CreateBuilder(args);

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

var configuration = builder.Configuration;
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
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

        cfg.ReceiveEndpoint("OrderCompleted2", xr =>
        {
            xr.Bind("order");
            xr.ConfigureConsumer<OrderConsumer2>(context);
        });
    });

    x.AddConsumer<OrderConsumer2>();
});

builder.Host.ConfigureLogging(logging => logging.ClearProviders().ConfigureLogging(tracingAndLogSection));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.Run();
