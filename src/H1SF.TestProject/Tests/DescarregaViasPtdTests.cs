using H1SF.Application.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace H1SF.TestProject.Tests
{
    [TestClass]
    public class DescarregaViasPtdTests
    {
        [TestMethod]
        public void DescarregaViasPtd_InterfaceExists()
        {
            // Arrange & Act
            Type interfaceType = typeof(IDescarregaViasPtdService);

            // Assert
            Assert.IsNotNull(interfaceType);
            Assert.IsTrue(interfaceType.IsInterface);
        }

        [TestMethod]
        public void DescarregaViasPtdService_ImplementsInterface()
        {
            // Arrange & Act
            Type serviceType = typeof(DescarregaViasPtdService);
            Type interfaceType = typeof(IDescarregaViasPtdService);

            // Assert
            Assert.IsTrue(interfaceType.IsAssignableFrom(serviceType));
        }

        [TestMethod]
        public void DescarregaViasPtdService_HasExecuteMethod()
        {
            // Arrange & Act
            Type serviceType = typeof(DescarregaViasPtdService);
            var method = serviceType.GetMethod("Execute");

            // Assert
            Assert.IsNotNull(method);
        }
    }
}
