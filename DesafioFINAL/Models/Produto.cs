using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DesafioFINAL
{
    [Table("Produtos")]
    public class Produto
    {
        [Key]
        [Column("IdProduto", TypeName = "integer")]
        [Display(Name = "Id do Produto")]
        public int IdProduto { get; set; }

        [Required]
        [StringLength(13, ErrorMessage = "O {0} precisar ter exatamente {1} digitos.", MinimumLength = 13)]
        [Column("CodigoEAN", TypeName = "varchar(13)")]
        [Display(Name = "Código de Barras")]
        public string CodigoEAN { get; set; } = null!;

        [Required]
        [Column("Nome", TypeName = "varchar(250)")]
        [Display(Name = "Nome do Produto")]
        public string Nome { get; set; } = null!;

        [Required]
        [DataType(DataType.Currency)]
        [Column("Preco", TypeName = "decimal(10,2)")]
        [Display(Name = "Preço do Produto")]
        public decimal Preco { get; set; }

        [Required]       
        [Column("Estoque", TypeName = "integer")]
        [Display(Name = "Quantidade em Estoque")]
        public int Estoque { get; set; }

        [Required]
        [Column("Fornecedor", TypeName = "varchar(250)")]
        [Display(Name = "Nome do Fornecedor")]
        public string Fornecedor{ get; set; } = null!;

        [Required]
        [StringLength(18, ErrorMessage = "O {0} precisar ter exatamente {1} digitos.", MinimumLength = 18)]
        [Column("CNPJFornecedor", TypeName = "varchar(18)")]
        [Display(Name = "CNPJ do Fornecedor")]
        public string CNPJFornecedor { get; set; } = null!;

    }
}
