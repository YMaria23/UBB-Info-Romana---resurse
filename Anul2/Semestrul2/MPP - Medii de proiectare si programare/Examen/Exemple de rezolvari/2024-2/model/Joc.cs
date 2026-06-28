using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace model;

[Table("Jocuri")]
public class Joc
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int id { get; set; }
    
    [Column("poreclaJucator")]
    public string poreclaJucator { get; set; }
    
    [Column("punctaj")]
    public int punctaj { get; set; }
    
    [Column("idConfig")]
    public int idConfig { get; set; }
    
    [Column("litereAsemenea")]
    public int litereAsemenea { get; set; }
    
    [Column("rundaCurenta")]
    public int rundaCurenta { get; set; }
    
    [Column("status")]
    public string status { get; set; }
    
    [Column("oraInceput")]
    public DateTime oraInceput { get; set; }
}