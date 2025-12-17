using H1SF.Application.Services.EmiteLink;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace H1SF.TestProject.Tests
{
    [TestClass]
    public class EmiteLinkTests
    {
        [TestMethod]
        public void EmiteLinkH1SF_InterfaceExists()
        {
            // Arrange & Act
            Type interfaceType = typeof(IEmiteLinkH1SFService);

            // Assert
            Assert.IsNotNull(interfaceType);
            Assert.IsTrue(interfaceType.IsInterface);
        }

        [TestMethod]
        public void EmiteLinkH1SFService_ImplementsInterface()
        {
            // Arrange & Act
            Type serviceType = typeof(EmiteLinkH1SFService);
            Type interfaceType = typeof(IEmiteLinkH1SFService);

            // Assert
            Assert.IsTrue(interfaceType.IsAssignableFrom(serviceType));
        }

        [TestMethod]
        public void EmiteLinkH1SFService_HasExecutarMethod()
        {
            // Arrange & Act
            Type serviceType = typeof(EmiteLinkH1SFService);
            var method = serviceType.GetMethod("Executar");

            // Assert
            Assert.IsNotNull(method);
        }
    }
}
