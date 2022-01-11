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
using static SportsStore.Domain.Entities.Cart;

namespace SportsStore.UnitTests
{
    [TestClass]
    public class Class1
    {
        /// <summary>
        /// 카트 테스트
        /// </summary>
        [TestMethod]
        public void Cart_Test()
        {
            // Arrange
            Product p1 = new Product { ProductID = 1, Name = "P1" };
            Product p2 = new Product { ProductID = 2, Name = "P2" };
            Cart target = new Cart();

            // Act
            target.AddItem(p1, 1);
            target.AddItem(p2, 1);
            CartLine[] result = target.Lines.ToArray();

            // Assert
            Assert.AreEqual(result.Length, 2);
            Assert.AreEqual(result[0].Product, p1);
            Assert.AreEqual(result[1].Product, p2);
        }
        /// <summary>
        /// 카트 추가 테스트
        /// </summary>
        [TestMethod]
        public void Cart_Add_Test()
        {
            // Arrange
            Product p1 = new Product { ProductID = 1, Name = "P1" };
            Product p2 = new Product { ProductID = 2, Name = "P2" };
            Cart target = new Cart();

            // Act
            target.AddItem(p1, 1);
            target.AddItem(p2, 1);
            target.AddItem(p1, 10);
            CartLine[] result = target.Lines.OrderBy(c=>c.Product.ProductID).ToArray();

            // Assert
            Assert.AreEqual(result.Length, 2);
            Assert.AreEqual(result[0].Quantity, 11);
            Assert.AreEqual(result[1].Quantity, 1);

        }
        /// <summary>
        /// 카트 삭제 테스트
        /// </summary>
        [TestMethod]
        public void Cart_Delete_Test()
        {
            // Arrange
            Product p1 = new Product { ProductID = 1, Name = "P1" };
            Product p2 = new Product { ProductID = 2, Name = "P2" };
            Product p3 = new Product { ProductID = 3, Name = "P3" };
            Cart target = new Cart();
            target.AddItem(p1, 1);
            target.AddItem(p2, 1);
            target.AddItem(p3, 5);
            target.AddItem(p2, 3);

            // Act
            target.RemoveLine(p2);

            // Assert
            Assert.AreEqual(target.Lines.Where(c=>c.Product==p2).Count(),0);
            Assert.AreEqual(target.Lines.Count(),2);

        }
        /// <summary>
        /// 카트 합계 테스트
        /// </summary>
        [TestMethod]
        public void Cart_Sum_Test()
        {
            // Arrange
            Product p1 = new Product { ProductID = 1, Name = "P1",Price = 100M };
            Product p2 = new Product { ProductID = 2, Name = "P2" ,Price= 50M};
            Product p3 = new Product { ProductID = 3, Name = "P3" , Price= 30M};
            Cart target = new Cart();
            target.AddItem(p1, 1);
            target.AddItem(p2, 1);
            target.AddItem(p2, 3);

            // Act
            decimal total = target.ComputeTotalValue();

            // Assert
            Assert.AreEqual(total, 300M);
        }
        /// <summary>
        /// 카트 비우기 테스트
        /// </summary>
        [TestMethod]
        public void Cart_Clear_Test()
        {
            // Arrange
            Product p1 = new Product { ProductID = 1, Name = "P1", Price = 100M };
            Product p2 = new Product { ProductID = 2, Name = "P2", Price = 50M };
            Product p3 = new Product { ProductID = 3, Name = "P3", Price = 30M };
            Cart target = new Cart();
            target.AddItem(p1, 1);
            target.AddItem(p2, 1);
            target.AddItem(p2, 3);

            // Act
            target.Clear();

            // Assert
            Assert.AreEqual(target.Lines.Count(),0);
        }
        /// <summary>
        /// 카트에 추가 테스트
        /// </summary>
        [TestMethod]
        public void Can_Add_To_Cart()
        {
            // Arrange
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(s => s.Products).Returns(new Product[]
            {
                new Product{ProductID=1, Name="P1",Category ="Apples"}
            }.AsQueryable());

            Cart cart = new Cart();
            CartController cartController = new CartController(mock.Object);

            // Act
            cartController.AddToCart(cart, 1, null);

            // Assert
            Assert.AreEqual(cart.Lines.Count(), 1);
            Assert.AreEqual(cart.Lines.ToArray()[0].Product.ProductID, 1);
        }
       
    }
}
