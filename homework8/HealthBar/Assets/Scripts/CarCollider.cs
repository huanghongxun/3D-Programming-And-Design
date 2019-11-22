using UnityEngine;

public class CarCollider : EntityHealth
{
    public ParticleSystem smokeLeft;
    public ParticleSystem smokeRight;

    void Awake()
    {
        health = 20;
        maxHealth = 20;
    }

    void OnCollisionEnter(Collision collision)
    {
        health -= gameObject.GetComponent<Rigidbody>().velocity.magnitude;
        health = Mathf.Max(health, 0);
    }

    void UpdateSmoke(ParticleSystem smoke)
    {
        var color = smoke.colorOverLifetime;
        var gradient = new Gradient();
        gradient.SetKeys(
            new[] { new GradientColorKey(Color.black, 0), new GradientColorKey(Color.gray, 0.5f), new GradientColorKey(Color.white, 1) },
            new[] { new GradientAlphaKey(0, 0), new GradientAlphaKey((30 - health) / 30f, 0.5f), new GradientAlphaKey(0, 1) });
        color.color = gradient;
    }

    void Update()
    {
        UpdateSmoke(smokeLeft);
        UpdateSmoke(smokeRight);
    }

    void OnGUI()
    {
        GUI.Label(new Rect(5, 5, 1000, 20), "Health: " + health);
    }
}
