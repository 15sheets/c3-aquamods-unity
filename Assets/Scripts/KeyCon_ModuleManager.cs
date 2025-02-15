using Unity.Properties;
using UnityEngine;

public class KeyCon_ModuleManager : MonoBehaviour
{
    //public bool net;
    public float vspeedROC = 2;
    public float moduleUIPosDist = 1.35f;

    public ModuleInterpreter.ModuleIdentity mod1_init;
    public ModuleInterpreter.ModuleIdentity mod2_init = ModuleInterpreter.ModuleIdentity.NONE;

    public ModuleInterpreter mod1;
    public ModuleInterpreter mod2;

    private SpriteRenderer mod1spr;
    private SpriteRenderer mod2spr;

    public GameObject playerui;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //mod1.net = net;
        //mod2.net = net;

        mod1.vspeedROC = vspeedROC;
        mod2.vspeedROC = vspeedROC;

        mod1.identity = mod1_init;
        mod2.identity = mod2_init;
    }

    // Update is called once per frame
    void Update()
    {
        mod1spr.sprite = ModuleParking.mp.moduleSprites[mod1.identity];
        mod2spr.sprite = ModuleParking.mp.moduleSprites[mod2.identity];
    }

    public void createKeyConModuleUI(int playerid, ControllerSettings.ControllerType contype)
    {
        GameObject ui = Instantiate(playerui, transform);
        mod1spr = ui.transform.GetChild(0).GetComponent<SpriteRenderer>();
        mod2spr = mod1spr.transform.GetChild(0).GetComponent<SpriteRenderer>();
        
        // move ui object to correct position
        if (playerid == 2)
        {
            mod1spr.transform.position -= 2 * Vector3.right * mod1spr.transform.position.x;
        }
        
        // change mod2spr position based on keycon and playerid
        if (contype != ControllerSettings.ControllerType.CON)
        {
            mod2spr.transform.position = mod1spr.transform.position + Vector3.up * moduleUIPosDist;
        } else
        {
            mod1spr.transform.position -= (playerid == 1) ? Vector3.zero : moduleUIPosDist * Vector3.right;
            mod2spr.transform.position += moduleUIPosDist * Vector3.right;
            //mod2spr.transform.position = mod1spr.transform.position + ((playerid == 1) ? 1 : -1) * Vector3.right * moduleUIPosDist;
        }

    }
}
