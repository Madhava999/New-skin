using Microsoft.VisualStudio.TestTools.UnitTesting;
using NewSkin.Util;
using System;
using System.Threading;
using OpenQA.Selenium;
using System.Collections.Generic;

namespace NewSkin.Tests
{
    /// <summary>
    ///     Summary description for Contacts
    /// </summary> b
    [TestClass]
    public class ResidualAdjustment : BaseTest
    {
        private LocatorReader _adjustment;

        [TestInitialize]
        public void Initialize()
        {
            Browser = Pegasus.LoginCom("tseaa");
            _adjustment = new LocatorReader("ResidualAdjustment.xml");
        }
        [TestMethod]
        public void AdjustmentsToolPage()
        {
            Browser.MouseOver(_adjustment.Get("ResidualIncomeTab"))
                .MouseOver(_adjustment.Get("MasterData"))
                .Click(_adjustment.Get("AdjustmentTool"));
            Assert.AreEqual("Adjustments Tool", Browser.Title);

        }
        [TestMethod]
        public void CreateAdjustment()
        {
            AdjustmentsToolPage();
            Browser.Click(_adjustment.Get("CreateButton"))
                .FillForm(_adjustment.Get("AdjustmentName"), "Test")
                .DropdownSelectByText(_adjustment.Get("AdjustmentType"), "Transaction")
                .DropdownSelectByText(_adjustment.Get("AdjustmentFor"), "Agent")
                .DropdownSelectByText(_adjustment.Get("ReportingPeriod"), "July")
                .DropdownSelectByText(_adjustment.Get("Processor"), "NPC");
            Thread.Sleep(2000);
            Browser.Click(_adjustment.Get("Save"));
            Assert.AreEqual("Master Adjustment Rules Created Successfully.",
                Browser.FindElement(Common.Get("flash-message")).Text);

        }
        [TestMethod]
        public void CreateAdjustmentBlank()
        {
            AdjustmentsToolPage();
            Browser.Click(_adjustment.Get("CreateButton"))
                .Click(_adjustment.Get("Save"));
            Assert.IsTrue(Browser.ElementsVisible(_adjustment.Get("FieldRequiredMessage")));

        }

        [TestMethod]
        public void CancelAdjustment()
        {
            AdjustmentsToolPage();
            Browser.Click(_adjustment.Get("CreateButton"))
                .FillForm(_adjustment.Get("AdjustmentName"), "Test")
                .DropdownSelectByText(_adjustment.Get("AdjustmentType"), "Transaction")
                .DropdownSelectByText(_adjustment.Get("AdjustmentFor"), "Agent")
                .DropdownSelectByText(_adjustment.Get("ReportingPeriod"), "July")
                .DropdownSelectByText(_adjustment.Get("Processor"), "NPC");
            Thread.Sleep(2000);
            Browser.Click(_adjustment.Get("Cancel"));
            Assert.AreEqual("Adjustments Tool", Browser.Title);
        }

        [TestMethod]
        public void LameTest()
        {
            AdjustmentsToolPage();
            Browser.Click(_adjustment.Get("CreateButton"));

            var processorsC = Browser.FindElements(_adjustment, "ProcessorDropdownOptions");
            var processors = new string[processorsC.Count];

            for (var i = 0; i < processorsC.Count; ++i)
            {
                processors[i] = processorsC[i].Text;
            }

            Browser.MouseOver(_adjustment.Get("Username"))
                .Click(_adjustment.Get("Admin"))
                .MouseOver(_adjustment.Get("MasterData"))
                .Click(_adjustment.Get("ProcessorAdmin"));

            Thread.Sleep(3000);
            processorsC = Browser.FindElements(_adjustment, "ProcessorAdminList");
            var processors2 = new string[processorsC.Count];
            for (var i = 0; i < processorsC.Count; ++i)
            {
                processors2[i] = processorsC[i].Text;
            }

            var set1 = new HashSet<string>(processors);
            var set2 = new HashSet<string>(processors2);

            Assert.IsTrue(set1.SetEquals(set2));
        }
        [TestMethod]
        public void ClearMerchantAdjustment()
        {
            AdjustmentsToolPage();
            Browser.Click(_adjustment.Get("CreateButton"))
                .Click(_adjustment.Get("SpecificMerchant"))
                .Click(_adjustment.Get("SelectMerchant"));
            Thread.Sleep(2000);
            Browser.Click(_adjustment.Get("Client"));
            Assert.IsTrue(Browser.ElementsVisible(_adjustment.Get("Green")));
            Thread.Sleep(1000);
            Browser.Click(_adjustment.Get("SelectMerchant"));
            Assert.IsTrue(Browser.ElementsVisible(_adjustment.Get("SelectMerchant")));
        }
        [TestMethod]
        public void AddAmountWithNonNumericValue()
        {
            AdjustmentsToolPage();
            Browser.Click(_adjustment.Get("CreateButton"))
                .FillForm(_adjustment.Get("AdjustmentName"), "Test")
                .DropdownSelectByText(_adjustment.Get("AdjustmentType"), "Transaction")
                .DropdownSelectByText(_adjustment.Get("AdjustmentFor"), "Agent")
                .DropdownSelectByText(_adjustment.Get("ReportingPeriod"), "July")
                .DropdownSelectByText(_adjustment.Get("Processor"), "NPC")
                .DropdownSelectByText(_adjustment.Get("Type"), "Percentage")
                .FillForm(_adjustment.Get("Amount"), "Test Amount")
                .DropdownSelectByText(_adjustment.Get("AddRemove"), "Add this amount for calculation");
            Thread.Sleep(2000);
            Browser.Click(_adjustment.Get("Save"));
            Assert.IsTrue(Browser.ElementsVisible(_adjustment.Get("EnterValidNumber")));
        }
        [TestMethod]
        public void AddAmountWithNegativeNumber()
        {
            AdjustmentsToolPage();
            Browser.Click(_adjustment.Get("CreateButton"))
                .FillForm(_adjustment.Get("AdjustmentName"), "Test")
                .DropdownSelectByText(_adjustment.Get("AdjustmentType"), "Transaction")
                .DropdownSelectByText(_adjustment.Get("AdjustmentFor"), "Agent")
                .DropdownSelectByText(_adjustment.Get("ReportingPeriod"), "July")
                .DropdownSelectByText(_adjustment.Get("Processor"), "NPC")
                .DropdownSelectByText(_adjustment.Get("Type"), "Percentage")
                .FillForm(_adjustment.Get("Amount"), "-345")
                .DropdownSelectByText(_adjustment.Get("AddRemove"), "Add this amount for calculation");
            Thread.Sleep(2000);
            Browser.Click(_adjustment.Get("Save"));
            Assert.IsTrue(Browser.ElementsVisible(_adjustment.Get("EnterValidNumber")));

        }
        [TestMethod]
        public void AddAmountWithDecimals()
        {
            AdjustmentsToolPage();
            Browser.Click(_adjustment.Get("CreateButton"))
                .FillForm(_adjustment.Get("AdjustmentName"), "Test")
                .DropdownSelectByText(_adjustment.Get("AdjustmentType"), "Transaction")
                .DropdownSelectByText(_adjustment.Get("AdjustmentFor"), "Agent")
                .DropdownSelectByText(_adjustment.Get("ReportingPeriod"), "July")
                .DropdownSelectByText(_adjustment.Get("Processor"), "NPC")
                .DropdownSelectByText(_adjustment.Get("Type"), "Flat Amount")
                .FillForm(_adjustment.Get("Amount"), "44.2343")
                .DropdownSelectByText(_adjustment.Get("AddRemove"), "Add this amount for calculation");
            Thread.Sleep(2000);
            Browser.Click(_adjustment.Get("Save"));
            Assert.IsTrue(Browser.ElementsVisible(_adjustment.Get("EnterValidNumber")));

        }
        [TestMethod]
        public void TestPercentageInput()
        {
            AdjustmentsToolPage();
            Browser.Click(_adjustment.Get("CreateButton"))
                .FillForm(_adjustment.Get("AdjustmentName"), "Test")
                .DropdownSelectByText(_adjustment.Get("AdjustmentType"), "Transaction")
                .DropdownSelectByText(_adjustment.Get("AdjustmentFor"), "Agent")
                .DropdownSelectByText(_adjustment.Get("ReportingPeriod"), "July")
                .DropdownSelectByText(_adjustment.Get("Processor"), "NPC")
                .DropdownSelectByText(_adjustment.Get("Type"), "Percentage")
                .FillForm(_adjustment.Get("Amount"), "500")
                .DropdownSelectByText(_adjustment.Get("AddRemove"), "Add this amount for calculation");
            Thread.Sleep(2000);
            Browser.Click(_adjustment.Get("Save"));
            Assert.IsTrue(Browser.ElementsVisible(_adjustment.Get("EnterValidNumber")));
        }

        [TestMethod]
        public void CalculationWizard()
        {
            Browser.MouseOver(_adjustment.Get("ResidualIncomeTab"))
                .MouseOver(_adjustment.Get("OfficePayouts"))
                  .Click(_adjustment.Get("PayoutSummary"))
                  .DropdownSelectByText(_adjustment.Get("ProcessorSearch"), "Elavon");

            Browser.ImplicitWait = 5;
            Browser.Click(_adjustment.Get("CalculationWizard"));
            Assert.AreEqual("File Calculations By Reporting Period", Browser.Title);

        }
        [TestMethod]
        public void CalculationWizardViewRevenueShares()
        {
            CalculationWizard();
            Browser.ImplicitWait = 5;
            if (Browser.FindElements(_adjustment, "Startover").Count > 0)
            {
                Browser.Click(_adjustment.Get("Startover"))
                    .AlertAccept();
                Thread.Sleep(2000);
            }
            Thread.Sleep(5000);
            Browser.Click(_adjustment.Get("AgentLookUp"));
            Thread.Sleep(7000);
            Browser.Click(_adjustment.Get("CalculateAdjustments"));
            Thread.Sleep(7000);
            Browser.Click(_adjustment.Get("CalculatePayouts"));
            Thread.Sleep(7000);
            Browser.Click(_adjustment.Get("ViewPayouts"));
            Assert.AreEqual("Residual Income - Payouts", Browser.Title);
        }

        [TestMethod]
        public void TestRuleForAllMerchants()
        {
            AdjustmentsToolPage();
            Browser.Click(_adjustment.Get("CreateButton"))
                .FillForm(_adjustment.Get("AdjustmentName"), "Test")
                .DropdownSelectByText(_adjustment.Get("AdjustmentType"), "Transaction")
                .DropdownSelectByText(_adjustment.Get("AdjustmentFor"), "Office")
                .DropdownSelectByText(_adjustment.Get("ReportingPeriod"), "Any")
                .DropdownSelectByText(_adjustment.Get("Processor"), "Elavon")
                .DropdownSelectByText(_adjustment.Get("Type"), "Flat Amount")
                .FillForm(_adjustment.Get("Amount"), "1000")
                .DropdownSelectByText(_adjustment.Get("AddRemove"), "Remove this amount for calculation");
            Thread.Sleep(2000);
            Browser.Click(_adjustment.Get("Save"));
            Assert.AreEqual("Master Adjustment Rules Created Successfully.",
               Browser.FindElement(Common.Get("flash-message")).Text);
            Browser.ImplicitWait = 10;
            CalculationWizardViewRevenueShares();

        }
        [TestMethod]
        public void TestRuleForSpecificMerchant()
        {
            AdjustmentsToolPage();
            Browser.Click(_adjustment.Get("CreateButton"))
                .FillForm(_adjustment.Get("AdjustmentName"), "Test")
                .DropdownSelectByText(_adjustment.Get("AdjustmentType"), "Transaction")
                .DropdownSelectByText(_adjustment.Get("AdjustmentFor"), "Office")
                .DropdownSelectByText(_adjustment.Get("ReportingPeriod"), "Any")
                .DropdownSelectByText(_adjustment.Get("Processor"), "Elavon")
                .Click(_adjustment.Get("SpecificMerchant"))
                .Click(_adjustment.Get("SelectMerchant"));
            Thread.Sleep(2000);
            Browser.Click(_adjustment.Get("Client"));
            Assert.IsTrue(Browser.ElementsVisible(_adjustment.Get("Green")));
            Thread.Sleep(1000);
            Browser.Click(_adjustment.Get("SelectMerchant"))
                .DropdownSelectByText(_adjustment.Get("Type"), "Flat Amount")
                .FillForm(_adjustment.Get("Amount"), "1000")
                .DropdownSelectByText(_adjustment.Get("AddRemove"), "Remove this amount for calculation");
            Thread.Sleep(2000);
            Browser.Click(_adjustment.Get("Save"));
            Assert.AreEqual("Master Adjustment Rules Created Successfully.",
               Browser.FindElement(Common.Get("flash-message")).Text);
            Browser.ImplicitWait = 10;
            CalculationWizardViewRevenueShares();
        }
        [TestMethod]
        public void TestRuleForAllAgents()
        {
            AdjustmentsToolPage();
            Browser.Click(_adjustment.Get("CreateButton"))
                .FillForm(_adjustment.Get("AdjustmentName"), "Test")
                .DropdownSelectByText(_adjustment.Get("AdjustmentFor"), "Agent")
                .DropdownSelectByText(_adjustment.Get("AdjustmentType"), "Final Payout")
                .DropdownSelectByText(_adjustment.Get("ReportingPeriod"), "Any")
                .DropdownSelectByText(_adjustment.Get("Processor"), "Elavon")
                .DropdownSelectByText(_adjustment.Get("Type"), "Flat Amount")
                .FillForm(_adjustment.Get("Amount"), "1000")
                .DropdownSelectByText(_adjustment.Get("AddRemove"), "Remove this amount for calculation");
            Thread.Sleep(2000);
            Browser.Click(_adjustment.Get("Save"));
            Assert.AreEqual("Master Adjustment Rules Created Successfully.",
               Browser.FindElement(Common.Get("flash-message")).Text);
            Browser.ImplicitWait = 10;
            CalculationWizardViewRevenueShares();
        }
        [TestMethod]
        public void TestRuleForSpecificAgents()
        {
            AdjustmentsToolPage();
            Browser.Click(_adjustment.Get("CreateButton"))
                .FillForm(_adjustment.Get("AdjustmentName"), "Test")
                .DropdownSelectByText(_adjustment.Get("AdjustmentFor"), "Agent")
                .DropdownSelectByText(_adjustment.Get("AdjustmentType"), "Final Payout")
                .DropdownSelectByText(_adjustment.Get("ReportingPeriod"), "Any")
                .DropdownSelectByText(_adjustment.Get("Processor"), "Elavon")
                 .Click(_adjustment.Get("SpecificAgent"))
                .Click(_adjustment.Get("SelectAgent"));
            Thread.Sleep(2000);
            Browser.Click(_adjustment.Get("Agent"));
            Assert.IsTrue(Browser.ElementsVisible(_adjustment.Get("Green2")));
            Thread.Sleep(1000);
            Browser.Click(_adjustment.Get("SpecificAgent"))
                .DropdownSelectByText(_adjustment.Get("Type"), "Flat Amount")
                .FillForm(_adjustment.Get("Amount"), "1000")
                .DropdownSelectByText(_adjustment.Get("AddRemove"), "Remove this amount for calculation");
            Thread.Sleep(2000);
            Browser.Click(_adjustment.Get("Save"));
            Assert.AreEqual("Master Adjustment Rules Created Successfully.",
               Browser.FindElement(Common.Get("flash-message")).Text);
            Browser.ImplicitWait = 10;
            CalculationWizardViewRevenueShares();
        }
        [TestMethod]
        public void ApplyRulesBeforeRevenueShareCalculationAgent()
            {
             AdjustmentsToolPage();
             Browser.Click(_adjustment.Get("CreateButton"))
                .FillForm(_adjustment.Get("AdjustmentName"), "Test")
                .DropdownSelectByText(_adjustment.Get("AdjustmentFor"), "Agent")
                .DropdownSelectByText(_adjustment.Get("AdjustmentType"), "Transaction")
                .DropdownSelectByText(_adjustment.Get("ReportingPeriod"), "Any")
                .DropdownSelectByText(_adjustment.Get("Processor"), "Elavon")
                .DropdownSelectByText(_adjustment.Get("Type"), "Flat Amount")
                .FillForm(_adjustment.Get("Amount"), "1000")
                .DropdownSelectByText(_adjustment.Get("AddRemove"), "Remove this amount for calculation");
            Thread.Sleep(2000);
            Browser.Click(_adjustment.Get("Save"));
            Assert.AreEqual("Master Adjustment Rules Created Successfully.",
               Browser.FindElement(Common.Get("flash-message")).Text);
            }
        [TestMethod]
        public void ApplyRulesAfterRevenueShareCalculationAgent()
        {
            AdjustmentsToolPage();
            Browser.Click(_adjustment.Get("CreateButton"))
               .FillForm(_adjustment.Get("AdjustmentName"), "Test")
               .DropdownSelectByText(_adjustment.Get("AdjustmentFor"), "Agent")
               .DropdownSelectByText(_adjustment.Get("AdjustmentType"), "Transaction")
               .DropdownSelectByText(_adjustment.Get("ReportingPeriod"), "Any")
               .DropdownSelectByText(_adjustment.Get("Processor"), "Elavon")
               .Click(_adjustment.Get("RulesAfterRevenue"))
               .DropdownSelectByText(_adjustment.Get("Type"), "Flat Amount")
               .FillForm(_adjustment.Get("Amount"), "1000")
               .DropdownSelectByText(_adjustment.Get("AddRemove"), "Remove this amount for calculation");
            Thread.Sleep(2000);
            Browser.Click(_adjustment.Get("Save"));
            Assert.AreEqual("Master Adjustment Rules Created Successfully.",
               Browser.FindElement(Common.Get("flash-message")).Text);
        }
        [TestMethod]
        public void ClearMerchantAfterSelectingSpecificMErchant()
        {
            AdjustmentsToolPage();
            Browser.Click(_adjustment.Get("CreateButton"))
                .FillForm(_adjustment.Get("AdjustmentName"), "Test")
                .DropdownSelectByText(_adjustment.Get("AdjustmentType"), "Transaction")
                .DropdownSelectByText(_adjustment.Get("AdjustmentFor"), "Office")
                .DropdownSelectByText(_adjustment.Get("ReportingPeriod"), "Any")
                .DropdownSelectByText(_adjustment.Get("Processor"), "Elavon")
                .Click(_adjustment.Get("SpecificMerchant"));
            Thread.Sleep(2000);
                Browser.Click(_adjustment.Get("SelectMerchant"));
            Thread.Sleep(2000);
            Browser.Click(_adjustment.Get("Client"));
            Assert.IsTrue(Browser.ElementsVisible(_adjustment.Get("Green")));
            Thread.Sleep(1000);
            Browser.Click(_adjustment.Get("SelectMerchant"));
                Thread.Sleep(2000);
                Browser.DropdownSelectByText(_adjustment.Get("Type"), "Flat Amount")
                .FillForm(_adjustment.Get("Amount"), "1000")
                .DropdownSelectByText(_adjustment.Get("AddRemove"), "Remove this amount for calculation");
            Thread.Sleep(2000);
            Browser.Click(_adjustment.Get("Save"));
            Assert.AreEqual("Master Adjustment Rules Created Successfully.",
               Browser.FindElement(Common.Get("flash-message")).Text);
        }
        [TestMethod]
        public void AnyReportingPeriodForOffice()
        {
            AdjustmentsToolPage();
            Browser.Click(_adjustment.Get("CreateButton"))
                .FillForm(_adjustment.Get("AdjustmentName"), "Test")
                .DropdownSelectByText(_adjustment.Get("AdjustmentType"), "Transaction")
                .DropdownSelectByText(_adjustment.Get("AdjustmentFor"), "Office")
                .DropdownSelectByText(_adjustment.Get("ReportingPeriod"), "Any")
                .DropdownSelectByText(_adjustment.Get("Processor"), "NPC");
            Thread.Sleep(2000);
            Browser.Click(_adjustment.Get("Save"));
            Assert.AreEqual("Master Adjustment Rules Created Successfully.",
                Browser.FindElement(Common.Get("flash-message")).Text);
        }
        [TestMethod]
        public void SpecificReportingPeriod()
        {
            AdjustmentsToolPage();
            Browser.Click(_adjustment.Get("CreateButton"))
                .FillForm(_adjustment.Get("AdjustmentName"), "Test")
                .DropdownSelectByText(_adjustment.Get("AdjustmentType"), "Transaction")
                .DropdownSelectByText(_adjustment.Get("AdjustmentFor"), "Office")
                .DropdownSelectByText(_adjustment.Get("ReportingPeriod"), "July")
                .DropdownSelectByText(_adjustment.Get("Processor"), "NPC");
            Thread.Sleep(2000);
            Browser.Click(_adjustment.Get("Save"));
            Assert.AreEqual("Master Adjustment Rules Created Successfully.",
                Browser.FindElement(Common.Get("flash-message")).Text);
        }
        [TestMethod]
        public void TestAnyProcessorOption()
        {
            AdjustmentsToolPage();
            Browser.Click(_adjustment.Get("CreateButton"))
                .FillForm(_adjustment.Get("AdjustmentName"), "Test")
                .DropdownSelectByText(_adjustment.Get("AdjustmentType"), "Transaction")
                .DropdownSelectByText(_adjustment.Get("AdjustmentFor"), "Office")
                .DropdownSelectByText(_adjustment.Get("ReportingPeriod"), "July")
                .DropdownSelectByText(_adjustment.Get("Processor"), "Any");
            Thread.Sleep(2000);
            Browser.Click(_adjustment.Get("Save"));
            Assert.AreEqual("Master Adjustment Rules Created Successfully.",
                Browser.FindElement(Common.Get("flash-message")).Text);
        }
        [TestMethod]
        public void TestSpecificProcessor()
        {
            AdjustmentsToolPage();
            Browser.Click(_adjustment.Get("CreateButton"))
                .FillForm(_adjustment.Get("AdjustmentName"), "Test")
                .DropdownSelectByText(_adjustment.Get("AdjustmentType"), "Transaction")
                .DropdownSelectByText(_adjustment.Get("AdjustmentFor"), "Office")
                .DropdownSelectByText(_adjustment.Get("ReportingPeriod"), "July")
                .DropdownSelectByText(_adjustment.Get("Processor"), "NPC");
            Thread.Sleep(2000);
            Browser.Click(_adjustment.Get("Save"));
            Assert.AreEqual("Master Adjustment Rules Created Successfully.",
                Browser.FindElement(Common.Get("flash-message")).Text);
        }
        [TestMethod]
        public void TestFlatAmount()
        {
            AdjustmentsToolPage();
            Browser.Click(_adjustment.Get("CreateButton"))
                .FillForm(_adjustment.Get("AdjustmentName"), "Test")
                .DropdownSelectByText(_adjustment.Get("AdjustmentType"), "Transaction")
                .DropdownSelectByText(_adjustment.Get("AdjustmentFor"), "Office")
                .DropdownSelectByText(_adjustment.Get("ReportingPeriod"), "July")
                .DropdownSelectByText(_adjustment.Get("Processor"), "NPC")
                .DropdownSelectByText(_adjustment.Get("Type"), "Flat Amount")
                .FillForm(_adjustment.Get("Amount"), "1000")
                .DropdownSelectByText(_adjustment.Get("AddRemove"), "Remove this amount for calculation");
            Thread.Sleep(2000);
            Browser.Click(_adjustment.Get("Save"));
            Assert.AreEqual("Master Adjustment Rules Created Successfully.",
                Browser.FindElement(Common.Get("flash-message")).Text);
        }
        [TestMethod]
        public void TestPercentage()
        {
            AdjustmentsToolPage();
            Browser.Click(_adjustment.Get("CreateButton"))
                .FillForm(_adjustment.Get("AdjustmentName"), "Test")
                .DropdownSelectByText(_adjustment.Get("AdjustmentType"), "Transaction")
                .DropdownSelectByText(_adjustment.Get("AdjustmentFor"), "Office")
                .DropdownSelectByText(_adjustment.Get("ReportingPeriod"), "July")
                .DropdownSelectByText(_adjustment.Get("Processor"), "NPC")
                .DropdownSelectByText(_adjustment.Get("Type"), "Percentage")
                .FillForm(_adjustment.Get("Amount"), "10")
                .DropdownSelectByText(_adjustment.Get("AddRemove"), "Remove this amount for calculation");
            Thread.Sleep(2000);
            Browser.Click(_adjustment.Get("Save"));
            Assert.AreEqual("Master Adjustment Rules Created Successfully.",
                Browser.FindElement(Common.Get("flash-message")).Text);
        }
        [TestMethod]
        public void TestFields()
        {
            AdjustmentsToolPage();
            Browser.Click(_adjustment.Get("CreateButton"))
                .FillForm(_adjustment.Get("AdjustmentName"), "Test")
                .DropdownSelectByText(_adjustment.Get("AdjustmentFor"), "Agent")
                .DropdownSelectByText(_adjustment.Get("AdjustmentType"), "Final Payout")
                .DropdownSelectByText(_adjustment.Get("ReportingPeriod"), "July")
                .DropdownSelectByText(_adjustment.Get("Processor"), "Elavon")
                .DropdownSelectByText(_adjustment.Get("Type"), "Distribute")
                .FillForm(_adjustment.Get("Amount"), "100")
                .DropdownSelectByText(_adjustment.Get("AddRemove"), "Remove this amount for calculation");
            Thread.Sleep(2000);
            Browser.Click(_adjustment.Get("Save"));
            Assert.AreEqual("Master Adjustment Rules Created Successfully.",
                Browser.FindElement(Common.Get("flash-message")).Text);
        }
        [TestMethod]
        public void TestRemoveAmount()
        {
            AdjustmentsToolPage();
            Browser.Click(_adjustment.Get("CreateButton"))
                .FillForm(_adjustment.Get("AdjustmentName"), "Test")
                .DropdownSelectByText(_adjustment.Get("AdjustmentFor"), "Agent")
                .DropdownSelectByText(_adjustment.Get("AdjustmentType"), "Transaction")
                .DropdownSelectByText(_adjustment.Get("ReportingPeriod"), "Any")
                .DropdownSelectByText(_adjustment.Get("Processor"), "Elavon")
                .DropdownSelectByText(_adjustment.Get("Type"), "Flat Amount")
                .FillForm(_adjustment.Get("Amount"), "1000")
                .DropdownSelectByText(_adjustment.Get("AddRemove"), "Remove this amount for calculation");
            Thread.Sleep(2000);
            Browser.Click(_adjustment.Get("Save"));
            Assert.AreEqual("Master Adjustment Rules Created Successfully.",
               Browser.FindElement(Common.Get("flash-message")).Text);
        }
       
    }
}
