using UnityEngine;

public class EntityRenderer : MonoBehaviour
{
    public string PrefabPath { get; set; }

    public virtual void OnCollect() { }

    public virtual void OnSpawn() { }
}
