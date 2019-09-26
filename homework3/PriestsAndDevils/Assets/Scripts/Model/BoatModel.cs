using System;
using System.Collections.Generic;
using UnityEngine;

public class BoatModel : MonoBehaviour
{
    public readonly Vector3[] departures = { new Vector3(0, 0.25f, -0.5f), new Vector3(0, 0.25f, 0.5f) };

    private readonly GameObject[] onBoat = new GameObject[2];

    public event EventHandler<StoppedAtEventArgs> StoppedAt;


    public IList<GameObject> GetOnBoat()
    {
        return onBoat;
    }

    public bool GoOnShore(GameObject obj)
    {
        for (int i = 0; i < onBoat.Length; ++i)
        {
            if (onBoat[i] == obj)
            {
                onBoat[i] = null;
                GetComponentInParent<CoastModel>()?.GoOnShore(obj);
                return true;
            }
        }

        return false;
    }

    public bool TakeBoat(GameObject obj)
    {
        for (var i = 0; i < 2; ++i)
        {
            if (onBoat[i]) continue;
            onBoat[i] = obj;
            obj.transform.parent = transform;
            var action = obj.AddComponent<MoveAction>();
            action.Duration = 2;
            action.AccelerationTime = 0.5f;
            action.MoveLocalPosition(departures[i]);
            action.StartAction();
            return true;
        }

        return false;
    }

    public void StopAt(CoastModel coast, ActionStyle style = ActionStyle.Animation)
    {
        transform.parent = coast.transform;
        var action = gameObject.AddComponent<BoatMoveAction>();
        action.Action += delegate
        {
            StoppedAt?.Invoke(this, new StoppedAtEventArgs
            {
                coast = coast
            });
        };
        action.MovePosition(coast.BoatStopPosition, Quaternion.LookRotation(coast.BoatStopDirection), style);
        action.StartAction();
    }

    public class StoppedAtEventArgs : EventArgs
    {
        public CoastModel coast { get; set; }
    }
}
