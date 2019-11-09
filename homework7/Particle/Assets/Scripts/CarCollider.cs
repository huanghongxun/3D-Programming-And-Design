using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarCollider : MonoBehaviour
{
    public float damage = 0;
    public ParticleSystem smokeLeft;
    public ParticleSystem smokeRight;

    void OnCollisionEnter(Collision collision)
    {
        damage += gameObject.GetComponent<Rigidbody>().velocity.magnitude;
    }

    void UpdateSmoke(ParticleSystem smoke)
    {
        var color = smoke.colorOverLifetime;
        var gradient = new Gradient();
        gradient.SetKeys(
            new[] { new GradientColorKey(Color.black, 0), new GradientColorKey(Color.gray, 0.5f), new GradientColorKey(Color.white, 1) },
            new[] { new GradientAlphaKey(0, 0), new GradientAlphaKey(Math.Min(damage + 10, 30) / 30f, 0.5f), new GradientAlphaKey(0, 1) });
        color.color = gradient;
    }

    void Update()
    {
        UpdateSmoke(smokeLeft);
        UpdateSmoke(smokeRight);
    }

    void OnGUI()
    {
        GUI.Label(new Rect(5, 5, 1000, 20), "Damage: " + damage);
    }
}
