namespace model;

using Microsoft.EntityFrameworkCore;

public class MPPDbContext : DbContext
{
    
    public DbSet<Jucator> Jucatori { get; set; }
    public DbSet<Mutare> Mutari { get; set; }
    
    public DbSet<Joc> Jocuri { get; set; }
    public DbSet<Participare> Participare { get; set; }
    
    // constructor default
    public MPPDbContext(): base(){}
    
    
    // configurare directa (implicita)
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // pooling-ul este activat implicit
        string connectionString = @"Data Source = C:\Facultate\Semestrul 4\MPP\ex\MutariMPP.db";
        optionsBuilder.UseSqlite(connectionString);
        optionsBuilder.LogTo(Console.WriteLine); 
    }
    
}