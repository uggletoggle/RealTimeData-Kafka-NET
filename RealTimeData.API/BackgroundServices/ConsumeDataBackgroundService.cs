using Confluent.Kafka;
using RealTimeData.API.Services;
using System.Net;

namespace RealTimeData.API.BackgroundServices
{
    public class ConsumeDataBackgroundService : BackgroundService
    {
        private readonly IKafkaConsumer<Ignore, string> _consumer;
        public ConsumeDataBackgroundService(IKafkaConsumer<Ignore, string> kafkaConsumer)
        {
            _consumer = kafkaConsumer;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                _consumer.Subscribe(new[] { "fv2-deposit-decline-board-job" });
                _ = Task.Run( () => 
                {
                    while (true)
                    {
                        var consumeResult = _consumer.Consume();
                        _consumer.OnMessage(consumeResult);
                    }
                });
            }

            catch (Exception ex)
            {
                Console.WriteLine($"{(int)HttpStatusCode.InternalServerError} ConsumeFailedOnTopic - compresor-1-data-topic, {ex}");
            }

        }

        public override void Dispose()
        {
            _consumer.Dispose();
            base.Dispose();
        }
    }
}
