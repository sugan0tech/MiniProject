using Confluent.Kafka;
using MatrimonyEventConsumer.Models;
using MatrimonyEventConsumer.Repos;
using Newtonsoft.Json;

namespace MatrimonyEventConsumer.Services;

public class ConsumerService(
    ILogger<ConsumerService> logger,
    IConfiguration configuration,
    IServiceScopeFactory serviceScopeFactory)
    : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var config = new ConsumerConfig
        {
            BootstrapServers = configuration["Kafka:BootstrapServers"],
            GroupId = configuration["Kafka:ConsumerGroupId"],
            AutoOffsetReset = AutoOffsetReset.Earliest
        };

        using var consumer = new ConsumerBuilder<int, string>(config).Build();
        consumer.Subscribe("address-events");

        try
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var consumeResult = consumer.Consume(stoppingToken);
                    if (consumeResult != null)
                    {
                        await ProcessMessage(consumeResult.Message);
                    }
                }
                catch (ConsumeException e)
                {
                    logger.LogError($"Error occurred: {e.Error.Reason}");
                }
            }
        }
        finally
        {
            consumer.Close();
        }
    }

    private async Task ProcessMessage(Message<int, string> message)
    {
        try
        {
            var eventPayload = JsonConvert.DeserializeObject<EventPayload>(message.Value);
            if (eventPayload != null)
            {
                using var scope = serviceScopeFactory.CreateScope();
                var repo = scope.ServiceProvider.GetRequiredService<IBaseRepo<Address>>();

                switch (eventPayload.EventType)
                {
                    case "AddressCreatedEvent":
                        await repo.Add(eventPayload.Address);
                        break;
                    case "AddressUpdatedEvent":
                        await repo.Update(eventPayload.Address);
                        break;
                    case "AddressDeletedEvent":
                        await repo.DeleteById(eventPayload.Address.Id);
                        break;
                    default:
                        logger.LogWarning($"Unknown event type: {eventPayload.EventType}");
                        break;
                }

                logger.LogInformation($"Processed {eventPayload.EventType} event for Address ID: {eventPayload.Address.Id}");
            }
        }
        catch (Exception ex)
        {
            logger.LogError($"Error processing message: {ex.Message}");
        }
    }
}