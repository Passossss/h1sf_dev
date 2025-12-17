using H1SF.Application.DTOs.ContabilizaItem;
using H1SF.Application.Services.ContabilizaItem;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;

namespace H1SF.TestProject.Tests
{
    [TestClass]
    public class ContabilizaItemTests
    {
        [TestMethod]
        public void ContabilizaItemInput_CanBeCreated()
        {
            // Arrange & Act
            var input = new ContabilizaItemInput
            {
                DataSelecaoFaturamento = DateTime.Now,
                NumeroNotaContabil = "12345"
            };

            // Assert
            Assert.IsNotNull(input);
            Assert.IsNotNull(input.DataSelecaoFaturamento);
            Assert.AreEqual("12345", input.NumeroNotaContabil);
        }

        [TestMethod]
        public void ContabilizaItemOutput_CanBeCreated()
        {
            // Arrange & Act
            var output = new ContabilizaItemOutput();

            // Assert
            Assert.IsNotNull(output);
        }
    }
}
