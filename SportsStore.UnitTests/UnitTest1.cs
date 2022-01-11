using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using SportsStore.Domain.Context;
using SportsStore.Domain.Entities;
using SportsStore.Domain.IProductRepo;
using SportsStore.WebUI.Controllers;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using SportsStore.WebUI.Models;
using SportsStore.WebUI.HtmlHelpers;

namespace SportsStore.UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        /// <summary>
        /// 상품정보 출력 테스트
        /// </summary>
        [TestMethod]
        public void TestMethod1()
        {
            // Arrange
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]
            {
                new Product{ProductID =1, Name="P1"},
                new Product{ProductID =2, Name="P2"},
                new Product{ProductID =3, Name="P3"},
                new Product{ProductID =4, Name="P4"},
                new Product{ProductID =5, Name="P5"},
            });
            ProductController controller = new ProductController(mock.Object);
            controller.PageSize = 3;

            // Act
            ProductListViewModel result = (ProductListViewModel)controller.List(null,2).Model;

            // Assert
            Product[] prodArray = result.Products.ToArray();
            Assert.IsTrue(prodArray.Length == 2);
            Assert.AreEqual(prodArray[0].Name, "P4");
            Assert.AreEqual(prodArray[1].Name, "P5");
        }
        /// <summary>
        /// 페이지 정보 테스트
        /// </summary>
        [TestMethod]
        public void Can_Generate_Page_Links()
        {
            // Arrange
            HtmlHelper myHelper = null;
            // PageInfo 데이터를 생성한다.
            PageInfo pageInfo = new PageInfo
            {
                CurrentPage = 2,
                TotalItems = 28,
                ItemsPerPage = 10
            };
            Func<int, string> pageUrlDelegate = i => "page" + i;

            // Act
            MvcHtmlString result = myHelper.PageLinks(pageInfo, pageUrlDelegate);

            // Assert
            Assert.AreEqual(@"<a class=""btn btn-default"" href=""page1"">1</a>" + @"<a class=""btn btn-default btn-primary selected"" href=""page2"">2</a>" +
                @"<a class=""btn btn-default"" href=""page3"">3</a>", result.ToString());
        }
        /// <summary>
        /// 최종 페이지 출력 테스트
        /// </summary>
        [TestMethod]
        public void Can_Generate_Page_ViewModel()
        {
            // Arrange
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]
            {
                new Product{ProductID =1, Name="P1"},
                new Product{ProductID =2, Name="P2"},
                new Product{ProductID =3, Name="P3"},
                new Product{ProductID =4, Name="P4"},
                new Product{ProductID =5, Name="P5"},
            });
            ProductController controller = new ProductController(mock.Object);
            controller.PageSize = 3;

            // Act
            ProductListViewModel result = (ProductListViewModel)controller.List(null, 2).Model;

            // Assert

            PageInfo pageInfo = result.PageInfo;
            Assert.AreEqual(pageInfo.CurrentPage, 2);
            Assert.AreEqual(pageInfo.ItemsPerPage, 3);
            Assert.AreEqual(pageInfo.TotalItems, 5);
            Assert.AreEqual(pageInfo.TotalPages, 2);
        }
        /// <summary>
        /// 카테고리 필터 테스트
        /// </summary>
        [TestMethod]
        public void Can_Generate_Filter_Category()
        {
            // Arrange
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]
            {
                new Product{ProductID =1, Name="P1", Category ="Cat1"},
                new Product{ProductID =2, Name="P2", Category ="Cat2"},
                new Product{ProductID =3, Name="P3", Category ="Cat1"},
                new Product{ProductID =4, Name="P4", Category ="Cat2"},
                new Product{ProductID =5, Name="P5", Category ="Cat3"},
            });
            ProductController controller = new ProductController(mock.Object);
            controller.PageSize = 3;

            // Act
            Product[] result = ((ProductListViewModel)controller.List("Cat2", 1).Model).Products.ToArray();

            // Assert
            Assert.AreEqual(result.Length,2);
            Assert.IsTrue(result[0].Name == "P2" && result[0].Category == "Cat2");
            Assert.IsTrue(result[1].Name == "P4" && result[1].Category == "Cat2");
        }
        /// <summary>
        /// 카테고리 목록 테스트
        /// </summary>
        [TestMethod]
        public void Can_Create_Category()
        {
            // Arrange
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]
            {
                new Product{ProductID =1, Name="P1", Category ="Cat1"},
                new Product{ProductID =2, Name="P2", Category ="Cat2"},
                new Product{ProductID =3, Name="P3", Category ="Cat1"},
                new Product{ProductID =4, Name="P4", Category ="Cat2"},
                new Product{ProductID =5, Name="P5", Category ="Cat3"},
            });
            NavController controller = new NavController(mock.Object);
            
            // Act
            string[] result = ((IEnumerable<string>)controller.Menu().Model).ToArray();

            // Assert
            Assert.AreEqual(result.Length, 3);
            Assert.AreEqual(result[0], "Cat1");
            Assert.AreEqual(result[1], "Cat2");
            Assert.AreEqual(result[2], "Cat3");

        }
        /// <summary>
        /// 카테고리 체크 테스트
        /// </summary>
        [TestMethod]
        public void Check_Selected_Category()
        {
            // Arrange
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]
            {
                new Product{ProductID =1, Name="P1", Category ="Cat1"},
                new Product{ProductID =2, Name="P2", Category ="Cat2"},
                new Product{ProductID =3, Name="P3", Category ="Cat1"},
                new Product{ProductID =4, Name="P4", Category ="Cat2"},
                new Product{ProductID =5, Name="P5", Category ="Cat3"},
            });
            NavController controller = new NavController(mock.Object);
            string CategoryToSelected = "Cat1";
            
            // Act
            string result = controller.Menu(CategoryToSelected).ViewBag.SelectedCategory;

            // Assert
            Assert.AreEqual(CategoryToSelected,result);

        }
        /// <summary>
        /// 카테고리별 페이징 테스트
        /// </summary>
        [TestMethod]
        public void Check_Category_Page()
        {
            // Arrange
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]
            {
                new Product{ProductID =1, Name="P1", Category ="Cat1"},
                new Product{ProductID =2, Name="P2", Category ="Cat2"},
                new Product{ProductID =3, Name="P3", Category ="Cat1"},
                new Product{ProductID =4, Name="P4", Category ="Cat2"},
                new Product{ProductID =5, Name="P5", Category ="Cat3"},
            });
            ProductController controller = new ProductController(mock.Object);
            controller.PageSize = 1;
            
            // Act
            int result1 = ((ProductListViewModel)controller.List("Cat1").Model).PageInfo.TotalItems;
            int result2 = ((ProductListViewModel)controller.List("Cat2").Model).PageInfo.TotalItems;
            int result3 = ((ProductListViewModel)controller.List("Cat3").Model).PageInfo.TotalItems;
            int result4 = ((ProductListViewModel)controller.List(null).Model).PageInfo.TotalItems;

            // Assert
            Assert.AreEqual(result1,2);
            Assert.AreEqual(result2,2);
            Assert.AreEqual(result3,1);
            Assert.AreEqual(result4,5);

        }
    }
}
