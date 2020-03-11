using System;
using System.Threading;
using System.Threading.Tasks;
using GiG.Core.Orleans.Streams.Abstractions;
using GiG.Core.Orleans.Tests.Integration.Lifetimes;
using GiG.Core.Orleans.Tests.Integration.Mocks;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace GiG.Core.Orleans.Tests.Integration.Tests
{
    [Trait("Category", "Integration")]
    public class OrleansCommandDispatcherTests : BasicClusterLifeTime
    {
        [Fact]
        public async Task DispatchAsync_NoEvents_ReturnsTimeout()
        {
            // Arrange
            var grainId = Guid.NewGuid();

            // Act
            using var commandDispatcher = ClientServiceProvider.GetRequiredService<ICommandDispatcherFactory<MockCommand, MockSuccessEvent, MockFailureEvent>>()
                .Create(grainId, Constants.StreamProviderName)
                .WithCommand(new MockCommand(true), MockCommand.MockCommandNamespace);

            var response = await commandDispatcher.DispatchAsync(5000);
            
            // Assert
            Assert.NotNull(response);
            Assert.Equal("timeout", response.ErrorCode);
        }
        
        [Fact]
        public async Task DispatchAsync_SetupSuccessEventOnly_ReturnsSuccessEvent()
        {
            // Arrange
            var grainId = Guid.NewGuid();

            // Act
            using var commandDispatcher = ClientServiceProvider.GetRequiredService<ICommandDispatcherFactory<MockCommand, MockSuccessEvent, MockFailureEvent>>()
                .Create(grainId, Constants.StreamProviderName)
                .WithCommand(new MockCommand(true), MockCommand.MockCommandNamespace)
                .WithSuccessEvent(MockSuccessEvent.MockSuccessEventNamespace);

            var response = await commandDispatcher.DispatchAsync(5000);
            
            // Assert
            Assert.NotNull(response);
            Assert.NotNull(response.Data);
            Assert.Equal(response.Data.GrainId, grainId);
        }
        
        [Fact]
        public async Task DispatchAsync_SetupFailureEventOnly_ReturnsFailureEvent()
        {
            // Arrange
            var grainId = Guid.NewGuid();

            // Act
            using var commandDispatcher = ClientServiceProvider.GetRequiredService<ICommandDispatcherFactory<MockCommand, MockSuccessEvent, MockFailureEvent>>()
                .Create(grainId, Constants.StreamProviderName)
                .WithCommand(new MockCommand(false), MockCommand.MockCommandNamespace)
                .WithFailureEvent(MockFailureEvent.MockFailureEventNamespace);

            var response = await commandDispatcher.DispatchAsync(5000);
            
            // Assert
            Assert.NotNull(response);
            Assert.Equal(MockFailureEvent.MockErrorCode, response.ErrorCode);
            Assert.Equal(MockFailureEvent.MockErrorMessage, response.ErrorMessage);
        }
        
        [Fact]
        public async Task DispatchAsync_WithSuccessAndFailureEvents_ReturnsSuccessEvent()
        {
            // Arrange
            var grainId = Guid.NewGuid();

            // Act
            using var commandDispatcher = ClientServiceProvider.GetRequiredService<ICommandDispatcherFactory<MockCommand, MockSuccessEvent, MockFailureEvent>>()
                .Create(grainId, Constants.StreamProviderName)
                .WithCommand(new MockCommand(true), MockCommand.MockCommandNamespace)
                .WithSuccessEvent(MockSuccessEvent.MockSuccessEventNamespace)
                .WithFailureEvent(MockFailureEvent.MockFailureEventNamespace);
            var response = await commandDispatcher.DispatchAsync(5000);

            // Assert
            Assert.NotNull(response);
            Assert.NotNull(response.Data);
            Assert.Equal(response.Data.GrainId, grainId);
        }
        
        [Fact]
        public async Task DispatchAsync_WithSuccessAndFailureEvents_ReturnsFailureEvent()
        {
            // Arrange
            var grainId = Guid.NewGuid();

            // Act
            using var commandDispatcher =  ClientServiceProvider.GetRequiredService<ICommandDispatcherFactory<MockCommand, MockSuccessEvent, MockFailureEvent>>()
                .Create(grainId, Constants.StreamProviderName)
                .WithCommand(new MockCommand(false), MockCommand.MockCommandNamespace)
                .WithSuccessEvent(MockSuccessEvent.MockSuccessEventNamespace)
                .WithFailureEvent(MockFailureEvent.MockFailureEventNamespace);

            var response = await commandDispatcher.DispatchAsync(5000);
            
            // Assert
            Assert.NotNull(response);
            Assert.Equal(MockFailureEvent.MockErrorCode, response.ErrorCode);
            Assert.Equal(MockFailureEvent.MockErrorMessage, response.ErrorMessage);
        }
        
        [Fact]
        public async Task DispatchAsync_WithSuccessAndFailureEvents_ThrowsTaskCanceledException()
        {
            // Arrange
            var grainId = Guid.NewGuid();
            var cts = new CancellationTokenSource();
    
            // Act
            using var commandDispatcher = ClientServiceProvider.GetRequiredService<ICommandDispatcherFactory<MockCommand, MockSuccessEvent, MockFailureEvent>>()
                .Create(grainId, Constants.StreamProviderName)
                .WithCommand(new MockCommand(true), MockCommand.MockCommandNamespace)
                .WithSuccessEvent(MockSuccessEvent.MockSuccessEventNamespace)
                .WithFailureEvent(MockFailureEvent.MockFailureEventNamespace);

            cts.Cancel();

            // Assert
            await Assert.ThrowsAsync<TaskCanceledException>(() => commandDispatcher.DispatchAsync(5000, cts.Token));
        }
    }
}