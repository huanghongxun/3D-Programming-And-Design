using UnityEngine;
using UnityEditor;

public class Character : MonoBehaviour
{
    public void Explode()
    {
        var prefab = Resources.Load<GameObject>("Prefabs/Explosion");
        GameObject explosion = Instantiate(prefab);
        explosion.transform.position = transform.position;
    }
}