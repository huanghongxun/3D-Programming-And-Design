using System;
using System.Collections.Generic;

public class SequenceAction : Action
{
    private IList<Action> sequence = new List<Action>();
    public int repeat = 1;
    public int currentAction = 0;
    private bool first = false;

    private void OnAction(object source, EventArgs e)
    {
        currentAction++;
        if (currentAction >= sequence.Count)
        {
            currentAction = 0;
            if (repeat > 0) repeat--;
            if (repeat == 0)
            {
                OnFinished();
                RequestDestroy();
            }
            else
            {
                sequence[currentAction].StartAction();
            }
        }
        else
        {
            sequence[currentAction].StartAction();
        }
    }

    public override void UpdateAction()
    {
        if (sequence.Count == 0) return;
        if (currentAction < sequence.Count)
        {
            if (first)
            {
                sequence[currentAction].StartAction();
            }
            sequence[currentAction].UpdateAction();
        }
    }

    private void OnDestroy()
    {
        foreach (var action in sequence)
        {
            Destroy(action);
        }
    }

    public void AddAction(params Action[] actions)
    {
        foreach (var action in actions)
        {
            action.Finished += OnAction;
            action.allowDestroy = false;
            sequence.Add(action);
        }
    }
}
