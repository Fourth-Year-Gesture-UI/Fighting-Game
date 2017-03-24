using UnityEngine;

class GlobeBarSystem : ScrollBarEssentials
{
    public GlobeBarSystem(Rect sb_dimen, bool vbar, Texture sb_bt, Texture st, float rot)
        : base(sb_dimen, vbar, sb_bt, st, rot)
    {

    }

    public void Initialize()
    {
        max_value = DetermineMaxVal(200);
        current_value = 100;
    }

    public void Update()
    {
        if (current_value <= 0)
            current_value = 0;
        else if (current_value >= max_value)
            current_value = max_value;
    }

    public override void DrawBar()
    {
        Matrix4x4 saved_matrix = GUI.matrix;
        GUIUtility.RotateAroundPivot(texture_rotation, pivotVector);

        if (!VerticleBar)
        {
            GUI.BeginGroup(ScrollBarDimens);

            GUI.BeginGroup(new Rect(0, 0, current_value * (ScrollBarDimens.width / max_value), ScrollBarDimens.height));
            GUI.DrawTexture(new Rect(0, 0, ScrollBarDimens.width, ScrollBarDimens.height), ScrollTexture);
            GUI.EndGroup();

            for (int i = 0; i < ScrollBarDimens.width / ScrollBarBubbleTexture.width; i++)
                GUI.DrawTexture(new Rect(i * ScrollBarBubbleTexture.width, 0, ScrollBarBubbleTexture.width, ScrollBarBubbleTexture.height), ScrollBarBubbleTexture);

            GUI.EndGroup();
        }
        else
        {
            GUI.BeginGroup(ScrollBarDimens);
            
            GUI.BeginGroup(new Rect(0, 0, ScrollBarDimens.width, current_value * (ScrollBarDimens.height / max_value)));
            GUI.DrawTexture(new Rect(0, 0, ScrollBarDimens.width, ScrollBarDimens.height), ScrollTexture);
            GUI.EndGroup();

            for (int i = 0; i < ScrollBarDimens.height / ScrollBarBubbleTexture.height; i++)
                GUI.DrawTexture(new Rect(0, i * ScrollBarBubbleTexture.height, ScrollBarBubbleTexture.width, ScrollBarBubbleTexture.height), ScrollBarBubbleTexture);

            GUI.EndGroup();
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
}
