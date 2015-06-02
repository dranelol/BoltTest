using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour 
{
    public GameObject ServerManagerPrefab;
    public GameObject ClientManagerPrefab;

    private static GameManager _instance;

    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<GameManager>();
                DontDestroyOnLoad(_instance.gameObject);
            }

            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance == null)
        {
            //If I am the first instance, make me the Singleton
            _instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Debug.Log(Network.player.externalIP.ToString());
            StartMenuGUIManager.Instance.Show("Login_Menu");
        }
    }

    public void StartServer()
    {
        Instantiate(GameManager.Instance.ServerManagerPrefab);
        //BoltLauncher.StartServer(UdpKit.UdpEndPoint.Parse("127.0.0.1:27000"));
        BoltLauncher.StartServer(UdpKit.UdpEndPoint.Any);

        BoltNetwork.SetHostInfo("Here You Go", null);
        //BoltNetwork.LoadScene("Tutorial1");

        // go to lobby
        CoreGUIManager.Instance.Show("GameLobby");
    }

    
}
