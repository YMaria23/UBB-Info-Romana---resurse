using System.Data;
using log4net;
using model;

namespace persistance.repos;

public class RepoJucatori : IRepoJucatori
{
    private static readonly ILog log = LogManager.GetLogger("repoJucatori");
    IDictionary<String, string> props;

    public RepoJucatori(IDictionary<String, string> props)
    {
        log.Info("Creating RepoJucatori");
        this.props = props;
    }

    public Jucator? Save(Jucator entity)
    {
        return null;
    }

    public Jucator? Update(Jucator entity)
    {
        return null;
    }

    public Jucator? Delete(int id)
    {
        return null;
    }

    public Jucator? FindById(int id)
    {
        log.Info($"FindById RepoJucatori: {id}");
        using (MPPDbContext db = new MPPDbContext())
        {
            var jucator = db.Jucatori.Find(id);
            return jucator;
        }
    }

    public List<Jucator> FindAll()
    {
        log.Info("FindAll RepoJucatori ");
        using (MPPDbContext db = new MPPDbContext())
        {
            var jucatori = db.Jucatori.ToList();
            return jucatori;
        }
    }

    public Jucator? FindByName(string name)
    {
        log.Info("FindByName RepoJucatori: " + name);
        using (MPPDbContext db = new MPPDbContext())
        {
            var jucator = db.Jucatori.FirstOrDefault(j => j.name == name);
            return jucator;
        }
    }
}