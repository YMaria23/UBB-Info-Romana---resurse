// See https://aka.ms/new-console-template for more information

using System.Configuration;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using persistance;
using persistance.repos;
using services;

public class Program
{
    public static void  Main(String[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowReactClient", policy =>
            {
                // se adauga portul pe care ruleaza clientul web
                policy.WithOrigins("http://localhost:5173", "http://127.0.0.1:5173")
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
            });
        });
        
        // configurare pentru SignalR
        builder.Services.AddSignalR();
        
        string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["templateDB"]?.ConnectionString;
        
        // Pentru Port
        string port = System.Configuration.ConfigurationManager.AppSettings["port"];
        string ipS=System.Configuration.ConfigurationManager.AppSettings["ip"];
        
        IDictionary<string, string> props = new SortedList<string, string>();
        props.Add("connectionString", GetConnectionStringByName("templateDB"));

        if (string.IsNullOrEmpty(connectionString))
        {
            Console.WriteLine("Warning: Connection string 'templateDB' not found in app.config!");
        }
        
        // searches for marked classes ([ApiController])
        builder.Services.AddControllers();
        
        
        builder.Services.AddScoped<IRepoJucatori>(sp => 
            new RepoJucatori(props));
        builder.Services.AddScoped<IRepoJocuri>(sp => 
            new RepoJocuri(props));
        builder.Services.AddScoped<IRepoBarci>(sp => 
            new RepoBarci(props));
        builder.Services.AddScoped<IRepoRunde>(sp => 
            new RepoRunde(props));
        
        builder.Services.AddScoped<Service>(sp => new Service(
            sp.GetRequiredService<IRepoBarci>(),
            sp.GetRequiredService<IRepoJucatori>(),
            sp.GetRequiredService<IRepoRunde>(),
            sp.GetRequiredService<IRepoJocuri>()
        ));
        
        // configure - REPOS & SERVICE
        /*
        builder.Services.AddScoped<IRepoJucatori>(sp => 
            new RepoJucatori(props));
        builder.Services.AddScoped<IRepoJoc>(sp => 
            new RepoJocuri(props));
        builder.Services.AddScoped<IRepoMutari>(sp => 
            new RepoMutari(props));
        builder.Services.AddScoped<IRepoParticipari>(sp => 
            new RepoParticipari(props));
        builder.Services.AddScoped<IRepoConfiguratii>(sp => 
            new RepoConfiguratii(props));
        
        builder.Services.AddScoped<Service>(sp => new Service(
            2,
            sp.GetRequiredService<IRepoJucatori>(),
            sp.GetRequiredService<IRepoConfiguratii>(),
            sp.GetRequiredService<IRepoMutari>(),
            sp.GetRequiredService<IRepoJoc>(),
            sp.GetRequiredService<IRepoParticipari>()
        ));
        */
        
        var app = builder.Build();
        // configuring the pipeline
        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseRouting();
        app.UseCors("AllowReactClient");
        
        // activarea securitatii
        //app.UseAuthentication(); 
        //app.UseAuthorization();

        // link between the URLs and the methods (from Controller)
        app.MapControllers(); 
        
        // mapare endpoint websocket
        app.MapHub<THub>("/tHub");
        app.Run("http://localhost:55556");
    }
    
    static string GetConnectionStringByName(string name)
    {
        // Assume failure.
        string returnValue = null;

        // Look for the name in the connectionStrings section.
        ConnectionStringSettings settings =System.Configuration.ConfigurationManager.ConnectionStrings[name];

        // If found, return the connection string.
        if (settings != null)
            returnValue = settings.ConnectionString;

        return returnValue;
    }
}

/* PENTRU CREAREA CLIENTULUI WEB
npm create vite@latest nume-proiect -- --template react
cd nume-proiect
npm install
npm run dev
*/