using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace model;

[Table ("Jocuri")]
public class Joc
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int id { get; set; }
    
    [Column("rundaCurenta")]
    public int rundaCurenta { get; set; }
    
    [Column("oraInceput")]
    public DateTime oraInceput { get; set; }
    
    [Column("punctaj")]
    public int punctaj { get; set; }
    
    [Column("idBarca")]
    public int idBarca { get; set; }
    
    [Column("status")]
    public string status { get; set; }
    
    [Column("poreclaJucator")]
    public string poreclaJucator { get; set; }
    
    [Column("pozitiiGhicite")]
    public int pozitiiGhicite { get; set; }
}