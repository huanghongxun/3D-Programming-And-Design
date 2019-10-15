using UnityEngine;
using System.Collections;

public class UFOModel : Model
{
    public int score { get; set; }

    public Ruler game { get; set; }

    public bool success { get; set; }

    public override void Start()
    {
    }

    public void OnShot()
    {
        success = true;
        game.AddScore(score);
        ExplodeAction.StartExplodeAction(gameObject.GetComponent<Rigidbody>().position);
        EntityRendererFactory.Instance.Collect(gameObject);
    }

    public void Send(Vector3 initialPosition, Vector3 initialDirection, float speed)
    {
        gameObject.transform.position = initialPosition;

        Rigidbody rigidbody = gameObject.GetComponent<Rigidbody>();
        rigidbody.AddForce(initialDirection * speed, ForceMode.Impulse);
    }
}
