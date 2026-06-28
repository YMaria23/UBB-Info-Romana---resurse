using System.Data;
using log4net;
using Microsoft.Data.Sqlite;
using model;

namespace persistance.repos;

public class RepoConfiguratii : IRepoConfiguratii
{
    private static readonly ILog log = LogManager.GetLogger("repoConfiguratii");
    private readonly string _connectionString;
    
    public RepoConfiguratii(IDictionary<String, string> props)
    {
        log.Info("Creating RepoConfiguratii");
        _connectionString = props["connectionString"];
    }

    public Configuratie? Save(Configuratie entity)
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
    }
}