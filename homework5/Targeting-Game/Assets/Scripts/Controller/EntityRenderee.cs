using UnityEngine;

public abstract class EntityRenderee<TModel, TRenderer> : Entity
    where TModel : Model
    where TRenderer : EntityRenderer
{
    public TRenderer renderer;
    public TModel model;

}
