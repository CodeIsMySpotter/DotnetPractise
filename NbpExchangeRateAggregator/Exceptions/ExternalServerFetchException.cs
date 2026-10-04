


namespace NbpExchangeRateAggregator.Exceptions;

public class ExternalServerFetchException : Exception
{
    public ExternalServerFetchException(string Message) : base(Message) { }
    public ExternalServerFetchException() : base("External Server returned no resonse") { }
    
    
    
}
