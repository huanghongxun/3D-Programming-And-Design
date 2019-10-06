using UnityEngine;
using System.Collections;

public class UFORenderer : EntityRenderer
{
    public Vector3 initialPosition = new Vector3(-10, 1, -10);
    public Vector3 initialDirection = new Vector3(5, 8, 5);
    public float speedMultiplierMin = 1, speedMultiplierMax = 1.3f;

    public void SetColor(Color color)
    {
        GetComponent<Renderer>().material.color = color;
    }

    public override void OnSpawn()
    {
        var rigidBody = gameObject.GetComponent<Rigidbody>();
        rigidBody.WakeUp();
        rigidBody.useGravity = true;
    }

    public override void OnCollect()
    {
        var rigidBody = gameObject.GetComponent<Rigidbody>();
        rigidBody.Sleep();
        rigidBody.useGravity = false;
    }
}
