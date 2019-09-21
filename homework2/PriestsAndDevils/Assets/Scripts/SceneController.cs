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

    protected T Instantiate<T>(string name, string path)
        where T : Component
    {
        GameObject obj = Instantiate(name, path);
        return obj.AddComponent<T>();
    }
}