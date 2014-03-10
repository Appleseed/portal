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

namespace Appleseed.Framework.Web.UI.WebControls
{
    /// <summary>
    /// LanguageSwitcherType lists all variations
    /// </summary>
    public enum LanguageSwitcherType // FXCop rule: Enumerations should use System.Int32 as the underlying type
    {
        /// <summary>
        /// Language Switch appears as a form (drop-down box)
        /// </summary>
        DropDownList = 0,

        /// <summary>
        /// Language Switch appears as a list of links (vertical)
        /// </summary>
        VerticalLinksList = 1,

        /// <summary>
        /// Language Switch appears as a list of links (horizontal)
        /// </summary>
        HorizontalLinksList = 3
    }

    /// <summary>
    /// LanguageSwitcherName lists all possible names
    /// </summary>
    public enum LanguageSwitcherName // FXCop rule: Enumerations should use System.Int32 as the underlying type
    {
        /// <summary>
        /// Language Switch names appear in the specific language.
        /// en-US is shown in Englis, it-It is shown in Italina, zh-CN in Chinese
        /// </summary>
        NativeName = 0,

        /// <summary>
        /// Language Switch names appear in English
        /// </summary>
        EnglishName = 1,

        /// <summary>
        /// Language Switch names appear in the current UI language
        /// </summary>
        DisplayName = 3
    }

    /// <summary>
    /// LanguageSwitcherAction lists all possible ways for changing languages
    /// </summary>
    /// <remarks>
    /// FXCop rule: Enumerations should use System.Int32 as the underlying type
    /// FXCop rule: Only Flag Enumerations Should Have Plural Names
    /// </remarks> 
    public enum LanguageSwitcherAction
    {
        /// <summary>
        /// Language is switched using url.
        /// This is the default and recommended way 
        /// because it is search engine friendly.
        /// </summary>
        LinkRedirect = 0,

        /// <summary>
        /// Language is switched using postbacks.
        /// A LanguageChanged event is thrown.
        /// </summary>
        PostBack = 1,
    }


    /// <summary>
    /// LanguageSwitcherDisplay
    /// </summary>
    public enum LanguageSwitcherDisplay // FXCop rule: Enumerations should use System.Int32 as the underlying type
    {
        /// <summary>
        /// Hide the element
        /// </summary>
        DisplayNone = 0,

        /// <summary>
        /// Display the UI Culture from current list
        /// </summary>
        DisplayUICultureList = 1,

        /// <summary>
        /// Display the Culture from current list
        /// </summary>
        DisplayCultureList = 2
    }
}