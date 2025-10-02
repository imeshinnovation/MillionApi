using NUnit.Framework;
using Moq;
using MillionApi.Application.Services;
using MillionApi.Domain.Entities;
using MillionApi.Domain.Interfaces;


namespace MillionApi.Tests.Application
{
    public class PropertyServiceTests
    {
        private Mock<IPropertyRepository>? _mockRepo;
        private PropertyService? _service;

        [SetUp]
        public void Setup()
        {
            _mockRepo = new Mock<IPropertyRepository>();
            _service = new PropertyService(_mockRepo.Object);
        }

        [Test]
        public async Task GetPropertyById_ShouldReturnProperty_WhenExists()
        {
            var property = new Property { Id = "123", Name = "Test House", PriceProperty = 1500m };
            _mockRepo!.Setup(r => r.GetById("123")).ReturnsAsync(property);

            var result = await _service!.GetPropertyById("123");

            Assert.That(result, Is.Not.Null);
            Assert.That(result!.Name, Is.EqualTo("Test House"));
        }
    }
}
