using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APIVENDASCORE.Models
{
    public class HistPedido
    {
        public int CodPed { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int NroSeq { get; set; }

        public Nullable<System.DateTime> DataOcorrencia { get; set; }
        public string Obs { get; set; }
    }
}
