using System;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NewSkin.Util;

namespace NewSkin.Tests
{
    [TestClass]
    public class DocumentManager : BaseTest
    {
        private LocatorReader doc;
        Random rand = new Random();

        [TestInitialize]
        public void TestInitialize()
        {
            doc = new LocatorReader("DocumentManager.xml");
            Browser = Pegasus.LoginCom("seloffice");
            Thread.Sleep(500);
        }

        private void GoToDocuments()
        {
            Browser.MouseOver(doc, "activities-tab")
                .Click(doc, "documents-link")
                .Wait(1);
        }

        //********** CREATE DOCUMENT **********
        [TestMethod]
        public void DocCreateButton()
        {
            GoToDocuments();
            Browser.Click(doc, "create-button")
                .Wait(1);

            Assert.AreEqual("Create a New Document", Browser.Title);
        }

        [TestMethod]
        public void DocCreateRequiredFields()
        {
            DocCreateButton();
            Browser.Click(doc, "save-button");

            Assert.IsTrue(Browser.ElementCount(doc, "required-message") == 2);
            Assert.AreEqual("This field is required.",
                Browser.FindElement(doc, "required-message").Text);
        }

        [TestMethod]
        public void DocValidDoc()
        {
            var name = "Document " + rand.Next(int.MaxValue);

            DocCreateButton();
            Browser.FillForm(doc, "create-name", name)
                .UploadFile(doc, "upload-button", "invalid.dll");

            Assert.AreEqual("please select a valid file!", Browser.AlertText);
        }

        [TestMethod]
        public void DocValidDoc2()
        {
            var name = "Document " + rand.Next(int.MaxValue);

            DocCreateButton();
            Browser.FillForm(doc, "create-name", name)
                .UploadFile(doc, "upload-button", "invalid2.inf")
                .Click(doc, "save-button");

            Assert.AreEqual("Document Attachment Invalid file.",
                Browser.FindElement(Common, "flash-message").Text);
        }

        [TestMethod]
        public void DocFakeDoc()
        {
            var name = "Document " + rand.Next(int.MaxValue);

            DocCreateButton();
            Browser.FillForm(doc, "create-name", name)
                .UploadFile(doc, "upload-button", "fake.pdf")
                .Click(doc, "save-button");

            Assert.AreEqual("Document Attachment Invalid file.",
                Browser.FindElement(Common, "flash-message").Text);
        }

        [TestMethod]
        public void DocCreateSave()
        {
            var name = "Document " + rand.Next(int.MaxValue);

            DocCreateButton();
            Browser.FillForm(doc, "create-name", name)
                .UploadFile(doc, "upload-button", "real.pdf")
                .Click(doc, "save-button");

            Assert.AreEqual("Document saved successfully.",
                Browser.FindElement(Common, "flash-message").Text);
        }

        [TestMethod]
        public void DocCreateCancel()
        {
            var name = "Document " + rand.Next(int.MaxValue);

            DocCreateButton();
            Browser.FillForm(doc, "create-name", name)
                .UploadFile(doc, "upload-button", "real.pdf")
                .Click(doc, "cancel-button")
                .Wait(1);

            Assert.AreNotEqual(name, Browser.FindElement(doc, "last-entry").Text);
        }

        //********** EDIT DOCUMENT **********
        [TestMethod]
        public void DocEditButton()
        {
            GoToDocuments();
            Thread.Sleep(1000);
            Browser.Click(doc, "edit-button")
                .Wait(1);

            Assert.AreEqual("Edit Document", Browser.Title);
        }

        [TestMethod]
        public void DocEditRequiredFields()
        {
            DocEditButton();
            Browser.ClearForm(doc, "create-name")
                .Click(doc, "save-button2");

            Assert.IsTrue(Browser.ElementCount(doc, "required-message") == 1);
            Assert.AreEqual("This field is required.",
                Browser.FindElement(doc, "required-message").Text);
        }

        [TestMethod]
        public void DocEditSave()
        {
            DocEditButton();
            Browser.Click(doc, "save-button2");

            Assert.AreEqual("Document updated successfully.",
                Browser.FindElement(Common, "flash-message").Text);
        }

        //********** ADD NEW VERSION **********
        [TestMethod]
        public void DocAddNewVerButton()
        {
            DocEditButton();
            Browser.Click(doc, "add-new-version");

            Assert.IsTrue(Browser.ElementsVisible(doc, "new-version-window"));
        }

        [TestMethod]
        public void DocAddNewVersRequiredFields()
        {
            DocAddNewVerButton();
            Browser.Click(doc, "save-button3");

            Assert.AreEqual(Browser.ElementCount(doc, "required-message"), 2);
            Assert.AreEqual("This field is required.",
                Browser.FindElement(doc, "required-message").Text);
        }

        [TestMethod]
        public void DocAddNewVerValidDoc()
        {
            DocAddNewVerButton();
            Browser.UploadFile(doc, "upload-button", "invalid.dll");

            Assert.AreEqual("please select a valid file!", Browser.AlertText);
        }

        [TestMethod]
        public void DocAddNewValidDoc2()
        {
            DocAddNewVerButton();
            Browser.UploadFile(doc, "upload-button", "invalid2.inf")
                .FillForm(doc, "new-version-comment", "Test Comment")
                .Click(doc, "save-button3");

            Assert.AreEqual("New Version Invalid file.",
                Browser.FindElement(Common, "flash-message").Text);
        }

        [TestMethod]
        public void DocAddNewFakeDoc()
        {
            DocAddNewVerButton();
            Browser.UploadFile(doc, "upload-button", "fake.pdf")
                .FillForm(doc, "new-version-comment", "Test Comment")
                .Click(doc, "save-button3");

            Assert.AreEqual("New Version Invalid file.",
                Browser.FindElement(Common, "flash-message").Text);
        }

        [TestMethod]
        public void DocAddNewVerSave()
        {
            DocEditButton();
            Browser.Click(doc, "add-new-version")
                .FillForm(doc, "new-version-comment", "Test Comment")
                .UploadFile(doc, "new-version-upload", "real.pdf")
                .Click(doc, "save-button3");

            Assert.AreEqual("New Version File Uploaded successfully.",
                Browser.FindElement(Common, "flash-message").Text);
        }

        [TestMethod]
        public void DocDeleteVersion()
        {
            DocAddNewVerSave();
            Browser.Click(doc, "new-version-delete")
                .Wait(1)
                .AlertAccept()
                .Wait(1);

            Assert.AreEqual("Document Permenantly Deleted.",
                Browser.FindElement(Common, "flash-message").Text);
        }

        //********** DELETE DOCUMENT / RECYCLE BIN **********
        [TestMethod]
        public void DocDelete()
        {
            DocCreateSave();
            Thread.Sleep(1000);
            Browser.Click(doc, "doc-check-box")
                .Click(doc, "delete-button")
                .Wait(1)
                .AlertAccept()
                .Wait(1);

            Assert.AreEqual("Document deleted successfully.",
                Browser.FindElement(Common, "flash-message").Text);
        }

        [TestMethod]
        public void DocPermanentDelete()
        {
            DocDelete();
            Browser.Click(doc, "recycle-button")
                .Click(doc, "permanent-delete")
                .Wait(1)
                .AlertAccept()
                .Wait(1);

            Assert.AreEqual("Document Permanently Deleted.",
                Browser.FindElement(Common, "flash-message").Text);
        }

        [TestMethod]
        public void DocRestore()
        {
            DocDelete();
            Browser.Click(doc, "recycle-button")
                .Click(doc, "restore");

            Assert.AreEqual("Document Restored Successfully.",
                Browser.FindElement(Common, "flash-message").Text);
        }

        //********** CLIENT **********
        [TestMethod]
        public void DocAddFromClientButton()
        {
            Browser.Click(doc, "clients-tab")
                .Wait(1)
                .Click(doc, "client")
                .Click(doc, "add-doc-button")
                .Wait(1);

            Assert.IsTrue(Browser.ElementsVisible(doc, "add-doc-window"));
        }

        [TestMethod]
        public void DocAddFromClientValidDoc()
        {
            var name = "Client Document " + rand.Next(int.MaxValue);

            DocAddFromClientButton();
            Browser.FillForm(doc, "add-doc-name", name)
                .UploadFile(doc, "add-doc-upload", "invalid.dll");

            Assert.IsTrue(Browser.ElementsVisible(doc, "invalid-file"));
        }

        [TestMethod]
        public void DocAddFromClientValidDoc2()
        {
            var name = "Client Document " + rand.Next(int.MaxValue);

            DocAddFromClientButton();
            Browser.FillForm(doc, "add-doc-name", name)
                .UploadFile(doc, "add-doc-upload", "invalid2.inf")
                .Click(doc, "save-button4");

            Assert.AreEqual("Sorry your 1 Attachment Invalid file out of 1.",
                Browser.FindElement(Common, "flash-message").Text);
        }

        [TestMethod]
        public void DocAddFromClientFakeDoc()
        {
            var name = "Client Document " + rand.Next(int.MaxValue);

            DocAddFromClientButton();
            Browser.FillForm(doc, "add-doc-name", name)
                .UploadFile(doc, "add-doc-upload", "fake.pdf")
                .Click(doc, "save-button4");

            Assert.AreEqual("Sorry your 1 Attachment Invalid file out of 1.",
                Browser.FindElement(Common, "flash-message").Text);
        }

        [TestMethod]
        public void DocAddFromClientSave()
        {
            var name = "Client Document " + rand.Next(int.MaxValue);

            DocAddFromClientButton();
            Browser.FillForm(doc, "add-doc-name", name)
                .UploadFile(doc, "add-doc-upload", "real.pdf")
                .Click(doc, "save-button4");

            Assert.AreEqual("Documents successfully Added.",
                Browser.FindElement(Common, "flash-message").Text);
        }
    }
}