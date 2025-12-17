using H1SF.Application.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace H1SF.TestProject.Tests
{
    [TestClass]
    public class InterfaceCmeDanfeTests
    {
        [TestMethod]
        public void InterfaceCmeDanfe860_InterfaceExists()
        {
            // Arrange & Act
            Type interfaceType = typeof(IInterfaceCmeDanfe860Service);

            // Assert
            Assert.IsNotNull(interfaceType);
            Assert.IsTrue(interfaceType.IsInterface);
        }

        [TestMethod]
        public void InterfaceCmeDanfe860Service_ImplementsInterface()
        {
            // Arrange & Act
            Type serviceType = typeof(InterfaceCmeDanfe860Service);
            Type interfaceType = typeof(IInterfaceCmeDanfe860Service);

            // Assert
            Assert.IsTrue(interfaceType.IsAssignableFrom(serviceType));
        }

        [TestMethod]
        public void InterfaceCmeDanfe860Service_HasPublicProperties()
        {
            // Arrange & Act
            Type serviceType = typeof(InterfaceCmeDanfe860Service);
            var properties = serviceType.GetProperties();

            // Assert
            Assert.IsTrue(properties.Length > 0);
            Assert.IsNotNull(serviceType.GetProperty("SF0002ItdDtcSelFtrm"));
        }
    }
}
