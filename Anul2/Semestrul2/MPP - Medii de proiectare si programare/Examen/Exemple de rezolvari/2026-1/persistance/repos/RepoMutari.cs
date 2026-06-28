using log4net;
using model;

namespace persistance.repos;

public class RepoMutari : IRepoMutari
{
    private static readonly ILog log = LogManager.GetLogger("repoMutari");
    IDictionary<String, string> props;

    public RepoMutari(IDictionary<String, string> props)
    {
        log.Info("Creating RepoMutari");
        this.props = props;
    }
    
    public Mutare? Save(Mutare entity)
    {
        try
        {
            using (var db = new MPPDbContext())
            {
                db.Mutari.Add(entity);
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
        }
    }

    public Mutare? Update(Mutare entity)
    {
        return null;
    }

    public Mutare? Delete(int id)
    {
        return null;
    }

    public Mutare? FindById(int id)
    {
        using (var db = new MPPDbContext())
        {
            var mutare = db.Mutari.Find(id);
            return mutare;
        }
    }

    public List<Mutare> FindAll()
    {
        using (var db = new MPPDbContext())
        {
            var mutari = db.Mutari.ToList();
            return mutari;
        }
    }
}