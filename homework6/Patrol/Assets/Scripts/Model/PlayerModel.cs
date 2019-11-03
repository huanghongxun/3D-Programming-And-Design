using UnityEngine;
using System.Collections.Generic;

public class PlayerModel : Model
{
    public Ruler game { get; set; }

    public GameObject target { get; set; }

    public float speed { get; set; }

    public Queue<Vector3> points { get; set; }

    public override void Update()
    {

    }

    public void OnCollisionWithEnemy(GameObject gameObject)
    {
        game.Over();
    }
}
