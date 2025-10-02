using MillionApi.Domain.Entities;
using NUnit.Framework;

namespace MillionApi.Tests.Domain
{
    public class PropertyTests
    {
        [Test]
        public void IsValidPrice_ShouldReturnTrue_WhenPriceIsPositive()
        {
            var property = new Property { PriceProperty = 1000m };
            Assert.That(property.IsValidPrice(), Is.True);
        }

        [Test]
        public void IsValidPrice_ShouldReturnFalse_WhenPriceIsZeroOrNegative()
        {
            var property1 = new Property { PriceProperty = 0 };
            var property2 = new Property { PriceProperty = -500 };

            Assert.That(property1.IsValidPrice(), Is.False);
            Assert.That(property2.IsValidPrice(), Is.False);
        }
    }
}
