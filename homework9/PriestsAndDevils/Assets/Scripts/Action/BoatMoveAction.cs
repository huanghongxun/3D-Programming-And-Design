using UnityEngine;
using System;

public class BoatMoveAction : Action
{

    public float Duration = 10;
    public float AccelerationTime = 2.5f;
    private Vector3 source, target;
    private Quaternion middleRotation, targetRotation;
    private float time;
    private bool local;
    public event EventHandler Action;

    public void MovePosition(Vector3 target, Quaternion targetRotation, ActionStyle style = ActionStyle.Animation)
    {
        source = transform.position;
        this.targetRotation = targetRotation;
        middleRotation = Quaternion.LookRotation(target - source);
        this.target = target;
        time = style == ActionStyle.Animation ? 0 : Duration;
        local = false;
    }

    public void MoveLocalPosition(Vector3 target, Quaternion targetRotation, ActionStyle style = ActionStyle.Animation)
    {
        source = transform.localPosition;
        this.targetRotation = targetRotation;
        middleRotation = Quaternion.LookRotation(target - source);
        this.target = target;
        time = style == ActionStyle.Animation ? 0 : Duration;
        local = true;
    }

    public override void UpdateAction()
    {
        var vmax = 1 / (4 / (float)Math.PI * AccelerationTime + Duration - 2 * AccelerationTime);
        var coe = 2 * AccelerationTime / (float)Math.PI;
        var rel = (target - source) * vmax;
        time += Time.deltaTime;
        if (time <= Duration)
        {
            if (time < AccelerationTime)
            {
                rel *= coe * (1 - (float)Math.Cos(time / coe));
                transform.rotation = Quaternion.Slerp(transform.rotation, middleRotation, time / AccelerationTime);
            }
            else if (time > Duration - AccelerationTime)
            {
                rel *= coe + Duration - 2 * AccelerationTime +
                       coe * (float)Math.Sin((time - Duration + AccelerationTime) / coe);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, (time - (Duration - AccelerationTime)) / AccelerationTime);
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
        var newPosition = source + rel;
        if (local) transform.localPosition = newPosition;
        else transform.position = newPosition;
        time += Time.deltaTime;
        if (time >= Duration)
        {
            Action?.Invoke(this, EventArgs.Empty);
            Destroy(this);
        }
    }
}
