namespace Infrastructure.Configuration
{
    /// <summary>
    /// Конфигурации подключения к сервису stockApi
    /// </summary>
    public class StockApiGrpcServiceConfiguration
    {
        /// <summary>
        /// Адрес сервера
        /// </summary>
        public string ServerAddress { get; set; }
    }
}