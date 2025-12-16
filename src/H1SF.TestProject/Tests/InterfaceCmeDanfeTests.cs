using H1SF.Application.Services.InterfaceCmeDanfe;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace H1SF.TestProject.Tests
{
    [TestClass]
    public class InterfaceCmeDanfeTests
    {
        [TestMethod]
        public void InterfaceCmeDanfe860Service_ServiceCreation_Succeeds()
        {
            // Arrange & Act
            var service = new InterfaceCmeDanfe860Service();

            // Assert
            Assert.IsNotNull(service);
        }

        [TestMethod]
        public void InterfaceCmeDanfe860Service_ImplementsInterface_Success()
        {
            // Arrange
            var service = new InterfaceCmeDanfe860Service();

            // Act & Assert
            Assert.IsNotNull(service);
            Assert.IsInstanceOfType(service, typeof(IInterfaceCmeDanfe860Service));
        }
    }
}
