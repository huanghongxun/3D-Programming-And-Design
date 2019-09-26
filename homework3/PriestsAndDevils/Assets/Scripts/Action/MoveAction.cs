using System;
using UnityEngine;

public class MoveAction : Action
{
    public float Duration = 10;
    public float AccelerationTime = 2.5f;
    private Vector3 source, target;
    private float time;
    private bool local;
    public event EventHandler Action;

    public void MovePosition(Vector3 target, ActionStyle style = ActionStyle.Animation)
    {
        source = transform.position;
        this.target = target;
        time = style == ActionStyle.Animation ? 0 : Duration;
        local = false;
    }

    public void MoveLocalPosition(Vector3 target, ActionStyle style = ActionStyle.Animation)
    {
        source = transform.localPosition;
        this.target = target;
        time = style == ActionStyle.Animation ? 0 : Duration;
        local = true;
    }

    public override void UpdateAction()
    {
        var vmax = 1 / (4 / (float)Math.PI * AccelerationTime + Duration - 2 * AccelerationTime);
        var coe = 2 * AccelerationTime / (float)Math.PI;
        Vector3 rel = (target - source) * vmax;
        time += Time.deltaTime;
        if (time <= Duration)
        {
            if (time < AccelerationTime)
            {
                rel *= coe * (1 - (float)Math.Cos(time / coe));
            }
            else if (time > Duration - AccelerationTime)
            {
                rel *= coe + Duration - 2 * AccelerationTime +
                       coe * (float) Math.Sin((time - Duration + AccelerationTime) / coe);
            }
            else
            {
                rel *= coe + time - AccelerationTime;
            }
        }
        else
        {
            rel = target - source;
        }
        Vector3 newPosition = source + rel;
        if (local) transform.localPosition = newPosition;
        else transform.position = newPosition;
        time += Time.deltaTime;
        if (time >= Duration)
        {
            Action?.Invoke(this, EventArgs.Empty);
            RequestDestroy();
        }
    }
}
