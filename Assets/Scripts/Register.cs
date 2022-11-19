using UnityEngine;
using UnityEngine.UI;

public class Register : MonoBehaviour
{
    [Header("SelectUserTypePage")]
    public Toggle ResToggle;
    public Button SelectUserTypeBackBtn;

    [Header("SignUpPage")]
    public InputField username;
    public InputField password;
    public InputField confirm_password;
    public Button SignUpNextBtn;

    [Header("ResInfoEditPage")]
    public GameObject ResInfoEditPage;
    public InputField LicenseNum;
    public InputField ResName;
    public InputField ResContactNum;
    public InputField ResAddress;
    public Button ResInfoEditNextBtn;

    [Header("ResInfoEditPage")]
    public GameObject CusInfoEditPage;
    public InputField CusPhoneNum;
    public InputField CusName;
    public InputField CusEmail;
    public InputField CusAddress;
    public Button CusInfoEditNextBtn;

    [Header("TagEditPage")]
    public GameObject TagEditPage;
    public Text TagTitle;
    public Button AddTagBtn;
    public InputField AddTagInputField;
    public ToggleGroup TagToggleGroup;
    public Button AddTagNextBtn;
    // Start is called before the first frame update
    void Start()
    {
        SelectUserTypeBackBtn.onClick.AddListener(() =>
        {
            GameObject LoginPagePrefab = (GameObject)Resources.Load("Prefabs/LoginPage");
            Instantiate(LoginPagePrefab);
            Destroy(gameObject); // Destroy Register page
        });
        SignUpNextBtn.onClick.AddListener(() => 
        {
            if (RegisterCheck(username.text, password.text, confirm_password.text))
            {
                if (ResToggle.isOn) ResInfoEditPage.SetActive(true);
                else CusInfoEditPage.SetActive(true);
            }

        });

        ResInfoEditNextBtn.onClick.AddListener(() =>
        {
            if (ResInfoCheck(LicenseNum.text, ResName.text, ResContactNum.text, ResAddress.text))
            {
                AddTagInputField.placeholder.GetComponent<Text>().text = "add tag...";
                TagTitle.text = "What make you special...";
                TagEditPage.SetActive(true);
            }
        });

        CusInfoEditNextBtn.onClick.AddListener(() =>
        {
            if (CusInfoCheck(CusPhoneNum.text, CusName.text, CusEmail.text, CusAddress.text))
            {
                AddTagInputField.placeholder.GetComponent<Text>().text = "add favor...";
                TagTitle.text = "Tell me what you like...";
                TagEditPage.SetActive(true);
            }
        });

        AddTagBtn.onClick.AddListener(() =>
        {
            if(TagCheck(AddTagInputField.text))
            {

            }
        });

        AddTagNextBtn.onClick.AddListener(() =>
        {
            int userID = InsertUser(username.text, password.text, GetUserType());
            if (userID > 0)
            {
                int ResOrCusID = 0;
                // tag not insert yet
                if (GetUserType() == 1) ResOrCusID = InsertRes(userID, LicenseNum.text, ResName.text, null, ResContactNum.text, ResAddress.text);
                if (GetUserType() == 2) ResOrCusID = InsertCus(userID, CusPhoneNum.text, CusEmail.text, CusName.text, null, null, CusAddress.text);

                if (ResOrCusID > 0)
                {
                    TagEditPage.SetActive(false);
                    gameObject.SetActive(false);
                    UserManager.SpawnUserCanvas(userID, username.text, GetUserType());
                    Destroy(gameObject);
                }
                if (ResOrCusID == 0)
                {
                    TipsManager.SpawnTips("Connection error");
                }
            }
            else TipsManager.SpawnTips("Connection error");
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool RegisterCheck(string username, string password, string confirm_password)
    {
        bool isOK = false;

        if (password.Length == 0)
        {
            TipsManager.SpawnTips("password connot be empty");
            return isOK;
        }
        if (username.Length > 12)
        {
            TipsManager.SpawnTips("username connot longer than 12 characters");
            return isOK;
        }
        if (password.Length > 20)
        {
            TipsManager.SpawnTips("password connot longer than 20 characters");
            return isOK;
        }
        if (password.Contains(" "))
        {
            TipsManager.SpawnTips("password connot contain space");
            return isOK;
        }
        // check passwords are the same
        if (!password.Equals(confirm_password))
        {
            TipsManager.SpawnTips("inconsistent passwords");
            return isOK;
        }
        // check illegal charactor

        // check exist
        if (MySqlManager.UsernameIsExist(username))
        {
            TipsManager.SpawnTips("username already existed");
            return isOK;
        }

        isOK = true;
        return isOK;
    }

    public bool ResInfoCheck(string LicenseNum,string ResName, string ResContactNum, string ResAddress)
    {
        bool isOK = false;

        // check license number
        int.TryParse(LicenseNum, out int licenseNum);
        if (!MySqlManager.ResLicenseCheck(licenseNum))
        {
            TipsManager.SpawnTips("Invalid license number");
            return isOK;
        }
        if (ResName.Length == 0)
        {
            TipsManager.SpawnTips("Eatery name cannot be empty");
            return isOK;
        }
        if (ResContactNum.Length == 0)
        {
            TipsManager.SpawnTips("Contact number cannot be empty");
            return isOK;
        }
        if (ResAddress.Length == 0)
        {
            TipsManager.SpawnTips("Eatery address cannot be empty");
            return isOK;
        }


        isOK = true;
        return isOK;
    }

    public bool CusInfoCheck(string CusPhoneNum, string CusName, string CusEmail, string CusAddress)
    {
        bool isOK = false;

        if (CusPhoneNum.Length == 0)
        {
            TipsManager.SpawnTips("Phone number cannot be empty");
            return isOK;
        }

        isOK = true;
        return isOK;
    }

    public bool TagCheck(string tag)
    {
        bool isOK = false;

        if (tag.Length == 0)
        {
            TipsManager.SpawnTips("Tag cannot be empty");
            return isOK;
        }

        isOK = true;
        return isOK;
    }
    int GetUserType()
    {
        int userType;
        if (ResToggle.isOn) userType = 1;
        else userType = 2;
        return userType;
    }

    public int InsertUser(string username, string password, int usertpye)
    {
        // insert user
        string query = "INSERT INTO `user` VALUES (NULL, '" + username + "', '" + password + "', " + usertpye + ");";
        int AffectRowNum = MySqlManager.Insert(query);

        // get user id
        int userID = 0;
        if (AffectRowNum > 0) userID = MySqlManager.GetUserIdOfLastRow();

        return userID;
    }

    public int InsertRes(int userID, string liscenseNum, string ResName, string Tag, string ResContactNum, string ResAddress)
    {
        string tag = "Spicy Food|Sweet Food|Fast Food|seafood|Rice|Fried|steamed|salad|";
        // insert res
        int.TryParse(liscenseNum, out int licenseNum_int);
        string query = "INSERT INTO `restaurant` VALUES (NULL,"+ userID+", "+ licenseNum_int + ", '"+ResName+"', NULL , NULL, '" + ResContactNum + "', NULL, '" + ResAddress + "', NULL,NULL, NULL,'"+ tag+"',NULL,NULL);";
        MySqlManager.Insert(query);

        // get res id
        int ResID = MySqlManager.GetResIdByUserID(userID);
        return ResID;
    }

    public int InsertCus(int userID, string CusPhoneNum, string CusEmail, string CusName, string FavorDishTag, string FavorResTag, string CusAddress)
    {
        string tag1 = "Spicy Food|Sweet Food|Fast Food|seafood|Rice|Fried|steamed|salad|";
        string tag2 = "";
        // insert cus
        string query = "INSERT INTO `customer` VALUES(NULL," + userID + ", '" + CusPhoneNum + "', '" + CusEmail + "', '" + CusName + "','"+ tag1 + "','"+ tag2 + "', NULL,'"+ CusAddress + "',NULL);";
        MySqlManager.Insert(query);

        // get cus id
        int CusID = MySqlManager.GetCusIdByUserID(userID);
        return CusID;
    }
}
