using System;
using UnityEngine;

public abstract class Action : MonoBehaviour
{
    public bool start = false;
    public bool allowDestroy = true;

    public event EventHandler Finished;

    public virtual void StartAction()
    {
        start = true;
    }

    public virtual void StopAction()
    {
        start = false;
    }

    private void Update()
    {
        if (start)
            UpdateAction();
    }

    public abstract void UpdateAction();

    protected void OnFinished()
    {
        Finished?.Invoke(this, EventArgs.Empty);
    }

    protected void RequestDestroy()
    {
        if (allowDestroy)
            Destroy(this);

        StopAction();
    }
}
