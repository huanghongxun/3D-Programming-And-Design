using UnityEngine;
using UnityEditor;

public enum ActionEventType
{
    Started,
    Completed
}

public interface IActionCallback
{
    void OnAction(Action source, ActionEventType events);
}