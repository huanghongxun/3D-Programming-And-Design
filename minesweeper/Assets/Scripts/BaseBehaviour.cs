using UnityEngine;
using System.Collections.Generic;

public class BaseBehaviour : MonoBehaviour
{
    protected GuiScreen screen;

    protected virtual void OnGUI()
    {
        if (screen != null)
            screen.OnGUI();
    }
}
