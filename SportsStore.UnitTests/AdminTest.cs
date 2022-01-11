using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SportsStore.Domain.Entities;
using SportsStore.Domain.IProductRepo;
using SportsStore.WebUI.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsStore.UnitTests
{
    [TestClass]
    public class AdminTest
    {
        /// <summary>
        ///  인덱스 테스트
        /// </summary>
        [TestMethod]
        public void Index_Contain_Product()
        {
            // Arrange
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(s => s.Products).Returns(new Product[]
            {
                new Product {ProductID =1,Name ="P1"},
                new Product {ProductID =2,Name ="P2"},
                new Product {ProductID =3,Name ="P3"},
            });
            AdminController target = new AdminController(mock.Object);

            // Action
            Product[] result = ((IEnumerable<Product>)target.Index().ViewData.Model).ToArray();

            // Assert
            Assert.AreEqual(result.Length, 3);
            Assert.AreEqual("P1",result[0].Name);
            Assert.AreEqual("P2",result[1].Name);
            Assert.AreEqual("P3",result[2].Name);

        }
        /// <summary>
        /// 어드민 수정 테스트
        /// </summary>
        [TestMethod]
        public void Can_Edit_Product()
        {
            // Arrange
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(s => s.Products).Returns(new Product[]
            {
                new Product {ProductID =1,Name ="P1"},
                new Product {ProductID =2,Name ="P2"},
                new Product {ProductID =3,Name ="P3"},
            });
            AdminController target = new AdminController(mock.Object);

            // Action
            Product p1 = target.Edit(1).ViewData.Model as Product;
            Product p2 = target.Edit(2).ViewData.Model as Product;
            Product p3 = target.Edit(3).ViewData.Model as Product;
            Product p4 = target.Edit(4).ViewData.Model as Product;

            // Assert
            Assert.AreEqual(1,p1.ProductID);
            Assert.AreEqual(2,p2.ProductID);
            Assert.AreEqual(3,p3.ProductID);
            Assert.IsNull(p4);


        }
    }
}
