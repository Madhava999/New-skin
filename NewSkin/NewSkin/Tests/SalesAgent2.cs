using Microsoft.VisualStudio.TestTools.UnitTesting;
using NewSkin.PageHelper;
using NewSkin.PageHelper.Comm;
using System;
using System.IO;
using System.Xml;

namespace NewSkin.Tests
{
    [TestClass]
    public class SalesAgent2 : DriverTestCase2
    {
        private ChyHelper chyHelper;
        private string ticketSubject;
        private string companyName;


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
            xml.Load("../../Locators/SalesAgent2.xml");
            //string xmlcontent = xml.InnerXml;
            xml.SelectSingleNode("//" + xmlNode).InnerXml = xPathName;
            //  string xmlcontent2 = xml.InnerXml;
            xml.Save("../../Locators/SalesAgent2.xml");

        }
        public int GetRandNum()
        {
            Random rand = new Random();

            int randomNum = rand.Next(1, 9999); ;
            return randomNum;
        }


        [TestMethod]
        public void TestSalesAgentEditContactInfo()
        {
            LoginUser("username12", "password12");
            chyHelper = new ChyHelper(GetWebDriver(), "/SalesAgent2.xml");

            string newName = "TestName" + GetRandNum();

            chyHelper.MouseHover("Navigate/UserIcon");
            chyHelper.ClickElement("Navigate/ProfileTab");
            chyHelper.ClickElement("Edit/EditProfileButton");

            chyHelper.TypeText("Edit/EnterAgentFirstName", newName);
            chyHelper.ClickElement("Navigate/SaveButton");
            chyHelper.WaitForWorkAround(2000);
            chyHelper.VerifyPageText(newName);

            //change back for future tests
            chyHelper.ClickElement("Edit/EditProfileButton");

            chyHelper.TypeText("Edit/EnterAgentFirstName", "Randy");
            chyHelper.ClickElement("Navigate/SaveButton");
            chyHelper.WaitForWorkAround(2000);
            chyHelper.VerifyPageText("Randy");

        }
        [TestMethod]
        public void TestSalesAgentEditPortalSettings()
        {
            LoginUser("username12", "password12");
            chyHelper = new ChyHelper(GetWebDriver(), "/SalesAgent2.xml");

            chyHelper.MouseHover("Navigate/UserIcon");
            chyHelper.ClickElement("Navigate/ProfileTab");

            chyHelper.ClickElement("Edit/SettingsButton");

            chyHelper.SelectByText("Edit/SelectNameFormat", "Last, First");
            chyHelper.ClickElement("Edit/SaveSettingsButton");

            chyHelper.WaitForWorkAround(2000);
            chyHelper.VerifyPageText("Jackson, Randy");

            //Change it back for future tests
            chyHelper.MouseHover("Navigate/UserIcon");
            chyHelper.ClickElement("Navigate/ProfileTab");

            chyHelper.ClickElement("Edit/SettingsButton");

            chyHelper.SelectByText("Edit/SelectNameFormat", "First Last");
            chyHelper.ClickElement("Edit/SaveSettingsButton");
            chyHelper.WaitForWorkAround(2000);
            chyHelper.VerifyPageText("Randy Jackson");
        }
        [TestMethod]
        public void TestSalesAgentCreateATicket()
        {
            LoginUser("username12", "password12");
            chyHelper = new ChyHelper(GetWebDriver(), "/SalesAgent2.xml");

            chyHelper.MouseHover("Navigate/TicketsTab");
            chyHelper.WaitForWorkAround(2000);
            chyHelper.ClickElement("Tickets/ClickCreateATicket");

            ticketSubject = "Test Ticket " + GetRandomNumber();
            chyHelper.TypeText("Tickets/EnterName", ticketSubject);

            chyHelper.ClickElement("Tickets/SelectClientButton");

            chyHelper.TypeText("Tickets/EnterIntoSearchByNameBox", "Chy Company");
            chyHelper.ClickElement("Tickets/ClickSearchButton");

            chyHelper.ClickElement("Tickets/ChooseClient");
            chyHelper.SelectByText("Tickets/SelectStatus", "Open");
            chyHelper.SelectByText("Tickets/SelectAssignedTo", "Howard Tang");
            chyHelper.ClickElement("Navigate/SaveButton");
            chyHelper.WaitForWorkAround(3000);
            chyHelper.VerifyAnyNodeWithText(true, ticketSubject);
        }
        [TestMethod]
        public void TestSalesAgentViewATicket()
        {
            TestSalesAgentCreateATicket();
            SetXPath("Tickets/ClickOnTicket", "<![CDATA[//a[text()='" + ticketSubject + "']]]>");
            chyHelper.WaitForWorkAround(3000);
            chyHelper.ClickElement("Tickets/ClickOnTicket");
            chyHelper.WaitForWorkAround(2000);
            chyHelper.VerifyPageText("View");
        }

        [TestMethod]
        public void TestSalesAgentCreateaTask()
        {
            LoginUser("username12", "password12");
            chyHelper = new ChyHelper(GetWebDriver(), "/SalesAgent2.xml");

            chyHelper.MouseHover("Navigate/ActivitiesTab");
            chyHelper.MouseHover("Navigate/TasksTab");
            chyHelper.ClickElement("Navigate/ClickCreateATask");

            string taskSubject = "Test Task " + GetRandomNumber();
            chyHelper.TypeText("CreateATask/EnterSubject", taskSubject);

            chyHelper.ClickElement("CreateATask/ClickStartDateBox");
            chyHelper.ClickElement("CreateATask/ChooseStartDate");

            chyHelper.ClickElement("CreateATask/ClickDueDateBox");
            chyHelper.ClickElement("CreateATask/ChooseDueDate");

            chyHelper.ClickElement("CreateATask/ClickSaveButton");

            chyHelper.MouseHover("Navigate/ActivitiesTab");
            chyHelper.ClickElement("Navigate/TasksTab");
            chyHelper.WaitForWorkAround(2000);
            chyHelper.VerifyPageText(taskSubject);
        }
        [TestMethod]
        public void TestSalesAgentCreateAMeeting()
        {
            LoginUser("username12", "password12");
            chyHelper = new ChyHelper(GetWebDriver(), "/SalesAgent2.xml");
            chyHelper.MouseHover("Navigate/ActivitiesTab");
            chyHelper.WaitForWorkAround(2000);
            chyHelper.MouseHover("Navigate/MeetingsTab");
            chyHelper.WaitForWorkAround(2000);
            chyHelper.ClickElement("Navigate/ClickCreateAMeeting");

            string meetingSubject = "Test Meeting " + GetRandomNumber();
            chyHelper.TypeText("CreateAMeeting/EnterSubject", meetingSubject);

            chyHelper.ClickElement("CreateAMeeting/ClickStartDateBox");
            chyHelper.ClickElement("CreateAMeeting/ChooseStartDate");

            chyHelper.ClickElement("CreateAMeeting/ClickEndDateBox");
            chyHelper.ClickElement("CreateAMeeting/ChooseEndDate");

            //Choose Related To field
            chyHelper.ClickElement("CreateAMeeting/SelectFromRelatedToBox");
            chyHelper.ClickElement("CreateAMeeting/SelectTypeClient");

            chyHelper.ClickElement("CreateAMeeting/ClickChooseClient");

            chyHelper.TypeText("CreateAMeeting/EnterIntoSearchByNameBox", "Chy Company");
            chyHelper.ClickElement("CreateAMeeting/ClickSearchButton");

            chyHelper.ClickElement("CreateAMeeting/ChooseClient");

            chyHelper.ClickElement("CreateAMeeting/ClickSaveButton");

            chyHelper.MouseHover("Navigate/ActivitiesTab");
            chyHelper.ClickElement("Navigate/MeetingsTab");
            chyHelper.WaitForWorkAround(2000);
            chyHelper.VerifyPageText(meetingSubject);
        }
        [TestMethod]
        public void TestSalesAgentSendEmail()
        {
            LoginUser("username12", "password12");
            chyHelper = new ChyHelper(GetWebDriver(), "/SalesAgent2.xml");
            string emailSubject = "Test Email" + GetRandomNumber();
            chyHelper.ClickElement("Navigate/ClientsTab");
            chyHelper.TypeText("Emails/SearchForCompanyClient", "Chy Company");
            chyHelper.WaitForWorkAround(2000);
            chyHelper.ClickElement("Emails/ChooseClient");
            chyHelper.ClickElement("Emails/SendEmailButton");
            chyHelper.TypeText("Emails/EnterSubject", emailSubject);
            chyHelper.ClickElement("Emails/SaveEmailButton");
            chyHelper.WaitForWorkAround(3000);
            chyHelper.VerifyPageText(emailSubject);

        }

        public void CreateOpportunity()
        {
            LoginUser("username12", "password12");
            chyHelper = new ChyHelper(GetWebDriver(), "/SalesAgent2.xml");

            companyName = "Test Company " + GetRandomNumber();

            Random rand = new Random();
            int nameInt = rand.Next(1, 1000);

            chyHelper.MouseHover("Navigate/OpportunitiesTab");
            chyHelper.ClickElement("Navigate/CreateAnOpportunityTab");

            chyHelper.TypeText("Opportunities/EnterOppName", "Test" + nameInt);
            chyHelper.TypeText("Opportunities/EnterCompanyName", companyName);


            chyHelper.SelectByText("Opportunities/SelectStatus", "New");
            chyHelper.SelectByText("Opportunities/SelectResponsibility", "Howard Tang");

            chyHelper.ClickElement("Navigate/SaveButton");

        }
        public void CreateLead()
        {
            LoginUser("username12", "password12");
            chyHelper = new ChyHelper(GetWebDriver(), "/SalesAgent2.xml");

            companyName = "Test Company " + GetRandomNumber();

            chyHelper.MouseHover("Navigate/LeadsTab");
            chyHelper.ClickElement("Leads/CreateALeadButton");

            Random rand = new Random();
            int nameInt = rand.Next(1, 1000);

            string newName = "Tester" + nameInt;
            chyHelper.MouseHover("Navigate/LeadsTab");
            chyHelper.ClickElement("Leads/CreateALeadButton");


            chyHelper.SelectByText("Leads/SelectStatus", "New");
            chyHelper.SelectByText("Leads/SelectResponsibility", "Howard Tang");

            chyHelper.ClickElement("Leads/CompanyDetailsTab");

            chyHelper.TypeText("Leads/EnterLastName", newName);
            chyHelper.TypeText("Leads/EnterFirstName", newName);
            chyHelper.TypeText("Leads/EnterCompanyName", companyName);
            chyHelper.ClickElement("Navigate/SaveButton");

        }

        public void CreateClient()
        {
            LoginUser("username12", "password12");
            chyHelper = new ChyHelper(GetWebDriver(), "/SalesAgent2.xml");

            companyName = "Test Company " + GetRandomNumber();

            chyHelper.MouseHover("Navigate/ClientsTab");
            chyHelper.ClickElement("Clients/CreateAClientButton");

            Random rand = new Random();
            int nameInt = rand.Next(1, 1000);

            string newName = "Tester" + nameInt;
            chyHelper.MouseHover("Navigate/ClientsTab");
            chyHelper.ClickElement("Clients/CreateAClientButton");


            chyHelper.SelectByText("Clients/SelectStatus", "New");
            chyHelper.SelectByText("Clients/SelectResponsibility", "Howard Tang");

            chyHelper.ClickElement("Clients/CompanyDetailsTab");

            //Need to enter legal name and DBA name
            chyHelper.TypeText("Clients/EnterCompanyDBAName", companyName);
            chyHelper.TypeText("Clients/EnterCompanyLegalName", newName);
            chyHelper.ClickElement("Navigate/SaveButton");

        }
        [TestMethod]
        public void TestSalesAgentEditOpportunity()
        {
            CreateOpportunity();
            chyHelper.WaitForWorkAround(2000);
            chyHelper.ClickElement("Navigate/OpportunitiesTab");
            SetXPath("Opportunities/ChooseOpp", "<![CDATA[//a[text()='" + companyName + "']]]>");
            chyHelper.ClickElement("Opportunities/ChooseOpp");
            chyHelper.ClickElement("Opportunities/EditButton");
            companyName = "Test Company " + GetRandomNumber();
            chyHelper.TypeText("Opportunities/EnterCompanyName", companyName);
            chyHelper.ClickElement("Navigate/SaveButton");
            chyHelper.WaitForWorkAround(2000);
            chyHelper.VerifyPageText(companyName);
        }
        [TestMethod]
        public void TestSalesAgentEditLead()
        {
            CreateLead();
            chyHelper.ClickElement("Navigate/LeadsTab");
            SetXPath("Leads/ChooseLead", "<![CDATA[//a[text()='" + companyName + "']]]>");
            chyHelper.ClickElement("Leads/ChooseLead");
            chyHelper.ClickElement("Navigate/CompanyDetailsTab");
            companyName = "Test Company " + GetRandomNumber();
            chyHelper.TypeText("Leads/EnterCompanyName", companyName);
            chyHelper.ClickElement("Navigate/SaveButton");
            chyHelper.ClickElement("Navigate/LeadsTab");
            chyHelper.WaitForWorkAround(2000);
            chyHelper.VerifyAnyNodeWithText(true, companyName);
        }
        [TestMethod]
        public void TestSalesAgentEditClient()
        {

            CreateClient();
            chyHelper.WaitForWorkAround(2000);
            chyHelper.ClickElement("Navigate/ClientsTab");
            SetXPath("Clients/ChooseClient", "<![CDATA[//a[text()='" + companyName + "']]]>");
            chyHelper.WaitForWorkAround(3000);
            chyHelper.TypeText("Search/SearchForCompanyClient", companyName);
            chyHelper.WaitForWorkAround(2000);
            chyHelper.ClickElement("Clients/ChooseClient");
            chyHelper.ClickElement("Navigate/CompanyDetailsTab");
            companyName = "Test Company " + GetRandomNumber();
            chyHelper.TypeText("Clients/EnterCompanyDBAName", companyName);
            chyHelper.ClickElement("Navigate/SaveButton");
            chyHelper.WaitForWorkAround(2000);
            chyHelper.ClickElement("Navigate/ClientsTab");
            chyHelper.WaitForWorkAround(2000);
            chyHelper.VerifyPageText(companyName);
        }
        [TestMethod]
        public void TestSalesAgentCancelEditOpportunity()
        {
            CreateOpportunity();
            chyHelper.ClickElement("Navigate/OpportunitiesTab");
            SetXPath("Opportunities/ChooseOpp", "<![CDATA[//a[text()='" + companyName + "']]]>");
            chyHelper.ClickElement("Opportunities/ChooseOpp");
            chyHelper.ClickElement("Opportunities/EditButton");
            companyName = "Test Company " + GetRandomNumber();
            chyHelper.TypeText("Opportunities/EnterCompanyName", companyName);
            chyHelper.ClickElement("Navigate/CancelButton");
            chyHelper.VerifyTrueOrFalse(false, companyName);
        }
        [TestMethod]
        public void TestSalesAgentCancelEditLead()
        {
            CreateLead();
            chyHelper.ClickElement("Navigate/LeadsTab");
            SetXPath("Leads/ChooseLead", "<![CDATA[//a[text()='" + companyName + "']]]>");
            chyHelper.ClickElement("Leads/ChooseLead");
            chyHelper.ClickElement("Navigate/CompanyDetailsTab");
            companyName = "Test Company " + GetRandomNumber();
            chyHelper.TypeText("Leads/EnterCompanyName", companyName);
            chyHelper.ClickElement("Navigate/CancelButton");
            chyHelper.ClickElement("Navigate/LeadsTab");
            chyHelper.VerifyTrueOrFalse(false, companyName);
        }
        [TestMethod]
        public void TestSalesAgentCancelEditClient()
        {
            CreateClient();
            chyHelper.ClickElement("Navigate/ClientsTab");
            SetXPath("Clients/ChooseClient", "<![CDATA[//a[text()='" + companyName + "']]]>");
            chyHelper.WaitForWorkAround(2000);
            chyHelper.TypeText("Search/SearchForCompanyClient", companyName);
            chyHelper.WaitForWorkAround(2000);
            chyHelper.ClickElement("Clients/ChooseClient");
            chyHelper.ClickElement("Navigate/CompanyDetailsTab");
            companyName = "Test Company " + GetRandomNumber();
            chyHelper.TypeText("Clients/EnterCompanyDBAName", companyName);
            chyHelper.ClickElement("Navigate/CancelButton");
            chyHelper.ClickElement("Navigate/ClientsTab");
            chyHelper.WaitForWorkAround(2000);
            chyHelper.VerifyTrueOrFalse(false, companyName);
        }
        [TestMethod]
        public void TestSalesAgentConvertOppToLead()
        {
            CreateOpportunity();
            chyHelper.ClickElement("Navigate/OpportunitiesTab");
            SetXPath("Opportunities/ChooseOpp", "<![CDATA[//a[text()='" + companyName + "']]]>");
            chyHelper.ClickElement("Opportunities/ChooseOpp");
            chyHelper.ClickElement("Navigate/ConvertButton");

            chyHelper.ClickElement("Opportunities/ConvertToLead");
            chyHelper.ClickElement("Opportunities/SaveConversion");
            chyHelper.ClickElement("Navigate/OpportunitiesTab");
            chyHelper.VerifyTrueOrFalse(false, companyName);

        }
        [TestMethod]
        public void TestSalesAgentConvertOppToClient()
        {
            CreateOpportunity();
            chyHelper.ClickElement("Navigate/OpportunitiesTab");
            SetXPath("Opportunities/ChooseOpp", "<![CDATA[//a[text()='" + companyName + "']]]>");
            chyHelper.ClickElement("Opportunities/ChooseOpp");
            chyHelper.ClickElement("Navigate/ConvertButton");

            chyHelper.ClickElement("Opportunities/ConvertToClient");
            chyHelper.ClickElement("Opportunities/SaveConversion");
            chyHelper.ClickElement("Navigate/OpportunitiesTab");
            chyHelper.VerifyTrueOrFalse(false, companyName);
        }
        [TestMethod]
        public void TestSalesAgentConvertLeadToClient()
        {
            CreateLead();
            chyHelper.WaitForWorkAround(2000);
            chyHelper.ClickElement("Navigate/LeadsTab");
            SetXPath("Leads/ChooseLead", "<![CDATA[//a[text()='" + companyName + "']]]>");
            chyHelper.ClickElement("Leads/ChooseLead");
            chyHelper.ClickElement("Navigate/ConvertButton");

            chyHelper.ClickElement("Leads/SaveConversion");
            chyHelper.ClickElement("Navigate/LeadsTab");
            chyHelper.VerifyTrueOrFalse(false, companyName);
        }
    }
}
   
