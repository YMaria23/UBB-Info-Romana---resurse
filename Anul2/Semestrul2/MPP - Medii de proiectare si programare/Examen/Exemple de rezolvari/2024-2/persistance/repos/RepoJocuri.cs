using log4net;
using model;

namespace persistance.repos;

public class RepoJocuri: IRepoJocuri
{
    private static readonly ILog log = LogManager.GetLogger("repo");
    private readonly string _connectionString;
    
    public RepoJocuri(IDictionary<String, string> props)
    {
        log.Info("Creating Repo");
        _connectionString = props["connectionString"];
    }


    public Joc? Save(Joc entity)
    {
        try
        {
            using (var db = new TDbContext())
            {
                db.Jocuri.Add(entity);
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

    public Joc? Update(Joc entity)
    {
        try
        {
            using (var db = new TDbContext())
            {
                db.Jocuri.Update(entity);
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

    public Joc? Delete(int id)
    {
        return null;
    }

    public Joc? FindById(int id)
    {
        log.Info($"FindById RepoJocuri: {id}");
        using (var db = new TDbContext())
        {
            var jucator = db.Jocuri.Find(id);
            return jucator;
        }
    }

    public List<Joc> FindAll()
    {
        log.Info("FindAll RepoJocuri ");
        using (var db = new TDbContext())
        {
            var jucatori = db.Jocuri.ToList();
            return jucatori;
        }
    }
}