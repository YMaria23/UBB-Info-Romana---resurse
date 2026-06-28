using log4net;
using model;

namespace persistance.repos;

public class RepoJucatori: IRepoJucatori
{
    private static readonly ILog log = LogManager.GetLogger("repo");
    private readonly string _connectionString;
    
    public RepoJucatori(IDictionary<String, string> props)
    {
        log.Info("Creating Repo");
        _connectionString = props["connectionString"];
    }


    public Jucator? Save(Jucator entity)
    {
        return null;
    }

    public Jucator? Update(Jucator entity)
    {
        try
        {
            using (var db = new TDbContext())
            {
                db.Jucatori.Update(entity);
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

    public Jucator? Delete(int id)
    {
        return null;
    }

    public Jucator? FindById(int id)
    {
        log.Info($"FindById RepoJucatori: {id}");
        using (var db = new TDbContext())
        {
            var jucator = db.Jucatori.Find(id);
            return jucator;
        }
    }

    public List<Jucator> FindAll()
    {
        log.Info("FindAll RepoJucatori ");
        using (var db = new TDbContext())
        {
            var jucatori = db.Jucatori.ToList();
            return jucatori;
        }
    }

    public Jucator? FindByPorecla(string porecla)
    {
        log.Info("FindByName RepoJucatori: " +porecla);
        using (TDbContext db = new TDbContext())
        {
            var jucator = db.Jucatori.FirstOrDefault(j => j.porecla == porecla);
            return jucator;
        }
    }
}