using H1SF.Application.DTOs.DefinidorPjl;
using H1SF.Application.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;

namespace H1SF.TestProject.Tests
{
    [TestClass]
    public class DefinidorPjlOutbinP145Tests
    {
        [TestMethod]
        public void DefinidorPjlOutbinP145_InterfaceExists()
        {
            // Arrange & Act
            Type interfaceType = typeof(IDefinidorPjlOutbinP145);

            // Assert
            Assert.IsNotNull(interfaceType);
            Assert.IsTrue(interfaceType.IsInterface);
        }

        [TestMethod]
        public void DefinidorPjlOutbinP145Service_ImplementsInterface()
        {
            // Arrange & Act
            Type serviceType = typeof(DefinidorPjlOutbinP145Service);
            Type interfaceType = typeof(IDefinidorPjlOutbinP145);

            // Assert
            Assert.IsTrue(interfaceType.IsAssignableFrom(serviceType));
        }

        [TestMethod]
        public void DefinidorPjlOutbinP145Input_CanBeCreated()
        {
            // Arrange & Act
            var input = new DefinidorPjlOutbinP145Input
            {
                QuantidadeViasPackList = 3,
                CodigoMercadoriaDestino = "E",
                CodigoModalidadeTransporte = "6"
            };

            // Assert
            Assert.IsNotNull(input);
            Assert.AreEqual(3, input.QuantidadeViasPackList);
            Assert.AreEqual("E", input.CodigoMercadoriaDestino);
        }

        [TestMethod]
        public void DefinidorPjlOutbinP145Output_CanBeCreated()
        {
            // Arrange & Act
            var output = new DefinidorPjlOutbinP145Output();

            // Assert
            Assert.IsNotNull(output);
        }
    }
}
