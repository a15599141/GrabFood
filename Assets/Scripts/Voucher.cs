using UnityEngine;
using UnityEngine.UI;

public class Voucher : MonoBehaviour
{
    public Text Voucher_ID;
    public Text Res_ID;
    public Text Res_Name;
    public Text Name;
    public Text Discount;
    public Text Stock;
    public Text State;
    public Text Type;
    public Text StartTime;
    public Text EndTime;
    public Text StartDate;
    public Text EndDate;

    public Text Purpose; // 1 for res edit, 2 for cus view, 3 for cus collect
    //public RawImage VoucherImage;
    public GameObject StockTitle;
    private GameObject VoucherEditorPrefab;
    private GameObject VoucherCodePrefab;

    public GameObject VoucherEditBtnGroup;
    public Button EditBtn;
    public Button VoucherOnBtn;
    public Button VoucherOffBtn;
    public Button ViewVoucherCodeBtn;
    public Button CollectBtn;
    // Start is called before the first frame update
    void Start()
    {
        if (Purpose.text.Equals("1"))  // 1 for res edit
        {
            ViewVoucherCodeBtn.gameObject.SetActive(false);
            VoucherEditBtnGroup.SetActive(true);
            Res_Name.gameObject.SetActive(false);
            Name.gameObject.SetActive(true);
            Stock.gameObject.SetActive(true);
            StockTitle.SetActive(true);
        }
        else if(Purpose.text.Equals("2")) //  2 for cus view
        {
            ViewVoucherCodeBtn.gameObject.SetActive(true);
            VoucherEditBtnGroup.SetActive(false);
            Res_Name.gameObject.SetActive(true);
            Name.gameObject.SetActive(true);
            Stock.gameObject.SetActive(false);
            StockTitle.SetActive(false);
        }
        else if(Purpose.text.Equals("3")) // 3 for cus collect
        {
            ViewVoucherCodeBtn.gameObject.SetActive(false);
            VoucherEditBtnGroup.SetActive(false);
            Res_Name.gameObject.SetActive(false);
            Name.gameObject.SetActive(true);
            Stock.gameObject.SetActive(true);
            StockTitle.SetActive(true);
            CollectBtn.gameObject.SetActive(true);
            int.TryParse(Voucher_ID.text, out int voucherID);
            if (Stock.text.Equals("0"))
            {
                CollectBtn.transform.Find("Text").GetComponent<Text>().text = "Sold out";
                CollectBtn.interactable = false;
            }
            else if (MySqlManager.isCusAlreadyOwnVoucher(voucherID))
            {
                CollectBtn.transform.Find("Text").GetComponent<Text>().text = "Collected";
                CollectBtn.interactable = false;
            }
        }
        else
        {
            gameObject.SetActive(false);
        }

        EditBtn.onClick.AddListener(() =>
        {
            SpawnEditVoucherPage();
        });
        VoucherOnBtn.onClick.AddListener(() =>
        {
            if (UpdateVoucherToOn() > 0)
            {
                UpdateResMaxDiscount();
                State.text = "1";
                transform.Find("VoucherEditBtnGroup/VoucherOnBtn").gameObject.SetActive(false);
                transform.Find("VoucherEditBtnGroup/VoucherOffMask").gameObject.SetActive(false);
                transform.Find("VoucherEditBtnGroup/VoucherOffBtn").gameObject.SetActive(true);
                transform.Find("VoucherEditBtnGroup/VoucherOnMask").gameObject.SetActive(true);
            }
            else TipsManager.SpawnTips("Voucher on sale error");
        });
        VoucherOffBtn.onClick.AddListener(() =>
        {
            if (UpdateVoucherToOff() > 0)
            {
                UpdateResMaxDiscount();
                State.text = "0";
                transform.Find("VoucherEditBtnGroup/VoucherOnBtn").gameObject.SetActive(true);
                transform.Find("VoucherEditBtnGroup/VoucherOffMask").gameObject.SetActive(true);
                transform.Find("VoucherEditBtnGroup/VoucherOffBtn").gameObject.SetActive(false);
                transform.Find("VoucherEditBtnGroup/VoucherOnMask").gameObject.SetActive(false);
            }
            else TipsManager.SpawnTips("Voucher off shelve error");
        });
        ViewVoucherCodeBtn.onClick.AddListener(() =>
        {
            SpawnVoucherCodePage();
        });
        CollectBtn.onClick.AddListener(() =>
        {
            if (InsertCusVoucher()>0 )
            {
                UpdateVoucherStock(-1); // voucher stock -1
                CollectBtn.transform.Find("Text").GetComponent<Text>().text = "Collected";
                CollectBtn.interactable = false;
                Customer.SpawnMyVouchers();
                Customer.SpawnCollectVouchers();
                TipsManager.SpawnTips("Voucher collected");
            }
            else TipsManager.SpawnTips("Connection error");
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnEditVoucherPage() // pop up voucher editor any sync the voucher data to voucher editor
    {
        VoucherEditorPrefab = (GameObject)Resources.Load("Prefabs/VoucherEditor");

        VoucherEditorPrefab.transform.Find("Res_Name").GetComponent<Text>().text = Res_Name.text;
        VoucherEditorPrefab.transform.Find("VoucherEditorTitle").GetComponent<Text>().text = "Edit Voucher";

        VoucherEditorPrefab.transform.Find("DeleteBtn").gameObject.SetActive(true);
        VoucherEditorPrefab.transform.Find("UpdateBtn").gameObject.SetActive(true);
        VoucherEditorPrefab.transform.Find("InsertBtn").gameObject.SetActive(false);

        VoucherEditorPrefab.transform.Find("NameInputField").GetComponent<InputField>().text = Name.text;
        VoucherEditorPrefab.transform.Find("DiscountInputField").GetComponent<InputField>().text = Discount.text;
        VoucherEditorPrefab.transform.Find("StockInputField").GetComponent<InputField>().text = Stock.text;

        VoucherEditorPrefab.transform.Find("StartTimeSelectBtn").Find("StartTime").GetComponent<Text>().text = StartTime.text;
        VoucherEditorPrefab.transform.Find("EndTimeSelectBtn").Find("EndTime").GetComponent<Text>().text = EndTime.text;
        VoucherEditorPrefab.transform.Find("StartDateSelectBtn").Find("StartDate").GetComponent<Text>().text = StartDate.text;
        VoucherEditorPrefab.transform.Find("EndDateSelectBtn").Find("EndDate").GetComponent<Text>().text = EndDate.text;

        if (Type.text.Equals("1"))
        {
            VoucherEditorPrefab.transform.Find("Type").Find("BothToggle").GetComponent<Toggle>().isOn = false;
            VoucherEditorPrefab.transform.Find("Type").Find("TakeAwayToggle").GetComponent<Toggle>().isOn = false;
            VoucherEditorPrefab.transform.Find("Type").Find("EatInToggle").GetComponent<Toggle>().isOn = true;
        }
        if (Type.text.Equals("2"))
        {
            VoucherEditorPrefab.transform.Find("Type").Find("BothToggle").GetComponent<Toggle>().isOn = false;
            VoucherEditorPrefab.transform.Find("Type").Find("EatInToggle").GetComponent<Toggle>().isOn = false;
            VoucherEditorPrefab.transform.Find("Type").Find("TakeAwayToggle").GetComponent<Toggle>().isOn = true;
        }
        if (Type.text.Equals("0"))
        {
            VoucherEditorPrefab.transform.Find("Type").Find("TakeAwayToggle").GetComponent<Toggle>().isOn = false;
            VoucherEditorPrefab.transform.Find("Type").Find("EatInToggle").GetComponent<Toggle>().isOn = false;
            VoucherEditorPrefab.transform.Find("Type").Find("BothToggle").GetComponent<Toggle>().isOn = true;
        }


        Instantiate(VoucherEditorPrefab);
    }
    public void SpawnVoucherCodePage()
    {
        VoucherCodePrefab = (GameObject)Resources.Load("Prefabs/VoucherCode");

        Instantiate(VoucherCodePrefab);
    }
    public int UpdateVoucherToOn()
    {
        string query = "UPDATE `voucher` SET  `state` = 1 WHERE `voucher_ID` =" + Voucher_ID.text;
        return MySqlManager.Update(query);
    }
    public int UpdateVoucherToOff()
    {
        string query = "UPDATE `voucher` SET  `state` = 0 WHERE `voucher_ID` =" + Voucher_ID.text;
        return MySqlManager.Update(query);
    }

    public int UpdateResMaxDiscount()
    {
        float max_discount = MySqlManager.GetAvailableMaxDiscountByResID(UserManager.ResOrCusID);
        string query = "UPDATE `restaurant` SET `max_discount` = " + max_discount + " where res_id = " + UserManager.ResOrCusID;
        return MySqlManager.Update(query);
    }
    public int InsertCusVoucher()
    {
        string startdate, enddate;
        if (StartDate.text.Equals("inf")) startdate = "NULL";
        else startdate = "'" + StartDate.text + "'";
        if (EndDate.text.Equals("inf")) enddate = "NULL";
        else enddate = "'"+EndDate.text+"'";
        int state = 0; // initial state is unused
        string query = "INSERT INTO `voucher_list` VALUES (NULL, "+ Voucher_ID.text+ ", "+UserManager.ResOrCusID+", "+ Res_ID.text+", '"+Res_Name.text+"', '"+Name.text+"',"+Discount.text+", "+Type.text+","+ state + ", '"+StartTime.text+"', '"+EndTime.text+"', "+startdate+", "+enddate+");";
        return MySqlManager.Insert(query);
    }
    public int UpdateVoucherStock(int changeValue)
    {
        int.TryParse(Stock.text, out int stock);
        stock = stock + changeValue;
        if(stock<0) stock = 0;
        string query = "UPDATE `voucher` SET `stock` = " + stock + " where voucher_id = " + Voucher_ID.text;
        return MySqlManager.Update(query);
    }

}
