using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DishEditor : MonoBehaviour
{
    public Text Name;
    public Text Price;
    public Text Stock;
    public InputField StockInputField;
    public Text Description;
    public static ToggleGroup CuisineToggleGroup;

    public static string Name_before; // dish name before edit (use to allocate which dish to update)

    public static Text SelectCuisineBtnText;
    public Button SelectCuisineBtn;
    public Button AddStockBtn;
    public Button MinusStockBtn;

    public Button InsertBtn;
    public Button UpdateBtn;
    public Button DeleteBtn;
    public Button CancelBtn;
    public GameObject CuisineViewContent;
    
    [Header("ServeType")]
    public Toggle EatInToggle;
    public Toggle TakeAwayToggle;
    public Toggle BothToggle;
    // Start is called before the first frame update
    void Start()
    {
        Name_before = Name.text;
        CuisineToggleGroup = gameObject.transform.Find("CuisineView").GetComponent<ToggleGroup>();
        SelectCuisineBtnText = gameObject.transform.Find("SelectCuisineBtn/Text").GetComponentInChildren<Text>();

        InsertBtn.onClick.AddListener(() =>
        {
            if (string.IsNullOrEmpty(Name.text)) TipsManager.SpawnTips("Dish name cannot be empty");
            else if (MySqlManager.isDishNameExist(Name.text)) TipsManager.SpawnTips("Dish name already existed in menu");
            // insert dish
            else
            {
                if (InsertDish() > 0)
                {
                    TipsManager.SpawnTips("Dish added");
                    Restaurant.SpawnDishes(CUISINE: SelectCuisineBtnText.text);
                    Destroy(gameObject);
                }
                else TipsManager.SpawnTips("Add dish error");
                
            }
        });

        UpdateBtn.onClick.AddListener(() =>
        {
            if (string.IsNullOrEmpty(Name.text)) TipsManager.SpawnTips("Dish name cannot be empty");
            else if (!Name.text.Equals(Name_before) && MySqlManager.isDishNameExist(Name.text)) TipsManager.SpawnTips("Dish name already existed in menu");
            // update dish
            else
            {
                if (UpdateDish() > 0)
                {
                    TipsManager.SpawnTips("Dish updated");
                    Restaurant.SpawnDishes(CUISINE: SelectCuisineBtnText.text);
                    Destroy(gameObject);
                }
                else TipsManager.SpawnTips("Update dish error");
            }
        });

        DeleteBtn.onClick.AddListener(() =>
        {
            // delete dish
            if (DeleteDish() > 0)
                {
                    TipsManager.SpawnTips("Dish deleted");
                    Restaurant.SpawnDishes();
                    Destroy(gameObject);
                }
            else TipsManager.SpawnTips("Delete dish error");
            
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

        SelectCuisineBtn.onClick.AddListener(() =>
        {
            SpawnCuisine();
        });

    }

    // Update is called once per frame
    void Update()
    {
        if (Stock.text.Length == 0 || Stock.text.Equals("0")) MinusStockBtn.interactable = false;
        else MinusStockBtn.interactable = true;
    }

    public void SpawnCuisine()
    {
        //clear all eateries
        if (CuisineViewContent.transform.childCount > 0)
        {
            for (int i = 1; i < CuisineViewContent.transform.childCount; i++) // ignore the default cuisine "All"
            {
                Destroy(CuisineViewContent.transform.GetChild(i).gameObject);
            }
        }
        // read eateries from database and spawn on scene
        List<TagStruct> tagList = MySqlManager.GetAllTagByResID(UserManager.ResOrCusID);
        if (tagList.Count == 0) TipsManager.SpawnTips("No cuisine found");
        GameObject CuisinePrefab = (GameObject)Resources.Load("Prefabs/Cuisine");

        for (int i = 0; i < tagList.Count; i++)
        {
            CuisinePrefab.transform.Find("Label").GetComponent<Text>().text = tagList[i].name;
            CuisinePrefab.GetComponent<Toggle>().group = CuisineToggleGroup;
            Instantiate(CuisinePrefab, CuisineViewContent.transform);
        }
    }

    public int InsertDish()
    {
        int.TryParse(Stock.text, out int stock);
        if (string.IsNullOrEmpty(Stock.text)) stock = 0;
        float.TryParse(Price.text, out float price);
        if (string.IsNullOrEmpty(Price.text)) price = 0;
        int sale = 0;
        int state = 0;
        string tag="";
        string cuisine = SelectCuisineBtnText.text;
        int serveType;
        if(EatInToggle.isOn) serveType = 1;
        else if (TakeAwayToggle.isOn) serveType = 2;
        else serveType = 0;
        string image = "";
        string query = "INSERT INTO `dish` VALUES (NULL," + UserManager.ResOrCusID + ", '" + Name.text + "', " + price + ","+ stock + " ," + sale + "," + state + ", '"+tag+"','"+ cuisine + "',"+ serveType + ", '"+Description.text+ "','"+ image+"');";
        return MySqlManager.Insert(query);
    }

    public int UpdateDish()
    {
        int.TryParse(Stock.text, out int stock);
        if (string.IsNullOrEmpty(Stock.text)) stock = 0;
        float.TryParse(Price.text, out float price);
        if (string.IsNullOrEmpty(Price.text)) price = 0;
        // int state = 0; 
        string tag = "";
        string cuisine = SelectCuisineBtnText.text;
        int serveType;
        if (EatInToggle.isOn) serveType = 1;
        else if (TakeAwayToggle.isOn) serveType = 2;
        else serveType = 0;
        string query = "UPDATE `dish` SET  `name` = '" + Name.text + "', `price` = " + price + ", `stock` = " + stock + ",  `tag` = '" + tag + "', `cuisine` = '" + cuisine + "', `serve_type` = " + serveType + ", `description` = '" + Description.text + "', `image` = NULL where res_id = " + UserManager.ResOrCusID + " and name = '" + Name_before + "'";
        return MySqlManager.Update(query);
    }
    public int DeleteDish()
    {
        string query = "DELETE FROM dish where res_id = " + UserManager.ResOrCusID + " and name = '" + Name_before + "'";
        return MySqlManager.Delete(query);
    }
}

    