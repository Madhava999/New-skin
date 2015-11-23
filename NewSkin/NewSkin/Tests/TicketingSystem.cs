using Microsoft.VisualStudio.TestTools.UnitTesting;
using NewSkin.PageHelper;
using NewSkin.PageHelper.Comm;
using System;
using System.Xml;

namespace NewSkin.Tests
{
    [TestClass]
    public class TicketingSystem : DriverTestCase2
    {
        private ChyHelper chyHelper;
        private string ticketSubject;

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
            xml.Load("../../Locators/TicketingSystem.xml");
            //string xmlcontent = xml.InnerXml;
            xml.SelectSingleNode("//" + xmlNode).InnerXml = xPathName;
            //  string xmlcontent2 = xml.InnerXml;
            xml.Save("../../Locators/TicketingSystem.xml");

        }
        public void ClickCreateATicket()
        {
            chyHelper = new ChyHelper(GetWebDriver(), "/TicketingSystem.xml");
            chyHelper.WaitForWorkAround(2000);
            chyHelper.MouseHover("Navigate/TicketsTab");

            chyHelper.ClickElement("CreateATicket/ClickCreateATicket");
        }
        public void NavigateToResolvedTickets()
        {
            chyHelper.MouseHover("Navigate/TicketsTab");
            chyHelper.MouseHover("Navigate/AllTicketsTab");
            chyHelper.ClickElement("Navigate/ResolvedTicketsTab");
        }
        public void NavigateToOpenTickets()
        {
            chyHelper.MouseHover("Navigate/TicketsTab");
            chyHelper.MouseHover("Navigate/AllTicketsTab");
            chyHelper.ClickElement("Navigate/OpenTicketsTab");
        }
        public void NavigateToClosedTickets()
        {
            chyHelper.MouseHover("Navigate/TicketsTab");
            chyHelper.MouseHover("Navigate/AllTicketsTab");
            chyHelper.ClickElement("Navigate/ClosedTicketsTab");
        }
        [TestMethod]
        public void TicketingSearchClientByID()
        {
            LoginUser("username9", "password9");
            ClickCreateATicket();

            chyHelper.ClickElement("CreateATicket/SelectClientButton");
            chyHelper.TypeText("CreateATicket/EnterIntoSearchByIDBox", "86093");
            chyHelper.ClickElement("CreateATicket/ClickSearchButton");
            chyHelper.WaitForWorkAround(2000);
            chyHelper.VerifyPageText("Chy Company");

        }
        [TestMethod]
        public void TicketingSearchClientByName()
        {

            LoginUser("username9", "password9");
            ClickCreateATicket();

            chyHelper.ClickElement("CreateATicket/SelectClientButton");
            chyHelper.TypeText("CreateATicket/EnterIntoSearchByNameBox", "Chy Company");
            chyHelper.ClickElement("CreateATicket/ClickSearchButton");
            chyHelper.WaitForWorkAround(2000);
            chyHelper.VerifyPageText("86093");

        }
        [TestMethod]
        public void BulkUpdateTickets()
        {
            //TestCreateATicket();
            //TestCreateATicket();
            LoginUser("username9", "password9");
            chyHelper = new ChyHelper(GetWebDriver(), "/TicketingSystem.xml");

            chyHelper.ClickElement("Navigate/TicketsTab");

            chyHelper.ClickElement("EditTickets/ClickCheckBox1");
            chyHelper.ClickElement("EditTickets/ClickCheckBox2");

            chyHelper.ClickElement("EditTickets/ClickBulkUpdate");
            chyHelper.ClickElement("EditTickets/ChooseToChangeStatus");
            chyHelper.SelectByText("EditTickets/ChooseStatusToChangeTo", "Setup");

            chyHelper.ClickElement("EditTickets/ClickUpdateButton");

            chyHelper.AcceptAlert();
            chyHelper.WaitForWorkAround(2000);

            chyHelper.VerifyPageText("updated");

        }
        [TestMethod]
        public void TicketFilterByCategory1()
        {
            LoginUser("username9", "password9");
            chyHelper = new ChyHelper(GetWebDriver(), "/TicketingSystem.xml");

            chyHelper.ClickElement("Navigate/TicketsTab");
            chyHelper.WaitForWorkAround(2000);
            chyHelper.SelectByText("SearchTickets/CategoryBox", "Billing");
            chyHelper.ClickElement("SearchTickets/RandomElement");
            chyHelper.WaitForWorkAround(2000);
            chyHelper.VerifyAnyNodeWithText(false, "Account Ticket");
        }
      
        [TestMethod]
        public void TestCreateATicket()
        {
            LoginUser("username9", "password9");
            ClickCreateATicket();
            
            ticketSubject = "Test Ticket " + GetRandomNumber();
            chyHelper.TypeText("CreateATicket/EnterName", ticketSubject);
            chyHelper.ClickElement("CreateATicket/SelectClientButton");
            
            chyHelper.TypeText("CreateATicket/EnterIntoSearchByNameBox", "Chy Company");
            chyHelper.ClickElement("CreateATicket/ClickSearchButton");

            chyHelper.ClickElement("CreateATicket/ChooseClient");

            chyHelper.SelectByText("CreateATicket/SelectAssignedTo","Howard Tang");
            chyHelper.SelectByText("CreateATicket/SelectStatus", "Open");
            chyHelper.ClickElement("Navigate/SaveButton");
            chyHelper.WaitForWorkAround(3000);
            chyHelper.VerifyAnyNodeWithText(true,ticketSubject);
        }
        [TestMethod]
        public void TestDeleteTicket()
        {
            TestCreateATicket();


            chyHelper.ClickElement("EditTickets/ClickCheckBox1");
            chyHelper.ClickElement("EditTickets/DeleteButton");

            chyHelper.AcceptAlert();
            chyHelper.AcceptAlert();
            chyHelper.WaitForWorkAround(2000);

            chyHelper.VerifyTrueOrFalse(false, ticketSubject);
        }
        [TestMethod]
        public void TicketCheckAdvancedFilters()
        {
            LoginUser("username9", "password9");
            chyHelper = new ChyHelper(GetWebDriver(), "/TicketingSystem.xml");

            chyHelper.ClickElement("Navigate/TicketsTab");
            chyHelper.ClickElement("SearchTickets/AdvancedFilterButton");
            chyHelper.WaitForWorkAround(2000);
            chyHelper.VerifyPageText("Layout Options");

        }
        [TestMethod]
        public void TestResolveTicketPopup()
        {

            TestCreateATicket();
            SetXPath("EditTickets/SelectTicket", "<![CDATA[//a[text()='" + ticketSubject + "']]]>");
            
            chyHelper.ClickElement("EditTickets/SelectTicket");
            chyHelper.WaitForWorkAround(2000);
            chyHelper.ClickElement("EditTickets/ResolveButton");
            chyHelper.WaitForWorkAround(2000);
            chyHelper.VerifyPageText("Add Resolution");
        }
        [TestMethod]
        public void TestResolveATicket()
        {
            TestCreateATicket();
            chyHelper.WaitForWorkAround(2000);
            SetXPath("EditTickets/SelectTicket", "<![CDATA[//a[text()='" + ticketSubject + "']]]>");
            chyHelper.TypeText("SearchTickets/SearchForTicket", ticketSubject);
            chyHelper.WaitForWorkAround(2000);
            chyHelper.ClickElement("EditTickets/SelectTicket");
            chyHelper.WaitForWorkAround(2000);
            chyHelper.ClickElement("EditTickets/ResolveButton");

            chyHelper.SelectByText("EditTickets/SelectResolution","Issue Resolved");
            chyHelper.ClickElement("EditTickets/SaveResolutionButton");

            NavigateToOpenTickets();
            chyHelper.VerifyTrueOrFalse(false,ticketSubject);

            NavigateToResolvedTickets();
            chyHelper.WaitForWorkAround(2000);
            chyHelper.VerifyPageText(ticketSubject);
        }
        [TestMethod]
        public void TestCloseATicket()
        {
            TestCreateATicket();
            SetXPath("EditTickets/SelectTicket", "<![CDATA[//a[text()='" + ticketSubject + "']]]>");
            chyHelper.TypeText("SearchTickets/SearchForTicket", ticketSubject);
            chyHelper.WaitForWorkAround(2000);
            chyHelper.ClickElement("EditTickets/SelectTicket");
            chyHelper.WaitForWorkAround(2000);
            chyHelper.ClickElement("EditTickets/ResolveButton");

            chyHelper.SelectByText("EditTickets/SelectResolution", "Issue Resolved");
            chyHelper.ClickElement("EditTickets/SaveResolutionButton");

            NavigateToResolvedTickets();
            chyHelper.ClickElement("EditTickets/SelectTicket");

            chyHelper.ClickElement("EditTickets/CloseButton");

            NavigateToResolvedTickets();
            chyHelper.WaitForWorkAround(2000);
            chyHelper.VerifyTrueOrFalse(false, ticketSubject);

            NavigateToClosedTickets();
            chyHelper.WaitForWorkAround(2000);
            chyHelper.VerifyPageText(ticketSubject);
        }
        [TestMethod]
        public void TestReOpenATicket()
        {
            TestCreateATicket();
            SetXPath("EditTickets/SelectTicket", "<![CDATA[//a[text()='" + ticketSubject + "']]]>");
            chyHelper.WaitForWorkAround(2000);
            chyHelper.ClickElement("EditTickets/SelectTicket");
            chyHelper.WaitForWorkAround(2000);
            chyHelper.ClickElement("EditTickets/ResolveButton");

            chyHelper.SelectByText("EditTickets/SelectResolution", "Issue Resolved");
            chyHelper.ClickElement("EditTickets/SaveResolutionButton");

          //  NavigateToResolvedTickets();
            chyHelper.ClickElement("EditTickets/SelectTicket");

            chyHelper.ClickElement("EditTickets/CloseButton");

            chyHelper.ClickElement("EditTickets/SelectTicket");

            chyHelper.ClickElement("EditTickets/ReOpenButton");
            chyHelper.WaitForWorkAround(2000);
            NavigateToClosedTickets();
            chyHelper.WaitForWorkAround(2000);
            chyHelper.VerifyTrueOrFalse(false, ticketSubject);

            NavigateToOpenTickets();
            chyHelper.WaitForWorkAround(2000);
            chyHelper.VerifyPageText(ticketSubject);
        }
        [TestMethod]
        public void ChangeTicketSettings()
        {
            LoginUser("username9", "password9");
            chyHelper = new ChyHelper(GetWebDriver(), "/TicketingSystem.xml");

            chyHelper.MouseHover("Navigate/UserName");
            chyHelper.ClickElement("Navigate/AdminTab");
            chyHelper.MouseHover("Navigate/TicketsTab");
            chyHelper.ClickElement("Navigate/TicketsSettingsTab");

            chyHelper.SelectByText("ChangeSettings/SelectCategory", "Account Support");
            chyHelper.SelectByText("ChangeSettings/SelectDepartment", "IT");
            chyHelper.SelectByText("ChangeSettings/SelectPriority", "Critical");
            chyHelper.WaitForWorkAround(2000);
            chyHelper.SelectByText("ChangeSettings/SelectAssignedTo", "Howard Tang");
            chyHelper.SelectByText("ChangeSettings/SelectManager", "A M");

            chyHelper.ClickElement("Navigate/SaveButton");


            //To test that settings work
            chyHelper.MouseHover("Navigate/UserName");
            chyHelper.ClickElement("Navigate/MainSiteTab");

            chyHelper.MouseHover("Navigate/TicketsTab");

            chyHelper.ClickElement("CreateATicket/ClickCreateATicket");

            ticketSubject = "Test Ticket " + GetRandomNumber();
            chyHelper.TypeText("CreateATicket/EnterName", ticketSubject);

            chyHelper.ClickElement("CreateATicket/SelectClientButton");
            chyHelper.TypeText("CreateATicket/EnterIntoSearchByNameBox", "Chy Company");
            chyHelper.ClickElement("CreateATicket/ClickSearchButton");
            chyHelper.ClickElement("CreateATicket/ChooseClient");

            chyHelper.SelectByText("CreateATicket/SelectCategory", "Account Support");
            chyHelper.ClickElement("Navigate/SaveButton");

            SetXPath("EditTickets/SelectTicket", "<![CDATA[//a[text()='" + ticketSubject + "']]]>");
            chyHelper.ClickElement("EditTickets/SelectTicket");
            chyHelper.WaitForWorkAround(2000);
            chyHelper.VerifyAnyNodeWithText(true,"Account Support");
            chyHelper.VerifyAnyNodeWithText(true,"IT");
            chyHelper.VerifyAnyNodeWithText(true,"Critical");
            chyHelper.VerifyAnyNodeWithText(true,"Howard Tang");
            chyHelper.VerifyAnyNodeWithText(true,"A  M");

            //Set them back for future tests
            chyHelper.MouseHover("Navigate/UserName");
            chyHelper.ClickElement("Navigate/AdminTab");
            chyHelper.MouseHover("Navigate/TicketsTab");
            chyHelper.ClickElement("Navigate/TicketsSettingsTab");

            chyHelper.SelectByText("ChangeSettings/SelectCategory", "Select Category");
            chyHelper.SelectByText("ChangeSettings/SelectDepartment", "Select Department");
            chyHelper.SelectByText("ChangeSettings/SelectPriority", "Select Priority");
            chyHelper.WaitForWorkAround(2000);
            chyHelper.SelectByText("ChangeSettings/SelectAssignedTo", "Select Owner");
            chyHelper.SelectByText("ChangeSettings/SelectManager", "Select Manager");

            chyHelper.ClickElement("Navigate/SaveButton");

        }
    }
}
