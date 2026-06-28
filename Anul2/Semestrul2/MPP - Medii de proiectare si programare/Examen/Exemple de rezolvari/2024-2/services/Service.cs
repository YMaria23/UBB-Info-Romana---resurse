using log4net;
using model;
using persistance;

namespace services;

public class Service
{
    // se introduc repo-urile corespunzatoare
    IRepoConfigs repoConfigs;
    IRepoJucatori repoJucatori;
    IRepoRunde repoRunde;
    IRepoJocuri repoJocuri;
    
    
    private static readonly ILog log = LogManager.GetLogger("service");

    // constructorul
    public Service(IRepoConfigs configs, IRepoJocuri repoJocuri, IRepoJucatori repoJucatori, IRepoRunde repoRunde)
    {
        this.repoConfigs = configs;
        this.repoJucatori = repoJucatori;
        this.repoJocuri = repoJocuri;
        this.repoRunde = repoRunde;
    }
    
    // LOGIN
    public async Task<object> JoinGame(string player)
    {
        // verifica porecla
        var jucator = repoJucatori.FindByPorecla(player);
        if (jucator == null)
            throw new Exception("Jucatori insuficienti");
        
        // DACA AM JOC IN DB
        
        // alege configuratie
        var nrBarci = repoConfigs.FindAll().Count;
        Random rnd = new Random();
        int numar = rnd.Next(1, nrBarci + 1);
        var config = repoConfigs.FindById(numar);
        
        var game = new Joc
        {
            rundaCurenta = 1,
            poreclaJucator = player,
            status = "Playing",
            punctaj = 0,
            idConfig = numar,
            litereAsemenea = 0,
            oraInceput = DateTime.UtcNow
        };

        var val = repoJocuri.Save(game);
        if (val == null)
        {
            throw new Exception("Eroare la salvarea jocului");
        }

        List<string> lista = new List<string>();
        lista.Add(config.opt1);
        lista.Add(config.opt2);
        lista.Add(config.opt3);
        lista.Add(config.opt4);
        

        return new
        {
            gameId = game.id,
            lista = lista
        };
    }
    
    // REST 2 - se modifica o configuratie
    public async Task<object> UpdateConfig(string opt1, string opt2, string opt3, string opt4, int id)
    {
        var config = repoConfigs.FindById(id);
        if (config == null)
            throw new Exception("Nu exista configuratia");

        var conf = new Configuratie
        {
            opt1 = opt1,
            opt2 = opt2,
            opt3 = opt3,
            opt4 = opt4,
            id = id
        };

        repoConfigs.Update(conf);
        return new
        {
            status = "updated",
        };
    }
    
    // REST 1 - vizualizarea tuturor jocurilor finalizate ale unui jucator in care a ghicit minim o pozitie
    public async Task<object> ViewGames(string porecla)
    {
        var jocuri = repoJocuri.FindAll()
            .Where(j => j.poreclaJucator == porecla && j.litereAsemenea >=1)
            .ToList();
        
        var rezultat = new List<object>();
        
        foreach (var joc in jocuri)
        {
            var runde = repoRunde.FindAll()
                .Where(r => r.idJoc == joc.id)
                .ToList();

            var pozitii = new List<object>();

            foreach (var runda in runde)
                pozitii.Add(new
                {
                    runda = runda.nrRunda,
                    ghicitJucator = runda.alegereJucator,
                    ghicitServer = runda.alegereJoc
                });
            
            rezultat.Add(new
            {
                punctajTotal = joc.punctaj,
                pozitii = pozitii
            });
        }
        
        return rezultat;
    }
    
    // Alegere litera
    public async Task<object> ChooseLitera(string opt, string porecla)
    {
        // se determina jocul
        var game = repoJocuri.FindAll()
            .FirstOrDefault(j => j.poreclaJucator == porecla && j.status == "Playing");
        
        // se determina config barcii
        var config = repoConfigs.FindAll()
            .FirstOrDefault(b => b.id == game.idConfig);

        var runda = game.rundaCurenta;

        // serverul alege un nr
        Random rnd = new Random();
        int numar = rnd.Next(1, 5);

        var ales = config.opt1;
        if (numar == 2)
            ales = config.opt2;
        else if (numar == 3)
            ales = config.opt3;
        else if (numar == 4)
            ales = config.opt4;
        
        // se verifica optiunile
        var opJuc = opt.Split(",");
        var opJoc = ales.Split(",");
        
        var optJucnr = int.Parse(opJuc[1]);
        var optJocnr = int.Parse(opJoc[1]);

        var punctaj = 0;

        if (optJucnr < optJocnr)
        {
            punctaj -= optJucnr;
        }
        else if (optJucnr > optJocnr)
        {
            punctaj += optJucnr + optJocnr;
        }
        else
        {
            //cazul in care sunt egale
            game.litereAsemenea++;
        }

        game.rundaCurenta++;
        game.punctaj += punctaj;
        repoJocuri.Update(game);
        
        // se creeaza si mutarea curenta
        var mutare = new Runda
        {
            alegereJoc = ales,
            alegereJucator = opt,
            poreclaJucator = porecla,
            nrRunda = runda,
            idJoc = game.id
        };

        repoRunde.Save(mutare);

        var status = "";
        if(runda < 4)
            status = "playing";
        else
        {
            game.status = "Finished";
            repoJocuri.Update(game);
            
            status = "finished";
        }

        return new
        {
            status = status,
            punctaj = punctaj,
            alegereJoc = ales
        };
    }
    
    // Finalul jocului
    public async Task<object> FinishGame(string porecla, int idGame)
    {
        var game = repoJocuri.FindById(idGame);
        var clasament = await Clasament();
        
        return new
        {
            punctaj = game.punctaj,
            lista = clasament
        };
    }

    public async Task<object> Clasament()
    {
        // se returneaza toate jocurile finalizate, descrescator dupa punctajul obtinut
        var jocuri = repoJocuri.FindAll()
            .Where(j => j.status == "Finished")
            .OrderByDescending(j => j.punctaj)
            .ToList();

        return jocuri;
    }
}