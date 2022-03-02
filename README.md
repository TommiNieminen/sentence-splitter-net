# sentence-splitter-net
.NET port of Koehn and Schroeder's sentence splitter

Text to sentence splitter using heuristic algorithm by Philipp Koehn and Josh Schroeder.

This module allows splitting of text paragraphs into sentences. It is based on scripts developed by Philipp Koehn and
Josh Schroeder for processing the Europarl corpus (<http://www.statmt.org/europarl/>).

This .NET port is based on the Python port (https://github.com/mediacloud/sentence-splitter) of Lingua::Sentence Perl module (<http://search.cpan.org/perldoc?Lingua::Sentence>).

Usage
-----

The module uses punctuation and capitalization clues to split plain text into a list of sentences:

    using SentenceSplitterNet;
    using System;

    namespace SentenceSplitterExample
    {
        class Program
        {
            static void Main(string[] args)
            {
                var splitter = new SentenceSplitter("fi");
                var input_text = "This is a paragraph. It contains several sentences. \"But why,\" you ask?";
                var sentences = splitter.Split(input_text);
                foreach (var sentence in sentences)
                {
                    Console.WriteLine(sentence);
                }
                Console.ReadLine();
            }
        }
    }

License
-------
.NET port, Copyright (C) 2022 Tommi Nieminen

Copyright (C) 2010 by Digital Silk Road, 2017 Linas Valiukas.

Portions Copyright (C) 2005 by Philip Koehn and Josh Schroeder (used with permission).

This program is free software: you can redistribute it and/or modify it under the terms of the GNU Lesser General Public
License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later
version.

This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied
warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU Lesser General Public License for more
details.

You should have received a copy of the GNU Lesser General Public License along with this program. If not, see
<http://www.gnu.org/licenses/>.
