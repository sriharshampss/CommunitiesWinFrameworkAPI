using System.Runtime.Serialization;

namespace CommunitiesWinFrameworkApi.classes
{
    [DataContract]
    public class VendorCategoryItem
    {
        [DataMember]
        public long Phone { get; set; }
        [DataMember]
        public string CategoryName { get; set; }
        [DataMember]
        public bool? Enable { get; set; }
    }
}
