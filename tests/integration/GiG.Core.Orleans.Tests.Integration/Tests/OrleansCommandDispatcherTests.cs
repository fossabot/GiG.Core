using System;
using System.Threading;
using System.Threading.Tasks;
using GiG.Core.Orleans.Streams.Abstractions;
using GiG.Core.Orleans.Tests.Integration.Fixtures;
using GiG.Core.Orleans.Tests.Integration.Lifetimes;
using GiG.Core.Orleans.Tests.Integration.Mocks;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace GiG.Core.Orleans.Tests.Integration.Tests
{
    [Trait("Category", "Integration")]
    public class OrleansCommandDispatcherTests : IClassFixture<BasicClusterFixture>
    {
        private readonly BasicClusterFixture _fixture;

        public OrleansCommandDispatcherTests(BasicClusterFixture fixture)
        {
            _fixture = fixture;
        }
        
        [Fact]
        public async Task DispatchAsync_NoEvents_ThrowsInvalidOperationException()
        {
            // Arrange
            var grainId = Guid.NewGuid();

            // Act
            await using var commandDispatcher = _fixture.ClientServiceProvider.GetRequiredService<ICommandDispatcherFactory<MockCommand, MockSuccessEvent, MockFailureEvent>>()
                .Create(grainId, Constants.StreamProviderName)
                .WithCommand(new MockCommand(true), MockCommand.MockCommandNamespace);

            await commandDispatcher.SubscribeAsync();
            
            var exception = Assert.ThrowsAsync<InvalidOperationException>(() => commandDispatcher.DispatchAsync(5000));

            // Assert
            Assert.Equal("Not Subscribed to Success or Failure Event", exception.Result.Message);
        }
        
        [Fact]
        public async Task DispatchAsync_SetupSuccessEventOnly_ThrowsInvalidOperationException()
        {
            // Arrange
            var grainId = Guid.NewGuid();

            // Act
            await using var commandDispatcher = _fixture.ClientServiceProvider
                .GetRequiredService<ICommandDispatcherFactory<MockCommand, MockSuccessEvent, MockFailureEvent>>()
                .Create(grainId, Constants.StreamProviderName)
                .WithCommand(new MockCommand(true), MockCommand.MockCommandNamespace)
                .WithSuccessEvent(MockSuccessEvent.MockSuccessEventNamespace);
          
            await commandDispatcher.SubscribeAsync();
            
            var exception = Assert.ThrowsAsync<InvalidOperationException>(() => commandDispatcher.DispatchAsync(5000));

            // Assert
            Assert.Equal("Not Subscribed to Success or Failure Event", exception.Result.Message);
        }
        
        [Fact]
        public async Task DispatchAsync_SetupFailureEventOnly_ThrowsInvalidOperationException()
        {
            // Arrange
            var grainId = Guid.NewGuid();

            // Act
            await using var commandDispatcher = _fixture.ClientServiceProvider.GetRequiredService<ICommandDispatcherFactory<MockCommand, MockSuccessEvent, MockFailureEvent>>()
                .Create(grainId, Constants.StreamProviderName)
                .WithCommand(new MockCommand(false), MockCommand.MockCommandNamespace)
                .WithFailureEvent(MockFailureEvent.MockFailureEventNamespace);

            await commandDispatcher.SubscribeAsync();
            
            var exception = Assert.ThrowsAsync<InvalidOperationException>(() => commandDispatcher.DispatchAsync(5000));

            // Assert
            Assert.Equal("Not Subscribed to Success or Failure Event", exception.Result.Message);
        }
        
        [Fact]
        public async Task DispatchAsync_WithSuccessAndFailureEvents_ReturnsSuccessEvent()
        {
            // Arrange
            var grainId = Guid.NewGuid();

            // Act
            await using var commandDispatcher = _fixture.ClientServiceProvider.GetRequiredService<ICommandDispatcherFactory<MockCommand, MockSuccessEvent, MockFailureEvent>>()
                .Create(grainId, Constants.StreamProviderName)
                .WithCommand(new MockCommand(true), MockCommand.MockCommandNamespace)
                .WithSuccessEvent(MockSuccessEvent.MockSuccessEventNamespace)
                .WithFailureEvent(MockFailureEvent.MockFailureEventNamespace);

            await commandDispatcher.SubscribeAsync();
            
            var response = await commandDispatcher.DispatchAsync(5000);

            // Assert
            Assert.NotNull(response);
            Assert.NotNull(response.Data);
            Assert.True(response.IsSuccess);
            Assert.False(response.IsError);
            Assert.Equal(response.Data.GrainId, grainId);
        }
        
        [Fact]
        public async Task DispatchAsync_WithSuccessAndFailureEvents_ReturnsFailureEvent()
        {
            // Arrange
            var grainId = Guid.NewGuid();

            // Act
            await using var commandDispatcher =  _fixture.ClientServiceProvider.GetRequiredService<ICommandDispatcherFactory<MockCommand, MockSuccessEvent, MockFailureEvent>>()
                .Create(grainId, Constants.StreamProviderName)
                .WithCommand(new MockCommand(false), MockCommand.MockCommandNamespace)
                .WithSuccessEvent(MockSuccessEvent.MockSuccessEventNamespace)
                .WithFailureEvent(MockFailureEvent.MockFailureEventNamespace);
            
            await commandDispatcher.SubscribeAsync();

            var response = await commandDispatcher.DispatchAsync(5000);
            
            // Assert
            Assert.NotNull(response);
            Assert.False(response.IsSuccess);
            Assert.True(response.IsError);
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
            await using var commandDispatcher = _fixture.ClientServiceProvider.GetRequiredService<ICommandDispatcherFactory<MockCommand, MockSuccessEvent, MockFailureEvent>>()
                .Create(grainId, Constants.StreamProviderName)
                .WithCommand(new MockCommand(true), MockCommand.MockCommandNamespace)
                .WithSuccessEvent(MockSuccessEvent.MockSuccessEventNamespace)
                .WithFailureEvent(MockFailureEvent.MockFailureEventNamespace);
            
            await commandDispatcher.SubscribeAsync();

            cts.Cancel();

            // Assert
            await Assert.ThrowsAsync<TaskCanceledException>(() => commandDispatcher.DispatchAsync(5000, cts.Token));
        }
        
        [Fact]
        public async Task DispatchAsync_WithSuccessAndFailureEvents_ThrowsInvalidOperationException()
        {
            // Arrange
            var grainId = Guid.NewGuid();

            // Act
            await using var commandDispatcher = _fixture.ClientServiceProvider.GetRequiredService<ICommandDispatcherFactory<MockCommand, MockSuccessEvent, MockFailureEvent>>()
                .Create(grainId, Constants.StreamProviderName)
                .WithCommand(new MockCommand(true), MockCommand.MockCommandNamespace)
                .WithSuccessEvent(MockSuccessEvent.MockSuccessEventNamespace)
                .WithFailureEvent(MockFailureEvent.MockFailureEventNamespace);
            
            var exception = Assert.ThrowsAsync<InvalidOperationException>(() => commandDispatcher.DispatchAsync(5000));

            // Assert
            Assert.Equal("Not Subscribed to Success or Failure Event", exception.Result.Message);
        }
    }
}