namespace ExtractorSharp.Loose.Unit {
    class Student {
        public string Name;
        public int Age;
        public char Sex;
        public string School;
        private string NickName;
        public byte[] buf;
        public void SetNickName(string NickName) {
            this.NickName = NickName;
        }
        public Teacher Teacher;
        public string GetNickName() => NickName;
    }
}
