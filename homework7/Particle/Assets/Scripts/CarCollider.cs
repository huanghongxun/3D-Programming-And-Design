using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarCollider : MonoBehaviour
{
    public float damage = 0;
    public ParticleSystem smoke;

    void OnCollisionEnter(Collision collision)
    {
        damage += gameObject.GetComponent<Rigidbody>().velocity.magnitude;
    }

    void Update()
    {
        if (damage > 10)
        {
            var color = smoke.colorOverLifetime;
            var gradient = new Gradient();
            gradient.SetKeys(
                new GradientColorKey[] { new GradientColorKey(Color.black, 0), new GradientColorKey(),  new GradientColorKey(Color.white, 1),  },
                new GradientAlphaKey[] {  });
            color.color = gradient;
        }
        else if (damage > 20)
        {

        }
    }

    void OnGUI()
    {
        GUI.Label(new Rect(5, 5, 100, 20), "Damage: " + damage);
    }
}
