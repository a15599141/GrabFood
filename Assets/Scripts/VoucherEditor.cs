using UnityEngine;
using UnityEngine.UI;

public class VoucherEditor : MonoBehaviour
{
    public Text Res_Name;
    public Text Name;
    public Text Discount;
    public Text Stock;
    public InputField StockInputField;
    public Text Start_Time;
    public Text End_Time;
    public Text Start_Date;
    public Text End_Date;

    public static string Name_before; // voucher name before edit (use to allocate which voucher to update)

    public Button AddStockBtn;
    public Button MinusStockBtn;
    public Button StartTimeSelectBtn, EndTimeSelectBtn;
    public Button StartDateSelectBtn, EndDateSelectBtn;

    public Button InsertBtn;
    public Button UpdateBtn;
    public Button DeleteBtn;
    public Button CancelBtn;

    [Header("Type")]
    public Toggle EatInToggle;
    public Toggle TakeAwayToggle;
    public Toggle BothToggle;

    [Header("TimeSelector")]
    public GameObject TimeSelector;
    public GameObject HourViewVontent, MinViewVontent, SecViewVontent;
    public ToggleGroup HourViewToggleGroup, MinViewToggleGroup, SecViewToggleGroup;
    private GameObject TimeNumberTogglePrefab;
    public Button TimeSelectOkBtn;
    public static Text TimeSelectorHour, TimeSelectorMin, TimeSelectorSec;

    [Header("DateSelector")]
    public GameObject DateSelector;
    public GameObject MonthViewContect, DayViewContent;
    public ToggleGroup YearViewToggleGroup, MonthViewToggleGroup, DayViewToggleGroup;
    private GameObject DayNumberTogglePrefab;
    public Button DateSelectOkBtn;
    public static Text DateSelectorYear, DateSelectorMonth, DateSelectorDay;
    //public Toggle Jan, Feb, Mar, Apr, May, Jun, Jul, Aug, Sep, Oct, Nov, Dec; 

    // Start is called before the first frame update
    void Start()
    {
        Name_before = Name.text;
        TimeSelectorHour = transform.Find("TimeSelector/Hour").GetComponent<Text>();
        TimeSelectorMin = transform.Find("TimeSelector/Min").GetComponent<Text>();
        TimeSelectorSec = transform.Find("TimeSelector/Sec").GetComponent<Text>();

        DateSelectorYear = transform.Find("DateSelector/Year").GetComponent<Text>();
        DateSelectorMonth = transform.Find("DateSelector/Month").GetComponent<Text>();
        DateSelectorDay = transform.Find("DateSelector/Day").GetComponent<Text>();

        // spawn different numbers of days according to month
        for (int i = 0; i < 12; i++)
        {
            if ( i == 0 || i == 2 || i == 4 || i == 6 || i == 7 || i == 9 || i == 11 )
            {
                MonthViewContect.transform.GetChild(i).GetComponent<Toggle>().onValueChanged.AddListener(Spawn31DayNumberToggle);
            }
            if (i == 3 || i == 5 || i == 8 || i == 10)
            {
                MonthViewContect.transform.GetChild(i).GetComponent<Toggle>().onValueChanged.AddListener(Spawn30DayNumberToggle);
            }
            if (i == 1)
            {
                MonthViewContect.transform.GetChild(i).GetComponent<Toggle>().onValueChanged.AddListener(Spawn28DayNumberToggle);
            }
        }


        InsertBtn.onClick.AddListener(() =>
        {
            float.TryParse(Discount.text, out float discount);
            if (string.IsNullOrEmpty(Name.text)) TipsManager.SpawnTips("Voucher name cannot be empty");
            else if (MySqlManager.isVoucherNameExist(Name.text)) TipsManager.SpawnTips("Voucher name already existed");
            else if (discount > 1.0f) TipsManager.SpawnTips("Voucher discount must around 0 to 1");
            // insert voucher
            else
            {
                if (InsertVoucher() > 0)
                {
                    UpdateResMaxDiscount();
                    TipsManager.SpawnTips("Voucher added");
                    Restaurant.SpawnVouchers();
                    Destroy(gameObject);
                }
                else TipsManager.SpawnTips("Add voucher error");
            }
        });

        UpdateBtn.onClick.AddListener(() =>
        {
            float.TryParse(Discount.text, out float discount);
            if (string.IsNullOrEmpty(Name.text)) TipsManager.SpawnTips("Voucher name cannot be empty");
            else if (!Name.text.Equals(Name_before) && MySqlManager.isVoucherNameExist(Name.text)) TipsManager.SpawnTips("Voucher name already existed");
            else if (discount > 1.0f) TipsManager.SpawnTips("Voucher discount must around 0 to 1");
            // update voucher
            else
            {
                if (UpdateVoucher() > 0)
                {
                    UpdateResMaxDiscount();
                    TipsManager.SpawnTips("Voucher updated");
                    Restaurant.SpawnVouchers();
                    Destroy(gameObject);
                }
                else TipsManager.SpawnTips("Update voucher error");
            }
        });

        DeleteBtn.onClick.AddListener(() =>
        {
            // delete voucher
            if (DeleteVoucher() > 0)
            {
                UpdateResMaxDiscount();
                TipsManager.SpawnTips("Voucher deleted");
                Restaurant.SpawnVouchers();
                Destroy(gameObject);
            }
            else TipsManager.SpawnTips("Delete voucher error");
        });

        CancelBtn.onClick.AddListener(() =>
        {
            Destroy(gameObject);
        });

        AddStockBtn.onClick.AddListener(() =>
        {
            int stock = 0;
            int.TryParse(Stock.text, out stock);
            stock = stock + 1;
            StockInputField.text = stock.ToString();
        });
        MinusStockBtn.onClick.AddListener(() =>
        {
            int.TryParse(Stock.text, out int stock);
            stock = stock - 1;
            StockInputField.text = stock.ToString();
        });

        StartTimeSelectBtn.onClick.AddListener(() =>
        {
            SpawnTimeNumberToggle();
            EndTimeSelectBtn.interactable = false;
            TimeSelector.SetActive(true);
        });
        EndTimeSelectBtn.onClick.AddListener(() =>
        {
            SpawnTimeNumberToggle();
            StartTimeSelectBtn.interactable = false;
            TimeSelector.SetActive(true);
        });

        StartDateSelectBtn.onClick.AddListener(() =>
        {
            Spawn31DayNumberToggle(true);
            EndDateSelectBtn.interactable = false;
            DateSelector.SetActive(true);
        });
        EndDateSelectBtn.onClick.AddListener(() =>
        {
            Spawn31DayNumberToggle(true);
            StartDateSelectBtn.interactable = false;
            DateSelector.SetActive(true);
        });

        TimeSelectOkBtn.onClick.AddListener(() =>
        {
            if(StartTimeSelectBtn.IsInteractable())
            {
                Start_Time.text = TimeSelectorHour.text + ":" + TimeSelectorMin.text + ":" + TimeSelectorSec.text;
                EndTimeSelectBtn.interactable = true;
            }
            else
            {
                End_Time.text = TimeSelectorHour.text + ":" + TimeSelectorMin.text + ":" + TimeSelectorSec.text;
                StartTimeSelectBtn.interactable = true;
            }
            TimeSelector.SetActive(false);
        });
        DateSelectOkBtn.onClick.AddListener(() =>
        {
            if (StartDateSelectBtn.IsInteractable())
            {
                Start_Date.text = DateSelectorYear.text + "-" + DateSelectorMonth.text + "-" + DateSelectorDay.text;
                EndDateSelectBtn.interactable = true;
            }
            else
            {
                End_Date.text = DateSelectorYear.text + "-" + DateSelectorMonth.text + "-" + DateSelectorDay.text;
                StartDateSelectBtn.interactable = true;
            }
            DateSelector.SetActive(false);
        });
    }

    // Update is called once per frame
    void Update()
    {
        if (Stock.text.Length == 0 || Stock.text.Equals("0")) MinusStockBtn.interactable = false;
        else MinusStockBtn.interactable = true;
    }

    public void SpawnTimeNumberToggle()
    {
       TimeNumberTogglePrefab = (GameObject)Resources.Load("Prefabs/TimeNumberToggle");

        for (int i = 0; i < 24; i++)  // 24 hours
        {
            TimeNumberTogglePrefab.transform.Find("Label").GetComponent<Text>().text = i.ToString();
            TimeNumberTogglePrefab.transform.Find("TimeUnit").GetComponent<Text>().text = "h";
            TimeNumberTogglePrefab.GetComponent<Toggle>().group = HourViewToggleGroup;
            Instantiate(TimeNumberTogglePrefab, HourViewVontent.transform);
        }

        for (int i = 0; i<60; i++)  // 60 mins
        {
            TimeNumberTogglePrefab.transform.Find("Label").GetComponent<Text>().text = i.ToString();
            TimeNumberTogglePrefab.transform.Find("TimeUnit").GetComponent<Text>().text = "m";
            TimeNumberTogglePrefab.GetComponent<Toggle>().group = MinViewToggleGroup;
            Instantiate(TimeNumberTogglePrefab, MinViewVontent.transform);
        }

        for (int i = 0; i < 60; i++) // 60 seconds
        {
            TimeNumberTogglePrefab.transform.Find("Label").GetComponent<Text>().text = i.ToString();
            TimeNumberTogglePrefab.transform.Find("TimeUnit").GetComponent<Text>().text = "s";
            TimeNumberTogglePrefab.GetComponent<Toggle>().group = SecViewToggleGroup;
            Instantiate(TimeNumberTogglePrefab, SecViewVontent.transform);
        }
    }
    public void Spawn31DayNumberToggle(bool isOn) // fot months that have 31 days
    {
        if (isOn)
        {
            DayNumberTogglePrefab = (GameObject)Resources.Load("Prefabs/DayNumberToggle");
            // clear all days
            if (DayViewContent.transform.childCount > 0)
            {
                for (int i = 0; i < DayViewContent.transform.childCount; i++)
                {
                    Destroy(DayViewContent.transform.GetChild(i).gameObject);
                }
            }
            for (int i = 0; i < 31; i++)  
            {
                DayNumberTogglePrefab.transform.Find("Label").GetComponent<Text>().text = (i + 1).ToString();
                DayNumberTogglePrefab.GetComponent<Toggle>().group = DayViewToggleGroup;
                Instantiate(DayNumberTogglePrefab, DayViewContent.transform);
            }
        }
    } 
    public void Spawn30DayNumberToggle(bool isOn) // fot months that have 30 days
    { 
        if (isOn)
        {
            DayNumberTogglePrefab = (GameObject)Resources.Load("Prefabs/DayNumberToggle");
            // clear all days
            if (DayViewContent.transform.childCount > 0)
            {
                for (int i = 0; i < DayViewContent.transform.childCount; i++)
                {
                    Destroy(DayViewContent.transform.GetChild(i).gameObject);
                }
            }
            for (int i = 0; i < 30; i++) 
            {
                DayNumberTogglePrefab.transform.Find("Label").GetComponent<Text>().text = (i+1).ToString();
                DayNumberTogglePrefab.GetComponent<Toggle>().group = DayViewToggleGroup;
                Instantiate(DayNumberTogglePrefab, DayViewContent.transform);
            }
        }
    }
    public void Spawn28DayNumberToggle(bool isOn)  // fot months that have 28 days
    {
        if(isOn)
        {
            DayNumberTogglePrefab = (GameObject)Resources.Load("Prefabs/DayNumberToggle");
            // clear all days
            if (DayViewContent.transform.childCount > 0)
            {
                for (int i = 0; i < DayViewContent.transform.childCount; i++)
                {
                    Destroy(DayViewContent.transform.GetChild(i).gameObject);
                }
            }
            for (int i = 0; i < 28; i++) 
            {
                DayNumberTogglePrefab.transform.Find("Label").GetComponent<Text>().text = (i + 1).ToString();
                DayNumberTogglePrefab.GetComponent<Toggle>().group = DayViewToggleGroup;
                Instantiate(DayNumberTogglePrefab, DayViewContent.transform);
            }
        }

    }
   
    public int InsertVoucher()
    {
        int.TryParse(Stock.text, out int stock);
        if (string.IsNullOrEmpty(Stock.text)) stock = 0;
        float.TryParse(Discount.text, out float discount);
        if (string.IsNullOrEmpty(Discount.text)) discount = 1;
        int state = 0;

        int type;
        if (EatInToggle.isOn) type = 1;
        else if (TakeAwayToggle.isOn) type = 2;
        else type = 0;

        //check time and date
        string start_time, end_time, start_date, end_date;
        if (string.IsNullOrEmpty(Start_Time.text)) start_time = "00:00:00";
        else start_time = Start_Time.text;
        if (string.IsNullOrEmpty(End_Time.text)) end_time = "23:59:59";
        else end_time = End_Time.text;

        if (string.IsNullOrEmpty(Start_Date.text) || Start_Date.text.Equals("inf")) start_date = "NULL";
        else start_date = "'" + Start_Date.text + "'";
        if (string.IsNullOrEmpty(End_Date.text) || End_Date.text.Equals("inf")) end_date = "NULL";
        else end_date = "'" + End_Date.text + "'";

        string image = "";
        string query = "INSERT INTO `voucher` VALUES (NULL," + UserManager.ResOrCusID + ",'"+ Res_Name.text+ "', '" + Name.text + "', " + discount + "," + stock  + "," + state + ", " + type + ",'" + start_time + "', '"  + end_time + "'," + start_date +","+ end_date + ",'"+image + "');";

        if (discount > MySqlManager.GetAvailableMaxDiscountByResID(UserManager.ResOrCusID))
        {
            string query2 = "UPDATE `restaurant` SET `max_discount` = " + discount + " where res_id = " + UserManager.ResOrCusID;
            if (MySqlManager.Update(query2) > 0) return MySqlManager.Insert(query);
            else
            {
                TipsManager.SpawnTips("Connection error");
                return 0;
            }
        }

        return MySqlManager.Insert(query);
    }
    public int UpdateVoucher()
    {
        int.TryParse(Stock.text, out int stock);
        if (string.IsNullOrEmpty(Stock.text)) stock = 0;
        float.TryParse(Discount.text, out float discount);
        if (string.IsNullOrEmpty(Discount.text)) discount = 1;
        int type;
        if (EatInToggle.isOn) type = 1;
        else if (TakeAwayToggle.isOn) type = 2;
        else type = 0;
        string image = "";

        // time check
        string start_time, end_time, start_date, end_date;
        if (string.IsNullOrEmpty(Start_Time.text)) start_time = "00:00:00";
        else start_time = Start_Time.text;
        if (string.IsNullOrEmpty(End_Time.text)) end_time = "23:59:59";
        else end_time = End_Time.text;
       
        // date check
        if (string.IsNullOrEmpty(Start_Date.text) || Start_Date.text.Equals("inf")) start_date = "NULL";
        else start_date = "'"+Start_Date.text+"'";
        if (string.IsNullOrEmpty(End_Date.text) || End_Date.text.Equals("inf")) end_date = "NULL";
        else end_date = "'" + End_Date.text + "'";

        string query = "UPDATE `voucher` SET  `name` = '" + Name.text + "', `discount` = " + discount + ", `stock` = " + stock + ",  `type` = " + type + ", `start_time` = '" + start_time + "', `end_time` = '" + end_time + "', `start_date` = " + start_date + ", `end_date` = "+ end_date + ", `image` = '"+image+"' where res_id = " + UserManager.ResOrCusID + " and name = '" + Name_before + "'";

        if (discount > MySqlManager.GetAvailableMaxDiscountByResID(UserManager.ResOrCusID))
        {
            string query2 = "UPDATE `restaurant` SET `max_discount` = " + discount + " where res_id = "+ UserManager.ResOrCusID;
            if (MySqlManager.Update(query2) > 0) return MySqlManager.Update(query);
            else
            {
                TipsManager.SpawnTips("Connection error");
                return 0;
            }
        }
        
        return MySqlManager.Update(query);
    }
    public int DeleteVoucher()
    {
        string query = "DELETE FROM voucher where res_id = " + UserManager.ResOrCusID + " and name = '" + Name_before + "'";
        return MySqlManager.Delete(query);
    }

    public int UpdateResMaxDiscount()
    {
        float max_discount = MySqlManager.GetAvailableMaxDiscountByResID(UserManager.ResOrCusID);
        string query = "UPDATE `restaurant` SET `max_discount` = " + max_discount + " where res_id = " + UserManager.ResOrCusID;
        return MySqlManager.Update(query);
    }



}
