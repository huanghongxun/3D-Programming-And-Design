using UnityEngine;
using UnityEditor;

public class Character : Entity
{
    public enum State
    {
        OnShore,
        OffShore
    }

    private Coast shore;
    private Boat boat;


}