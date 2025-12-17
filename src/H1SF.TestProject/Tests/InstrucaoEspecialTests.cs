using H1SF.Application.Services.InstrucaoEspecial;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace H1SF.TestProject.Tests
{
    [TestClass]
    public class InstrucaoEspecialTests
    {
        [TestMethod]
        public void InstrucaoEspecial960_InterfaceExists()
        {
            // Arrange & Act
            Type interfaceType = typeof(IInstrucaoEspecial960Service);

            // Assert
            Assert.IsNotNull(interfaceType);
            Assert.IsTrue(interfaceType.IsInterface);
        }

        [TestMethod]
        public void InstrucaoEspecial960Service_ImplementsInterface()
        {
            // Arrange & Act
            Type serviceType = typeof(InstrucaoEspecial960Service);
            Type interfaceType = typeof(IInstrucaoEspecial960Service);

            // Assert
            Assert.IsTrue(interfaceType.IsAssignableFrom(serviceType));
        }

        [TestMethod]
        public void InstrucaoEspecial960Service_HasPublicProperties()
        {
            // Arrange & Act
            Type serviceType = typeof(InstrucaoEspecial960Service);
            var properties = serviceType.GetProperties();

            // Assert
            Assert.IsTrue(properties.Length > 0);
        }
    }
}
