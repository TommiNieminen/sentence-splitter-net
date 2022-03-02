using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SentenceSplitterNet;

namespace SentenceSplitterTests
{
    [TestClass]
    public class SentenceSplitterTests
    {
        
        private void TestParagraph(SentenceSplitter splitter, string input_text, List<string> expected_sentences)
        {
            var actual_sentences = splitter.Split(input_text);
            Assert.IsTrue(expected_sentences.SequenceEqual(actual_sentences));
        }

        [TestMethod]
        public void TestFi()
        {
            var splitter = new SentenceSplitter("fi");

            this.TestParagraph(
               splitter,
               "Talvisodassa hän palveli mm. toimistopäällikkönä 5. divisioonassa sekä V. armeijakunnan esikunnassa. Jatkosodan aikana hän komensi pataljoonaa JR 53:ssa.",
               new List<string> {
                   "Talvisodassa hän palveli mm. toimistopäällikkönä 5. divisioonassa sekä V. armeijakunnan esikunnassa.",
                   "Jatkosodan aikana hän komensi pataljoonaa JR 53:ssa."
               });
        }

        [TestMethod]
        public void TestEs()
        {
            var splitter = new SentenceSplitter("pt");

            this.TestParagraph(
               splitter,
               "La UE ofrece una gran variedad de empleos en un entorno multinacional y multilingüe. La Oficina Europea de Selección de Personal (EPSO) se ocupa de la contratación, sobre todo mediante oposiciones generales.",
               new List<string> {
                   "La UE ofrece una gran variedad de empleos en un entorno multinacional y multilingüe.",
                    "La Oficina Europea de Selección de Personal (EPSO) se ocupa de la contratación, sobre todo mediante oposiciones generales."
               });
        }

        [TestMethod]
        public void TestPt()
        {
            var splitter = new SentenceSplitter("pt");

            this.TestParagraph(
               splitter,
               "Isto é um parágrafo. Contém várias frases. «Mas porquê,» perguntas tu?",
               new List<string> {
                   "Isto é um parágrafo.",
                    "Contém várias frases.",
                    "«Mas porquê,» perguntas tu?"
               });
        }

        [TestMethod]
        public void TestEl()
        {
            var splitter = new SentenceSplitter("el");

            this.TestParagraph(
               splitter,
               "Όλα τα συστήματα ανώτατης εκπαίδευσης σχεδιάζονται σε εθνικό επίπεδο. Η ΕΕ αναλαμβάνει κυρίως να συμβάλει στη βελτίωση της συγκρισιμότητας μεταξύ των διάφορων συστημάτων και να βοηθά φοιτητές και καθηγητές να μετακινούνται με ευκολία μεταξύ των συστημάτων των κρατών μελών.",
               new List<string> {
                   "Όλα τα συστήματα ανώτατης εκπαίδευσης σχεδιάζονται σε εθνικό επίπεδο.",
                   "Η ΕΕ αναλαμβάνει κυρίως να συμβάλει στη βελτίωση της συγκρισιμότητας μεταξύ των διάφορων συστημάτων και να βοηθά φοιτητές και καθηγητές να μετακινούνται με ευκολία μεταξύ των συστημάτων των κρατών μελών."
               });
        }

        [TestMethod]
        public void TestFr()
        {
            var splitter = new SentenceSplitter("fr");

            this.TestParagraph(
               splitter,
               "Brookfield Office Properties Inc. (« BOPI »), dont les actifs liés aux immeubles directement...",
               new List<string> { "Brookfield Office Properties Inc. (« BOPI »), dont les actifs liés aux immeubles directement..." });
        }

        [TestMethod]
        public void TestDe()
        {
            var splitter = new SentenceSplitter("de");

            this.TestParagraph(
               splitter,
               "Nie hätte das passieren sollen. Dr. Soltan sagte: \"Der Fluxcompensator war doch kalibriert!\".",
               new List<string> { "Nie hätte das passieren sollen.", "Dr. Soltan sagte: \"Der Fluxcompensator war doch kalibriert!\"." });
        }

        [TestMethod]
        public void TestEnSentenceWithinBrackets()
        {
            var splitter = new SentenceSplitter("en");

            this.TestParagraph(
               splitter,
               "Foo bar. (Baz foo.) Bar baz.",
               new List<string> { "Foo bar.", "(Baz foo.)", "Bar baz." });
        }

        [TestMethod]
        public void TestEnUpperCaseAcronym()
        {
            var splitter = new SentenceSplitter("en");

            this.TestParagraph(
               splitter,
               "Hello. .NATO. Good bye.",
               new List<string> { "Hello. .NATO. Good bye."});
        }

        [TestMethod]
        public void TestEnNumericOnly()
        {
            var splitter = new SentenceSplitter("en");

            this.TestParagraph(
               splitter,
               "Hello. No. 1. No. 2. Prefix. 1. Prefix. 2. Good bye.",
               new List<string> { "Hello.", "No. 1.", "No. 2.", "Prefix.", "1.", "Prefix.", "2.", "Good bye." });
        }

        [TestMethod]
        public void TestEn()
        {
            var splitter  = new SentenceSplitter("en");
            
            this.TestParagraph(
                splitter,
                "This is a paragraph. It contains several sentences. \"But why,\" you ask?",
                new List<string> { "This is a paragraph.", "It contains several sentences.", "\"But why,\" you ask?" });

            this.TestParagraph(
               splitter,
               "Hey! Now.",
               new List<string> { "Hey!", "Now." });

            this.TestParagraph(
               splitter,
               "Hey... Now.",
               new List<string> { "Hey...", "Now." });

            this.TestParagraph(
               splitter,
               "Hey.  Now.",
               new List<string> { "Hey.", "Now." });
        }
    }
}
