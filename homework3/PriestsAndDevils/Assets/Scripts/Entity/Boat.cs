using UnityEngine;
using System.Collections;

public class Boat : EntityRenderee<BoatModel, BoatRenderer>
{
    public override void OnAction(GameObject gameObject)
    {
        if (GetComponent<MoveAction>() || GetComponentInChildren<MoveAction>())
            return;

        if (model.GoOnShore(gameObject))
            return;

        // clicking boat pass to parent entity
        base.OnAction(gameObject);
    }
}
