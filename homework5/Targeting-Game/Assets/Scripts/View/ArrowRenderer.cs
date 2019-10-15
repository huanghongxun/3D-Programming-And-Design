using UnityEngine;
using System.Collections;

public class ArrowRenderer : EntityRenderer
{
    public override void OnSpawn()
    {
        var rigidBody = gameObject.GetComponent<Rigidbody>();
        rigidBody.isKinematic = true;
        rigidBody.useGravity = true;
        rigidBody.centerOfMass = new Vector3(0, 0, 5.7f);
    }

    public override void OnCollect()
    {
        var rigidBody = gameObject.GetComponent<Rigidbody>();
        rigidBody.isKinematic = true;
    }
}
