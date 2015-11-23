using Microsoft.VisualStudio.TestTools.UnitTesting;
using NewSkin.Util;
using OpenQA.Selenium.Support.UI;
using System;
using System.Threading;


namespace NewSkin.Tests
{
    /// <summary>
    ///     Summary description for Integrated Residual
    /// </summary>
    [TestClass]
    public class IntegratedResidual : BaseTest
    {
        private LocatorReader _integrated;

        [TestInitialize]
        public void Initialize()
        {
            Browser = Pegasus.LoginCom("selcorp");
            _integrated = new LocatorReader("IntegratedResidual.xml");
        }

        [TestMethod]
        public void TestImportsLink()
        {
            Browser.MouseOver(_integrated.Get("ResidualIncomeTab"))
                .Click(_integrated.Get("Imports"));
            Thread.Sleep(1000);
            Assert.AreEqual("Residual Income - Imports", Browser.Title);
        }
        [TestMethod]
        public void TestSelectFileDate()
        {
            TestImportsLink();
            Browser.ImplicitWait = 5;
            Browser.Click(_integrated.Get("ImportNewButton"))
                .Click(_integrated.Get("FileDate"));

            new SelectElement(Browser.FindElement(_integrated, "ProcessorType")).SelectByIndex(1);
            Thread.Sleep(1000);
            Browser.Click(_integrated.Get("Date"))
            .UploadFile(_integrated.Get("ChooseFile"), "rir_pegasus_samples.csv")
            .Click(_integrated.Get("ImportButton"));
            Thread.Sleep(2000);
            Assert.AreEqual("Residual Income - Import New", Browser.Title);
        }
        [TestMethod]
        public void FilterResiduals()
        {
            TestImportsLink();
            Browser.FillForm(_integrated.Get("Count"), "test");
            Browser.ImplicitWait = 5;
            Assert.IsTrue(Browser.ElementsVisible(_integrated.Get("NoMatchingRecords")));
        }

        [TestMethod]
        public void DeleteImportedFile()
        {
            TestImportsLink();
            Browser.Click(_integrated.Get("Delete"))
                .Wait(2)
                .AlertAccept()
                .Wait(2);
            Assert.AreEqual("File deleted successfully.",
               Browser.FindElement(Common.Get("flash-message")).Text);
        }

        [TestMethod]
        public void DownloadImportedFile()
        {
            TestImportsLink();
            Browser.Click(_integrated.Get("Download"));
            Thread.Sleep(2000);
        }
        [TestMethod]
        public void ViewTransactions()
        {
            TestImportsLink();
            Browser.Click(_integrated.Get("ViewTransactions"));
            Assert.IsTrue(Browser.ElementsVisible(_integrated.Get("Export")));

        }
        [TestMethod]
        public void CalculationWizard()
        {
            TestImportsLink();
            Browser.ImplicitWait = 5;
            Browser.Click(_integrated.Get("CalculationWizard"))
                .Wait(2);
            Assert.AreEqual("File Wizard", Browser.Title);

        }

        [TestMethod]
        public void CalculationWizardRevenueShare()
        {
            CalculationWizardViewRevenueShares();
            Browser.FillForm(_integrated.Get("RevenuePercentage"), "55.5")
            .Click(_integrated.Get("ApplyToAllOffices"))
            .Click(_integrated.Get("Save"))
            .Wait(2);
            Assert.AreEqual("Revenue Share Created.",
              Browser.FindElement(Common.Get("flash-message")).Text);


        }
        [TestMethod]
        public void CalculationWizardViewRevenueShares()
        {
            CalculationWizard();
            Browser.ImplicitWait = 5;
            if (Browser.FindElements(_integrated, "Startover").Count > 0)
            {
                Browser.Click(_integrated.Get("Startover"))
                    .Wait(2)
                    .AlertAccept();
                Thread.Sleep(2000);
            }
            Thread.Sleep(500);
            Browser.Click(_integrated.Get("Step1"))
                .Wait(2);
            Assert.AreEqual("Residualmasterrules", Browser.Title);
        }
        [TestMethod]
        public void CalculationWizardNegativeRevenueShare()
        {
            CalculationWizardViewRevenueShares();
            Browser.FillForm(_integrated.Get("RevenuePercentage"), "-55.5")
               .Click(_integrated.Get("ApplyToAllOffices"))
               .Click(_integrated.Get("Save"))
               .Wait(2);
            Assert.AreEqual("Revenue Share Created.",
              Browser.FindElement(Common.Get("flash-message")).Text);
        }
        [TestMethod]
        public void CancelCalculationWizard()
        {
            CalculationWizardViewRevenueShares();
            Browser.Click(_integrated.Get("Cancel"))
                .Wait(2);
            Assert.AreEqual("File Wizard", Browser.Title);
        }
        [TestMethod]
        public void CalculationWizardSkipStep1()
        {
            CalculationWizard();
            Browser.ImplicitWait = 5;
            if (Browser.FindElements(_integrated, "Startover").Count > 0)
            {
                Browser.Click(_integrated.Get("Startover"))
                    .Wait(2)
                    .AlertAccept();
                Thread.Sleep(2000);
            }
            Thread.Sleep(500);
            Browser.Click(_integrated.Get("SkipStep1"));
            Thread.Sleep(300);
            Assert.IsTrue(Browser.ElementsVisible(_integrated.Get("Step2")));
        }
        [TestMethod]
        public void CalculationWizardStep2()
        {
            CalculationWizardSkipStep1();
            Thread.Sleep(500);
            Browser.Click(_integrated.Get("Step2"))
                .Wait(2);
            Assert.AreEqual("Residualmasterrules", Browser.Title);
        }
        [TestMethod]
        public void CalculationWizardRuleSet()
        {
            var a = new System.Random();
            var rand = a.Next(10000);
            CalculationWizardStep2();
            Browser.FillForm(_integrated.Get("RuleSetName"), "Test" + rand)
            .Click(_integrated.Get("SaveStep2"))
            .Wait(2);
            Assert.AreEqual("Residual Rule is Updated.",
             Browser.FindElement(Common.Get("flash-message")).Text);
        }
        [TestMethod]
        public void CalculationWizardRuleSetFlatAmount()
        {
            var a = new System.Random();
            var rand = a.Next(10000);
            CalculationWizardStep2();
            Browser.FillForm(_integrated.Get("RuleSetName"), "Test" + rand)
                .DropdownSelectByText(_integrated.Get("RuleType"), "Flat Amount")
                .FillForm(_integrated.Get("FlatAmount"), "23.99")
                .DropdownSelectByText(_integrated.Get("AddRemove"), "Add this amount for calculation")
                .Click(_integrated.Get("SaveStep2"))
                .Wait(2);
            Assert.AreEqual("Residual Rule is Updated.",
            Browser.FindElement(Common.Get("flash-message")).Text);
        }
        [TestMethod]
        public void CalculationWizardSkipStep2()
        {
            Browser.ImplicitWait = 5;
            CalculationWizardSkipStep1();
            Thread.Sleep(1000);
            Browser.Click(_integrated.Get("SkipStep2"));
            Thread.Sleep(1000);
            Assert.IsTrue(Browser.ElementsVisible(_integrated.Get("Step4")));
            Thread.Sleep(500);
            Assert.IsTrue(Browser.ElementsVisible(_integrated.Get("Recalculate")));
        }
        [TestMethod]
        public void CalculationWizardRecalculate()
        {
            CalculationWizardSkipStep2();
            Thread.Sleep(500);
            Browser.Click(_integrated.Get("Recalculate"))
                .Wait(2);
            Assert.AreEqual("File Wizard", Browser.Title);

            //var message = Browser.FindElement(_integrated, "Recalculate-Message");  /////// 
            // Assert.IsTrue(message.Displayed);
        }
        [TestMethod]
        public void ViewOfficePayouts()
        {
            CalculationWizardSkipStep2();
            Thread.Sleep(500);
            Browser.Click(_integrated.Get("ViewOfficePayouts"))
                .Wait(2);
            Assert.AreEqual("Residual Income - Payouts", Browser.Title);
        }
        [TestMethod]
        public void ExportOfficePayouts()
        {
            Browser.ImplicitWait = 5;
            ViewOfficePayouts();
            Browser.ImplicitWait = 5;
            Browser.Click(_integrated.Get("ExportButton"))
                .Click(_integrated.Get("Excel"));
        }
        [TestMethod]
        public void PrintOfficePayouts()
        {
            ViewOfficePayouts();
            Thread.Sleep(500);
            Browser.Click(_integrated.Get("Print"));
            Thread.Sleep(500);
        }
        [TestMethod]
        public void ViewReports()
        {
            CalculationWizardSkipStep2();
            Browser.ImplicitWait = 5;
            Thread.Sleep(2000);
            Browser.Click(_integrated.Get("ViewReports"))
                .DropdownSelectByText(_integrated.Get("Processor"), "All")
                .DropdownSelectByText(_integrated.Get("FileDateReports"), "All");
            Thread.Sleep(2000);
            Browser.Click(_integrated.Get("SearchOffices"));
            Thread.Sleep(2000);
            Browser.Click(_integrated.Get("ReportBlue"))
                .Wait(2);
            Assert.AreEqual("Residual Income - Reports", Browser.Title);

        }
        [TestMethod]
        public void DownloadPDFReports()
        {
            ViewReports();
            Thread.Sleep(7000);
            Browser.Click(_integrated.Get("PDF"));
            Thread.Sleep(1000);
        }
        [TestMethod]
        public void EmailReport()
        {
            ViewReports();
            Thread.Sleep(7000);
            Browser.Click(_integrated.Get("Email"));
            Assert.AreEqual("Residual Income - Reports", Browser.Title);

        }
        [TestMethod]
        public void ReportInExcel()
        {
            ViewReports();
            Thread.Sleep(3000);
            Browser.Click(_integrated.Get("ExcelIcon"));
            Thread.Sleep(500);
            
        }
        [TestMethod]
        public void ReportInCSV()
        {
            ViewReports();
            Thread.Sleep(3000);
            Browser.Click(_integrated.Get("CSVIcon"));
            Thread.Sleep(1000);

        }
        [TestMethod]
        public void PublishPayouts()
        {
            Browser.ImplicitWait = 5;
            CalculationWizardSkipStep2();
            Thread.Sleep(1500);
            Browser.Click(_integrated.Get("Step4"));
            Thread.Sleep(3000);
            Assert.AreEqual("Residual Income - Imports", Browser.Title); 
        }
        [TestMethod]
        public void ResidualIncomeReports()
        {
            Browser.MouseOver(_integrated.Get("ResidualIncomeTab"))
               .MouseOver(_integrated.Get("PayoutsTab"))
               .Click(_integrated.Get("ReportsTab"))
               .Wait(2)
               .DropdownSelectByText(_integrated.Get("ReportingPeriod"), "August 2015")
               .Click(_integrated.Get("SearchOffices"))
               .Wait(2);
            Assert.AreEqual("Residual Income - Reports", Browser.Title);
        }
        [TestMethod]
        public void ResidualIncomeReportSearch()
        {
            ResidualIncomeReports();
            Thread.Sleep(2000);
            Browser.DropdownSelectByText(_integrated.Get("Processor"), "All")
                  .DropdownSelectByText(_integrated.Get("FileDateReports"), "All");

            new SelectElement(Browser.FindElement(_integrated, "ReportingPeriod")).SelectByIndex(1);
            Thread.Sleep(2000);
            Browser.Click(_integrated.Get("SearchOffices"));
            Thread.Sleep(2000);
            Browser.Click(_integrated.Get("ReportBlue"));
            Thread.Sleep(4000);
            Assert.IsTrue(Browser.ElementsVisible(_integrated.Get("ResidualReportsTitle")));
        }
        [TestMethod]
        public void MassEmailReportPDF()
        {
            ResidualIncomeReports();
            Browser.Click(_integrated.Get("CheckBoxFirst"))
                .Click(_integrated.Get("SendButton"));
            Assert.IsTrue(Browser.ElementsVisible(_integrated.Get("MailSentSucessfully")));
        }
        [TestMethod]
        public void MassEmailReportToAllPDF()
        {
            ResidualIncomeReports();
            Browser.Click(_integrated.Get("CheckBoxFirst"))
                .Click(_integrated.Get("AllOffices"))
                .Click(_integrated.Get("SendButton"));
            Assert.IsTrue(Browser.ElementsVisible(_integrated.Get("MailSentSucessfully")));
        }
        [TestMethod]
        public void MassEmailReportToAllExcel()
        {
            ResidualIncomeReports();
            Browser.Click(_integrated.Get("CheckBoxFirst"))
                .Click(_integrated.Get("AllOffices"))
                .Click(_integrated.Get("ReportTypeExcel"))
                .Click(_integrated.Get("SendButton"));
            Assert.IsTrue(Browser.ElementsVisible(_integrated.Get("MailSentSucessfully")));
        }
        [TestMethod]
        public void MassEmailReportToAllCSV()
        {
            ResidualIncomeReports();
            Browser.Click(_integrated.Get("CheckBoxFirst"))
                .Click(_integrated.Get("AllOffices"))
                .Click(_integrated.Get("ReportTypeCSV"))
                .Click(_integrated.Get("SendButton"))
                .Wait(2);
            Assert.IsTrue(Browser.ElementsVisible(_integrated.Get("MailSentSucessfully")));
        }
    }
}