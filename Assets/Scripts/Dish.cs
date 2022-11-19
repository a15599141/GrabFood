using System;
using UnityEngine;
using UnityEngine.UI;

public class Dish : MonoBehaviour
{
    public Text Dish_ID;
    public Text ResID;
    public Text Name;
    public Text Price;
    public Text Stock;
    public Text Sale;
    public Text State;
    public Text Tag;
    public Text Cuisine;
    public Text ServeType;
    public Text Decription;
    //public RawImage DishImage;
    private GameObject DishEditorPrefab;
    private GameObject DishDetailPrefab;
    public Button EditBtn;
    public Button ViewDishDetailBtn;
    public Button DishOnBtn;
    public Button DishOffBtn;
    public GameObject DishEditBtnGroup;
    // Start is called before the first frame update
    void Start()
    {
        if (UserManager.UserType == 1)
        {
            ViewDishDetailBtn.gameObject.SetActive(false);
            DishEditBtnGroup.SetActive(true);
        }
        else
        {
            ViewDishDetailBtn.gameObject.SetActive(true);
            DishEditBtnGroup.SetActive(false);
        }

        EditBtn.onClick.AddListener(() =>
        {
            SpawnEditDishPage();
        });
        ViewDishDetailBtn.onClick.AddListener(() =>
        {
            SpawnDishDitailPage();
        });
        DishOnBtn.onClick.AddListener(() =>
        {
            if(UpdateDishToOn()>0)
            {
                State.text = "1";
                transform.Find("DishEditBtnGroup/DishOnBtn").gameObject.SetActive(false);
                transform.Find("DishOffMask").gameObject.SetActive(false);
                transform.Find("DishEditBtnGroup/DishOffBtn").gameObject.SetActive(true);
                transform.Find("DishOnMask").gameObject.SetActive(true);
            }
            else TipsManager.SpawnTips("Dish on sale error");
        });
        DishOffBtn.onClick.AddListener(() =>
        {
            if (UpdateDishToOff() > 0)
            {
                State.text = "0";
                transform.Find("DishEditBtnGroup/DishOnBtn").gameObject.SetActive(true);
                transform.Find("DishOffMask").gameObject.SetActive(true);
                transform.Find("DishEditBtnGroup/DishOffBtn").gameObject.SetActive(false);
                transform.Find("DishOnMask").gameObject.SetActive(false);
            }
            else TipsManager.SpawnTips("Dish off shelve error");
        });
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void SpawnEditDishPage() //pop up dish editor for eatery user to edit dish
    {
        DishEditorPrefab = (GameObject)Resources.Load("Prefabs/DishEditor");

        DishEditorPrefab.transform.Find("DishEditorTitle").GetComponent<Text>().text = "Edit Dish";

        DishEditorPrefab.transform.Find("DeleteBtn").gameObject.SetActive(true);
        DishEditorPrefab.transform.Find("UpdateBtn").gameObject.SetActive(true);
        DishEditorPrefab.transform.Find("InsertBtn").gameObject.SetActive(false);

        DishEditorPrefab.transform.Find("NameInputField").GetComponent<InputField>().text= Name.text;

        if (string.IsNullOrEmpty(Cuisine.text)) DishEditorPrefab.transform.Find("SelectCuisineBtn/Text").GetComponent<Text>().text = "All";
        else DishEditorPrefab.transform.Find("SelectCuisineBtn/Text").GetComponent<Text>().text = Cuisine.text;
        DishEditorPrefab.transform.Find("StockInputField").GetComponent<InputField>().text = Stock.text;
        DishEditorPrefab.transform.Find("PriceInputField").GetComponent<InputField>().text = Price.text;
        DishEditorPrefab.transform.Find("DescriptionInputField").GetComponent<InputField>().text = Decription.text;

        if (ServeType.text.Equals("1"))
        {
            DishEditorPrefab.transform.Find("ServeType").Find("BothToggle").GetComponent<Toggle>().isOn = false;
            DishEditorPrefab.transform.Find("ServeType").Find("TakeAwayToggle").GetComponent<Toggle>().isOn = false;
            DishEditorPrefab.transform.Find("ServeType").Find("EatInToggle").GetComponent<Toggle>().isOn = true;
        }
        if (ServeType.text.Equals("2"))
        {
            DishEditorPrefab.transform.Find("ServeType").Find("BothToggle").GetComponent<Toggle>().isOn = false;
            DishEditorPrefab.transform.Find("ServeType").Find("EatInToggle").GetComponent<Toggle>().isOn = false;
            DishEditorPrefab.transform.Find("ServeType").Find("TakeAwayToggle").GetComponent<Toggle>().isOn = true;
        }
        if (ServeType.text.Equals("0"))
        {
            DishEditorPrefab.transform.Find("ServeType").Find("TakeAwayToggle").GetComponent<Toggle>().isOn = false;
            DishEditorPrefab.transform.Find("ServeType").Find("EatInToggle").GetComponent<Toggle>().isOn = false;
            DishEditorPrefab.transform.Find("ServeType").Find("BothToggle").GetComponent<Toggle>().isOn = true;
        }

        Instantiate(DishEditorPrefab);
    }

    public void SpawnDishDitailPage() //pop up dish detail page for customer user to view dish details
    {
        DishDetailPrefab = (GameObject)Resources.Load("Prefabs/DishDetail");

        DishDetailPrefab.transform.Find("Dish_ID").GetComponent<Text>().text = Dish_ID.text;
        DishDetailPrefab.transform.Find("Name").GetComponent<Text>().text = Name.text;
        DishDetailPrefab.transform.Find("Sale").GetComponent<Text>().text = Sale.text;
        DishDetailPrefab.transform.Find("State").GetComponent<Text>().text = State.text;
        DishDetailPrefab.transform.Find("Stock").GetComponent<Text>().text = Stock.text;
        DishDetailPrefab.transform.Find("Price").GetComponent<Text>().text = Price.text;
        DishDetailPrefab.transform.Find("Tags").GetComponent<Text>().text = Tag.text;
        if (ServeType.text.Equals("1"))
        {
            DishDetailPrefab.transform.Find("ServeType").GetComponent<Text>().text = "Eat-in Only";
            DishDetailPrefab.transform.Find("ServeTypeImage").gameObject.SetActive(true);
        }
        if (ServeType.text.Equals("2"))
        {
            DishDetailPrefab.transform.Find("ServeType").GetComponent<Text>().text = "Take-out Only";
            DishDetailPrefab.transform.Find("ServeTypeImage").gameObject.SetActive(true);
        }
        if (ServeType.text.Equals("0")||String.IsNullOrEmpty(ServeType.text))
        {
            DishDetailPrefab.transform.Find("ServeType").GetComponent<Text>().text = "";
            DishDetailPrefab.transform.Find("ServeTypeImage").gameObject.SetActive(false);
        }

        Instantiate(DishDetailPrefab);
    }
    public int UpdateDishToOn()
    {
        string query = "UPDATE `dish` SET  `state` = 1 WHERE `dish_id` =" + Dish_ID.text;
        return MySqlManager.Update(query);
    }
    public int UpdateDishToOff()
    {
        string query = "UPDATE `dish` SET  `state` = 0 WHERE `dish_id` =" + Dish_ID.text;
        return MySqlManager.Update(query);
    }
}
