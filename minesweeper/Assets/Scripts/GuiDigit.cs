using UnityEngine;

public class GuiDigitPanel : GuiComponent
{
    public readonly DigitScript images;
    private readonly int digits;

    public GuiDigitPanel(DigitScript images, int digits)
    {
        this.images = images;
        this.digits = digits;

        for (int i = 0; i < digits; ++i)
        {
            Vector2 pos = new Vector2(4 + i * 14, 0);
            AddChild(new GuiDigit(this, digits - i - 1)
            {
                position = () => pos,
                size = new Vector2(14, 27)
            });
        }
    }

    public override void OnGUI()
    {
        base.OnGUI();

        SpriteDrawUtility.DrawSprite(images.left, new Rect(GetPosition(), new Vector2(4, 27)), GUI.color);
        SpriteDrawUtility.DrawSprite(images.right, new Rect(GetPosition() + new Vector2(4 + digits * 14, 0), new Vector2(3, 27)), GUI.color);
    }

    public int number { get; set; }
}

/// <summary>
/// 表示一个显示数字的页面
/// </summary>
public class GuiDigit : GuiComponent
{
    private GuiDigitPanel panel;
    private int digit;

    public GuiDigit(GuiDigitPanel panel, int digit)
    {
        this.panel = panel;
        this.digit = digit;
    }

    public override void OnGUI()
    {
        base.OnGUI();

        Sprite sprite = null;
        int num = panel.number;
        for (int i = 0; i < digit; ++i)
            num /= 10;

        switch (num % 10)
        {
            case 0: sprite = panel.images.digit0; break;
            case 1: sprite = panel.images.digit1; break;
            case 2: sprite = panel.images.digit2; break;
            case 3: sprite = panel.images.digit3; break;
            case 4: sprite = panel.images.digit4; break;
            case 5: sprite = panel.images.digit5; break;
            case 6: sprite = panel.images.digit6; break;
            case 7: sprite = panel.images.digit7; break;
            case 8: sprite = panel.images.digit8; break;
            case 9: sprite = panel.images.digit9; break;
        }

        SpriteDrawUtility.DrawSprite(sprite, new Rect(GetPosition(), size), GUI.color);
    }
}
