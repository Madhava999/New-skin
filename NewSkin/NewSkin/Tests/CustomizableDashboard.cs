using Microsoft.VisualStudio.TestTools.UnitTesting;
using NewSkin.PageHelper;
using NewSkin.PageHelper.Comm;
using System;
/*
 *Class Preconditions: 
 * 1) Module can not be ran on same accounts as TasksAndMeetings module
 *    since this module changes the dashboard homepage and the TasksAndMeetings module checks
 *    the calendar on the default dashboard homepage
 * 2) Need two separate user accounts under the same office to run tests 
 */


namespace NewSkin.Tests
{
    [TestClass]
    public class CustomizableDashboards : DriverTestCase2
    {
        private ChyHelper chyHelper;
        private string dashName;
        
        //Reusable method to login with admin credentials
        public void LoginUser(string user,string pass)
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
        //Reusable method to click the "All Dashboards" link
        public void ClickAllDashboards()
        {
            chyHelper = new ChyHelper(GetWebDriver(), "/CustomizableDashboards.xml");


            chyHelper.MouseHover("AllDashboards/HoverOverReportTab");
            chyHelper.WaitForWorkAround(2000);
            chyHelper.MouseHover("AllDashboards/HoverOverDashboardsTab");
            chyHelper.WaitForWorkAround(2000);
            chyHelper.ClickElement("AllDashboards/ClickOnAllDashboards");
        }
        //Reusable method to click the "Create a Dashboard" link
        public void ClickCreateADashboard()
        {
            chyHelper = new ChyHelper(GetWebDriver(), "/CustomizableDashboards.xml");

            chyHelper.MouseHover("CreateDashboard/HoverOverReportTab");

            chyHelper.MouseHover("CreateDashboard/HoverOverDashboardsTab");
            chyHelper.WaitForWorkAround(2000);
            chyHelper.ClickElement("CreateDashboard/ClickOnCreateADashboard");

        }
        /// <summary>
        ///     Admin/agent can click on an already made dashboard and preview it 
        ///</summary>
        ///<Logins>
        ///     username13: admin13
        ///     password13: admin13
        /// </Logins>
        ///<preconditions>
        ///     1) Need at least one dashboard already made
        ///</preconditions>
        [TestMethod]
        public void ViewDashboard ()
        {

            LoginUser("username13","password13");
            ClickAllDashboards();
            chyHelper.WaitForWorkAround(2000);
            chyHelper.ClickElement("ViewDashboard/ClickOnSingleDashboard2");
            chyHelper.WaitForWorkAround(3000);
            chyHelper.VerifyPageText("Edit");

            
        }


        ///<Summary>
        ///     Admin/agent can edit an already existent dashboard by clicking the pencil 
        ///</Summary>
        ///<Logins>
        ///     username13: admin13
        ///     password13: admin13
        /// </Logins>
        ///<Preconditions>
        ///     1) Need a clients, leads, and calls report already made 
        ///</Preconditions>

        [TestMethod]
        public void PencilEditDashboard()
        {
            CreateDashboardWithReports();
            ClickAllDashboards();

            chyHelper.ClickElement("EditDashboard/ClickPencil");
            //Make changes to dashboard

            chyHelper.ClickElement("DashletSelections/SelectTeamClientDashlet");


            //Go to next page 
            chyHelper.ClickElement("Navigate/ClickNextButton");

           //Save dashboard
           chyHelper.ClickElement("Navigate/ClickSaveButton");

            //Verify Text on final page to make sure you are viewing the dashboard
            chyHelper.WaitForWorkAround(4000);
            chyHelper.VerifyPageText("My Team's Clients");

        }
        ///<summary>
        ///     Admin/agent can edit an already existing dashboard by clicking
        ///      the "Edit" button on the view dashboard page
        /// </summary>
        ///<Logins>
        ///     username13: admin13
        ///     password13: admin13
        /// </Logins>
        ///<Preconditions>
        ///     1) Need a clients, leads, and calls report already made 
        ///</Preconditions>
        [TestMethod]
        public void ButtonEditDashboard()
        {
            CreateDashboardWithReports();
 


            chyHelper.ClickElement("EditDashboard/ClickEditButton");


            chyHelper.ClickElement("DashletSelections/SelectTeamClientDashlet");


            chyHelper.ClickElement("Navigate/ClickNextButton");

            chyHelper.ClickElement("Navigate/ClickSaveButton");

            chyHelper.WaitForWorkAround(4000);

            chyHelper.VerifyPageText("My Team's Clients");

        }
        ///<summary>
        ///     Admin/agent should be able to create a dashborad
        ///      with an already created Leads/Clients/Calls report
        /// </summary>
        ///<Logins>
        ///     username13: admin13
        ///     password13: admin13
        /// </Logins>
        ///<Preconditions>
        ///     1)Need a clients, leads, and calls report already made 
        ///</Preconditions>

        [TestMethod]
        public void CreateDashboardWithReports()
        {

            LoginUser("username13", "password13");
            ClickCreateADashboard();

            dashName="Test Dash "+ GetRandomNumber();
            chyHelper.TypeText("CreateDashboard/EnterName", dashName);

            chyHelper.ClickElement("Reports/SelectReport1");
            chyHelper.ClickElement("Reports/SelectReport2");
            chyHelper.ClickElement("Reports/SelectReport3");

            chyHelper.ClickElement("Navigate/ClickNextButton");

            chyHelper.ClickElement("Navigate/ClickSaveButton");

            chyHelper.VerifyPageText("Report");

        }
        ///<summary>
        ///      Admin/agent should be able to set a new dashboard as your home
        ///      page by clicking the link at the top when viewing it
        /// </summary>
        ///<Logins>
        ///     username13: admin13
        ///     password13: admin13
        /// </Logins>
        ///<Preconditions>
        ///     1)Need 1 report already made 
        ///</Preconditions>

        [TestMethod]
        public void SetDashHomePageLink()
        {
            //no need to login since this methoid logs in already
            CreateDashWithButton();

            chyHelper.ClickElement("Homepage/ClickSetHomepageLink");
            //chyHelper.VerifyPageText("Dashboard is successfully changed");
            chyHelper.WaitForWorkAround(2000);
            chyHelper.VerifyPageText("custom_widget_heading");

        }
        ///<summary>
        ///      Admin/agent should be able to set a new dashboard as 
        ///      their home page by making the selection when creating it
        /// </summary>
        ///<Logins>
        ///     username13: admin13
        ///     password13: admin13
        /// </Logins>
        ///<Preconditions>
        ///     1)Need 1 report already made 
        ///</Preconditions>

        [TestMethod]
        public void CreateDashSetHomePage()
        {
            LoginUser("username13", "password13");
            ClickCreateADashboard();

            dashName = "Test Dash " + GetRandomNumber();
            chyHelper.TypeText("CreateDashboard/EnterName", dashName);

            chyHelper.ClickElement("Reports/SelectReport1");

            chyHelper.ClickElement("Homepage/ClickSetHomepageCheckBox");

            chyHelper.ClickElement("Navigate/ClickNextButton");

            chyHelper.ClickElement("Navigate/ClickSaveButton");

     

            chyHelper.VerifyPageText("Remove this Dashboard as My Home Page");
        }
        ///<summary>
        ///      Admin/agent should be able to remove a dashboard as their home page by 
        ///      clicking the link while viewing the dashboard
        /// </summary>
        ///<Logins>
        ///     username13: admin13
        ///     password13: admin13
        /// </Logins>
        ///<Preconditions>
        ///     1)Need 1 report already made 
        ///</Preconditions>
      
        [TestMethod]
        public void RemoveDashboardHomepageLink()
        {
            //First create new dash and set as home page
            CreateDashSetHomePage();

            //Go back to All Dashboards page
            ClickAllDashboards();

            //Then pick first dash on page (which will be one we just created and remove as homepage
            chyHelper.ClickElement("ViewDashboard/ClickOnSingleDashboard1");
            chyHelper.ClickElement("Homepage/RemoveAsHomepageLink");

            chyHelper.VerifyPageText("Dashboard is successfully changed");

        }
        ///<summary>
        ///      Admin/agent should be able to 
        ///      set dashboard as default from "All Dashboards" page
        /// </summary>
        ///<Logins>
        ///     username13: admin13
        ///     password13: admin13
        /// </Logins>
        ///<Preconditions>
        ///     1)Need 1 report already made 
        ///</Preconditions>

        [TestMethod]
        public void MakeDashDefaultButton()
        {
            CreateDashWithButton();
            ClickAllDashboards();
            chyHelper.ClickElement("Homepage/ClickMakeDefaultButton");

            chyHelper.VerifyPageText("Dashboard is successfully changed");
        }
        ///<summary>
        ///      Should be able to create dashboard with 
        ///      different selections under Opportunities data
        /// </summary>
        ///<Logins>
        ///     username13: admin13
        ///     password13: admin13
        /// </Logins>
        ///<Preconditions>
        ///     --------
        ///</Preconditions>
        
        [TestMethod]
        public void CreateDashboardWithOpportunities()
        {
            LoginUser("username13", "password13");
            ClickCreateADashboard();

            dashName = "Test Dash " + GetRandomNumber();
            chyHelper.TypeText("CreateDashboard/EnterName", dashName);

            chyHelper.ClickElement("DashletSelections/ClickOtherDashletsMenu");

            chyHelper.ClickElement("DashletSelections/SelectOpportunityDashlet1");
            chyHelper.ClickElement("DashletSelections/SelectOpportunityDashlet2");
            chyHelper.ClickElement("DashletSelections/SelectOpportunityDashlet3");

            chyHelper.ClickElement("Navigate/ClickNextButton");

            chyHelper.ClickElement("Navigate/ClickSaveButton");

            chyHelper.VerifyPageText("Edit");
            chyHelper.VerifyPageText("All Opportunities");
        }
        ///<summary>
        ///      Admin/agent should be able to create dashboard 
        ///      with different selections under Leads data
        /// </summary>
        ///<Logins>
        ///     username13: admin13
        ///     password13: admin13
        /// </Logins>
        ///<Preconditions>
        ///     --------
        ///</Preconditions>
       
        [TestMethod]
        public void CreateDashboardWithLeads()
        {
            LoginUser("username13", "password13");
            ClickCreateADashboard();

           dashName = "Test Dash " + GetRandomNumber();
            chyHelper.TypeText("CreateDashboard/EnterName", dashName);

            chyHelper.ClickElement("DashletSelections/ClickOtherDashletsMenu");

            chyHelper.ClickElement("DashletSelections/SelectLeadsDashlet1");
            chyHelper.ClickElement("DashletSelections/SelectLeadsDashlet2");
            chyHelper.ClickElement("DashletSelections/SelectLeadsDashlet3");

            chyHelper.ClickElement("Navigate/ClickNextButton");

            chyHelper.ClickElement("Navigate/ClickSaveButton");

            chyHelper.VerifyPageText("Edit");
            chyHelper.VerifyPageText("All Leads");
        }
        ///<summary>
        ///      Admin/agent should be able to create dashboard with
        ///      different selections under Clients data
        /// </summary>
        ///<Logins>
        ///     username13: admin13
        ///     password13: admin13
        /// </Logins>
        ///<Preconditions>
        ///     --------
        ///</Preconditions>
      
        [TestMethod]
        public void CreateDashboardWithClients()
        {
            LoginUser("username13", "password13");
            ClickCreateADashboard();

            dashName = "Test Dash " + GetRandomNumber();
            chyHelper.TypeText("CreateDashboard/EnterName", dashName);

            chyHelper.ClickElement("DashletSelections/ClickOtherDashletsMenu");

            chyHelper.ClickElement("DashletSelections/SelectClientsDashlet1");
            chyHelper.ClickElement("DashletSelections/SelectClientsDashlet2");
            chyHelper.ClickElement("DashletSelections/SelectClientsDashlet3");

            chyHelper.ClickElement("Navigate/ClickNextButton");

            chyHelper.ClickElement("Navigate/ClickSaveButton");

            chyHelper.VerifyPageText("Edit");
            chyHelper.VerifyPageText("All Clients");
        }
        ///<summary>
        ///      Admin/agent should be able to create 
        ///     dashboard with different selections under Activites data
        /// </summary>
        ///<Logins>
        ///     username13: admin13
        ///     password13: admin13
        /// </Logins>
        ///<Preconditions>
        ///     --------
        ///</Preconditions>
        [TestMethod]
        public void CreateDashboardWithActivities()
        {
            LoginUser("username13", "password13");
            ClickCreateADashboard();

             dashName = "Test Dash " + GetRandomNumber();
            chyHelper.TypeText("CreateDashboard/EnterName", dashName);

            chyHelper.ClickElement("DashletSelections/ClickOtherDashletsMenu");

            chyHelper.ClickElement("DashletSelections/SelectActivitiesDashlet1");
            chyHelper.ClickElement("DashletSelections/SelectActivitiesDashlet2");
            chyHelper.ClickElement("DashletSelections/SelectActivitiesDashlet3");

            chyHelper.ClickElement("Navigate/ClickNextButton");

            chyHelper.ClickElement("Navigate/ClickSaveButton");

            chyHelper.VerifyPageText("Edit");
            chyHelper.VerifyPageText("Upcoming");
        }
        ///<summary>
        ///      Admin/agent should be able to set permissions for dashboard for
        ///     other users when creating a new dashboard
        /// </summary>
        ///<Logins>
        ///     username13: admin13
        ///     password13: admin13
        ///     username14: admin14
        ///     password14: admin14
        /// </Logins>
        ///<Preconditions>
        ///     1) Need 1 Report Already Made
        ///</Preconditions>

        [TestMethod]
        public void CreateDashSetPermissions()
        {
            LoginUser("username13", "password13");
            ClickCreateADashboard();

             dashName = "Test Dash " + GetRandomNumber();
            chyHelper.TypeText("CreateDashboard/EnterName", dashName);

            chyHelper.ClickElement("Reports/SelectReport1");

            chyHelper.ClickElement("PermissionSelections/ClickPermissionsMenu");

            chyHelper.ClickElement("PermissionSelections/DeselectPerm6");
            chyHelper.ClickElement("PermissionSelections/DeselectPerm7");
            chyHelper.ClickElement("PermissionSelections/DeselectPerm9");
            chyHelper.ClickElement("PermissionSelections/DeselectPerm10");
            chyHelper.ClickElement("PermissionSelections/DeselectPerm12");
            chyHelper.ClickElement("PermissionSelections/DeselectPerm13");
            chyHelper.ClickElement("PermissionSelections/DeselectPerm15");
            chyHelper.ClickElement("PermissionSelections/DeselectPerm16");

            chyHelper.ClickElement("Navigate/ClickNextButton");

            chyHelper.ClickElement("Navigate/ClickSaveButton");

            chyHelper.VerifyPageText("Edit");
            Logout();

            //Login as other user to see if you can view dashboard

            LoginUser("username14", "password14");
            ClickAllDashboards();
            chyHelper.VerifyTrueOrFalse(false,dashName);
        }
        ///<summary>
        ///      Admin/agent should be able to set permissions for dashboard for
        ///     other users when editing a dashboard
        /// </summary>
        ///<Logins>
        ///     username13: admin13
        ///     password13: admin13
        ///     username14: admin14
        ///     password14: admin14
        /// </Logins>
        ///<Preconditions>
        ///     1) Need a leads, clients, and calls report already made
        ///</Preconditions>
        
        [TestMethod]
        public void EditDashSetPermissions()
        {
    
            //Create new dash 
            CreateDashboardWithReports();


            //Go and edit dashboard
            ClickAllDashboards();
            chyHelper.ClickElement("ViewDashboard/ClickOnSingleDashboard1");
            chyHelper.ClickElement("EditDashboard/ClickEditButton");


            chyHelper.ClickElement("PermissionSelections/ClickPermissionsMenu");

            chyHelper.ClickElement("PermissionSelections/DeselectPerm6");
            chyHelper.ClickElement("PermissionSelections/DeselectPerm7");
            chyHelper.ClickElement("PermissionSelections/DeselectPerm9");
            chyHelper.ClickElement("PermissionSelections/DeselectPerm10");
            chyHelper.ClickElement("PermissionSelections/DeselectPerm12");
            chyHelper.ClickElement("PermissionSelections/DeselectPerm13");
            chyHelper.ClickElement("PermissionSelections/DeselectPerm15");
            chyHelper.ClickElement("PermissionSelections/DeselectPerm16");

            chyHelper.ClickElement("Navigate/ClickNextButton");

            chyHelper.ClickElement("Navigate/ClickSaveButton");

            chyHelper.VerifyPageText("Edit");
            Logout();

            //Login as other user to see if you can view dashboard
            LoginUser("username14", "password14");
            ClickAllDashboards();
            chyHelper.VerifyTrueOrFalse(false, dashName);
        }
        ///<summary>
        ///     Admin/agent should not be able to delete dashboard that is 
        ///     currently being used as someone's home page
        /// </summary>
        ///<Logins>
        ///     username13: admin13
        ///     password13: admin13
        /// </Logins>
        ///<Preconditions>
        ///     1) Need Reports already made
        ///</Preconditions>
       
        [TestMethod]
        public void TestDeleteDash()
        {
            CreateDashSetHomePage();
            ClickAllDashboards();

            chyHelper.ClickElement("Delete/ClickTrashCanButton1");
            chyHelper.AcceptAlert();
            chyHelper.VerifyPageText(dashName);
        }

        ///<summary>
        ///     Admin/agent should be able to cancel creating a new dashboard
        /// </summary>
        ///<Logins>
        ///     username13: admin13
        ///     password13: admin13
        /// </Logins>
        ///<Preconditions>
        ///     1) Need 1 Report already made
        ///</Preconditions>
        
        [TestMethod]
        public void CancelCreateDashboard()
        {

            LoginUser("username13", "password13");
            ClickCreateADashboard();

            dashName = "Test Dash " + GetRandomNumber();
            chyHelper.TypeText("CreateDashboard/EnterName", dashName);

            chyHelper.ClickElement("Reports/SelectReport1");

            chyHelper.ClickElement("Navigate/ClickNextButton");

            chyHelper.ClickElement("Navigate/ClickCancelButton");

            chyHelper.VerifyTrueOrFalse(false,dashName);

        }
        ///<summary>
        ///     Admin/agent should be able to cancel editing a dashboard
        /// </summary>
        ///<Logins>
        ///     username13: admin13
        ///     password13: admin13
        /// </Logins>
        ///<Preconditions>
        ///     1) Need leads, clients, and calls Report already made
        ///</Preconditions>
        
        [TestMethod]
        public void CancelEditDashboard()
        {

            CreateDashboardWithReports();
            ClickAllDashboards();

            chyHelper.ClickElement("EditDashboard/ClickPencil");
            //Make changes to dashboard

            chyHelper.ClickElement("DashletSelections/SelectTeamClientDashlet");


            //Go to next page 
            chyHelper.ClickElement("Navigate/ClickNextButton");

            //Save dashboard
            chyHelper.ClickElement("Navigate/ClickCancelButton");

            chyHelper.ClickElement("ViewDashboard/ClickOnSingleDashboard1");

            //Verify Text on final page to make sure you are viewing the dashboard
            chyHelper.VerifyTrueOrFalse(false,"My Team's Clients");

        }
        ///<summary>
        ///      User should be able to create dashboard from "All Dashboards" page
        /// </summary>
        ///<Logins>
        ///     username13: admin13
        ///     password13: admin13
        /// </Logins>
        ///<Preconditions>
        ///     1) Need 1 Report already made
        ///</Preconditions>
        
        [TestMethod]
        public void CreateDashWithButton()
        {

            LoginUser("username13", "password13");
            ClickAllDashboards();

            chyHelper.ClickElement("CreateDashboard/ClickCreateButton");


            dashName = "Test Dash " + GetRandomNumber();
            chyHelper.TypeText("CreateDashboard/EnterName", dashName);

            chyHelper.ClickElement("Reports/SelectReport1");

            chyHelper.ClickElement("Navigate/ClickNextButton");

            chyHelper.ClickElement("Navigate/ClickSaveButton");

            chyHelper.VerifyPageText(dashName);

        }

        ///<summary>
        ///      Dashboard made default should remain as 
        ///     dashboard after logging out and then logging back in
        /// </summary>
        ///<Logins>
        ///     username13: admin13
        ///     password13: admin13
        /// </Logins>
        ///<Preconditions>
        ///     1) Need Reports already made
        ///</Preconditions>
       
        [TestMethod]
        public void LogOutDashSaved()
        {

            CreateDashSetHomePage();
            // TeardownTest();
            Logout();
            //Login
           // SetupTest();
            LoginUser("username13", "password13");
            chyHelper.WaitForWorkAround(3000);
            chyHelper.VerifyPageText("custom_widget_heading");

        }
    }
}