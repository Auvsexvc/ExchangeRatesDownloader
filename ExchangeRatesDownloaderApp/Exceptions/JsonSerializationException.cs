namespace ExchangeRatesDownloaderApp.Exceptions
{
    public class JsonSerializationException : Exception
    {
        public JsonSerializationException() : base()
        {
        }

        public JsonSerializationException(string? message) : base(message)
        {
        }

        public JsonSerializationException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
