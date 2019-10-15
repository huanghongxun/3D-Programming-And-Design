using UnityEngine;
using System.Collections;

public class CCActionManager : IActionManager
{
    public void SendUFO(Game game, Ruler ruler, int round)
    {
        float speed = 0.1f;
        for (int i = 1; i < round; ++i) speed *= 1.1f;

        float actualSpeed = Random.Range(speed, speed * 1.3f);

        UFO ufo = UFO.Factory.Instance.Instantiate(new UFOModel
        {
            score = Random.Range(1, 5),
            game = ruler
        });
        ufo.gameObject.transform.parent = game.transform;
        ufo.gameObject.transform.position = ufo.renderer.initialPosition;
        Vector3 dir = ufo.renderer.initialDirection.normalized * 30;
        dir.y /= 4;
        MoveAction action = ufo.gameObject.AddComponent<MoveAction>();
        action.MovePosition( ufo.renderer.initialDirection + dir);
        action.Duration /= actualSpeed * 10;
        action.StartAction();
    }

    public bool IsUFODead(UFO ufo)
    {
        return !ufo.gameObject.GetComponent<MoveAction>();
    }
}
