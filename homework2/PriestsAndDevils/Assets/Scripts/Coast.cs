using UnityEngine;
using System.Collections.Generic;

public class Coast : Entity
{
    public static readonly Vector3[] positions = { new Vector3(-1.5f, 1.25f, -0.25f), new Vector3(-1.5f, 1.25f, -0.25f), new Vector3(0, 1.25f, -0.25f), new Vector3(0, 1.25f, 0.25f), new Vector3(1.5f, 1.25f, -0.25f), new Vector3(1.5f, 1.25f, 0.25f),  };

    public Vector3 BoatStopPosition { get; set; }

    private readonly Entity[] onShore = new Entity[6];

    public IList<Entity> GetOnShore()
    {
        return onShore;
    }

    public override void OnAction(Entity entity)
    {
        GoOffShore(entity);
    }

    public void GoOffShore(Entity entity)
    {
        for (int i = 0; i < onShore.Length; ++i)
        {
            if (onShore[i] == entity)
            {
                onShore[i] = null;
                transform.GetComponentInChildren<Boat>()?.TakeBoat(entity);
                break;
            }
        }
    }

    public void GoOnShore(Entity entity)
    {
        for (var i = 0; i < onShore.Length; ++i)
        {
            if (onShore[i]) continue;
            entity.transform.parent = transform;
            entity.transform.localPosition = positions[i];
            onShore[i] = entity;
            break;
        }
    }
}
