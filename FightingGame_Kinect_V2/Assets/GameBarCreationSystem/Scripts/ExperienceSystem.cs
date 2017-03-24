using UnityEngine;

class ExperienceSystem : ScrollBarEssentials
{
    private int level = 1;

    public ExperienceSystem(Rect sb_dimen, bool vbar, Texture sb_bt, Texture st, float rot) : base(sb_dimen, vbar, sb_bt, st, rot)
    {
        
    }

    // Use this for initialization
    public void Initialize()
    {
        max_value = DetermineMaxVal(level);
    }

    public void OnGUI()
    {
        this.DrawBar();
    }

    // Update is called once per frame
    public void Update()
    {
        if (this.getCurrentValue() < 0)
        {
            if (level > 1)
            {
                max_value = DetermineMaxVal(level - 1);

                current_value = max_value + current_value;

                level--;
            }
            else
            {
                current_value = 0;
            }
        }
        else if (this.getCurrentValue() >= this.getMaxValue(level))
        {
            level++;

            current_value = current_value - max_value;

            max_value = DetermineMaxVal(level);
        }
    }

    public override void IncrimentBar(int value)
    {
        ProcessValue(value);
    }

    protected override int DetermineMaxVal(int level)
    {
        // edit this formula to anything you wish for your specific needs
        // (Best to use a graphing calculator) to determine realistic max_exp values for your game
        // This is a good example of a formula to determine the needed experience points to gain a new level for each level

        return (int)(100*level + (level*Mathf.Exp(Mathf.Pow(level, .333f))) * Mathf.Log(Mathf.Pow(level, .333f)));
    }

    public int getLevel()
    {
        int temp_value = level;

        return temp_value;
    }
}