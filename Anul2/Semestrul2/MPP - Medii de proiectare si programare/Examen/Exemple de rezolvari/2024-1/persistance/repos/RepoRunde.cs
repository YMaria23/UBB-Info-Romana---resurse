using log4net;
using model;

namespace persistance.repos;

public class RepoRunde : IRepoRunde
{
    
    private static readonly ILog log = LogManager.GetLogger("repo");
    private readonly string _connectionString;
    
    public RepoRunde(IDictionary<String, string> props)
    {
        log.Info("Creating Repo");
        _connectionString = props["connectionString"];
    }
    
    public Runda? Save(Runda entity)
    {
        try
        {
            using (var db = new TDbContext())
            {
                db.Runde.Add(entity);
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

    public Runda? Update(Runda entity)
    {
        return null;
    }

    public Runda? Delete(int id)
    {
        return null;
    }

    public Runda? FindById(int id)
    {
        log.Info($"FindById RepoJocuri: {id}");
        using (var db = new TDbContext())
        {
            var jucator = db.Runde.Find(id);
            return jucator;
        }
    }

    public List<Runda> FindAll()
    {
        log.Info("FindAll RepoJocuri ");
        using (var db = new TDbContext())
        {
            var jucatori = db.Runde.ToList();
            return jucatori;
        }
    }
}