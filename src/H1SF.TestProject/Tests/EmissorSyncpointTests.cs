using H1SF.Application.Services.Transacao;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;

namespace H1SF.TestProject.Tests
{
    [TestClass]
    public class EmissorSyncpointTests
    {
        [TestMethod]
        public void EmissorSyncpoint_InterfaceExists()
        {
            // Arrange & Act
            // Verify the IEmissorSyncpoint interface exists and is properly defined
            Type interfaceType = typeof(IEmissorSyncpoint);

            // Assert
            Assert.IsNotNull(interfaceType);
            Assert.IsTrue(interfaceType.IsInterface);
        }

        [TestMethod]
        public void EmissorSyncpoint_HasEmitirSyncpointAsyncMethod()
        {
            // Arrange & Act
            var method = typeof(IEmissorSyncpoint).GetMethod("EmitirSyncpointAsync");

            // Assert
            Assert.IsNotNull(method);
            Assert.AreEqual(typeof(Task), method.ReturnType);
        }

        [TestMethod]
        public void EmissorSyncpoint_ServiceImplementsInterface()
        {
            // Arrange & Act
            Type serviceType = typeof(EmissorSyncpoint);
            Type interfaceType = typeof(IEmissorSyncpoint);

            // Assert
            Assert.IsTrue(interfaceType.IsAssignableFrom(serviceType));
        }
    }
}
