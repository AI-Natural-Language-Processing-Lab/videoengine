namespace Jugnoon.Entity
{
    public class MemberEntity : ContentEntity
    {
        public string id { get; set; }
        public byte emailconfirmed { get; set; } = 1; // 0: not confirmed, 1: confirmed, 2: ignore filter
        public byte lockoutenabled { get; set; } = 2; 
        public long topicid { get; set; } = 0;
        public string countryname { get; set; } = "";
        public int accounttype { get; set; } = 0;
        public string gender { get; set; } = "";
        public int type { get; set; } = 3;
        public bool advancelist { get; set; } = false;
        public string character { get; set; } = "";
        public bool havephoto { get; set; } = false;
        public bool isadmin { get; set; } = false;
    }
}
 