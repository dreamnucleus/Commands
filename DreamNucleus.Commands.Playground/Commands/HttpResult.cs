namespace DreamNucleus.Commands.Playground.Commands
{
    public class HttpResult
    {
        public int StatusCode { get; }

        public HttpResult(int statusCode)
        {
            StatusCode = statusCode;
        }
    }
}
