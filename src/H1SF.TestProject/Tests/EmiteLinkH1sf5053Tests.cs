using H1SF.Application.DTOs.EmiteLinkH1sf5053;
using H1SF.Application.Services.EmiteLinkH1sf5053;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace H1SF.TestProject.Tests
{
    [TestClass]
    public class EmiteLinkH1sf5053Tests
    {
        [TestMethod]
        public void EmiteLinkH1sf5053Input_CanBeCreated()
        {
            // Arrange & Act
            var input = new EmiteLinkH1sf5053Input();

            // Assert
            Assert.IsNotNull(input);
        }

        [TestMethod]
        public void EmiteLinkH1sf5053Output_CanBeCreated()
        {
            // Arrange & Act
            var output = new EmiteLinkH1sf5053Output();

            // Assert
            Assert.IsNotNull(output);
        }

        [TestMethod]
        public void EmiteLinkH1sf5053_InterfaceExists()
        {
            // Arrange & Act
            Type interfaceType = typeof(IEmiteLinkH1sf5053);

            // Assert
            Assert.IsNotNull(interfaceType);
            Assert.IsTrue(interfaceType.IsInterface);
        }

        [TestMethod]
        public void EmiteLinkH1sf5053Service_ImplementsInterface()
        {
            // Arrange & Act
            Type serviceType = typeof(EmiteLinkH1sf5053Service);
            Type interfaceType = typeof(IEmiteLinkH1sf5053);

            // Assert
            Assert.IsTrue(interfaceType.IsAssignableFrom(serviceType));
        }
    }
}
