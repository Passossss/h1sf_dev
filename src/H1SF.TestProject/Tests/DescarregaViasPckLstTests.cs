using H1SF.Application.DTOs.DescarregaVias;
using H1SF.Application.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;

namespace H1SF.TestProject.Tests
{
    [TestClass]
    public class DescarregaViasPckLstTests
    {
        [TestMethod]
        public void DescarregaViasPckLst_InterfaceExists()
        {
            // Arrange & Act
            Type interfaceType = typeof(IDescarregaViasPckLst);

            // Assert
            Assert.IsNotNull(interfaceType);
            Assert.IsTrue(interfaceType.IsInterface);
        }

        [TestMethod]
        public void DescarregaViasPckLstService_ImplementsInterface()
        {
            // Arrange & Act
            Type serviceType = typeof(DescarregaViasPckLstService);
            Type interfaceType = typeof(IDescarregaViasPckLst);

            // Assert
            Assert.IsTrue(interfaceType.IsAssignableFrom(serviceType));
        }

        [TestMethod]
        public void DescarregaViasPckLstInput_CanBeCreated()
        {
            // Arrange & Act
            var input = new DescarregaViasPckLstInput
            {
                NumeroViasVolume = 2,
                IndiceImpressao = 3
            };

            // Assert
            Assert.IsNotNull(input);
            Assert.AreEqual(2, input.NumeroViasVolume);
            Assert.AreEqual(3, input.IndiceImpressao);
        }

        [TestMethod]
        public void DescarregaViasPckLstOutput_CanBeCreated()
        {
            // Arrange & Act
            var output = new DescarregaViasPckLstOutput();

            // Assert
            Assert.IsNotNull(output);
        }
    }
}
