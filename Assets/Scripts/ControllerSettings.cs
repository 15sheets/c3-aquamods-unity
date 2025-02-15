using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class ControllerSettings : MonoBehaviour
{
    public static ControllerSettings cs { get; private set; }

    public GameObject LKeyPrefab;
    public GameObject RKeyPrefab;
    public GameObject ControllerPrefab;
    public GameObject AQPrefab; // custom controllers

    public GameObject moduleParkingPrefab;

    public enum ControllerType { LKEY, RKEY, CON, AQ };
    public ControllerType player1_contype;
    public ControllerType player2_contype;

    public bool player1_net;

    //public bool playerInputExists;

    public KeyCon_ModuleManager player1_kc { get; private set; }
    public KeyCon_ModuleManager player2_kc { get; private set; }
    public ModuleInterpreter player1_aq { get; private set; }
    public ModuleInterpreter player2_aq { get; private set; }

    private void Awake()
    {
        if (cs != null && cs != this)
        {
            Destroy(this);
        }
        else
        {
            cs = this;
        }

        DontDestroyOnLoad(this.gameObject);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // this is the code that should run upon entering the main game scene for the first time
        createNewPlayer1();
        createNewPlayer2();
        if (player1_contype != ControllerType.AQ)
        {
            player1_kc.createKeyConModuleUI(1, player1_contype);
            player2_kc.createKeyConModuleUI(2, player2_contype);
        }
        if (player2_contype == ControllerType.CON) // only 1 prefab for con so need to manually make sure module identity is correct
        {
            forceSetModuleIdentity(player2_kc.mod1, ModuleInterpreter.ModuleIdentity.AIM);
        }
        // later, need to think about changing conroller types mid-game? think abt how to deal with module inventory. or just reset every time shrug
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void createNewPlayer1()
    {
        if (player1_contype == ControllerType.AQ)
        {
            player1_aq = createNewAQPlayer();
            player1_aq.net = player1_net;
            player1_aq.transform.parent = StatMan.sm.subfollow;
        }
        else
        {
            player1_kc = createNewKeyConPlayer(player1_contype);
            player1_kc.mod1.net = player1_net;
            player1_kc.mod2.net = player1_net;
            player1_kc.transform.parent = StatMan.sm.subfollow;
        }
    }

    private void createNewPlayer2()
    {
        if (player2_contype == ControllerType.AQ)
        {
            player2_aq = createNewAQPlayer();
            player2_aq.net = !player1_net;
            player2_aq.transform.parent = StatMan.sm.subfollow;
        }
        else
        {
            player2_kc = createNewKeyConPlayer(player2_contype);
            player2_kc.mod1.net = !player1_net;
            player2_kc.mod2.net = !player1_net;
            player2_kc.transform.parent = StatMan.sm.subfollow;
        }
    }

    private KeyCon_ModuleManager createNewKeyConPlayer(ControllerType contype)
    {
        if (ModuleParking.mp == null)
        {
            Instantiate(moduleParkingPrefab, StatMan.sm.subfollow);
        }

        switch (contype)
        {
            case ControllerType.LKEY:
                return PlayerInput.Instantiate(LKeyPrefab, controlScheme: "KeyLeft", pairWithDevice: Keyboard.current).GetComponent<KeyCon_ModuleManager>();
            case ControllerType.RKEY:
                return PlayerInput.Instantiate(RKeyPrefab, controlScheme: "KeyRight", pairWithDevice: Keyboard.current).GetComponent<KeyCon_ModuleManager>();
            case ControllerType.CON:
                return PlayerInput.Instantiate(ControllerPrefab, controlScheme: "Gamepad").GetComponent<KeyCon_ModuleManager>();
            default:
                Debug.Log("Tried to create a player with invalid controller type");
                return null;
        }
    }

    // fill out later
    private ModuleInterpreter createNewAQPlayer()
    {
        return null;
    }

    private void forceSetModuleIdentity(ModuleInterpreter module, ModuleInterpreter.ModuleIdentity id)
    {
        module.identity = id;
    }

    public void createNewPI()
    {
        Scene currScene = SceneManager.GetActiveScene();
        bool spawnPI = currScene.name == "angela_scene";
        if (spawnPI)
        {
            PlayerInput.Instantiate(LKeyPrefab, controlScheme: "KeyLeft", pairWithDevice: Keyboard.current);
            PlayerInput.Instantiate(RKeyPrefab, controlScheme: "KeyRight", pairWithDevice: Keyboard.current);
        }
        if (spawnPI && ModuleParking.mp == null)
        {
            Instantiate(moduleParkingPrefab);
        }
    }
}
