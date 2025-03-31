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

    public bool playerInputExists=false;

    public KeyCon_ModuleManager player1_kc { get; private set; }
    public KeyCon_ModuleManager player2_kc { get; private set; }
    public ModuleInterpreter player1_aq { get; private set; }
    public ModuleInterpreter player2_aq { get; private set; }

    private PlayerInput p1_pinput;
    private PlayerInput p2_pinput;

    private void Awake()
    {
        if (cs != null && cs != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            cs = this;
        }

        DontDestroyOnLoad(this.gameObject);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        playerInputExists = false;
    }

    /// <summary>
    /// returns PlayerInput of player associated with id -- should be 1 or 2
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public PlayerInput getPlayer(int id)
    {
        if (id == 1)
        {
            return p1_pinput;
        } 
        else if (id == 2)
        {
            return p2_pinput;
        }
        else
        {
            return null;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!playerInputExists && StatMan.sm.subfollow != null)
        {
            playerInputExists = true;

            // this is the code that should run upon entering the main game scene for the first time
            createNewPlayer1();
            createNewPlayer2();
            if (player1_contype != ControllerType.AQ)
            {
                player1_kc.createKeyConModuleUI(1, player1_contype);
                player2_kc.createKeyConModuleUI(2, player2_contype);

                forceSetModuleIdentity(player1_kc, ModuleInterpreter.ModuleIdentity.STEER);
                forceSetModuleIdentity(player2_kc, ModuleInterpreter.ModuleIdentity.AIM);
            }
            // later, need to think about changing controller types mid-game? think abt how to deal with module inventory. or just reset every time shrug
        }
    }

    private void createNewPlayer1()
    {
        if (player1_contype == ControllerType.AQ)
        {
            player1_aq = createNewAQPlayer();
            player1_aq.net = player1_net;
            player1_aq.transform.parent = StatMan.sm.subfollow;

            p1_pinput = player1_aq.GetComponent<PlayerInput>();
        }
        else
        {
            player1_kc = createNewKeyConPlayer(player1_contype);
            player1_kc.mod1.net = player1_net;
            player1_kc.mod2.net = player1_net;
            player1_kc.transform.parent = StatMan.sm.subfollow;

            p1_pinput = player1_kc.GetComponent<PlayerInput>();
        }
    }

    private void createNewPlayer2()
    {
        if (player2_contype == ControllerType.AQ)
        {
            player2_aq = createNewAQPlayer();
            player2_aq.net = !player1_net;
            player2_aq.transform.parent = StatMan.sm.subfollow;

            p2_pinput = player1_aq.GetComponent<PlayerInput>();
        }
        else
        {
            player2_kc = createNewKeyConPlayer(player2_contype);
            player2_kc.mod1.net = !player1_net;
            player2_kc.mod2.net = !player1_net;
            player2_kc.transform.parent = StatMan.sm.subfollow;

            p2_pinput = player1_kc.GetComponent<PlayerInput>();
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

    private ModuleInterpreter createNewAQPlayer()
    {
        return PlayerInput.Instantiate(AQPrefab).GetComponent<ModuleInterpreter>();
        //Debug.Log("createNewAQPlayer isn't implemented yet");
        //return null;
    }

    private void forceSetModuleIdentity(KeyCon_ModuleManager player, ModuleInterpreter.ModuleIdentity id1, ModuleInterpreter.ModuleIdentity id2=ModuleInterpreter.ModuleIdentity.NONE)
    {
        player.mod1_init = id1;
        player.mod2_init = id2;

        player.mod1.identity = id1;
        player.mod2.identity = id2;
    }

    /// <summary>
    /// create new playerinputs upon game starting for the first time
    /// </summary>
    public void createNewPI()
    {
        // this is old stuff. dead code will replace. not needed rn
        /*
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
        */
    }
}
