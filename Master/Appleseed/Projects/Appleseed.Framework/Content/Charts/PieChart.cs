using System;
using System.Collections;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Appleseed.Content.Charts
{
    //*********************************************************************
    //
    // PieChart Class
    //
    // This class uses GDI+ to render Pie Chart.
    //
    //*********************************************************************

    /// <summary>
    /// One of the Graphing classes from the reporting asp.net starter kit
    /// on www.asp.net - http://www.asp.net/Default.aspx?tabindex=9&amp;tabID=47
    /// Made very minor changes to the code to use with monitoring module.
    /// Imported by Paul Yarrow, paul@paulyarrow.com
    /// </summary>
    public class PieChart : Chart
    {
        private const int _bufferSpace = 125;
        private ArrayList _chartItems;
        private int _perimeter;
        private Color _backgroundColor;
        private Color _borderColor;
        private float _total;
        private int _legendWidth;
        private int _legendHeight;
        private int _legendFontHeight;
        private string _legendFontStyle;
        private float _legendFontSize;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:PieChart"/> class.
        /// </summary>
        public PieChart()
        {
            _chartItems = new ArrayList();
            _perimeter = 250;
            _backgroundColor = Color.White;
            _borderColor = Color.FromArgb(63, 63, 63);
            _legendFontSize = 8;
            _legendFontStyle = "Verdana";
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:PieChart"/> class.
        /// </summary>
        /// <param name="bgColor">Color of the bg.</param>
        public PieChart(Color bgColor)
        {
            _chartItems = new ArrayList();
            _perimeter = 250;
            _backgroundColor = bgColor;
            _borderColor = Color.FromArgb(63, 63, 63);
            _legendFontSize = 8;
            _legendFontStyle = "Verdana";
        }

        /// <summary>
        /// This method collects all data points and calculate all the necessary dimensions
        /// to draw the chart.  It is the first method called before invoking the Draw() method.
        /// </summary>
        /// <param name="xValues">The x values.</param>
        /// <param name="yValues">The y values.</param>
        public void CollectDataPoints(string[] xValues, string[] yValues)
        {
            _total = 0.0f;

            for (int i = 0; i < xValues.Length; i++)
            {
                float ftemp = Convert.ToSingle(yValues[i]);
                _chartItems.Add(new ChartItem(xValues[i], xValues.ToString(), ftemp, 0, 0, Color.AliceBlue));
                _total += ftemp;
            }

            float nextStartPos = 0.0f;
            int counter = 0;
            foreach (ChartItem item in _chartItems)
            {
                item.StartPos = nextStartPos;
                item.SweepSize = item.Value/_total*360;
                nextStartPos = item.StartPos + item.SweepSize;
                item.ItemColor = GetColor(counter++);
            }

            CalculateLegendWidthHeight();
        }

        /// <summary>
        /// This method returns a bitmap to the calling function.  This is the method
        /// that actually draws the pie chart and the legend with it.
        /// </summary>
        /// <returns></returns>
        public override Bitmap Draw()
        {
            int perimeter = _perimeter;
            Rectangle pieRect = new Rectangle(0, 0, perimeter, perimeter - 1);
            Bitmap bmp = new Bitmap(perimeter + _legendWidth, perimeter);
            Graphics grp = null;
            StringFormat sf = null;

            try
            {
                grp = Graphics.FromImage(bmp);
                sf = new StringFormat();

                //Paint Back ground
                grp.FillRectangle(new SolidBrush(_backgroundColor), 0, 0, perimeter + _legendWidth, perimeter);

                //Align text to the right
                sf.Alignment = StringAlignment.Far;

                //Draw all wedges and legends
                for (int i = 0; i < _chartItems.Count; i++)
                {
                    ChartItem item = (ChartItem) _chartItems[i];
                    SolidBrush brs = null;
                    try
                    {
                        brs = new SolidBrush(item.ItemColor);
                        grp.FillPie(brs, pieRect, item.StartPos, item.SweepSize);
                        grp.FillRectangle(brs, perimeter + _bufferSpace, i*_legendFontHeight + 15, 10, 10);

                        grp.DrawString(item.Label, new Font(_legendFontStyle, _legendFontSize),
                                       new SolidBrush(Color.Black), perimeter + _bufferSpace + 20,
                                       i*_legendFontHeight + 13);

                        grp.DrawString(item.Value.ToString(), new Font(_legendFontStyle, _legendFontSize),
                                       new SolidBrush(Color.Black), perimeter + _bufferSpace + 200,
                                       i*_legendFontHeight + 13, sf);
                    }
                    finally
                    {
                        if (brs != null)
                            brs.Dispose();
                    }
                }

                //draws the border around Pie
                grp.DrawEllipse(new Pen(_borderColor, 2), pieRect);

                //draw border around legend
                grp.DrawRectangle(new Pen(_borderColor, 1), perimeter + _bufferSpace - 10, 10, 220,
                                  _chartItems.Count*_legendFontHeight + 25);

                //Draw Total under legend
                grp.DrawString("Total", new Font(_legendFontStyle, _legendFontSize, FontStyle.Bold),
                               new SolidBrush(Color.Black), perimeter + _bufferSpace + 30,
                               (_chartItems.Count + 1)*_legendFontHeight, sf);
                grp.DrawString(_total.ToString(), new Font(_legendFontStyle, _legendFontSize, FontStyle.Bold),
                               new SolidBrush(Color.Black), perimeter + _bufferSpace + 200,
                               (_chartItems.Count + 1)*_legendFontHeight, sf);

                grp.SmoothingMode = SmoothingMode.AntiAlias;
            }
            finally
            {
                if (sf != null) sf.Dispose();
                if (grp != null) grp.Dispose();
            }
            return bmp;
        }

        /// <summary>
        /// This method calculates the space required to draw the chart legend.
        /// </summary>
        private void CalculateLegendWidthHeight()
        {
            Font fontLegend = new Font(_legendFontStyle, _legendFontSize);
            _legendFontHeight = fontLegend.Height + 5;
            _legendHeight = fontLegend.Height*(_chartItems.Count + 1);
            if (_legendHeight > _perimeter) _perimeter = _legendHeight;

            _legendWidth = _perimeter + _bufferSpace;
        }
    }
}