using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Web.UI;
using Appleseed.Content.Charts;

namespace AppleseedCharts
{
    /// <summary>
    /// One of the Graphing classes from the reporting asp.net starter kit
    /// on www.asp.net - http://www.asp.net/Default.aspx?tabindex=9&amp;tabID=47
    /// Made very minor changes to the code to use with monitoring module.
    /// Imported by Paul Yarrow, paul@paulyarrow.com
    /// </summary>
    public partial class ChartGenerator : System.Web.Mvc.ViewPage
    {
        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="T:System.EventArgs"/> instance containing the event data.</param>
        private void Page_Load(object sender, EventArgs e)
        {
            // This is a quick check for referrer server name against host server
            // For best security practice in a deployed application, an authenticating mechanism should be in placed.
            // Image should only be rendered for authenticated users only.

            // set return type to png image format
            Response.ContentType = "image/png";

            string xValues, yValues, chartType, print;
            bool boolPrint;

            // Get input parameters from query string
            chartType = Request.QueryString["chartType"];
            xValues = Request.QueryString["xValues"];
            yValues = Request.QueryString["yValues"];
            print = Request.QueryString["Print"];

            if (chartType == null)
                chartType = string.Empty;

            // check for printing option 
            if (print == null)
                boolPrint = false;
            else
            {
                try
                {
                    boolPrint = Convert.ToBoolean(print);
                }
                catch
                {
                    boolPrint = false;
                }
            }

            if (xValues != null && yValues != null)
            {
                Color bgColor;

                if (boolPrint)
                    bgColor = Color.White;
                else
                    bgColor = Color.White;

                Bitmap StockBitMap;
                MemoryStream memStream = new MemoryStream();

                switch (chartType)
                {
                    case "bar":
                        BarGraph bar = new BarGraph(bgColor);

                        bar.VerticalLabel = string.Empty;
                        bar.VerticalTickCount = 5;
                        bar.ShowLegend = false;
                        bar.ShowData = false;
                        bar.Height = 400;
                        bar.Width = 700;

                        bar.CollectDataPoints(xValues.Split("|".ToCharArray()), yValues.Split("|".ToCharArray()));
                        StockBitMap = bar.Draw();
                        break;
                    default:
                        PieChart pc = new PieChart(bgColor);

                        pc.CollectDataPoints(xValues.Split("|".ToCharArray()), yValues.Split("|".ToCharArray()));

                        StockBitMap = pc.Draw();

                        break;
                }

                // Render BitMap Stream Back To Client
                StockBitMap.Save(memStream, ImageFormat.Png);
                memStream.WriteTo(Response.OutputStream);
            }
        }

        #region Web Form Designer generated code

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init"></see> event to initialize the page.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"></see> that contains the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(this.Page_Load);
            base.OnInit(e);
        }

        #endregion
    }
}