using System.Collections.Generic;
using System.Net;
using System.Threading;
using EasyHttp.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NewSkin.Util;

namespace NewSkin.Tests
{
    [TestClass]
    public class WebsiteApi : BaseTest
    {
        private LocatorReader _websiteApi;

        [TestInitialize]
        public void TestInitialize()
        {
            Browser = Pegasus.Login("seloffice");
            _websiteApi = new LocatorReader("WebsiteApi.xml");
        }

        [TestMethod]
        public void TestApiLinkExists()
        {
            GoToAdmin();
            Browser.MouseOver(_websiteApi.Get("integrations-menu"));
            Assert.IsTrue(Browser.ElementsVisible(_websiteApi.Get("api-codes-link")));
        }

        [TestMethod]
        public void TestApiLinkWorks()
        {
            Browser.ImplicitWait = 5;
            GoToAdmin();
            Browser.ImplicitWait = 5;
            Browser.MouseOver(_websiteApi.Get("integrations-menu"))
                .Click(_websiteApi.Get("api-codes-link"));
            Thread.Sleep(1000);
            Assert.AreEqual("API Codes", Browser.Title);
        }

        [TestMethod]
        public void TestCreateButtonWorks()
        {
            TestApiLinkWorks();
            Thread.Sleep(1000);
            Browser.Click(_websiteApi.Get("create-button"));
            Assert.IsTrue(Browser.ElementsVisible(_websiteApi.Get("api-keys-title")));
        }

        [TestMethod]
        public void TestRequiredFields()
        {
            TestCreateButtonWorks();
            Thread.Sleep(2000);
            Browser.Click(_websiteApi.Get("save-button"));
            Assert.IsTrue(Browser.ElementCount(_websiteApi.Get("required-message")) > 2);
        }

        [TestMethod]
        public void TestKeyGeneration()
        {
            TestCreateButtonWorks();
            Browser.ImplicitWait = 5;
            Browser.Click(_websiteApi.Get("generate-button"));
            Thread.Sleep(250);
            Assert.AreNotEqual("", Browser.FindElement(_websiteApi.Get("api-code-field")).GetAttribute("value"));
        }

        [TestMethod]
        public void TestCreateApiKey()
        {
            TestCreateButtonWorks();
            Browser.ImplicitWait = 5;
            Browser.Click(_websiteApi.Get("generate-button"))
                .FillForm(_websiteApi.Get("version-field"), "123")
                .DropdownSelectByText(_websiteApi.Get("status-field"), "New")
                .DropdownSelectByIndex(_websiteApi.Get("responsibility-field"), 1)
                .Click(_websiteApi.Get("save-button"));
            Assert.AreEqual("API Code saved successfully.",
                Browser.FindElement(Common.Get("flash-message")).Text);
        }

        [TestMethod]
        public void TestDuplicateKeyRejection()
        {
            TestCreateButtonWorks();
            Browser.ImplicitWait = 5;
            Browser.FillForm(_websiteApi.Get("api-code-field"), "duplicate")
                .FillForm(_websiteApi.Get("version-field"), "123")
                .DropdownSelectByText(_websiteApi.Get("status-field"), "New")
                .DropdownSelectByIndex(_websiteApi.Get("responsibility-field"), 1)
                .Click(_websiteApi.Get("save-button"));
            Assert.AreEqual("API Code is already exists.",
                Browser.FindElement(Common.Get("flash-message")).Text);
        }
    }

    /// <summary>
    ///     This is a separate test class for the test cases using POST requests.
    /// </summary>
    [TestClass]
    public class WebsiteApi2
    {
        private const string ApiEndpoint = "https://pegasus-test.com/apis/lead_create";
        private const string ApiKey = "N9mZgSy2gp9WFW78S3Bc";
        private HttpClient _client;

        [TestInitialize]
        public void TestInitialize()
        {
            _client = new HttpClient();
        }

        [TestMethod]
        public void TestApiEndpoint()
        {
            var res = _client.Post(ApiEndpoint, null, null);
            Assert.AreEqual(HttpStatusCode.OK, res.StatusCode);
        }

        [TestMethod]
        public void TestInvalidKey()
        {
            var res = _client.Post(ApiEndpoint, new Dictionary<string, object>
            {
                {"api_key", "invalid api key"}
            }, HttpContentTypes.ApplicationJson);

            var body = ((string) res.DynamicBody).Trim();
            Assert.AreEqual("Given API Code is Wrong", body);
        }

        [TestMethod]
        public void TestLeadCreate()
        {
            var res = _client.Post(ApiEndpoint, new Dictionary<string, object>
            {
                {"api_key", ApiKey},
                {"first_name", "Art"},
                {"last_name", "Vandelay"},
                {"company_name", "Vandelay Industries"},
                {"phone_number", "1234567890"},
                {"email", "art@vandelay.com"}
            }, HttpContentTypes.ApplicationJson);

            var body = ((string) res.DynamicBody).Trim();
            Assert.AreEqual("Lead created Successfully.", body);
        }

        [TestMethod]
        public void TestMissingFields()
        {
            var res = _client.Post(ApiEndpoint, new Dictionary<string, object>
            {
                {"api_key", ApiKey}
            }, HttpContentTypes.ApplicationJson);

            var body = ((string) res.DynamicBody).Trim();
            Assert.AreEqual("Lead not created successfully, First Name, Last Name, " +
                            "Company Name, Phone Number, eAddress  are empty.", body);
        }
    }
}