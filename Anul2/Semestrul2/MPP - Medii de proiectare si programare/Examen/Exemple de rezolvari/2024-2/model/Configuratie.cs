using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace model;

[Table("Configuratii")]
public class Configuratie
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int id { get; set; }
    
    [Column("opt1")]
    public string opt1 { get; set; }
    
    [Column("opt2")]
    public string opt2 { get; set; }
    
    [Column("opt3")]
    public string opt3 { get; set; }
    
    [Column("opt4")]
    public string opt4 { get; set; }
}