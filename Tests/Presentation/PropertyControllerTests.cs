using Microsoft.AspNetCore.Mvc;
using MillionApi.Application.Services;
using MillionApi.Domain.Entities;
using MillionApi.Domain.Interfaces;
using MillionApi.Presentation.Controllers;
using Moq;
using NUnit.Framework;

namespace MillionApi.Tests.Presentation
{
    public class PropertyControllerTests
    {
        private Mock<IPropertyService>? _mockService;
        private PropertyController? _controller;

        [SetUp]
        public void SetUp()
        {
            _mockService = new Mock<IPropertyService>();
            _controller = new PropertyController(_mockService.Object);
        }

        [Test]
        public void GetProperty_ShouldReturnOk_WhenPropertyExists()
        {
            var mockService = new Mock<IPropertyService>();
            mockService.Setup(s => s.GetPropertyById("123"))
                       .ReturnsAsync(new Property { Id = "123", Name = "Casa Bonita", PriceProperty = 3000m });
            var controller = new PropertyController(mockService.Object);
            // Arrange


            // Act
            var result = controller.GetProperty("123");

            // El resultado es Task<IActionResult>, necesitamos obtener el resultado
            Assert.That(result, Is.InstanceOf<Task<IActionResult>>());

            var actionResult = result.Result; // ← Obtener el resultado del Task
            Assert.That(actionResult, Is.InstanceOf<OkObjectResult>());
            var okResult = actionResult as OkObjectResult;
            Assert.That(okResult!.StatusCode, Is.EqualTo(200));
        }

        [Test]
        public async Task GetProperty_ShouldReturnNotFound_WhenPropertyDoesNotExist()
        {
            var mockService = new Mock<IPropertyService>();
            // Usar Task.FromResult explícitamente
            Property nullProperty = null!;
            mockService.Setup(s => s.GetPropertyById("999"))
                       .ReturnsAsync(nullProperty);

            var controller = new PropertyController(mockService.Object);

            var result = await controller.GetProperty("999");

            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }
    }
}
