using UnityEngine;

class ExampleGUIAspectsController : MonoBehaviour
{
    private ExperienceSystem exp_bar;
    private int last_level = 1;

    private HealthSystem health_bar;
    private ManaSystem mana_bar;

    private GlobeBarSystem globe_test_bar;

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

    public Rect GlobeBarDimens;
    public bool VerticleGlobeBar;
    public Texture GlobeBubbleTexture;
    public Texture GlobeTexture;
    public float GlobeBubbleTextureRotation;

    public void Start()
    {
        health_bar = new HealthSystem(HealthBarDimens, VerticleHealthBar, HealthBubbleTexture, HealthTexture, HealthBubbleTextureRotation);
        exp_bar = new ExperienceSystem(ExpBarDimens, VerticleExpBar, ExpBubbleTexture, ExperienceTexture, ExpTextureRotation);
        mana_bar = new ManaSystem(ManaBarDimens, ManaBarScrollerDimens, VerticleManaBar, ManaBubbleTexture, ManaTexture, ManaBubbleTextureRotation);
        globe_test_bar = new GlobeBarSystem(GlobeBarDimens, VerticleGlobeBar, GlobeBubbleTexture, GlobeTexture, GlobeBubbleTextureRotation);

        exp_bar.Initialize();
        health_bar.Initialize();
        mana_bar.Initialize();
        globe_test_bar.Initialize();
    }

    public void OnGUI()
    {
        health_bar.DrawBar();

        exp_bar.DrawBar();

        mana_bar.DrawBar();

        globe_test_bar.DrawBar();
        
        // These are the example Incriment and Deincriment buttons, mainly for demonstration purposes only
        if (GUI.Button(new Rect(health_bar.getScrollBarRect().x + (health_bar.getScrollBarRect().width / 2) - (128/2), health_bar.getScrollBarRect().y + (health_bar.getScrollBarRect().height / 2) - 30, 128, 20), "Increase Health"))
        {
            health_bar.IncrimentBar(Random.Range(1, 6));
        }
        else if (GUI.Button(new Rect(health_bar.getScrollBarRect().x + (health_bar.getScrollBarRect().width / 2) - (128 / 2), health_bar.getScrollBarRect().y + (health_bar.getScrollBarRect().height / 2) + (20/2), 128, 20), "Decrease Health"))
        {
            health_bar.IncrimentBar(Random.Range(-6, -1));
        }

        if (GUI.Button(new Rect(mana_bar.getScrollBarRect().x + (mana_bar.getScrollBarRect().width / 2) - (128 / 2), mana_bar.getScrollBarRect().y + (mana_bar.getScrollBarRect().height / 2) - 50, 128, 20), "Increase Mana"))
        {
            mana_bar.IncrimentBar(Random.Range(1, 12));
        }
        else if (GUI.Button(new Rect(mana_bar.getScrollBarRect().x + (mana_bar.getScrollBarRect().width / 2) - (128 / 2), mana_bar.getScrollBarRect().y + (mana_bar.getScrollBarRect().height / 2) + 30, 128, 20), "Decrease Mana"))
        {
            mana_bar.IncrimentBar(Random.Range(-12, -1));
        }

        if (GUI.Button(new Rect(exp_bar.getScrollBarRect().x + (exp_bar.getScrollBarRect().width / 2) - (132 / 2), exp_bar.getScrollBarRect().y + (exp_bar.getScrollBarRect().height / 2) - 50, 132, 20), "Increase Experience"))
        {
            exp_bar.IncrimentBar(Random.Range(1, 12));
        }
        else if (GUI.Button(new Rect(exp_bar.getScrollBarRect().x + (exp_bar.getScrollBarRect().width / 2) - (140 / 2), exp_bar.getScrollBarRect().y + (exp_bar.getScrollBarRect().height / 2) + 30, 140, 20), "Decrease Experience"))
        {
            exp_bar.IncrimentBar(Random.Range(-12, -1));
        }

        if (GUI.Button(new Rect(globe_test_bar.getScrollBarRect().x + (globe_test_bar.getScrollBarRect().width / 2) - (140 / 2), globe_test_bar.getScrollBarRect().y + (globe_test_bar.getScrollBarRect().height / 2) - 85, 140, 20), "Increase Globe Value"))
        {
            globe_test_bar.IncrimentBar(Random.Range(1, 12));
        }
        else if (GUI.Button(new Rect(globe_test_bar.getScrollBarRect().x + (globe_test_bar.getScrollBarRect().width / 2) - (145 / 2), globe_test_bar.getScrollBarRect().y + (globe_test_bar.getScrollBarRect().height / 2) + 70, 145, 20), "Decrease Globe Value"))
        {
            globe_test_bar.IncrimentBar(Random.Range(-12, -1));
        }
        // End of example buttons
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

        globe_test_bar.Update();
    }
}