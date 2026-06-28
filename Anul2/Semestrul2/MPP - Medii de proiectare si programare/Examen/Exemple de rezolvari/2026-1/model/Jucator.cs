using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace model;

[Table("Jucatori")]
public class Jucator
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int id { get; set; }
    
    [MaxLength(20)]
    [Required]
    [Column("name")]
    public string name { get; set; }
}