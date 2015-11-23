using System;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NewSkin.Util;

namespace NewSkin.Tests
{
    [TestClass]
    public class PDFImportWizard1 : BaseTest
    {
        private LocatorReader pdf;
        Random rand = new Random();

        [TestInitialize]
        public void TestInitialize()
        {
            pdf = new LocatorReader("PDFImportWizard1.xml");
        }

        private void GoTo(String which, String page)
        {
            if (which == "corp")
            {
                Browser = Pegasus.LoginMyPeg("selcorp");
                //Browser = Pegasus.LoginCom("selcorp");
                Thread.Sleep(500);
            }
            else
            {
                Browser = Pegasus.LoginMyPeg("seloffice");
                //Browser = Pegasus.LoginCom("seloffice");
                Thread.Sleep(500);
                GoToAdmin();
                Thread.Sleep(2000);
            }

            Browser.MouseOver(pdf, "pdf-tab")
                .Click(pdf, page)
                .Wait(1);
        }

        [TestMethod]
        public void PDFCorpAddCategory()
        {
            var name = "PDF Category " + rand.Next(int.MaxValue);

            GoTo("corp", "pdf-categories");
            Browser.Click(pdf, "create-pdf-category")
                .FillForm(pdf, "pdf-category-name", name)
                .Click(pdf, "save-button");

            Assert.AreEqual("Category Created Successfully",
                Browser.FindElement(Common, "flash-message").Text);
        }

        [TestMethod]
        public void PDFCorpExtensionCheck()
        {
            GoTo("corp", "pdf-import-wizard");
            Browser.DropdownSelectByText(pdf, "module-select", "Clients")
                .UploadFile(pdf, "import-file", "fake.csv")
                .Click(pdf, "import-button")
                .Wait(1);

            Assert.AreEqual("The file selected is not a PDF document. " +
                "Please either select the correct PDF file or convert " +
                "the file to a .PDF format and try again",
                Browser.AlertText);
        }

        [TestMethod]
        public void PDFCorpRealPDFCheck()
        {
            GoTo("corp", "pdf-import-wizard");
            Browser.DropdownSelectByText(pdf, "module-select", "Clients")
                .UploadFile(pdf, "import-file", "fake.csv").Wait(1)
                .Click(pdf, "import-button")
                .Wait(2);

            Assert.AreEqual("The file selected is not a PDF document. " +
                "Please either select the correct PDF file or convert " +
                "the file to a .PDF format and try again",
                Browser.AlertText);
        }

        [TestMethod]
        public void PDFCorpModuleRequired()
        {
            GoTo("corp", "pdf-import-wizard");
            Browser.UploadFile(pdf, "import-file", "real.pdf").Wait(1)
                .Click(pdf, "import-button")
                .Wait(1);

            Assert.AreEqual("This field is required.",
                Browser.FindElement(pdf, "required-message").Text);
        }

        [TestMethod]
        public void PDFCorpDetectFields()
        {
            GoTo("corp", "pdf-import-wizard");
            Browser.DropdownSelectByText(pdf, "module-select", "Clients")
                .UploadFile(pdf, "import-file", "real.pdf")
                .Click(pdf, "import-button")
                .Wait(1);

            Assert.IsTrue(Browser.ElementsVisible(pdf, "pdf-fields"));
        }

        [TestMethod]
        public void PDFCorpImportCancel()
        {
            GoTo("corp", "pdf-import-wizard");
            Browser.DropdownSelectByText(pdf, "module-select", "Clients")
                .UploadFile(pdf, "import-file", "real.pdf")
                .Click(pdf, "import-button")
                .Wait(5)
                .Click(pdf, "cancel-button")
                .Wait(1)
                .AlertAccept()
                .Wait(3);

            Assert.IsTrue(Browser.ElementsVisible(pdf, "upload-pdf"));
        }

        [TestMethod]
        public void PDFOfficeReplaceCategory()
        {
            GoTo("office", "pdf-categories");
            Browser.Click(pdf, "delete-category")
                .Wait(1)
                .AlertAccept()
                .Wait(1);

            Assert.IsTrue(Browser.ElementsVisible(pdf, "replace-category"));
        }

        [TestMethod]
        public void PDFOfficePackagePopUpMessage()
        {
            GoTo("office", "pdf-templates");
            Browser.Click(pdf, "pdf-package")
                .Click(pdf, "select-pdf-template")
                .Wait(1);

            Assert.AreEqual("Please Select module", Browser.AlertText);
        }

        [TestMethod]
        public void PDFOfficeDisplayPackage()
        {
            var name = "Test Package " + rand.Next(int.MaxValue);

            GoTo("office", "pdf-templates");
            Browser.Click(pdf, "pdf-package")
                .FillForm(pdf, "package-name", name)
                .DropdownSelectByText(pdf, "package-module", "Leads")
                .DropdownSelectByText(pdf, "package-category", "Card Service Agreements")
                .Click(pdf, "display-tab-box")
                .Wait(1)
                .Click(pdf, "package-access")
                .Click(pdf, "save-button")
                .Wait(1);

            GoToMain();
            Thread.Sleep(1000);
            Browser.Click(pdf, "leads-tab")
                .Wait(1)
                .Click(pdf, "lead")
                .Wait(1)
                .Click(pdf, "pdfs-tab");

            Assert.IsTrue(Browser.TextExists(name));
        }

        [TestMethod]
        public void PDFOfficePackageRequiredFields()
        {
            GoTo("office", "pdf-templates");
            Browser.Click(pdf, "pdf-package")
                .Click(pdf, "save-button");

            Assert.IsTrue(Browser.ElementCount(pdf, "required-message") == 3);
            Assert.AreEqual("This field is required.",
                Browser.FindElement(pdf, "required-message").Text);
        }

        [TestMethod]
        public void PDFOfficeSearchPDFs()
        {
            GoTo("office", "pdf-templates");
            Thread.Sleep(3000);
            Browser.Click(pdf, "leads-select")
                .Wait(2);

            Assert.IsTrue(Browser.ElementCount(pdf, "clients-count") == 0);
        }

        [TestMethod]
        public void PDFOfficeExitInstructions()
        {
            GoTo("office", "pdf-import-wizard");
            Browser.Click(pdf, "instruction-link")
                .Wait(1)
                .Click(pdf, "instruction-exit")
                .Wait(1);

            Assert.IsFalse(Browser.ElementsVisible(pdf, "instruction"));
        }

        [TestMethod]
        public void PDFOfficeInstructions()
        {
            GoTo("office", "pdf-import-wizard");
            Browser.Click(pdf, "instruction-link")
                .Wait(1);

            Assert.IsTrue(Browser.ElementsVisible(pdf, "instruction"));
        }

        [TestMethod]
        public void PDFOfficeAddCategory()
        {
            var name = "PDF Category " + rand.Next(int.MaxValue);

            GoTo("office", "pdf-categories");
            Browser.Click(pdf, "create-pdf-category")
                .FillForm(pdf, "pdf-category-name2", name)
                .Click(pdf, "save-button");

            Assert.AreEqual("Category Created Successfully",
                Browser.FindElement(Common, "flash-message").Text);
        }

        [TestMethod]
        public void PDFOfficeCancelCategory()
        {
            GoTo("office", "pdf-categories");
            Browser.Click(pdf, "create-pdf-category")
                .Wait(1)
                .Click(pdf, "cancel-button")
                .Wait(1);

            Assert.IsFalse(Browser.ElementsVisible(pdf, "add-category-window"));
        }

        [TestMethod]
        public void PDFOfficeCategoryNameRequired()
        {
            GoTo("office", "pdf-categories");
            Browser.Click(pdf, "create-pdf-category")
                .Wait(1)
                .Click(pdf, "save-button");

            Assert.IsTrue(Browser.ElementCount(pdf, "required-message") == 1);
            Assert.AreEqual("This field is required.",
                Browser.FindElement(pdf, "required-message").Text);
        }

        [TestMethod]
        public void PDFOfficeEditCategory()
        {
            GoTo("office", "pdf-categories");
            Browser.Click(pdf, "edit-category")
                .Wait(1)
                .Click(pdf, "save-button");

            Assert.AreEqual("Category Updated Successfully",
                Browser.FindElement(Common, "flash-message").Text);
        }

        [TestMethod]
        public void PDFOfficeEditCancelCategory()
        {
            GoTo("office", "pdf-categories");
            Browser.Click(pdf, "edit-category")
                .Wait(1)
                .Click(pdf, "cancel-button")
                .Wait(1);

            Assert.IsFalse(Browser.ElementsVisible(pdf, "add-category-window"));
        }

        [TestMethod]
        public void PDFOfficeEditCategoryBlankName()
        {
            GoTo("office", "pdf-categories");
            Browser.Click(pdf, "edit-category")
                .Wait(1)
                .ClearForm(pdf, "pdf-category-name2")
                .Click(pdf, "save-button");

            Assert.IsTrue(Browser.ElementCount(pdf, "required-message") == 1);
            Assert.AreEqual("This field is required.",
                Browser.FindElement(pdf, "required-message").Text);
        }

        [TestMethod]
        public void PDFOfficeDeleteCategory()
        {
            GoTo("office", "pdf-categories");
            Browser.Click(pdf, "create-pdf-category");
            Browser.FillForm(pdf, "pdf-category-name2", "Test Delete")
                .Click(pdf, "save-button")
                .Wait(1);

            Browser.Refresh();

            Browser.Click(pdf, "delete-category")
                .Wait(1)
                .AlertAccept()
                .Wait(1)
                .DropdownSelectByIndex(pdf, "delete-replace-category", 1)
                .Wait(1)
               .Click(pdf, "save-button2")
               .Wait(1);

            Assert.AreEqual("Category Replaced Successfully.",
                Browser.FindElement(Common, "flash-message").Text);
        }
    }
}