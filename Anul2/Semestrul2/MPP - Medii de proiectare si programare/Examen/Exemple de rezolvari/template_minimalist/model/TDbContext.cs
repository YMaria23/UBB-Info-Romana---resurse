namespace model;

using Microsoft.EntityFrameworkCore;

public class TDbContext : DbContext
{
    /* SE DEFINESC TABELELE
    public DbSet<Jucator> Jucatori { get; set; }
    public DbSet<Mutare> Mutari { get; set; }
    
    public DbSet<Joc> Jocuri { get; set; }
    public DbSet<Participare> Participare { get; set; }
    */
    
    // constructor default
    public TDbContext(): base(){}
    
    
    // configurare directa (implicita)
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // pooling-ul este activat implicit
        // SE PUNE BAZA DE DATE CURENTA
        string connectionString = @"Data Source = C:\Facultate\MPPExam\templ\template.db";
        optionsBuilder.UseSqlite(connectionString);
        optionsBuilder.LogTo(Console.WriteLine); 
    }
    
    // se mai pot adauga relatii, dar mai safe e sa nu
    
}