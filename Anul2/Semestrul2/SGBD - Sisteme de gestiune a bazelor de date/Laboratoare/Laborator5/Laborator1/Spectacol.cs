using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("Spectacole")]
public class Spectacol
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int IdS { get; set; }

    [Required]
    [MaxLength(100)]
    [Column("Denumire")]
    public string Denumire { get; set; }


    [MaxLength(100)]
    [Column("Descriere")]
    public string? Descriere { get; set; }

   
    [Column("IdT")]
    public int IdT { get; set; }

    [ForeignKey("IdT")]

    [Timestamp]
    public byte[] RowVersion { get; set; }

    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }
    public string DeletedBy { get; set; }
    public virtual TeatruNational TeatruNational { get; set; }
}