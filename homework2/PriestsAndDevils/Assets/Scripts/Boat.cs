using UnityEngine;
using System.Collections.Generic;

public class Boat : Entity
{
    public static readonly Vector3[] departures = { new Vector3(-0.5f, 0.5f, 0), new Vector3(0.5f, 0.5f, 0) };

    private readonly Entity[] onBoat = new Entity[2];
    
    public override void OnAction(Entity entity)
    {
        // if (GetComponent<MoveAction>() || GetComponentInChildren<MoveAction>())
        //     return;

        for (int i = 0; i < onBoat.Length; ++i)
        {
            if (onBoat[i] == entity)
            {
                onBoat[i] = null;
                GetComponentInParent<Coast>()?.GoOnShore(entity);
                return;
            }
        }

        // clicking boat pass to parent entity
        base.OnAction(entity);
    }

    public IList<Entity> GetOnBoat()
    {
        return onBoat;
    }

    public bool TakeBoat(Entity entity)
    {
        for (var i = 0; i < 2; ++i)
        {
            if (onBoat[i]) continue;
            onBoat[i] = entity;
            entity.gameObject.transform.parent = transform;
            entity.gameObject.transform.localPosition = departures[i];
            // entity.gameObject.AddComponent<MoveAction>().MoveLocalPosition(departures[i]);
            return true;
        }

        return false;
    }

    public void StopAt(Coast coast)
    {
        transform.parent = coast.transform;
        gameObject.transform.position = coast.BoatStopPosition;
        // gameObject.AddComponent<MoveAction>().MovePosition(Port.BoatStopPosition);
    }
}
