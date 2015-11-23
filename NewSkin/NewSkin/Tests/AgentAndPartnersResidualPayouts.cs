using Microsoft.VisualStudio.TestTools.UnitTesting;
using NewSkin.PageHelper;
using NewSkin.PageHelper.Comm;
using System;
using System.IO;

namespace NewSkin.Tests
{
    [TestClass]
    public class AgentAndPartnersResidualPayouts : DriverTestCase2
    {
        private ChyHelper chyHelper;

        //Setup residuals
        [ClassInitialize]
        public static void setup(TestContext context)
        {
            AgentAndPartnersResidualPayouts s = new AgentAndPartnersResidualPayouts();
            s.SetupTest();
            //Import from corporate
            s.LoginUser("username8", "password8");
            s.chyHelper = new ChyHelper(s.GetWebDriver(), "/AgentAndPartnerResidualPayouts.xml");

            s.chyHelper.MouseHover("ResidualIncome/ResidualIncomeTab");
            s.chyHelper.ClickElement("ResidualIncome/ImportsTabOption");
            s.chyHelper.ClickElement("ResidualIncome/ImportNewButton");

            s.chyHelper.SelectByText("ResidualIncome/SelectProcessor", "Chy Processor");
            s.chyHelper.Select("ResidualIncome/SelectReportingMonth", "12");
            s.chyHelper.Select("ResidualIncome/SelectReportingYear", "2016");
            s.chyHelper.WaitForWorkAround(3000);
            s.chyHelper.ClickElement("ResidualIncome/FileDateBox");
            // s.chyHelper.WaitForWorkAround(3000);
            s.chyHelper.ClickElement("ResidualIncome/ChooseFileDate");
            s.chyHelper.Upload("ResidualIncome/UploadResidualFile", Path.GetFullPath("../../Resources/SelOfficePartnerResiduals.csv"));
            s.chyHelper.ClickElement("ResidualIncome/ImportButton");

            s.chyHelper.ClickElement("ResidualIncome/ProcessButton");

            //Calculate residual in corporate
            s.chyHelper.ClickElement("ResidualIncome/CalculationWizardButtonCorp");
            s.chyHelper.ClickElement("ResidualIncome/SkipStep1");
            s.chyHelper.WaitForWorkAround(2000);
            s.chyHelper.ClickElement("ResidualIncome/CalculatePayoutsButtonCorp");
            s.chyHelper.WaitForWorkAround(2000);
            s.chyHelper.ClickElement("ResidualIncome/PublishPayoutsButton");
            s.chyHelper.WaitForWorkAround(3000);
            s.Logout();

            //calculate residual in office for agents
            s.LoginUser("username9", "password9");
            s.chyHelper.MouseHover("ResidualIncome/ResidualIncomeTab");
            s.chyHelper.MouseHover("ResidualIncome/OfficePayoutsTab");
            s.chyHelper.WaitForWorkAround(2000);
            s.chyHelper.ClickElement("ResidualIncome/PayoutsSummaryTabOption");

            s.chyHelper.ClickElement("ResidualIncome/CalculationWizardButtonOffice");
            s.chyHelper.ClickElement("ResidualIncome/AgentLookupButton");
            s.chyHelper.WaitForWorkAround(3000);
            s.chyHelper.ClickElement("ResidualIncome/CalculatePayoutsButtonOffice");
            s.chyHelper.WaitForWorkAround(3000);
            s.chyHelper.ClickElement("ResidualIncome/PublishPayoutsButtonOffice");
            s.chyHelper.WaitForWorkAround(5000);

            s.GetWebDriver().Quit();
        }
        //clears all shared files after all tests
        [ClassCleanup]
        public static void cleanup()
        {
            AgentAndPartnersResidualPayouts s = new AgentAndPartnersResidualPayouts();
            s.SetupTest();
            s.LoginUser("username8", "password8");
            s.chyHelper = new ChyHelper(s.GetWebDriver(), "/AgentAndPartnerResidualPayouts.xml");

            s.chyHelper.MouseHover("ResidualIncome/ResidualIncomeTab");
            s.chyHelper.ClickElement("ResidualIncome/ImportsTabOption");
            s.chyHelper.ClickElement("ResidualIncome/DeleteFirstResidualFile");
            s.chyHelper.AcceptAlert();
            s.chyHelper.WaitForWorkAround(3000);
            s.chyHelper.VerifyTrueOrFalse(false, "PartnerResiduals");

            s.GetWebDriver().Quit();
        }
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
        [TestMethod]
        public void TestSalesAgentViewPayoutSummary()
        {

            chyHelper = new ChyHelper(GetWebDriver(), "/AgentAndPartnerResidualPayouts.xml");
            LoginUser("username12", "password12");
            chyHelper.MouseHover("Payouts/ResidualIncomeTab");
            chyHelper.MouseHover("Payouts/UserPayoutsTab");
            chyHelper.WaitForWorkAround(2000);
            chyHelper.ClickElement("Payouts/PayoutsSummaryTab");
            chyHelper.WaitForWorkAround(3000);
            chyHelper.ClickElement("Payouts/SpecificPayoutSummary2");

            chyHelper.VerifyAnyNodeWithText(true,"Chy Company");


        }

        [TestMethod]
        public void TestSalesAgentViewPayoutReport()
        {
            chyHelper = new ChyHelper(GetWebDriver(), "/AgentAndPartnerResidualPayouts.xml");
            LoginUser("username12", "password12");

            chyHelper.MouseHover("Payouts/ResidualIncomeTab");
            chyHelper.MouseHover("Payouts/UserPayoutsTab");
            chyHelper.ClickElement("Payouts/PayoutsReportsTab");
            chyHelper.WaitForWorkAround(2000);
            chyHelper.VerifyAnyNodeWithText(true,"Chy Company");


        }
        [TestMethod]
        public void TestPartnerViewPayoutSummary()
        {
            chyHelper = new ChyHelper(GetWebDriver(), "/AgentAndPartnerResidualPayouts.xml");
            LoginUser("username10", "password10");

            chyHelper.ClickElement("Payouts/PayoutsSummaryTab");
            chyHelper.WaitForWorkAround(2000);
            chyHelper.ClickElement("Payouts/SpecificPayoutSummary1");

            chyHelper.VerifyAnyNodeWithText(true, "Chy Company");


        }

        /// <summary>
        ///     Partner should be able to view specific payout reports
        ///</summary>
        ///<Logins>
        ///     username10: mmatthew12
        ///     password10: mmatthew12
        /// </Logins>
        ///<preconditions>
        ///    1) Residual file "SelOfficePartnerResiduals"file should be in Resources folder
        ///    2) Partner agent Mark Matthews should have agent revenue share code 1234
        ///    3) Chy processor should be added as processor option
        ///    4) Payout lookup settings should be set to individual lookup
        /// </preconditions>
        [TestMethod]
        public void TestPartnerViewPayoutReport()
        {
            chyHelper = new ChyHelper(GetWebDriver(), "/AgentAndPartnerResidualPayouts.xml");
            LoginUser("username10", "password10");

            chyHelper.ClickElement("Payouts/PayoutsReportsTab");
            chyHelper.WaitForWorkAround(2000);
            chyHelper.VerifyAnyNodeWithText(true, "Chy Company");



        }
    }

}