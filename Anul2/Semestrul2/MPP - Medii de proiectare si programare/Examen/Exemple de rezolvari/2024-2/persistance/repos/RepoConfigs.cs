using log4net;
using model;

namespace persistance.repos;

public class RepoConfigs: IRepoConfigs
{
    private static readonly ILog log = LogManager.GetLogger("repo");
    private readonly string _connectionString;
    
    public RepoConfigs(IDictionary<String, string> props)
    {
        log.Info("Creating Repo");
        _connectionString = props["connectionString"];
    }
    
    public Configuratie? Save(Configuratie entity)
    {
        try
        {
            using (var db = new TDbContext())
            {
                db.Configs.Add(entity);
                var linesModified = db.SaveChanges();
                
                if (linesModified > 0)
                    return entity;
                
                return null;
            }
        }
        catch (Exception e)
        {
            log.Error("Error during Save", e);
            return null;
        };   
    }

    public Configuratie? Update(Configuratie entity)
    {
        try
        {
            using (var db = new TDbContext())
            {
                db.Configs.Update(entity);
                db.SaveChanges();
                return entity;
            }
        }
        catch (Exception e)
        {
            log.Error("Error during Update", e);
            return null;
        }
    }

    public Configuratie? Delete(int id)
    {
        return null;
    }

    public Configuratie? FindById(int id)
    {
        log.Info($"FindById RepoJocuri: {id}");
        using (var db = new TDbContext())
        {
            var jucator = db.Configs.Find(id);
            return jucator;
        }
    }

    public List<Configuratie> FindAll()
    {
        log.Info("FindAll RepoJocuri ");
        using (var db = new TDbContext())
        {
            var jucatori = db.Configs.ToList();
            return jucatori;
        }
    }
}