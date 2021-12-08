using Confluent.Kafka;

namespace Infrastructure.MessageBroker
{
    public interface IProducerBuilderWrapper
    {
        /// <summary>
        /// Producer instance
        /// </summary>
        IProducer<string, string> Producer { get; set; }

        /// <summary>
        /// Топик для отправки сообщения что пришла новая поставка
        /// </summary>
        string EmailNotificationTopic { get; set; }
    }
}
