using System.ComponentModel.DataAnnotations;
namespace Jugnoon.Framework
{
    public partial class JGN_User_Settings
    {
        [Key]
        public long id { get; set; }
        [MaxLength(100)]
        public string userid { get; set; }
        public byte issendmessages { get; set; }
        public byte isemail { get; set; }
    }
}
