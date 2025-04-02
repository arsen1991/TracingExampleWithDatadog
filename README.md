# TracingExample
This project is created based on https://github.com/persiandeveloper/TracingExample.

I have added also a ParallelConsumer project to show how to use the tracing in a parallel consumer.

The flow is below:
1. Tracing.WebApi publishes a message to RabbitMQ
2. Tracing.Worker and ParallelConsumer consume the message
3. Tracing.Worker publishes a message to RabbitMQ
4. Tracing.WebApi consumes the message

All of the applications send the trace to Jaeger and Datadog.

But we have found that Jaeger shows the traces better than Datadog. 
In Datadog we don't see the correlation between the traces.

The screenshots are attached.
![Alt text](DatadogTrace.png?raw=true "Datadog")
![Alt text](JaegerTrace.png?raw=true "Jaeger")
