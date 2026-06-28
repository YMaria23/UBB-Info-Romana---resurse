using model;

namespace persistance;

public interface IRepoJucatori : IRepo<Jucator,int>
{
    public Jucator? FindByPorecla(string porecla);
}