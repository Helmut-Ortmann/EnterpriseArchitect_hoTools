using System;
using System.Collections.Generic;
using System.Linq;

namespace DuoVia.FuzzyStrings.ComparePhraseEngine
{
    /// <summary>
    /// An object of the class is a phrase (a PossibleValue or a Query) parsed into array of words
    /// </summary>
    class Phrase
    {
        public string Text { get; set; }
        public Word[] Words { get; set; }
    }

    class Word
    {
        public enum Importance
        {
            ArticleEtc = 1,
            MainClass = 2,
        }
        /// <summary>
        /// An original Word "as is"
        /// </summary>
        public string Text { get; set; }
        /// <summary>
        /// The "canonical" form here means - "case-insensitive"
        /// </summary>
        public string Canonical { get; set; }
        /// <summary>
        /// True if the Text contains at least 1 uppercase symbol 
        /// </summary>
        public bool AreUppercasesPresent { get; set; }
        public Importance ImportanceRank { get; set; }


        public Word(string text)
        {
            Text = text.Trim();
            Canonical = Text.ToLower();
            AreUppercasesPresent = Text.Any(Char.IsUpper);
            ImportanceRank = GetImportanceRank(Canonical);
        }

        public static Importance GetImportanceRank(string canonical)
        {
            if (ArticlesEtc.Contains(canonical))
                return Importance.ArticleEtc;
            return Importance.MainClass;
        }
        /// <summary>
        /// Words like articles etc are "second rank" ones; they are less important than others
        /// </summary>
        private static readonly List<string> ArticlesEtc = new List<string>()
                        {
                            "the",
                            "a",
                            "at",
                            "in",
                            "on",
                            "of",
                            "off",
                            "into",
                            "onto",
                            "by",
                        };
    }
}