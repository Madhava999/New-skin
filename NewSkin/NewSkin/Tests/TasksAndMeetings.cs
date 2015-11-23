using Microsoft.VisualStudio.TestTools.UnitTesting;
using NewSkin.PageHelper;
using NewSkin.PageHelper.Comm;
using System;
using System.Xml;


   /*
    *User login can not be the same as the customizable dashboards tests login, need
    * home dashboard to always have meetings calendar and remain unchanged from default
    *
    */
namespace NewSkin.Tests
{
    [TestClass]
    public class TasksAndMeetings : DriverTestCase2
    {
        private ChyHelper chyHelper;
        private string taskSubject, meetingSubject;

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
        public void GoToAllTasks()
        {
            chyHelper.MouseHover("Navigate/ActivitiesTab");
            chyHelper.ClickElement("Navigate/TasksTab");
        }
        public void GoToAllMeetings()
        {
            chyHelper.MouseHover("Navigate/ActivitiesTab");
            chyHelper.ClickElement("Navigate/MeetingsTab");
        }
        //Method to set just made CDATA XPath in XML file
        public void SetXPath(string xmlNode, string xPathName)
        {


            XmlDocument xml = new XmlDocument();
            xml.Load("../../Locators/TasksAndMeetings.xml");
            //string xmlcontent = xml.InnerXml;
            xml.SelectSingleNode("//" + xmlNode).InnerXml = xPathName;
            //  string xmlcontent2 = xml.InnerXml;
            xml.Save("../../Locators/TasksAndMeetings.xml");

        }

        //method used to choose date exacty 1 day in the future and set new xml node value
        public void GetAndChangeFutureDate(string xmlNode)
        {


            int todaysDate = Int32.Parse(DateTime.Now.ToString("dd"));
            int futureDate = todaysDate + 1;

            string dateCData;
            if (todaysDate >= 28)
                dateCData = "<![CDATA[//td[@class='new day' and text()='1']]]>";

            else
                dateCData = "<![CDATA[//td[@class='day' and text()='" + futureDate + "']]]>";


            XmlDocument xml = new XmlDocument();
            xml.Load("../../Locators/TasksAndMeetings.xml");
            //string xmlcontent = xml.InnerXml;
            xml.SelectSingleNode("//" + xmlNode).InnerXml = dateCData;
            //  string xmlcontent2 = xml.InnerXml;
            xml.Save("../../Locators/TasksAndMeetings.xml");


        }
        //method used to choose date exactly one day in the past and set new xml node value
        public void GetAndChangePastDate(string xmlNode)
        {

            int todaysDate = Int32.Parse(DateTime.Now.ToString("dd"));
            int pastDate = todaysDate - 1;

            string dateCData;
            //Might not always work if today's date is the 1st and the days showing from the previous month don't contain the 30th
            if (todaysDate == 1)
                dateCData = "<![CDATA[//td[contains(@class,'old day') and text()='30']]]>";

            else
                dateCData = "<![CDATA[//td[@class='day' and text()='" + pastDate + "']]]>";


            XmlDocument xml = new XmlDocument();
            xml.Load("../../Locators/TasksAndMeetings.xml");
            //string xmlcontent = xml.InnerXml;
            xml.SelectSingleNode("//" + xmlNode).InnerXml = dateCData;
            //  string xmlcontent2 = xml.InnerXml;
            xml.Save("../../Locators/TasksAndMeetings.xml");

        }

        [TestMethod]
        public void ClickCreateATaskLink()
        {
            LoginUser("username9", "password9");
            chyHelper = new ChyHelper(GetWebDriver(), "/TasksAndMeetings.xml");
            chyHelper.MouseHover("Navigate/ActivitiesTab");
            chyHelper.MouseHover("Navigate/TasksTab");
            chyHelper.ClickElement("Navigate/ClickCreateATask");
            chyHelper.VerifyPageText("Create");
        }
        [TestMethod]
        public void CreateTaskValidateSubject()
        {
            ClickCreateATaskLink();

            chyHelper.ClickElement("CreateATask/ClickStartDateBox");
            chyHelper.WaitForWorkAround(2000);
            chyHelper.ClickElement("CreateATask/ChooseStartDate");

            chyHelper.ClickElement("CreateATask/ClickDueDateBox");
            chyHelper.WaitForWorkAround(2000);
            chyHelper.ClickElement("CreateATask/ChooseDueDate");

            chyHelper.ClickElement("CreateATask/ClickSaveButton");

            chyHelper.VerifyPageText("This field is required");
        }
        [TestMethod]
        public void CreateTaskValidateStartDate()
        {
            ClickCreateATaskLink();

            taskSubject = "Test Task " + GetRandomNumber();
            chyHelper.TypeText("CreateATask/EnterSubject", taskSubject);

            chyHelper.ClickElement("CreateATask/ClickDueDateBox");
            chyHelper.ClickElement("CreateATask/ChooseDueDate");

            chyHelper.ClickElement("CreateATask/ClickSaveButton");

            chyHelper.VerifyPageText("This field is required");

        }
        [TestMethod]
        public void CreateTaskValidateDueDate()
        {
            ClickCreateATaskLink();

            taskSubject = "Test Task " + GetRandomNumber();
            chyHelper.TypeText("CreateATask/EnterSubject", taskSubject);

            chyHelper.ClickElement("CreateATask/ClickStartDateBox");
            chyHelper.WaitForWorkAround(2000);
            chyHelper.ClickElement("CreateATask/ChooseStartDate");

            chyHelper.ClickElement("CreateATask/ClickSaveButton");

            chyHelper.VerifyPageText("This field is required");
        }
        [TestMethod]
        public void TestCreateaTask()
        {
            ClickCreateATaskLink();

            taskSubject = "Test Task " + GetRandomNumber();
            chyHelper.TypeText("CreateATask/EnterSubject", taskSubject);

            chyHelper.ClickElement("CreateATask/ClickStartDateBox");
            chyHelper.WaitForWorkAround(2000);
            chyHelper.ClickElement("CreateATask/ChooseStartDate");

            chyHelper.ClickElement("CreateATask/ClickDueDateBox");
            chyHelper.WaitForWorkAround(2000);
            chyHelper.ClickElement("CreateATask/ChooseDueDate");

            chyHelper.ClickElement("CreateATask/ClickSaveButton");

            GoToAllTasks();
            chyHelper.WaitForWorkAround(2000);
            chyHelper.VerifyPageText(taskSubject);
        }
        [TestMethod]
        public void CreateTaskCheckUpcomingTasks()
        {
            GetAndChangeFutureDate("CreateATask/ChooseFutureStartDate");
            GetAndChangeFutureDate("CreateATask/ChooseFutureDueDate");

            ClickCreateATaskLink();

            taskSubject = "Test Task " + GetRandomNumber();
            chyHelper.TypeText("CreateATask/EnterSubject", taskSubject);


            chyHelper.ClickElement("CreateATask/ClickStartDateBox");
            chyHelper.WaitForWorkAround(2000);
            //  chyHelper.ClickElement("Navigate/NextMonthButton");
            chyHelper.ClickElement("CreateATask/ChooseFutureStartDate");


            chyHelper.ClickElement("CreateATask/ClickDueDateBox");
            chyHelper.WaitForWorkAround(2000);
            //  chyHelper.ClickElement("Navigate/NextMonthButton");
            chyHelper.ClickElement("CreateATask/ChooseFutureDueDate");

            chyHelper.ClickElement("CreateATask/ClickSaveButton");

            chyHelper.WaitForWorkAround(2000);

            chyHelper.MouseHover("Navigate/ActivitiesTab");
            chyHelper.MouseHover("Navigate/TasksTab");

            chyHelper.WaitForWorkAround(2000);

            chyHelper.ClickElement("Navigate/ViewUpcomingTasks");
            chyHelper.WaitForWorkAround(2000);
            chyHelper.VerifyPageText(taskSubject);
        }
        [TestMethod]
        public void CreateTaskCheckPastDueTasks()
        {
            //Might not always work if today's date is the 1st and the days showing from the previous month don't contain the 30th
            GetAndChangePastDate("CreateATask/ChoosePastStartDate");
            GetAndChangePastDate("CreateATask/ChoosePastDueDate");

            ClickCreateATaskLink();

            taskSubject = "Test Task " + GetRandomNumber();
            chyHelper.TypeText("CreateATask/EnterSubject", taskSubject);


            chyHelper.ClickElement("CreateATask/ClickStartDateBox");
            chyHelper.WaitForWorkAround(2000);
            chyHelper.ClickElement("CreateATask/ChoosePastStartDate");


            chyHelper.ClickElement("CreateATask/ClickDueDateBox");
            chyHelper.WaitForWorkAround(2000);
            chyHelper.ClickElement("CreateATask/ChoosePastDueDate");

            chyHelper.ClickElement("CreateATask/ClickSaveButton");

            chyHelper.WaitForWorkAround(2000);

            chyHelper.MouseHover("Navigate/ActivitiesTab");
            chyHelper.MouseHover("Navigate/TasksTab");

            chyHelper.WaitForWorkAround(2000);

            chyHelper.ClickElement("Navigate/ViewPastDueTasks");
            chyHelper.WaitForWorkAround(2000);
            chyHelper.VerifyPageText(taskSubject);
        }

        [TestMethod]
        public void ClickCreateAMeetingLink()
        {
            LoginUser("username9", "password9");
            chyHelper = new ChyHelper(GetWebDriver(), "/TasksAndMeetings.xml");
            chyHelper.MouseHover("Navigate/ActivitiesTab");
            chyHelper.MouseHover("Navigate/MeetingsTab");
            chyHelper.ClickElement("Navigate/ClickCreateAMeeting");
            chyHelper.VerifyPageText("Create");
        }
        [TestMethod]
        public void CreateMeetingValidateSubject()
        {
            ClickCreateAMeetingLink();

            chyHelper.ClickElement("CreateAMeeting/ClickStartDateBox");
            chyHelper.WaitForWorkAround(2000);
            chyHelper.ClickElement("CreateAMeeting/ChooseStartDate");


            chyHelper.ClickElement("CreateAMeeting/ClickEndDateBox");
            chyHelper.WaitForWorkAround(2000);
            chyHelper.ClickElement("CreateAMeeting/ChooseEndDate");

            //Choose Related To field
            chyHelper.ClickElement("CreateAMeeting/SelectFromRelatedToBox");
            chyHelper.ClickElement("CreateAMeeting/SelectTypeClient");

            chyHelper.ClickElement("CreateAMeeting/ClickChooseClient");

            chyHelper.TypeText("CreateAMeeting/EnterIntoSearchByNameBox", "Chy Company");
            chyHelper.ClickElement("CreateAMeeting/ClickSearchButton");
            
            chyHelper.ClickElement("CreateAMeeting/ChooseClient");

            chyHelper.ClickElement("CreateAMeeting/ClickSaveButton");

            chyHelper.VerifyPageText("This field is required");
        }
        [TestMethod]
        public void CreateMeetingValidateStartDate()
        {
            ClickCreateAMeetingLink();

            meetingSubject = "Test Meeting " + GetRandomNumber();
            chyHelper.TypeText("CreateAMeeting/EnterSubject", meetingSubject);

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

            chyHelper.VerifyPageText("This field is required");

        }
        //Can't test due date because it automatically populates
        /*
        [TestMethod]
        public void CreateMeetingValidateDueDate()
        {
            ClickCreateAMeetingLink();

            meetingSubject = "Test Meeting " + GetRandomNumber();
            chyHelper.TypeText("CreateAMeeting/EnterSubject", meetingSubject);

            chyHelper.ClickElement("CreateAMeeting/ClickStartDateBox");
            chyHelper.ClickElement("CreateAMeeting/ChooseStartDate");

            //Choose Related To field
            chyHelper.ClickElement("CreateAMeeting/SelectFromRelatedToBox");
            chyHelper.ClickElement("CreateAMeeting/SelectTypeClient");

            chyHelper.ClickElement("CreateAMeeting/ClickChooseClient");
            chyHelper.ClickElement("CreateAMeeting/ChooseClient");


            chyHelper.ClickElement("CreateAMeeting/ClickSaveButton");

            chyHelper.VerifyPageText("This field is required");
        }
        */
        [TestMethod]
        public void CreateMeetingValidateRelatedTo()
        {
            ClickCreateAMeetingLink();

            meetingSubject = "Test Meeting " + GetRandomNumber();
            chyHelper.TypeText("CreateAMeeting/EnterSubject", meetingSubject);

            chyHelper.ClickElement("CreateAMeeting/ClickStartDateBox");
            chyHelper.WaitForWorkAround(2000);
            chyHelper.ClickElement("CreateAMeeting/ChooseStartDate");

            chyHelper.ClickElement("CreateAMeeting/ClickEndDateBox");
            chyHelper.WaitForWorkAround(2000);
            chyHelper.ClickElement("CreateAMeeting/ChooseEndDate");

            chyHelper.ClickElement("CreateAMeeting/ClickSaveButton");

            chyHelper.VerifyPageText("This field is required");
        }
        [TestMethod]
        public void TestCreateAMeeting()
        {
            ClickCreateAMeetingLink();

            meetingSubject = "Test Meeting " + GetRandomNumber();
            chyHelper.TypeText("CreateAMeeting/EnterSubject", meetingSubject);

            chyHelper.ClickElement("CreateAMeeting/ClickStartDateBox");
            chyHelper.WaitForWorkAround(2000);
            chyHelper.ClickElement("CreateAMeeting/ChooseStartDate");

            chyHelper.ClickElement("CreateAMeeting/ClickEndDateBox");
            chyHelper.WaitForWorkAround(2000);
            chyHelper.ClickElement("CreateAMeeting/ChooseEndDate");

            //Choose Related To field
            chyHelper.ClickElement("CreateAMeeting/SelectFromRelatedToBox");
            chyHelper.ClickElement("CreateAMeeting/SelectTypeClient");

            chyHelper.ClickElement("CreateAMeeting/ClickChooseClient");
            chyHelper.TypeText("CreateAMeeting/EnterIntoSearchByNameBox", "Chy Company");
            chyHelper.ClickElement("CreateAMeeting/ClickSearchButton");
            chyHelper.ClickElement("CreateAMeeting/ChooseClient");

            chyHelper.ClickElement("CreateAMeeting/ClickSaveButton");

            GoToAllMeetings();
            chyHelper.WaitForWorkAround(2000);
            chyHelper.VerifyPageText(meetingSubject);
        }
        [TestMethod]
        public void TestMeetingRelatedToAutoFill()
        {
            LoginUser("username9", "password9");
            chyHelper = new ChyHelper(GetWebDriver(), "/TasksAndMeetings.xml");
            chyHelper.ClickElement("Navigate/ClickClientsTab");
            chyHelper.ClickElement("CreateAMeeting/ChooseClient");
            chyHelper.ClickElement("CreateAMeeting/ClickNewMeeting");
            chyHelper.VerifyNodeValue("CreateAMeeting/VerifyRelatedToValue", "Chy Company");
        }
        [TestMethod]
        public void TestMeetingReminderSaved()
        {
            ClickCreateAMeetingLink();
         

            meetingSubject = "Test Meeting " + GetRandomNumber();
            chyHelper.TypeText("CreateAMeeting/EnterSubject", meetingSubject);

            chyHelper.ClickElement("CreateAMeeting/ClickStartDateBox");
            chyHelper.WaitForWorkAround(2000);
            chyHelper.ClickElement("CreateAMeeting/ChooseStartDate");

            chyHelper.ClickElement("CreateAMeeting/ClickEndDateBox");
            chyHelper.WaitForWorkAround(2000);
            chyHelper.ClickElement("CreateAMeeting/ChooseEndDate");

            //Set Reminders
            chyHelper.ClickElement("CreateAMeeting/ClickPopupReminder");
            chyHelper.ClickElement("CreateAMeeting/ClickPopupReminderSelectionBox");
            chyHelper.ClickElement("CreateAMeeting/SelectPopupTime");

            chyHelper.ClickElement("CreateAMeeting/ClickEmailReminder");
            chyHelper.ClickElement("CreateAMeeting/ClickEmailReminderSelectionBox");
            chyHelper.ClickElement("CreateAMeeting/SelectEmailTime");
         
            //Choose Related To field
            chyHelper.ClickElement("CreateAMeeting/SelectFromRelatedToBox");
            chyHelper.ClickElement("CreateAMeeting/SelectTypeClient");

            chyHelper.ClickElement("CreateAMeeting/ClickChooseClient");
            chyHelper.ClickElement("CreateAMeeting/ChooseClient");

            chyHelper.ClickElement("CreateAMeeting/ClickSaveButton");

            GoToAllMeetings();
     
            //Check reminders
            SetXPath("CreateAMeeting/SelectMeeting", "<![CDATA[//*[@title='" + meetingSubject + "']]]>");
            chyHelper.ClickElement("CreateAMeeting/SelectMeeting");



            chyHelper.VerifyPageText("Popup 2 hours prior");
            chyHelper.VerifyPageText("E-Mail 2 hours prior");
        }
        [TestMethod]
        public void CheckCalendarForMeeting()
        {
            /*
            int todaysDateDay = Int32.Parse(DateTime.Now.ToString("dd"));
            SetXPath("CreateAMeeting/ChooseTodaysDate", "<![CDATA[//td[text()='" + todaysDateDay + "']]]>");

            string wholeDate = DateTime.Now.ToString("yyyy-mm-dd");
            SetXPath("CreateAMeeting/ViewCalendarMeetings", "<![CDATA[//td[contains(@data-date,'"+ wholeDate + "')]]]>");
            */
            LoginUser("username9", "password9");
            chyHelper = new ChyHelper(GetWebDriver(), "/TasksAndMeetings.xml");

            chyHelper.MouseHover("Navigate/ActivitiesTab");
            chyHelper.MouseHover("Navigate/MeetingsTab");
            chyHelper.ClickElement("Navigate/ClickCreateAMeeting");
  
            meetingSubject = "Test Meeting " + GetRandomNumber();
            chyHelper.TypeText("CreateAMeeting/EnterSubject", meetingSubject);

            chyHelper.ClickElement("CreateAMeeting/ClickStartDateBox");
            chyHelper.WaitForWorkAround(2000);
            chyHelper.ClickElement("CreateAMeeting/ChooseFirstCalendarDay");

            //Choose Related To field
            chyHelper.ClickElement("CreateAMeeting/SelectFromRelatedToBox");
            chyHelper.ClickElement("CreateAMeeting/SelectTypeClient");

            chyHelper.ClickElement("CreateAMeeting/ClickChooseClient");
            chyHelper.ClickElement("CreateAMeeting/ChooseClient");

            chyHelper.ClickElement("CreateAMeeting/ClickSaveButton");

            //chyHelper.ClickElement("Navigate/HomeButton");

            chyHelper.ClickElement("CreateAMeeting/ViewCalendarMeetings");

            chyHelper.VerifyPageText(meetingSubject);
        }
        [TestMethod]
        public void CheckCalendarForTask()
        {
            LoginUser("username9", "password9");
            chyHelper = new ChyHelper(GetWebDriver(), "/TasksAndMeetings.xml");

            chyHelper.MouseHover("Navigate/ActivitiesTab");
            chyHelper.MouseHover("Navigate/TasksTab");
            chyHelper.ClickElement("Navigate/ClickCreateATask");

            taskSubject = "Test task " + GetRandomNumber();
            chyHelper.TypeText("CreateATask/EnterSubject", taskSubject);

            chyHelper.ClickElement("CreateATask/ClickStartDateBox");
            chyHelper.WaitForWorkAround(2000);
            chyHelper.ClickElement("CreateATask/ChooseFirstCalendarStartDay");


            chyHelper.ClickElement("CreateATask/ClickDueDateBox");
            chyHelper.ClickElement("CreateATask/ChooseFirstCalendarEndDay");
            

            chyHelper.ClickElement("CreateATask/ClickSaveButton");

            //chyHelper.ClickElement("Navigate/HomeButton");

            chyHelper.ClickElement("CreateATask/ViewCalendarTasks");

            chyHelper.VerifyAnyNodeWithText(true,taskSubject);
        }
    }
}
