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

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            StartCoroutine(Login());
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

        WWW createAccount = new WWW(getUrl);

        yield return createAccount;

        if (createAccount.error != null)
        {
            Debug.Log("There was an error creating the account: " + createAccount.error);
        }

        else
        {
            if(createAccount.text == "")
            {
                Debug.Log("account created successfully!");
            }

            else
            {
                Debug.Log("An error occured!");
                Debug.Log(createAccount.text);
            }
        }

        yield return null;
    }

    public IEnumerator Login()
    {
        string name = "Matt";
        string password = "test123";
        string ip = "76.72.4.40";

        string getUrl = loginURL +
            "Name=" + WWW.EscapeURL(name) +
            "&Password=" + WWW.EscapeURL(password) +
            "&Key=" + WWW.EscapeURL(key) +
            "&CurrentIP=" + WWW.EscapeURL(ip);

        WWW login = new WWW(getUrl);

        yield return login;

        if (login.error != null)
        {
            Debug.Log("There was an error logging in: " + login.error);
        }

        else
        {
            if (login.text == "")
            {
                Debug.Log("Login successful!");
            }

            else
            {
                Debug.Log("An error occured!");
                Debug.Log(login.text);
            }
        }

        yield return null;
    }



}
