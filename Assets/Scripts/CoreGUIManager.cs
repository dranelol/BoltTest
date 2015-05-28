using UnityEngine;
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

    const string k_OpenTransitionName = "Open";
    const string k_ClosedStateName = "Closed";
    private int m_OpenParameterId;
    public GameObject GameWindow;
    public GameObject VideoWindow;
    public GameObject AudioWindow;
    private Animator m_Open;
    private GameObject m_PreviouslySelected;

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
        m_OpenParameterId = Animator.StringToHash(k_OpenTransitionName);
        base.OnEnable();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            Show("Login_Menu");
        }
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

    // Keep a list of pointer input modules that we have disabled so that we can re-enable them
    List<PointerInputModule> disabled = new List<PointerInputModule>();

    int disableCount = 0;    // How many times has disable been called?

    public void Disable()
    {
        if (disableCount++ == 0)
        {
            UpdateState(false);
        }
    }

    public void Enable(bool enable)
    {
        if (!enable)
        {
            Disable();
            return;
        }
        if (--disableCount == 0)
        {
            UpdateState(true);
            if (disableCount > 0)
            {
                Debug.LogWarning("Warning UIDisableInput.Enable called more than Disable");
            }
        }
    }


    void UpdateState(bool enabled)
    {
        // First re-enable all systems
        for (int i = 0; i < disabled.Count; i++)
        {
            if (disabled[i])
            {
                disabled[i].enabled = true;
            }
        }

        disabled.Clear();

        EventSystem es = EventSystem.current;

        if (es == null) return;

        es.sendNavigationEvents = enabled;

        if (!enabled)
        {
            // Find all PointerInputModules and disable them
            PointerInputModule[] pointerInput = es.GetComponents<PointerInputModule>();
            if (pointerInput != null)
            {
                for (int i = 0; i < pointerInput.Length; i++)
                {
                    PointerInputModule pim = pointerInput[i];
                    if (pim.enabled)
                    {
                        pim.enabled = false;
                        // Keep a list of disabled ones
                        disabled.Add(pim);
                    }
                }
            }

            // Cause EventSystem to update it's list of modules
            es.enabled = false;
            es.enabled = true;
        }
    }

    #region MENU PANEL ACTIONS
    public void OpenPanel(Animator anim)
    {
        if (m_Open == anim)
            return;
        anim.gameObject.SetActive(true);
        var newPreviouslySelected = EventSystem.current.currentSelectedGameObject;

        //anim.transform.SetAsLastSibling();

        CloseCurrent();

        m_PreviouslySelected = newPreviouslySelected;

        m_Open = anim;
        m_Open.SetBool(m_OpenParameterId, true);
    }

    public void OpenPanelWithoutClose(Animator anim)
    {
        if (m_Open == anim)
            return;
        anim.gameObject.SetActive(true);
        var newPreviouslySelected = EventSystem.current.currentSelectedGameObject;

        //anim.transform.SetAsLastSibling();

        m_PreviouslySelected = newPreviouslySelected;

        anim.SetBool(m_OpenParameterId, true);
    }
    public void CloseCurrent()
    {
        if (m_Open == null)
            return;
        m_Open.SetBool(m_OpenParameterId, false);
        StartCoroutine(DisablePanelDelayed(m_Open));
        m_Open = null;
    }
    public void CloseWindow(Animator anim)
    {
        if (!anim.gameObject.active)
        {
            return;
        }
        anim.SetBool(m_OpenParameterId, false);
        StartCoroutine(DisablePanelDelayed(anim));
    }

    public IEnumerator DisablePanelDelayed(Animator anim)
    {
        bool closedStateReached = false;
        bool wantToClose = true;
        while (!closedStateReached && wantToClose)
        {
            if (!anim.IsInTransition(0))
                closedStateReached = anim.GetCurrentAnimatorStateInfo(0).IsName(k_ClosedStateName);

            wantToClose = !anim.GetBool(m_OpenParameterId);

            yield return new WaitForEndOfFrame();
        }
        if (wantToClose)
        {
            if (anim.gameObject.tag == "Settings")
            {
                if (GameWindow.activeSelf)
                {
                    GameWindow.SetActive(false);
                }
                else if (GameWindow.activeSelf)
                {
                    VideoWindow.SetActive(false);
                }
                else if (GameWindow.activeSelf)
                {
                    AudioWindow.SetActive(false);
                }
            }
            anim.gameObject.SetActive(false);
        }

        Hide(anim.gameObject.name);

    }

    public void SetSelected()
    {
        EventSystem.current.SetSelectedGameObject(null);
    }

    #endregion
}
