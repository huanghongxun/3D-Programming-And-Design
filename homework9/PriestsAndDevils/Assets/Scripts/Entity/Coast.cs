using UnityEngine;
using System.Collections;

public class Coast : EntityRenderee<CoastModel, CoastRenderer>
{
    public override void OnAction(GameObject obj)
    {
        model.GoOffShore(obj);
    }
}
