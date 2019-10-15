using UnityEngine;
using System.Collections;

public class GuiIngame : MonoBehaviour
{
    public GameState state { get; set; }

    public int score { get; set; }
    public int round { get; set; }
    public int trial { get; set; }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void OnGUI()
    {
        var titleStyle = new GUIStyle
        {
            fontSize = 15,
            alignment = TextAnchor.MiddleLeft
        };
        var textStyle = new GUIStyle
        {
            fontSize = 40,
            alignment = TextAnchor.MiddleCenter
        };
        var buttonStyle = new GUIStyle("button")
        {
            fontSize = 30
        };

        GUI.Label(new Rect(20, 5, 100, 50), "Round: " + round, titleStyle);
        GUI.Label(new Rect(20, 25, 100, 50), "Trial: " + trial, titleStyle);
        GUI.Label(new Rect(20, 45, 100, 50), "Score: " + score, titleStyle);

        if (state == GameState.Win || state == GameState.Lose)
        {
            GetComponent<EntityController>().enabled = false;

            var text = state == GameState.Win ? "You Win!" : "You Lose!";
            GUI.Label(new Rect(Screen.width / 2f - 50, Screen.height / 2f - 85, 100, 50), text, textStyle);
            // When user presses the Restart button.
            if (GUI.Button(new Rect(Screen.width / 2f - 70, Screen.height / 2f, 140, 70), "Restart", buttonStyle))
            {
                SSDirector.GetInstance().CurrentScene?.Restart();
            }
        }
        else
        {
            GetComponent<EntityController>().enabled = true;
        }
    }
}
