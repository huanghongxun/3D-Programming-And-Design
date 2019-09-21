using System;
using UnityEngine;

public class MoveAction : MonoBehaviour
{
    public float Duration = 20;
    private Vector3 source, target;
    private float time;
    private bool local;

    public void MovePosition(Vector3 target)
    {
        source = transform.position;
        this.target = target;
        time = 0;
        local = false;
    }

    public void MoveLocalPosition(Vector3 target)
    {
        source = transform.localPosition;
        this.target = target;
        time = 0;
        local = true;
    }

    public event EventHandler OnAction;

    void Update()
    {
        Vector3 newPosition = source + (target - source) *
                              (float) (Math.Cos((time + Time.deltaTime) / Duration * (Math.PI / 2)) -
                                       Math.Cos(time / Duration * (Math.PI / 2)));
        if (time >= Duration) newPosition = target;
        if (local) transform.localPosition = newPosition;
        else transform.position = newPosition;
        time += Time.deltaTime;
        if (time >= Duration)
        {
            OnAction?.Invoke(this, EventArgs.Empty);
            Destroy(this);
        }
    }
}
