using log4net;
using model;
using persistance;

namespace services;

public class Service
{
    // se introduc repo-urile corespunzatoare
    IRepoBarci repoBarci;
    IRepoJucatori repoJucatori;
    IRepoRunde repoRunde;
    IRepoJocuri repoJocuri;

    string[][] mesaj = [["- ","- ", "- ", "- ", "- "], ["- ", "- ", "- ", "- ", "- "], ["- ", "- ", "- ", "- ", "- "], ["- ","- ", "- ", "- ", "- "],["- ","- ","- ","- ","- "]];
    
    
    private static readonly ILog log = LogManager.GetLogger("service");

    // constructorul
    public Service(IRepoBarci repoBarci,IRepoJucatori repoJucatori,IRepoRunde repoRunde,IRepoJocuri repoJocuri)
    {
        this.repoBarci = repoBarci;
        this.repoJucatori = repoJucatori;
        this.repoRunde = repoRunde;
        this.repoJocuri = repoJocuri;
    }
    
    // LOGIN
    public async Task<object> JoinGame(string player)
    {
        // verifica porecla
        var jucator = repoJucatori.FindByPorecla(player);
        if (jucator == null)
            throw new Exception("Nu exista jucatorul in baza de date!");
        
        // DACA AM JOC IN DB
        // trebuie sa creez joc in db
        
        // alege configuratie barca 
        var nrBarci = repoBarci.FindAll().Count;
        Random rnd = new Random();
        int numar = rnd.Next(1, nrBarci + 1);
        
        var game = new Joc
        {
                rundaCurenta = 1,
                poreclaJucator = player,
                status = "Playing",
                oraInceput = DateTime.UtcNow,
                punctaj = 0,
                idBarca = numar,
        };

        var val = repoJocuri.Save(game);
        if (val == null)
        {
            throw new Exception("Eroare la salvarea jocului");
        }

        var jocuri = repoJocuri.FindAll()
            .Where(j => j.status == "Finished")
            .ToList();
        

        return new
        {
            gameId = game.id,
            template = mesaj,
            jocuri = jocuri
        };
    }
    
    
    
    // REST 2 - adauga conf barca
    public async Task<object> AddBarca(string poz1, string poz2, string poz3)
    {
        // se verifica daca e valid
        var pozitie1 = poz1.Split(",");
        var pozitie2 = poz2.Split(",");
        var pozitie3 = poz3.Split(",");

        if (!((pozitie1[0] == pozitie2[0] && pozitie2[0] == pozitie3[0]) ||
              (pozitie1[1] == pozitie2[1] && pozitie2[1] == pozitie3[1]) ||
              (pozitie1[0] == pozitie2[0] && pozitie2[1] == pozitie3[1]) ||
              (pozitie1[1] == pozitie2[1] && pozitie2[0] == pozitie3[0]) ||
              (pozitie1[0] == pozitie3[0] && pozitie3[1] == pozitie2[1]) ||
              (pozitie1[1] == pozitie3[1] && pozitie3[0] == pozitie2[0])))
            throw new Exception("Formatare invalida!");
        
        // acum se poate adauga barca
        var barca = new Barca
        {
            pozitie1 = poz1,
            pozitie2 = poz2,
            pozitie3 = poz3
        };
        
        repoBarci.Save(barca);

        return new
        {
            status = "succes"
        };
    }
    
    
    // REST 1 - vizualizarea tuturor jocurilor finalizate ale unui jucator in care a ghicit minim o pozitie
    public async Task<object> ViewGames(string porecla)
    {
        var jocuri = repoJocuri.FindAll()
            .Where(j => j.poreclaJucator == porecla && j.pozitiiGhicite >=1)
            .ToList();
        
        var rezultat = new List<object>();
        
        foreach (var joc in jocuri)
        {
            var barca = repoBarci.FindById(joc.idBarca);
            
            var runde = repoRunde.FindAll()
                .Where(r => r.idJoc == joc.id)
                .ToList();

            var pozitii = "";
            
            foreach(var runda in runde)
                if (runda.nrRunda == 1)
                {
                    pozitii += $"Runda1: poz_corecta {barca.pozitie1}, poz_aleasa {runda.pozitieGhicita}";
                }
                else if (runda.nrRunda == 2)
                {
                    pozitii += $"Runda2: poz_corecta {barca.pozitie2}, poz_aleasa {runda.pozitieGhicita}";
                }
                else
                {
                    pozitii += $"Runda3: poz_corecta {barca.pozitie3}, poz_aleasa {runda.pozitieGhicita}";
                }
            

            rezultat.Add(new
            {
                punctajTotal = joc.punctaj,
                pozitii = pozitii
            });
        }
        
        return rezultat;
    }
    
    
    // Ghiceste pozitia
    public async Task<object> ChoosePoz(string pozitie, string porecla, string[][]m)
    {
        // se determina jocul
        var game = repoJocuri.FindAll()
            .FirstOrDefault(j => j.poreclaJucator == porecla && j.status == "Playing");
        
        // se determina config barcii
        var barca = repoBarci.FindAll()
            .FirstOrDefault(b => b.id == game.idBarca);

        var runda = game.rundaCurenta;

        if (runda <= 3)
        {
            var punctaj = 0;
            var poz = pozitie.Split(",").Select(int.Parse).ToArray();
            var ghicit = 0;
            
            if (pozitie == barca.pozitie1 || pozitie == barca.pozitie2 || pozitie == barca.pozitie3)
            {
                punctaj = 5;
                m[poz[0] - 1][poz[1] - 1] = "B";
                ghicit = 1;

                game.pozitiiGhicite += 1;
                repoJocuri.Update(game);
            }
            else
            {
                punctaj = -3;
                
                var poz1 = barca.pozitie1.Split(",").Select(int.Parse).ToArray();
                var poz2 = barca.pozitie2.Split(",").Select(int.Parse).ToArray();
                var poz3 = barca.pozitie3.Split(",").Select(int.Parse).ToArray();
                
                // se determina cea mai mica distanta
                
                var dist1 = Math.Pow((poz[0] -  poz1[0]),2) + Math.Pow((poz[1] -  poz1[1]),2);
                var dist2 = Math.Pow((poz[0] -  poz2[0]),2) + Math.Pow((poz[1] -  poz2[1]),2);
                var dist3 = Math.Pow((poz[0] -  poz3[0]),2) + Math.Pow((poz[1] -  poz3[1]),2);
                
                var dist = Math.Min(Math.Min(dist1, dist2), dist3);

                m[poz[0] - 1][poz[1] - 1] = dist.ToString();
            }
            
            // se creeaza runda
            var mutare = new Runda
            {
                poreclaJucator = porecla,
                punctaj = punctaj,
                ghicit = ghicit,
                nrRunda = runda,
                pozitieGhicita = pozitie,
                idJoc = game.id
            };

            repoRunde.Save(mutare);
            
            // se actualizeaza nr rundei
            game.rundaCurenta++;
            game.punctaj += punctaj;
            repoJocuri.Update(game);

            if (runda < 3)
            {
                return new
                {
                    status = "playing",
                    mesaj = m
                };
            }
            else
            {
                return new
                {
                    status = "finished",
                    mesaj = m
                };
            }
        }

        return null;
    }

    public async Task<object> FinishGame(int gameId)
    {
        var pozitii = "";
        var game = repoJocuri.FindById(gameId);

        if (game.rundaCurenta > 3)
        {
            var barca = repoBarci.FindById(game.idBarca);

            pozitii += barca.pozitie1 + " ; " + barca.pozitie2 + " ; " + barca.pozitie3;
            
            game.status = "Finished";
            repoJocuri.Update(game);

            return new
            {
                status = "finished",
                pozitii = pozitii,
                punctaj = game.punctaj
            };
        }

        return new
        {
            status = "playing"
        };
    }
}