
using System.ComponentModel.DataAnnotations;

namespace Jugnoon.Framework
{    
    public partial class JGN_MailTemplates
    {
        [Key]
        public short id { get; set; }
        [MaxLength(10)]
        public string templatekey { get; set; }
        public string description { get; set; }
        [MaxLength(300)]
        public string tags { get; set; }
        [MaxLength(100)]
        public string subjecttags { get; set; }
        [MaxLength(150)]
        public string subject { get; set; }
        public string contents { get; set; }
        [MaxLength(15)]
        public string type { get; set; }
    }
}
