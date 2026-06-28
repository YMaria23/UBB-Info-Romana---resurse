using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Identity.Client;
using System.Diagnostics;
using System.Drawing.Text;
using System.IO;
using EFCoreSecondLevelCacheInterceptor;
using Microsoft.Extensions.Logging;

public class TeatruDbContext : DbContext
{
    public DbSet<TeatruNational> TeatruNational { get; set; }
    public DbSet<Spectacol> Spectacole { get; set; }
    public DbSet<Angajati> Angajati { get; set; }

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

            var serviceProvider = new ServiceCollection()
                                    .AddLogging()
                                    .AddMemoryCache()
                                    .AddEFSecondLevelCache(options =>
                                    {
                                        options.UseMemoryCacheProvider();
                                        //options.CacheAllQueries(CacheExpirationMode.Absolute, TimeSpan.FromMinutes(5)); -> doar daca vreau sa fie activat pe oricare query
                                    })
                                    .BuildServiceProvider();

            // se extrage stringul de conexiune
            string connectionString = configuration.GetConnectionString("TeatruConnection");
            string connectionWithoutPool = configuration.GetConnectionString("WithoutPoolConnection");

            // se aplica configuratia
            if(usePooling)
                optionsBuilder.UseSqlServer(connectionString);
            else
                optionsBuilder.UseSqlServer(connectionWithoutPool);

            var interceptor = serviceProvider.GetRequiredService<SecondLevelCacheInterceptor>();
            optionsBuilder.AddInterceptors(interceptor);

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


        modelBuilder.Entity<TeatruNational>()
        .HasMany(d => d.Angajati)
        .WithOne(e => e.TeatruNational)
        .HasForeignKey(e => e.IdT);

        // pentru coloana Denumire - unique
        modelBuilder.Entity<TeatruNational>()
            .HasIndex(a => a.Denumire)
            .IsUnique();
    }
}