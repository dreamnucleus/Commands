namespace DreamNucleus.Commands.Playground
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
