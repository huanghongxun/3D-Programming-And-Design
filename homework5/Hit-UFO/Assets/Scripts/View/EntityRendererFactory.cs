using UnityEngine;
using System.Collections.Generic;

public class EntityRendererFactory : Singleton<EntityRendererFactory>
{
    private Dictionary<string, GameObject> prefabs = new Dictionary<string, GameObject>();
    private Dictionary<string, Queue<GameObject>> objects = new Dictionary<string, Queue<GameObject>>();
    
    public TRenderer CreateGameObject<TRenderer>(string path)
        where TRenderer : EntityRenderer
    {
        GameObject obj;
        if (objects.TryGetValue(path, out Queue<GameObject> q) && q.Count > 0)
        {
            obj = q.Dequeue();
        }
        else
        {
            GameObject prefab;
            if (!prefabs.TryGetValue(path, out prefab))
                prefabs.Add(path, prefab = Resources.Load<GameObject>(path));
            obj = Instantiate(prefab);
        }
        obj.SetActive(true);

        EntityRenderer entityRenderer = obj.GetComponent<EntityRenderer>();
        TRenderer renderer;
        if (entityRenderer is TRenderer tRenderer)
        {
            renderer = tRenderer;
        }
        else
        {
            Destroy(entityRenderer);
            renderer = obj.AddComponent<TRenderer>();
        }
        renderer.OnSpawn();
        renderer.PrefabPath = path;
        return renderer;
    }

    public void Collect(GameObject obj)
    {
        EntityRenderer renderer = obj.GetComponent<EntityRenderer>();
        if (!renderer)
        {
            Destroy(obj);
            return;
        }
        obj.transform.parent = null;
        renderer.OnCollect();
        obj.SetActive(false);
        if (!objects.ContainsKey(renderer.PrefabPath))
            objects.Add(renderer.PrefabPath, new Queue<GameObject>());
        objects[renderer.PrefabPath].Enqueue(obj);
    }
}
