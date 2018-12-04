using Benefits.Provider.Forms;

namespace Benefits.Provider
{
    internal class DetailProductMedical : IDetailProduct
    {
        public bool? HasTransport { get; set; }
        public bool? HasEmergency { get; set; }
        public decimal? DailyCover { get; set; }
    }
}