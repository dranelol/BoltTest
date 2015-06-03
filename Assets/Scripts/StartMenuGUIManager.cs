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

    [SerializeField]
    private GameObject userPrefab;

    [SerializeField]
    private GameObject lobbyUsers;

    public PanelState CurrentPanelState;

    // map display name to GUI object
    public Dictionary<string, GameObject> lobbyMembers = new Dictionary<string, GameObject>();

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

        Messenger.AddListener<string>("UserAddedToLobby", AddToLobby);
        Messenger.AddListener<string>("UserRemovedFromLobby", RemoveFromLobby);
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

    public void AddToLobby(string userDisplayName)
    {
        if (lobbyMembers.ContainsKey(userDisplayName) == false)
        {
            // instatiate "user" gui prefab

            GameObject user = (GameObject)Instantiate(userPrefab, Vector3.zero, Quaternion.identity);

            user.transform.parent = lobbyUsers.transform;
            user.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            Vector3 position = user.GetComponent<RectTransform>().anchoredPosition3D;

            position.z = 0.0f;

            user.GetComponent<RectTransform>().anchoredPosition3D = position;

            user.transform.GetChild(0).GetComponent<Text>().text = userDisplayName;

            lobbyMembers[userDisplayName] = user;

            


        }
    }

    public void RemoveFromLobby(string userDisplayName)
    {
        if(lobbyMembers.ContainsKey(userDisplayName))
        {
            Destroy(lobbyMembers[userDisplayName]);
        }
    }

    public void SetServerLobbyTitle(string text)
    {
        ServerLobbyText.text = text;
    }
    
}
