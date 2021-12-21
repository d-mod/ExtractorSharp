namespace ExtractorSharp.Core.Lib {
    public class Optional<T> {

        private Optional() {

        }

        public T Get() {
            return default;
        }

        public Optional<T> Or(GetValue get) {

            return this;
        }

        public delegate void GetValue(out T t);

        public static Optional<T> With(GetValue get) {
            var optional = new Optional<T>();

            return null;
        }

    }
}
