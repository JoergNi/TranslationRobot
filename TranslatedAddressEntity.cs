using Microsoft.WindowsAzure.Storage.Table;

namespace TranslationRobot
{
    public class TranslatedAddressEntity:TableEntity
    {
        public const string DefaultPartitionKey = "Assorted";
        public const string TableName = "TranslatedAddress";

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