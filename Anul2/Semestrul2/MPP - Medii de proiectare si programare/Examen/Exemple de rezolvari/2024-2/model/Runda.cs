using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace model;

[Table("Runda")]
public class Runda
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int id { get; set; }
    
    [Column("alegereJucator")]
    public string alegereJucator { get; set; }
    
    [Column("alegereJoc")]
    public string alegereJoc { get; set; }
    
    [Column("poreclaJucator")]
    public string poreclaJucator { get; set; }
    
    [Column("idJoc")]
    public int idJoc { get; set; }
    
    [Column("nrRunda")]
    public int nrRunda { get; set; }
}