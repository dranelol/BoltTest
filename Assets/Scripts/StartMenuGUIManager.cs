using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class StartMenuGUIManager : GUIManager, IGUIBehavior
{
    [SerializeField]
    private Camera guiCamera;

    public Camera GUICamera
    {
        get
        {
            return guiCamera;
        }
    }

    public enum PanelState
    {
        Login,
        CreateAccount,
        StartMenu,
        ServerBrowser,
        Lobby
    }

    [SerializeField]
    private Text ServerLobbyText;

    [SerializeField]
    private GameObject serverBrowser;

    public PanelState CurrentPanelState;

    public HashSet<GameObject> LobbyMembers = new HashSet<GameObject>();

    private static StartMenuGUIManager _instance;

    public static StartMenuGUIManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<StartMenuGUIManager>();
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

        base.Awake();

        Messenger.AddListener<CredentialToken>("UserAddedToLobby", AddToLobby);
        Messenger.AddListener<CredentialToken>("UserRemovedFromLobby", RemoveFromLobby);
    }

    protected override void OnEnable()
    {
        base.OnEnable();
    }

    void Update()
    {

    }

    public override void Show(string item)
    {

        base.Show(item);

        if(item == "ServerBrowser")
        {
            BoltLauncher.StartClient();
            serverBrowser.GetComponent<PopulateServerBrowser>().Populate();
        }
    }

    public void SetActiveButton(string name)
    {
        //print("setactivebutton " + name);
        if (elements.ContainsKey(name))
        {
            // set active object

            //UICamera.selectedObject = _elements[name];
        }
    }

    public void AddToLobby(CredentialToken user)
    {

    }

    public void RemoveFromLobby(CredentialToken user)
    {

    }

    public void SetServerLobbyTitle(string text)
    {
        ServerLobbyText.text = text;
    }
    
}
