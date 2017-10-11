using hoTools.Utils.Extension;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;


namespace DuoVia.FuzzyStrings.ComparePhraseEngine
{
    public class Processor
    {
        #region Public methods
        public Processor()
        {
        }

        public Processor(IEnumerable<string> possibleValues)
        {
            foreach (var possibleValue in possibleValues)
            {
                AddPossibleValue(possibleValue);
            }
        }
       

        /// <summary>
        /// To add another PossibleValue into the internal storage.
        /// </summary>
        /// <param name="item"></param>
        public void AddPossibleValue(string item)
        {
            if (string.IsNullOrEmpty(item))
                throw new ArgumentOutOfRangeException();

            var newObj = MakePhrase(item);
            _queries.Add(newObj);
        }

        /// <summary>
        /// To delete the specified PossibleValue from the internal storage.
        /// </summary>
        /// <param name="item"></param>
        public void DeletePossibleValue(string item)
        {
            if (string.IsNullOrEmpty(item))
                throw new ArgumentOutOfRangeException();

            var found = _queries.FirstOrDefault(query => query.Text == item);
            if (found == null)
                throw new ArgumentOutOfRangeException(string.Format("No PossibleValue {0} is found",
                        item));
            _queries.Remove(found);
        }

        public double CalculateRank(string hayString, string needleString)
        {
            var needlePhraseh = MakePhrase(needleString);
            var hayPhrase = MakePhrase(hayString);
            return CalcRank(hayPhrase.Words, needlePhraseh.Words);
        }

        /// <summary>
        /// Searches for the auto-suggestions for the specified Query
        /// </summary>
        /// <param name="queryStr"></param>
        /// <returns>
        /// List of found auto-suggestions ordered by decreasing of their "relevancу"
        /// </returns>
        public IList<string> Search(string queryStr)
        {
            if (string.IsNullOrEmpty(queryStr))
                return new List<string>();

            var right = MakePhrase(queryStr);
            var rankedSequence = _queries.Select(q =>
                new RankedItem()
                {
                    Rank = CalcRank(q.Words, right.Words),
                    Text = q.Text

                });
            var sortedSequence = rankedSequence.Where(r => r.Rank > 0.0).
                OrderByDescending(r => r.Rank).
                Select(r => r.Text);
            return sortedSequence.ToList();
        }

        public IList<string> GetAllPossibleValues()
        {
            return _queries.Select(q => q.Text).ToList();
        }
        #endregion 

        IList<Phrase> _queries = new List<Phrase>();
        private const double Dif = 0.00000001;
        private const double MaxQueryRelativeWeight = 1.0; //In ]0, 1] range
        private const double MinQueryRelativeWeight = 0.5; //In ]0, MinQueryRelativeWeight[ range
        private const double IncreasingForUppercases = 1.1;
        private const double DecreasingFor2NdClassWord = 0.2;
        private const double AddendForWordWeightCalculation = 10;
        private const double WordPositionFactorBonusFor1StWord = 2.0; //must be > 1.0
        private const double WordPositionFactorAddendForCalculation = 10;
        private const double WordPositionFactorMinValue = 0.3; //In ]0, 1[ range
        
        

        /// <summary>
        /// Splits the input string on separate words using a pre-defined separators list
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private static Word[] SplitToWords(string text)
        {
            text = text.SplitCamelCase();
            var tokens = text.Split(new[] {' ', '\t', '!', '.', ',', ';', '(', ')', '\\', '/', '+', '-', ':', '\'', '"', '[', ']',
                '{', '}', '|', 
                '?', '—' /*the Long Dash*/, '–'/*the Short Dash*/},
                StringSplitOptions.RemoveEmptyEntries);

            var ret = tokens.Select(token =>
                new Word(token)).ToArray();

            return ret;
        }
        

        private static Phrase MakePhrase(string originalQueryStr)
        {
            var ret = new Phrase {Text = originalQueryStr, 
                    Words = SplitToWords(originalQueryStr)};
            return ret;
        }

        /// <summary>
        /// Calculates a "degree of similarity" between the 2 specified words. 
        /// The mininal requirement is: the right word should be a prefix the left word
        /// The algorithm of the "words similarity" uses the following factors: 
        ///     - the "words length factor": the closer are their lengths, the more "similar" are the words;
        ///     - the "capital letters factor": if a Query has capital letter(s) and the PossibleValue matches them case-sensitively, it increases the "similarity"
        ///     - the "word class factor”: some of the words (articles, prepositions) are "less important" than others
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        private static double CalcWordsSimilarityRank(Word left, Word right)
        {
            if (right.Canonical.Length > left.Canonical.Length)
                return 0.0; // The minimal requirement is: the right word should be a prefix the left word
            if (right.Canonical != left.Canonical.Substring(0, right.Canonical.Length)/*the Canonical string provides the case-insensitive comparison*/)
                return 0.0; // The minimal requirement is: the right word should be a [case-insensitive] prefix of the left word
            #region "words length factor": the closer are their lengths, the more "similar" are the words
            var ret = (double)right.Canonical.Length / left.Canonical.Length;
            #endregion
            #region "capital letters factor": if a Query has capital letter(s) and the PossibleValue matches them case-sensitively, it increases the "similarity"
            if (right.AreUppercasesPresent && right.Text.Length <= left.Text.Length && right.Text == left.Text.Substring(0, right.Text.Length))
            {//The right words contain uppercase letter(s); this increases its Similarity Rank
                ret *= IncreasingForUppercases;
            }
            #endregion  
            #region "word class factor”: some of the words (articles, prepositions) are "less important" than others
            if (left.ImportanceRank == Word.Importance.ArticleEtc)
            {//The left word is a "2nd class word" (like articles etc); this decreases its Similarity Rank
                ret *= DecreasingFor2NdClassWord;
            }
            #endregion
            Debug.Assert(ret >= 0.0);
            return ret;
        }

        /// <summary>
        /// Calculates SimilarityRank of the 2 phrases (represented as arrays of Words).
        /// </summary>
        /// <param name="possibleValueWords"></param>
        /// <param name="queryWords"></param>
        /// <returns></returns>
        private double CalcRank(Word[] possibleValueWords, Word[] queryWords)
        {
            var ret = 0.0;

            #region Calculate the "PhraseLengthFactor"
            var weightOfPossibleValuePhrase = CalcPhraseWeight(possibleValueWords);
            var weightOfQueryPhrase = CalcPhraseWeight(queryWords);
            var phraseLengthFactor = CalcPhraseLengthFactor(weightOfPossibleValuePhrase, weightOfQueryPhrase);
            #endregion 

            //-Enumerate all the possible "occurences" of queryWords within possibleValueWords; calculate SimlarityRank for each of them; choose one with highest SimlarityRank
            for (var i = 0; i <= possibleValueWords.Count() - queryWords.Count(); ++i)
            {
                var similarityRankOfTheOccurence = FindOccurenceAndCalcRank(possibleValueWords, i, queryWords,
                                                                            phraseLengthFactor);
                ret = Math.Max(ret, similarityRankOfTheOccurence);
            }
            return ret; 
        }

        /// <summary>
        /// Looks for an "occurence" of queryWords within possibleValueWords starting with position startPositionInPossibleValue.
        /// NOTE: if possibleValueWords[startPositionInPossibleValue] does not match to queryWords[0], we immediately stop the search and return 0. A calling code should then increment the startPositionInPossibleValue and call the method again; etc.
        /// </summary>
        /// <param name="possibleValueWords"></param>
        /// <param name="startPositionInPossibleValue"></param>
        /// <param name="queryWords"></param>
        /// <param name="phraseLengthFactor"></param>
        /// <returns>
        ///     0 when no occurence is found
        ///     > 0 when an occurence is found; in this case the returned value is the SimilarityRank of the found occurence
        /// </returns>
        private double FindOccurenceAndCalcRank(Word[] possibleValueWords, int startPositionInPossibleValue, Word[] queryWords, double phraseLengthFactor)
        {
            var ret = 0.0;
            int indxRight = 0;
            int indxLeft = startPositionInPossibleValue;
            while (indxLeft < possibleValueWords.Count() && indxRight < queryWords.Count())
            {
                var similarityRank = CalcWordsSimilarityRank(possibleValueWords[indxLeft], queryWords[indxRight]);
                if (similarityRank <= 0.0)
                {
                    if (indxLeft == startPositionInPossibleValue)
                        return 0.0; //if the very 1st tried word of the possibleValueWords does not match to queryWords - we can return immediately. 
                    ++indxLeft;
                    continue;
                }
                ret += similarityRank * CalcWordPositionFactor(indxLeft);
                ++indxRight;
                ++indxLeft;
            }
            if (indxRight < queryWords.Count())
                return 0.0; //not ALL the queryWords were found in the possibleValueWords; so "no occurence" is found here
            //all the queryWords were found in the possibleValueWords; we have found an "occurence" 
            ret *= phraseLengthFactor;
            return ret;
        }

        #region the PhraseLengthFactor factor
        // The "PhraseLengthFactor" factor says - how much space a matched Query pharse occupies in a given PossibleValue phrase.

        /// <summary>
        /// Calculates "Weight" of a separate Word
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        private double CalcWordWeight (Word word)
        {
            double ret = 0.0;

            ret += AddendForWordWeightCalculation + word.Canonical.Length;

            return ret;
        }

        /// <summary>
        /// Calculates "total Weight" of a phrase of Words
        /// </summary>
        /// <param name="phrase"></param>
        /// <returns></returns>
        private double CalcPhraseWeight(IEnumerable<Word> phrase)
        {
            var ret = phrase.Sum(word => 
                CalcWordWeight(word));
            return ret;
        }

        /// <summary>
        /// Calculates "PhraseLengthFactor" for the specified Weights
        /// </summary>
        /// <param name="weightOfPossibleValue"></param>
        /// <param name="weightOfQuery"></param>
        /// <returns></returns>
        private static double CalcPhraseLengthFactor(double weightOfPossibleValue, double weightOfQuery)
        {
            if (weightOfPossibleValue < weightOfQuery)
                return 1; //in fact, the PossibleValue should not match the Query in this situation. So we just return 1.0.

            Debug.Assert(weightOfQuery >= 0.0);
            Debug.Assert(MinQueryRelativeWeight > 0);
            Debug.Assert(MinQueryRelativeWeight <= 1.0);

            if (Math.Abs(weightOfPossibleValue) < Dif)
                return 1.0;
            if (Math.Abs(MinQueryRelativeWeight - MaxQueryRelativeWeight) < Dif)
                return 1.0;

            double ret = weightOfQuery / weightOfPossibleValue;

            ret = MinQueryRelativeWeight + ret * (MaxQueryRelativeWeight - MinQueryRelativeWeight);
            ret = Math.Min(ret, MaxQueryRelativeWeight);

            return ret;
        }
        #endregion

        
        private static double CalcWordPositionFactor(int positionInPossibleValue)
        {
            double ret = WordPositionFactorAddendForCalculation / (WordPositionFactorAddendForCalculation + positionInPossibleValue);
            if (positionInPossibleValue == 0)
                ret *= WordPositionFactorBonusFor1StWord;
            ret = Math.Max(ret, WordPositionFactorMinValue);
            return ret;
        }
    }


}
