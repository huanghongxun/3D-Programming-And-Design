using UnityEngine;
using System.Collections.Generic;

public class RegionModel : Model
{
    public Ruler game { get; set; }

    public int score { get; set; } = 1;

    public int x { get; set; }
    public int y { get; set; }

    public void OnCollisionWithPlayer(GameObject gameObject)
    {
        game.Trace(x, y);
        game.AddScore(1);
    }
}
