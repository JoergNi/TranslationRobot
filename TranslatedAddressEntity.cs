using Microsoft.WindowsAzure.Storage.Table;

namespace TranslationRobot.Entity
{
    public class TranslatedAddressEntity:TableEntity
    {
        public const string DefaultPartitionKey = "Assorted";
        public TranslatedAddressEntity()
        {

        }

        public TranslatedAddressEntity(string input)
        {
            RowKey = input;
            PartitionKey = DefaultPartitionKey;
        }

        public string Country { get; set; }

        public string Translation   { get; set; }
    }
}