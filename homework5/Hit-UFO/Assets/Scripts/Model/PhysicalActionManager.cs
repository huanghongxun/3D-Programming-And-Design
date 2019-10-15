using UnityEngine;

public class PhysicalActionManager : IActionManager
{
    public void SendUFO(Game game, Ruler ruler, int round)
    {
        UFO ufo = UFO.Factory.Instance.Instantiate(new UFOModel
        {
            score = Random.Range(1, 5),
            game = ruler
        });
        ufo.gameObject.transform.parent = game.transform;

        var rigidBody = ufo.gameObject.GetComponent<Rigidbody>();
        rigidBody.WakeUp();
        rigidBody.useGravity = true;
        rigidBody.velocity = Vector3.zero;
        rigidBody.angularVelocity = Vector3.zero;
        rigidBody.rotation = Quaternion.Euler(Vector3.zero);

        float speed = 0.1f;
        for (int i = 1; i < round; ++i) speed *= 1.1f;

        float actualSpeed = Random.Range(speed, speed * 1.3f);
        ufo.Send(actualSpeed);
    }

    public bool IsUFODead(UFO ufo)
    {
        return ufo.gameObject.transform.position.y < 0;
    }
}
