using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace model;

[Table("Jucatori")]
public class Jucator
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int id { get; set; }
    
    [Column("porecla")]
    public string porecla { get; set; }
}