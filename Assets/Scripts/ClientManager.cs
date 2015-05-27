using UnityEngine;
using System.Collections;

public class ClientManager : MonoBehaviour 
{
    private string key = "ulVGD2015";

    private string createAccountURL = "http://donionrings.me/Games/NetworkingDemo/CreateAccount.php?";
    private string loginURL = "http://donionrings.me/Games/NetworkingDemo/Login.php?";

    [SerializeField]
    private int authLevel;

    [SerializeField]
    private string loginName;

    [SerializeField]
    private string displayName;

    [SerializeField]
    private string loginPassword;

    public string loginIP;

    public bool SettingIP = false;

    private static ClientManager _instance;

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

    public static ClientManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<ClientManager>();
                DontDestroyOnLoad(_instance.gameObject);
            }

            return _instance;
        }
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
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
        string getUrl = createAccountURL +
            "Name=" + WWW.EscapeURL(loginName) +
            "LoginName=" + WWW.EscapeURL(displayName) +
            "&Password=" + WWW.EscapeURL(loginPassword) +
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
            if (createAccount.text == "")
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

        string getUrl = loginURL +
            "Name=" + WWW.EscapeURL(loginName) +
            "LoginName=" + WWW.EscapeURL(displayName) +
            "&Password=" + WWW.EscapeURL(loginPassword) +
            "&Key=" + WWW.EscapeURL(key) +
            "&CurrentIP=" + WWW.EscapeURL(loginIP);

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

    public IEnumerator SetPublicIP()
    {
        SettingIP = true;

        WWW myExtIPWWW = new WWW("http://checkip.dyndns.org");

        if (myExtIPWWW == null)
        {

            yield return null;
        }

        else
        {
            yield return myExtIPWWW;

            string myExtIP = myExtIPWWW.text;

            myExtIP = myExtIP.Substring(myExtIP.IndexOf(":") + 1);

            myExtIP = myExtIP.Substring(0, myExtIP.IndexOf("<"));
            Debug.Log(myExtIP);
            loginIP = myExtIP;
            yield return null;
        }

        SettingIP = false;

    }


}
