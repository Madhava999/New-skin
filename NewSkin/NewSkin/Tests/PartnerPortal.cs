using Microsoft.VisualStudio.TestTools.UnitTesting;
using NewSkin.PageHelper;
using NewSkin.PageHelper.Comm;
using System;
using System.IO;
using System.Xml;

namespace NewSkin.Tests
{
    [TestClass]
    public class PartnerPortal : DriverTestCase2
    {
        private ChyHelper chyHelper;
        private string leadCompanyName,userName;
        

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
            xml.Load("../../Locators/PartnerPortal.xml");
            //string xmlcontent = xml.InnerXml;
            xml.SelectSingleNode("//" + xmlNode).InnerXml = xPathName;
            //  string xmlcontent2 = xml.InnerXml;
            xml.Save("../../Locators/PartnerPortal.xml");

        }
        public void SetXPath2(string xmlNode, string xPathName)
        {


            XmlDocument xml = new XmlDocument();
            xml.Load("../../Config/ApplicationSettings.xml");
            //string xmlcontent = xml.InnerXml;
            xml.SelectSingleNode("//" + xmlNode).InnerXml = xPathName;
            //  string xmlcontent2 = xml.InnerXml;
            xml.Save("../../Config/ApplicationSettings.xml");

        }
        
        public void AdminPermanentDeleteLead()
        {
            
            chyHelper.ClickElement("Navigate/LeadsTab");
            chyHelper.ClickElement("AdminSection/CheckboxFirstCompany");
            chyHelper.ClickElement("AdminSection/DeleteButton");
            chyHelper.AcceptAlert();
            chyHelper.ClickElement("AdminSection/RecycleBinLeads");
            chyHelper.ClickElement("AdminSection/DeleteLeadPermanentlyButton");
            chyHelper.AcceptAlert();
            chyHelper.WaitForWorkAround(2000);
        }
        public void AdminPermanentDeleteClient()
        {
            chyHelper.ClickElement("Navigate/ClientsTab");
            chyHelper.ClickElement("AdminSection/CheckboxFirstCompany");
            chyHelper.ClickElement("AdminSection/DeleteButton");
            chyHelper.AcceptAlert();
            chyHelper.AcceptAlert();
            chyHelper.ClickElement("AdminSection/RecycleBinClients");
            chyHelper.ClickElement("AdminSection/DeleteClientPermanentlyButton");
            chyHelper.AcceptAlert();
            chyHelper.WaitForWorkAround(2000);
        }
        public void AdminConvertLeadToClient()
        {
            chyHelper.ClickElement("Navigate/LeadsTab");
            SetXPath("AdminSection/ClickOnLead", "<![CDATA[//a[text()='"+leadCompanyName+"']]]>");
            chyHelper.WaitForWorkAround(2000);
            chyHelper.ClickElement("AdminSection/ClickOnLead");
            chyHelper.WaitForWorkAround(2000);
            chyHelper.ClickElement("AdminSection/ConvertButton");
            chyHelper.WaitForWorkAround(2000);
            chyHelper.ClickElement("AdminSection/PopupSaveButton");
            chyHelper.WaitForWorkAround(2000);
        }
     
        /// <summary>
        ///     Partner should be able to create a new Lead
        ///</summary>
        ///<Logins>
        ///     username10: mmatthew12
        ///     password10: mmatthew12
        /// </Logins>
        ///<preconditions>
        ///     1) Need "Howard Tang"as user to be selected as responsibility
        ///     2) Need "Web Site"as source option
        ///</preconditions>
        [TestMethod]
        public void TestPartnerCreateALead()
        {
            LoginUser("username10", "password10");
            chyHelper = new ChyHelper(GetWebDriver(), "/PartnerPortal.xml");

            leadCompanyName= "Test Company " + GetRandomNumber();
            Random rand = new Random();
            int nameInt = rand.Next(1, 1000);

            string newName = "TestName" + nameInt;
            chyHelper.WaitForWorkAround(2000);
            chyHelper.ClickElement("Navigate/LeadsTab");
            chyHelper.WaitForWorkAround(2000);
            chyHelper.ClickElement("CreateALead/CreateALeadButton");

            chyHelper.TypeText("CreateALead/EnterFirstName",newName);
            chyHelper.TypeText("CreateALead/EnterLastName",newName);
            chyHelper.TypeText("CreateALead/EnterCompanyName",leadCompanyName);

            chyHelper.SelectByText("CreateALead/SelectSource","Web Site");
            chyHelper.SelectByText("CreateALead/SelectResponsibility","Howard Tang");

            chyHelper.ClickElement("Navigate/SaveButton");
            chyHelper.WaitForWorkAround(2000);
            chyHelper.VerifyNodeTextTrue("CreateALead/CheckLeadsTable",leadCompanyName);
        }
        /// <summary>
        ///     Partner should not be able to save new Lead if required field is missing
        ///</summary>
        ///<Logins>
        ///     username10: mmatthew12
        ///     password10: mmatthew12
        /// </Logins>
        ///<preconditions>
        ///     --------
        ///</preconditions>
        [TestMethod]
        public void TestPartnerLeadMissingField()
        {
            LoginUser("username10", "password10");
            chyHelper = new ChyHelper(GetWebDriver(), "/PartnerPortal.xml");

            leadCompanyName = "Test Company " + GetRandomNumber();
            Random rand = new Random();
            int nameInt = rand.Next(1, 1000);

            string newName = "TestName" + nameInt;

            chyHelper.ClickElement("Navigate/LeadsTab");
            chyHelper.ClickElement("CreateALead/CreateALeadButton");

            chyHelper.TypeText("CreateALead/EnterFirstName", newName);
            chyHelper.TypeText("CreateALead/EnterLastName", newName);
            chyHelper.TypeText("CreateALead/EnterCompanyName", leadCompanyName);

            chyHelper.SelectByText("CreateALead/SelectSource", "Web Site");
          //  chyHelper.Select("CreateALead/SelectResponsibility", "601");

            chyHelper.ClickElement("Navigate/SaveButton");
            chyHelper.VerifyTrueOrFalse(true,"This field is required");
        }
    
        /// <summary>
        ///     Partner should be able to cancel creating new lead and none of the 
        ///     information be saved
        ///</summary>
        ///<Logins>
        ///     username10: mmatthew12
        ///     password10: mmatthew12
        /// </Logins>
        ///<preconditions>
        ///     --------
        ///</preconditions>
        [TestMethod]
        public void TestPartnerCreateLeadCancel()
        {
            LoginUser("username10", "password10");
            chyHelper = new ChyHelper(GetWebDriver(), "/PartnerPortal.xml");

            leadCompanyName = "Test Company " + GetRandomNumber();
            Random rand = new Random();
            int nameInt = rand.Next(1, 1000);

            string newName = "TestName" + nameInt;

            chyHelper.ClickElement("Navigate/LeadsTab");
            chyHelper.ClickElement("CreateALead/CreateALeadButton");

            chyHelper.TypeText("CreateALead/EnterFirstName", newName);
            chyHelper.TypeText("CreateALead/EnterLastName", newName);
            chyHelper.TypeText("CreateALead/EnterCompanyName", leadCompanyName);

            chyHelper.SelectByText("CreateALead/SelectSource", "Web Site");
            //  chyHelper.Select("CreateALead/SelectResponsibility", "601");

            chyHelper.ClickElement("Navigate/CancelButton");
            chyHelper.WaitForWorkAround(3000);
            chyHelper.VerifyNodeTextFalse("CreateALead/CheckLeadsTable", leadCompanyName);
        }
        
        /// <summary>
        ///     Partner should be able to edit leads and save new changes
        ///</summary>
        ///<Logins>
        ///     username10: mmatthew12
        ///     password10: mmatthew12
        ///     username9: newthemeoffice
        ///     password9: seWelcome2
        /// </Logins>
        ///<preconditions>
        ///     1) Need "Howard Tang"as user to be selected as responsibility
        ///     2) Need "Web Site"as source option
        ///</preconditions>
        [TestMethod]
        public void TestPartnerEditLead()
        {
            TestPartnerCreateALead();

            chyHelper.ClickElement("Navigate/PencilEditButton");

            chyHelper.TypeText("CreateALead/EnterFirstName", "ChangedName");

            chyHelper.ClickElement("Navigate/SaveButton");
            chyHelper.VerifyTrueOrFalse(true,"ChangedName");

            //Logout and login into the office to delete lead so future tests aren''t messed up
            Logout();
            LoginUser("username9", "password9");
            AdminPermanentDeleteLead();
        }
        
        /// <summary>
        ///     Partner should be able to view lead they just created in Assignments table
        ///     on home page
        ///</summary>
        ///<Logins>
        ///     username10: mmatthew12
        ///     password10: mmatthew12
        /// </Logins>
        ///<preconditions>
        ///     1) Need "Howard Tang"as user to be selected as responsibility
        ///     2) Need "Web Site"as source option
        ///</preconditions>
        [TestMethod]
        public void TestPartnerViewLead()
        {
            TestPartnerCreateALead();

            chyHelper.ClickElement("Navigate/HomeTab");
            chyHelper.WaitForWorkAround(2000);
            chyHelper.VerifyNodeTextTrue("CreateALead/CheckLeadMainPage", leadCompanyName);
        }
      
        /// <summary>
        ///     Partner should be able to see leads that were converted to a client in their
        ///     assignments table as type Client for one entry and if "Move to recycle bin"
        ///      was chosen the lead should no longer show up on Leads page 
        ///</summary>
        ///<Logins>
        ///     username10: mmatthew12
        ///     password10: mmatthew12
        ///     username9: newthemeoffice
        ///     password9: seWelcome2
        /// </Logins>
        ///<preconditions>
        ///     1) Need "Howard Tang"as user to be selected as responsibility
        ///     2) Need "Web Site"as source option
        ///</preconditions>
        [TestMethod]
        public void TestPartnerViewConvertedLead()
        {
            TestPartnerCreateALead();
            Logout();

            LoginUser("username9", "password9");
            AdminConvertLeadToClient();
            Logout();

            LoginUser("username10", "password10");
            chyHelper.WaitForWorkAround(2000);
            chyHelper.VerifyNodeTextTrue("CreateALead/CheckClientMainPage", "Clients");

            chyHelper.ClickElement("Navigate/LeadsTab");
            chyHelper.WaitForWorkAround(3000);
            chyHelper.VerifyNodeTextFalse("CreateALead/CheckLeadsTable", leadCompanyName);
            
            Logout();
            LoginUser("username9", "password9");
            AdminPermanentDeleteClient();
        }
        
        /// <summary>
        ///     Partner should be able to edit their own contact information
        ///</summary>
        ///<Logins>
        ///     username10: mmatthew12
        ///     password10: mmatthew12
        /// </Logins>
        ///<preconditions>
        ///    -----
        /// </preconditions>
        [TestMethod]
        public void TestPartnerEditInfo()
        {
         
            LoginUser("username10", "password10");
            chyHelper = new ChyHelper(GetWebDriver(), "/PartnerPortal.xml");

            Random rand = new Random();
            int nameInt = rand.Next(1, 1000);

            string newName = "TestName" + nameInt;
            chyHelper.ClickElement("Edit/EditButton");
            chyHelper.TypeText("Edit/EnterAgentFirstName",newName);
            chyHelper.ClickElement("Edit/SaveButton");

            chyHelper.VerifyTrueOrFalse(true,newName);

            //Now change name back for other testing purposes
            chyHelper.ClickElement("Edit/EditButton");
            chyHelper.TypeText("Edit/EnterAgentFirstName", "Mark");
            chyHelper.ClickElement("Edit/SaveButton");

            chyHelper.VerifyTrueOrFalse(true, "Mark");


        }
        
        /// <summary>
        ///     Partner should be able to view their payouts
        ///</summary>
        ///<Logins>
        ///     username10: mmatthew12
        ///     password10: mmatthew12
        ///     username8: newthemecorp
        ///     password8: seWelcome2
        ///     username9: newthemeoffice
        ///     password9: seWelcome2
        /// </Logins>
        ///<preconditions>
        ///    1) Residual file "SelOfficePartnerResiduals"file should be in Resources folder
        ///    2) Partner agent Mark Matthews should have agent revenue share code 1234
        ///    3) Chy processor should be added as processor option
        ///    4) Payout lookup settings should be set to individual lookup
        /// </preconditions>
        
        
        /// <summary>
        ///     Partner should be able to view lead that their name was added to
        ///</summary>
        ///<Logins>
        ///     username10: mmatthew12
        ///     password10: mmatthew12
        ///     username9: newthemeoffice
        ///     password9: seWelcome2
        /// </Logins>
        ///<preconditions>
        ///    1) Partner agent with name Mark Matthews and above login should be 
        ///     present in office
        ///     2) Howard  Tang should be available as user with abover username9 login
        /// </preconditions>
        [TestMethod]
        public void TestPartnerViewAdminAddedLead()
        {
            //Office logs in
            LoginUser("username9", "password9");
            chyHelper = new ChyHelper(GetWebDriver(), "/PartnerPortal.xml");

            leadCompanyName = "Test Company " + GetRandomNumber();
            Random rand = new Random();
            int nameInt = rand.Next(1, 1000);

            string newName = "Tester" + nameInt;
            chyHelper.MouseHover("Navigate/LeadsTab");
            chyHelper.WaitForWorkAround(2000);
            chyHelper.ClickElement("AdminSection/CreateALeadLink");
            chyHelper.WaitForWorkAround(2000);

            chyHelper.SelectByText("AdminSection/SelectStatus", "New");
            chyHelper.SelectByText("AdminSection/SelectResponsibility", "Howard Tang");
            chyHelper.SelectByText("AdminSection/SelectPartner", "Mark Matthews");

            chyHelper.ClickElement("AdminSection/CompanyDetailsTab");

            chyHelper.TypeText("CreateALead/EnterLastName", newName);
            chyHelper.TypeText("AdminSection/EnterFirstName", newName);
            chyHelper.TypeText("AdminSection/EnterCompanyName", leadCompanyName);
            chyHelper.ClickElement("Navigate/SaveButton");

            Logout();
            LoginUser("username10","password10");
            chyHelper.ImplicitWait(50);
            chyHelper.VerifyNodeTextTrue("CreateALead/CheckLeadMainPage", leadCompanyName);
        }
        
        /// <summary>
        ///     Partner should not be able to view lead that their name is taken off of
        ///</summary>
        ///<Logins>
        ///     username10: mmatthew12
        ///     password10: mmatthew12
        ///     username9: newthemeoffice
        ///     password9: seWelcome2
        /// </Logins>
        ///<preconditions>
        ///     1) Partner agent with name Fake Partner should be present in office (login
        ///        doens't matter)
        ///     2) Howard  Tang should be available as user with abover username9 login
        ///     3) Partner agent with name Mark Matthews and above login should be 
        ///     present in office
        /// </preconditions>
        [TestMethod]
        public void TestPartnerAdminRemovedLead()
        {
            TestPartnerViewAdminAddedLead();
            chyHelper.WaitForWorkAround(2000);
            Logout();
            LoginUser("username9", "password9");
            SetXPath("AdminSection/ClickOnLead", "<![CDATA[//a[text()='" + leadCompanyName + "']]]>");

            chyHelper.ClickElement("Navigate/LeadsTab");
            chyHelper.ClickElement("AdminSection/ClickOnLead");

            chyHelper.ClickElement("AdminSection/AssignmentsAndTemplatesTab");
            chyHelper.SelectByText("AdminSection/SelectPartner", "Fake Partner");
            chyHelper.ClickElement("Navigate/SaveButton");

            Logout();
            LoginUser("username10", "password10");
            chyHelper.WaitForWorkAround(2000);
            chyHelper.VerifyNodeTextFalse("CreateALead/CheckLeadMainPage", leadCompanyName);
        }
    
        /// <summary>
        ///     Partner should not be able to log into Pegasus if their account has been made
        ///     inactive/been disabled
        ///</summary>
        ///<Logins>
        ///     username9: newthemeoffice
        ///     password9: seWelcome2
        /// </Logins>
        ///<preconditions>   
        ///     1) Howard  Tang should be available as user admin with above login
        /// </preconditions>
        [TestMethod]
        public void TestPartnerAccountDiasbled()
        {

            TestPartnerAccountCreated();
            //Office logs back in to disable user
            Logout();
            LoginUser("username9", "password9");

            chyHelper.MouseHover("Navigate/UserIcon");
            chyHelper.ClickElement("Navigate/AdminTab");

            chyHelper.MouseHover("Navigate/OfficeTab");
            chyHelper.ClickElement("Navigate/UsersTab");

            SetXPath("CreateUser/ClickPartnerAgentName", "<![CDATA[//a[text()='" + userName+" "+userName + "']]]>");
            chyHelper.ClickElement("CreateUser/ClickPartnerAgentName");

            chyHelper.ClickElement("CreateUser/DisableButton1");
            chyHelper.ClickElement("CreateUser/DisableButton2");

            Logout();
            LoginUser("username55", "password55");
            chyHelper.WaitForWorkAround(3000);

            chyHelper.VerifyTrueOrFalse(true,"Sorry your account is not active");
        }
       
        /// <summary>
        ///     Partner should be able to log in after partner user account is created
        ///</summary>
        ///<Logins>
        ///     username9: newthemeoffice
        ///     password9: seWelcome2
        /// </Logins>
        ///<preconditions>   
        ///     1) Howard  Tang should be available as user admin with above login
        /// </preconditions>
        [TestMethod]
        public void TestPartnerAccountCreated()
        {
            Random rand = new Random();
            int nameInt = rand.Next(1, 10000);
            userName = "Tester" + nameInt;


            //Office logs in
            LoginUser("username9", "password9");
            chyHelper = new ChyHelper(GetWebDriver(), "/PartnerPortal.xml");
           
            chyHelper.MouseHover("Navigate/UserIcon");
            chyHelper.WaitForWorkAround(1500);
            chyHelper.ClickElement("Navigate/AdminTab");

            chyHelper.WaitForWorkAround(1500);
            chyHelper.MouseHover("Navigate/OfficeTab");
            chyHelper.WaitForWorkAround(1500);
            chyHelper.ClickElement("Navigate/UsersTab");
            chyHelper.WaitForWorkAround(1500);
            chyHelper.ClickElement("CreateUser/CreateUserButton");
            chyHelper.SelectByText("CreateUser/SelectUserType", "Partner Agent");
            chyHelper.ClickElement("CreateUser/SelectCreateNew");

            chyHelper.TypeText("CreateUser/EnterFirstName", userName);
            chyHelper.TypeText("CreateUser/EnterLastName", userName);
            chyHelper.TypeText("CreateUser/EnterEAddress", "fakeaddress@email.com");
          
            chyHelper.ClickElement("CreateUser/UncheckAutoGen");
            chyHelper.ClickElement("CreateUser/CheckDoNotSend");
            chyHelper.ClickElement("CreateUser/CheckPartnerUser");

            chyHelper.TypeText("CreateUser/EnterUsername", userName);
            chyHelper.TypeText("CreateUser/EnterPassword", userName);


            chyHelper.ClickElement("Navigate/SaveButton");

            Logout();
            SetXPath2("NewSiteCredentials/username55",userName );
            SetXPath2("NewSiteCredentials/password55", userName);
            LoginUser("username55", "password55");
            chyHelper.WaitForWorkAround(3000);

            chyHelper.VerifyTrueOrFalse(true,"Partner Agents");

        }
       
        /// <summary>
        ///     Lead edited to have both a partner agent and a partner association should 
        ///     show up in both the agent's and association's leads page
        ///</summary>
        ///<Logins>
        ///     username10: mmatthew12
        ///     password10: mmatthew12
        ///     username9: newthemeoffice
        ///     password9: seWelcome2
        ///     username11: slane12
        ///     password11:slane12
        /// </Logins>
        ///<preconditions>
        ///     1) Partner association with name Same Lane and above login should be present
        ///      in office 
        ///     2) Howard  Tang should be available as admin user with abover username9 login
        ///     3) Partner agent with name Mark Matthews and above login should be 
        ///     present in office
        /// </preconditions>
        [TestMethod]
        public void TestPartnerAssocViewAdminAddedLead()
        {
            Random rand = new Random();
            int nameInt = rand.Next(1, 10000);
            userName = "Tester" + nameInt;

            //Office logs in
            LoginUser("username9", "password9");
            chyHelper = new ChyHelper(GetWebDriver(), "/PartnerPortal.xml");

            leadCompanyName = "Test Company " + GetRandomNumber();

            chyHelper.MouseHover("Navigate/LeadsTab");
            chyHelper.WaitForWorkAround(2000);
            chyHelper.ClickElement("AdminSection/CreateALeadLink");


            chyHelper.SelectByText("AdminSection/SelectStatus", "New");
            chyHelper.SelectByText("AdminSection/SelectResponsibility", "Howard Tang");
            chyHelper.SelectByText("AdminSection/SelectPartner", "Mark Matthews");
            chyHelper.SelectByText("AdminSection/SelectPartnerAssociation", "Sam Lane");

            chyHelper.ClickElement("AdminSection/CompanyDetailsTab");

            chyHelper.TypeText("AdminSection/EnterFirstName", userName);
            chyHelper.TypeText("AdminSection/EnterLastName", userName);
            chyHelper.TypeText("AdminSection/EnterCompanyName", leadCompanyName);
            chyHelper.ClickElement("Navigate/SaveButton");

            Logout();
            LoginUser("username10", "password10");
            chyHelper.WaitForWorkAround(3000);
            chyHelper.VerifyNodeTextTrue("CreateALead/CheckLeadMainPage", leadCompanyName);

            Logout();
            LoginUser("username11", "password11");
            chyHelper.WaitForWorkAround(3000);
            chyHelper.VerifyNodeTextTrue("CreateALead/CheckLeadMainPage", leadCompanyName);
        }

    }
}
