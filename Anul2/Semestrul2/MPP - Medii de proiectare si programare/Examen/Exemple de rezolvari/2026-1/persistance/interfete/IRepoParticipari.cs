using model;

namespace persistance;

public interface IRepoParticipari : IRepo<Participare,int>
{
    public Participare? FindByJucator(string jucator);
    public Participare? FindByJoc(int idJoc);
}