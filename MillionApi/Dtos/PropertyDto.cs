namespace MillionApi.Dtos
{
    public class PropertyDto
    {
        public string IdOwner { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string AddressProperty { get; set; } = null!;
        public decimal PriceProperty { get; set; }
        public string ImageUrl { get; set; } = null!;
    }
}

