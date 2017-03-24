using UnityEngine;

class ManaSystem : ScrollBarEssentials
{
    public ManaSystem(Rect sb_dimen, bool vbar, Texture sb_bt, Texture st, float rot)
        : base(sb_dimen, vbar, sb_bt, st, rot)
    {

    }

    public ManaSystem(Rect sb_dimen, Rect sbs_dimen, bool vbar, Texture sb_bt, Texture st, float rot)
        : base(sb_dimen, sbs_dimen, vbar, sb_bt, st, rot)
    {

    }

    public void Initialize()
    {
        max_value = DetermineMaxVal(400);
    }

    public void Update()
    {
        if (current_value <= 0)
            current_value = 0;
        else if (current_value >= max_value)
            current_value = max_value;
    }

    public override void IncrimentBar(int value)
    {
        ProcessValue(value);
    }

    protected override int DetermineMaxVal(int value)
    {
        return base.DetermineMaxVal(value);
    }
}