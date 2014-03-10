// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Chart.cs" company="--">
//   Copyright © -- 2010. All Rights Reserved.
// </copyright>
// <summary>
//   Chart Class
//   Base class implementation for BarChart and PieChart
//   One of the Graphing classes from the reporting asp.net starter kit
//   on www.asp.net - http://www.asp.net/Default.aspx?tabindex=9&amp;tabID=47
//   Made very minor changes to the code to use with monitoring module.
//   Imported by Paul Yarrow, paul@paulyarrow.com
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appleseed.Content.Charts
{
    using System.Collections.ObjectModel;
    using System.Drawing;

    /// <summary>
    /// Chart Class
    ///   Base class implementation for BarChart and PieChart
    ///   One of the Graphing classes from the reporting asp.net starter kit
    ///   on www.asp.net - http://www.asp.net/Default.aspx?tabindex=9&amp;tabID=47
    ///   Made very minor changes to the code to use with monitoring module.
    ///   Imported by Paul Yarrow, paul@paulyarrow.com
    /// </summary>
    public abstract class Chart
    {
        #region Constants and Fields

        /// <summary>
        /// The color limit.
        /// </summary>
        private const int ColorLimit = 25;

        /// <summary>
        /// The color.
        /// </summary>
        private readonly Color[] color = {
                                             Color.ForestGreen, Color.Beige, Color.SlateBlue, Color.Brown, Color.Coral, 
                                             Color.Crimson, Color.DarkCyan, Color.AliceBlue, Color.Gold, Color.Green, 
                                             Color.BlueViolet, Color.HotPink, Color.Orange, Color.RoyalBlue, Color.Navy, 
                                             Color.Olive, Color.Ivory, Color.Orchid, Color.PapayaWhip, Color.Pink, 
                                             Color.Plum, Color.Red, Color.Goldenrod, Color.Salmon, Color.Blue
                                         };

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref = "Chart" /> class.
        /// </summary>
        protected Chart()
        {
            this.DataPoints = new Collection<ChartItem>();
        }

        #endregion

        #region Properties

        /// <summary>
        ///   Gets or sets the data points.
        /// </summary>
        /// <value>The data points.</value>
        public Collection<ChartItem> DataPoints { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Draws this instance.
        /// </summary>
        /// <returns>
        /// A bitmap image.
        /// </returns>
        public abstract Bitmap Draw();

        /// <summary>
        /// Gets the color.
        /// </summary>
        /// <param name="index">
        /// The index.
        /// </param>
        /// <returns>
        /// The color.
        /// </returns>
        public Color GetColor(int index)
        {
            // Changed for cycle. jviladiu@portalServices.net (05/07/2004)
            // if (index < _colorLimit) 
            // {
            // return _color[index];
            // }
            // else
            // {
            // throw new Exception("Color Limit is " + _colorLimit);
            // }
            return this.color[index % ColorLimit];
        }

        /// <summary>
        /// Sets the color.
        /// </summary>
        /// <param name="index">
        /// The index.
        /// </param>
        /// <param name="newColor">
        /// The new color.
        /// </param>
        public void SetColor(int index, Color newColor)
        {
            // Changed for cycle. jviladiu@portalServices.net (05/07/2004)
            // if (index < _colorLimit) 
            // {
            // _color[index] = NewColor;
            // }
            // else
            // {
            // throw new Exception("Color Limit is " + _colorLimit);
            // }
            this.color[index % ColorLimit] = newColor;
        }

        #endregion
    }
}