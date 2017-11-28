namespace ExtractorSharp.Loose {
    enum LSToken {
        None = -1,           // Used to denote no Lookahead available
        LBrace,
        RBrace,
        LBracket,
        RBracket,
        Colon,
        Comma,
        Dot,
        Charcter,
        String,
        Number,
        True,
        False,
        Null,
        Identifier,
        Expression,
    }
}
