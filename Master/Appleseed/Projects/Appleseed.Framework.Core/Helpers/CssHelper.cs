// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CssHelper.cs" company="--">
//   Copyright © -- 2010. All Rights Reserved.
// </copyright>
// <summary>
//   CssHelper object (Jes1111)
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appleseed.Framework.Helpers
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Web;

    using Appleseed.Framework.Site.Configuration;

    /// <summary>
    /// CssHelper object (Jes1111)
    /// </summary>
    public class CssHelper
    {
        #region Constants and Fields

        /// <summary>
        /// The allow imports.
        /// </summary>
        private const bool AllowImports = true;

        /// <summary>
        /// The include comments.
        /// </summary>
        private const bool IncludeComments = true;

        /// <summary>
        /// The parse imports.
        /// </summary>
        private const bool ParseImports = true;

        /// <summary>
        /// The selector prefix.
        /// </summary>
        private const string SelectorPrefix = "";

        /// <summary>
        ///   The portal settings.
        /// </summary>
        private PortalSettings PortalSettings;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref = "CssHelper" /> class.
        /// </summary>
        public CssHelper()
        {
            if (HttpContext.Current != null)
            {
                this.PortalSettings = (PortalSettings)HttpContext.Current.Items["PortalSettings"];
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Parses the CSS.
        /// </summary>
        /// <param name="cssFileName">
        /// Name of the CSS file.
        /// </param>
        /// <returns>
        /// A string value...
        /// </returns>
        public string ParseCss(string cssFileName)
        {
            return this.ParseCss(cssFileName, SelectorPrefix);
        }

        /// <summary>
        /// Parses the CSS.
        /// </summary>
        /// <param name="cssFileName">
        /// Name of the CSS file.
        /// </param>
        /// <param name="selectorPrefix">
        /// The selector prefix.
        /// </param>
        /// <param name="allowImports">
        /// if set to <c>true</c> [allow imports].
        /// </param>
        /// <param name="parseImports">
        /// if set to <c>true</c> [parse imports].
        /// </param>
        /// <param name="includeComments">
        /// if set to <c>true</c> [include comments].
        /// </param>
        /// <returns>
        /// A string value...
        /// </returns>
        public string ParseCss(
            string cssFileName, string selectorPrefix, bool allowImports, bool parseImports, bool includeComments)
        {
            return this.ParseCss(cssFileName, selectorPrefix, allowImports, parseImports, includeComments, null);
        }

        /// <summary>
        /// Parses the CSS.
        /// </summary>
        /// <param name="cssFileName">
        /// Name of the CSS file.
        /// </param>
        /// <param name="selectorPrefix">
        /// The selector prefix.
        /// </param>
        /// <param name="allowImports">
        /// if set to <c>true</c> [allow imports].
        /// </param>
        /// <param name="parseImports">
        /// if set to <c>true</c> [parse imports].
        /// </param>
        /// <param name="includeComments">
        /// if set to <c>true</c> [include comments].
        /// </param>
        /// <param name="sb">
        /// The sb.
        /// </param>
        /// <returns>
        /// A string value...
        /// </returns>
        public string ParseCss(
            string cssFileName, 
            string selectorPrefix, 
            bool allowImports, 
            bool parseImports, 
            bool includeComments, 
            StringBuilder sb)
        {
            using (var sr = new StreamReader(cssFileName))
            {
                var st = new StringTokenizer(sr);

                if (sb == null)
                {
                    sb = new StringBuilder();
                }

                Token token;

                try
                {
                    do
                    {
                        token = st.Next();

                        switch (token.Kind)
                        {
                            case TokenKind.Comment:

                                if (includeComments)
                                {
                                    sb.Append(token.Value);

                                    // sb.Append("\n");
                                }

                                break;

                            case TokenKind.Selector:

                                if (selectorPrefix == string.Empty)
                                {
                                    sb.Append(token.Value);
                                }
                                else
                                {
                                    sb.Append(selectorPrefix);
                                    sb.Append(" ");
                                    sb.Append(token.Value);
                                }

                                break;

                            case TokenKind.AtRule:

                            case TokenKind.Block:
                                sb.Append(token.Value);
                                break;

                            case TokenKind.ImportRule:

                                if (allowImports && parseImports)
                                {
                                    // temp
                                    // sb.Append(token.Value);
                                    var filename = token.Value.Replace("@import", string.Empty);
                                    filename = filename.Replace("url", string.Empty);
                                    filename = filename.Replace("(", string.Empty);
                                    filename = filename.Replace(")", string.Empty);
                                    filename = filename.Replace("'", string.Empty);
                                    filename = filename.Replace("\"", string.Empty);
                                    filename = filename.Replace(";", string.Empty).Trim();
                                    filename =
                                        string.Concat(
                                            cssFileName.Substring(0, cssFileName.LastIndexOf(@"\")).Trim(), 
                                            "\\", 
                                            filename);
                                    var loop = new CssHelper();
                                    loop.ParseCss(
                                        filename, selectorPrefix, allowImports, parseImports, includeComments, sb);
                                }
                                else if (allowImports && !parseImports)
                                {
                                    sb.Append(token.Value);
                                }

                                break;
                            default:
                                sb.Append(token.Value);
                                break;
                        }
                    }
                    while (token.Kind != TokenKind.EOF);
                }
                catch (Exception ex)
                {
                    ErrorHandler.Publish(
                        LogLevel.Error, 
                        string.Format("Error in parsing CSS file: {0} Message was: {1}", cssFileName, ex.Message));
                }
                finally
                {
                }

                return sb.ToString();
            }
        }

        /// <summary>
        /// Parses the CSS.
        /// </summary>
        /// <param name="cssFileName">
        /// Name of the CSS file.
        /// </param>
        /// <param name="selectorPrefix">
        /// The selector prefix.
        /// </param>
        /// <param name="allowImports">
        /// if set to <c>true</c> [allow imports].
        /// </param>
        /// <param name="parseImports">
        /// if set to <c>true</c> [parse imports].
        /// </param>
        /// <returns>
        /// A string value...
        /// </returns>
        public string ParseCss(string cssFileName, string selectorPrefix, bool allowImports, bool parseImports)
        {
            return this.ParseCss(cssFileName, selectorPrefix, allowImports, parseImports, IncludeComments);
        }

        /// <summary>
        /// Parses the CSS.
        /// </summary>
        /// <param name="cssFileName">
        /// Name of the CSS file.
        /// </param>
        /// <param name="selectorPrefix">
        /// The selector prefix.
        /// </param>
        /// <param name="allowImports">
        /// if set to <c>true</c> [allow imports].
        /// </param>
        /// <returns>
        /// A string value...
        /// </returns>
        public string ParseCss(string cssFileName, string selectorPrefix, bool allowImports)
        {
            return this.ParseCss(cssFileName, selectorPrefix, allowImports, ParseImports);
        }

        /// <summary>
        /// Parses the CSS.
        /// </summary>
        /// <param name="cssFileName">
        /// Name of the CSS file.
        /// </param>
        /// <param name="selectorPrefix">
        /// The selector prefix.
        /// </param>
        /// <returns>
        /// A string value...
        /// </returns>
        public string ParseCss(string cssFileName, string selectorPrefix)
        {
            return this.ParseCss(cssFileName, selectorPrefix, AllowImports);
        }

        #endregion
    }

    /// <summary>
    /// StringTokenizer tokenized string (or stream) into tokens.
    /// 
    ///   ********************************************************
    ///   *	Author: Andrew Deren
    ///   *	Date: July, 2004
    ///   *	http://www.adersoftware.com
    ///   * 
    ///   *	StringTokenizer class. You can use this class in any way you want
    ///   * as long as this header remains in this file.
    ///   * 
    ///   **********************************************************
    /// </summary>
    /// <remarks>
    /// modified by Jes1111 to be specific to CSS
    /// </remarks>
    public class StringTokenizer
    {
        #region Constants and Fields

        /// <summary>
        ///   The EOF.
        /// </summary>
        private const char EOF = (char)0;

        /// <summary>
        ///   The data.
        /// </summary>
        private readonly string data;

        /// <summary>
        ///   The column.
        /// </summary>
        private int column;

        /// <summary>
        ///   The ignore white space.
        /// </summary>
        private bool ignoreWhiteSpace;

        /// <summary>
        ///   The line.
        /// </summary>
        private int line;

        /// <summary>
        ///   The pos.
        /// </summary>
        private int pos; // position within data

        /// <summary>
        ///   The save col.
        /// </summary>
        private int saveCol;

        /// <summary>
        ///   The save line.
        /// </summary>
        private int saveLine;

        /// <summary>
        ///   The save pos.
        /// </summary>
        private int savePos;

        /// <summary>
        ///   The symbol chars.
        /// </summary>
        private char[] symbolChars;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="StringTokenizer"/> class.
        /// </summary>
        /// <param name="data">
        /// The data.
        /// </param>
        /// <returns>
        /// A void value...
        /// </returns>
        public StringTokenizer(string data)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }

            this.data = data;
            this.Reset();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StringTokenizer"/> class.
        /// </summary>
        /// <param name="reader">
        /// The reader.
        /// </param>
        /// <returns>
        /// A void value...
        /// </returns>
        public StringTokenizer(StreamReader reader)
        {
            if (reader == null)
            {
                throw new ArgumentNullException("reader");
            }

            this.data = reader.ReadToEnd();
            this.Reset();
        }

        #endregion

        #region Properties

        /// <summary>
        ///   if set to true, white space characters will be ignored,
        ///   but EOL and whitespace inside of string will still be tokenized
        /// </summary>
        /// <value><c>true</c> if [ignore white space]; otherwise, <c>false</c>.</value>
        public bool IgnoreWhiteSpace
        {
            get
            {
                return this.ignoreWhiteSpace;
            }

            set
            {
                this.ignoreWhiteSpace = value;
            }
        }

        /// <summary>
        ///   gets or sets which characters are part of TokenKind.Symbol
        /// </summary>
        public char[] SymbolChars
        {
            get
            {
                return this.symbolChars;
            }

            set
            {
                this.symbolChars = value;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Next this instance.
        /// </summary>
        /// <returns>
        /// A Appleseed.Framework.Helpers.Token value...
        /// </returns>
        public Token Next()
        {
            ReadToken:
            var ch = this.LA(0);

            switch (ch)
            {
                case EOF:
                    return this.CreateToken(TokenKind.EOF, string.Empty);

                case ' ':

                case '\t':
                    {
                        if (this.ignoreWhiteSpace)
                        {
                            this.Consume();
                            goto ReadToken;
                        }

                        return this.ReadWhitespace();
                    }

                case '\r':
                    {
                        this.StartRead();
                        this.Consume();

                        if (this.LA(0) == '\n')
                        {
                            this.Consume(); // on DOS/Windows we have \r\n for new line
                        }

                        this.line++;
                        this.column = 1;
                        return this.CreateToken(TokenKind.EOL);
                    }

                case '\n':
                    {
                        this.StartRead();
                        this.Consume();
                        this.line++;
                        this.column = 1;
                        return this.CreateToken(TokenKind.EOL);
                    }

                case '@':
                    {
                        return this.LA(1) == 'i' || this.LA(1) == 'I' ? this.ReadImportRule() : this.ReadAtRule();
                    }

                case '/':
                    {
                        if (this.LA(1) == '*')
                        {
                            // comment
                            return this.ReadComment();
                        }

                        this.Consume();
                        return this.CreateToken(TokenKind.Symbol);
                    }

                case '{':
                    {
                        // block
                        return this.ReadBlock();
                    }

                default:
                    {
                        // selector
                        return this.ReadSelector();

                        // 					if (Char.IsLetter(ch) || ch == '_')
                        // 						return ReadWord();
                        // 					else if (IsSymbol(ch))
                        // 					{
                        // 						StartRead();
                        // 						Consume();
                        // 						return CreateToken(TokenKind.Symbol);
                        // 					}
                        // 					else
                        // 					{
                        // 						StartRead();
                        // 						Consume();
                        // 						return CreateToken(TokenKind.Unknown);						
                        // 					}
                    }
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Consumes this instance.
        /// </summary>
        /// <returns>
        /// A char value...
        /// </returns>
        protected char Consume()
        {
            var ret = this.data[this.pos];
            this.pos++;
            this.column++;
            return ret;
        }

        /// <summary>
        /// Creates the token.
        /// </summary>
        /// <param name="kind">
        /// The kind.
        /// </param>
        /// <returns>
        /// A Appleseed.Framework.Helpers.Token value...
        /// </returns>
        protected Token CreateToken(TokenKind kind)
        {
            var tokenData = this.data.Substring(this.savePos, this.pos - this.savePos);
            return new Token(kind, tokenData, this.saveLine, this.saveCol);
        }

        /// <summary>
        /// Creates the token.
        /// </summary>
        /// <param name="kind">
        /// The kind.
        /// </param>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <returns>
        /// A Appleseed.Framework.Helpers.Token value...
        /// </returns>
        protected Token CreateToken(TokenKind kind, string value)
        {
            return new Token(kind, value, this.line, this.column);
        }

        /// <summary>
        /// checks whether c is a symbol character.
        /// </summary>
        /// <param name="c">
        /// The c.
        /// </param>
        /// <returns>
        /// <c>true</c> if the specified c is symbol; otherwise, <c>false</c>.
        /// </returns>
        protected bool IsSymbol(char c)
        {
            return this.symbolChars.Any(t => t == c);
        }

        /// <summary>
        /// LAs the specified count.
        /// </summary>
        /// <param name="count">
        /// The count.
        /// </param>
        /// <returns>
        /// A char value...
        /// </returns>
        protected char LA(int count)
        {
            return this.pos + count >= this.data.Length ? EOF : this.data[this.pos + count];
        }

        /// <summary>
        /// Reads at rule.
        /// </summary>
        /// <returns>
        /// A Appleseed.Framework.Helpers.Token value...
        /// </returns>
        protected Token ReadAtRule()
        {
            this.StartRead();
            this.Consume(); // read '@'

            while (true)
            {
                var ch = this.LA(0);

                if (ch == EOF)
                {
                    break;
                }

                if (ch == '\r')
                {
                    // handle CR in strings
                    this.Consume();

                    if (this.LA(0) == '\n')
                    {
                        // for DOS & windows
                        this.Consume();
                    }

                    this.line++;
                    this.column = 1;
                }
                else if (ch == '\n')
                {
                    // new line in quoted string
                    this.Consume();
                    this.line++;
                    this.column = 1;
                }
                else if (ch == ';')
                {
                    // read ';'
                    this.Consume();
                    break;
                }
                else if (ch == '{')
                {
                    break;
                }
                else
                {
                    this.Consume();
                }
            }

            return this.CreateToken(TokenKind.AtRule);
        }

        /// <summary>
        /// Reads the block.
        /// </summary>
        /// <returns>
        /// A Appleseed.Framework.Helpers.Token value...
        /// </returns>
        protected Token ReadBlock()
        {
            this.StartRead();
            this.Consume(); // read '{'

            while (true)
            {
                var ch = this.LA(0);

                if (ch == EOF)
                {
                    break;
                }

                if (ch == '\r')
                {
                    // handle CR in strings
                    this.Consume();

                    if (this.LA(0) == '\n')
                    {
                        // for DOS & windows
                        this.Consume();
                    }

                    this.line++;
                    this.column = 1;
                }
                else if (ch == '\n')
                {
                    // new line in quoted string
                    this.Consume();
                    this.line++;
                    this.column = 1;
                }
                else if (ch == '}')
                {
                    this.Consume();
                    break;
                }
                else
                {
                    this.Consume();
                }
            }

            return this.CreateToken(TokenKind.Block);
        }

        /// <summary>
        /// Reads the comment.
        /// </summary>
        /// <returns>
        /// A Appleseed.Framework.Helpers.Token value...
        /// </returns>
        protected Token ReadComment()
        {
            this.StartRead();
            this.Consume(); // read '/'
            this.Consume(); // read '*'

            while (true)
            {
                var ch = this.LA(0);

                if (ch == EOF)
                {
                    break;
                }

                if (ch == '\r')
                {
                    // handle CR in strings
                    this.Consume();

                    if (this.LA(0) == '\n')
                    {
                        // for DOS & windows
                        this.Consume();
                    }

                    this.line++;
                    this.column = 1;
                }
                else if (ch == '\n')
                {
                    // new line in quoted string
                    this.Consume();
                    this.line++;
                    this.column = 1;
                }
                else if (ch == '*' && this.LA(1) == '/')
                {
                    this.Consume(); // read '*'
                    this.Consume(); // read '/'
                    break;
                }
                else
                {
                    this.Consume();
                }
            }

            return this.CreateToken(TokenKind.Comment);
        }

        /// <summary>
        /// Reads the import rule.
        /// </summary>
        /// <returns>
        /// A Appleseed.Framework.Helpers.Token value...
        /// </returns>
        protected Token ReadImportRule()
        {
            this.StartRead();
            this.Consume(); // read '@'

            while (true)
            {
                var ch = this.LA(0);

                if (ch == EOF)
                {
                    break;
                }

                if (ch == '\r')
                {
                    // handle CR in strings
                    this.Consume();

                    if (this.LA(0) == '\n')
                    {
                        // for DOS & windows
                        this.Consume();
                    }

                    this.line++;
                    this.column = 1;
                }
                else if (ch == '\n')
                {
                    // new line in quoted string
                    this.Consume();
                    this.line++;
                    this.column = 1;
                }
                else if (ch == ';')
                {
                    this.Consume(); // read ';'
                    break;
                }
                else
                {
                    this.Consume();
                }
            }

            return this.CreateToken(TokenKind.ImportRule);
        }

        /// <summary>
        /// reads number. Number is: DIGIT+ ("." DIGIT*)?
        /// </summary>
        /// <returns>
        /// </returns>
        protected Token ReadNumber()
        {
            this.StartRead();
            var hadDot = false;
            this.Consume(); // read first digit

            while (true)
            {
                var ch = this.LA(0);

                if (Char.IsDigit(ch))
                {
                    this.Consume();
                }
                else if (ch == '.' && !hadDot)
                {
                    hadDot = true;
                    this.Consume();
                }
                else
                {
                    break;
                }
            }

            return this.CreateToken(TokenKind.Number);
        }

        /// <summary>
        /// Reads the selector.
        /// </summary>
        /// <returns>
        /// A Appleseed.Framework.Helpers.Token value...
        /// </returns>
        protected Token ReadSelector()
        {
            this.StartRead();

            while (true)
            {
                var ch = this.LA(0);

                if (ch == EOF)
                {
                    return this.CreateToken(TokenKind.Error); // shouldn't encounter this
                }

                if (ch == '\r')
                {
                    // handle CR in strings
                    this.Consume();

                    if (this.LA(0) == '\n')
                    {
                        // for DOS & windows
                        this.Consume();
                    }

                    this.line++;
                    this.column = 1;
                }
                else if (ch == '\n')
                {
                    // new line in quoted string
                    this.Consume();
                    this.line++;
                    this.column = 1;
                }
                else if (ch == ';')
                {
                    this.Consume(); // read ';' - shouldn't encounter this
                    return this.CreateToken(TokenKind.Error);
                }
                else if (ch == '{')
                {
                    break;
                }
                else
                {
                    this.Consume();
                }
            }

            return this.CreateToken(TokenKind.Selector);
        }

        /// <summary>
        /// reads all characters until next " is found.
        ///   If string.Empty (2 quotes) are found, then they are consumed as
        ///   part of the string
        /// </summary>
        /// <returns>
        /// </returns>
        protected Token ReadString()
        {
            this.StartRead();
            this.Consume(); // read "

            while (true)
            {
                var ch = this.LA(0);

                if (ch == EOF)
                {
                    break;
                }

                if (ch == '\r')
                {
                    // handle CR in strings
                    this.Consume();

                    if (this.LA(0) == '\n')
                    {
                        // for DOS & windows
                        this.Consume();
                    }

                    this.line++;
                    this.column = 1;
                }
                else if (ch == '\n')
                {
                    // new line in quoted string
                    this.Consume();
                    this.line++;
                    this.column = 1;
                }
                else if (ch == '"')
                {
                    this.Consume();

                    if (this.LA(0) != '"')
                    {
                        break; // done reading, and this quotes does not have escape character
                    }

                    this.Consume(); // consume second ", because first was just an escape
                }
                else
                {
                    this.Consume();
                }
            }

            return this.CreateToken(TokenKind.QuotedString);
        }

        /// <summary>
        /// reads all whitespace characters (does not include newline)
        /// </summary>
        /// <returns>
        /// </returns>
        protected Token ReadWhitespace()
        {
            this.StartRead();
            this.Consume(); // consume the looked-ahead whitespace char

            while (true)
            {
                var ch = this.LA(0);

                if (ch == '\t' || ch == ' ')
                {
                    this.Consume();
                }
                else
                {
                    break;
                }
            }

            return this.CreateToken(TokenKind.WhiteSpace);
        }

        /// <summary>
        /// reads word. Word contains any alpha character or _
        /// </summary>
        /// <returns>
        /// </returns>
        protected Token ReadWord()
        {
            this.StartRead();
            this.Consume(); // consume first character of the word

            while (true)
            {
                var ch = this.LA(0);

                if (Char.IsLetter(ch) || ch == '_')
                {
                    this.Consume();
                }
                else
                {
                    break;
                }
            }

            return this.CreateToken(TokenKind.Word);
        }

        /// <summary>
        /// Resets this instance.
        /// </summary>
        private void Reset()
        {
            this.ignoreWhiteSpace = false;
            this.symbolChars = new[]
                {
                    '=', '+', '-', '/', ',', '.', '*', '~', '!', '@', '#', '$', '%', '^', '&', '(', ')', '{', '}', '[', 
                    ']', ':', ';', '<', '>', '?', '|', '\\'
                };
            this.line = 1;
            this.column = 1;
            this.pos = 0;
        }

        /// <summary>
        /// save read point positions so that CreateToken can use those
        /// </summary>
        private void StartRead()
        {
            this.saveLine = this.line;
            this.saveCol = this.column;
            this.savePos = this.pos;
        }

        #endregion
    }

    /// <summary>
    /// ********************************************************
    ///   *	Author: Andrew Deren
    ///   *	Date: July, 2004
    ///   *	http://www.adersoftware.com
    ///   * 
    ///   *	StringTokenizer class. You can use this class in any way you want
    ///   * as long as this header remains in this file.
    ///   * 
    ///   **********************************************************
    /// </summary>
    /// <remarks>
    /// modified by Jes1111 to be specific to CSS
    /// </remarks>
    public enum TokenKind
    {
        /// <summary>
        ///   The unknown.
        /// </summary>
        Unknown, 

        /// <summary>
        ///   The word.
        /// </summary>
        Word, 

        /// <summary>
        ///   The number.
        /// </summary>
        Number, 

        /// <summary>
        ///   The quoted string.
        /// </summary>
        QuotedString, 

        /// <summary>
        ///   The white space.
        /// </summary>
        WhiteSpace, 

        /// <summary>
        ///   The symbol.
        /// </summary>
        Symbol, 

        /// <summary>
        ///   The eol.
        /// </summary>
        EOL, 

        /// <summary>
        ///   The eof.
        /// </summary>
        EOF, 

        /// <summary>
        ///   The comment.
        /// </summary>
        Comment, 

        /// <summary>
        ///   The selector.
        /// </summary>
        Selector, 

        /// <summary>
        ///   The block.
        /// </summary>
        Block, 

        /// <summary>
        ///   The at rule.
        /// </summary>
        AtRule, 

        /// <summary>
        ///   The import rule.
        /// </summary>
        ImportRule, 

        /// <summary>
        ///   The error.
        /// </summary>
        Error
    }

    /// <summary>
    /// The token.
    /// </summary>
    /// <remarks>
    /// </remarks>
    public class Token
    {
        #region Constants and Fields

        /// <summary>
        ///   The column.
        /// </summary>
        private readonly int column;

        /// <summary>
        ///   The kind.
        /// </summary>
        private readonly TokenKind kind;

        /// <summary>
        ///   The line.
        /// </summary>
        private readonly int line;

        /// <summary>
        ///   The value.
        /// </summary>
        private readonly string value;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Token"/> class.
        /// </summary>
        /// <param name="kind">
        /// The kind.
        /// </param>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <param name="line">
        /// The line.
        /// </param>
        /// <param name="column">
        /// The column.
        /// </param>
        /// <returns>
        /// A void value...
        /// </returns>
        public Token(TokenKind kind, string value, int line, int column)
        {
            this.kind = kind;
            this.value = value;
            this.line = line;
            this.column = column;
        }

        #endregion

        #region Properties

        /// <summary>
        ///   Gets the column.
        /// </summary>
        /// <value>The column.</value>
        /// <remarks>
        /// </remarks>
        public int Column
        {
            get
            {
                return this.column;
            }
        }

        /// <summary>
        ///   Gets the kind.
        /// </summary>
        /// <value>The kind.</value>
        /// <remarks>
        /// </remarks>
        public TokenKind Kind
        {
            get
            {
                return this.kind;
            }
        }

        /// <summary>
        ///   Gets the line.
        /// </summary>
        /// <value>The line.</value>
        /// <remarks>
        /// </remarks>
        public int Line
        {
            get
            {
                return this.line;
            }
        }

        /// <summary>
        ///   Gets the value.
        /// </summary>
        /// <value>The value.</value>
        /// <remarks>
        /// </remarks>
        public string Value
        {
            get
            {
                return this.value;
            }
        }

        #endregion
    }
}