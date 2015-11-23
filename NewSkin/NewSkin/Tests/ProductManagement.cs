using System;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NewSkin.Util;

namespace NewSkin.Tests
{
    [TestClass]
    public class ProductManagement : BaseTest
    {
        private LocatorReader _product;
        Random rand = new Random();

        [TestInitialize]
        public void TestInitialize()
        {
            Browser = Pegasus.LoginMyPeg ("seloffice");
            //Browser = Pegasus.LoginCom("seloffice");
            _product = new LocatorReader("ProductManagement.xml");
            Thread.Sleep(500);
        }

        private void GoToProductCategories()
        {
            GoToAdmin();
            Thread.Sleep(2000);
            Browser.MouseOver(_product, "product-tab")
                .Click(_product, "categories-link");
            Thread.Sleep(2000);
            Assert.AreEqual("Product Categories", Browser.Title);
        }

        private void GoToProducts()
        {
            GoToAdmin();
            Thread.Sleep(2000);
            Browser.MouseOver(_product, "product-tab")
                .Click(_product, "products-link");
            Thread.Sleep(2000);
            Assert.AreEqual("Products", Browser.Title);
        }

        //************** PRODUCT CATEGORIES **************
        [TestMethod]
        public void ProductCategoryCreateButton()
        {
            GoToProductCategories();
            Browser.Click(_product, "categories-create")
                .Wait(1);

            Assert.IsTrue(Browser.ElementsVisible(_product, "add-new-category"));
        }

        [TestMethod]
        public void ProductCategoryRequiredFields()
        {
            GoToProductCategories();
            Browser.Click(_product, "categories-create")
                .Wait(1)
                .Click(_product, "save-button");

            Assert.IsTrue(Browser.ElementCount(_product, "required-message") == 1);
            Assert.AreEqual("This field is required.",
                Browser.FindElement(_product, "required-message").Text);
        }

        [TestMethod]
        public void ProductCategorySave()
        {
            var name = "Category " + rand.Next(int.MaxValue);

            GoToProductCategories();
            Browser.Click(_product, "categories-create")
                .FillForm(_product, "categories-name", name)
                .Wait(1)
                .Click(_product, "save-button");

            Assert.AreEqual("Category Created Successfully",
                Browser.FindElement(Common, "flash-message").Text);
        }

        [TestMethod]
        public void ProductCategoryCancel()
        {
            var name = "Category " + rand.Next(int.MaxValue);

            GoToProductCategories();
            Browser.Click(_product, "categories-create")
                .FillForm(_product, "categories-name", name)
                .Click(_product, "cancel-button")
                .Wait(1);

            Assert.IsFalse(Browser.ElementsVisible(_product, "add-new-category"));
        }

        [TestMethod]
        public void ProductCategoryEdit()
        {
            GoToProductCategories();
            Browser.Click(_product, "categories-edit")
                .Wait(1);

            Assert.AreEqual("Edit Category", Browser.Title);
        }

        [TestMethod]
        public void ProductCategoryEditSave()
        {
            GoToProductCategories();
            Browser.Click(_product, "categories-edit")
                .Wait(1)
                .Click(_product, "save-button2");

            Assert.AreEqual("Category Updated Successfully",
                Browser.FindElement(Common, "flash-message").Text);
        }

        [TestMethod]
        public void ProductCategoryDelete()
        {
            GoToProductCategories();
            Browser.Click(_product, "categories-create")
                .FillForm(_product, "categories-name", "Test Delete")
                .Click(_product, "save-button")
                .Wait(1)
                .Click(_product, "categories-delete")
                .Wait(1)
                .AlertAccept()
                .Wait(1);

            Assert.AreEqual("Category Deleted Successfully",
                Browser.FindElement(Common, "flash-message").Text);
        }

        [TestMethod]
        public void ProductCategoryDuplicate()
        {
            var name = "Category " + rand.Next(int.MaxValue);

            GoToProductCategories();
            Browser.Click(_product, "categories-create")
                .FillForm(_product, "categories-name", name)
                .Click(_product, "save-button")
                .Wait(1)
                .Click(_product, "categories-create")
                .FillForm(_product, "categories-name", name)
                .Click(_product, "save-button")
                .Wait(1);

            Assert.AreEqual("Category Already Existed",
                Browser.FindElement(Common, "flash-message").Text);
        }

        [TestMethod]
        public void ProductCategoryAddProduct()
        {
            GoToProductCategories();
            Browser.Click(_product, "categories-edit")
                .Wait(1)
                .Click(_product, "categories-add-product-button")
                .Wait(1);

            Assert.IsTrue(Browser.ElementsVisible(_product, "categories-add-product-window"));
        }

        [TestMethod]
        public void ProductCategoryAddProductRequiredFields()
        {
            GoToProductCategories();
            Browser.Click(_product, "categories-edit")
                .Wait(1)
                .Click(_product, "categories-add-product-button")
                .FillForm(_product, "categories-product-name", "")
                .Click(_product, "save-button3");

            Assert.IsTrue(Browser.ElementCount(_product, "required-message") == 1);
        }

        [TestMethod]
        public void ProductCategoryAddProductSave()
        {
            var name = "Product " + rand.Next(int.MaxValue);

            GoToProductCategories();
            Browser.Click(_product, "categories-edit")
                .Wait(1)
                .Click(_product, "categories-add-product-button")
                .FillForm(_product, "categories-product-name", name)
                .Click(_product, "save-button3")
                .Wait(1)
                .Click(_product, "save-button2");

            Assert.AreEqual("Category Updated Successfully",
                Browser.FindElement(Common, "flash-message").Text);
        }

        //************** PRODUCTS **************
        [TestMethod]
        public void ProductCreateButton()
        {
            GoToProducts();
            Browser.Click(_product, "products-create");

            Assert.AreEqual("Products", Browser.Title);
        }

        [TestMethod]
        public void ProductRequiredFields()
        {
            GoToProducts();
            Browser.Click(_product, "products-create")
                .Wait(1)
                .Click(_product, "save-button2");

            Assert.IsTrue(Browser.ElementCount(_product, "required-message") == 1);
        }

        [TestMethod]
        public void ProductDuplicate()
        {
            var name = "Product " + rand.Next(int.MaxValue);

            GoToProducts();
            Browser.Click(_product, "products-create")
                .FillForm(_product, "products-name-field", name)
                .Click(_product, "save-button2")
                .Wait(1)
                .Click(_product, "products-create")
                .FillForm(_product, "products-name-field", name)
                .Click(_product, "save-button2");

            Assert.AreEqual("Product Already Exists",
                Browser.FindElement(Common, "flash-message").Text);
        }

        [TestMethod]
        public void ProductSave()
        {
            var name = "Product " + rand.Next(int.MaxValue);

            GoToProducts();
            Browser.Click(_product, "products-create")
                .FillForm(_product, "products-name-field", name)
                .Click(_product, "save-button2");

            Assert.AreEqual("Product Created Successfully",
                Browser.FindElement(Common, "flash-message").Text);
        }

        [TestMethod]
        public void ProductEdit()
        {
            GoToProducts();
            Browser.Click(_product, "products-edit")
                .Wait(1)
                .Click(_product, "save-button2");

            Assert.AreEqual("Product Updated Successfully",
                Browser.FindElement(Common, "flash-message").Text);
        }

        [TestMethod]
        public void ProductDelete()
        {
            GoToProducts();
            Browser.Click(_product, "products-create")
                .FillForm(_product, "products-name-field", "Test Delete")
                .Click(_product, "save-button2")
                .Wait(1)
                .Click(_product, "products-delete")
                .Wait(1)
                .AlertAccept()
                .Wait(1);

            Assert.AreEqual("Product Deleted Successfully",
                Browser.FindElement(Common, "flash-message").Text);
        }

        //************** Custom Fields **************
        [TestMethod]
        public void ProductCustomFieldButton()
        {
            GoToProducts();
            Browser.Click(_product, "products-create")
                .Wait(1)
                .Click(_product, "product-custom-field")
                .Wait(1);

            Assert.IsTrue(Browser.ElementsVisible(_product, "custom-field-window"));
        }

        [TestMethod]
        public void ProductCustomFieldRequiredFields()
        {
            ProductCustomFieldButton();
            Browser.Click(_product, "save-button");

            Assert.IsTrue(Browser.ElementCount(_product, "required-message") == 2);
        }

        [TestMethod]
        public void ProductCustomFieldDigitInput()
        {
            ProductCustomFieldButton();
            Browser.FillForm(_product, "custom-field-name", "test")
                .DropdownSelectByText(_product, "custom-field-type", "Text Box")
                .DropdownSelectByText(_product, "custom-field-content", "Number")
                .FillForm(_product, "custom-max-range", "test")
                .Click(_product, "save-button");

            Assert.IsTrue(Browser.ElementCount(_product, "digit-message") == 1);
            Assert.AreEqual("Please enter only digits.", 
                Browser.FindElement(_product, "digit-message").Text);
        }

        [TestMethod]
        public void ProductCustomFieldAddOption()
        {
            ProductCustomFieldButton();
            Browser.FillForm(_product, "custom-field-name", "test")
                .DropdownSelectByText(_product, "custom-field-type", "Drop Down")
                .Click(_product, "custom-add-another");

            Assert.IsTrue(Browser.ElementCount(_product, "custom-options") == 2);
        }

        [TestMethod]
        public void ProductCustomFieldValidEmail()
        {
            var name = "Custom Field " + rand.Next(int.MaxValue);

            GoToProducts();
            Browser.Click(_product, "products-edit")
                .Wait(1)
                .Click(_product, "product-custom-field")
                .FillForm(_product, "custom-field-name2", name)
                .DropdownSelectByText(_product, "custom-field-type2", "Text Box")
                .DropdownSelectByText(_product, "custom-field-content2", "Text")
                .DropdownSelectByText(_product, "custom-data-valid", "E-Mail")
                .Click(_product, "save-button")
                .Wait(1)
                .Click(_product, "save-button2")
                .Wait(1);

            Thread.Sleep(500);
            GoToMain();
            Thread.Sleep(500);

            Browser.Click(_product, "client")
                .Click(_product, "client-create")
                .DropdownSelectByText(_product, "client-status", "New")
                .DropdownSelectByText(_product, "client-responsibility", "Brian Sales Agent")
                .Click(_product, "client-company-details")
                .FillForm(_product, "client-DBA", name)
                .Click(_product, "client-products")
                .Click(_product, "client-products-box")
                .FillForm(_product, "client-products-entry", "test")
                .Click(_product, "save-button");

            Assert.IsTrue(Browser.ElementCount(_product, "email-message") == 1);
            Assert.AreEqual("Please enter a valid email address.",
                Browser.FindElement(_product, "email-message").Text);
        }
    }
}