using H1SF.Application.DTOs.Interface;
using H1SF.Application.Services.InterfaceSDeItem880;
using H1SF.Application.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace H1SF.TestProject.Tests
{
    [TestClass]
    public class InterfaceDeaItem880Tests
    {
        [TestMethod]
        public void InterfaceSDeItem880Input_CanBeCreated()
        {
            // Arrange & Act
            var input = new InterfaceSDeItem880Input
            {
                DataSelecaoFaturamento = DateTime.Now
            };

            // Assert
            Assert.IsNotNull(input);
            Assert.IsNotNull(input.DataSelecaoFaturamento);
        }

        [TestMethod]
        public void InterfaceSDeItem880Output_CanBeCreated()
        {
            // Arrange & Act
            var output = new InterfaceSDeItem880Output();

            // Assert
            Assert.IsNotNull(output);
        }

        [TestMethod]
        public void InterfaceSDeItem880_InterfaceExists()
        {
            // Arrange & Act
            Type interfaceType = typeof(IInterfaceSDeItem880);

            // Assert
            Assert.IsNotNull(interfaceType);
            Assert.IsTrue(interfaceType.IsInterface);
        }

        [TestMethod]
        public void InterfaceSDeItem880Service_ImplementsInterface()
        {
            // Arrange & Act
            Type serviceType = typeof(InterfaceSDeItem880Service);
            Type interfaceType = typeof(IInterfaceSDeItem880);

            // Assert
            Assert.IsTrue(interfaceType.IsAssignableFrom(serviceType));
        }
    }
}
