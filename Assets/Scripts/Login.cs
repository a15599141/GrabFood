using UnityEngine;
using UnityEngine.UI;

public class Login : MonoBehaviour
{
    public InputField username;
    public InputField password;
    public Button LoginBtn;
    public Button JoinUsBtn;
    private GameObject RegisterPagePrefab;
    // Start is called before the first frame update
    void Start()
    {
        LoginBtn.onClick.AddListener(() =>
        {

            int result = MySqlManager.LoginCheck(username.text, password.text);
            if(string.IsNullOrEmpty(username.text)) TipsManager.SpawnTips("Username cannot be empty");
            else if (string.IsNullOrEmpty(password.text)) TipsManager.SpawnTips("Password cannot be empty");
            else if (result == -1) TipsManager.SpawnTips("Connection error");
            else if (result == 0) TipsManager.SpawnTips("username not find");
            else if (result == 1) TipsManager.SpawnTips("Incorrect username or password.");
            else if (result == 2)
            {
                int userid = MySqlManager.GetUserIdByUsername(username.text);
                int usertype = MySqlManager.GetUserTypeByUsername(username.text);
                if (userid == 0) TipsManager.SpawnTips("Login error. (invalid ID)");
                else if (usertype == 0) TipsManager.SpawnTips("Login error. (invalid usertype)");
                else
                {
                    UserManager.SpawnUserCanvas(userid, username.text, usertype);
                    Destroy(gameObject);
                }
            }
        });
        JoinUsBtn.onClick.AddListener(() =>
        {
            SpawnRegisterPage();
            Destroy(gameObject);
        });
    }
    public void SpawnRegisterPage()
    {
        RegisterPagePrefab = (GameObject)Resources.Load("Prefabs/RegisterPage");
        Instantiate(RegisterPagePrefab);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
