using UnityEngine;
using System.Collections;

public abstract class SceneController : MonoBehaviour
{
    public virtual void Awake()
    {
        SSDirector.GetInstance().OnSceneWake(this);
    }

    public abstract void LoadResources();

    public abstract void Restart();

}
