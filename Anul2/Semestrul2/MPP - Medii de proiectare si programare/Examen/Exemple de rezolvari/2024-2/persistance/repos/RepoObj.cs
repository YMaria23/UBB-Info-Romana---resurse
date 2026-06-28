using log4net;

namespace persistance.repos;

// va implementa o interfata
public class RepoObj
{
    // SCHIMBA NUMELE IN LOG4NET.CONFIG + SETEAZA COPY IF NEWER
    private static readonly ILog log = LogManager.GetLogger("repo");
    private readonly string _connectionString;
    
    public RepoObj(IDictionary<String, string> props)
    {
        log.Info("Creating Repo");
        _connectionString = props["connectionString"];
    }
    
    // METODE ORM
    /*
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
    */
    
    ////////////////////////////////////////////////////////////////////////////////////
    //METODE CLASICE
    /* public Configuratie? Save(Configuratie entity)
    {
        log.InfoFormat("Saving task {0}", entity);
        using (var con = new SqliteConnection(_connectionString))
        {
            con.Open();
            using (var comm = con.CreateCommand())
            {
                comm.CommandText = "insert into Configuratii (n,valPozitii) values (@n,@valori); SELECT last_insert_rowid();";

                var paramDesc = comm.CreateParameter();
                paramDesc.ParameterName = "@n";
                paramDesc.Value = entity.N;
                comm.Parameters.Add(paramDesc);
                
                var paramDesc1 = comm.CreateParameter();
                paramDesc1.ParameterName = "@valori";
                paramDesc1.Value = entity.ValPozitii;
                comm.Parameters.Add(paramDesc1);

                var result = comm.ExecuteScalar();
                if (result == null)
                {
                    log.InfoFormat("Exiting Save with value {0}", null);
                    throw new Exception("No activities added !");
                }
                else
                {
                    int generatedId = Convert.ToInt32(result);
                    entity.Id = generatedId;
                    log.InfoFormat("Task saved {0}", entity);
                }
            }
        }
        log.InfoFormat("Exiting Save with value {0}", entity);
        return entity;
    }

    public Configuratie? Update(Configuratie entity)
    {
        return null;
    }

    public Configuratie? Delete(int id)
    {
        return null;
    }

    public Configuratie? FindById(int id)
    {
        return null;
    }

    public List<Configuratie> FindAll()
    {
        log.Info("Retrieving All Configuraties");
        List<Configuratie> conf = new List<Configuratie>();
        using (var con = new SqliteConnection(_connectionString))
        {
            con.Open();
            using (var comm = con.CreateCommand())
            {
                comm.CommandText = "select id,n, valPozitii from Configuratii";

                using (var dataR = comm.ExecuteReader())
                {
                    while (dataR.Read())
                    {
                        int id = dataR.GetInt32(0);
                        int n = dataR.GetInt32(1);
                        string valPozitii = dataR.GetString(2);
                        //SortingOrder order = (SortingOrder)Enum.Parse(typeof(SortingOrder), dataR.GetString(3));
                        //SortingAlgorithm algo = (SortingAlgorithm)Enum.Parse(typeof(SortingAlgorithm), dataR.GetString(4));
                        Configuratie c = new Configuratie(id, n, valPozitii);
                        conf.Add(c);
                    }
                }
            }
        }

        return conf;
    }*/
}