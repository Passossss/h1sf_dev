using H1SF.Application.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace H1SF.TestProject.Tests
{
    [TestClass]
    public class DescargaDanfeTests
    {
        [TestMethod]
        public void Danfe_InterfaceExists()
        {
            // Arrange & Act
            Type interfaceType = typeof(IDanfeService);

            // Assert
            Assert.IsNotNull(interfaceType);
            Assert.IsTrue(interfaceType.IsInterface);
        }

        [TestMethod]
        public void DanfeService_ImplementsInterface()
        {
            // Arrange & Act
            Type serviceType = typeof(DanfeService);
            Type interfaceType = typeof(IDanfeService);

            // Assert
            Assert.IsTrue(interfaceType.IsAssignableFrom(serviceType));
        }

        [TestMethod]
        public void DanfeService_HasDescarregaViasDanfeMethod()
        {
            // Arrange & Act
            Type serviceType = typeof(DanfeService);
            var method = serviceType.GetMethod("DescarregaViasDanfe");

            // Assert
            Assert.IsNotNull(method);
        }
    }
}
