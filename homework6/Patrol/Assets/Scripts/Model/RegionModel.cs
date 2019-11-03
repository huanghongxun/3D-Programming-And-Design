using System;
using UnityEngine;
using System.Collections.Generic;

public class RegionModel : Model
{
    public event EventHandler<Vector2Int> Collision;

    public int score { get; set; } = 1;

    public int x { get; set; }
    public int y { get; set; }

    public void OnCollisionWithPlayer(GameObject gameObject)
    {
        Collision?.Invoke(this, new Vector2Int(x, y));
    }
}
