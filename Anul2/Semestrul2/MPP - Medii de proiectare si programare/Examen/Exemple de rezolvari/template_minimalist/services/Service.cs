using log4net;

namespace services;

public class Service
{
    // se introduc repo-urile corespunzatoare
    
    
    private static readonly ILog log = LogManager.GetLogger("service");

    // constructorul
    public Service()
    {
    }
    
    // LOGIN
    public async Task<object> JoinGame(string player)
    {
        /*
        // verifica porecla
        var jucator = repoJucatori.FindByName(player);
        if (jucator == null)
            throw new Exception("Jucatori insuficienti");
        
        // DACA AM JOC IN DB
        
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

        
        // VERIFICA SA VADA DACA A FOST LOGAT ANTERIOR
        var alreadyJoined = repoParticipari.FindAll()
            .Any(p => p.IdJoc == game.id && p.PoreclaJucator == player);

        if (alreadyJoined)
        {
            throw new Exception("Jucatorul este deja in joc.");
        }

        var count = repoParticipari.FindAll()
            .Count(p => p.IdJoc == game.id);

        // SALVEAZA PARTICIPAREA LA JOC
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
        
        // IN CAZUL IN CARE SUNT MAI MULTI JUCATORI SI TREBUIE SA SE STRANGA PT A JUCA
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
        */
        return null;
    }
}