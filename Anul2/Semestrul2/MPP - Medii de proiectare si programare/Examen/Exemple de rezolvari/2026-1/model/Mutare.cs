using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace model;

[Table("Mutari")]
public class Mutare
{
    
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int id { get; set; }
    
    [Column("poreclaJucator")]
    public string poreclaJucator { get; set; }
    
    [Column("idJoc")]
    public int idJoc { get; set; }
    
    [Column("runda")]
    public int runda { get; set; }
    
    [Column("pozInceput")]
    public int pozInceput { get; set; }
    
    [Column("pozFinal")]
    public int pozFinal { get; set; }
    
    [Column("puncteMutare")]
    public int puncteMuntare { get; set; }
    
    [Column("puncteTotal")]
    public int puncteTotal { get; set; }
}