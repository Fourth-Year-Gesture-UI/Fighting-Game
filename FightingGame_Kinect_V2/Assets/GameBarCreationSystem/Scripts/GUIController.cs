using UnityEngine;

class GUIController : MonoBehaviour
{
    private ExperienceSystem exp_bar;
    private int last_level = 1;

    private HealthSystem health_bar;
    private ManaSystem mana_bar;

    public Rect HealthBarDimens;
    public bool VerticleHealthBar;
    public Texture HealthBubbleTexture;
    public Texture HealthTexture;
    public float HealthBubbleTextureRotation;

    public Rect ExpBarDimens;
    public bool VerticleExpBar;    
    public Texture ExpBubbleTexture;
    public Texture ExperienceTexture;
    public float ExpTextureRotation;

    public Rect ManaBarDimens;
    public Rect ManaBarScrollerDimens;
    public bool VerticleManaBar;
    public Texture ManaBubbleTexture;
    public Texture ManaTexture;
    public float ManaBubbleTextureRotation;

    public void Start()
    {
        health_bar = new HealthSystem(HealthBarDimens, VerticleHealthBar, HealthBubbleTexture, HealthTexture, HealthBubbleTextureRotation);
        exp_bar = new ExperienceSystem(ExpBarDimens, VerticleExpBar, ExpBubbleTexture, ExperienceTexture, ExpTextureRotation);
        mana_bar = new ManaSystem(ManaBarDimens, ManaBarScrollerDimens, VerticleManaBar, ManaBubbleTexture, ManaTexture, ManaBubbleTextureRotation);

        exp_bar.Initialize();
        health_bar.Initialize();
        mana_bar.Initialize();
    }

    public void OnGUI()
    {
        health_bar.DrawBar();

        exp_bar.DrawBar();

        mana_bar.DrawBar();
    }

    public void Update()
    {
        exp_bar.Update();

        if (exp_bar.getLevel() - last_level >= 1)
        {
            // level changed: change stuff here
            //Debug.Log("DING! You Are Now Level " + exp_bar.getLevel());

            last_level = exp_bar.getLevel();
        }

        health_bar.Update();

        mana_bar.Update();
    }
}
