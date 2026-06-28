using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("Angajati")]
public class Angajati
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int IdAngajat { get; set; }

    [Required]
    [MaxLength(100)]
    [Column("Nume")]
    public string Nume { get; set; }

    [Required]
    [MaxLength(100)]
    [Column("Prenume")]
    public string Prenume { get; set; }


    [Required]
    [RegularExpression(@"^[0-9]{10}$", ErrorMessage = "Telefonul trebuie sa aiba exact 10 cifre.")]
    [MaxLength(100)]
    [Column("Telefon")]
    public string Telefin { get; set; }

    [Column("Salariu")]
    public int? Salariu { get; set; }

   
    [Column("IdT")]
    public int IdT { get; set; }

    [ForeignKey("IdT")]
    public virtual TeatruNational TeatruNational { get; set; }
}