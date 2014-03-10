// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ChartItem.cs" company="--">
//   Copyright © -- 2010. All Rights Reserved.
// </copyright>
// <summary>
//   ChartItem Class
//   This class represents a data point in a chart
//   One of the Graphing classes from the reporting asp.net starter kit
//   on www.asp.net - http://www.asp.net/Default.aspx?tabindex=9&amp;tabID=47
//   Made very minor changes to the code to use with monitoring module.
//   Imported by Paul Yarrow, paul@paulyarrow.com
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appleseed.Content.Charts
{
    using System.Drawing;

    /// <summary>
    /// ChartItem Class
    ///   This class represents a data point in a chart
    ///   One of the Graphing classes from the reporting asp.net starter kit
    ///   on www.asp.net - http://www.asp.net/Default.aspx?tabindex=9&amp;tabID=47
    ///   Made very minor changes to the code to use with monitoring module.
    ///   Imported by Paul Yarrow, paul@paulyarrow.com
    /// </summary>
    public class ChartItem
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ChartItem"/> class.
        /// </summary>
        /// <param name="label">
        /// The label.
        /// </param>
        /// <param name="description">
        /// The description.
        /// </param>
        /// <param name="data">
        /// The data value.
        /// </param>
        /// <param name="start">
        /// The start.
        /// </param>
        /// <param name="sweep">
        /// The sweep.
        /// </param>
        /// <param name="itemColor">
        /// The color.
        /// </param>
        public ChartItem(string label, string description, float data, float start, float sweep, Color itemColor)
        {
            this.Label = label;
            this.Description = description;
            this.Value = data;
            this.StartPos = start;
            this.SweepSize = sweep;
            this.ItemColor = itemColor;
        }

        #endregion

        #region Properties

        /// <summary>
        ///   Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        public string Description { get; set; }

        /// <summary>
        ///   Gets or sets the color of the item.
        /// </summary>
        /// <value>The color of the item.</value>
        public Color ItemColor { get; set; }

        /// <summary>
        ///   Gets or sets the label.
        /// </summary>
        /// <value>The label.</value>
        public string Label { get; set; }

        /// <summary>
        ///   Gets or sets the start pos.
        /// </summary>
        /// <value>The start pos.</value>
        public float StartPos { get; set; }

        /// <summary>
        ///   Gets or sets the size of the sweep.
        /// </summary>
        /// <value>The size of the sweep.</value>
        public float SweepSize { get; set; }

        /// <summary>
        ///   Gets or sets the value.
        /// </summary>
        /// <value>The value.</value>
        public float Value { get; set; }

        #endregion
    }
}