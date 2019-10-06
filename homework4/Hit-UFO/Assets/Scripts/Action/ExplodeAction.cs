using UnityEngine;
using System.Collections;
using UnityStandardAssets.Effects;

public class ExplodeAction
{
    public static void StartExplodeAction(Vector3 position)
    {
        var explosion = EntityRendererFactory.Instance.CreateGameObject<EntityRenderer>("Prefabs/Explosion");
        explosion.GetComponent<ExplosionPhysicsForce>().explosionForce = 1;
        explosion.GetComponent<ParticleSystemMultiplier>().multiplier = 0.1f;
        explosion.GetComponent<Light>().range = 1f;
        explosion.transform.position = position;
    }
}
