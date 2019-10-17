using UnityEngine;
using System.Collections;

public class ArrowModel : Model
{
    public Ruler game { get; set; }

    public bool success { get; set; }

    public float strength { get; set; }

    public override void Start()
    {
    }

    public void Hit(Vector3 point)
    {
        success = true;
        int score = game.AddScore(point);
        if (score > 0)
        {
            game.NextTrial();
            gameObject.GetComponent<Rigidbody>().isKinematic = true;
            gameObject.GetComponent<Collider>().enabled = false;
            gameObject.AddComponent<ShakeAction>().Shake(point, Vector3.up, 3.92f, strength * 10, strength / 20f + 0.5f);
        }
    }

    public void AimAt(Vector3 direction)
    {
        gameObject.transform.rotation = Quaternion.LookRotation(direction);
    }

    public void Shoot(Vector3 direction)
    {
        AimAt(direction);
        Rigidbody body = gameObject.GetComponent<Rigidbody>();
        body.isKinematic = false;
        body.AddForce(direction, ForceMode.Impulse);
        strength = direction.magnitude;
    }

    public void CheckAlive()
    {
        if (gameObject.transform.position.y < -10)
        {
            EntityRendererFactory.Instance.Collect(gameObject);
            game.NextTrial();
        }
    }
}
