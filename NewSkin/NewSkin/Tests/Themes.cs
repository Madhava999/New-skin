using Microsoft.VisualStudio.TestTools.UnitTesting;
using NewSkin.Util;
using Newtonsoft.Json.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Web;

namespace NewSkin.Tests
{

    [TestClass]
    public class Themes : BaseTest
    {
        private LocatorReader _themes;
        private Random _rand;

        [TestInitialize]
        public void Initialize()
        {
            Browser = Pegasus.LoginCom("selcorp");
            _themes = new LocatorReader("Themes.xml");
            _rand = new Random();
        }

        private string RandomColor()
        {
            return string.Format("#{0:X6}", _rand.Next(0x1000000));
        }

        private void AssertEqual(string a, object b)
        {
            Assert.AreEqual(a.ToLower(), b.ToString().ToLower());
        }

        private IWebElement GetColorInput()
        {
            var inputs = Browser.FindElements(_themes, "ColorInput");
            foreach (var i in inputs)
            {
                if (i.Displayed)
                {
                    return i;
                }
            }

            return null;
        }

        /// <summary>
        ///     Select a dropdown by its label and return the name of the selected value.
        /// </summary>
        /// <param name="label">The label of the dropdown field.</param>
        /// <returns>The text of the selected option.</returns>
        private string RandomDropdown(string label)
        {
            var dropdown = new SelectElement(Browser.FindElement(_themes.Get("FontDropdown", label)));
            var count = dropdown.Options.Count;
            var option = _rand.Next(count - 1) + 1;
            dropdown.SelectByIndex(option);
            return dropdown.Options[option].Text;
        }

        [TestMethod]
        public void ChangeThemes()
        {
            // Go to themes page.
            Browser.ImplicitWait = 10;
            Browser.MouseOver(_themes, "SystemTab")
                .Click(_themes, "Themes")
                .Wait(2)
                .Click(_themes, "EditTheme")
                .Wait(1);

            var colors = new Dictionary<string, string>
            {
                {"LeftMenuNavBar", RandomColor()},
                {"LeftMenuNavBarActive", RandomColor()},
                {"TopHeaderBackground", RandomColor()},
                {"TopActionBarBackground", RandomColor()},
                {"PageBackground", RandomColor()},
                {"ButtonBackground", RandomColor()},
                {"ButtonFont", RandomColor()},
                {"DataListTableColumnBackground", RandomColor()},
                {"DataListTableColumnFont", RandomColor()},
                {"DataListTableRowBackground", RandomColor()},
                {"FormInputBorderActive", RandomColor()},
                {"WidgetHeaderBackground", RandomColor()},
                {"WidgetBodyBackground", RandomColor()},
                {"FormInputBackground", RandomColor()},
                {"UsernameBadgeFont", RandomColor()},
                {"LeftMenuBarFont", RandomColor()},
                {"LeftMenuBarActiveFont", RandomColor()},
                {"NavigationLinksFont", RandomColor()},
                {"BreadcrumbsFontColor", RandomColor()},
                {"TabsFontColor", RandomColor()},
                {"WidgetHeadingFontColor", RandomColor()},
                {"FormSectionFontColor", RandomColor()},
                {"FormLabelFontColor", RandomColor()},
                {"FormInputFontColor", RandomColor()},
                {"ViewLabelFontColor", RandomColor()},
                {"NormalTextFontColor", RandomColor()},

            };

            // Set random colors.
            Browser
                .Click(_themes.Get("ColorDropdown", "Left Menu Navigation Bar Color"))
                .FillFormReplace(GetColorInput(), colors["LeftMenuNavBar"])
                .Click(_themes.Get("ColorDropdown", "Left Menu Navigation Bar Active Color"))
                .FillFormReplace(GetColorInput(), colors["LeftMenuNavBarActive"])

                .Click(_themes.Get("ColorDropdown", "Top Header Background Color"))
                .FillFormReplace(GetColorInput(), colors["TopHeaderBackground"])
                .Click(_themes.Get("ColorDropdown", "Top Action Bar Background Color"))
                .FillFormReplace(GetColorInput(), colors["TopActionBarBackground"])

                .Click(_themes.Get("ColorDropdown", "Page Background Color"))
                .FillFormReplace(GetColorInput(), colors["PageBackground"])
                .Click(_themes.Get("ColorDropdown", "Button Background Color"))
                .FillFormReplace(GetColorInput(), colors["ButtonBackground"])

                .Click(_themes.Get("ColorDropdown", "Button Font Color"))
                .FillFormReplace(GetColorInput(), colors["ButtonFont"])
                .Click(_themes.Get("ColorDropdown", "Data List Table Column Background Color"))
                .FillFormReplace(GetColorInput(), colors["DataListTableColumnBackground"])

                .Click(_themes.Get("ColorDropdown", "Data List Table Column Font Color"))
                .FillFormReplace(GetColorInput(), colors["DataListTableColumnFont"])
                .Click(_themes.Get("ColorDropdown", "Data List Table Row Background Color"))
                .FillFormReplace(GetColorInput(), colors["DataListTableRowBackground"])

                 .Click(_themes.Get("ColorDropdown", "Form Input Border Active Color"))
                .FillFormReplace(GetColorInput(), colors["FormInputBorderActive"])
                .Click(_themes.Get("ColorDropdown", "Widget Header Background Color"))
                .FillFormReplace(GetColorInput(), colors["WidgetHeaderBackground"])

                .Click(_themes.Get("ColorDropdown", "Widget Body Background Color"))
                .FillFormReplace(GetColorInput(), colors["WidgetBodyBackground"])
                .Click(_themes.Get("ColorDropdown", "Form Input Background Color"))
                .FillFormReplace(GetColorInput(), colors["FormInputBackground"])
                .Click(_themes.Get("ColorDropdown", "User Name Badge font Color "))
                .FillFormReplace(GetColorInput(), colors["UsernameBadgeFont"])

                .Click(_themes.Get("ColorDropdown", "Left Menu Bar Font Color"))
                .FillFormReplace(GetColorInput(), colors["LeftMenuBarFont"])
                .Click(_themes.Get("ColorDropdown", "Left Menu Bar Active Font Color"))
                .FillFormReplace(GetColorInput(), colors["LeftMenuBarActiveFont"])
                .Click(_themes.Get("ColorDropdown", "Navigation Links Font Color"))
                .FillFormReplace(GetColorInput(), colors["NavigationLinksFont"])

                .Click(_themes.Get("BreadcrumbsFontColor"))
                .FillFormReplace(GetColorInput(), colors["BreadcrumbsFontColor"])
                .Click(_themes.Get("TabsFontColor"))
                .FillFormReplace(GetColorInput(), colors["TabsFontColor"])
                .Click(_themes.Get("WidgetHeadingFontColor"))
                .FillFormReplace(GetColorInput(), colors["WidgetHeadingFontColor"])

                .Click(_themes.Get("FormSectionFontColor"))
                .FillFormReplace(GetColorInput(), colors["FormSectionFontColor"])
                .Click(_themes.Get("FormLabelFontColor"))
                .FillFormReplace(GetColorInput(), colors["FormLabelFontColor"])
                .Click(_themes.Get("FormInputFontColor"))
                .FillFormReplace(GetColorInput(), colors["FormInputFontColor"])
                .Click(_themes.Get("ViewLabelFontColor"))
                .FillFormReplace(GetColorInput(), colors["ViewLabelFontColor"])
                .Click(_themes.Get("NormalTextFontColor"))
                .FillFormReplace(GetColorInput(), colors["NormalTextFontColor"]);
              


            var fonts = new Dictionary<string, string>
            {
                {"left_menu_font_size", RandomDropdown("Left Menu Bar Font Size")},
                {"left_menu_font_type", RandomDropdown("Left Menu Bar Font Type")},
                {"left_menu_font_weight", RandomDropdown("Left Menu Bar Font Weight")},
                {"left_menu_active_font_size", RandomDropdown("Left Menu Bar Active Font Size")},
                {"left_menu_active_font_type", RandomDropdown("Left Menu Bar Active Font Type")},
                {"left_menu_active_font_weight", RandomDropdown("Left Menu Bar Active Font Weight")},
                {"navigation_links_font_size", RandomDropdown("Navigation Links Font Size")},
                {"navigation_links_font_type", RandomDropdown("Navigation Links Font Type")},
                {"navigation_links_font_weight", RandomDropdown("Navigation Links Font Weight")},

            };

         

            // Click save button.
            Browser.Click(Common, "save-button");

            Assert.AreEqual("Theme Configuration has been updated.",
                Browser.FindElement(Common.Get("flash-message")).Text);
            
            // Get parsed JSON theme from url request.
            var href = Browser.FindElement(_themes.Get("Colors")).GetAttribute("href");
            var themeString = HttpUtility.ParseQueryString(new Uri(href).Query).Get("theme");
            var theme = JObject.Parse(themeString);

            AssertEqual(colors["LeftMenuNavBar"], theme["left_menu_color"]);
            AssertEqual(colors["LeftMenuNavBarActive"], theme["left_menu_active_color"]);
            AssertEqual(colors["TopHeaderBackground"], theme["top_header_color"]);
            AssertEqual(colors["TopActionBarBackground"], theme["top_action_bar_color"]);
            AssertEqual(colors["PageBackground"], theme["page_bg_color"]);
            AssertEqual(colors["ButtonBackground"], theme["button_bg_color"]);
            AssertEqual(colors["ButtonFont"], theme["button_font_color"]);
            AssertEqual(colors["DataListTableColumnBackground"], theme["table_header_bg_color"]);
            AssertEqual(colors["DataListTableColumnFont"], theme["table_header_font_color"]);
            AssertEqual(colors["DataListTableRowBackground"], theme["table_row_bg_color"]);
            AssertEqual(colors["FormInputBorderActive"], theme["input_border_color"]);
            AssertEqual(colors["WidgetHeaderBackground"], theme["widget_header_color"]);
            AssertEqual(colors["WidgetBodyBackground"], theme["widget_body_color"]);
            AssertEqual(colors["FormInputBackground"], theme["form_input_box_color"]);
            AssertEqual(colors["UsernameBadgeFont"], theme["user_name_badge_color"]);
            AssertEqual(colors["LeftMenuBarFont"], theme["left_menu_font_color"]);
            AssertEqual(colors["LeftMenuBarActiveFont"], theme["left_menu_active_font_color"]);
            AssertEqual(colors["NavigationLinksFont"], theme["navigation_links_font_color"]);
            AssertEqual(colors["BreadcrumbsFontColor"], theme["breadcrumbs_font_color"]);
            AssertEqual(colors["TabsFontColor"], theme["tabs_font_color"]);
            AssertEqual(colors["WidgetHeadingFontColor"], theme["widget_heading_font_color"]);
            AssertEqual(colors["FormSectionFontColor"], theme["form_section_font_color"]);
            AssertEqual(colors["FormLabelFontColor"], theme["form_label_font_color"]);
            AssertEqual(colors["FormInputFontColor"], theme["form_input_font_color"]);
            AssertEqual(colors["ViewLabelFontColor"], theme["view_label_font_color"]);
            AssertEqual(colors["NormalTextFontColor"], theme["normal_text_font_color"]);




            // Assert font changed succesfully.
            foreach (var i in fonts)
            {
                if (i.Value.ToLower() == "sans serif")
                {
                    AssertEqual("sans-serif", theme[i.Key]);
                }
                else
                {
                    AssertEqual(i.Value, theme[i.Key]);
                }        
            }
        }



        [TestMethod]
        public void Test()
        {
            var href = Browser.FindElement(_themes.Get("Colors")).GetAttribute("href");
            var themeString = HttpUtility.ParseQueryString(new Uri(href).Query).Get("theme");
            var theme = JObject.Parse(themeString);

         
           
            var test27 = (string)theme["breadcrumbs_font_size"];
            var test28 = (string)theme["breadcrumbs_font_type"];
            var test29 = (string)theme["breadcrumbs_font_weight"];
            
            var test31 = (string)theme["tabs_font_size"];
            var test32 = (string)theme["tabs_font_type"];
            var test33 = (string)theme["tabs_font_weight"];

            var test35 = (string)theme["widget_heading_font_size"];
            var test36 = (string)theme["widget_heading_font_type"];
            var test37 = (string)theme["widget_heading_font_weight"];
     
            var test39 = (string)theme["form_section_font_size"];
            var test40 = (string)theme["form_section_font_type"];
            var test41 = (string)theme["form_section_font_weight"];
          
            var test43 = (string)theme["form_label_font_size"];
            var test44 = (string)theme["form_label_font_type"];
            var test45 = (string)theme["form_label_font_weight"];
  
            var test47 = (string)theme["form_input_font_size"];
            var test48 = (string)theme["form_input_font_type"];
            var test49 = (string)theme["form_input_font_weight"];

            var test51 = (string)theme["view_label_font_size"];
            var test52 = (string)theme["view_label_font_type"];
            var test53 = (string)theme["view_label_font_weight"];

            var test55 = (string)theme["normal_text_font_size"];
            var test56 = (string)theme["normal_text_font_type"];
            var test57 = (string)theme["normal_text_font_weight"];


          
        }
        [TestMethod]
        public void Fonts()
        {
            // Go to themes page.
            Browser.ImplicitWait = 10;
            Browser.MouseOver(_themes, "SystemTab")
                .Click(_themes, "Themes")
                .Wait(2)
                .Click(_themes, "EditTheme")
                .Wait(1);

            Browser.DropdownSelectByText(_themes.Get("BreadcrumbsFS"), "15px");
        }
    }
}