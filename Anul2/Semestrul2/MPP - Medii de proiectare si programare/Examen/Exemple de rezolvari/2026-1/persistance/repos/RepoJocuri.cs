using log4net;
using model;

namespace persistance.repos;

public class RepoJocuri : IRepoJoc
{
    private static readonly ILog log = LogManager.GetLogger("repoJocuri");
    IDictionary<String, string> props;

    public RepoJocuri(IDictionary<String, string> props)
    {
        log.Info("Creating RepoJocuri");
        this.props = props;
    }

    public Joc? Save(Joc entity)
    {
        try
        {
            using (var db = new MPPDbContext())
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
        }
    }

    public Joc? Update(Joc entity)
    {
        try
        {
            using (var db = new MPPDbContext())
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
        using (MPPDbContext db = new MPPDbContext())
        {
            var jucator = db.Jocuri.Find(id);
            return jucator;
        }
    }

    public List<Joc> FindAll()
    {
        log.Info("FindAll RepoJocuri ");
        using (MPPDbContext db = new MPPDbContext())
        {
            var jucatori = db.Jocuri.ToList();
            return jucatori;
        }
    }
}