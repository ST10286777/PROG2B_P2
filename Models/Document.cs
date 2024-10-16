using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PROG_P1.Models
{
    public class Document
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }

        // Foreign key to Claims
        public int ClaimsID { get; set; }
        [ForeignKey("ClaimsID")]
        public Claims Claims { get; set; }
    }

}
