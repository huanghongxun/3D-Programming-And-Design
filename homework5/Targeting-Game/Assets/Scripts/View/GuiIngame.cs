using UnityEngine;
using System.Collections;

public class GuiIngame : MonoBehaviour
{
    public Texture strengthTexture;

    public GameState state { get; set; }

    public int score { get; set; }
    public float strength { get; set; }
    public int trial { get; set; }
    public Vector2 wind { get; set; }

    // Use this for initialization
    void Start()
    {

    }

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction * 30, Color.yellow);
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

        GUI.Label(new Rect(20, 25, 100, 50), "Trial: " + trial, titleStyle);
        GUI.Label(new Rect(20, 45, 100, 50), "Score: " + score, titleStyle);
        GUI.Label(new Rect(20, 85, 100, 50), "Wind: x:" + wind.x + ", y: " + wind.y, titleStyle);

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

            DrawStrengthBar(new Rect(80, 80, 100, 20), strength);
        }

        GUI.Label(new Rect(20, 65, 100, 50), "Strength: " + strength * 10, titleStyle);
    }

    private void DrawStrengthBar(Rect rect, float progress)
    {
        GUI.DrawTextureWithTexCoords(new Rect(rect.x, rect.y, rect.width * progress, rect.height), strengthTexture,
            new Rect(0, 0, progress, 1));
    }
}
