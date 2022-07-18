using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DesafioFINAL.Models
{
     public class Usuario : IdentityUser
    {
        [Required]
        [StringLength(250, ErrorMessage = "O {0} pode ter no máximo {1} digitos.")]
        [Column("Nome", TypeName = "varchar(250)")]
        [Display(Name = "Nome")]
        public string Nome { get; set; }

        [Required]
        [StringLength(250, ErrorMessage = "O {0} pode ter no máximo {1} digitos.")]
        [Column("Sobrenome", TypeName = "varchar(250)")]
        [Display(Name = "Sobrenome")]
        public string Sobrenome { get; set; }

        [Required]
        [StringLength(20, ErrorMessage = "O {0} pode ter no máximo {1} digitos.")]
        [Column("CEP", TypeName = "varchar(20)")]
        [Display(Name = "CEP")]
        public string CEP { get; set; }

        [Required]
        [StringLength(250, ErrorMessage = "O {0} pode ter no máximo {1} digitos.")]
        [Column("Endereco", TypeName = "varchar(250)")]
        [Display(Name = "Endereço")]
        public string Endereco { get; set; }

        [Required]
        [Column("Numero", TypeName = "integer")]
        [Display(Name = "Número")]
        public int Numero { get; set; }

        [Required]
        [StringLength(250, ErrorMessage = "O {0} pode ter no máximo {1} digitos.")]
        [Column("Bairro", TypeName = "varchar(250)")]
        [Display(Name = "Bairro")]
        public string Bairro { get; set; }

        [Required]
        [StringLength(250, ErrorMessage = "O {0} pode ter no máximo {1} digitos.")]
        [Column("Cidade", TypeName = "varchar(250)")]
        [Display(Name = "Cidade")]
        public string Cidade { get; set; }

        [Required]
        [StringLength(2, ErrorMessage = "O {0} pode ter no máximo {1} digitos.")]
        [Column("Estado", TypeName = "varchar(2)")]
        [Display(Name = "Estado")]
        public string Estado { get; set; }

        [Required]
        [Column("TipoUsuario", TypeName = "varchar(50)")]
        [Display(Name = "Tipo de Usuário")]
        public string TipoUsuario { get; set; }

    }
}
