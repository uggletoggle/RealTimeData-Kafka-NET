using Confluent.Kafka;
using Confluent.Kafka.Admin;

namespace RealTimeData.API.Services
{
    public class KafkaConsumer : IKafkaConsumer<Ignore, string>
    {
        private readonly IConsumer<Ignore, string> _consumer;
        private readonly ILogger<KafkaConsumer> _logger;

        public KafkaConsumer(ILogger<KafkaConsumer> logger)
        {
            _logger = logger;
            var config = new ConsumerConfig
            {
                BootstrapServers = "broker:29092",
                FetchMaxBytes = 1048576 * 1,
                AutoOffsetReset = AutoOffsetReset.Latest,
                MaxPartitionFetchBytes = 1048576 * 1,
                SocketKeepaliveEnable = true,
                ReconnectBackoffMs = 2000,
                TopicMetadataRefreshIntervalMs = 10000,
                GroupId = "foo"
            };

            using (var adminClient = new AdminClientBuilder(new AdminClientConfig { BootstrapServers = "broker:29092" }).Build())
            {
                try
                {
                    adminClient.CreateTopicsAsync(new TopicSpecification[] {
                    new TopicSpecification { Name = "fv2-deposit-decline-board-job", ReplicationFactor = 1, NumPartitions = 1 } });
                }
                catch (CreateTopicsException e)
                {
                    _logger.LogError($"An error occured creating topic {e.Results[0].Topic}: {e.Results[0].Error.Reason}");
                }
            }

            try
            {
                _consumer = new ConsumerBuilder<Ignore, string>(config).Build();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ConsumeResult<Ignore, string> Consume()
        {
            return _consumer.Consume();
        }

        public void Subscribe(IEnumerable<string> topics)
        {
            _consumer.Subscribe(topics);
        }

        public void OnMessage(ConsumeResult<Ignore, string> result)
        {
            _logger.LogInformation(result.Message.Value);
        }

        #region Dispose
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            _consumer.Close();
            _consumer.Dispose();
        } 
        #endregion
    }
}
