using UnityEngine;
using System.Collections;

public class ClientManager : MonoBehaviour 
{
    private string key = "ulVGD2015";

    private string createAccountURL = "http://donionrings.me/Games/NetworkingDemo/CreateAccount.php?";
    private string loginURL = "http://donionrings.me/Games/NetworkingDemo/Login.php?";
    private string getTokenInfoURL = "http://donionrings.me/Games/NetworkingDemo/GetTokenInfo.php?";

    private string loginName;
    private string loginPassword;

    private int authLevel;

    public int AuthLevel
    {
        get
        {
            return authLevel;
        }
    }

    private string displayName;

    public string DisplayName
    {
        get
        {
            return displayName;
        }
    }

    private string loginIP;

    public string LoginIP
    {
        get
        {
            return loginIP;
        }
    }

    private bool gettingLoginInfo;

    public bool GettingLoginInfo
    {
        get
        {
            return gettingLoginInfo;
        }
    }

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
        authLevel = 0;

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

    public IEnumerator GetLoginInfo()
    {
        gettingLoginInfo = true;

        WWW myExtIPWWW = new WWW("http://checkip.dyndns.org");

        if (myExtIPWWW == null)
        {
            Debug.Log("there was an error accessing the public IP checker");
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

            string getUrl = getTokenInfoURL +
                "Name=" + WWW.EscapeURL(loginName) +
                "&Key=" + WWW.EscapeURL(key);

            WWW getTokenInfo = new WWW(getUrl);

            yield return getTokenInfo;

            if (getTokenInfo.error != null)
            {
                Debug.Log("There was an error getting info: " + getTokenInfo.error);
            }

            else
            {
                Debug.Log("Info successful!");
                string tokenInfo = getTokenInfo.text;
                string displayName = tokenInfo.Substring(0, tokenInfo.IndexOf("|"));
                string authString = tokenInfo.Substring(tokenInfo.IndexOf("|"));

                Debug.Log(displayName);
                Debug.Log(authString);
            }


            yield return null;
        }

        gettingLoginInfo = false;

    }

    public void SetUserPass(string user, string pass)
    {
        loginName = user;
        loginPassword = pass;
    }

    public void SetDisplayName(string display)
    {
        displayName = display;
    }
}
