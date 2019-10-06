using UnityEngine;

public class UFOModel : Model
{
    public int score { get; set; }

    public Ruler game { get; set; }

    public bool success { get; set; } = false;

    public void OnShot()
    {
        success = true;
        game.AddScore(score);
        ExplodeAction.StartExplodeAction(gameObject.GetComponent<Rigidbody>().position);
        EntityRendererFactory.Instance.Collect(gameObject);
    }

    public Rigidbody Send(Vector3 initialPosition, Vector3 initialDirection, float speed)
    {
        gameObject.transform.position = initialPosition;

        Rigidbody rigidbody = gameObject.GetComponent<Rigidbody>();
        rigidbody.AddForce(initialDirection * speed, ForceMode.Impulse);
        return rigidbody;
    }
}
