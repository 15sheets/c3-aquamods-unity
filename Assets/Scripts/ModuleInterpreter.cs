using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;

public class ModuleInterpreter : MonoBehaviour
{
    public bool net; // toggle whether player controls net or harpoon

    public float vspeedROC=2;

    //public SubmarineModules submarine; 

    public enum ModuleIdentity { NONE, STEER, SPEED, AIM, SHOOT, SHIELD, BATTERY };
    public ModuleIdentity identity; // set initial value in editor

    private float sliderValue;
    private bool buttonValue;

    private SubmarineModules submarine;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // in the future, decide this based on player 1 vs 2 ...?
        //identity = (net) ? ModuleIdentity.STEER : ModuleIdentity.SPEED;
        /*
        if (StatMan.sm.submods != null) { 
            submarine = StatMan.sm.submods; 
        }
        */
        
    } 

    // Update is called once per frame
    void Update()
    {
        SubmarineModules submarine = StatMan.sm.submods;

        // update submarine values here based on inputs
        switch (identity)
        {
            case ModuleIdentity.STEER:
                submarine.setSteer(sliderValue);
                break;
            case ModuleIdentity.SPEED:
                submarine.setSpeed(sliderValue * vspeedROC * Time.deltaTime, true);
                break;
            case ModuleIdentity.AIM:
                if (net) { submarine.setNetAim(sliderValue); }
                else { submarine.setHarpoonAim(sliderValue); }
                break;
            case ModuleIdentity.SHOOT:
                if (net) { submarine.setNetFire(buttonValue); }
                else { submarine.setHarpoonFire(buttonValue); }
                break;
            case ModuleIdentity.SHIELD:
                submarine.setShield(sliderValue);
                break;
            case ModuleIdentity.BATTERY: // TODO !!!
                submarine.setBattery(buttonValue);
                break;
            default:
                break;
        }
    }

    // setting keycon values
    public void setSliderValue(InputAction.CallbackContext input)
    {
        sliderValue = input.ReadValue<float>(); // double check that this is correct...
    }

    public void setButtonValue(InputAction.CallbackContext input)
    {
        buttonValue = input.ReadValueAsButton();
    }
    
    public void switchWith1(InputAction.CallbackContext input)
    {
        if (input.performed) {
            Debug.Log("switch event is happening");
            identity = ModuleParking.mp.switchWithSlot(identity, 1);
        }
    }

    public void switchWith2(InputAction.CallbackContext input)
    {
        if (input.performed) {
            identity = ModuleParking.mp.switchWithSlot(identity, 2);
        }
    }

    public void switchWith3(InputAction.CallbackContext input)
    {
        if (input.performed) {
            identity = ModuleParking.mp.switchWithSlot(identity, 3);
        }
    }

    public void switchWith4(InputAction.CallbackContext input)
    {
        if (input.performed) {
            identity = ModuleParking.mp.switchWithSlot(identity, 4);
        }
    }

    // keyboard specific functions... check if shift isn't pressed before switching
    // jank ass method :/ thanks unity
    public void Lkey_switch1With1(InputAction.CallbackContext input)
    {
        if (!Input.GetKey(KeyCode.LeftShift))
        {
            switchWith1(input);
        }
    }

    public void Lkey_switch1With2(InputAction.CallbackContext input)
    {
        if (!Input.GetKey(KeyCode.LeftShift))
        {
            switchWith2(input);
        }
    }

    public void Lkey_switch1With3(InputAction.CallbackContext input)
    {
        if (!Input.GetKey(KeyCode.LeftShift))
        {
            switchWith3(input);
        }
    }

    public void Lkey_switch1With4(InputAction.CallbackContext input)
    {
        if (!Input.GetKey(KeyCode.LeftShift))
        {
            switchWith4(input);
        }
    }

    public void Rkey_switch1With1(InputAction.CallbackContext input)
    {
        if (!Input.GetKey(KeyCode.RightShift))
        {
            switchWith1(input);
        }
    }

    public void Rkey_switch1With2(InputAction.CallbackContext input)
    {
        if (!Input.GetKey(KeyCode.RightShift))
        {
            switchWith2(input);
        }
    }

    public void Rkey_switch1With3(InputAction.CallbackContext input)
    {
        if (!Input.GetKey(KeyCode.RightShift))
        {
            switchWith3(input);
        }
    }

    public void Rkey_switch1With4(InputAction.CallbackContext input)
    {
        if (!Input.GetKey(KeyCode.RightShift))
        {
            switchWith4(input);
        }
    }

    // setting custom values (call functions directly)
    //TODO: FINISH FILLING IN ONCE WE'VE NAILED DOWN INTERFACE BTWN ARDUINO AND UNITY
    /*
    public void setSpeed(InputAction.CallbackContext input)
    {

    }

    public void setSteer(InputAction.CallbackContext input)
    {

    }

    public void setShield(InputAction.CallbackContext input)
    {

    }

    public void setAim(InputAction.CallbackContext input)
    {
        if (net) { submarine.setNetAim(sliderValue); }
        else { submarine.setHarpoonAim(sliderValue); }
    }

    public void setShoot(InputAction.CallbackContext input)
    {
        bool button = input.ReadValueAsButton();

        if (net) { submarine.setNetFire(button); }
        else { submarine.setHarpoonFire(button); }
    }

    //TODO
    public void setBattery(InputAction.CallbackContext input)
    {
        // FILL IN ONCE BATTERY IS WORKING
    }
    */
}
