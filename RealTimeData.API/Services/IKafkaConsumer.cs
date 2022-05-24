using Confluent.Kafka;

namespace RealTimeData.API.Services
{
    public interface IKafkaConsumer<TKey, TValue> : IDisposable
    {
        void Subscribe(IEnumerable<TValue> topics);
        ConsumeResult<TKey, TValue> Consume();
        void OnMessage(ConsumeResult<TKey, TValue> result);
    }
}
