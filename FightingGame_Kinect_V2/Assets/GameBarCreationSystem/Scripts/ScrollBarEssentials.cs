using UnityEngine;

class ScrollBarEssentials
{
    protected GUIStyle style = new GUIStyle();
    protected Vector2 string_size;

    protected Vector2 pivotVector = Vector2.zero;
    protected bool MouseInRect = false;    

    protected int current_value = 0;
    protected int max_value = 0;

    protected Rect ScrollBarDimens = new Rect();
    protected Rect ScrollBarTextureDimens;
    protected bool VerticleBar = false;
    protected float texture_rotation = 0;
    protected Texture ScrollBarBubbleTexture;
    protected Texture ScrollTexture;

    public ScrollBarEssentials(Rect sb_dimen, bool vbar, Texture sb_bt, Texture st, float rot)
    {
        ScrollBarDimens         = sb_dimen;
        VerticleBar             = vbar;
        ScrollBarBubbleTexture  = sb_bt;
        ScrollTexture           = st;
        texture_rotation        = rot;

        pivotVector.x = ScrollBarDimens.x + (ScrollBarDimens.width / 2);
        pivotVector.y = ScrollBarDimens.y + (ScrollBarDimens.height / 2);

        style.fontStyle = FontStyle.Bold;
        style.normal.textColor = Color.white;
    }

    public ScrollBarEssentials(Rect sb_dimen, Rect sbv_dimen, bool vbar, Texture sb_bt, Texture st, float rot)
    {
        ScrollBarDimens         = sb_dimen;
        ScrollBarTextureDimens  = sbv_dimen;
        VerticleBar             = vbar;
        ScrollBarBubbleTexture  = sb_bt;
        ScrollTexture           = st;
        texture_rotation        = rot;

        pivotVector.x = ScrollBarDimens.x + (ScrollBarDimens.width / 2);
        pivotVector.y = ScrollBarDimens.y + (ScrollBarDimens.height / 2);

        style.fontStyle = FontStyle.Bold;
        style.normal.textColor = Color.white;
    }

    protected virtual int DetermineMaxVal(int value)
    {
        // override this formula to anything you wish for your specific ScrollBar needs
        // (Best to use a graphing calculator to determine realistic max_values for your game)

        return value;
    }

    protected void ProcessValue(int value)
    {
        current_value += value;
    }

    public virtual void DrawBar()
    {
        Matrix4x4 saved_matrix = GUI.matrix;
        GUIUtility.RotateAroundPivot(texture_rotation, pivotVector);

        if (!VerticleBar)
        {
            if (ScrollBarTextureDimens.width != 0 && ScrollBarTextureDimens.height != 0)
                GUI.DrawTexture(new Rect(ScrollBarDimens.x + ScrollBarTextureDimens.x, ScrollBarDimens.y + ScrollBarTextureDimens.y, current_value * (ScrollBarTextureDimens.width / max_value), ScrollBarTextureDimens.height), ScrollTexture);
            else
                GUI.DrawTexture(new Rect(ScrollBarDimens.x, ScrollBarDimens.y, current_value * (ScrollBarDimens.width / max_value), ScrollBarBubbleTexture.height), ScrollTexture);

            for (int i = 0; i < ScrollBarDimens.width / ScrollBarBubbleTexture.width; i++)
                GUI.DrawTexture(new Rect(ScrollBarDimens.x + i * ScrollBarBubbleTexture.width, ScrollBarDimens.y, ScrollBarBubbleTexture.width, ScrollBarBubbleTexture.height), ScrollBarBubbleTexture);
        }
        else
        {
            if (ScrollBarTextureDimens.width != 0 && ScrollBarTextureDimens.height != 0)
                GUI.DrawTexture(new Rect(ScrollBarDimens.x + ScrollBarTextureDimens.x, ScrollBarDimens.y + ScrollBarTextureDimens.y + ScrollBarTextureDimens.height, ScrollBarTextureDimens.width, -current_value * (ScrollBarTextureDimens.height / max_value)), ScrollTexture);
            else
                GUI.DrawTexture(new Rect(ScrollBarDimens.x, ScrollBarDimens.y + ScrollBarDimens.height, ScrollBarBubbleTexture.width, -current_value * (ScrollBarDimens.height / max_value)), ScrollTexture);

            for (int i = 0; i < ScrollBarDimens.height / ScrollBarBubbleTexture.height; i++)
                GUI.DrawTexture(new Rect(ScrollBarDimens.x, ScrollBarDimens.y + i * ScrollBarBubbleTexture.height, ScrollBarBubbleTexture.width, ScrollBarBubbleTexture.height), ScrollBarBubbleTexture);
        }

        if (ScrollBarDimens.Contains(Event.current.mousePosition))
            MouseInRect = true;
        else
            MouseInRect = false;

        if (MouseInRect)
        {
            GUIUtility.RotateAroundPivot(-texture_rotation, pivotVector);
            string_size = style.CalcSize(new GUIContent(current_value + " / " + max_value));
            GUI.Label(new Rect(ScrollBarDimens.x + (ScrollBarDimens.width / 2) - (string_size.x / 2), ScrollBarDimens.y + (ScrollBarDimens.height / 2) - (string_size.y / 2), string_size.x, string_size.y + (string_size.y / 2)), current_value + " / " + max_value, style);
        }

        GUI.matrix = saved_matrix;
    }

    public virtual void IncrimentBar(int value)
    {
        ProcessValue(value);
    }

    public int getCurrentValue()
    {
        int temp_current = current_value;

        return temp_current;
    }

    public int getMaxValue(int value)
    {
        return DetermineMaxVal(value);
    }

    public Rect getScrollBarRect()
    {
        return ScrollBarDimens;
    }

    public Rect getScrollBarTextureDimens()
    {
        return ScrollBarTextureDimens;
    }
}