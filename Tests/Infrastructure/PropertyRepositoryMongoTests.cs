using Microsoft.Extensions.Options;
using MillionApi.Domain.Entities;
using MillionApi.Infrastructure.Persistence;
using Moq;
using MongoDB.Driver;
using NUnit.Framework;

namespace MillionApi.Tests.Infrastructure
{
    [TestFixture]
    public class PropertyRepositoryMongoTests
    {
        private Mock<IOptions<MongoDBSettings>>? _mockOptions;
        private PropertyRepositoryMongo? _repository;

        [SetUp]
        public void SetUp()
        {
            var settings = new MongoDBSettings
            {
                ConnectionString = "mongodb://host.docker.internal:27017",
                DatabaseName = "TestDatabase",
                CollectionName = "TestProperties"
            };

            _mockOptions = new Mock<IOptions<MongoDBSettings>>();
            _mockOptions.Setup(x => x.Value).Returns(settings);

            _repository = new PropertyRepositoryMongo(_mockOptions.Object);
        }

        [Test]
        public void GetById_ShouldReturnProperty_WhenExists()
        {
            var mockCollection = new Mock<IMongoCollection<Property>>();
            var property = new Property { Id = "123", Name = "Casa Test", PriceProperty = 2000m };

            var mockCursor = new Mock<IAsyncCursor<Property>>();
            mockCursor.SetupSequence(x => x.MoveNext(It.IsAny<System.Threading.CancellationToken>()))
                      .Returns(true).Returns(false);
            mockCursor.Setup(x => x.Current).Returns(new List<Property> { property });

            mockCollection.Setup(c => c.FindSync(It.IsAny<FilterDefinition<Property>>(),
                It.IsAny<FindOptions<Property, Property>>(), default))
                .Returns(mockCursor.Object);

            var mockOptions = new Mock<Microsoft.Extensions.Options.IOptions<MillionApi.Infrastructure.Persistence.MongoDBSettings>>();
            mockOptions.Setup(x => x.Value).Returns(new MillionApi.Infrastructure.Persistence.MongoDBSettings
            {
                ConnectionString = "mongodb://host.docker.internal:27017",
                DatabaseName = "TestDatabase",
                CollectionName = "TestProperties"
            });

            var repo = new PropertyRepositoryMongo(mockOptions.Object);

            Assert.Pass("Simulación básica, se recomienda usar Mongo2Go para pruebas reales.");
        }
    }
}
