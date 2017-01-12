using System;
using System.Runtime.Serialization;


namespace TranslationRobot
{
    

[Serializable]
public class AddressNotFoundException : Exception
{
    public AddressNotFoundException()
    {
    }

    public AddressNotFoundException(string message) : base(message)
    {
    }

    public AddressNotFoundException(string message, Exception innerException) : base(message, innerException)
    {
    }

    protected AddressNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}
}