using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Text.RegularExpressions;

public class ClientManager : MonoBehaviour 
{
    private string key = "ulVGD2015";

    private string createAccountURL = "http://donionrings.me/Games/NetworkingDemo/CreateAccount.php?";
    private string loginURL = "http://donionrings.me/Games/NetworkingDemo/Login.php?";
    private string getTokenInfoURL = "http://donionrings.me/Games/NetworkingDemo/GetTokenInfo.php?";

    private string loginName;
    private string loginPassword;

    private int authLevel;

    public UnityEvent OnLoginSuccessful;

    public UnityEvent OnLoginUnsuccessful;

    public UnityEvent OnAccountCreateSuccessful;

    public UnityEvent OnAccountCreateUnsuccessful;

    public UnityEvent OnPublicIPUnsuccessful;



    //public UnityEvent<string, string> OnLoginSuccessful;

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
    }

    public void CreateAccount()
    {
        StartCoroutine(createAccount());
    }

    private IEnumerator createAccount()
    {
        if (displayName == "" || loginName == "" || loginPassword == "")
        {
            CoreGUIManager.Instance.SetNotificationSubmitText("All fields not filled out!");
            CoreGUIManager.Instance.Show("NotificationSubmit");
        }

        else
        {
            authLevel = 0;

            Debug.Log(loginName);
            Debug.Log(displayName);
            Debug.Log(loginPassword);

            string getUrl = createAccountURL +
                "Name=" + WWW.EscapeURL(loginName) +
                "&DisplayName=" + WWW.EscapeURL(displayName) +
                "&Password=" + WWW.EscapeURL(loginPassword) +
                "&Key=" + WWW.EscapeURL(key) +
                "&AuthLevel=" + authLevel;

            WWW createAccount = new WWW(getUrl);

            yield return createAccount;

            if (createAccount.error != null)
            {
                Debug.Log("There was an error creating the account: " + createAccount.error);
                CoreGUIManager.Instance.SetNotificationSubmitText("Error creating account: " + createAccount.error);
                CoreGUIManager.Instance.Show("NotificationSubmit");
            }

            else
            {
                if (createAccount.text == "")
                {
                    Debug.Log("account created successfully!");
                    OnAccountCreateSuccessful.Invoke();
                }

                else
                {
                    Debug.Log("An error occured!");
                    Debug.Log(createAccount.text);
                    CoreGUIManager.Instance.SetNotificationSubmitText("Error creating account: " + createAccount.text);
                    CoreGUIManager.Instance.Show("NotificationSubmit");
                }
            }
        }

        yield return null;
    }

    public void Login()
    {
        Debug.Log(loginName);
        Debug.Log(loginPassword);
        StartCoroutine(login());
    }

    private IEnumerator login()
    {
        CoreGUIManager.Instance.SetNotificationText("Logging in...");
        CoreGUIManager.Instance.Show("Notification");

        yield return StartCoroutine(GetLoginInfo());

        string getUrl = loginURL +
            "Name=" + WWW.EscapeURL(loginName) +
            "&Password=" + WWW.EscapeURL(loginPassword) +
            "&Key=" + WWW.EscapeURL(key) +
            "&CurrentIP=" + WWW.EscapeURL(loginIP);

        WWW login = new WWW(getUrl);

        yield return login;

        if (login.error != null)
        {
            Debug.Log("There was an error logging in: " + login.error);
            CoreGUIManager.Instance.Hide("Notification");
            CoreGUIManager.Instance.SetNotificationSubmitText("Error logging in: " + login.error);
            CoreGUIManager.Instance.Show("NotificationSubmit");
        }

        else
        {
            if (login.text == "")
            {
                Debug.Log("Login successful!");
                CoreGUIManager.Instance.Hide("Notification");
                OnLoginSuccessful.Invoke();
            }

            else
            {
                Debug.Log("An error occured!");
                CoreGUIManager.Instance.Hide("Notification");
                CoreGUIManager.Instance.SetNotificationSubmitText("Error logging in: " + login.text);
                CoreGUIManager.Instance.Show("NotificationSubmit");
                Debug.Log(login.text);
            }
        }

        yield return null;
    }

    public IEnumerator GetLoginInfo()
    {
        gettingLoginInfo = true;

        WWW myExtIPWWW = new WWW("https://api.ipify.org/");

        if (myExtIPWWW == null)
        {
            Debug.Log("there was an error accessing the public IP checker");
            OnPublicIPUnsuccessful.Invoke();
            yield return null;
        }

        else
        {
            yield return myExtIPWWW;
            Debug.Log("return from www");
            
            string myExtIP = myExtIPWWW.text;

            Debug.Log(myExtIP);

            if (myExtIP == "")
            {
                Debug.Log("there was an error accessing the public IP checker");
                OnPublicIPUnsuccessful.Invoke();
                yield return null;

            }

            else
            {

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
                    string tokenInfo = getTokenInfo.text;
                    Debug.Log(tokenInfo);
                    if (tokenInfo != "")
                    {
                        Debug.Log("Info successful!");

                        displayName = tokenInfo.Substring(0, tokenInfo.IndexOf("|"));
                        string authLevelString = tokenInfo.Substring(tokenInfo.IndexOf("|") + 1);
                        authLevel = System.Convert.ToInt32(authLevelString);
                        Debug.Log(authLevelString);
                        Debug.Log(displayName);

                    }

                    else
                    {
                        Debug.Log("Info unsuccessful!");
                    }
                }
            }


            yield return null;
        }

        gettingLoginInfo = false;

    }

    public void GetPublicIP()
    {
        StartCoroutine(getPublicIP());
    }

    private IEnumerator getPublicIP()
    {
        loginIP = "";
        WWW myExtIPWWW = new WWW("http://checkip.dyndns.org");

        if (myExtIPWWW == null)
        {
            Debug.Log("there was an error accessing the public IP checker");
            OnPublicIPUnsuccessful.Invoke();
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
        }

        yield return null;
    }

    public void SetUser(InputField userText)
    {
        loginName = userText.text;
    }

    public void SetPass(InputField passwordText)
    {
        loginPassword = passwordText.text;
    }

    public void SetDisplayName(InputField displayNameText)
    {
        displayName = displayNameText.text;
    }

    public CredentialToken GetToken()
    {
        CredentialToken token = new CredentialToken();
        
        token.AuthLevel = authLevel;
        token.DisplayName = displayName;
        token.LoginName = loginName;
        token.IP = loginIP;
        token.Password = loginPassword;
        
        return token;
    }
}
