using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("Managers")]
public class Manager
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int IdM { get; set; }

    [Required]
    [MaxLength(20)]
    [Column("Nume")]
    public string Nume { get; set; }


    [Required]
    [MaxLength(20)]
    [Column("Prenume")]
    public string Prenume { get; set; }


    [Column("IdT")]
    public int IdT { get; set; }

    [ForeignKey("IdT")]

    public virtual TeatruNational TeatruNational { get; set; }
}