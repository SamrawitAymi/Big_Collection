using Microsoft.VisualStudio.TestTools.UnitTesting;
using Products.Model;
using Products.Repository;
using Products.UnitTest.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Products.UnitTest
{
    [TestClass]
    public class ProductsTest
    {

        public static IProductRepository ProductRepository { get; set; }
        public static TestProductsDbContext ProductTestContext { get; set; }

        [ClassInitialize]
        public static void TestFixtureSetup(TestContext context)
        {
            ProductTestContext = new TestProductsDbContext();
            ProductRepository = new ProductRepository(ProductTestContext.DbContext);
        }


        [TestMethod]
        public void CreateNewProduct_CreatingNewProduct_ReturnCreatedNewProduct()
        {
            //Arrange
            var dummyProduct = DummyProduct.Product();

            //Act
            var newProduct = ProductRepository.CreateProductAsync(dummyProduct).Result;

            //Assert
            Assert.AreEqual(dummyProduct, newProduct);
            DeleteDummyProductFromDatabase(dummyProduct);
        }

        [TestMethod]
        public void CreateNullProduct_CreatingNullProduct_ReturnNull()
        {
            //Arrange
            Product dummyProduct = null;
            //Act
            var dummyNewProduct = ProductRepository.CreateProductAsync(dummyProduct).Result;
            //Assert
            Assert.AreEqual(dummyProduct, null);
        }

        [TestMethod]
        public void CreateProdut_CreatingExistingProductInDatabase_ReturnNull()
        {
            //Arrange
            var dummyProduct1 = DummyProduct.Product();
            var dummyProduct2 = dummyProduct1;

            //Act
            var reCreateProduct1 = ProductRepository.CreateProductAsync(dummyProduct1).Result;
            var reCreateProduct2 = ProductRepository.CreateProductAsync(dummyProduct2).Result;

            //Assert
            Assert.AreEqual(reCreateProduct2, null);
        }

        [TestMethod]
        public void GetProductById_TryToGetProductById_ReturnProduct()
        {
            //Arrange
            var dummyProduct = CreateDummyProductToDatabase();

            //Act
            var product = ProductRepository.GetProductById(dummyProduct.Id).Result;

            //Assert
            Assert.AreEqual(product.Id, dummyProduct.Id);
            DeleteDummyProductFromDatabase(dummyProduct);
        }

        [TestMethod]
        public void GetProductByCategoryName_TryToGetProductByCategoryName_ReturnProductbyCategory()
        {
            //Arrange
            var dummyProduct = CreateDummyProductToDatabase();
            var searchString = "Electronics";

            //Act
            var products = ProductRepository.GetProductByCategory(dummyProduct.Name, searchString).Result;

            //Assert
            Assert.IsInstanceOfType(products, typeof(IEnumerable<Product>));
            DeleteDummyProductFromDatabase(dummyProduct);
        }

        [TestMethod]
        public void UpdateProduct_TryToUpdateValidProduct_ReturnUpdatedProduct()
        {
            //Arrange
            Product dummyProduct = CreateDummyProductToDatabase();
            var oldName = "Red Emma Willis Wrap Dress";

            //Act
            dummyProduct.Name = "Orange Tie Front Midi Dress";
            var product = ProductRepository.UpdateProduct(dummyProduct).Result;

            //Assert
            Assert.AreNotEqual(oldName, product.Name);
            Assert.AreEqual(dummyProduct.Name, product.Name);
            DeleteDummyProductFromDatabase(dummyProduct);
        }

        [TestMethod]
        public void UpdateProduct_TryToUpdateProductNotExistInDatabase_ReturnNull()
        {
            //Arrange
            var dummyProduct = DummyProduct.Product();
            //Act
            var updateNullProduct = ProductRepository.UpdateProduct(dummyProduct).Result;
            //Assert
            Assert.IsNull(updateNullProduct);
        }

        [TestMethod]
        public void DeleteProductByIdFromDb_DeletingProductByIdFromDb_ReturnProductDeletedFromDatabase()
        {
            //Arrange
            Product dummyProduct = CreateDummyProductToDatabase();

            //Act
            var deleteProduct = ProductRepository.DeleteProductById(dummyProduct.Id).Result;

            //Assert
            Assert.AreEqual(dummyProduct, deleteProduct);
        }


        [TestMethod]
        public void DeleteProductById_TryToDeleteNotExistProductByIdFromDb_ReturnNull()
        {
            //Arrange
            var dummyProductId = Guid.NewGuid();
            //Act
            var deleteNullProduct = ProductRepository.DeleteProductById(dummyProductId).Result;
            //Assert
            Assert.IsNull(deleteNullProduct);
        }

        [TestMethod]
        public void GetAllProductsFromDatabase_TryToGetAllProducts_ReturnAllProductList()
        {
            //Arrange

            //Act
            var allProductList = ProductRepository.GetAllProducts().Result;

            //Assert
            Assert.IsInstanceOfType(allProductList, typeof(IEnumerable<Product>));
        }

        [TestMethod]
        public void GetAllProducts_TryToGetProductsFromNullDb_ReturnNull()
        {
            //Act
            var productList = ProductRepository.GetAllProducts().Result;
            productList = null;

            //Assert
            Assert.IsNull(productList);
        }



        private static void DeleteDummyProductFromDatabase(Product newProduct)
        {
            ProductTestContext.DbContext.Remove(newProduct);
            ProductTestContext.DbContext.SaveChanges();
        }
        private static Product CreateDummyProductToDatabase()
        {
            var dummyProduct = DummyProduct.Product();
            ProductTestContext.DbContext.Product.Add(dummyProduct);
            ProductTestContext.DbContext.SaveChanges();
            return dummyProduct;
        }
    }
}
