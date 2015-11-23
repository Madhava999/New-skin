using Microsoft.VisualStudio.TestTools.UnitTesting;
using NewSkin.PageHelper;
using NewSkin.PageHelper.Comm;
using System;
using System.IO;
using System.Threading;
using System.Xml;
   /*
    *This class is incomplete
    *
    */


namespace NewSkin.Tests
{
    [TestClass]
    public class OfficeFieldDictionary : DriverTestCase2
    {
        private ChyHelper chyHelper;
        private string tabSectionName, tabName, sectionFieldName;


        //Reusable method to login with admin credentials
        public void LoginUser(string user, string pass)
        {
            string[] username = null;
            string[] password = null;


            var oXMLData = new XMLParse();
            oXMLData.LoadXML("../../Config/ApplicationSettings.xml");

            username = oXMLData.getData("settings/NewSiteCredentials", user);
            password = oXMLData.getData("settings/NewSiteCredentials", pass);

            var loginHelper = new LoginHelper(GetWebDriver());

            //Login with valid username and password
            Login(username[0], password[0]);
            Console.WriteLine("Logged in as: " + username[0] + " / " + password[0]);

        }
        //Method to set just made CDATA XPath in XML file
        public void SetXPath(string xmlNode, string xPathName)
        {


            XmlDocument xml = new XmlDocument();
            xml.Load("../../Locators/OfficeFieldDictionary.xml");
            //string xmlcontent = xml.InnerXml;
            xml.SelectSingleNode("//" + xmlNode).InnerXml = xPathName;
            //  string xmlcontent2 = xml.InnerXml;
            xml.Save("../../Locators/OfficeFieldDictionary.xml");

        }

        public void GoToEditTabsPage()
        {
            chyHelper.MouseHover("Navigate/UserName");
            chyHelper.ClickElement("Navigate/AdminTab");

            chyHelper.MouseHover("Navigate/FieldDictionaryTab");
            chyHelper.ClickElement("Navigate/TabsTab");
        }
        public void GoToEditTabSectionsPage()
        {
            chyHelper.MouseHover("Navigate/UserName");
            chyHelper.ClickElement("Navigate/AdminTab");

            chyHelper.MouseHover("Navigate/FieldDictionaryTab");
            chyHelper.WaitForWorkAround(1500);
            chyHelper.ClickElement("Navigate/SectionsTab");
        }
        public void MoveFieldBack()
        {
            chyHelper.MouseHover("Navigate/FieldDictionaryTab");
            chyHelper.MouseHover("Navigate/FieldsTab");
            chyHelper.ClickElement("Navigate/FieldPropertiesTab");

            chyHelper.Select("SectionFields/SelectModule", "20");
            chyHelper.ClickElement("SectionFields/SearchButton");
            chyHelper.ClickElement("SectionFields/ChooseCompanyLogo");
            chyHelper.WaitForWorkAround(2000);
            chyHelper.SelectByText("SectionFields/SelectNewSection", "Company Details");
            chyHelper.ClickElement("SectionFields/SaveNewFieldInfo");
        }
        public void CreateTab()
        {
            Random rand = new Random();
            int nameInt = rand.Next(1, 1000);

            tabName = "Test Tab" + nameInt;

            GoToEditTabsPage();
            chyHelper.ClickElement("Tabs/CreateTabButton");
            chyHelper.TypeText("Tabs/EnterNewTabName", tabName);
            chyHelper.ClickElement("Tabs/CreateSaveButton");
            chyHelper.WaitForWorkAround(2000);
        }
        public void CreateTabSection()
        {
            Random rand = new Random();
            int nameInt = rand.Next(1, 1000);

            tabSectionName = "Test Section" + nameInt;

            GoToEditTabSectionsPage();
            chyHelper.ClickElement("TabSections/CreateSectionButton");
            chyHelper.WaitForWorkAround(2000);
            chyHelper.TypeText("TabSections/EnterNewSectionName", tabSectionName);
            chyHelper.ClickElement("TabSections/CreateSaveButton");
            chyHelper.AcceptAlert();
            chyHelper.WaitForWorkAround(2000);
        }
        public void DeleteLastTab()
        {
            GoToEditTabsPage();

            chyHelper.ClickElement("Tabs/DeleteTab");
            chyHelper.AcceptAlert();
            chyHelper.WaitForWorkAround(3000);
        }
        public void DeleteLastTabSection()
        {
            GoToEditTabSectionsPage();

            chyHelper.ClickElement("TabSections/DeleteTabSection");
            chyHelper.AcceptAlert();
            chyHelper.WaitForWorkAround(3000);
        }
        public void GoToClientPage()
        {
            chyHelper.MouseHover("Navigate/UserName");
            chyHelper.ClickElement("Navigate/MainSiteTab");
            chyHelper.ClickElement("Navigate/ClientsTab");
            chyHelper.WaitForWorkAround(2000);
            chyHelper.ClickElement("ClientsSection/ChooseClient");
            chyHelper.WaitForWorkAround(2000);
        }
        [TestMethod]
        public void TestCreateNewTab()
        {
            LoginUser("username9", "password9");
            chyHelper = new ChyHelper(GetWebDriver(), "/OfficeFieldDictionary.xml");


            CreateTab();
            
            chyHelper.VerifyPageText(tabName);

            GoToClientPage();
            chyHelper.VerifyPageText(tabName);
            DeleteLastTab();

        }
        [TestMethod]
        public void TestDeleteTab()
        {
            
            TestCreateNewTab();
            chyHelper.VerifyTrueOrFalse(false, tabName);

            GoToClientPage();
            chyHelper.VerifyTrueOrFalse(false, tabName);

        }
        [TestMethod]
        public void TestEditExistingTab()
        {
            LoginUser("username9", "password9");
            chyHelper = new ChyHelper(GetWebDriver(), "/OfficeFieldDictionary.xml");

            Random rand = new Random();
            int nameInt = rand.Next(1, 1000);

            tabName = "Test Tab" + nameInt;

            GoToEditTabsPage();

            chyHelper.ClickElement("Tabs/EditFirstTab");
            chyHelper.WaitForWorkAround(2000);
            chyHelper.WaitUntilElementIsVisible("Tabs/EnterEditedTabName");
            
            chyHelper.TypeText("Tabs/EnterEditedTabName", tabName);
            chyHelper.ClickElement("Tabs/EditSaveButton");

            chyHelper.VerifyTrueOrFalse(false, "Info");
            chyHelper.VerifyPageText(tabName);

            GoToClientPage();
           chyHelper.VerifyNodeTextFalse("Tabs/InfoPageTab1", "Info");
            chyHelper.VerifyNodeTextTrue("Tabs/InfoPageTab1", tabName);

            //Clean Up to be things back as they were for future tests
            GoToEditTabsPage();
            chyHelper.ClickElement("Tabs/EditFirstTab");
            chyHelper.TypeText("Tabs/EnterEditedTabName", "Info");
            chyHelper.ClickElement("Tabs/EditSaveButton");
            chyHelper.VerifyTrueOrFalse(false, tabName);
            chyHelper.VerifyPageText("Info");
        }
        [TestMethod]
        public void TestEditTabSection()
        {
            LoginUser("username9", "password9");
            chyHelper = new ChyHelper(GetWebDriver(), "/OfficeFieldDictionary.xml");
            chyHelper.WaitForWorkAround(1000);
            GoToEditTabSectionsPage();

            Random rand = new Random();
            int nameInt = rand.Next(1, 1000);

            tabSectionName = "Test Section " + nameInt;


            chyHelper.ClickElement("TabSections/EditSection");
            chyHelper.TypeText("TabSections/EditSectionName",tabSectionName);
            chyHelper.ClickElement("TabSections/EditSaveButton");
            chyHelper.AcceptAlert();
            chyHelper.WaitForWorkAround(1000);

            GoToClientPage();
            chyHelper.ClickElement("ClientsSection/CompanyDetailsTab");
            chyHelper.WaitForWorkAround(1000);
            chyHelper.VerifyTrueOrFalse(true, tabSectionName);
           chyHelper.VerifyTrueOrFalse(false, "Site Survey");

            //Clean Up and change section name back to its original value for future tests
            GoToEditTabSectionsPage();
            chyHelper.ClickElement("TabSections/EditSection");
            chyHelper.TypeText("TabSections/EditSectionName", "Description");
            chyHelper.ClickElement("TabSections/EditSaveButton");
            chyHelper.AcceptAlert();
            chyHelper.WaitForWorkAround(1000);
            chyHelper.VerifyTrueOrFalse(false, tabSectionName);
            chyHelper.VerifyTrueOrFalse(true, "Site Survey");
        }
        [TestMethod]
        public void TestCreateNewTabSection()
        {
            LoginUser("username9", "password9");
            chyHelper = new ChyHelper(GetWebDriver(), "/OfficeFieldDictionary.xml");


            CreateTabSection();

            chyHelper.VerifyPageText(tabSectionName);

            //Have to add field to new section to make it appear in tab
            chyHelper.MouseHover("Navigate/FieldDictionaryTab");
            chyHelper.MouseHover("Navigate/FieldsTab");
            chyHelper.ClickElement("Navigate/FieldPropertiesTab");

            chyHelper.Select("SectionFields/SelectModule", "20");
            chyHelper.ClickElement("SectionFields/SearchButton");
            chyHelper.ClickElement("SectionFields/ChooseCompanyLogo");
            chyHelper.WaitForWorkAround(2000);
            chyHelper.SelectByText("SectionFields/SelectNewSection", tabSectionName);
            chyHelper.ClickElement("SectionFields/SaveNewFieldInfo");


            //Check on Clients Page
            GoToClientPage();
            chyHelper.ClickElement("ClientsSection/CompanyDetailsTab");
            chyHelper.WaitForWorkAround(1000);
            chyHelper.VerifyPageText(tabSectionName);

            //Do cleanup for future tests, move field back to right section and  delete created section
            DeleteLastTabSection();
            chyHelper.WaitForWorkAround(2000);
            chyHelper.VerifyTrueOrFalse(false, tabSectionName);
            MoveFieldBack();


        }
        [TestMethod]
        public void TestDeleteTabSection()
        {

            TestCreateNewTabSection();

            GoToClientPage();
            chyHelper.WaitForWorkAround(2000);
            chyHelper.VerifyTrueOrFalse(false, tabSectionName);

        }
        //Fails because of bug in Pegasus
       // [TestMethod]
        public void TestMoveSectionNewTab()
        {
            LoginUser("username9", "password9");
            chyHelper = new ChyHelper(GetWebDriver(), "/OfficeFieldDictionary.xml");
            chyHelper.WaitForWorkAround(1000);
            GoToEditTabSectionsPage();

            chyHelper.ClickElement("TabSections/MoveSection1");
            chyHelper.SelectByText("TabSections/SelectMoveToTab", "Contacts");
            chyHelper.ClickElement("TabSections/MoveSaveButton");
            chyHelper.AcceptAlert();
            chyHelper.WaitForWorkAround(1000);

            GoToClientPage();
            chyHelper.ClickElement("ClientsSection/CompanyDetailsTab");
            chyHelper.WaitForWorkAround(1000);
            chyHelper.VerifyTrueOrFalse(false, "Site Survey");

            chyHelper.ClickElement("ClientsSection/ContactsTab");
            chyHelper.WaitForWorkAround(1000);
            chyHelper.VerifyTrueOrFalse(true, "Site Survey");

            //Clean Up and change section name back to its original value for future tests
            GoToEditTabSectionsPage();
           //Need to Finish 
        }
        [TestMethod]
        public void TestEditExistingField()
        {
            LoginUser("username9", "password9");
            chyHelper = new ChyHelper(GetWebDriver(), "/OfficeFieldDictionary.xml");

            Random rand = new Random();
            int nameInt = rand.Next(1, 1000);

            sectionFieldName = "Test Section " + nameInt;

            chyHelper.MouseHover("Navigate/UserName");
            chyHelper.ClickElement("Navigate/AdminTab");

            chyHelper.MouseHover("Navigate/FieldDictionaryTab");
            chyHelper.MouseHover("Navigate/FieldsTab");
            chyHelper.WaitForWorkAround(1500);
            chyHelper.ClickElement("Navigate/FieldPropertiesTab");

            chyHelper.Select("SectionFields/SelectModule", "20");
            chyHelper.ClickElement("SectionFields/SearchButton");
            chyHelper.ClickElement("SectionFields/ChooseCompanyLogo");
            chyHelper.WaitForWorkAround(2000);
            chyHelper.TypeText("SectionFields/ChangeFieldName",sectionFieldName );
            chyHelper.ClickElement("SectionFields/SaveNewFieldInfo");

            //Check if name changed
            GoToClientPage();
            chyHelper.ClickElement("ClientsSection/CompanyDetailsTab");
            chyHelper.WaitForWorkAround(1000);
            chyHelper.VerifyTrueOrFalse(false, "Company Logo");
            chyHelper.VerifyTrueOrFalse(true, sectionFieldName);

            //Change name back
            SetXPath("SectionFields/ChooseTestField", "<![CDATA[//a[text()='" + sectionFieldName + "']]]>");
            chyHelper.MouseHover("Navigate/UserName");
            chyHelper.ClickElement("Navigate/AdminTab");

            chyHelper.MouseHover("Navigate/FieldDictionaryTab");
            chyHelper.MouseHover("Navigate/FieldsTab");
            chyHelper.ClickElement("Navigate/FieldPropertiesTab");

            chyHelper.Select("SectionFields/SelectModule", "20");
            chyHelper.ClickElement("SectionFields/SearchButton");
            chyHelper.ClickElement("SectionFields/ChooseTestField");
            chyHelper.WaitForWorkAround(2000);
            chyHelper.TypeText("SectionFields/ChangeFieldName", "Company Logo");
            chyHelper.ClickElement("SectionFields/SaveNewFieldInfo");

            GoToClientPage();
            chyHelper.ClickElement("ClientsSection/CompanyDetailsTab");
            chyHelper.WaitForWorkAround(1000);
            chyHelper.VerifyTrueOrFalse(true, "Company Logo");

        }
        [TestMethod]
        public void TestMoveFieldToNewTab()
        {
            LoginUser("username9", "password9");
            chyHelper = new ChyHelper(GetWebDriver(), "/OfficeFieldDictionary.xml");

            //Go to fields section and move field to new tab
            chyHelper.MouseHover("Navigate/UserName");
            chyHelper.ClickElement("Navigate/AdminTab");
            chyHelper.WaitForWorkAround(1500);
            chyHelper.MouseHover("Navigate/FieldDictionaryTab");
            chyHelper.MouseHover("Navigate/FieldsTab");
            chyHelper.ClickElement("Navigate/FieldPropertiesTab");

            chyHelper.Select("SectionFields/SelectModule", "20");
            chyHelper.ClickElement("SectionFields/SearchButton");
            chyHelper.ClickElement("SectionFields/ChooseCompanyLogo");
            chyHelper.WaitForWorkAround(2000);
            //Move to this tab and section
            chyHelper.SelectByText("SectionFields/SelectNewTab", "Business Details");
            chyHelper.WaitForWorkAround(1000);
            chyHelper.SelectByText("SectionFields/SelectNewSection", "Merchant Account Data");
            chyHelper.ClickElement("SectionFields/SaveNewFieldInfo");

            //Go check to make sure field is moved
            GoToClientPage();
            chyHelper.ClickElement("ClientsSection/CompanyDetailsTab");
            chyHelper.WaitForWorkAround(1000);
            chyHelper.VerifyTrueOrFalse(false, "Company Logo");

            chyHelper.ClickElement("ClientsSection/BusinessDetailsTab");
            chyHelper.VerifyTrueOrFalse(true, "Company Logo");

            //Move field back for future tests
            chyHelper.MouseHover("Navigate/UserName");
            chyHelper.ClickElement("Navigate/AdminTab");
            chyHelper.MouseHover("Navigate/FieldDictionaryTab");
            chyHelper.MouseHover("Navigate/FieldsTab");
            chyHelper.ClickElement("Navigate/FieldPropertiesTab");

            chyHelper.Select("SectionFields/SelectModule", "20");
            chyHelper.ClickElement("SectionFields/SearchButton");
            chyHelper.ClickElement("SectionFields/ChooseCompanyLogo");
            chyHelper.WaitForWorkAround(2000);
            //Move to this tab and section
            chyHelper.SelectByText("SectionFields/SelectNewTab", "Company Details");
            chyHelper.WaitForWorkAround(1000);
            chyHelper.SelectByText("SectionFields/SelectNewSection", "Company Details");
            chyHelper.ClickElement("SectionFields/SaveNewFieldInfo");

        }
        //[TestMethod]
        public void TestRearrangeTabs()
        {
            LoginUser("username9", "password9");
            chyHelper = new ChyHelper(GetWebDriver(), "/OfficeFieldDictionary.xml");

            chyHelper.MouseHover("Navigate/UserName");
            chyHelper.ClickElement("Navigate/AdminTab");

            chyHelper.MouseHover("Navigate/FieldDictionaryTab");
            chyHelper.ClickElement("Navigate/TabsTab");

            chyHelper.DragAndDropElement("Tabs/AssignmentsTab", "Tabs/BoardingTab");
            chyHelper.WaitForWorkAround(2000);
            chyHelper.AcceptAlert();

        }
        [TestMethod]
        public void TestFieldGroupingTemplate()
        {
            //Create Template
            LoginUser("username9", "password9");
            chyHelper = new ChyHelper(GetWebDriver(), "/OfficeFieldDictionary.xml");

            Random rand = new Random();
            int nameInt = rand.Next(1, 1000);
            string fieldTemplateName ="Test Template "+nameInt;

            chyHelper.MouseHover("Navigate/UserName");
            chyHelper.ClickElement("Navigate/AdminTab");
            chyHelper.WaitForWorkAround(1500);
            chyHelper.MouseHover("Navigate/FieldDictionaryTab");
            chyHelper.ClickElement("Navigate/FieldGroupingTab");

            chyHelper.ClickElement("GroupingTemplate/CreateNewButton");
            chyHelper.TypeText("GroupingTemplate/EnterName",fieldTemplateName);
            chyHelper.SelectByText("GroupingTemplate/SelectModule", "Clients");
            chyHelper.WaitForWorkAround(2000);
            

            chyHelper.SelectByText("GroupingTemplate/SelectTab", "Company Details");
            chyHelper.SelectByText("GroupingTemplate/SelectField", "Company DBA Name");

            chyHelper.ClickElement("GroupingTemplate/AddFieldButton");
            chyHelper.ClickElement("GroupingTemplate/SaveButton");


            //Check selections in pdf import wizard
            chyHelper.MouseHover("Navigate/PDFTemplatesTab");

            chyHelper.ClickElement("Navigate/PDFImportWizardTab");

            chyHelper.SelectByText("ImportPDF/SelectPDFModule", "Clients");

            chyHelper.Upload("ImportPDF/UploadPDFFile", Path.GetFullPath("../../Resources/MarineMPA.pdf"));
            chyHelper.WaitForWorkAround(2000);
            chyHelper.ClickElement("ImportPDF/ImportButton");
            chyHelper.WaitForWorkAround(2000);
            chyHelper.ClickElement("GroupingTemplate/SelectMappingByTemplate");
            chyHelper.SelectByText("GroupingTemplate/SelectTemplateName", fieldTemplateName);
            chyHelper.SelectByText("GroupingTemplate/SelectTab", "Company Details");
            chyHelper.WaitForWorkAround(2000);

            //determines if test failed or not by what options are in menu
            bool isThere = chyHelper.CheckSelectOptionThere("GroupingTemplate/SelectField", "Company Legal Name");
            
            if (isThere == true)
                Assert.IsTrue(false);
            else
                Assert.IsTrue(true);

            isThere = chyHelper.CheckSelectOptionThere("GroupingTemplate/SelectField", "Company DBA Name");

            if (isThere == true)
                Assert.IsTrue(true);
            else
                Assert.IsTrue(false);
        }
    }
}
