using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace model;

[Table("Jocuri")]
public class Joc
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int id { get; set; }
    
    [Column("n")]
    public int n { get; set; }
    
    [Column("conf")]
    public string? conf { get; set; }
    
    [Column("status")]
    public string status { get; set; }
    
    [Column("rundaCurenta")]
    public int rundaCurenta { get; set; }
    
    [Column("jucatorCurentIndex")]
    public int jucatorCurentIndex { get; set; }
    
}