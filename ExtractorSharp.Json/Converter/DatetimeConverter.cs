using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtractorSharp.Json.Converter {
    class DatetimeConverter : IConverter
    {
        public LSType Type => LSType.String;

        public string Pattern => "";

        public bool Convert<T>(object Value, out T t)
        {
            t = default;
            return false;
        }
    }
}
