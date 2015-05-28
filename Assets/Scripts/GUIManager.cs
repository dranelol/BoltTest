using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GUIManager : MonoBehaviour, IGUIBehavior
{
    [SerializeField]
    private List<GameObject> registered = new List<GameObject>();

    protected Dictionary<string, GameObject> elements = new Dictionary<string, GameObject>();
    protected bool debug;
    protected bool init;

    public void Show(string item)
    {
        //Debug.Log("show " + item);

        if (elements[item].activeSelf == false && elements[item] != null)
        {
            elements[item].transform.SetAsLastSibling();

            elements[item].SetActive(true);

            

            Animator anim = elements[item].GetComponent<Animator>();
            if(anim != null)
            {
                CoreGUIManager.Instance.OpenPanel(anim);
            }
        }
        else
        {
            Debug.Log("could not show " + item);
        }
    }

    //this will only disable the object
    //the object that is disabled will handle popping itself off the menustate stack
    public void Hide(string item)
    {

        if (elements[item].activeSelf == true && elements[item] != null)
        {
            elements[item].transform.SetAsFirstSibling();

            elements[item].SetActive(false);

            
            
        }
        else
        {
            //Debug.Log("could not hide: " + item);
        }
    }

    /// <summary>
    /// setactive true this objects elements
    /// </summary>
    public void ShowAll()
    {
        foreach (KeyValuePair<string, GameObject> entry in elements)
        {
            if (entry.Value != null)
            {
                Show(entry.Key);
            }
        }

    }
    /// <summary>
    /// setactive false this objects elements
    /// </summary>
    public void HideAll()
    {
        foreach (KeyValuePair<string, GameObject> entry in elements)
        {
            if (entry.Value != null)
            {
                Hide(entry.Key);
            }
        }
    }

    public void EnableButtonInteraction(string item)
    {
        if (elements[item] != null)
        {
            // get all elements in this object
            Button[] buttons = elements[item].GetComponentsInChildren<Button>();

            // enable interaction on each one
            foreach(Button button in buttons)
            {
                button.interactable = true;
            }
        }
    }

    public void DisableButtonInteraction(string item)
    {
        if (elements[item] != null)
        {
            // get all elements in this object
            Button[] buttons = elements[item].GetComponentsInChildren<Button>();

            // disable interaction on each one
            foreach (Button button in buttons)
            {
                button.interactable = false;
            }
        }
    }

    public void EnableAllButtonInteraction()
    {
        foreach (KeyValuePair<string, GameObject> entry in elements)
        {
            // if GO has a button, enable interaction
            Button button = entry.Value.GetComponent<Button>();

            if(button != null)
            {
                button.interactable = true;
            }
        }
    }

    public void DisableAllButtonInteraction()
    {
        foreach (KeyValuePair<string, GameObject> entry in elements)
        {
            // if GO has a button, disable interaction
            Button button = entry.Value.GetComponent<Button>();

            if (button != null)
            {
                button.interactable = false;
            }
        }
    }

    protected virtual void Awake()
    {
        //all elements register at this point
        if (debug)
        {
            Debug.Log("Awake for " + gameObject.name + " Manager");
        }

    }

    protected virtual void OnEnable()
    {
        if (!init)
        {
            if (debug)
            {
                Debug.Log("Set init for " + gameObject.name);
            }

            init = true;

            HideAll();
        }

        else
        {
            if (debug)
            {
                Debug.Log(name + " already init :: first OnEnable triggered");
            }
        }

    }

    protected virtual void OnDisable()
    {

    }

    public virtual void Register(GameObject ele)
    {
        if (debug)
        {
            Debug.Log("Register " + ele.name);
        }
        elements.Add(ele.name, ele);
        registered.Add(ele);

    }

    //when it goes disable pop it off
    public virtual void Unregister(GameObject ele)
    {

    }

    
}
