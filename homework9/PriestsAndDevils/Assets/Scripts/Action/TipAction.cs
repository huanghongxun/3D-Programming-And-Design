using UnityEngine;
using System.Collections;

public class TipAction : MonoBehaviour
{
    public Vector2 Position { get; set; }

    public string Text { get; set; }

    public Color Color { get; set; }

    public float Duration { get; set; }

    private float time = 0;

    void Start()
    {

    }

    // Update is called once per frame
    void OnGUI()
    {
        if (time > Duration)
        {
            Destroy(gameObject);
            return;
        }

        var textStyle = new GUIStyle
        {
            fontSize = 20,
            alignment = TextAnchor.MiddleCenter
        };
        time += Time.deltaTime;
        Color c = Color;
        c.a = (Duration - time) / Duration;
        textStyle.normal.textColor = c;
        GUI.Label(new Rect(Position.x - 100, Position.y - 100 - time / Duration * 50, 200, 200), Text, textStyle);
    }

    public static TipAction StartTipAction()
    {
        GameObject obj = new GameObject();
        return obj.AddComponent<TipAction>();
    }
}