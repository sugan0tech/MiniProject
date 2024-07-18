using Confluent.Kafka;
using Newtonsoft.Json;

namespace MatrimonyApiService.AddressCQRS.Event;

public class EventProducerService(IConfiguration configuration, ILogger<EventProducerService> logger)
{
    public void Produce(AddressProducerEvent addressEvent)
    {
        ProducerConfig config = new ProducerConfig { BootstrapServers = configuration["Kafka:BootstrapServers"] };

        using var producer = new ProducerBuilder<int, string>(config).Build();
        try
        {
            var dr = producer.ProduceAsync("address-events", new Message<int, string> {Key = addressEvent.AddressId, Value = JsonConvert.SerializeObject(addressEvent.EventPayload) }).Result;
            logger.LogInformation($"Delivered '{dr.Value}' to '{dr.TopicPartitionOffset}'");
        }
        catch (ProduceException<Null, string> e)
        {
            logger.LogError($"Delivery failed: {e.Error.Reason}");
        }
    }
}