namespace ExtractorSharp.Json.Converter {
    internal class DatetimeConverter : IConverter {
        public LSType Type => LSType.String;

        public string Pattern => "";

        public bool Convert<T>(object Value, out T t) {
            t = default;
            return false;
        }
    }
}