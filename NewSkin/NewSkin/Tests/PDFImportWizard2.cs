using Microsoft.VisualStudio.TestTools.UnitTesting;
using NewSkin.PageHelper;
using NewSkin.PageHelper.Comm;
using System;
using System.IO;
using System.Xml;

namespace NewSkin.Tests
{
    [TestClass]
    public class PDFImportWizard2 : DriverTestCase2
    {
        private ChyHelper chyHelper;
        private string pdfPackageName;
        

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
        public void SelectAllPDFPermissions()
        {
            chyHelper.SelectByText("ImportPDF/SelectPDFCategory", "Card Service Agreements");
            chyHelper.ClickElement("ImportPDF/CheckDisplayInTabBox");
            chyHelper.ClickElement("ImportPDF/CheckShareBox");
            chyHelper.ClickElement("ImportPDF/CheckEmailBox");
            chyHelper.ClickElement("ImportPDF/ChooseAllOffices");
        }
        public void ImportPDFToLeads()
        {
            

            chyHelper.MouseHover("Navigate/PDFTemplatesTab");

            chyHelper.ClickElement("Navigate/PDFImportWizardTab");

            chyHelper.SelectByText("ImportPDF/SelectPDFModule", "Leads");

            chyHelper.Upload("ImportPDF/UploadPDFFile", Path.GetFullPath("../../Resources/MarineMPA.pdf"));
            chyHelper.WaitForWorkAround(2000);
            chyHelper.ClickElement("ImportPDF/ImportButton");

            chyHelper.SelectByText("ImportPDF/SelectPegasusTab", "Company Details");
            chyHelper.WaitForWorkAround(2000);
            chyHelper.SelectByText("ImportPDF/SelectPegasusField", "Company Name");
            chyHelper.ClickElement("ImportPDF/PDFSaveMappingsButton");

            chyHelper.ClickElement("Navigate/NextButton");
            chyHelper.ClickElement("Navigate/NextButton");

            SelectAllPDFPermissions();

            chyHelper.ClickElement("Navigate/SaveImportButton");

        }
        public void ImportPDFToClients()
        {
            

            chyHelper.MouseHover("Navigate/PDFTemplatesTab");

            chyHelper.ClickElement("Navigate/PDFImportWizardTab");

            chyHelper.SelectByText("ImportPDF/SelectPDFModule", "Clients");

            chyHelper.Upload("ImportPDF/UploadPDFFile", Path.GetFullPath("../../Resources/MarineMPA.pdf"));
            chyHelper.WaitForWorkAround(2000);
            chyHelper.ClickElement("ImportPDF/ImportButton");
            chyHelper.WaitForWorkAround(2000);

            chyHelper.ClickElement("ImportPDF/ClickMapWithRule");
            chyHelper.SelectByText("ImportPDF/SelectSetValue", "Set Value");
            chyHelper.TypeText("ImportPDF/EnterSetValue", "1234.56");
            /*
            chyHelper.SelectByText("ImportPDF/SelectPegasusTab", "Company Details");
            chyHelper.WaitForWorkAround(2000);
            chyHelper.SelectByText("ImportPDF/SelectPegasusField", "Company DBA Name");
     */

            chyHelper.ClickElement("ImportPDF/PDFSaveRuleButton");

            chyHelper.ClickElement("Navigate/NextButton");
            chyHelper.ClickElement("Navigate/NextButton");


            SelectAllPDFPermissions();

            chyHelper.ClickElement("Navigate/SaveImportButton");
        }
        
        public void GoToLeadsPDFTab()
        {
            chyHelper.ClickElement("Navigate/LeadsTab");
            chyHelper.TypeText("CheckInOffice/SearchForCompanyLead", "Mack Company");
            chyHelper.WaitForWorkAround(2000);
            chyHelper.TypeText("CheckInOffice/SearchForCompanyLead", "Mack Company");
            chyHelper.ClickElement("CheckInOffice/ClickMackCompanyLead");
            chyHelper.ClickElement("CheckInOffice/PDFsTab");
        }
        public void GoToClientsPDFTab()
        {
            chyHelper.ClickElement("Navigate/ClientsTab");
            chyHelper.TypeText("CheckInOffice/SearchForCompanyClient", "Chy Company");
            chyHelper.WaitForWorkAround(2000);
            chyHelper.ClickElement("CheckInOffice/ClickChyCompanyClient");
            chyHelper.ClickElement("CheckInOffice/PDFsTab");
        }
        [TestCleanup]
        public void DeletePDF()
        {
            Logout();
            LoginUser("username8", "password8");
            //chyHelper = new ChyHelper(GetWebDriver(), "/PDFImportWizard2.xml");
            
            chyHelper.MouseHover("Navigate/PDFTemplatesTab");
            chyHelper.ClickElement("Navigate/PDFTemplatesInnerTab");

            while (chyHelper.IsElementPresentLocator("DeletePDF/PDFToDelete"))
            {
                chyHelper.ClickElement("DeletePDF/FirstCheckBox");
                chyHelper.ClickElement("DeletePDF/DeleteButton");
                chyHelper.AcceptAlert();
            }


        }
        /// <summary>
        ///     Corporate user should be able import a PDF into the Clients module
        ///</summary>
        ///<Logins>
        ///     username8: newthemecorp
        ///     password8: seWelcome2
        ///     username9: newthemeoffice
        ///     password9: seWelcome2
        /// </Logins>
        ///<preconditions>
        ///    1) PDF MarineMPA.pdf must be in Resources folder
        ///    2) Client "Chy Company" must already exist
        ///     3) Must have PDF Category"Card Service Agreements"
        /// </preconditions>
        [TestMethod]
        public void TestPDFCorpImportToClients()
        {
            LoginUser("username8", "password8");
            chyHelper = new ChyHelper(GetWebDriver(), "/PDFImportWizard2.xml");

            ImportPDFToClients();

            Logout();

            //Login as other user to see if PDF is available

            LoginUser("username9", "password9");

            GoToClientsPDFTab();
            //chyHelper.ClickElement("CheckInOffice/ClickImportedPDF");

            chyHelper.VerifyPageText("MarineMPA");
            
        }
   
        /// <summary>
        ///     Corporate user should be able import a PDF into the Leads module
        ///</summary>
        ///<Logins>
        ///     username8: newthemecorp
        ///     password8: seWelcome2
        ///     username9: newthemeoffice
        ///     password9: seWelcome2
        /// </Logins>
        ///<preconditions>
        ///    1) PDF MarineMPA.pdf must be in Resources folder
        ///    2) Client "Mack Company" must already exist
        ///     3) Must have PDF Category"Card Service Agreements"
        /// </preconditions>
        [TestMethod]
        public void TestPDFCorpImportToLeads()
        {
            LoginUser("username8", "password8");
            chyHelper = new ChyHelper(GetWebDriver(), "/PDFImportWizard2.xml");

            ImportPDFToLeads();
            Logout();

            //Login as other user to see if PDF is available

            LoginUser("username9", "password9");

            GoToLeadsPDFTab();
            //chyHelper.ClickElement("CheckInOffice/ClickImportedPDF");

            chyHelper.VerifyPageText("MarineMPA");
            
        }
     
        /// <summary>
        ///     Should be able to make PDF inactive/active with thumb 
       ///       image button in Corporate Portal
        ///</summary>
        ///<Logins>
        ///     username8: newthemecorp
        ///     password8: seWelcome2
        ///     username9: newthemeoffice
        ///     password9: seWelcome2
        /// </Logins>
        ///<preconditions>
        ///    1) PDF MarineMPA.pdf must be in Resources folder
        ///    2) Client "Chy Company" must already exist
        ///     3) Must have PDF Category"Card Service Agreements"
        /// </preconditions>
        [TestMethod]
        public void TestPDFCorpInactiveButton()
        {
            LoginUser("username8", "password8");

            chyHelper = new ChyHelper(GetWebDriver(), "/PDFImportWizard2.xml");

            //First Import File to work with

            ImportPDFToClients();


            //Then Check that PDF is there
            Logout();

            LoginUser("username9", "password9");

            GoToClientsPDFTab();
            //chyHelper.ClickElement("CheckInOffice/ClickImportedPDF");

            chyHelper.VerifyPageText("MarineMPA");


            //Now make file inactive in corporate
            Logout();
            LoginUser("username8", "password8");

            chyHelper.MouseHover("Navigate/PDFTemplatesTab");
            chyHelper.ClickElement("Navigate/PDFTemplatesInnerTab");

            chyHelper.ClickElement("EditPDF/MakeInactiveButton");
            chyHelper.AcceptAlert();

            //Now check to see if file is there in office
            Logout();
            LoginUser("username9", "password9");

            GoToClientsPDFTab();

            chyHelper.VerifyTrueOrFalse(false,"MarineMPA");

            

        }
     
        /// <summary>
        ///     Should be able to make PDF inactive/active with "Make Inactive/Active"
        ///     link on PDF info page in Corporate Portal
        ///</summary>
        ///<Logins>
        ///     username8: newthemecorp
        ///     password8: seWelcome2
        ///     username9: newthemeoffice
        ///     password9: seWelcome2
        /// </Logins>
        ///<preconditions>
        ///    1) PDF MarineMPA.pdf must be in Resources folder
        ///    2) Client "Chy Company" must already exist
        ///     3) Must have PDF Category"Card Service Agreements"
        /// </preconditions>
        [TestMethod]
        public void TestPDFCorpInactiveLink()
        {
            LoginUser("username8", "password8");

            chyHelper = new ChyHelper(GetWebDriver(), "/PDFImportWizard2.xml");

            //First Import File to work with

            ImportPDFToClients();


            //Then Check that PDF is there
            Logout();

            LoginUser("username9", "password9");

            GoToClientsPDFTab();
            //chyHelper.ClickElement("CheckInOffice/ClickImportedPDF");

            chyHelper.VerifyPageText("MarineMPA");


            //Now make file inactive in corporate
            Logout();
            LoginUser("username8", "password8");

            chyHelper.MouseHover("Navigate/PDFTemplatesTab");
            chyHelper.ClickElement("Navigate/PDFTemplatesInnerTab");

            chyHelper.ClickElement("EditPDF/ClickOnPDFName");
            chyHelper.ClickElement("EditPDF/MakeInactiveLink");

            //Now check to see if file is there in office
            Logout();
            LoginUser("username9", "password9");

            GoToClientsPDFTab();

            chyHelper.VerifyTrueOrFalse(false, "MarineMPA");

            

        }
       
        /// <summary>
        ///     Should be able to edit PDF mappings through Corporate Portal
        ///</summary>
        ///<Logins>
        ///     username8: newthemecorp
        ///     password8: seWelcome2
        /// </Logins>
        ///<preconditions>
        ///    1) PDF MarineMPA.pdf must be in Resources folder
        ///    2) Must have PDF Category "Card Service Agreements"
        /// </preconditions>
        [TestMethod]
        public void TestPDFCorpEditMappings()
        {
            LoginUser("username8", "password8");

            chyHelper = new ChyHelper(GetWebDriver(), "/PDFImportWizard2.xml");

            //First Import File to work with Company DBA mapped
            ImportPDFToClients();


            //Edit mappings

            chyHelper.ClickElement("EditPDF/EditMappingsButton");
            chyHelper.WaitForWorkAround(2000);
            chyHelper.ClickElement("EditPDF/ClickPrintedNameField");
            chyHelper.VerifyPageText("1234.56");


            chyHelper.TypeText("ImportPDF/EnterSetValue", "789.67");
            chyHelper.ClickElement("ImportPDF/PDFSaveRuleButton");
            chyHelper.ClickElement("EditPDF/UpdateButton");

            //Check that field was saved
            chyHelper.ClickElement("EditPDF/EditMappingsButton");
            chyHelper.WaitForWorkAround(2000);
            chyHelper.ClickElement("EditPDF/ClickPrintedNameField");
            chyHelper.VerifyPageText("789.67");

            

        }
        
        /// <summary>
        ///     Should be able to delete imported PDF in Corporate Portal
        ///</summary>
        ///<Logins>
        ///     username8: newthemecorp
        ///     password8: seWelcome2
        /// </Logins>
        ///<preconditions>
        ///    1) PDF MarineMPA.pdf must be in Resources folder
        ///    2)Must have PDF Category"Card Service Agreements"
        /// </preconditions>
        [TestMethod]
        public void TestPDFCorpDeletePDF()
        {
            LoginUser("username8", "password8");

            chyHelper = new ChyHelper(GetWebDriver(), "/PDFImportWizard2.xml");

            //First Import File to work with Company DBA mapped
            ImportPDFToClients();

            chyHelper.MouseHover("Navigate/PDFTemplatesTab");
            chyHelper.ClickElement("Navigate/PDFTemplatesInnerTab");

            chyHelper.ClickElement("DeletePDF/FirstCheckBox");
            chyHelper.ClickElement("DeletePDF/DeleteButton");
            chyHelper.AcceptAlert();
            chyHelper.WaitForWorkAround(3000);
            chyHelper.VerifyAnyNodeWithText(false, "MarineMPA");
            

        }

        /// <summary>
        ///     Should be able to edit PDF permissions in Corporate portal
        ///</summary>
        ///<Logins>
        ///     username8: newthemecorp
        ///     password8: seWelcome2
        ///     username9: newthemeoffice
        ///     password9: seWelcome2
        /// </Logins>
        ///<preconditions>
        ///    1) PDF MarineMPA.pdf must be in Resources folder
        ///    2) Client "Chy Company" must already exist
        ///    3) Must have PDF Category"Card Service Agreements"
        /// </preconditions>
        [TestMethod]
        public void TestPDFCorpEditPermissions()
        {
            LoginUser("username8", "password8");

            chyHelper = new ChyHelper(GetWebDriver(), "/PDFImportWizard2.xml");

            //First Import File to work with Company DBA mapped
            ImportPDFToClients();


            //Edit mappings

            chyHelper.ClickElement("EditPDF/EditPermissionsButton");
            chyHelper.ClickElement("EditPDF/ChooseNoOffices");
            chyHelper.ClickElement("EditPDF/PushToOffices");


            Logout();
            LoginUser("username9", "password9");

            GoToClientsPDFTab();

            chyHelper.VerifyTrueOrFalse(false, "MarineMPA");

           
        }

        /// <summary>
        ///     Should be able to edit PDF information in Corporate Portal
        ///</summary>
        ///<Logins>
        ///     username8: newthemecorp
        ///     password8: seWelcome2
        ///     username9: newthemeoffice
        ///     password9: seWelcome2
        /// </Logins>
        ///<preconditions>
        ///    1) PDF MarineMPA.pdf must be in Resources folder
        ///    2) Client "Chy Company" must already exist
        ///    3) Must have PDF Category"Card Service Agreements"
        /// </preconditions>
        [TestMethod]
        public void TestPDFCorpEditPDFInfo()
        {
            
            LoginUser("username8", "password8");

            chyHelper = new ChyHelper(GetWebDriver(), "/PDFImportWizard2.xml");


            ImportPDFToClients();


            //Edit info to make inactive

            chyHelper.ClickElement("EditPDF/EditPDFInfoButton");
            chyHelper.SelectByText("EditPDF/SelectStatusInactive", "Inactive");
            chyHelper.ClickElement("EditPDF/PushToOffices");

            //Check in Office portal
            Logout();
            LoginUser("username9", "password9");

            GoToClientsPDFTab();
            chyHelper.VerifyTrueOrFalse(false, "MarineMPA");

            

        }
        
        /// <summary>
        ///     Should be able to create new PDF package in Corporate portal
        ///</summary>
        ///<Logins>
        ///     username8: newthemecorp
        ///     password8: seWelcome2
        ///     username9: newthemeoffice
        ///     password9: seWelcome2
        /// </Logins>
        ///<preconditions>
        ///    1) PDF MarineMPA.pdf must be in Resources folder
        ///    2) Client "Chy Company" must already exist
        ///    3) Must have PDF Category"Card Service Agreements"
        ///     4) Must have at least 2 PDFs already created and available to add to package
        /// </preconditions>
        [TestMethod]
        public void TestPDFCreatePackage()
        {
            LoginUser("username8", "password8");
            chyHelper = new ChyHelper(GetWebDriver(), "/PDFImportWizard2.xml");

            chyHelper.MouseHover("Navigate/PDFTemplatesTab");
            chyHelper.ClickElement("Navigate/PDFTemplatesInnerTab");

            chyHelper.ClickElement("PDFPackage/CreatePDFPackageButton");

            pdfPackageName= "Test Package " + GetRandomNumber();
            chyHelper.TypeText("PDFPackage/EnterName", pdfPackageName);

            chyHelper.SelectByText("PDFPackage/SelectPackageModule", "Clients");
            chyHelper.WaitForWorkAround(2000);
            chyHelper.SelectByIndex("PDFPackage/SelectPDFsForPackage", 1);
            chyHelper.ClickElement("PDFPackage/AddPDFButton");
            chyHelper.SelectByIndex("PDFPackage/SelectPDFsForPackage", 2);
            chyHelper.ClickElement("PDFPackage/AddPDFButton");

            SelectAllPDFPermissions();

            chyHelper.ClickElement("PDFPackage/SaveButton");


            Logout();
            LoginUser("username9", "password9");

            GoToClientsPDFTab();
            chyHelper.VerifyPageText(pdfPackageName);

        }

        /// <summary>
        ///     Import wizard should detect signature fields
        ///</summary>
        ///<Logins>
        ///     username8: newthemecorp
        ///     password8: seWelcome2
        /// </Logins>
        ///<preconditions>
        ///    1) PDF MarineMPA.pdf must be in Resources folder
        /// </preconditions>
        [TestMethod]
        public void TestPDFDetectSignatures()
        {

            LoginUser("username8", "password8");
            chyHelper = new ChyHelper(GetWebDriver(), "/PDFImportWizard2.xml");

            chyHelper.MouseHover("Navigate/PDFTemplatesTab");

            chyHelper.ClickElement("Navigate/PDFImportWizardTab");

            chyHelper.SelectByText("ImportPDF/SelectPDFModule", "Clients");

            chyHelper.Upload("ImportPDF/UploadPDFFile", Path.GetFullPath("../../Resources/MarineMPA.pdf"));
            chyHelper.WaitForWorkAround(2000);
            chyHelper.ClickElement("ImportPDF/ImportButton");

            chyHelper.WaitForWorkAround(2000);
            chyHelper.ClickElement("Navigate/NextButton");

            chyHelper.VerifyPageText("Signatures Found on PDF Document");

            chyHelper.ClickElement("ImportPDF/CancelButton");
            chyHelper.AcceptAlert();
            chyHelper.WaitForWorkAround(2000);
        }
       
        /// <summary>
        ///     The field searcher in the PDF mapping wizard should work
        ///</summary>
        ///<Logins>
        ///     username8: newthemecorp
        ///     password8: seWelcome2
        /// </Logins>
        ///<preconditions>
        ///    1) PDF MarineMPA.pdf must be in Resources folder
        /// </preconditions>
        [TestMethod]
        public void TestPDFFieldSearcher()
        {
            LoginUser("username8", "password8");
            chyHelper = new ChyHelper(GetWebDriver(), "/PDFImportWizard2.xml");

            chyHelper.MouseHover("Navigate/PDFTemplatesTab");

            chyHelper.ClickElement("Navigate/PDFImportWizardTab");

            chyHelper.SelectByText("ImportPDF/SelectPDFModule", "Clients");

            chyHelper.Upload("ImportPDF/UploadPDFFile", Path.GetFullPath("../../Resources/MarineMPA.pdf"));
            chyHelper.WaitForWorkAround(2000);
            chyHelper.ClickElement("ImportPDF/ImportButton");


            chyHelper.TypeText("ImportPDF/SearchPDFFileField", "Printed Name of Representative");
            chyHelper.WaitForWorkAround(2500);

            
            if (chyHelper.IsElementPresentLocator("ImportPDF/SearchListVisibleElement"))
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsTrue(false);
            }
            chyHelper.ClickElement("ImportPDF/CancelButton");
            chyHelper.AcceptAlert();
            chyHelper.WaitForWorkAround(2000);
        }

        /// <summary>
        ///     PDF Mappings should remain when PDF is replaced
        ///</summary>
        ///<Logins>
        ///     username8: newthemecorp
        ///     password8: seWelcome2
        /// </Logins>
        ///<preconditions>
        ///    1) PDF MarineMPA.pdf must be in Resources folder
        ///    2) Must have PDF Category"Card Service Agreements"
        /// </preconditions>
        [TestMethod]
        public void TestPDFReplacement()
        {
            LoginUser("username8", "password8");
            chyHelper = new ChyHelper(GetWebDriver(), "/PDFImportWizard2.xml");

            ImportPDFToClients();
            //Edit info to make inactive

            chyHelper.ClickElement("EditPDF/EditPDFInfoButton");
            chyHelper.ClickElement("EditPDF/ReplacePDFLink");

            chyHelper.Upload("ImportPDF/UploadPDFFile", Path.GetFullPath("../../Resources/MarineMPA.pdf"));
            chyHelper.WaitForWorkAround(3000);

            chyHelper.ClickElement("EditPDF/SaveChanges");
            chyHelper.WaitForWorkAround(2000);
            chyHelper.ClickElement("EditPDF/ClickPrintedNameField");
            chyHelper.VerifyPageText("1234.56");

            

        }

        /// <summary>
        ///     User should be able to cancel editing a PDF and none of the changes be saved
        ///</summary>
        ///<Logins>
        ///     username8: newthemecorp
        ///     password8: seWelcome2
        /// </Logins>
        ///<preconditions>
        ///    1) PDF MarineMPA.pdf must be in Resources folder
        /// </preconditions>
        [TestMethod]
        public void TestPDFCorpCancelEdit()
        {
            LoginUser("username8", "password8");

            chyHelper = new ChyHelper(GetWebDriver(), "/PDFImportWizard2.xml");

            //First Import File to work with Company DBA mapped
            ImportPDFToClients();


            //Edit mappings

            chyHelper.ClickElement("EditPDF/EditMappingsButton");
            chyHelper.WaitForWorkAround(2000);
            chyHelper.ClickElement("EditPDF/ClickPrintedNameField");


            chyHelper.TypeText("ImportPDF/EnterSetValue", "6757.56");
       
            chyHelper.ClickElement("ImportPDF/PDFSaveRuleButton");

            chyHelper.ClickElement("EditPDF/CancelButton");

            //Check that field was NOT saved
            chyHelper.ClickElement("EditPDF/EditMappingsButton");
            chyHelper.WaitForWorkAround(2000);
            chyHelper.ClickElement("EditPDF/ClickPrintedNameField");
            chyHelper.WaitForWorkAround(3000);
            chyHelper.VerifyPageText("1234.56");
            

        }
    }

}
