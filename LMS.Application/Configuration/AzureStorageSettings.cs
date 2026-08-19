namespace LMS.Application.Configuration
{
    public class AzureStorageSettings
    {
        public string ConnectionString { get; set; } = string.Empty;

        public string ContainerName { get; set; } = string.Empty;

        public string UserRegistrationQueueName { get; set; } = string.Empty;

        public string PaymentQueueName { get; set; } = string.Empty;
    }
}