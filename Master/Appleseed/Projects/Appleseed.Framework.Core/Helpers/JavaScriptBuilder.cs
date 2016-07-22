// --------------------------------------------------------------------------------------------------------------------
// <copyright file="JavaScriptBuilder.cs" company="--">
//   Copyright © -- 2010. All Rights Reserved.
// </copyright>
// <summary>
//   Create well formatted or completely unformatted JavaScript
//   code with minimal effort.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appleseed.Framework.Helpers
{
    using System;
    using System.Text;

    /// <summary>
    /// Create well formatted or completely unformatted JavaScript
    ///  code with minimal effort.
    /// </summary>
    /// <remarks>
    /// Ported to Appleseed by mario [mario@hartmann.net]
    ///  Original written by Paul Riley
    ///  [http://www.codeproject.com/aspnet/AspNetControlJs.asp]
    /// </remarks>
    [History("mario@hartmann.net", "2004/05/19", "Ported to Appleseed by mario. Original written by Paul Riley.")]
    public class JavaScriptBuilder
    {
        #region Constants and Fields

        /// <summary>
        /// The format.
        /// </summary>
        private readonly bool format;

        /// <summary>
        /// The string builder.
        /// </summary>
        private readonly StringBuilder sb = new StringBuilder();

        /// <summary>
        /// The open blocks.
        /// </summary>
        private int openBlocks;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="JavaScriptBuilder"/> class. 
        ///   Instantiate a JavaScriptWriter for unformatted code
        /// </summary>
        public JavaScriptBuilder()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="JavaScriptBuilder"/> class. 
        /// Instantiate a JavaScriptWriter with Formatted switch
        /// </summary>
        /// <param name="formatted">
        /// Format Code?
        /// </param>
        public JavaScriptBuilder(bool formatted)
        {
            this.format = formatted;
        }

        #endregion

        #region Properties

        /// <summary>
        ///   Gets or sets the current level of indent
        /// </summary>
        public int Indent { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Add a comment line to the code, where formatting is set
        /// </summary>
        /// <param name="commentText">
        /// Parts of the comment as an array of strings
        /// </param>
        public void AddCommentLine(params string[] commentText)
        {
            if (this.format)
            {
                // Open the line with tab indent
                for (var i = 0; i < this.Indent; i++)
                {
                    this.sb.Append("\t");
                }

                // ... and a comment marker
                this.sb.Append("// ");

                // Append all the parts of the line
                foreach (var part in commentText)
                {
                    this.sb.Append(part);
                }

                // Throw in a new line
                this.sb.Append(Environment.NewLine);
            }
        }

        /// <summary>
        /// Add a line of code
        /// </summary>
        /// <param name="parts">
        /// Parts of the line as array of strings
        /// </param>
        public void AddLine(params string[] parts)
        {
            // Open line with tabs, where formatting is set
            if (this.format)
            {
                for (var i = 0; i < this.Indent; i++)
                {
                    this.sb.Append("\t");
                }
            }

            // Append parts of the line to StringBuilder individually
            // - much more efficient than sb.AppendFormat
            foreach (var part in parts)
            {
                this.sb.Append(part);
            }

            // Append a new line where formatting is set or a space
            // where it isn't
            if (this.format)
            {
                this.sb.Append(Environment.NewLine);
            }
            else if (parts.Length > 0)
            {
                this.sb.Append(" ");
            }
        }

        /// <summary>
        /// Close code block and decrease indent level
        /// </summary>
        public void CloseBlock()
        {
            // Check that there is at least one block open
            if (this.openBlocks < 1)
            {
                throw new InvalidOperationException("JavaScriptBuilder.CloseBlock() called when no blocks open");
            }

            this.Indent--;
            this.openBlocks--;
            this.AddLine("}");
        }

        /// <summary>
        /// Open a code block and increase indent level
        /// </summary>
        public void OpenBlock()
        {
            this.AddLine("{");
            this.Indent++;
            this.openBlocks++;
        }

        /// <summary>
        /// Convert to string (adding script start and end tags)
        /// </summary>
        /// <returns>
        /// Inner script text
        /// </returns>
        public override string ToString()
        {
            // Check that each indent has a matching outdent
            // - if not then there's almost certainly a problem in the JavaScript
            if (this.openBlocks > 0)
            {
                ErrorHandler.Publish(LogLevel.Error, "JavaScriptBuilder: code blocks are still open");
                return null;
            }
            else
            {
                // Add the <script> tags and some comment blocks, so that
                // browsers that don't support scripts will not crash horribly
                return string.Format(
                    "<script language=\"javascript\">{0}<!--{0}{1}// -->{0}</script>", Environment.NewLine, this.sb);
            }
        }

        #endregion
    }
}