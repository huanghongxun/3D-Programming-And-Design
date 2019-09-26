using UnityEngine;
using System.Collections.Generic;

public class CoastModel : MonoBehaviour
{
    public readonly Vector3[] positions = { new Vector3(-1.5f, 1.25f, -0.25f), new Vector3(-1.5f, 1.25f, 0.25f), new Vector3(0, 1.25f, -0.25f), new Vector3(0, 1.25f, 0.25f), new Vector3(1.5f, 1.25f, -0.25f), new Vector3(1.5f, 1.25f, 0.25f), };

    public Vector3 BoatStopPosition { get; set; }

    public Vector3 BoatStopDirection { get; set; }

    private readonly GameObject[] onShore = new GameObject[6];

    public IList<GameObject> GetOnShore()
    {
        return onShore;
    }

    public bool GoOffShore(GameObject obj)
    {
        for (int i = 0; i < onShore.Length; ++i)
        {
            if (onShore[i] != obj) continue;
            if (!(transform.GetComponentInChildren<BoatModel>()?.TakeBoat(obj) ?? false)) continue;
            onShore[i] = null;
            return true;
        }

        return false;
    }

    public bool GoOnShore(GameObject obj, ActionStyle style = ActionStyle.Animation)
    {
        for (var i = 0; i < onShore.Length; ++i)
        {
            if (onShore[i]) continue;
            obj.transform.parent = transform;
            var action = obj.AddComponent<MoveAction>();
            action.Duration = 2;
            action.AccelerationTime = 0.5f;
            action.MoveLocalPosition(positions[i], style);
            action.StartAction();
            onShore[i] = obj;
            return true;
        }

        return false;
    }
}
