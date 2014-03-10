// Esperantus - The Web translator
// Copyright (C) 2003 Emmanuele De Andreis
//
// This library is free software; you can redistribute it and/or
// modify it under the terms of the GNU Lesser General Public
// License as published by the Free Software Foundation; either
// version 2 of the License, or (at your option) any later version.
//
// This library is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
// Lesser General Public License for more details.
//
// You should have received a copy of the GNU Lesser General Public
// License along with this library; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
//
// Emmanuele De Andreis (manu-dea@hotmail dot it)

using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Web.UI;
using System.Web.UI.Design;

namespace Appleseed.Framework.Web.UI.WebControls
{
	/// <summary>
	/// Designer support for LanguageSwitcher
	/// </summary>
	public class LanguageSwitcherDesigner : ControlDesigner 
	{
		/// <summary>
		/// Component is the instance of the component or control that
		/// this designer object is associated with. This property is 
		/// inherited from System.ComponentModel.ComponentDesigner.
		/// </summary>
		/// <returns></returns>
		public override string GetDesignTimeHtml()
		{
			try
			{
                LanguageSwitcher langSwitcher = (LanguageSwitcher)Component;

				StringWriter sw = new StringWriter(CultureInfo.CurrentUICulture); //IFormatProvider should be passed
				HtmlTextWriter tw = new HtmlTextWriter(sw);

				langSwitcher.RenderControl(tw);

				return sw.ToString();
			}
			catch(Exception ex)
			{
				Debug.WriteLine("Exception occurred rendering Language Switcher at design time: ");
				Debug.WriteLine(ex.Message);
				throw;
			}
		}
	}
}
