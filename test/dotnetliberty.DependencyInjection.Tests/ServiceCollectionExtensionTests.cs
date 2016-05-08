using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;

namespace dotnetliberty.DependencyInjection.Tests
{
    public class ServiceCollectionExtensionTests
    {
        public class TestClass
        {
        }

        [Fact]
        public void SingletonIntegration()
        {
            // Arrange
            var collection = new ServiceCollection();
            var factory = new Mock<IServiceFactory<TestClass>>();
            var firstGiven = new TestClass();
            var secondGiven = new TestClass();
            factory.SetupSequence(x => x.Build())
                .Returns(firstGiven)
                .Returns(secondGiven);
            
            // Act
            collection.AddSingletonFactory<TestClass, IServiceFactory<TestClass>>(factory.Object);
            var serviceProvider = collection.BuildServiceProvider();
            var firstResolved = serviceProvider.GetRequiredService<TestClass>();
            var secondResolved = serviceProvider.GetRequiredService<TestClass>();

            // Assert
            Assert.Equal(firstGiven, firstResolved);
            Assert.Equal(firstGiven, secondResolved);
            factory.Verify(x => x.Build(), Times.Once);
        }

        [Fact]
        public void TransientIntegration()
        {
            // Arrange
            var collection = new ServiceCollection();
            var factory = new Mock<IServiceFactory<TestClass>>();
            var firstGiven = new TestClass();
            var secondGiven = new TestClass();
            factory.SetupSequence(x => x.Build())
                .Returns(firstGiven)
                .Returns(secondGiven);

            // Act
            collection.AddTransientFactory<TestClass, IServiceFactory<TestClass>>(factory.Object);
            var serviceProvider = collection.BuildServiceProvider();
            var firstResolved = serviceProvider.GetRequiredService<TestClass>();
            var secondResolved = serviceProvider.GetRequiredService<TestClass>();

            // Assert
            Assert.Equal(firstGiven, firstResolved);
            Assert.Equal(secondGiven, secondResolved);
            factory.Verify(x => x.Build(), Times.Exactly(2));
        }

        [Fact]
        public void ScopedUsingInstance()
        {
            // Arrange
            ServiceDescriptor descriptor = null;
            var collection = new Mock<IServiceCollection>();
            collection.Setup(x => x.Add(It.IsAny<ServiceDescriptor>()))
                .Callback<ServiceDescriptor>(x => descriptor = x);

            var provider = new Mock<IServiceProvider>();
            
            var factory = new Mock<IServiceFactory<TestClass>>();
            var given = new TestClass();
            factory.Setup(x => x.Build()).Returns(given);

            // Act
            collection.Object.AddScopedFactory<TestClass, IServiceFactory<TestClass>>(factory.Object);

            // Assert
            Assert.Equal(typeof (TestClass), descriptor.ServiceType);
            Assert.Equal(ServiceLifetime.Scoped, descriptor.Lifetime);
            var actual = descriptor.ImplementationFactory(provider.Object);
            Assert.Equal(given, actual);
        }

        [Fact]
        public void ScopedUsingType()
        {
            // Arrange
            ServiceDescriptor descriptor = null;
            var collection = new Mock<IServiceCollection>();
            collection.Setup(x => x.Add(It.IsAny<ServiceDescriptor>()))
                .Callback<ServiceDescriptor>(x => descriptor = x);
            
            var factory = new Mock<IServiceFactory<TestClass>>();
            var given = new TestClass();
            factory.Setup(x => x.Build()).Returns(given);

            var factoryFuncCollection = new ServiceCollection();
            factoryFuncCollection.AddInstance(factory.Object);

            // Act
            collection.Object.AddScopedFactory<TestClass, IServiceFactory<TestClass>>();

            // Assert
            Assert.Equal(typeof(TestClass), descriptor.ServiceType);
            Assert.Equal(ServiceLifetime.Scoped, descriptor.Lifetime);
            var actual = descriptor.ImplementationFactory(factoryFuncCollection.BuildServiceProvider());
            Assert.Equal(given, actual);
        }
    }
}
