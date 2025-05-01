using System.Net;
using System.Text.Json.Serialization;

namespace UdemyReact
{
    public class ResponseBody
    {
        public ResponseBody() { }
        public ResponseBody(HttpStatusCode status, string message) 
        {
            this.Status = status;
            this.Message = message;
        }

        [JsonPropertyName("source")]
        public string? Source { get; } = typeof(ResponseBody).Namespace;

        [JsonPropertyName("status")]
        public HttpStatusCode? Status { get; set; }

        [JsonPropertyName("title")]
        public string? Message { get; set; }
    }
}
