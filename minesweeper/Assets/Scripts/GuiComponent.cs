using UnityEngine;
using System.Reflection;
using System;
using System.Collections.Generic;

public enum Alignment
{
    TopLeft = 0,
    TopCenter = 1,
    TopRight = 2,
    CenterLeft = 3,
    Center = 4,
    CenterRight = 5,
    BottomLeft = 6,
    BottomCenter = 7,
    BottomRight = 8
}

public class GuiContent
{
    protected MethodInfo method;
    protected MethodInfo methodStyle;
    private Type contentType;

    public GuiContent(object content)
    {
        this.content = content;
    }

    public object content { get; set; }

    public object OnGUI(string methodName, Rect rect, GUIStyle style)
    {
        if (content == null) return null;
        if (contentType != content.GetType())
        {
            method = typeof(GUI).GetMethod(methodName,
                BindingFlags.Public | BindingFlags.Static | BindingFlags.Default,
                Type.DefaultBinder,
                new Type[] { typeof(Rect), contentType = content.GetType() },
                new ParameterModifier[] { new ParameterModifier(2) });
            methodStyle = typeof(GUI).GetMethod(methodName,
                BindingFlags.Public | BindingFlags.Static | BindingFlags.Default,
                Type.DefaultBinder,
                new Type[] { typeof(Rect), contentType = content.GetType(), typeof(GUIStyle) },
                new ParameterModifier[] { new ParameterModifier(3) });
        }
        if (style == null)
            return method.Invoke(null, new object[] { rect, content });
        else
            return methodStyle.Invoke(null, new object[] { rect, content, style });
    }
}

public class MouseEventArgs : EventArgs
{
    public KeyCode code { get; set; }
}

public abstract class GuiComponent
{
    public delegate Vector2 PositionCalc();

    public PositionCalc position { get; set; }

    public Vector2 size { get; set; }

    public Alignment alignment { get; set; }

    public GuiContent content { get; set; }

    public GUIStyle style { get; set; }

    public GuiComponent parent { get; protected set; }

    public bool enabled { get; set; } = true;

    private IList<GuiComponent> children = new List<GuiComponent>();

    public delegate void MouseEventHandler(object sender, MouseEventArgs eventArgs);

    public event EventHandler MouseOver;
    public event MouseEventHandler MousePressed;
    public event MouseEventHandler MouseReleased;
    public event MouseEventHandler MouseClicked;

    private static KeyCode[] mouseCodes = { KeyCode.Mouse0, KeyCode.Mouse1, KeyCode.Mouse2 };
    protected bool[] mousePressed = { false, false, false };

    protected virtual Vector2 GetPosition()
    {
        Vector2 pos = parent.GetPosition();
        if (position != null) pos += position();
        int horizontal = (int)alignment % 3, vertical = (int)alignment / 3;
        pos.x -= size.x * horizontal / 2;
        pos.y -= size.y * vertical / 2;

        return pos;
    }

    public void AddChild(GuiComponent component)
    {
        component.parent = this;
        children.Add(component);
    }

    public bool isMouseOver
    {
        get
        {
            Vector2 mousePosition = new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y);
            return VectorComparator.GreaterEqual(mousePosition, GetPosition()) &&
            VectorComparator.LessEqual(mousePosition, GetPosition() + size);
        }
    }

    public virtual void OnGUI()
    {
        foreach (GuiComponent component in children)
        {
            component.OnGUI();
        }

        Vector2 pos = GetPosition();
        if (isMouseOver)
        {
            MouseOver?.Invoke(this, EventArgs.Empty);

            for (int i = 0; i < 3; ++i)
                if (Input.GetMouseButtonDown(i))
                {
                    mousePressed[i] = true;
                    MousePressed?.Invoke(this, new MouseEventArgs
                    {
                        code = mouseCodes[i]
                    });
                }

            for (int i = 0; i < 3; ++i)
                if (mousePressed[i] && Input.GetMouseButtonUp(i))
                {
                    MouseClicked?.Invoke(this, new MouseEventArgs
                    {
                        code = mouseCodes[i]
                    });
                }
        }

        for (int i = 0; i < 3; ++i)
            if (mousePressed[i] && Input.GetMouseButtonUp(i))
            {
                mousePressed[i] = false;
                MouseReleased?.Invoke(this, new MouseEventArgs
                {
                    code = mouseCodes[i]
                });
            }
    }
}

public class GuiScreen : GuiComponent
{
    protected override Vector2 GetPosition()
    {
        return new Vector2(0, 0);
    }

    public override void OnGUI()
    {
        this.size = new Vector2(Screen.width, Screen.height);

        base.OnGUI();
    }
}

public class GuiButton : GuiComponent
{
    public event EventHandler Click;

    public override void OnGUI()
    {
        base.OnGUI();

        object result = content.OnGUI("Button", new Rect(GetPosition(), size), style);
        if (result != null && (bool)result && enabled)
        {
            Click(this, EventArgs.Empty);
        }
    }
}

public class GuiSpriteButton : GuiComponent
{
    private Sprite normal, over, pressed;
    private GuiLabel label;

    public GuiSpriteButton(Sprite normal, Sprite over, Sprite pressed)
    {
        this.normal = normal;
        this.over = over;
        this.pressed = pressed;

        label = new GuiLabel();
        AddChild(label);
    }

    public override void OnGUI()
    {
        Sprite sprite;
        if (mousePressed[0])
            sprite = pressed;
        else if (isMouseOver)
            sprite = over;
        else
            sprite = normal;

        SpriteDrawUtility.DrawSprite(sprite, new Rect(GetPosition(), size), GUI.color);

        label.size = size;
        label.style = style;
        label.content = content;
        base.OnGUI();
    }
}

public class GuiLabel : GuiComponent
{
    public event EventHandler Click;

    public override void OnGUI()
    {
        content.OnGUI("Label", new Rect(GetPosition(), size), style);
    }
}