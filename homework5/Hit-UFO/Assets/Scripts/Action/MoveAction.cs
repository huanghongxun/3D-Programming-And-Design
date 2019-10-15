﻿using System;
using UnityEngine;

public class MoveAction : Action
{
    public float Duration = 10;
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
        time += Time.deltaTime;
        if (time >= Duration) time = Duration;
        Vector3 rel = (target - source) * (time / Duration);
        Vector3 newPosition = source + rel;
        if (local) transform.localPosition = newPosition;
        else transform.position = newPosition;
        if (time >= Duration)
        {
            Action?.Invoke(this, EventArgs.Empty);
            RequestDestroy();
        }
    }
}
