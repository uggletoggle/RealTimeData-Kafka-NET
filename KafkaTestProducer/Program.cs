// See https://aka.ms/new-console-template for more information
using Confluent.Kafka;
using System.Text.Json;

Console.WriteLine("--> Start Kafka Producer");

var config = new ProducerConfig
{
    BootstrapServers = "localhost:9092",
    BatchNumMessages = 16384 * 1,
    LingerMs = 1,
    CompressionType = CompressionType.Gzip,
    Acks = Acks.Leader,
    MessageMaxBytes = 20000000
};

var producer = new ProducerBuilder<Null, string>(config).Build();

try
{
    while (true)
    {
        await Task.Delay(2000);
        Console.WriteLine("--> Producing message");
        var random = new Random();
        await producer.ProduceAsync("fv2-deposit-decline-board-job", new Message<Null, string>
        {
            Value = JsonSerializer.Serialize(new { data = random.Next() })
        });
    }
}
catch (Exception ex)
{
    Console.WriteLine("--> Error happened: " + ex.Message);
}

Console.ReadKey();