using System.Net;

namespace Models.Responses
{
    public class TrucksApiErrorResponse
    {
        public TrucksApiErrorResponse()
        {

        }

        public TrucksApiErrorResponse(int status, string title)
        {
            Status = status;
            Title = title;
        }

        public TrucksApiErrorResponse(HttpStatusCode status, string title) : this((int)status, title) { }

        public int Status { get; set; }
        public string Title { get; set; }
    }
}
