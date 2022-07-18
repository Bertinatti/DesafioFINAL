using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DesafioFINAL.Models
{
    [Table("LogProdutos")]
    public class LogProdutos
    {
        [Key]
        [Column("IdLog", TypeName = "integer")]
        [Display(Name = "Id do Log")]
        public int IdLog { get; set; }

        [Required]
        [Column("EmailUsuario", TypeName = "varchar(250)")]
        [Display(Name = "Usuário")]
        public string EmailUsuario { get; set; }

        [ForeignKey("Produto")]
        [Column("IdProduto", TypeName = "integer")]
        [Display(Name = "Id do Produto")]
        public int? IdProduto { get; set; }
        public virtual Produto? Produto { get; set; }

        [Required]
        [Column("AcaoLog", TypeName = "varchar(250)")]
        [Display(Name = "Ação realizada")]
        public string AcaoLog { get; set; } = null!;

        [Required]
        [Column("DataLog")]
        [Display(Name = "Data e hora da ação")]
        [DataType(DataType.DateTime)]
        public DateTime DataLog { get; set; }
    }
}
