using AutoMapper;
using GiG.Core.ObjectMapping.AutoMapper.Tests.Unit.Fixtures;
using GiG.Core.ObjectMapping.AutoMapper.Tests.Unit.Models;
using System;
using Xunit;
using IObjectMapper = GiG.Core.ObjectMapping.Abstractions.IObjectMapper;
using Person = GiG.Core.ObjectMapping.AutoMapper.Tests.Unit.Models.Person;

namespace GiG.Core.ObjectMapping.AutoMapper.Tests.Unit
{
    [Trait("Category", "Unit")]
    public abstract class ObjectMapperTests
    {
        private readonly IObjectMapper _objectMapper;

        protected ObjectMapperTests(AutoMapperFixtureBase fixture)
        {
            _objectMapper = fixture.ObjectMapper;
        }

        [Fact]
        public virtual void AssertConfigurationIsValid()
        {
            var configuration = new MapperConfiguration(
                cfg => cfg.AddProfile(new Profiles.MappingProfile()));

            configuration.AssertConfigurationIsValid();
        }

        [Fact]
        // Execute a mapping from the source object to a new destination object.
        public virtual void Map_SourceTypeInferred_ReturnsDestinationType()
        {
            // Arrange
            var person = new Person { FirstName = "Johnny", LastName = "Smith", DoB = new DateTime(1984, 6, 5) };

            // Act
            var response = _objectMapper.Map<PersonEntity>(person);

            // Assert
            Assert.Equal(person.FirstName, response.FirstName);
            Assert.Equal(person.LastName, response.LastName);
            Assert.Equal(person.DoB, response.DoB);
        }

        [Fact]
        public virtual void Map_SourceTypeKnown_ReturnsDestinationType()
        // Execute a mapping from the source object to a new destination object.
        {
            // Arrange
            var person = new Person { FirstName = "Johnny", LastName = "Smith", DoB = new DateTime(1984, 6, 5) };

            // Act
            var response = _objectMapper.Map<Person, PersonEntity>(person);

            // Assert
            Assert.Equal(person.FirstName, response.FirstName);
            Assert.Equal(person.LastName, response.LastName);
            Assert.Equal(person.DoB, response.DoB);
        }

        [Fact]
        // Execute a mapping from the source object to the existing destination object.
        public virtual void Map_SourceTypeKnownMapIntoDestination_ReturnsDestinationType()
        {
            // Arrange
            var person = new Person { FirstName = "Johnny", LastName = "Smith", DoB = new DateTime(1984, 6, 5) };
            var expectedResponse = new PersonEntity();

            // Act
            var response = _objectMapper.Map(person, expectedResponse);

            // Assert
            Assert.Equal(person.FirstName, response.FirstName);
            Assert.Equal(person.LastName, response.LastName);
            Assert.Equal(person.DoB, response.DoB);
            Assert.Equal(person.FirstName, expectedResponse.FirstName);
            Assert.Equal(person.LastName, expectedResponse.LastName);
            Assert.Equal(person.DoB, expectedResponse.DoB);
        }

        [Fact]
        // Execute a mapping from the source object to a new destination object with explicit System.Type objects
        public virtual void Map_SourceToNewDestinationWithExplicitTypes_ReturnsDestinationType()
        {
            // Arrange
            var person = new Person { FirstName = "Johnny", LastName = "Smith", DoB = new DateTime(1984, 6, 5) };

            // Act
            var response = _objectMapper.Map(person, typeof(Person), typeof(PersonEntity));
            
            // Assert
            Assert.True(response is PersonEntity);
            var actualResponse = response as PersonEntity;
            Assert.Equal(person.FirstName, actualResponse.FirstName);
            Assert.Equal(person.LastName, actualResponse.LastName);
            Assert.Equal(person.DoB, actualResponse.DoB);
        }

        [Fact]
        // Execute a mapping from the source object to existing destination object with explicit System.Type objects
        public virtual void Map_SourceToExistingDestinationWithExplicitTypes_ReturnsDestinationType()
        {
            // Arrange
            var person = new Person { FirstName = "Johnny", LastName = "Smith", DoB = new DateTime(1984, 6, 5) };
            var expectedResponse = new PersonEntity();

            // Act
            var response = _objectMapper.Map(person, expectedResponse, typeof(Person), typeof(PersonEntity));

            // Assert
            Assert.True(response is PersonEntity);
            var actualResponse = response as PersonEntity;
            Assert.Equal(person.FirstName, actualResponse.FirstName);
            Assert.Equal(person.LastName, actualResponse.LastName);
            Assert.Equal(person.DoB, actualResponse.DoB);
            Assert.Equal(person.FirstName, expectedResponse.FirstName);
            Assert.Equal(person.LastName, expectedResponse.LastName);
            Assert.Equal(person.DoB, expectedResponse.DoB);
        }
    }
}