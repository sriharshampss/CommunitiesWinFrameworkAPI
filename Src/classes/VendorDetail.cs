using System.Runtime.Serialization;

namespace CommunitiesWinApi.Models
{
    [DataContract]
    public partial class VendorDetail
    {
        public long VendorId { get; set; }
        [DataMember]
        public string VendorName { get; set; }
        [DataMember]
        public string Country { get; set; }
        [DataMember]
        public string State { get; set; }
        [DataMember]
        public string City { get; set; }
        [DataMember]
        public long Phone { get; set; }
        [DataMember]
        public string Pin { get; set; }
        [DataMember]
        public decimal? Latitude { get; set; }
        [DataMember]
        public decimal? Longitude { get; set; }       
    }
}
