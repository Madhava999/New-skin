using System;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NewSkin.Util;
using OpenQA.Selenium.Support.UI;

namespace NewSkin.Tests
{
    [TestClass]
    public class UniversalResidual : BaseTest
    {
        private static LocatorReader _r;

        [TestInitialize]
        public void TestInitialize()
        {
            _r = new LocatorReader("UniversalResidual.xml");
            Browser = Pegasus.Login("selcorp");
        }

        [TestMethod]
        public void TestViewProcessorList()
        {
            Browser.ImplicitWait = 10;
            Browser.MouseOver(_r, "master-data-menu")
                .Click(_r, "processors-link");
            Thread.Sleep(2000);
            Assert.AreEqual("Master Processors", Browser.Title);
        }

        [TestMethod]
        public void TestCreateProcessor()
        {
            var processorName = "Processor " + new Random().Next(int.MaxValue);

            TestViewProcessorList();
            Browser.Click(_r, "processors.create-button")
                .FillForm(_r, "processors.name-field", processorName)
                .FillForm(_r, "processors.code-field", "123");

            Browser.Click(_r, "processors.save-button");

            Assert.AreEqual("Processor is successfully created!!",
                Browser.FindElement(Common, "flash-message").Text);

            TestContext.Properties["ProcessorName"] = processorName;
        }

        [TestMethod]
        public void TestEditProcessor()
        {
            TestCreateProcessor();
            Thread.Sleep(2500);

            Browser.Click(_r.Get("processors.edit-button",
                TestContext.Properties["ProcessorName"]))
                .FillForm(_r, "processors.name-field", " Edited")
                .Click(_r, "processors.save-button");

            Thread.Sleep(2500);

            Assert.AreEqual("Processor is successfully updated!!",
                Browser.FindElement(Common, "flash-message").Text);
        }

        [TestMethod]
        public void TestDeleteProcessor()
        {
            TestCreateProcessor();
            Thread.Sleep(2500);

            Browser.Click(_r.Get("processors.delete-button",
                TestContext.Properties["ProcessorName"]));
            Thread.Sleep(1000);
            Browser.AlertAccept();

            Thread.Sleep(2500);

            Assert.AreEqual("The processor is successfully deleted!!",
                Browser.FindElement(Common, "flash-message").Text);
        }

        [TestMethod]
        public void TestProcessorValidation()
        {
            TestViewProcessorList();
            Browser.Click(_r, "processors.create-button")
                .Click(_r, "processors.save-button");

            Assert.AreEqual(2, Browser.FindElements(_r,
                "processors.validation-label").Count);
        }

        private void GoToResiduals()
        {
            Browser.MouseOver(_r, "residual-income-menu")
                .Click(_r, "residual-import-link");
        }

        private void GoToResidualsImport()
        {
            GoToResiduals();
            Browser.Click(_r, "residuals.import-button");
        }

        private void FillOutImportFields()
        {
            var oldWait = Browser.ImplicitWait;
            Browser.ImplicitWait = 5;

            var dropdown = new SelectElement(Browser.FindElement(_r, "residuals.import.processor"));
            dropdown.SelectByIndex(1); // Wil fail if there are no processors.
            //            Browser.DropdownSelectByText(_r, "residuals.import.processor", "Vantiv")
            Browser.Click(_r, "residuals.import.file-date");

            Thread.Sleep(2000);

            var days = Browser.FindElements(_r, "residuals.import.calendar-days");
            days[new Random().Next(days.Count)].Click();

            Browser.ImplicitWait = oldWait;
        }

        [TestMethod]
        public void TestInvalidFile()
        {
            GoToResidualsImport();
            FillOutImportFields();

            Browser.UploadFile(_r, "residuals.import.file", "emptyfile");
            Browser.Click(_r, "residuals.import.import-button");

            Assert.AreEqual("Please upload csv files only", Browser.AlertText);
        }

//        [TestMethod]
//        public void TestFakeCsv()
//        {
//            GoToResidualsImport();
//            FillOutImportFields();
//
//            Browser.UploadFile(_r, "residuals.import.file", "fake.csv");
//            Browser.Click(_r, "residuals.import.import-button");
//
//            Assert.AreEqual("Please upload csv files only", Browser.AlertText);
//        }

        [TestMethod]
        public void TestImportResiduals()
        {
            GoToResidualsImport();
            FillOutImportFields();

            Browser.ImplicitWait = 5;

            Browser.UploadFile(_r, "residuals.import.file", "rir_pegasus_samples.csv")
                .Click(_r, "residuals.import.import-button")
                .Click(_r, "residuals.import.process-button");

            Assert.AreEqual("Successfully imported rir_pegasus_samples",
                Browser.FindElement(Common, "flash-message").Text);
        }

        [TestMethod]
        public void TestDeleteResiduals()
        {
            GoToResiduals();
            Browser.ImplicitWait = 5;
            Browser.Click(_r, "residuals.delete-button");
            Assert.AreEqual("File deleted successfully.",
                Browser.FindElement(Common, "flash-message").Text);
        }

        [TestMethod]
        public void TestDownloadImported()
        {
            GoToResiduals();
            Browser.ImplicitWait = 5;

            Browser.Click(_r, "residuals.download-button");
        }

        [TestMethod]
        public void TestMappingWizard()
        {
            GoToResidualsImport();
            FillOutImportFields();

            Browser.ImplicitWait = 5;

            Browser.UploadFile(_r, "residuals.import.file", "rir_pegasus_samples.csv")
                .Click(_r, "residuals.import.import-button");

            Thread.Sleep(2000);

            Assert.AreEqual("Residual Income / Import New / Mapping Wizard",
                Browser.FindElement(Common, "breadcrumbs").Text);
        }

        [TestMethod]
        public void TestPreviousMappings()
        {
            TestMappingWizard();

            var previousMappings = Browser.FindElements(_r,
                "residuals.import.previous-mappings");

            Assert.IsTrue(previousMappings.Count > 0);

            Browser.DropdownSelectByText(_r, "residuals.import.previous-mappings-dropdown",
                previousMappings[new Random().Next(previousMappings.Count)].Text);
        }
    }
}