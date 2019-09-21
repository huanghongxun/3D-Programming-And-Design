using UnityEngine;
using UnityEditor;

public class GuiIngame : MonoBehaviour
{
    public GameState state = GameState.East;

    // Use this for initialization
    void Start()
    {
    }

    void OnGUI()
    {
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
        GUI.Label(new Rect(Screen.width / 2 - 50, Screen.height / 2 - 300, 100, 50), "Priests And Devils", textStyle);
        GUI.Label(new Rect(Screen.width / 2 - 50, Screen.height / 2 - 250, 100, 50), "Yuhui Huang", textStyle);
        // Show the result.
        if (state == GameState.Win || state == GameState.Lose)
        {
            var text = state == GameState.Win ? "You Win!" : "You Lose!";
            GUI.Label(new Rect(Screen.width / 2 - 50, Screen.height / 2 - 85, 100, 50), text, textStyle);
            // When user presses the Restart button.
            if (GUI.Button(new Rect(Screen.width / 2 - 70, Screen.height / 2, 140, 70), "Restart", buttonStyle))
            {
                SSDirector.GetInstance().CurrentScene?.Restart();
                state = GameState.East;
            }
        }
    }
}
