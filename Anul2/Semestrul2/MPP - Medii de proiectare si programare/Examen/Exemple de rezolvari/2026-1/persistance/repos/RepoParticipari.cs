using log4net;
using model;

namespace persistance.repos;

public class RepoParticipari: IRepoParticipari
{
    private static readonly ILog log = LogManager.GetLogger("repoParticipari");
    private readonly string _connectionString;
    
    public RepoParticipari(IDictionary<String, string> props)
    {
        log.Info("Creating RepoParticipari");
        _connectionString = props["connectionString"];
    }

    public Participare? Save(Participare entity)
    {
        try
        {
            using (var db = new MPPDbContext())
            {
                db.Participare.Add(entity);
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

    public Participare? Update(Participare entity)
    {
        try
        {
            using (var db = new MPPDbContext())
            {
                db.Participare.Update(entity);
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

    public Participare? Delete(int id)
    {
        return null;
    }

    public Participare? FindById(int id)
    {
        return null;
    }

    public List<Participare> FindAll()
    {
        log.Info("FindAll RepoParticipari ");
        using (MPPDbContext db = new MPPDbContext())
        {
            var jucatori = db.Participare.ToList();
            return jucatori;
        }
    }

    public Participare? FindByJucator(string jucator)
    {
        log.Info("FindByJucator RepoParticipari: " + jucator);
        using (MPPDbContext db = new MPPDbContext())
        {
            var j = db.Participare.FirstOrDefault(j => j.PoreclaJucator == jucator);
            return j;
        }
    }

    public Participare? FindByJoc(int idJoc)
    {
        log.Info("FindByJoc RepoParticipari: " + idJoc);
        using (MPPDbContext db = new MPPDbContext())
        {
            var jucator = db.Participare.FirstOrDefault(j => j.IdJoc == idJoc);
            return jucator;
        }
    }
}