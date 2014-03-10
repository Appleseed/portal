// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BarGraph.cs" company="--">
//   Copyright © -- 2010. All Rights Reserved.
// </copyright>
// <summary>
//   BarGraph Class
//   This class uses GDI+ to render Bar Chart.
//   One of the Graphing classes from the reporting asp.net starter kit
//   on www.asp.net - http://www.asp.net/Default.aspx?tabindex=9&amp;tabID=47
//   Made very minor changes to the code to use with monitoring module.
//   Imported by Paul Yarrow, paul@paulyarrow.com
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appleseed.Content.Charts
{
    using System;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Drawing.Imaging;

    /// <summary>
    /// BarGraph Class
    ///   This class uses GDI+ to render Bar Chart.
    ///   One of the Graphing classes from the reporting asp.net starter kit
    ///   on www.asp.net - http://www.asp.net/Default.aspx?tabindex=9&amp;tabID=47
    ///   Made very minor changes to the code to use with monitoring module.
    ///   Imported by Paul Yarrow, paul@paulyarrow.com
    /// </summary>
    public class BarGraph : Chart
    {
        #region Constants and Fields

        /// <summary>
        ///   The graph legend spacer.
        /// </summary>
        private const float GraphLegendSpacer = 15F;

        /// <summary>
        ///   The label font size.
        /// </summary>
        private const int LabelFontSize = 7;

        /// <summary>
        ///   The legend font size.
        /// </summary>
        private const int LegendFontSize = 9;

        /// <summary>
        ///   The legend rectangle size.
        /// </summary>
        private const float LegendRectangleSize = 10F;

        /// <summary>
        ///   The spacer.
        /// </summary>
        private const float Spacer = 5F;

        /// <summary>
        ///   The bar width.
        /// </summary>
        private float barWidth;

        /// <summary>
        ///   The bottom buffer.
        ///   Space from bottom to x axis
        /// </summary>
        private float bottomBuffer;

        /// <summary>
        ///   The graph height.
        /// </summary>
        private float graphHeight;

        /// <summary>
        ///   The graph width.
        /// </summary>
        private float graphWidth;

        /// <summary>
        ///   The legend width.
        /// </summary>
        private float legendWidth;

        /*
        /// <summary>
        /// The longest label.
        /// Used to calculate legend width
        /// </summary>
        private string longestLabel = string.Empty;
        */

        /// <summary>
        ///   The longest tick value.
        ///   Used to calculate max value width
        /// </summary>
        private string longestTickValue = string.Empty;

        /// <summary>
        ///   The max label width.
        /// </summary>
        private float maxLabelWidth;

        /// <summary>
        ///   The max tick value width.
        ///   Used to calculate left offset of bar graph
        /// </summary>
        private float maxTickValueWidth;

        /// <summary>
        ///   The max value.
        ///   = final tick value * tick count
        /// </summary>
        private float maxValue;

        /// <summary>
        ///   The x origin.
        ///   x position where graph starts drawing
        /// </summary>
        private float originX;

        /// <summary>
        ///   The y origin.
        ///   y position where graph starts drawing
        /// </summary>
        private float originY;

        /// <summary>
        ///   The scale factor.
        ///   = _maxValue / _graphHeight
        /// </summary>
        private float scaleFactor;

        /// <summary>
        ///   The space btw bars.
        ///   For now same as _barWidth
        /// </summary>
        private float spaceBtwBars;

        /// <summary>
        ///   Value for each tick = _maxValue/_yTickCount
        /// </summary>
        private float tickValueY;

        /// <summary>
        ///   The top buffer.
        ///   Space from top to the top of y axis
        /// </summary>
        private float topBuffer;

        /// <summary>
        ///   The total height.
        /// </summary>
        private float totalHeight;

        /// <summary>
        ///   The total width.
        /// </summary>
        private float totalWidth;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref = "BarGraph" /> class.
        /// </summary>
        public BarGraph()
        {
            this.AssignDefaultSettings();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BarGraph"/> class.
        /// </summary>
        /// <param name="backgroundColor">
        /// Color of the background.
        /// </param>
        public BarGraph(Color backgroundColor)
        {
            this.AssignDefaultSettings();
            this.BackgroundColor = backgroundColor;
        }

        #endregion

        // Legend related members
        #region Properties

        /// <summary>
        ///   Gets or sets the color of the background.
        /// </summary>
        /// <value>The color of the background.</value>
        public Color BackgroundColor { private get; set; }

        /// <summary>
        ///   Sets the bottom buffer.
        /// </summary>
        /// <value>The bottom buffer.</value>
        public int BottomBuffer
        {
            set
            {
                this.bottomBuffer = Convert.ToSingle(value);
            }
        }

        /// <summary>
        ///   Gets or sets the color of the font.
        /// </summary>
        /// <value>The color of the font.</value>
        public Color FontColor { private get; set; }

        /// <summary>
        ///   Gets or sets the font family.
        /// </summary>
        /// <value>The font family.</value>
        public string FontFamily { get; set; }

        /// <summary>
        ///   Gets or sets the height.
        /// </summary>
        /// <value>The height.</value>
        public int Height
        {
            get
            {
                return Convert.ToInt32(this.totalHeight);
            }

            set
            {
                this.totalHeight = Convert.ToSingle(value);
            }
        }

        /// <summary>
        ///   Gets or sets a value indicating whether [show data].
        /// </summary>
        /// <value><c>true</c> if [show data]; otherwise, <c>false</c>.</value>
        public bool ShowData { get; set; }

        /// <summary>
        ///   Gets or sets a value indicating whether [show legend].
        /// </summary>
        /// <value><c>true</c> if [show legend]; otherwise, <c>false</c>.</value>
        public bool ShowLegend { get; set; }

        /// <summary>
        ///   Sets the top buffer.
        /// </summary>
        /// <value>The top buffer.</value>
        public int TopBuffer
        {
            set
            {
                this.topBuffer = Convert.ToSingle(value);
            }
        }

        /// <summary>
        ///   Gets or sets the vertical label.
        /// </summary>
        /// <value>The vertical label.</value>
        public string VerticalLabel { get; set; }

        /// <summary>
        ///   Gets or sets the vertical tick count.
        /// </summary>
        /// <value>The vertical tick count.</value>
        public int VerticalTickCount { get; set; }

        /// <summary>
        ///   Gets or sets the width.
        /// </summary>
        /// <value>The width.</value>
        public int Width
        {
            get
            {
                return Convert.ToInt32(this.totalWidth);
            }

            set
            {
                this.totalWidth = Convert.ToSingle(value);
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// This method collects all data points and calculate all the necessary dimensions 
        ///   to draw the bar graph.  It is the method called before invoking the Draw() method.
        /// </summary>
        /// <param name="labels">
        /// labels is the x values.
        /// </param>
        /// <param name="values">
        /// values is the y values.
        /// </param>
        public void CollectDataPoints(string[] labels, string[] values)
        {
            if (labels.Length != values.Length)
            {
                throw new Exception("X data count is different from Y data count");
            }

            for (var i = 0; i < labels.Length; i++)
            {
                var temp = Convert.ToSingle(values[i]);
                var shortLbl = MakeShortLabel(labels[i]);

                // For now put 0.0 for start position and sweep size
                this.DataPoints.Add(new ChartItem(shortLbl, labels[i], temp, 0.0f, 0.0f, this.GetColor(i)));

                // Find max value from data; this is only temporary _maxValue
                if (this.maxValue < temp)
                {
                    this.maxValue = temp;
                }

                // Find the longest description
                if (this.ShowLegend)
                {
                    var currentLbl = string.Format("{0} ({1})", labels[i], shortLbl);
                    var currentWidth = CalculateImgFontWidth(currentLbl, LegendFontSize, this.FontFamily);
                    if (this.maxLabelWidth < currentWidth)
                    {
                        // this.longestLabel = currentLbl;
                        this.maxLabelWidth = currentWidth;
                    }
                }
            }

            this.CalculateTickAndMax();
            this.CalculateGraphDimension();
            this.CalculateBarWidth(this.DataPoints.Count, this.graphWidth);
            this.CalculateSweepValues();
        }

        /// <summary>
        /// Collects the data points.
        /// </summary>
        /// <param name="values">
        /// The values.
        /// </param>
        /// <remarks>
        /// Same as above; called when user doesn't care about the x values
        /// </remarks>
        public void CollectDataPoints(string[] values)
        {
            var labels = values;
            this.CollectDataPoints(labels, values);
        }

        /// <summary>
        /// Draws this instance.
        /// </summary>
        /// <returns>
        /// This method returns a bar graph bitmap to the calling function.  It is called after 
        ///   all dimensions and data points are calculated.
        /// </returns>
        public override Bitmap Draw()
        {
            var height = Convert.ToInt32(this.totalHeight);
            var width = Convert.ToInt32(this.totalWidth);

            var bmp = new Bitmap(width, height);

            using (var graph = Graphics.FromImage(bmp))
            {
                graph.CompositingQuality = CompositingQuality.HighQuality;
                graph.SmoothingMode = SmoothingMode.AntiAlias;

                // Set the background: need to draw one pixel larger than the bitmap to cover all area
                graph.FillRectangle(new SolidBrush(this.BackgroundColor), -1, -1, bmp.Width + 1, bmp.Height + 1);

                this.DrawVerticalLabelArea(graph);
                this.DrawBars(graph);
                this.DrawXLabelArea(graph);
                if (this.ShowLegend)
                {
                    this.DrawLegend(graph);
                }
            }

            return bmp;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Calculates the width of the image font.
        /// </summary>
        /// <param name="text">
        /// The text of the image.
        /// </param>
        /// <param name="size">
        /// The size of the font.
        /// </param>
        /// <param name="family">
        /// The font family.
        /// </param>
        /// <returns>
        /// The calculate image font width.
        /// </returns>
        private static float CalculateImgFontWidth(string text, int size, string family)
        {
            Bitmap bmp = null;
            Graphics graph = null;
            Font font = null;

            try
            {
                font = new Font(family, size);

                // Calculate the size of the string.
                bmp = new Bitmap(1, 1, PixelFormat.Format32bppArgb);
                graph = Graphics.FromImage(bmp);
                var measureString = graph.MeasureString(text, font);

                return measureString.Width;
            }
            finally
            {
                if (graph != null)
                {
                    graph.Dispose();
                }

                if (bmp != null)
                {
                    bmp.Dispose();
                }

                if (font != null)
                {
                    font.Dispose();
                }
            }
        }

        /// <summary>
        /// This method creates abbreviation from long description; used for making legend
        /// </summary>
        /// <param name="text">
        /// The text of the label.
        /// </param>
        /// <returns>
        /// The short label.
        /// </returns>
        private static string MakeShortLabel(string text)
        {
            var label = text;
            if (text.Length > 2)
            {
                var midPostition = Convert.ToInt32(Math.Floor((double)text.Length / 2));
                label = text.Substring(0, 1) + text.Substring(midPostition, 1) + text.Substring(text.Length - 1, 1);
            }

            return label;
        }

        /// <summary>
        /// Assigns the default settings.
        /// </summary>
        private void AssignDefaultSettings()
        {
            // default values
            this.totalWidth = 700f;
            this.totalHeight = 450f;
            this.FontFamily = "Verdana";
            this.BackgroundColor = Color.White;
            this.FontColor = Color.Black;
            this.topBuffer = 30f;
            this.bottomBuffer = 30f;
            this.VerticalTickCount = 2;
            this.ShowLegend = false;
            this.ShowData = false;
        }

        /// <summary>
        /// Calculates the width of the bar.
        /// </summary>
        /// <param name="dataCount">
        /// The data count.
        /// </param>
        /// <param name="barGraphWidth">
        /// Width of the bar graph.
        /// </param>
        private void CalculateBarWidth(int dataCount, float barGraphWidth)
        {
            // White space between each bar is the same as bar width itself
            this.barWidth = barGraphWidth / (dataCount * 2); // Each bar has 1 white space 
            this.spaceBtwBars = this.barWidth;
        }

        /// <summary>
        /// Calculates the graph dimension.
        /// </summary>
        private void CalculateGraphDimension()
        {
            this.FindLongestTickValue();

            // Need to add another character for spacing; this is not used for drawing, just for calculation
            this.longestTickValue += "0";
            this.maxTickValueWidth = CalculateImgFontWidth(this.longestTickValue, LabelFontSize, this.FontFamily);
            var leftOffset = Spacer + this.maxTickValueWidth;
            float rightOffset;

            if (this.ShowLegend)
            {
                this.legendWidth = Spacer + LegendRectangleSize + Spacer + this.maxLabelWidth + Spacer;
                rightOffset = GraphLegendSpacer + this.legendWidth + Spacer;
            }
            else
            {
                // Make graph in the middle
                rightOffset = Spacer;
            }

            this.graphHeight = this.totalHeight - this.topBuffer - this.bottomBuffer;

            // Buffer spaces are used to print labels
            this.graphWidth = this.totalWidth - leftOffset - rightOffset;
            this.originX = leftOffset;
            this.originY = this.topBuffer;

            // Once the correct _maxValue is determined, then calculate _scaleFactor
            this.scaleFactor = this.maxValue / this.graphHeight;
        }

        /// <summary>
        /// Calculates the sweep values.
        /// </summary>
        private void CalculateSweepValues()
        {
            // Called when all values and scale factor are known
            // All values calculated here are relative from (_xOrigin, _yOrigin)
            var i = 0;
            foreach (ChartItem item in this.DataPoints)
            {
                // This implementation does not support negative value
                if (item.Value >= 0)
                {
                    item.SweepSize = item.Value / this.scaleFactor;
                }

                // (_spaceBtwBars/2) makes half white space for the first bar
                item.StartPos = (this.spaceBtwBars / 2) + (i * (this.barWidth + this.spaceBtwBars));
                i++;
            }
        }

        /// <summary>
        /// Calculates the tick and max.
        /// </summary>
        private void CalculateTickAndMax()
        {
            float tempMax;

            // Give graph some head room first about 10% of current max
            this.maxValue *= 1.1f;

            if (this.maxValue != 0.0f)
            {
                // Find a rounded value nearest to the current max value
                // Calculate this max first to give enough space to draw value on each bar
                var exp = Convert.ToDouble(Math.Floor(Math.Log10(this.maxValue)));
                tempMax = Convert.ToSingle(Math.Ceiling(this.maxValue / Math.Pow(10, exp)) * Math.Pow(10, exp));
            }
            else
            {
                tempMax = 1.0f;
            }

            // Once max value is calculated, tick value can be determined; tick value should be a whole number
            this.tickValueY = tempMax / this.VerticalTickCount;
            var expTick = Convert.ToDouble(Math.Floor(Math.Log10(this.tickValueY)));
            this.tickValueY =
                Convert.ToSingle(Math.Ceiling(this.tickValueY / Math.Pow(10, expTick)) * Math.Pow(10, expTick));

            // Re-calculate the max value with the new tick value
            this.maxValue = this.tickValueY * this.VerticalTickCount;
        }

        /// <summary>
        /// This method draws all the bars for the graph.
        /// </summary>
        /// <param name="graph">
        /// The graph.
        /// </param>
        private void DrawBars(Graphics graph)
        {
            SolidBrush brsFont = null;
            Font valFont = null;
            StringFormat stringFormat = null;

            try
            {
                brsFont = new SolidBrush(this.FontColor);
                valFont = new Font(this.FontFamily, LabelFontSize);
                stringFormat = new StringFormat { Alignment = StringAlignment.Center };
                var i = 0;

                // Draw bars and the value above each bar
                foreach (ChartItem item in this.DataPoints)
                {
                    using (var barBrush = new SolidBrush(item.ItemColor))
                    {
                        var itemY = this.originY + this.graphHeight - item.SweepSize;

                        // When drawing, all position is relative to (_xOrigin, _yOrigin)
                        graph.FillRectangle(
                            barBrush, this.originX + item.StartPos, itemY, this.barWidth, item.SweepSize);

                        // Draw data value
                        if (this.ShowData)
                        {
                            var startX = this.originX + (i * (this.barWidth + this.spaceBtwBars));

                            // This draws the value on center of the bar
                            var startY = itemY - 2f - valFont.Height; // Positioned on top of each bar by 2 pixels
                            var recVal = new RectangleF(
                                startX, startY, this.barWidth + this.spaceBtwBars, valFont.Height);
                            graph.DrawString(item.Value.ToString("#,###.##"), valFont, brsFont, recVal, stringFormat);
                        }

                        i++;
                    }
                }
            }
            finally
            {
                if (brsFont != null)
                {
                    brsFont.Dispose();
                }

                if (valFont != null)
                {
                    valFont.Dispose();
                }

                if (stringFormat != null)
                {
                    stringFormat.Dispose();
                }
            }
        }

        /// <summary>
        /// Draws the legend.
        /// </summary>
        /// <param name="graph">
        /// The graph.
        /// </param>
        private void DrawLegend(Graphics graph)
        {
            Font lblFont = null;
            SolidBrush brs = null;
            StringFormat lblFormat = null;
            Pen pen = null;

            try
            {
                lblFont = new Font(this.FontFamily, LegendFontSize);
                brs = new SolidBrush(this.FontColor);
                lblFormat = new StringFormat();
                pen = new Pen(this.FontColor);
                lblFormat.Alignment = StringAlignment.Near;

                // Calculate Legend drawing start point
                var startX = this.originX + this.graphWidth + GraphLegendSpacer;
                var startY = this.originY;

                var colorCodeX = startX + Spacer;
                var legendTextX = colorCodeX + LegendRectangleSize + Spacer;
                var legendHeight = 0.0f;
                for (var i = 0; i < this.DataPoints.Count; i++)
                {
                    var point = this.DataPoints[i];
                    var text = string.Format("{0} ({1})", point.Description, point.Label);
                    var currentY = startY + Spacer + (i * (lblFont.Height + Spacer));
                    legendHeight += lblFont.Height + Spacer;

                    // Draw legend description
                    graph.DrawString(text, lblFont, brs, legendTextX, currentY, lblFormat);

                    // Draw color code
                    graph.FillRectangle(
                        new SolidBrush(this.DataPoints[i].ItemColor), 
                        colorCodeX, 
                        currentY + 3f, 
                        LegendRectangleSize, 
                        LegendRectangleSize);
                }

                // Draw legend border
                graph.DrawRectangle(pen, startX, startY, this.legendWidth, legendHeight + Spacer);
            }
            finally
            {
                if (lblFont != null)
                {
                    lblFont.Dispose();
                }

                if (brs != null)
                {
                    brs.Dispose();
                }

                if (lblFormat != null)
                {
                    lblFormat.Dispose();
                }

                if (pen != null)
                {
                    pen.Dispose();
                }
            }
        }

        /// <summary>
        /// This method draws the y label, tick marks, tick values, and the y axis.
        /// </summary>
        /// <param name="graph">
        /// The graph.
        /// </param>
        private void DrawVerticalLabelArea(Graphics graph)
        {
            Font lblFont = null;
            SolidBrush brs = null;
            StringFormat lblFormat = null;
            Pen pen = null;
            StringFormat stringFormatLabel = null;

            try
            {
                lblFont = new Font(this.FontFamily, LabelFontSize);
                brs = new SolidBrush(this.FontColor);
                lblFormat = new StringFormat();
                pen = new Pen(this.FontColor);
                stringFormatLabel = new StringFormat();

                lblFormat.Alignment = StringAlignment.Near;

                // Draw vertical label at the top of y-axis and place it in the middle top of y-axis
                var recVLabel = new RectangleF(
                    0f, this.originY - (2 * Spacer) - lblFont.Height, this.originX * 2, lblFont.Height);
                stringFormatLabel.Alignment = StringAlignment.Center;
                graph.DrawString(this.VerticalLabel, lblFont, brs, recVLabel, stringFormatLabel);

                // Draw all tick values and tick marks
                for (var i = 0; i < this.VerticalTickCount; i++)
                {
                    // ReSharper disable PossibleLossOfFraction

                    // Position for tick mark
                    var currentY = this.topBuffer + (i * this.tickValueY / this.scaleFactor);

                    // Place label in the middle of tick
                    var labelY = currentY - (lblFont.Height / 2);

                    var lblRec = new RectangleF(Spacer, labelY, this.maxTickValueWidth, lblFont.Height);

                    // Calculate tick value from top to bottom
                    var currentTick = this.maxValue - (i * this.tickValueY);
                    graph.DrawString(currentTick.ToString("#,###.##"), lblFont, brs, lblRec, lblFormat);

                    // Draw tick value  
                    // Draw tick mark
                    graph.DrawLine(pen, this.originX, currentY, this.originX - 4.0f, currentY);

                    // ReSharper restore PossibleLossOfFraction
                }

                // Draw y axis
                graph.DrawLine(pen, this.originX, this.originY, this.originX, this.originY + this.graphHeight);
            }
            finally
            {
                if (lblFont != null)
                {
                    lblFont.Dispose();
                }

                if (brs != null)
                {
                    brs.Dispose();
                }

                if (lblFormat != null)
                {
                    lblFormat.Dispose();
                }

                if (pen != null)
                {
                    pen.Dispose();
                }

                if (stringFormatLabel != null)
                {
                    stringFormatLabel.Dispose();
                }
            }
        }

        /// <summary>
        /// This method draws x axis and all x labels
        /// </summary>
        /// <param name="graph">
        /// The graph.
        /// </param>
        private void DrawXLabelArea(Graphics graph)
        {
            Font lblFont = null;
            SolidBrush brs = null;
            StringFormat lblFormat = null;
            Pen pen = null;

            try
            {
                lblFont = new Font(this.FontFamily, LabelFontSize);
                brs = new SolidBrush(this.FontColor);
                lblFormat = new StringFormat();
                pen = new Pen(this.FontColor);

                lblFormat.Alignment = StringAlignment.Center;

                // Draw x axis
                graph.DrawLine(
                    pen, 
                    this.originX, 
                    this.originY + this.graphHeight, 
                    this.originX + this.graphWidth, 
                    this.originY + this.graphHeight);

                // All x labels are drawn 2 pixels below x-axis
                var currentY = this.originY + this.graphHeight + 2.0f;

                // Fits exactly below the bar
                var labelWidth = this.barWidth + this.spaceBtwBars;
                var i = 0;

                // Draw x labels
                foreach (ChartItem item in this.DataPoints)
                {
                    var currentX = this.originX + (i * labelWidth);
                    var recLbl = new RectangleF(currentX, currentY, labelWidth, lblFont.Height);
                    var lblString = this.ShowLegend ? item.Label : item.Description;

                    // Decide what to show: short or long
                    graph.DrawString(lblString, lblFont, brs, recLbl, lblFormat);
                    i++;
                }
            }
            finally
            {
                if (lblFont != null)
                {
                    lblFont.Dispose();
                }

                if (brs != null)
                {
                    brs.Dispose();
                }

                if (lblFormat != null)
                {
                    lblFormat.Dispose();
                }

                if (pen != null)
                {
                    pen.Dispose();
                }
            }
        }

        // *********************************************************************
        // This method determines where to place the legend box.
        // It draws the legend border, legend description, and legend color code.
        // *********************************************************************

        /// <summary>
        /// This method determines the longest tick value from the given data points.
        ///   The result is needed to calculate the correct graph dimension.
        /// </summary>
        private void FindLongestTickValue()
        {
            float currentTick;
            string tickString;
            for (var i = 0; i < this.VerticalTickCount; i++)
            {
                currentTick = this.maxValue - (i * this.tickValueY);
                tickString = currentTick.ToString("#,###.##");
                if (this.longestTickValue.Length < tickString.Length)
                {
                    this.longestTickValue = tickString;
                }
            }
        }

        #endregion

        // *********************************************************************
        // This method calculates the image width in pixel for a given text
        // *********************************************************************

        // *********************************************************************
        // This method calculates the max value and each tick mark value for the bar graph.
        // *********************************************************************
    }
}