using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Client;
using System.Drawing.Text;
using System.IO;
using System.Diagnostics;

public class TeatruDbContext : DbContext
{
    public DbSet<TeatruNational> TeatruNational { get; set; }
    public DbSet<Spectacol> Spectacole { get; set; }

    private bool usePooling = true;

    // Constructor for configuration injection
    public TeatruDbContext(DbContextOptions<TeatruDbContext> options) : base(options) { }
    public TeatruDbContext():base() { }

    public TeatruDbContext(bool use) : base()
    {
        usePooling = use;
    }

    // Alternative: configure directly in DbContext
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {

        if (!optionsBuilder.IsConfigured)
        {
            // se citeste fisierul de configurare
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();

            // se extrage stringul de conexiune
            string connectionString = configuration.GetConnectionString("TeatruConnection");
            string connectionWithoutPool = configuration.GetConnectionString("WithoutPoolConnection");

            // se aplica configuratia
            if(usePooling)
                optionsBuilder.UseSqlServer(connectionString);
            else
                optionsBuilder.UseSqlServer(connectionWithoutPool);

                optionsBuilder.UseLazyLoadingProxies(); 
            optionsBuilder.LogTo(message => Debug.WriteLine(message)); 
        }

    }

    // Configure entity relationships using Fluent API
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TeatruNational>()
        .HasMany(d => d.Spectacole)
        .WithOne(e => e.TeatruNational)
        .HasForeignKey(e => e.IdT);

        // pentru coloana Denumire - unique
        modelBuilder.Entity<TeatruNational>()
            .HasIndex(a => a.Denumire)
            .IsUnique();
    }
}