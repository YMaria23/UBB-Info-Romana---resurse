namespace persistance;

public interface IRepo<TEntity,TID>
{
    /**
     * @param entity - the entity that will be added in the repository
     * @return - the entity if the save operation was possible
     *         - null if the operation failed
     * @throws exception if the entity is null
     **/
    public TEntity? Save(TEntity entity);

    /**
     * @param entity - the entity that will be updated in the repository
     * @return - the entity if the update operation was possible
     *         - null if the operation failed
     * @throws exception if the entity is null
     **/
    public TEntity? Update(TEntity entity);

    /**
     * @param id - the entity's id that will be deleted
     * @return - the entity if the operation was possible
     *         - null if the operation failed
     * @throws exception if the entity is null
     **/
    public TEntity? Delete(TID id);

    /**
     * @param id - the id of the entity that is searched
     * @return - the entity if it exists in the repository
     *         - null otherwise
     * @throws RuntimeException if the entity is null
     **/
    public TEntity? FindById(TID id);

    /**
     * @return - returns all entities and stores them in a list
     **/
    List<TEntity> FindAll();
}