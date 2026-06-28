using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace model;

[Table("Participari")]
public class Participare
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    [Column("IdJoc")]
    public int IdJoc { get; set; }
    
    [Column("PoreclaJucator")]
    public string PoreclaJucator { get; set; }
    
    [Column("EntryOrder")]
    public int EntryOrder { get; set; }
    
    [Column("Pozitie")]
    public int Pozitie { get; set; }
    
    [Column("Puncte")]
    public int Puncte { get; set; }
    
    //public virtual Joc Joc { get; set; }
}