using System.IO;

namespace ExtractorSharp.Host.Http {
    public class HttpRequest {
        public string Method { set; get; } = "POST";
        public string Url { set; get; }
        public string Host { set; get; }
        public string Content { set; get; }

        public static HttpRequest Create(string str) {
            if (string.IsNullOrEmpty(str)) {
                return null;
            }
            var reader = new StringReader(str);
            var frist=reader.ReadLine();
            var meta = frist.Split(' ');
            var request = new HttpRequest();
            request.Method = meta[0];
            request.Url = meta[1];
            request.Host = reader.ReadLine();
            var line = "";
            do {
                line = reader.ReadLine();
            } while (line != null && line != string.Empty);
            request.Content = reader.ReadToEnd();
            reader.Close();
            return request;
        }

       
    }
}
