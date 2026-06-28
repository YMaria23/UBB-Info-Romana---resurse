using log4net;
using model;

namespace persistance.repos;

public class RepoBarci:IRepoBarci
{
    private static readonly ILog log = LogManager.GetLogger("repo");
    private readonly string _connectionString;
    
    public RepoBarci(IDictionary<String, string> props)
    {
        log.Info("Creating Repo");
        _connectionString = props["connectionString"];
    }
    
    public Barca? Save(Barca entity)
    {
        try
        {
            using (var db = new TDbContext())
            {
                db.Barci.Add(entity);
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

    public Barca? Update(Barca entity)
    {
        return null;
    }

    public Barca? Delete(int id)
    {
        return null;
    }

    public Barca? FindById(int id)
    {
        log.Info($"FindById RepoJocuri: {id}");
        using (var db = new TDbContext())
        {
            var jucator = db.Barci.Find(id);
            return jucator;
        }
    }

    public List<Barca> FindAll()
    {
        log.Info("FindAll RepoJocuri ");
        using (var db = new TDbContext())
        {
            var jucatori = db.Barci.ToList();
            return jucatori;
        }
    }
}