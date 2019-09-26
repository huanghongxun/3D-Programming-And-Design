using System;
using UnityEngine;
using System.Collections;

public class EntityRenderer : MonoBehaviour
{

    public abstract class EntityRendererFactory<TEntity, TModel, TRenderer>
        where TModel : MonoBehaviour
        where TRenderer : EntityRenderer
        where TEntity : EntityRenderee<TModel, TRenderer>
    {
        public abstract string GetPath();
    }
}
