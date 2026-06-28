using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace model;

[Table ("Runde")]
public class Runda
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int id { get; set; }
    
    [Column("poreclaJucator")]
    public string poreclaJucator { get; set; }
    
    [Column("punctaj")]
    public int punctaj { get; set; }
    
    [Column("ghicit")]
    public int ghicit { get; set; }
    
    [Column("nrRunda")]
    public int nrRunda { get; set; }
    
    [Column("pozitieGhicita")]
    public string pozitieGhicita { get; set; }
    
    [Column("idJoc")]
    public int idJoc { get; set; }
}