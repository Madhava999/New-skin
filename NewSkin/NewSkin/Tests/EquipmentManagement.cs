using System;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NewSkin.Util;

namespace NewSkin.Tests
{
    [TestClass]
    public class EquipmentManagement : BaseTest
    {
        private LocatorReader _equipment;
        Random rand = new Random();

        [TestInitialize]
        public void TestInitialize()
        {
            Browser = Pegasus.LoginCom("seloffice");
            _equipment = new LocatorReader("EquipmentManagement.xml");
            Thread.Sleep(500);
        }

        private void GoToCreate(String link, String create)
        {
            GoToAdmin();
            Thread.Sleep(2000);
            Browser.MouseOver(_equipment, "equipment-tab")
                .Click(_equipment, link);
            Thread.Sleep(2000);
            Browser.Click(_equipment, create);
            Thread.Sleep(2000);
        }

        //************** EQUIPMENT **************
        [TestMethod]
        public void EquipmentLinkWorks()
        {
            GoToAdmin();
            Thread.Sleep(2000);
            Browser.MouseOver(_equipment, "equipment-tab")
                .Click(_equipment, "equip-link")
                .Wait(1);

            Assert.AreEqual("Equipment", Browser.Title);
        }

        [TestMethod]
        public void EquipmentCreateButton()
        {
            GoToCreate("equip-link", "equip-create-button");
            Browser.Wait(1);

            Assert.AreEqual("Equipment Create", Browser.Title);
        }

        [TestMethod]
        public void EquipmentRequiredFields()
        {
            GoToCreate("equip-link", "equip-create-button");
            Browser.Click(_equipment, "equip-save-button");

            Assert.IsTrue(Browser.ElementCount(_equipment, "required-message") == 3);
            Assert.AreEqual("This field is required.",
                Browser.FindElement(_equipment, "required-message").Text);
        }

        [TestMethod]
        public void EquipmentRequiredAddAnotherVersion()
        {
            var name = "Equipment " + rand.Next(int.MaxValue);

            GoToCreate("equip-link", "equip-create-button");
            Browser.FillForm(_equipment, "equip-name-field", name)
                .DropdownSelectByText(_equipment, "equip-type-field", "Check Reader")
                .FillForm(_equipment, "equip-version-field", "1")
                .Click(_equipment, "equip-add-another-button")
                .Click(_equipment, "equip-save-button");

            Assert.IsTrue(Browser.ElementCount(_equipment, "required-message") == 1);
            //uncomment when clicking button on a menu bar doesn't expand/collapse the section
            //Assert.AreEqual("This field is required.",
            //    Browser.FindElement(_equipment, "required-message").Text);
        }

        [TestMethod]
        public void EquipmentAddAnotherVersion()
        {
            int num = rand.Next(int.MaxValue);

            GoToCreate("equip-link", "equip-create-button");
            Browser.FillForm(_equipment, "equip-name-field", "Equipment " + num)
                .DropdownSelectByText(_equipment, "equip-type-field", "Check Reader")
              .FillForm(_equipment, "equip-version-field", "1")
                .Click(_equipment, "equip-add-another-button")
                .FillForm(_equipment, "equip-another-field", "2")
                .Click(_equipment, "equip-save-button");

            Assert.AreEqual("Equipment saved successfully",
                Browser.FindElement(Common, "flash-message").Text);
        }

        [TestMethod]
        public void EquipmentSave()
        {
            int num = rand.Next(int.MaxValue);

            GoToCreate("equip-link", "equip-create-button");
            Browser.FillForm(_equipment, "equip-name-field", "Equipment " + num)
                .DropdownSelectByText(_equipment, "equip-type-field", "Check Reader")
                .FillForm(_equipment, "equip-version-field", "1")
                .Click(_equipment, "equip-save-button");

            Assert.AreEqual("Equipment saved successfully", 
                Browser.FindElement(Common, "flash-message").Text);
        }

        [TestMethod]
        public void EquipmentCancel()
        {
            GoToCreate("equip-link", "equip-create-button");
            Browser.Click(_equipment, "cancel-button")
                .Wait(1);

            Assert.AreEqual("Equipment", Browser.Title);
        }

        //************** VENDORS **************
        [TestMethod]
        public void VendorLinkWorks()
        {
            GoToAdmin();
            Thread.Sleep(2000);
            Browser.MouseOver(_equipment, "equipment-tab")
                .Click(_equipment, "vendor-link")
                .Wait(1);

            Assert.AreEqual("Vendors", Browser.Title);
        }

        [TestMethod]
        public void VendorCreateButton()
        {
            GoToCreate("vendor-link", "vendor-create-button");
            Browser.Wait(1);

            Assert.AreEqual("Create a New Vendor", Browser.Title);
        }

        [TestMethod]
        public void VendorRequiredFields()
        {
            GoToCreate("vendor-link", "vendor-create-button");
            Browser.Click(_equipment, "save-button");

            Assert.IsTrue(Browser.ElementCount(_equipment, "required-message") == 7);
            Assert.AreEqual("This field is required.",
                Browser.FindElement(_equipment, "required-message").Text);
        }

        [TestMethod]
        public void VendorValidEmail()
        {
            GoToCreate("vendor-link", "vendor-create-button");
            Browser.DropdownSelectByText(_equipment.Get("vendor-etype"), "E-Mail")
                .DropdownSelectByText(_equipment.Get("vendor-elabel"), "Work")
                .FillForm(_equipment.Get("vendor-eaddress"), "test")
                .Click(_equipment.Get("save-button"));

            Assert.IsTrue(Browser.ElementCount(_equipment.Get("invalid-email-message")) == 1);
            Assert.AreEqual("Please enter a valid email address.",
                Browser.FindElement(_equipment, "invalid-email-message").Text);
        }

        [TestMethod]
        public void VendorValidURLs()
        {
            GoToCreate("vendor-link", "vendor-create-button");
            Browser.FillForm(_equipment, "vendor-website", "test")
                .FillForm(_equipment, "vendor-linkedin", "test")
                .FillForm(_equipment, "vendor-facebook", "test")
                .FillForm(_equipment, "vendor-twitter", "test")
                .Click(_equipment, "save-button");

            Assert.IsTrue(Browser.ElementCount(_equipment, "invalid-url-message") == 4);
            Assert.AreEqual("Please enter a valid URL.",
                Browser.FindElement(_equipment, "invalid-url-message").Text);
        }

        [TestMethod]
        public void VendorSave()
        {
            int num = rand.Next(int.MaxValue);

            GoToCreate("vendor-link", "vendor-create-button");
            //Browser.DropdownSelectByText(_equipment, "vendor-type", "..."));
            Browser.FillForm(_equipment, "vendor-name", "Vendor " + num)
                .FillForm(_equipment, "vendor-first", "first")
                .FillForm(_equipment, "vendor-last", "last")
                .DropdownSelectByText(_equipment, "vendor-etype", "E-Mail")
                .DropdownSelectByText(_equipment, "vendor-elabel", "Work")
                .FillForm(_equipment, "vendor-eaddress", "test@test.com")
                .Click(_equipment, "save-button");

            Assert.AreEqual("Vendor saved successfully",
                Browser.FindElement(Common, "flash-message").Text);
        }

        [TestMethod]
        public void VendorCancel()
        {
            GoToCreate("vendor-link", "vendor-create-button");
            Browser.Click(_equipment, "cancel-button")
                .Wait(1);

            Assert.AreEqual("Vendors", Browser.Title);
        }

        //************** Download IDs **************
        [TestMethod]
        public void DownloadIdLinkWorks()
        {
            GoToAdmin();
            Thread.Sleep(2000);
            Browser.MouseOver(_equipment, "equipment-tab")
                .Click(_equipment, "downloadid-link")
                .Wait(1);

            Assert.AreEqual("Download IDs", Browser.Title);
        }

        [TestMethod]
        public void DownloadIdCreateButton()
        {
            GoToCreate("downloadid-link", "downloadid-create-button");
            Browser.Wait(1);

            Assert.AreEqual("Manage Master Equipment Type Download Ids", Browser.Title);
        }

        [TestMethod]
        public void DownloadIdRequiredFields()
        {
            GoToCreate("downloadid-link", "downloadid-create-button");
            Browser.Click(_equipment, "save-button");

            Assert.IsTrue(Browser.ElementCount(_equipment, "required-message") == 3);
        }

        [TestMethod]
        public void DownloadIdSave()
        {
            int num = rand.Next(int.MaxValue);

            GoToCreate("downloadid-link", "downloadid-create-button");
            Browser.DropdownSelectByText(_equipment, "downloadid-type", "Terminal")
                .FillForm(_equipment, "downloadid-name", "DownloadID " + num)
                .FillForm(_equipment, "downloadid-id", num.ToString())
                .Click(_equipment, "save-button");

            Assert.AreEqual("The download id is successfully created!!",
                Browser.FindElement(Common, "flash-message").Text);
        }

        [TestMethod]
        public void DownloadIdCancel()
        {
            GoToCreate("downloadid-link", "downloadid-create-button");
            Browser.Click(_equipment, "cancel-button")
                .Wait(1);

            Assert.AreEqual("Download IDs", Browser.Title);
        }

        //************** Shipping Carriers **************
        [TestMethod]
        public void ShippingLinkWorks()
        {
            GoToAdmin();
            Thread.Sleep(2000);
            Browser.MouseOver(_equipment, "equipment-tab")
                .Click(_equipment, "shipping-link")
                .Wait(1);

            Assert.AreEqual("Shipping Carriers", Browser.Title);
        }

        [TestMethod]
        public void ShippingCreateButton()
        {
            GoToCreate("shipping-link", "shipping-create-button");
            Browser.Wait(1);

            Assert.AreEqual("Manage Shipping Carrier", Browser.Title);
        }

        [TestMethod]
        public void ShippingRequiredFields()
        {
            GoToCreate("shipping-link", "shipping-create-button");
            Browser.Click(_equipment, "save-button")
                .Wait(1);

            Assert.IsTrue(Browser.ElementCount(_equipment, "required-message") == 2);
        }

        [TestMethod]
        public void ShippingValidURLs()
        {
            int num = rand.Next(int.MaxValue);

            GoToCreate("shipping-link", "shipping-create-button");
            Browser.FillForm(_equipment, "shipping-name", "Shipping Carrier " + num)
                .FillForm(_equipment, "shipping-website", "test")
                .FillForm(_equipment, "shipping-tracking", "test")
                .Click(_equipment, "save-button");

            Assert.IsTrue(Browser.ElementCount(_equipment, "invalid-url-message") == 1);
            Assert.IsTrue(Browser.ElementCount(_equipment, "invalid-url-message2") == 1);
        }

        [TestMethod]
        public void ShippingDuplicate()
        {
            int num = rand.Next(int.MaxValue);

            GoToCreate("shipping-link", "shipping-create-button");
            Browser.FillForm(_equipment, "shipping-name", "Shipping Carrier " + num)
                .FillForm(_equipment, "shipping-website", "http://www.test.com")
                .FillForm(_equipment, "shipping-tracking", "http://www.test.com")
                .Click(_equipment, "save-button")
                .Wait(1)
                .Click(_equipment, "shipping-create-button")
                .FillForm(_equipment, "shipping-name", "Shipping Carrier " + num)
                .FillForm(_equipment, "shipping-website", "http://www.test.com")
                .FillForm(_equipment, "shipping-tracking", "http://www.test.com")
                .Click(_equipment, "save-button");

            Assert.AreEqual("The shipping carrier is already exists.",
                Browser.FindElement(Common, "flash-message").Text);
        }

        [TestMethod]
        public void ShippingSave()
        {
            int num = rand.Next(int.MaxValue);

            GoToCreate("shipping-link", "shipping-create-button");
            Browser.FillForm(_equipment, "shipping-name", "Shipping Carrier " + num)
                .FillForm(_equipment, "shipping-website", "http://www.test.com")
                .FillForm(_equipment, "shipping-tracking", "http://www.test.com")
                .Click(_equipment, "save-button");

            Assert.AreEqual("The shipping carrier is successfully created",
                Browser.FindElement(Common, "flash-message").Text);
        }

        [TestMethod]
        public void ShippingCancel()
        {
            GoToCreate("shipping-link", "shipping-create-button");
            Browser.Click(_equipment, "cancel-button")
                .Wait(1);

            Assert.AreEqual("Shipping Carriers", Browser.Title);
        }
    }
}