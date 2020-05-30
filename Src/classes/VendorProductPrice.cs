using System.Runtime.Serialization;

namespace CommunitiesWinFrameworkAPI.classes
{
    [DataContract]
    public class VendorProductPrice
    {
        [DataMember]
        public long Phone { get; set; }
        [DataMember]
        public string ProductName { get; set; }
        [DataMember]
        public string CategoryName { get; set; }
        [DataMember]
        public decimal? Price { get; set; }
        [DataMember]
        public decimal? MinOrderQuantity { get; set; }
        [DataMember]
        public string Units { get; set; }
    }
}