using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace model;


[Table("Barci")]
public class Barca
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int id { get; set; }
    
    [Column("pozitie1")]
    public string pozitie1 { get; set; }
    
    [Column("pozitie2")]
    public string pozitie2 { get; set; }
    
    [Column("pozitie3")]
    public string pozitie3 { get; set; }
}