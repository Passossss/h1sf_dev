using H1SF.Application.Services.GravaLançamentos;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace H1SF.TestProject.Tests
{
    [TestClass]
    public class GravaLancamentosTests
    {
        [TestMethod]
        public void GravaLancamentosCt_InterfaceExists()
        {
            // Arrange & Act
            Type interfaceType = typeof(IGravaLancamentosCtService);

            // Assert
            Assert.IsNotNull(interfaceType);
            Assert.IsTrue(interfaceType.IsInterface);
        }

        [TestMethod]
        public void GravaLancamentosCtService_ImplementsInterface()
        {
            // Arrange & Act
            Type serviceType = typeof(GravaLancamentosCtService);
            Type interfaceType = typeof(IGravaLancamentosCtService);

            // Assert
            Assert.IsTrue(interfaceType.IsAssignableFrom(serviceType));
        }

        [TestMethod]
        public void GravaLancamentosCtService_CanBeInstantiated()
        {
            // Arrange & Act
            Type serviceType = typeof(GravaLancamentosCtService);
            var constructor = serviceType.GetConstructor(Type.EmptyTypes);

            // Assert
            Assert.IsNotNull(constructor);
        }
    }
}
