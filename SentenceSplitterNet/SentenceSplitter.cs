using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SentenceSplitterNet
{
    //Adapted from https://github.com/mediacloud/sentence-splitter (based on algorithm by Philipp Koehn and Josh Schroeder)
    public class SentenceSplitter
    {
        private Dictionary<string, PrefixType> nonBreakingPrefixes;
        public enum PrefixType { NumericOnly, Default }

        public List<string> Split(string text)
        {
            // Add sentence breaks as needed:

            // Non-period end of sentence markers (?!) followed by sentence starters
            text = Regex.Replace(text,
                "([?!]) +(['\"([\\u00bf\\u00A1\\p{Pi}]*[\\p{Lu}\\p{Lo}])",
                "$1\n$2");
            // Multi-dots followed by sentence starters

            text = Regex.Replace(text,
                "(\\.[.]+) +(['\"([\\u00bf\\u00A1\\p{Pi}]*[\\p{Lu}\\p{Lo}])",
                "$1\n$2");

            // Add breaks for sentences that end with some sort of punctuation inside a quote or parenthetical and are
            // followed by a possible sentence starter punctuation and upper case
            text = Regex.Replace(text,
                "([?!.] *['\")\\]\\p{Pf}]+) +(['\"([\\u00bf\\u00A1\\p{Pi}]*[\\p{Lu}\\p{Lo}])",
                "$1\n$2");

            // Add breaks for sentences that end with some sort of punctuation are followed by a sentence starter punctuation
            // and upper case
            text = Regex.Replace(text,
                "([?!.]) +(['\"\\u00bf\\u00A1\\p{Pi}]+ *[\\p{Lu}\\p{Lo}])",
                "$1\n$2");

            // Special punctuation cases are covered. Check all remaining periods
            var words = Regex.Split(text, " +");
            var wordsAndNextWords =
                words.Take(words.Length - 1).Zip(words.Skip(1), (x, y) => Tuple.Create<string, string>(x, y));
            var textBuilder = new StringBuilder();

            foreach ((string word, string next_word) in wordsAndNextWords)
            {
                Boolean addBreak = false;
                var match = Regex.Match(word, "([\\w.-]*)(['\")\\]%\\p{Pf}]*)(\\.+)$");
                if (match.Success)
                {
                    var prefix = match.Groups[1].Value;
                    var starting_punct = match.Groups[2].Value;

                    if (this.IsPrefixHonorific(prefix, starting_punct))
                    {

                    }
                    else if (Regex.IsMatch(word, ".[\\p{Lu}\\p{Lo}-]+.+$"))
                    {

                    }
                    else if (Regex.IsMatch(next_word, "^ *['\"([\\u00bf\\u00A1\\p{Pi}]* *[\\p{Lu}\\p{Lo}0-9]"))
                    {
                        if (!this.IsNumeric(prefix, starting_punct, next_word))
                        {
                            addBreak = true;
                        }
                    }
                }

                textBuilder.Append(word);

                if (addBreak)
                {
                    textBuilder.Append('\n');
                }
                else
                {
                    textBuilder.Append(' ');
                }

            }

            // We stopped one token from the end to allow for easy look-ahead. Append it now.
            textBuilder.Append(words.Last());

            // Clean up spaces at head and tail of each line as well as any double-spacing
            var splitText = textBuilder.ToString();
            splitText = Regex.Replace(splitText, " +", " ");
            splitText = Regex.Replace(splitText, "\n ", "\n");
            splitText = Regex.Replace(splitText, " \n", "\n");
            splitText = splitText.Trim();

            var sentences = splitText.Split('\n');

            return sentences.ToList();
        }

        private Boolean IsNumeric(string prefix, string starting_punct, string next_word)
        {
            if (!String.IsNullOrWhiteSpace(prefix))
            {
                if (this.nonBreakingPrefixes.Keys.Contains(prefix))
                {
                    if (this.nonBreakingPrefixes[prefix] == PrefixType.NumericOnly)
                    {
                        if (String.IsNullOrWhiteSpace(starting_punct))
                        {
                            if (Regex.IsMatch(next_word, "^[0-9]+"))
                            {
                                return true;
                            }
                        }
                    }
                }
            }

            return false;
        }

        private Boolean IsPrefixHonorific(string prefix, string starting_punct)
        {
            if (!String.IsNullOrWhiteSpace(prefix))
            {
                if (this.nonBreakingPrefixes.Keys.Contains(prefix))
                {
                    if (this.nonBreakingPrefixes[prefix] == PrefixType.Default)
                    {
                        if (String.IsNullOrWhiteSpace(starting_punct))
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        public SentenceSplitter(string langIsoCode)
        {
            this.nonBreakingPrefixes = new Dictionary<string, PrefixType>();
            string nbpPath = $"non_breaking_prefixes\\{langIsoCode}.txt";
            if (File.Exists(nbpPath))
            {
                var nonBreakingPrefixLines = File.ReadAllLines(nbpPath).ToList().Where(
                    x => !x.StartsWith("#")).Where(
                    x => !String.IsNullOrWhiteSpace(x)).Select(
                    x => x.Trim()).ToList();

                foreach (var nonBreakingPrefixLine in nonBreakingPrefixLines)
                {
                    if (nonBreakingPrefixLine.Contains("#NUMERIC_ONLY#"))
                    {
                        var nonBreakingPrefixLineClean = nonBreakingPrefixLine.Replace("#NUMERIC_ONLY", "").Trim();
                        this.nonBreakingPrefixes[nonBreakingPrefixLineClean] = PrefixType.NumericOnly;
                    }
                    else
                    {
                        this.nonBreakingPrefixes[nonBreakingPrefixLine] = PrefixType.Default;

                    }
                }
            }
        }
    }
}
