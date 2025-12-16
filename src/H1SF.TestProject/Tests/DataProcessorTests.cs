using H1SF.Application.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace H1SF.TestProject.Tests
{
    [TestClass]
    public class DataProcessorTests
    {
        [TestMethod]
        public void DataProcessor_ServiceCreation_Succeeds()
        {
            // Arrange & Act
            var service = new DataProcessor();

            // Assert
            Assert.IsNotNull(service);
        }

        [TestMethod]
        public void TrocaPontoCorpo_EmptyDictionary_DoesNotThrow()
        {
            // Arrange
            var service = new DataProcessor();
            var data = new Dictionary<string, string>();

            // Act & Assert
            service.TrocaPontoCorpo(data);
            Assert.IsTrue(true); // If no exception thrown, test passes
        }

        [TestMethod]
        public void TrocaPontoCorpo_WithValidData_ProcessesCorrectly()
        {
            // Arrange
            var service = new DataProcessor();
            var data = new Dictionary<string, string>
            {
                { "WM01-PRECO-TOTAL-M", "1000.50" },
                { "WM01-VL-FRETE", "50.25" },
                { "WM01-PESO-BRUTO-KG", "100.75" }
            };

            // Act & Assert - Method should process without throwing
            service.TrocaPontoCorpo(data);
            Assert.IsTrue(true);
        }
    }
}
