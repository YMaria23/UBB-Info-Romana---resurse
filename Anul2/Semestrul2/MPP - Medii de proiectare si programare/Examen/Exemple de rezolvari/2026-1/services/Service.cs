using log4net;
using model;
using persistance;

namespace services;

public class Service
{
    private IRepoJucatori repoJucatori;
    private IRepoConfiguratii repoConfiguratii;
    private IRepoMutari repoMutari;
    private IRepoJoc repoJocuri;
    private IRepoParticipari repoParticipari;

    private int n;
    
    private static readonly ILog log = LogManager.GetLogger("service");

    public Service(int n,IRepoJucatori repoJucatori, IRepoConfiguratii repoConfiguratii, IRepoMutari repoMutari,IRepoJoc repoJocuri, IRepoParticipari repoParticipari)
    {
        this.n = n;
        this.repoJucatori = repoJucatori;
        this.repoConfiguratii = repoConfiguratii;
        this.repoMutari = repoMutari;
        this.repoJocuri = repoJocuri;
        this.repoParticipari = repoParticipari;
    }
    
    //LOGIN
    public async Task<object> JoinGame(string player)
    {
        // verifica porecla
        var jucator = repoJucatori.FindByName(player);
        if (jucator == null)
            throw new Exception("Jucatori insuficienti");
        
        // cauta jocul curent
        var games = repoJocuri.FindAll();
        var game = games.FirstOrDefault(g => g.status != "Finished");

        // daca nu am un joc deja inceput
        if (game == null)
        {
            game = new Joc
            {
                n = 2,
                conf = "",
                status = "WaitingConfig",
                rundaCurenta = 1,
                jucatorCurentIndex = 0
            };

            var val = repoJocuri.Save(game);
            if (val == null)
            {
                throw new Exception("Eroare la salvarea jocului");
            }
        }

        // verifica sa nu fie deja logat anterior
        var alreadyJoined = repoParticipari.FindAll()
            .Any(p => p.IdJoc == game.id && p.PoreclaJucator == player);

        if (alreadyJoined)
        {
            throw new Exception("Jucatorul este deja in joc.");
        }

        var count = repoParticipari.FindAll()
            .Count(p => p.IdJoc == game.id);

        // se inregistreaza participarea
        var participation = new Participare
        {
            IdJoc = game.id,
            PoreclaJucator = player,
            EntryOrder = count + 1,
            Pozitie = -1,
            Puncte = 0
        };

        var val1 = repoParticipari.Save(participation);
        if(val1 == null)
            throw  new Exception("Eroare la salvarea jocului");
        
        // 5. Daca a intrat al n-lea jucator, schimba statusul jocului
        string mesaj = "Jucatori insuficienti";
        if (count + 1 >= n)
        {
            game.status = "ReadyToStart";
            repoJocuri.Update(game);
            mesaj = "Jocul poate incepe";
        }

        return new
        {
            gameId = game.id,
            entryOrder = participation.EntryOrder,
            mesaj = mesaj
        };
    }
    
    // CONFIGURATIE
    public async Task<object> AlegeConfig()
    {
        // cauta jocul curent
        var games = repoJocuri.FindAll();
        var game = games.FirstOrDefault(g => g.status != "Finished");
        
        // se aleg 3 configuratii din care jucatorul n/2 poate sa aleaga
        var confs = repoConfiguratii.FindAll()
            .Where(c => c.N == n)
            .OrderBy(c => Guid.NewGuid())
            .Take(3)
            .ToList();

        var configuratii = new List<string>();
        foreach (var conf in confs)
        {
            configuratii.Add(conf.ValPozitii);
        }
        
        var participanti = repoParticipari.FindAll()
            .Where(p => p.IdJoc == game.id)
            .ToList();

        var chosen = participanti[n / 2 - 1].PoreclaJucator;

        return new
        {
            conf = configuratii,
            porecla = chosen
        };
    }

    public async Task<object> ChooseConfig(string config)
    {
        // cauta jocul curent
        var games = repoJocuri.FindAll();
        var game = games.FirstOrDefault(g => g.status != "Finished");
        
        // se actualizeaza configuratia curenta a jocului
        game.conf = config;
        repoJocuri.Update(game);

        return new
        {
            conf = config
        };
    }
    
    
    // REST 1 - FILTRARE JOCURI
    public async Task<object> FilterGames(string porecla)
    {
        // se cauta jocurile la care a participat utilizatorul si a obtinut minim 5 puncte
        var games = repoParticipari.FindAll()
            .Where(p => p.PoreclaJucator == porecla && p.Puncte >= 5)
            .Join(
                repoJocuri.FindAll(),
                p => p.IdJoc,
                j => j.id,
                (p, j) => new { id = j.id, puncte = p.Puncte, pozitia = p.Pozitie, runde = j.n }
            )
            .ToList();

        return games;
    }
    
    
    // REST 2 - ADAUGA CONFIGURATIE NOUA (in db presupun)
    public async Task<object> AddConfig(int n, string valori)
    {
        //se verifica
        var conf = valori.Split(",");
        if(conf.Length != n*2)
            throw new Exception("Valori incorecte!");
            
        
        // se creeaza configuratia
        var config = new Configuratie(1, n, valori);
        var raspuns = repoConfiguratii.Save(config);
        if (raspuns == null)
            throw new Exception("eroare la salvare");
        
        return config;
    }
    
    // MUTARI
    
    // se trimite randul carui jucator este
    public async Task<object> StartMoves()
    {
        // se obtine jocul curent
        var games = repoJocuri.FindAll();
        var game = games.FirstOrDefault(g => g.status != "Finished");

        
        var jucator = game.jucatorCurentIndex;

        if (jucator >= game.n)
        {
            // inseamna ca incepe o noua runda
            jucator = 0;
            game.jucatorCurentIndex = 0;

            game.rundaCurenta = game.rundaCurenta + 1;
            repoJocuri.Update(game);
        }

        return new
        {
            nrJucatori = game.n,
            curent = jucator
        };
    }

    public async Task<object> GenerateNumber(int number, string porecla)
    {
        // se determina jocul actual
        var games = repoJocuri.FindAll();
        var game = games.FirstOrDefault(g => g.status != "Finished");

        game.jucatorCurentIndex = game.jucatorCurentIndex + 1;
        game.status = "Playing";
        repoJocuri.Update(game);
        
        
        // se determina configuratia
        var config = game.conf.Split(",");
        
        // se determina noua pozitie a jucatorului
        var participare = repoParticipari.FindAll()
            .FirstOrDefault(p => p.IdJoc == game.id && p.PoreclaJucator == porecla);

        var pozitie = participare.Pozitie;
        var pozitieNoua = pozitie + number;
        if (pozitieNoua >= 2*n)
        {
            pozitieNoua %= 2 * n;
        }
        
        if (pozitieNoua < 0)
            pozitieNoua += config.Length;
        
        participare.Pozitie = pozitieNoua;
        repoParticipari.Update(participare);
        
        // se calculeaza punctajul obtinut in functie de pozitia aleasa
        // se verifica daca mai e cineva pe pozitia respectiva
        
        var verif = repoParticipari.FindAll()
            .Where(p => p.IdJoc == game.id && p.Pozitie == pozitieNoua && p.PoreclaJucator != porecla)
            .ToList();

        var punctaj = 0;
        var p = int.Parse(config[pozitieNoua]);
        if (verif.Count == 0)
        {
            punctaj = p;
        }
        else
        {
            punctaj = - p/ 2 * verif.Count;
            // se da punctaj restului de jucatori

            foreach (var j in verif)
            {
                j.Puncte += p / 2;
                repoParticipari.Update(j);
            }
        }
        
        // se creeaza si mutarea
        var punctajTotal = participare.Puncte + punctaj;
        participare.Puncte += punctaj;
        repoParticipari.Update(participare);
        
        var mutare = new Mutare
        {
            idJoc = game.id,
            poreclaJucator = porecla,
            pozFinal = pozitieNoua,
            pozInceput = pozitie,
            puncteMuntare = punctaj,
            puncteTotal = punctajTotal,
            runda = game.rundaCurenta
        };
        
        repoMutari.Save(mutare);
        
        return new
        {
            porecla = porecla,
            pozFinal = pozitieNoua,
            pozInceput = pozitie,
            punctaj = punctaj
        };
    }
    
    // FINALIZARE JOC
    public async Task<object> FinishGame(string porecla)
    {
        // se obtine jocul curent
        var games = repoJocuri.FindAll();
        var game = games.OrderByDescending(g => g.id).FirstOrDefault();
        
        var totalMutari = repoMutari.FindAll().Count(m => m.idJoc == game.id);

        if (totalMutari >= n*n)
        {
            // inseamna ca s-a terminat jocul
            // se vor afisa: nr de puncte al jucatorului, porecla castigatorului + punctaj, rundela castigate de jucatorul curent
            var participare = repoParticipari.FindAll()
                .FirstOrDefault(p => p.PoreclaJucator == porecla && p.IdJoc == game.id);
            
            var punctajTotal = participare.Puncte;
            
            var castigator = repoParticipari.FindAll()
                .Where(p => p.IdJoc == game.id)
                .OrderByDescending(p => p.Puncte)
                .FirstOrDefault();

            var poreclaCastigator = castigator.PoreclaJucator;
            var punctajCastig = castigator.Puncte;
            var mutari = "";
            
            // se determina si mutarile cele mai bune
            var rundacastigata = repoMutari.FindAll()
                .Where(m => m.idJoc == game.id)
                .GroupBy(m => m.runda)
                .Where(g => g.Where(m => m.poreclaJucator == porecla)
                    .Sum(m => m.puncteMuntare) == g.Max(m => m.puncteMuntare))
                .SelectMany(g => g.Where(m => m.poreclaJucator == porecla))
                .ToList();
            
            mutari = string.Join("\n", rundacastigata
                        .Select(m => $"Runda: {m.runda}, Punctaj: {m.puncteMuntare}"));

            game.status = "Finished";
            repoJocuri.Update(game);
            
            log.Info($"FinishGame: rundaCurenta={game.rundaCurenta}, jucatorCurentIndex={game.jucatorCurentIndex}, n={n}");
            
            return new
            {
                punctajEu = punctajTotal,
                poreclaCastigator = poreclaCastigator,
                punctajCastig = punctajCastig,
                mutariCastig = mutari
            };
        }

        return null;
    }
}