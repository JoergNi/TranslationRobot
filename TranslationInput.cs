using System.Text;

namespace TranslationRobot
{
    public class TranslationInput
    {
        private byte[] _encodedTranslation;
        public string Input { get; set; }
        public string Translation { get; set; }

        public byte[] EncodedTranslation
        {
            get
            {
                if (_encodedTranslation != null)
                    return _encodedTranslation;
                else
                {
                    return Encoding.UTF8.GetBytes(Translation);
                }
            }
            set { _encodedTranslation = value; }
        }
    }
}