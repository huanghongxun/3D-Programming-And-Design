using UnityEngine;
using UnityEditor;

public class SSDirector
{
    private static volatile SSDirector instance;
    
    public static SSDirector GetInstance()
    {
        return instance ?? (instance = new SSDirector());
    }

    public SceneController CurrentScene { get; private set; }

    public void OnSceneWake(SceneController scene)
    {
        CurrentScene = scene;
        scene.LoadResources();
        scene.gameObject.AddComponent<EntityController>();
    }
}