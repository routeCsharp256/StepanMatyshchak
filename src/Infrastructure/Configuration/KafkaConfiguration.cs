namespace Infrastructure.Configuration
{
    /// <summary>
    /// Модель конфигураций для подключения к кафке
    /// </summary>
    public class KafkaConfiguration
    {
        /// <summary>
        /// Идентификатор ConsumerGroup
        /// </summary>
        public string GroupId { get; set; }
        
        /// <summary>
        /// Топик для создания евентов
        /// </summary>
        public string Topic { get; set; }
        
        /// <summary>
        /// Адрес сервера кафки 
        /// </summary>
        public string BootstrapServers { get; set; }
    }
}