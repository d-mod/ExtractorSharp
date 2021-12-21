namespace ExtractorSharp.Composition.Stores {
    public interface IStoreFilter {

        bool IsMatch(string key);

        object Get(string key);

        void Set(string key, object value);

    }
}
