public abstract class EntityFactory<TEntity, TModel, TRenderer, TSelf> : Singleton<TSelf>
    where TModel : Model
    where TRenderer : EntityRenderer
    where TEntity : EntityRenderee<TModel, TRenderer>
    where TSelf : EntityFactory<TEntity, TModel, TRenderer, TSelf>
{

    public abstract TRenderer InstantiateImpl(TModel model);

    public TEntity Instantiate(TModel model)
    {
        TRenderer renderer = InstantiateImpl(model);

        foreach (var oldEntity in renderer.gameObject.GetComponents<Entity>())
        {
            Destroy(oldEntity);
        }

        TEntity entity = renderer.gameObject.AddComponent<TEntity>();
        entity.model = model;
        model.gameObject = renderer.gameObject;
        model.renderer = renderer;
        entity.renderer = renderer;
        return entity;
    }
}