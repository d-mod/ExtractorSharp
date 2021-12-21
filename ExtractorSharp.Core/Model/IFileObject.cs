using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtractorSharp.Core.Model {

    public interface IFileObject {

        string Name { set; get; }

        void Save(Stream stream);

        void Load(Stream stream);
         
    }
}
