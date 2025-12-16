using H1SF.Application.Services.InstrucaoEspecial;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace H1SF.TestProject.Tests
{
    [TestClass]
    public class InstrucaoEspecialTests
    {
        [TestMethod]
        public void InstrucaoEspecial960Service_ServiceCreation_Succeeds()
        {
            // Arrange & Act
            var service = new InstrucaoEspecial960Service();

            // Assert
            Assert.IsNotNull(service);
        }

        [TestMethod]
        public void InstrucaoEspecial960Service_ImplementsInterface_Success()
        {
            // Arrange
            var service = new InstrucaoEspecial960Service();

            // Act & Assert
            Assert.IsNotNull(service);
            Assert.IsInstanceOfType(service, typeof(IInstrucaoEspecial960Service));
        }
    }
}
