

using System.Web.Http;

namespace ExtractorSharp.Host.Control {
    class FileController :ApiController{

       public string Get(int id) {
            return id.ToString();
        }
    }
}
