using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class CoreGUIManager : GUIManager, IGUIBehavior
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

    public enum MenuState
    {
        Active,
        Paused,

    }

    public MenuState CurrentState;

    [SerializeField]
    private Text notificationText;

    [SerializeField]
    private Text notificationSubmitText;

    private static CoreGUIManager _instance;

    public static CoreGUIManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<CoreGUIManager>();
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
    }

    protected override void OnEnable()
    {
        base.OnEnable();
    }

    void Update()
    {
        
    }

    public void Pause(bool showMenu)
    {
        if (showMenu)
        {
            Show("Pause");
        }

        else
        {
            // disable input to entire gui?

            //GUIManager.instance.guiCamera.useKeyboard = false;
            //GUIManager.instance.guiCamera.useController = false;
            GUI.enabled = false;
        } 
        
        CurrentState = MenuState.Paused;

    }

    public void UnPause()
    {
        if (elements["Pause"].activeSelf == true)
        {
            Hide("Pause");
        }

        else
        {
            // re-enable input to entire gui?

            //GUIManager.instance.guiCamera.useKeyboard = true;
            //GUIManager.instance.guiCamera.useController = true;
            GUI.enabled = true;
        } 
        
        CurrentState = MenuState.Active;

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

    public void SetNotificationText(string text)
    {
        notificationText.text = text;
    }

    public void SetNotificationSubmitText(string text)
    {
        notificationSubmitText.text = text;
    }
}
