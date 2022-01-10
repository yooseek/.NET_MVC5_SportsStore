using Moq;
using Ninject;
using SportsStore.Domain.Context;
using SportsStore.Domain.Entities;
using SportsStore.Domain.IProductRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SportsStore.WebUI.Infrastructure
{
    public class DependencyResolver : IDependencyResolver
    {
        private IKernel kernel;
        public DependencyResolver(IKernel kernelParam)
        {
            kernel = kernelParam;
            AddBinding();
        }

        private void AddBinding()
        {
            // 바인딩 정보 입력   
            /*
            // 임시 mock 연결
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new List<Product>
            {
                new Product{Name="Football", Price=25},
                new Product{Name="Surf board", Price=179},
                new Product{Name="Runnig shoes", Price=95},
               // new Product{Name="Book", Price=125},
            });
            
            kernel.Bind<IProductRepository>().ToConstant(mock.Object);
            */

            kernel.Bind<IProductRepository>().To<EFProductRepository>();
        }
    
        public object GetService(Type serviceType)
        {
            return kernel.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return kernel.GetAll(serviceType);
        }
    }
}