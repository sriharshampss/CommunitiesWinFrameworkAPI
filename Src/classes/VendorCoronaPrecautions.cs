using System.Runtime.Serialization;

namespace CommunitiesWinFrameworkAPI.classes
{
    [DataContract]
    public class VendorCoronaPrecaution
    {
        [DataMember]
        public long Phone { get; set; }
        [DataMember]
        public bool? IsSocialDistance { get; set; }
        [DataMember]
        public bool? IsFeverScreen { get; set; }
        [DataMember]
        public bool? IsSanitizerUsed { get; set; }
        [DataMember]
        public bool? IsStampCheck { get; set; }
        [DataMember]
        public bool? IsContactLessPay { get; set; }
    }
}