using System.Collections.Specialized;
using System.Text;

namespace FirstRealize.App.WebAccelerator.Models
{
    public class ResponseCache
    {
        public NameValueCollection Headers { get; set; }
        public string ContentType { get; set; }
        public string Charset { get; set; }
        public Encoding ContentEncoding { get; set; }
        public byte[] Content { get; set; }
        public int StatusCode { get; set; }
        public int SubStatusCode { get; set; }
        public string StatusDescription { get; set; }
    }
}