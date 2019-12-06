using System;
using UnityEngine;
using UnityEditor;

public class GuiIngame : MonoBehaviour
{
    public BoatState state = BoatState.East;

    public event EventHandler<EventArgs> Next;

    void OnGUI()
    {
        var titleStyle = new GUIStyle
        {
            fontSize = 15,
            alignment = TextAnchor.MiddleCenter
        };
        var textStyle = new GUIStyle()
        {
            fontSize = 40,
            alignment = TextAnchor.MiddleCenter
        };
        var buttonStyle = new GUIStyle("button")
        {
            fontSize = 30
        };
        // Show the title and the author.
        GUI.Label(new Rect(Screen.width / 2.0f - 50, Screen.height - 60, 100, 50), "Priests And Devils", titleStyle);
        GUI.Label(new Rect(Screen.width / 2.0f - 50, Screen.height - 40, 100, 50), "Yuhui Huang", titleStyle);
        // Show the result.
        if (state == BoatState.Win || state == BoatState.Lose)
        {
            var text = state == BoatState.Win ? "You Win!" : "You Lose!";
            GUI.Label(new Rect(Screen.width / 2 - 50, Screen.height / 2 - 85, 100, 50), text, textStyle);
            // When user presses the Restart button.
            if (GUI.Button(new Rect(Screen.width / 2 - 70, Screen.height / 2, 140, 70), "Restart", buttonStyle))
            {
                SSDirector.GetInstance().CurrentScene?.Restart();
                state = BoatState.East;
            }
        }
        else
        {
            if (GUI.Button(new Rect(Screen.width - 100, Screen.height - 50, 50, 30), "Next"))
            {
                Next?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    public void ShowWarning(string message)
    {
        TipAction action = gameObject.AddComponent<TipAction>();
        action.Color = Color.red;
        action.Duration = 3;
        action.Text = message;
        action.Position = new Vector2(Screen.width / 2, Screen.height / 2);
    }
}
