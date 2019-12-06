using UnityEngine;
using UnityEditor;

public abstract class SceneController : MonoBehaviour
{
    public abstract void LoadResources();

    public abstract void Restart();

    protected GameObject Instantiate(string name, string path)
    {
        var prefab = Resources.Load<GameObject>(path);
        GameObject obj = Instantiate(prefab);
        obj.name = name;
        return obj;
    }

    protected TModel Instantiate<TModel>(string name, string path)
        where TModel : MonoBehaviour
    {
        var obj = Instantiate(name, path);
        obj.AddComponent<Entity>();
        return obj.AddComponent<TModel>();
    }

    protected TEntity InstantiateByFactory<TFactory, TRenderer, TEntity, TModel>(string name)
        where TFactory : EntityRenderer.EntityRendererFactory<TEntity, TModel, TRenderer>, new()
        where TRenderer : EntityRenderer
        where TModel : MonoBehaviour
        where TEntity : EntityRenderee<TModel, TRenderer>
    {
        var factory = new TFactory();
        return InstantiateByFactory<TFactory, TRenderer, TEntity, TModel>(name, factory);
    }

    protected TEntity InstantiateByFactory<TFactory, TRenderer, TEntity, TModel>(string name, TFactory factory)
        where TFactory : EntityRenderer.EntityRendererFactory<TEntity, TModel, TRenderer>, new()
        where TRenderer : EntityRenderer
        where TModel : MonoBehaviour
        where TEntity : EntityRenderee<TModel, TRenderer>
    {
        var obj = Instantiate(name, factory.GetPath());
        var renderer = obj.AddComponent<TRenderer>();
        var entity = obj.AddComponent<TEntity>();
        var model = obj.AddComponent<TModel>();
        entity.renderer = renderer;
        entity.model = model;
        return entity;
    }
}