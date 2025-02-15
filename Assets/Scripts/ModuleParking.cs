using System.Collections.Generic;
using UnityEngine;

public class ModuleParking : MonoBehaviour
{
    public static ModuleParking mp { get; private set; }

    public ModuleInterpreter.ModuleIdentity slot1;
    public ModuleInterpreter.ModuleIdentity slot2;
    public ModuleInterpreter.ModuleIdentity slot3;
    public ModuleInterpreter.ModuleIdentity slot4;

    public SpriteRenderer[] slotspr;
    public SpriteRenderer[] p1labels;
    public SpriteRenderer[] p2labels;

    public Sprite shieldsprite;
    public Sprite aimsprite;
    public Sprite batterysprite;
    public Sprite shootsprite;
    public Sprite steersprite;
    public Sprite speedsprite;
    public Sprite nonesprite;

    public Sprite[] LKeyLabels;
    public Sprite[] RKeyLabels;
    public Sprite[] ConLabels; 

    public Dictionary<ModuleInterpreter.ModuleIdentity, Sprite> moduleSprites;


    private void Awake()
    {
        if (mp != null && mp != this)
        {
            Destroy(this);
        }
        else
        {
            mp = this;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // steer and aim will be distributed to players

        /*
        slot1 = ModuleInterpreter.ModuleIdentity.SPEED;
        slot2 = ModuleInterpreter.ModuleIdentity.SHOOT;
        slot3 = ModuleInterpreter.ModuleIdentity.SHIELD;
        slot4 = ModuleInterpreter.ModuleIdentity.BATTERY;
        */

        // set up module sprite dict
        moduleSprites = new Dictionary<ModuleInterpreter.ModuleIdentity, Sprite>();
        moduleSprites.Add(ModuleInterpreter.ModuleIdentity.SHIELD, shieldsprite);
        moduleSprites.Add(ModuleInterpreter.ModuleIdentity.AIM, aimsprite);
        moduleSprites.Add(ModuleInterpreter.ModuleIdentity.BATTERY, batterysprite);
        moduleSprites.Add(ModuleInterpreter.ModuleIdentity.SHOOT, shootsprite);
        moduleSprites.Add(ModuleInterpreter.ModuleIdentity.STEER, steersprite);
        moduleSprites.Add(ModuleInterpreter.ModuleIdentity.SPEED, speedsprite);
        moduleSprites.Add(ModuleInterpreter.ModuleIdentity.NONE, nonesprite);

        // initialize slot sprites
        slotspr[0].sprite = moduleSprites[slot1];
        slotspr[1].sprite = moduleSprites[slot2];
        slotspr[2].sprite = moduleSprites[slot3];
        slotspr[3].sprite = moduleSprites[slot4];

        // set up label sprites
        updateMPLabelSprites();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public ModuleInterpreter.ModuleIdentity switchWithSlot(ModuleInterpreter.ModuleIdentity toSwitch, int slotid)
    {
        ModuleInterpreter.ModuleIdentity switched = toSwitch;

        // this is lowkey bad code, the slots could totally be an array instead of 4 seperate things. oh well.
        switch (slotid)
        {
            case 1:
                switched = slot1;
                slot1 = toSwitch;
                break;
            case 2:
                switched = slot2;
                slot2 = toSwitch;
                break;
            case 3:
                switched = slot3;
                slot3 = toSwitch;
                break;
            case 4:
                switched = slot4;
                slot4 = toSwitch;
                break;
            default:
                Debug.Log("Tried to switch with invalid slot id");
                break;
        }

        slotspr[slotid-1].sprite = moduleSprites[toSwitch];

        return switched;
    }

    public void updateMPLabelSprites()
    {
        Sprite[] p1;
        Sprite[] p2;
        switch (ControllerSettings.cs.player1_contype)
        {
            case ControllerSettings.ControllerType.LKEY:
                p1 = LKeyLabels;
                break;
            case ControllerSettings.ControllerType.RKEY:
                p1 = RKeyLabels;
                break;
            case ControllerSettings.ControllerType.CON:
                p1 = ConLabels;
                break;
            default:
                Debug.Log("Tried to set module parking labels for an invalid controller type");
                p1 = LKeyLabels;
                break;
        }
        switch (ControllerSettings.cs.player2_contype)
        {
            case ControllerSettings.ControllerType.LKEY:
                p2 = LKeyLabels;
                break;
            case ControllerSettings.ControllerType.RKEY:
                p2 = RKeyLabels;
                break;
            case ControllerSettings.ControllerType.CON:
                p2 = ConLabels;
                break;
            default:
                Debug.Log("Tried to set module parking labels for an invalid controller type");
                p2 = LKeyLabels;
                break;
        }

        for (int i=0; i < 4; i++)
        {
            p1labels[i].sprite = p1[i];
            p2labels[i].sprite = p2[i];
        }
    }
    
}
