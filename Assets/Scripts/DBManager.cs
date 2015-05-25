using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DBManager : MonoBehaviour
{
    private static DBManager _instance;

    private string key = "ulVGD2015";

    private string createAccountURL = "http://donionrings.me/Games/NetworkingDemo/CreateAccount.php?";
    private string loginURL = "http://donionrings.me/Games/NetworkingDemo/Login.php?";

    [SerializeField]
    private int authLevel;


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

    public static DBManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<DBManager>();
                DontDestroyOnLoad(_instance.gameObject);
            }

            return _instance;
        }
    }

    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            StartCoroutine(AccountCreate());
        }
    }

    public IEnumerator AccountCreate()
    {
        string name = "Matt";
        string password = "test123";

        string getUrl = createAccountURL +
            "Name=" + WWW.EscapeURL(name) +
            "&Password=" + WWW.EscapeURL(password) +
            "&Key=" + WWW.EscapeURL(key) +
            "&AuthLevel=" + authLevel;

        WWW createAccountGet = new WWW(getUrl);

        yield return createAccountGet;

        if (createAccountGet.error != null)
        {
            Debug.Log("There was an error creating the account: " + createAccountGet.error);
        }

        else
        {
            if(createAccountGet.text == "")
            {
                Debug.Log("account created successfully!");
            }

            else
            {
                Debug.Log("An error occured!");
                Debug.Log(createAccountGet.text);
            }
            
        }

        yield return null;
    }

    public IEnumerator Login()
    {

        yield return null;
    }



}
