// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MergeEngine.cs" company="--">
//   Copyright © -- 2011. All Rights Reserved.
// </copyright>
// <summary>
//   When we compare two files, we say we delete or add
//   some sub sequences in the original file to result
//   in the modified file. This is to define the strong
//   type for identifying the status of a such sequence.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

////#define MyDEBUG

// MergeEngine.cs
// This file contains the implementation of the functionality to compare
// and merge two Html strings. 
// The comparison is word-wise, not character wise. Thus, the process 
// consists of two steps. 
// The first step is to parse the html string into collection of English 
// words(strong typed collection WordsCollection is defined for this) in 
// such a way that:
//    1) anything starts with '<' and ends with '>' is treated as Html 
//       tag.
//    2) Html tags and whitespaces are treated as prefix or suffix to 
//       adjacent word and be put in the prefix or suffix fileds of the 
//       Word object.
//    3) English words separated by space(s), "&nbsp;", "&#xxx", 
//       tailing punctuation are treated as words and be put in the 
//       word field of Word class.
//    4) Whitespaces immediately after or before Html tags are ignored.
//      ( whitespaces == {' ', '\t', '\n'} )
// The second step is to compare and merge the two words collections by 
// the algorithm proposed by [1]. The follwoing are the basic steps of 
// the algorithm (read [1] for details):
//    1) Find the middle snake of the two sequences by searching from 
//       both the left-up and right-bottom corners of the edit graph at 
//       the same time. When the furthest reaching paths of the two 
//       searches first meet, the snake is reported as middle snake. It 
//       may be empty sequence(or most likely be?).
//    2) For the sub-sequences before the middle snake and the 
//       sub-sequences after the middle snake, do recursion on them.
//    3) Some key nomenclature:
//       Edit Graph -- for sequences A(N) and B(M), construct graph in 
//                     such a way that there is always edge from (A(i-1), B) 
//                     to (A(i), B) and edge from (A, B(j-1)) to 
//                     (A, B(j)) (vertical or parallel edge). If A(i) 
//                     == B(j) then there is edge from (A(i-1), B(j-1))
//                     to (A(i), B(j)) (diagonal edge).
//       Snake -- not the kind of animal here ..). a sequence of diagonal
//                edges surrounded by non-diagonal edges at both ends.
//       Furthest Reaching Path -- searching from the left-up corner toward
//                the right-bottom corner, the path that goes closest to
//                the right-bottom corner(in other words, there are more
//                disgonal edges on this path).
//       LCS / SES -- Longest Common Sequence and Shortest Edit Script. 
//                Simple say, the shortest path between left-up and right-bottom
//                corners of the edit graph. 
// [1] Eugene W. Myers, "An O(ND) Difference Algorithm and Its Variations"
//     A copy of the file can be found at:
//     http://www.xmailserver.org/diff2.pdf
// [2] http://cvs.sourceforge.net/viewcvs.py/*checkout*/cvsgui/cvsgui/cvs-1.10/diff/analyze.c?&rev=1.1.1.3     
// The file is created to be used inside Appleseed(www.Appleseedportal.net)
// to compare the staging and production contents of HtmlDocument module
// while working in Workflow mode. However, this file can be easily 
// modified to be used in other senario.
// All of the code in this file are implemented from scratch by the 
// author, with reference to the Unix Diff implementation in [2].
// This program is free and can be distributed or used for any purpose 
// with no restriction.  
// The author would like to thank Matt Cowan(mcowan@county.oxford.on.ca)
// for pushing this work and undertaking lots of testings.
// Author: Hongwei Shen 
// Email:  hongwei.shen@gmail.com
// Date:   June 22, 2005

namespace Appleseed.Framework.BLL.MergeEngine
{
    using System;
    using System.Collections;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;

    #region Data types

    /// <summary>
    /// When we compare two files, we say we delete or add 
    ///     some sub sequences in the original file to result 
    ///     in the modified file. This is to define the strong 
    ///     type for identifying the status of a such sequence.
    /// </summary>
    internal enum SequenceStatus
    {
        /// <summary>
        ///     The sequence is inside the original
        ///     file but not in the modified file
        /// </summary>
        Deleted = 0, 

        /// <summary>
        ///     The sequence is inside the modified
        ///     file but not in the original file
        /// </summary>
        Inserted, 

        /// <summary>
        ///     The sequence is in both the original 
        ///     and the modified files
        /// </summary>
        NoChange
    }

    /// <summary>
    /// The class defines the beginning and end html tag 
    ///     for marking up the deleted words in the merged
    ///     file.
    /// </summary>
    internal class CommentOff
    {
        #region Constants and Fields

        /// <summary>
        /// The begin tag.
        /// </summary>
        public static string BeginTag = "<span style=\"text-decoration: line-through; color: red\">";

        /// <summary>
        /// The end tag.
        /// </summary>
        public static string EndTag = "</span>";

        #endregion
    }

    /// <summary>
    /// The class defines the beginning and end html tag 
    ///     for marking up the added words in the merged
    ///     file.
    /// </summary>
    internal class Added
    {
        #region Constants and Fields

        /// <summary>
        /// The begin tag.
        /// </summary>
        public static string BeginTag = "<span style=\"background: SpringGreen\">";

        /// <summary>
        /// The end tag.
        /// </summary>
        public static string EndTag = "</span>";

        #endregion
    }

    /// <summary>
    /// Data structure for marking start and end indexes of a 
    ///     sequence
    /// </summary>
    internal class Sequence
    {
        #region Constants and Fields

        /// <summary>
        ///     The end index of the sequence. It is 
        ///     open end.
        /// </summary>
        public int EndIndex;

        /// <summary>
        ///     The start index of the sequence
        /// </summary>
        public int StartIndex;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Sequence"/> class. 
        ///     Default constructor
        /// </summary>
        public Sequence()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Sequence"/> class. 
        /// Overloaded Constructor that takes the start
        ///     and end indexes of the sequence. Note that 
        ///     the interval is open on right hand side, say,
        ///     it is like [startIndex, endIndex).
        /// </summary>
        /// <param name="startIndex">
        /// The starting index of the sequence
        /// </param>
        /// <param name="endIndex">
        /// The end index of the sequence.
        /// </param>
        public Sequence(int startIndex, int endIndex)
        {
            this.StartIndex = startIndex;
            this.EndIndex = endIndex;
        }

        #endregion
    }

    /// <summary>
    /// This class defines middle common sequence in the original 
    ///     file and the modified file. It is called middle in the 
    ///     sense that it is the common sequence when the furthest 
    ///     forward reaching path in the top-down seaching first overlaps 
    ///     the furthest backward reaching path in the bottom up search.
    ///     See the listed reference at the top for more details.
    /// </summary>
    internal class MiddleSnake
    {
        #region Constants and Fields

        /// <summary>
        ///     The indexes of middle snake in the destination 
        ///     sequence
        /// </summary>
        public Sequence Destination;

        /// <summary>
        ///     The length of the Shortest Edit Script for the 
        ///     path this snake is found.
        /// </summary>
        public int SES_Length;

        /// <summary>
        ///     The indexes of middle snake in source sequence
        /// </summary>
        public Sequence Source;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MiddleSnake"/> class.
        /// </summary>
        public MiddleSnake()
        {
            this.Source = new Sequence();
            this.Destination = new Sequence();
        }

        #endregion
    }

    /// <summary>
    /// An array indexer class that maps the index of an integer 
    ///     array from -N ~ +N to 0 ~ 2N.
    /// </summary>
    internal class IntVector
    {
        #region Constants and Fields

        /// <summary>
        /// The n.
        /// </summary>
        private readonly int n;

        /// <summary>
        /// The data.
        /// </summary>
        private readonly int[] data;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="IntVector"/> class.
        /// </summary>
        /// <param name="N">
        /// The n.
        /// </param>
        public IntVector(int N)
        {
            this.data = new int[2 * N];
            this.n = N;
        }

        #endregion

        #region Indexers

        /// <summary>
        /// Gets or sets the <see cref="System.Int32"/> at the specified index.
        /// </summary>
        /// <remarks></remarks>
        public int this[int index]
        {
            get
            {
                return this.data[this.n + index];
            }

            set
            {
                this.data[this.n + index] = value;
            }
        }

        #endregion
    }

    #endregion

    #region Word and Words Collection

    /// <summary>
    /// This class defines the data type for representing a 
    ///     word. The word may have leading or tailing html tags 
    ///     or other special characters. Those prefix or suffix 
    ///     are not compared.
    /// </summary>
    internal class Word : IComparable
    {
        #region Constants and Fields

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Word"/> class. 
        ///     Default constructor
        /// </summary>
        public Word()
        {
            this.word = string.Empty;
            this.Prefix = string.Empty;
            this.Suffix = string.Empty;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Word"/> class. 
        /// Overloaded constructor
        /// </summary>
        /// <param name="word">
        /// The word
        /// </param>
        /// <param name="prefix">
        /// The prefix of the word, such as html tags
        /// </param>
        /// <param name="suffix">
        /// The suffix of the word, such as spaces.
        /// </param>
        public Word(string word, string prefix, string suffix)
        {
            this.word = word;
            this.Prefix = prefix;
            this.Suffix = suffix;
        }

        #endregion

        #region Properties

        /// <summary>
        ///     Gets or sets the prefix of the word
        /// </summary>
        /// <value>The prefix.</value>
        public string Prefix { get; set; }

        /// <summary>
        ///     Gets or sets the suffix of the word
        /// </summary>
        /// <value>The suffix.</value>
        public string Suffix { get; set; }

        /// <summary>
        ///     Gets or sets the word itself
        /// </summary>
        /// <value>The word.</value>
        public string word { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Reconstruct the text string from the word
        ///     itself without any other decoration.
        /// </summary>
        /// <returns>
        /// Constructed string
        /// </returns>
        public string reconstruct()
        {
            return this.Prefix + this.word + this.Suffix;
        }

        /// <summary>
        /// Overloaded function reconstructing the text
        ///     string with additional decoration around the
        ///     _word.
        /// </summary>
        /// <param name="beginTag">
        /// The begining html tag to mark the _word
        /// </param>
        /// <param name="endTag">
        /// The end html tag to mark the _word
        /// </param>
        /// <returns>
        /// The constructed string
        /// </returns>
        public string reconstruct(string beginTag, string endTag)
        {
            return this.Prefix + beginTag + this.word + endTag + this.Suffix;
        }

        #endregion

        #region Implemented Interfaces

        #region IComparable

        /// <summary>
        /// Implementation of the CompareTo. It compares
        ///     the _word field.
        /// </summary>
        /// <param name="obj">
        /// An object to compare with this instance.
        /// </param>
        /// <returns>
        /// A 32-bit signed integer that indicates the relative order of the objects being compared. The return value has these meanings: Value Meaning Less than zero This instance is less than obj. Zero This instance is equal to obj. Greater than zero This instance is greater than obj.
        /// </returns>
        /// <exception cref="T:System.ArgumentException">
        /// obj is not the same type as this instance. 
        /// </exception>
        public int CompareTo(object obj)
        {
            if (obj is Word)
            {
                return this.word.CompareTo(((Word)obj).word);
            }
            
            throw new ArgumentException("The obj is not a Word", obj.ToString());
        }

        #endregion

        #endregion
    }

    /// <summary>
    /// Strongly typed collection of Word object
    /// </summary>
    internal class WordsCollection : CollectionBase
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="WordsCollection"/> class. 
        ///     Default constructor
        /// </summary>
        public WordsCollection()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WordsCollection"/> class. 
        /// Constructor to populate collection from an ArrayList
        /// </summary>
        /// <param name="list" type="ArrayList">
        /// ArrayList of Words
        /// </param>
        public WordsCollection(IEnumerable list)
        {
            foreach (var item in list.OfType<Word>())
            {
                this.List.Add(item);
            }
        }

        #endregion

        #region Indexers

        /// <summary>
        /// Array indexing operator -- get Word object at
        /// the index
        /// </summary>
        /// <value></value>
        public Word this[int index]
        {
            get
            {
                return (Word)this.List[index];
            }

            set
            {
                this.List[index] = value;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Add a Word object to the collection
        /// </summary>
        /// <param name="item" type="Word">
        /// Word object
        /// </param>
        /// <returns type="integer">
        /// Zero based index of the added Word object in 
        ///     the colleciton
        /// </returns>
        public int Add(Word item)
        {
            return this.List.Add(item);
        }

        /// <summary>
        /// Check if the Word object is in the collection
        /// </summary>
        /// <param name="item" type="Word">
        /// Word object
        /// </param>
        /// <returns type="bool">
        /// Boolean value of the checking result
        /// </returns>
        public bool Contains(Word item)
        {
            return this.List.Contains(item);
        }

        /// <summary>
        /// Copy this WordsCollection to another one 
        ///     starting at the specified index position
        /// </summary>
        /// <param name="col" type="WordsCollection">
        /// WordsCollection to be copied to
        /// </param>
        /// <param name="index" type="integer">
        /// Starting index to begin copy operations
        /// </param>
        public void CopyTo(WordsCollection col, int index)
        {
            for (var i = index; i < this.List.Count; i++)
            {
                col.Add(this[i]);
            }
        }

        /// <summary>
        /// Overloaded. Copy this WordsCollection to another one 
        ///     starting at the index zero
        /// </summary>
        /// <param name="col" type="WordCollection">
        /// WordsCollection to copy to
        /// </param>
        public void CopyTo(WordsCollection col)
        {
            this.CopyTo(col, 0);
        }

        /// <summary>
        /// Returns zero based index of the Word object in 
        ///     the collection
        /// </summary>
        /// <param name="item" type="Word">
        /// Word object to be checked for index
        /// </param>
        /// <returns type="integer">
        /// Zero based index of Word object in the collection
        /// </returns>
        public int IndexOf(Word item)
        {
            return this.List.IndexOf(item);
        }

        /// <summary>
        /// Add Word object to the collection at specified index
        /// </summary>
        /// <param name="index" type="integer">
        /// Zero based index
        /// </param>
        /// <param name="item" type="Word">
        /// Word object
        /// </param>
        public void Insert(int index, Word item)
        {
            this.List.Insert(index, item);
        }

        /// <summary>
        /// Remove the Word object from collection
        /// </summary>
        /// <param name="item" type="Word">
        /// Word object to be removed
        /// </param>
        public void Remove(Word item)
        {
            this.List.Remove(item);
        }

        #endregion
    }

    #endregion

    #region Html Text Paser

    /// <summary>
    /// The class defines static method that processes html text
    /// string in such a way that the text is striped out into
    /// separate english words with html tags and some special
    /// characters as the prefix or suffix of the words. This way,
    /// the original html text string can be reconstructed to
    /// retain the original appearance by concating each word
    /// object in the collection in such way as word.prefix +
    /// word.word + word.suffix.
    /// The generated words collection will be used to compare
    /// the difference with another html text string in such format.
    /// </summary>
    internal class HtmlTextParser
    {
        #region Public Methods

        /// <summary>
        /// Static method that parses the passed-in string into
        ///     Words collection
        /// </summary>
        /// <param name="s">
        /// String
        /// </param>
        /// <returns>
        /// Words Collection
        /// </returns>
        public static WordsCollection parse(string s)
        {
            var curPos = 0;
            var prefix = string.Empty;
            var suffix = string.Empty;
            var words = new WordsCollection();

            while (curPos < s.Length)
            {
                // eat the leading or tailing white spaces 
                int prevPos = curPos;
                while (curPos < s.Length && (char.IsControl(s[curPos]) || char.IsWhiteSpace(s[curPos])))
                {
                    curPos++;
                }

                prefix += s.Substring(prevPos, curPos - prevPos);

                if (curPos == s.Length)
                {
                    // it is possible that there are
                    // something in the prefix
                    if (prefix != string.Empty)
                    {
                        // report a empty word with prefix.
                        words.Add(new Word(string.Empty, prefix, string.Empty));
                    }

                    break;
                }

                // we have 3 different cases here, 
                // 1) if the string starts with '<', we assume 
                // that it is a html tag which will be put 
                // into prefix.
                // 2) starts with '&', we need to check if it is 
                // "&nbsp;" or "&#xxx;". If it is the former, 
                // we treat it as prefix and if it is latter, 
                // we treat it as a word.
                // 3) a string that may be a real word or a set 
                // of words separated by "&nbsp;" or may have
                // leading special character or tailing 
                // punctuation. 
                // Another possible case that is too complicated
                // or expensive to handle is that some special
                // characters are embeded inside the word with 
                // no space separation
                if (s[curPos] == '<')
                {
                    // it is a html tag, consume it
                    // as prefix.
                    prevPos = curPos;
                    while (s[curPos] != '>' && curPos < s.Length)
                    {
                        curPos++;
                    }

                    prefix += s.Substring(prevPos, curPos - prevPos + 1);

                    if (curPos == s.Length)
                    {
                        // if we come to this point, it means
                        // the html tag is not closed. Anyway,
                        // we are not validating html, so just 
                        // report a empty word with prefix.
                        words.Add(new Word(string.Empty, prefix, string.Empty));
                        break;
                    }

                    // curPos is pointing to '>', move
                    // it to next.
                    curPos++;
                    if (curPos == s.Length)
                    {
                        // the html tag is closed but nothing more 
                        // behind, so report a empty word with prefix.
                        words.Add(new Word(string.Empty, prefix, string.Empty));
                        break;
                    }

                    continue;
                }

                string word;
                if (s[curPos] == '&')
                {
                    prevPos = curPos;

                    // case for html whitespace
                    if (curPos + 6 < s.Length && s.Substring(prevPos, 6) == "&nbsp;")
                    {
                        prefix += "&nbsp;";
                        curPos += 6;
                        continue;
                    }

                    // case for special character like "&#123;" etc
                    var pattern = @"&#[0-9]{3};";
                    var r = new Regex(pattern);

                    if (curPos + 6 < s.Length && r.IsMatch(s.Substring(prevPos, 6)))
                    {
                        words.Add(new Word(s.Substring(prevPos, 6), prefix, string.Empty));
                        prefix = string.Empty;
                        curPos += 6;
                        continue;
                    }

                    // case for special character like "&#12;" etc
                    pattern = @"&#[0-9]{2};";
                    r = new Regex(pattern);
                    if (curPos + 5 < s.Length && r.IsMatch(s.Substring(prevPos, 5)))
                    {
                        words.Add(new Word(s.Substring(prevPos, 5), prefix, string.Empty));
                        prefix = string.Empty;
                        curPos += 5;
                        continue;
                    }

                    // can't think of anything else that is special,
                    // have to treat it as a '&' leaded word. Hope 
                    // it is just single '&' for and in meaning.
                    prevPos = curPos;
                    while (curPos < s.Length && !char.IsControl(s[curPos]) && !char.IsWhiteSpace(s[curPos]) &&
                           s[curPos] != '<')
                    {
                        curPos++;
                    }

                    word = s.Substring(prevPos, curPos - prevPos);

                    // eat the following witespace as suffix
                    prevPos = curPos;
                    while (curPos < s.Length && (char.IsControl(s[curPos]) || char.IsWhiteSpace(s[curPos])))
                    {
                        curPos++;
                    }

                    suffix += s.Substring(prevPos, curPos - prevPos);

                    words.Add(new Word(word, prefix, suffix));
                    prefix = string.Empty;
                    suffix = string.Empty;
                }
                else
                {
                    // eat the word
                    prevPos = curPos;
                    while (curPos < s.Length && !char.IsControl(s[curPos]) && !char.IsWhiteSpace(s[curPos]) &&
                           s[curPos] != '<' && s[curPos] != '&')
                    {
                        curPos++;
                    }

                    word = s.Substring(prevPos, curPos - prevPos);

                    // if there are newlines or spaces follow
                    // the word, consume it as suffix
                    prevPos = curPos;
                    while (curPos < s.Length && (char.IsControl(s[curPos]) || char.IsWhiteSpace(s[curPos])))
                    {
                        curPos++;
                    }

                    suffix = s.Substring(prevPos, curPos - prevPos);
                    ProcessWord(words, prefix, word, suffix);
                    prefix = string.Empty;
                    suffix = string.Empty;
                }
            }

            return words;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Further processing of a string
        /// </summary>
        /// <param name="words">
        /// Collection that new word(s) will be added in
        /// </param>
        /// <param name="prefix">
        /// prefix come with the string
        /// </param>
        /// <param name="word">
        /// A string that may be a real word or have leading or tailing
        ///     special character
        /// </param>
        /// <param name="suffix">
        /// suffix comes with the string.
        /// </param>
        private static void ProcessWord(WordsCollection words, string prefix, string word, string suffix)
        {
            // the passed in word may have leading special
            // characters such as '(', '"' etc or tailing 
            // punctuations. We need to sort this out.
            var length = word.Length;

            if (length == 1)
            {
                words.Add(new Word(word, prefix, suffix));
            }
            else if (!char.IsLetterOrDigit(word[0]))
            {
                // it is some kind of special character in the first place
                // report it separately
                words.Add(new Word(word[0].ToString(), prefix, string.Empty));
                words.Add(new Word(word.Substring(1), string.Empty, suffix));
                return;
            }
            else if (char.IsPunctuation(word[length - 1]))
            {
                // there is a end punctuation
                words.Add(new Word(word.Substring(0, length - 1), prefix, string.Empty));
                words.Add(new Word(word[length - 1].ToString(), string.Empty, suffix));
            }
            else
            {
                // it is a real word(hope so)
                words.Add(new Word(word, prefix, suffix));
            }
        }

        #endregion
    }

    #endregion

    #region Merge Engine

    /// <summary>
    /// The class provides functionality to compare two html 
    ///     files and merge them into a new file with differences
    ///     highlighted
    /// </summary>
    public class Merger
    {
        #region Constants and Fields

        /// <summary>
        /// The modified.
        /// </summary>
        private readonly WordsCollection modified;

        /// <summary>
        /// The original.
        /// </summary>
        private readonly WordsCollection original;

        /// <summary>
        /// The bwd vector.
        /// </summary>
        private readonly IntVector bwdVector;

        /// <summary>
        /// The fwd vector.
        /// </summary>
        private readonly IntVector fwdVector;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Merger"/> class. 
        /// </summary>
        /// <param name="original">
        /// The original.
        /// </param>
        /// <param name="modified">
        /// The modified.
        /// </param>
        public Merger(string original, string modified)
        {
            // parse the passed in string to words 
            // collections
            this.original = HtmlTextParser.parse(original);
            this.modified = HtmlTextParser.parse(modified);

            // for hold the forward searching front-line
            // in previous searching loop
            this.fwdVector = new IntVector(this.original.Count + this.modified.Count);

            // for hold the backward searching front-line
            // in the previous seaching loop
            this.bwdVector = new IntVector(this.original.Count + this.modified.Count);
        }

        #endregion

        #region Properties

        /// <summary>
        ///     Gets the number of words in the parsed modified file
        /// </summary>
        /// <value>The words in modified file.</value>
        public int WordsInModifiedFile
        {
            get
            {
                return this.modified.Count;
            }
        }

        /// <summary>
        ///     Gets the number of words in the parsed original file.
        /// </summary>
        /// <value>The words in original file.</value>
        public int WordsInOriginalFile
        {
            get
            {
                return this.original.Count;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// The public function merges the two copies of
        ///     files stored inside this class. The html tags
        ///     of the destination file is used in the merged
        ///     file.
        /// </summary>
        /// <returns>
        /// The merged file
        /// </returns>
        public string merge()
        {
            var src = new Sequence(0, this.original.Count);
            var des = new Sequence(0, this.modified.Count);

            return this.DoMerge(src, des);
        }

        #endregion

        #region Methods

        /// <summary>
        /// The function returns a html text string reconstructed
        ///     from the sub collection of words its starting and ending
        ///     indexes are marked by parameter seq and its collection is
        ///     denoted by parameter status. If the status is "deleted",
        ///     then the _original collection is used, otherwise, _modified
        ///     is used.
        /// </summary>
        /// <param name="seq">
        /// Sequence object that marks the start index and end
        ///     index of the sub sequence
        /// </param>
        /// <param name="status">
        /// Denoting the status of the sequence. When its value is
        ///     Deleted or Added, some extra decoration will be added
        ///     around the word.
        /// </param>
        /// <returns>
        /// The html text string constructed
        /// </returns>
        private string ConstructText(Sequence seq, SequenceStatus status)
        {
            var result = new StringBuilder();

            switch (status)
            {
                case SequenceStatus.Deleted:

                    // the sequence exists in _original and
                    // will be marked as deleted in the merged
                    // file.
                    for (var i = seq.StartIndex; i < seq.EndIndex; i++)
                    {
                        result.Append(this.original[i].reconstruct(CommentOff.BeginTag, CommentOff.EndTag));
                    }

                    break;
                case SequenceStatus.Inserted:

                    // the sequence exists in _modified and
                    // will be marked as added in the merged
                    // file.
                    for (var i = seq.StartIndex; i < seq.EndIndex; i++)
                    {
                        result.Append(this.modified[i].reconstruct(Added.BeginTag, Added.EndTag));
                    }

                    break;
                case SequenceStatus.NoChange:

                    // the sequence exists in both _original and
                    // _modified and will be left as what it is in
                    // the merged file. We chose to reconstruct from 
                    // _modified collection
                    for (var i = seq.StartIndex; i < seq.EndIndex; i++)
                    {
                        result.Append(this.modified[i].reconstruct());
                    }

                    break;
                default:

                    // this will not happen (hope)
                    break;
            }

            return result.ToString();
        }

        /// <summary>
        /// The function merges the two sequences and returns the merged
        ///     html text string with deleted(exists in source sequence but
        ///     not in destination sequence) and added(exists in destination
        ///     but not in source) decorated extra html tags defined in class
        ///     commentoff and class added.
        /// </summary>
        /// <param name="src">
        /// The source sequence
        /// </param>
        /// <param name="des">
        /// The DES sequence.
        /// </param>
        /// <returns>
        /// The merged html string
        /// </returns>
        private string DoMerge(Sequence src, Sequence des)
        {
            Sequence s;
            var result = new StringBuilder();
            var tail = string.Empty;

            var y = des.StartIndex;

            // strip off the leading common sequence
            while (src.StartIndex < src.EndIndex && des.StartIndex < des.EndIndex &&
                   this.original[src.StartIndex].CompareTo(this.modified[des.StartIndex]) == 0)
            {
                src.StartIndex++;
                des.StartIndex++;
            }

            if (des.StartIndex > y)
            {
                s = new Sequence(y, des.StartIndex);
                result.Append(this.ConstructText(s, SequenceStatus.NoChange));
            }

            y = des.EndIndex;

            // strip off the tailing common sequence
            while (src.StartIndex < src.EndIndex && des.StartIndex < des.EndIndex &&
                   this.original[src.EndIndex - 1].CompareTo(this.modified[des.EndIndex - 1]) == 0)
            {
                src.EndIndex--;
                des.EndIndex--;
            }

            if (des.EndIndex < y)
            {
                s = new Sequence(des.EndIndex, y);
                tail = this.ConstructText(s, SequenceStatus.NoChange);
            }

            // length of the sequences
            var n = src.EndIndex - src.StartIndex;
            var m = des.EndIndex - des.StartIndex;

            // Special cases
            if (n < 1 && m < 1)
            {
                // both source and destination are 
                // empty
                return result.Append(tail).ToString();
            }

            if (n < 1)
            {
                // source is already empty, report
                // destination as added
                result.Append(this.ConstructText(des, SequenceStatus.Inserted));
                result.Append(tail);
                return result.ToString();
            }

            if (m < 1)
            {
                // destination is empty, report source as
                // deleted
                result.Append(this.ConstructText(src, SequenceStatus.Deleted));
                result.Append(tail);
                return result.ToString();
            }
            
            if (m == 1 && n == 1)
            {
                // each of source and destination has only 
                // one word left. At this point, we are sure
                // that they are not equal.
                result.Append(this.ConstructText(src, SequenceStatus.Deleted));
                result.Append(this.ConstructText(des, SequenceStatus.Inserted));
                result.Append(tail);
                return result.ToString();
            }
            
            // find the middle snake
            MiddleSnake snake = this.FindMiddleSnake(src, des);

            if (snake.SES_Length > 1)
            {
                // prepare the parameters for recursion
                var leftSrc = new Sequence(src.StartIndex, snake.Source.StartIndex);
                var leftDes = new Sequence(des.StartIndex, snake.Destination.StartIndex);
                var rightSrc = new Sequence(snake.Source.EndIndex, src.EndIndex);
                var rightDes = new Sequence(snake.Destination.EndIndex, des.EndIndex);

                result.Append(this.DoMerge(leftSrc, leftDes));
                if (snake.Source.StartIndex < snake.Source.EndIndex)
                {
                    // the snake is not empty, report it as common 
                    // sequence
                    result.Append(this.ConstructText(snake.Destination, SequenceStatus.NoChange));
                }

                result.Append(this.DoMerge(rightSrc, rightDes));
                result.Append(tail);
                return result.ToString();
            }

            // Separating this case out can at least save one
            // level of recursion.
            // Only one edit edge suggests the 4 possible cases.
            // if N > M, it will be either:
            // -              or    \     
            // \   (case 1)        \   (case 2)
            // \                   -
            // if N < M, it will be either:
            // |              or    \     
            // \    (case 3)        \   (case 4)
            // \                    |
            // N and M can't be equal!
            if (n > m)
            {
                if (src.StartIndex != snake.Source.StartIndex)
                {
                    // case 1
                    var leftSrc = new Sequence(src.StartIndex, snake.Source.StartIndex);
                    result.Append(this.ConstructText(leftSrc, SequenceStatus.Deleted));
                    result.Append(this.ConstructText(snake.Destination, SequenceStatus.NoChange));
                }
                else
                {
                    // case 2
                    var rightSrc = new Sequence(snake.Source.StartIndex, src.EndIndex);
                    result.Append(this.ConstructText(rightSrc, SequenceStatus.Deleted));
                    result.Append(this.ConstructText(snake.Destination, SequenceStatus.NoChange));
                }
            }
            else
            {
                if (des.StartIndex != snake.Destination.StartIndex)
                {
                    // case 3
                    var updes = new Sequence(des.StartIndex, snake.Destination.StartIndex);
                    result.Append(this.ConstructText(updes, SequenceStatus.Inserted));
                    result.Append(this.ConstructText(snake.Destination, SequenceStatus.NoChange));
                }
                else
                {
                    // case 4
                    var bottomDes = new Sequence(snake.Destination.EndIndex, des.EndIndex);
                    result.Append(this.ConstructText(bottomDes, SequenceStatus.Inserted));
                    result.Append(this.ConstructText(snake.Destination, SequenceStatus.NoChange));
                }
            }

            result.Append(tail);
            return result.ToString();
        }

        /// <summary>
        /// In the edit graph for the sequences src and des, search for the
        ///     optimal(shortest) path from (src.StartIndex, des.StartIndex) to
        ///     (src.EndIndex, des.EndIndex).
        ///     The searching starts from both ends of the graph and when the
        ///     furthest forward reaching overlaps with the furthest backward
        ///     reaching, the overlapped point is reported as the middle point
        ///     of the shortest path.
        ///     See the listed reference for the detailed description of the
        ///     algorithm
        /// </summary>
        /// <param name="src">
        /// Represents a (sub)sequence of _original
        /// </param>
        /// <param name="des">
        /// The DES sequence.
        /// </param>
        /// <returns>
        /// The found middle snake
        /// </returns>
        private MiddleSnake FindMiddleSnake(Sequence src, Sequence des)
        {
            int d;
            var midSnake = new MiddleSnake();

            // the range of diagonal values
            var minDiag = src.StartIndex - des.EndIndex;
            var maxDiag = src.EndIndex - des.StartIndex;

            // middle point of forward searching
            var fwdMid = src.StartIndex - des.StartIndex;

            // middle point of backward searching
            var bwdMid = src.EndIndex - des.EndIndex;

            // forward seaching range 
            var fwdMin = fwdMid;
            var fwdMax = fwdMid;

            // backward seaching range 
            var bwdMin = bwdMid;
            var bwdMax = bwdMid;

            var odd = ((fwdMin - bwdMid) & 1) == 1;

            this.fwdVector[fwdMid] = src.StartIndex;
            this.bwdVector[bwdMid] = src.EndIndex;

#if (MyDEBUG)
            Debug.WriteLine("-- Entering Function findMiddleSnake(src, des) --");
#endif
            for (d = 1;; d++)
            {
                // extend or shrink the search range
                if (fwdMin > minDiag)
                {
                    this.fwdVector[--fwdMin - 1] = -1;
                }
                else
                {
                    ++fwdMin;
                }

                if (fwdMax < maxDiag)
                {
                    this.fwdVector[++fwdMax + 1] = -1;
                }
                else
                {
                    --fwdMax;
                }

#if (MyDEBUG)
                Debug.WriteLine(d, "  D path");
#endif

                // top-down search
                int k;
                int x;
                int y;
                for (k = fwdMax; k >= fwdMin; k -= 2)
                {
                    if (this.fwdVector[k - 1] < this.fwdVector[k + 1])
                    {
                        x = this.fwdVector[k + 1];
                    }
                    else
                    {
                        x = this.fwdVector[k - 1] + 1;
                    }

                    y = x - k;
                    midSnake.Source.StartIndex = x;
                    midSnake.Destination.StartIndex = y;

                    while (x < src.EndIndex && y < des.EndIndex && this.original[x].CompareTo(this.modified[y]) == 0)
                    {
                        x++;
                        y++;
                    }

                    // update forward vector
                    this.fwdVector[k] = x;
#if (MyDEBUG)
                    Debug.WriteLine("    Inside forward loop");
                    Debug.WriteLine(k, "    Diagonal value");
                    Debug.WriteLine(x, "    X value");
                    Debug.WriteLine(y, "    Y value");
#endif
                    if (odd && k >= bwdMin && k <= bwdMax && x >= this.bwdVector[k])
                    {
                        // this is the snake we are looking for
                        // and set the end indeses of the snake 
                        midSnake.Source.EndIndex = x;
                        midSnake.Destination.EndIndex = y;
                        midSnake.SES_Length = (2 * d) - 1;
#if (MyDEBUG)
                        Debug.WriteLine("!!!Report snake from forward search");
                        Debug.WriteLine(midSnake.Source.StartIndex, "  middle snake source start index");
                        Debug.WriteLine(midSnake.Source.EndIndex, "  middle snake source end index");
                        Debug.WriteLine(midSnake.Destination.StartIndex, "  middle snake destination start index");
                        Debug.WriteLine(midSnake.Destination.EndIndex, "  middle snake destination end index");
#endif
                        return midSnake;
                    }
                }

                // extend the search range
                if (bwdMin > minDiag)
                {
                    this.bwdVector[--bwdMin - 1] = int.MaxValue;
                }
                else
                {
                    ++bwdMin;
                }

                if (bwdMax < maxDiag)
                {
                    this.bwdVector[++bwdMax + 1] = int.MaxValue;
                }
                else
                {
                    --bwdMax;
                }

                // bottom-up search
                for (k = bwdMax; k >= bwdMin; k -= 2)
                {
                    if (this.bwdVector[k - 1] < this.bwdVector[k + 1])
                    {
                        x = this.bwdVector[k - 1];
                    }
                    else
                    {
                        x = this.bwdVector[k + 1] - 1;
                    }

                    y = x - k;
                    midSnake.Source.EndIndex = x;
                    midSnake.Destination.EndIndex = y;

                    while (x > src.StartIndex && y > des.StartIndex &&
                           this.original[x - 1].CompareTo(this.modified[y - 1]) == 0)
                    {
                        x--;
                        y--;
                    }

                    // update backward Vector
                    this.bwdVector[k] = x;

#if (MyDEBUG)
                    Debug.WriteLine("     Inside backward loop");
                    Debug.WriteLine(k, "    Diagonal value");
                    Debug.WriteLine(x, "    X value");
                    Debug.WriteLine(y, "    Y value");
#endif
                    if (!odd && k >= fwdMin && k <= fwdMax && x <= this.fwdVector[k])
                    {
                        // this is the snake we are looking for
                        // and set the start indexes of the snake 
                        midSnake.Source.StartIndex = x;
                        midSnake.Destination.StartIndex = y;
                        midSnake.SES_Length = 2 * d;
#if (MyDEBUG)
                        Debug.WriteLine("!!!Report snake from backward search");
                        Debug.WriteLine(midSnake.Source.StartIndex, "  middle snake source start index");
                        Debug.WriteLine(midSnake.Source.EndIndex, "  middle snake source end index");
                        Debug.WriteLine(midSnake.Destination.StartIndex, "  middle snake destination start index");
                        Debug.WriteLine(midSnake.Destination.EndIndex, "  middle snake destination end index");
#endif
                        return midSnake;
                    }
                }
            }
        }

        #endregion
    }

    #endregion
}