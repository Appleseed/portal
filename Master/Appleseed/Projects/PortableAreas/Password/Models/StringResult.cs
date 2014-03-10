using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Text;

namespace Password.Models {
    public class StringResult : ViewResult {
        public string Html { get; set; }
        public override void ExecuteResult(ControllerContext context) {
            if (context == null) {
                throw new ArgumentNullException("context");
            }
            if (string.IsNullOrEmpty(this.ViewName)) {
                this.ViewName =
                     context.RouteData.GetRequiredString("action");
            }
            ViewEngineResult result = null;
            if (this.View == null) {
                result = this.FindView(context);
                this.View = result.View;
            }
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            ViewContext viewContext = new ViewContext(
                    context, this.View, this.ViewData, this.TempData, writer);

            this.View.Render(viewContext, writer);
            writer.Flush();
            Html = HttpUtility.HtmlDecode(Encoding.UTF8.GetString(stream.ToArray()));

            if (result != null) {
                result.ViewEngine.ReleaseView(context, this.View);
            }
        }
    }
}