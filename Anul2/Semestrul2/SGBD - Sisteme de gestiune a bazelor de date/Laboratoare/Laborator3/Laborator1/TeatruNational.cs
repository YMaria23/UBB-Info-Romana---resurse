using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("TeatruNational")]
public class TeatruNational
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int IdT { get; set; }

    [Required]
    [MaxLength(100)]
    [Column("Denumire")]
    public string Denumire { get; set; }

    [Required]
    [MaxLength(100)]
    [Column("Oras")]
    public string Oras { get; set; }

    [Required]
    [Column("AnInfiintare")]
    public float AnInfiintare { get; set; }


    // Navigation property for the one-to-many relationship
    public virtual ICollection<Spectacol> Spectacole { get; set; }
    public TeatruNational()
    {
        Spectacole = new List<Spectacol>();
    }
}