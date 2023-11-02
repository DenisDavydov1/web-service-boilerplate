namespace BoilerPlate.Services.Kafka.AdminClient;

public interface IKafkaAdminClient
{
    Task CreateTopicAsync(string name, int partitions, int retries = 10, CancellationToken ct = default);
}