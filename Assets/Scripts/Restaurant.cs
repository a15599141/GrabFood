using MySqlX.XDevAPI.Relational;
using System;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using UnityEngine;
using UnityEngine.UI;

public class Restaurant : MonoBehaviour
{

    public static GameObject restaurant;
    public static string selected_cuisine_id, cuisineName_before;

    [Header("MenuPage")]
    public GameObject MenuPage;
    public Button AddDishBtn;
    public static GameObject CuisineViewContent;
    public static GameObject DishViewContent;
    public static ToggleGroup CuisineToggleGroup;

    [Header("CuisineEditor")]
    public static GameObject CuisineEditor;
    public static GameObject TipsForCuisineAll;
    public static InputField CuisineEditInputField;
    public static Button CuisineDeleteBtn, CuisineUpdateBtn;
    public static GameObject CuisineEditorAdd;
    public static Button CuisineAddBtn;
    public static InputField CuisineEditAddInputField;

    [Header("PromotionPage")]
    public GameObject PromotionPage;
    public Button AddVoucherBtn;
    public static ToggleGroup VoucherSortToggleGroup;
    public static GameObject VoucherViewContent;
    public Toggle VoucherSortByAllToggle, 
                  VoucherSortByStartTimeToggle, VoucherSortByEndTimeToggle, 
                  VoucherSortByStartDateToggle, VoucherSortByEndDateToggle, 
                  VoucherSortByDiscountToggle, VoucherSortByStockToggle;
    public Button SortAscendingBtn, SortDescendingBtn;

    [Header("Order")]
    public GameObject OrderPage;
    public static GameObject OrderViewContent;

    [Header("Eatery")]
    public GameObject EateryPage;
    public Button LogoutBtn;
    public static ToggleGroup EateryToggleGroup;
    public Button EateryProfileEditBtn, EateryProfileEditOkBtn;
    public Text Name, Rate, Sale, OpenTime, CloseTime, Contact, Postcode, Address, Description, Tag, Max_Discount;
    public InputField NameInputField, OpenTimeInputField, CloseTimeInputField, ContactInputField, PostcodeInputField, AddressInputField, DescriptionInputField;
    public static GameObject CommentViewContent;
    public Button CommentRefreshBtn;
    public GameObject PasswordChangePanel, SendFeedbackPanel, CancelAccountPanel;
    public InputField NewPasswordInputField, ConfirmNewPasswordInputField;
    public Button ResetPasswordBtn;
    public InputField FeedbackInputField;
    public Button SubmitFeedbackBtn;
    public InputField CancelAccountUsernameInputField, CancelAccountPasswordInputField;
    public Button CancelAccountConfirmFeedbackBtn;

    // Start is called before the first frame update
    void Start()
    {
        restaurant = gameObject;
        selected_cuisine_id = "";
        cuisineName_before = "";
        // initialize cuisine editor
        CuisineEditor = transform.Find("MenuPage/CuisineEditor").gameObject;
        TipsForCuisineAll = transform.Find("MenuPage/CuisineEditor/TipsForCuisineAll").gameObject;
        CuisineEditInputField = transform.Find("MenuPage/CuisineEditor/CuisineInputField").GetComponent<InputField>();
        CuisineDeleteBtn = transform.Find("MenuPage/CuisineEditor/DeleteBtn").GetComponent<Button>();
        CuisineUpdateBtn = transform.Find("MenuPage/CuisineEditor/UpdateBtn").GetComponent<Button>();

        // initialize cuisine editor (add)
        CuisineEditorAdd = transform.Find("MenuPage/CuisineEditorAdd").gameObject;
        CuisineEditAddInputField = transform.Find("MenuPage/CuisineEditorAdd/CuisineInputField").GetComponent<InputField>();
        CuisineAddBtn = transform.Find("MenuPage/CuisineEditorAdd/AddBtn").GetComponent<Button>();

        // initialize toggle group
        CuisineToggleGroup = transform.Find("ToggleGroups").Find("CuisineToggleGroup").GetComponent<ToggleGroup>();
        VoucherSortToggleGroup = transform.Find("ToggleGroups").Find("VoucherSortToggleGroup").GetComponent<ToggleGroup>();
        EateryToggleGroup = transform.Find("ToggleGroups").Find("EateryToggleGroup").GetComponent<ToggleGroup>();

        // initialize pages
        RefreshMenuPage(true);
        RefreshPromotionPage(true);
        RefreshOrderPage(true);
        RefreshEateryPage(true);
        AddDishBtn.onClick.AddListener(() =>
        {
            SpawnAddDishPage();
        });
        AddVoucherBtn.onClick.AddListener(() =>
        {
            SpawnAddVoucherPage();
        });
        LogoutBtn.onClick.AddListener(() =>
        {
            Logout();
        });
        EateryProfileEditBtn.onClick.AddListener(() =>
        {
            NameInputField.text = Name.text;
            OpenTimeInputField.text = OpenTime.text;
            CloseTimeInputField.text = CloseTime.text;
            ContactInputField.text = Contact.text;
            PostcodeInputField.text = Postcode.text;
            AddressInputField.text = Address.text;
            DescriptionInputField.text = Description.text;
        });
        EateryProfileEditOkBtn.onClick.AddListener(() =>
        {
            if (string.IsNullOrEmpty(NameInputField.text)) TipsManager.SpawnTips("Eatery name cannot be empty");
            else if(string.IsNullOrEmpty(ContactInputField.text)) TipsManager.SpawnTips("Contact cannot be empty");
            else if (string.IsNullOrEmpty(AddressInputField.text)) TipsManager.SpawnTips("Address cannot be empty");
            else
            {
                if (UpdateEateryProfile() > 0)
                {
                    NameInputField.gameObject.SetActive(false);
                    OpenTimeInputField.gameObject.SetActive(false);
                    CloseTimeInputField.gameObject.SetActive(false);
                    ContactInputField.gameObject.SetActive(false);
                    PostcodeInputField.gameObject.SetActive(false);
                    AddressInputField.gameObject.SetActive(false);
                    DescriptionInputField.gameObject.SetActive(false);
                    SpawnEateryProfile();
                    TipsManager.SpawnTips("Eatery profile updated");
                    EateryProfileEditBtn.gameObject.SetActive(true);
                    EateryProfileEditOkBtn.gameObject.SetActive(false);
                }
                else TipsManager.SpawnTips("Eatery profile updated error");
            }
        });

        //MenuToggle.onValueChanged.AddListener(RefreshMenuPage);
        //PromotionToggle.onValueChanged.AddListener(RefreshPromotionPage);
        //OrderToggle.onValueChanged.AddListener(RefreshOrderPage);
        //EateryToggle.onValueChanged.AddListener(RefreshEateryPage);)
        SortAscendingBtn.onClick.AddListener(() =>
        {
            int sort = 0;
            if (VoucherSortByAllToggle.isOn) sort = 0;
            if (VoucherSortByDiscountToggle.isOn) sort = 1;
            if (VoucherSortByStockToggle.isOn) sort = 2;
            if (VoucherSortByStartTimeToggle.isOn) sort = 3;
            if (VoucherSortByEndTimeToggle.isOn) sort = 4;
            if (VoucherSortByStartDateToggle.isOn) sort = 5;
            if (VoucherSortByEndDateToggle.isOn) sort = 6;

            int seq = 0;
            SpawnVouchers(sort, seq);

        });
        SortDescendingBtn.onClick.AddListener(() =>
        {
            int sort = 0;
            if (VoucherSortByAllToggle.isOn) sort = 0;
            if (VoucherSortByDiscountToggle.isOn) sort = 1;
            if (VoucherSortByStockToggle.isOn) sort = 2;
            if (VoucherSortByStartTimeToggle.isOn) sort = 3;
            if (VoucherSortByEndTimeToggle.isOn) sort = 4;
            if (VoucherSortByStartDateToggle.isOn) sort = 5;
            if (VoucherSortByEndDateToggle.isOn) sort = 6;

            int seq = 1;
            SpawnVouchers(sort, seq);
        });
        CommentRefreshBtn.onClick.AddListener(() =>
        {
            SpawnComments();
        });

        CuisineAddBtn.onClick.AddListener(() =>
        {
            string cuisine = CuisineEditAddInputField.text;
            if (string.IsNullOrEmpty(cuisine)) TipsManager.SpawnTips("Cuisine cannot be empty");
            else
            {
                int result = MySqlManager.CheckCuisineExsit(UserManager.ResOrCusID, CuisineEditAddInputField.text);
                if (result == 0) TipsManager.SpawnTips("Connection error");
                if (result == 1) TipsManager.SpawnTips("'"+cuisine+"' already in your menu");
                if (result == 2)
                {
                    if (InsertCuisine() > 0)
                    {
                        SpawnCuisines();
                        TipsManager.SpawnTips("Cuisine Added");
                    }
                    else TipsManager.SpawnTips("Connection error");
                }
            }
        });
        CuisineUpdateBtn.onClick.AddListener(() =>
        {
            string cuisine = CuisineEditInputField.text;
            if (string.IsNullOrEmpty(cuisine)) TipsManager.SpawnTips("Cuisine cannot be empty");
            else
            {
                int result = MySqlManager.CheckCuisineExsit(UserManager.ResOrCusID, CuisineEditInputField.text);
                if (result == 0) TipsManager.SpawnTips("Connection error");
                if (result == 1) TipsManager.SpawnTips("'" + cuisine + "' already in your menu");
                if (result == 2)
                {
                    if (UpdateCuisine() > 0)
                    {
                        RefreshMenuPage(true);
                        UpdateDishesWhenCuisineChange();
                        TipsManager.SpawnTips("Cuisine Updated");
                    }
                    else TipsManager.SpawnTips("Connection error");
                }
            }
        });
        CuisineDeleteBtn.onClick.AddListener(() =>
        {
            if (DeleteCuisine() > 0)
            {
                RefreshMenuPage(true);
                UpdateDishesWhenCuisineChange();
                TipsManager.SpawnTips("Cuisine Deleted");
            }
            else TipsManager.SpawnTips("Connection error");
        });

        ResetPasswordBtn.onClick.AddListener(() =>
        {
            if (string.IsNullOrEmpty(NewPasswordInputField.text) || string.IsNullOrEmpty(ConfirmNewPasswordInputField.text))
            {
                TipsManager.SpawnTips("Passwords cannot be empty");
            }
            else if (NewPasswordInputField.text.Contains(" ")|| ConfirmNewPasswordInputField.text.Contains(" "))
            {
                TipsManager.SpawnTips("Passwords cannot be empty");
            }
            else if (!NewPasswordInputField.text.Equals(ConfirmNewPasswordInputField.text))
            {
                TipsManager.SpawnTips("Inconsistent passwords");
            }
            else
            {
                if (UserManager.ResetPassword(NewPasswordInputField.text) > 0)
                {
                    NewPasswordInputField.text = "";
                    ConfirmNewPasswordInputField.text = "";
                    PasswordChangePanel.SetActive(false);
                    TipsManager.SpawnTips("Password Reset");
                }
                else TipsManager.SpawnTips("Connection Error");
            }        
        });

        SubmitFeedbackBtn.onClick.AddListener(() =>
        {
            if(string.IsNullOrEmpty(FeedbackInputField.text)) TipsManager.SpawnTips("Please tell us somethings");
            else
            {
                if (InsertFeedback() > 0)
                {
                    FeedbackInputField.text = "";
                    SendFeedbackPanel.SetActive(false);
                    TipsManager.SpawnTips("We have recieved you feedback, thanks!");
                }
                else TipsManager.SpawnTips("Connection Error");

            }
        });

        CancelAccountConfirmFeedbackBtn.onClick.AddListener(() =>
        {

            if (!CancelAccountUsernameInputField.text.Equals(UserManager.UserName)) TipsManager.SpawnTips("Incorrect Username");
            else
            {
                int result = MySqlManager.LoginCheck(UserManager.UserName, CancelAccountPasswordInputField.text);
                if (result == -1) TipsManager.SpawnTips("Connection Error");
                else if (result == 1) TipsManager.SpawnTips("Incorrect Password");
                else if (UserManager.DeleteUser() > 0)
                {
                    TipsManager.SpawnTips("Account Cancelled");
                    Logout();
                }
                else TipsManager.SpawnTips("Connection Error");
            }
        });

        VoucherSortByAllToggle.onValueChanged.AddListener(RefreshPromotionPage);
        VoucherSortByDiscountToggle.onValueChanged.AddListener(RefreshPromotionPage);
        VoucherSortByStockToggle.onValueChanged.AddListener(RefreshPromotionPage);
        VoucherSortByStartTimeToggle.onValueChanged.AddListener(RefreshPromotionPage);
        VoucherSortByEndTimeToggle.onValueChanged.AddListener(RefreshPromotionPage);
        VoucherSortByStartDateToggle.onValueChanged.AddListener(RefreshPromotionPage);
        VoucherSortByEndDateToggle.onValueChanged.AddListener(RefreshPromotionPage);
    }
    public void Logout()
    {
        GameObject LoginPagePrefab = (GameObject)Resources.Load("Prefabs/LoginPage");
        Instantiate(LoginPagePrefab);
        Destroy(gameObject);
    }

    public static void SpawnDishes(int SORT = 0, int SEQUENCE = 0, string CUISINE = "")
    {
        DishViewContent = restaurant.transform.Find("MenuPage/DishesView/Viewport/Content").gameObject;
        // clear all dish
        if (DishViewContent.transform.childCount > 0)
        {
            for (int i = 0; i < DishViewContent.transform.childCount; i++)
            {
                Destroy(DishViewContent.transform.GetChild(i).gameObject);
            }
        }
        // read dishes from database and spawn on scene
        List<DishStruct> dishList = MySqlManager.GetAllDishesByResID(UserManager.ResOrCusID, SORT, SEQUENCE, CUISINE);
        if (dishList.Count == 0) TipsManager.SpawnTips("No dish found");
        GameObject DishPrefab = (GameObject)Resources.Load("Prefabs/Dish");
        for (int i = 0; i < dishList.Count; i++)
        {
            DishPrefab.transform.Find("Dish_ID").GetComponent<Text>().text = dishList[i].dish_id;
            DishPrefab.transform.Find("ResID").GetComponent<Text>().text = dishList[i].res_id;
            DishPrefab.transform.Find("Name").GetComponent<Text>().text = dishList[i].name;

            if (string.IsNullOrEmpty(dishList[i].cuisine)) DishPrefab.transform.Find("Cuisine").GetComponent<Text>().text = "All";
            else DishPrefab.transform.Find("Cuisine").GetComponent<Text>().text = dishList[i].cuisine;

            if (string.IsNullOrEmpty(dishList[i].price)) DishPrefab.transform.Find("Price").GetComponent<Text>().text = "0";
            else DishPrefab.transform.Find("Price").GetComponent<Text>().text = dishList[i].price;

            if (string.IsNullOrEmpty(dishList[i].stock)) DishPrefab.transform.Find("Stock").GetComponent<Text>().text = "0";
            else DishPrefab.transform.Find("Stock").GetComponent<Text>().text = dishList[i].stock;

            if (string.IsNullOrEmpty(dishList[i].sale)) DishPrefab.transform.Find("Sale").GetComponent<Text>().text = "0";
            else DishPrefab.transform.Find("Sale").GetComponent<Text>().text = dishList[i].sale;

            if (string.IsNullOrEmpty(dishList[i].state) || dishList[i].state.Equals("0"))
            {
                DishPrefab.transform.Find("State").GetComponent<Text>().text = "0";
                DishPrefab.transform.Find("DishEditBtnGroup/DishOnBtn").gameObject.SetActive(true);
                DishPrefab.transform.Find("DishOffMask").gameObject.SetActive(true);
                DishPrefab.transform.Find("DishEditBtnGroup/DishOffBtn").gameObject.SetActive(false);
                DishPrefab.transform.Find("DishOnMask").gameObject.SetActive(false);
            }
            else
            {
                DishPrefab.transform.Find("State").GetComponent<Text>().text = dishList[i].state;
                DishPrefab.transform.Find("DishEditBtnGroup/DishOnBtn").gameObject.SetActive(false);
                DishPrefab.transform.Find("DishOffMask").gameObject.SetActive(false);
                DishPrefab.transform.Find("DishEditBtnGroup/DishOffBtn").gameObject.SetActive(true);
                DishPrefab.transform.Find("DishOnMask").gameObject.SetActive(true);
            }

            
            DishPrefab.transform.Find("Cuisine").GetComponent<Text>().text = dishList[i].tag;
            DishPrefab.transform.Find("ServeType").GetComponent<Text>().text = dishList[i].serve_type;
            DishPrefab.transform.Find("Description").GetComponent<Text>().text = dishList[i].description;
            //DishPrefab.transform.Find("Image").GetComponent<RawImage>().texture = dishList[i].image;
            Instantiate(DishPrefab, DishViewContent.transform);
        }
    }
    public static void SpawnCuisines()
    {
        CuisineViewContent = restaurant.transform.Find("MenuPage/CuisineView/Viewport/Content").gameObject;
        GameObject CuisinePrefab = (GameObject)Resources.Load("Prefabs/Cuisine");

        //clear all eateries
        if (CuisineViewContent.transform.childCount > 0)
        {
            for (int i = 1; i < CuisineViewContent.transform.childCount; i++) // ignore the the default cuisine "All"
            {
                Destroy(CuisineViewContent.transform.GetChild(i).gameObject);
            }
        }

        // read eateries from database and spawn on scene
        List<TagStruct> tagList = MySqlManager.GetAllTagByResID(UserManager.ResOrCusID);
        if (tagList.Count == 0) TipsManager.SpawnTips("No cuisine found");
        for (int i = 0; i < tagList.Count; i++)
        {
            CuisinePrefab.transform.Find("Label").GetComponent<Text>().text = tagList[i].name;
            CuisinePrefab.transform.Find("tag_id").GetComponent<Text>().text = tagList[i].tag_id;
            CuisinePrefab.GetComponent<Toggle>().group = CuisineToggleGroup;
            Instantiate(CuisinePrefab, CuisineViewContent.transform);
        }

        GameObject CuisineEditBtnPrefab = (GameObject)Resources.Load("Prefabs/CuisineEditBtn");
        Instantiate(CuisineEditBtnPrefab, CuisineViewContent.transform);

    }
    public static void SpawnVouchers(int SORT = 0, int SEQUENCE = 0)
    {
        VoucherViewContent = restaurant.transform.Find("PromotionPage/VoucherView/Viewport/Content").gameObject;
        // clear all vouchers
        if (VoucherViewContent.transform.childCount > 0)
        {
            for (int i = 0; i < VoucherViewContent.transform.childCount; i++)
            {
                Destroy(VoucherViewContent.transform.GetChild(i).gameObject);
            }
        }
        // read vouchers from database and spawn on scene
        List<VoucherStruct> voucherList = MySqlManager.GetAllVouchersByResID(UserManager.ResOrCusID, SORT, SEQUENCE);
        if (voucherList.Count == 0) TipsManager.SpawnTips("No voucher found");
        GameObject VoucherPrefab = (GameObject)Resources.Load("Prefabs/Voucher");
        for (int i = 0; i < voucherList.Count; i++)
        {
            VoucherPrefab.transform.Find("Purpose").GetComponent<Text>().text = "1";
            VoucherPrefab.transform.Find("Voucher_ID").GetComponent<Text>().text = voucherList[i].voucher_id;
            VoucherPrefab.transform.Find("Res_ID").GetComponent<Text>().text = voucherList[i].res_id;
            VoucherPrefab.transform.Find("Res_Name").GetComponent<Text>().text = voucherList[i].res_name;
            VoucherPrefab.transform.Find("Name").GetComponent<Text>().text = voucherList[i].name;

            if (string.IsNullOrEmpty(voucherList[i].discount)) VoucherPrefab.transform.Find("Discount").GetComponent<Text>().text = "1";
            else VoucherPrefab.transform.Find("Discount").GetComponent<Text>().text = voucherList[i].discount;

            if (string.IsNullOrEmpty(voucherList[i].stock)) VoucherPrefab.transform.Find("Stock").GetComponent<Text>().text = "0";
            else VoucherPrefab.transform.Find("Stock").GetComponent<Text>().text = voucherList[i].stock;

            if (string.IsNullOrEmpty(voucherList[i].start_time)) VoucherPrefab.transform.Find("StartTime").GetComponent<Text>().text = "00:00:00";
            else VoucherPrefab.transform.Find("StartTime").GetComponent<Text>().text = voucherList[i].start_time;
            if (string.IsNullOrEmpty(voucherList[i].end_time)) VoucherPrefab.transform.Find("EndTime").GetComponent<Text>().text = "23:59:59";
            else VoucherPrefab.transform.Find("EndTime").GetComponent<Text>().text = voucherList[i].end_time;

            if (string.IsNullOrEmpty(voucherList[i].start_date)) VoucherPrefab.transform.Find("StartDate").GetComponent<Text>().text = "inf";
            else VoucherPrefab.transform.Find("StartDate").GetComponent<Text>().text = voucherList[i].start_date;
            if (string.IsNullOrEmpty(voucherList[i].end_date)) VoucherPrefab.transform.Find("EndDate").GetComponent<Text>().text = "inf";
            else VoucherPrefab.transform.Find("EndDate").GetComponent<Text>().text = voucherList[i].end_date;


            if (string.IsNullOrEmpty(voucherList[i].state) || voucherList[i].state.Equals("0"))
            {
                VoucherPrefab.transform.Find("State").GetComponent<Text>().text = "0";
                VoucherPrefab.transform.Find("VoucherEditBtnGroup/VoucherOnBtn").gameObject.SetActive(true);
                VoucherPrefab.transform.Find("VoucherEditBtnGroup/VoucherOffMask").gameObject.SetActive(true);
                VoucherPrefab.transform.Find("VoucherEditBtnGroup/VoucherOffBtn").gameObject.SetActive(false);
                VoucherPrefab.transform.Find("VoucherEditBtnGroup/VoucherOnMask").gameObject.SetActive(false);
            }
            else
            {
                VoucherPrefab.transform.Find("State").GetComponent<Text>().text = voucherList[i].state;
                VoucherPrefab.transform.Find("VoucherEditBtnGroup/VoucherOnBtn").gameObject.SetActive(false);
                VoucherPrefab.transform.Find("VoucherEditBtnGroup/VoucherOffMask").gameObject.SetActive(false);
                VoucherPrefab.transform.Find("VoucherEditBtnGroup/VoucherOffBtn").gameObject.SetActive(true);
                VoucherPrefab.transform.Find("VoucherEditBtnGroup/VoucherOnMask").gameObject.SetActive(true);
            }

            VoucherPrefab.transform.Find("Type").GetComponent<Text>().text = voucherList[i].type;
            //VoucherPrefab.transform.Find("Image").GetComponent<RawImage>().texture = voucherList[i].image;
            Instantiate(VoucherPrefab, VoucherViewContent.transform);
        }
    }
    public void SpawnAddDishPage()
    {
        GameObject DishEditorPrefab = (GameObject)Resources.Load("Prefabs/DishEditor");

        DishEditorPrefab.transform.Find("DishEditorTitle").GetComponent<Text>().text = "Add Dish";
        DishEditorPrefab.transform.Find("DeleteBtn").gameObject.SetActive(false);
        DishEditorPrefab.transform.Find("UpdateBtn").gameObject.SetActive(false);
        DishEditorPrefab.transform.Find("InsertBtn").gameObject.SetActive(true);

        // clear all input
        DishEditorPrefab.transform.Find("NameInputField").GetComponent<InputField>().text = "";
        DishEditorPrefab.transform.Find("PriceInputField").GetComponent<InputField>().text = "";
        DishEditorPrefab.transform.Find("StockInputField").GetComponent<InputField>().text = "";
        DishEditorPrefab.transform.Find("DescriptionInputField").GetComponent<InputField>().text = "";
        DishEditorPrefab.transform.Find("ServeType").Find("EatInToggle").GetComponent<Toggle>().isOn = false;
        DishEditorPrefab.transform.Find("ServeType").Find("TakeAwayToggle").GetComponent<Toggle>().isOn = false;
        DishEditorPrefab.transform.Find("ServeType").Find("BothToggle").GetComponent<Toggle>().isOn = true;

        Instantiate(DishEditorPrefab);
    }
    public void SpawnAddVoucherPage()
    {
        GameObject VoucherEditorPrefab = (GameObject)Resources.Load("Prefabs/VoucherEditor");

        VoucherEditorPrefab.transform.Find("VoucherEditorTitle").GetComponent<Text>().text = "Add Voucher";
        VoucherEditorPrefab.transform.Find("DeleteBtn").gameObject.SetActive(false);
        VoucherEditorPrefab.transform.Find("UpdateBtn").gameObject.SetActive(false);
        VoucherEditorPrefab.transform.Find("InsertBtn").gameObject.SetActive(true);

        // clear all input
        VoucherEditorPrefab.transform.Find("Res_Name").GetComponent<Text>().text = Name.text;
        VoucherEditorPrefab.transform.Find("NameInputField").GetComponent<InputField>().text = "";
        VoucherEditorPrefab.transform.Find("DiscountInputField").GetComponent<InputField>().text = "";
        VoucherEditorPrefab.transform.Find("StockInputField").GetComponent<InputField>().text = "";
        VoucherEditorPrefab.transform.Find("NameInputField").GetComponent<InputField>().text = "";
        VoucherEditorPrefab.transform.Find("StartTimeSelectBtn").Find("StartTime").GetComponent<Text>().text = "00:00:00";
        VoucherEditorPrefab.transform.Find("EndTimeSelectBtn").Find("EndTime").GetComponent<Text>().text = "23:59:59";
        VoucherEditorPrefab.transform.Find("DateSelectBtn").Find("StartDate").GetComponent<Text>().text = null;
        VoucherEditorPrefab.transform.Find("DateSelectBtn").Find("EndDate").GetComponent<Text>().text = null;

        Instantiate(VoucherEditorPrefab);
    }
    public void SpawnEateryProfile()
    {
        RestaurantStruct res = MySqlManager.GetResByResID(UserManager.ResOrCusID);
        if (String.IsNullOrEmpty(res.res_id)) TipsManager.SpawnTips("Failed to load eatery profile");
        else
        {
            if (string.IsNullOrEmpty(res.name)) Name.text = "Eatery not found";
            else Name.text = res.name;

            if (string.IsNullOrEmpty(res.rate)) Rate.text = "5.0";
            else Rate.text = res.rate;

            if (string.IsNullOrEmpty(res.sale)) Sale.text = "0";
            else Sale.text = res.sale;

            if (string.IsNullOrEmpty(res.open_time)) OpenTime.text = "00:00:00";
            else OpenTime.text = res.open_time;

            if (string.IsNullOrEmpty(res.close_time)) CloseTime.text = "23:59:59";
            else CloseTime.text = res.close_time;

            Contact.text = res.contact_num;
            Postcode.text = res.postcode;
            Address.text = res.address;
            Description.text = res.description;
            Tag.text = res.tag;
        }
    }
    public static void SpawnOrders()
    {
        OrderViewContent = restaurant.transform.Find("OrderPage/OrderView/Viewport/Content").gameObject;
        // clear all dish
        if (OrderViewContent.transform.childCount > 0)
        {
            for (int i = 0; i < OrderViewContent.transform.childCount; i++)
            {
                Destroy(OrderViewContent.transform.GetChild(i).gameObject);
            }
        }
        // read dishes from database and spawn on scene
        List<OrderStruct> orderList = MySqlManager.GetAllOrderByResID(UserManager.ResOrCusID);
        if (orderList.Count == 0) TipsManager.SpawnTips("No comment found");
        GameObject OrderPrefab = (GameObject)Resources.Load("Prefabs/Order");
        for (int i = 0; i < orderList.Count; i++)
        {
            restaurant.transform.Find("EateryPage/CommentCount").GetComponent<Text>().text = "(" + orderList.Count.ToString() + ")";
            OrderPrefab.transform.Find("cus_name").GetComponent<Text>().text = orderList[i].cus_name;
            OrderPrefab.transform.Find("TotalPrice").GetComponent<Text>().text = orderList[i].total_price;
            OrderPrefab.transform.Find("TotalDish").GetComponent<Text>().text = orderList[i].total_amount;

            int.TryParse(orderList[i].state, out int state);
            if (state == 0) OrderPrefab.transform.Find("State").GetComponent<Text>().text = "Placed";
            else OrderPrefab.transform.Find("State").GetComponent<Text>().text = "Paid";


            Instantiate(OrderPrefab, OrderViewContent.transform);
        }
    }
    public static void SpawnComments()
    {
        CommentViewContent = restaurant.transform.Find("EateryPage/CommentView/Viewport/Content").gameObject;
        // clear all dish
        if (CommentViewContent.transform.childCount > 0)
        {
            for (int i = 0; i < CommentViewContent.transform.childCount; i++)
            {
                Destroy(CommentViewContent.transform.GetChild(i).gameObject);
            }
        }
        // read dishes from database and spawn on scene
        List<CommentStruct> commentList = MySqlManager.GetAllCommentsByResID(UserManager.ResOrCusID);
        if (commentList.Count == 0) TipsManager.SpawnTips("No comment found");
        GameObject CommentPrefab = (GameObject)Resources.Load("Prefabs/Comment");
        float total_rate = 0; // to compute average rate of the restaurant
        for (int i = 0; i < commentList.Count; i++)
        {
            restaurant.transform.Find("EateryPage/CommentCount").GetComponent<Text>().text = "("+commentList.Count.ToString()+")";
            CommentPrefab.transform.Find("Comment_ID").GetComponent<Text>().text = commentList[i].comment_id;
            CommentPrefab.transform.Find("Res_ID").GetComponent<Text>().text = commentList[i].res_id;
            CommentPrefab.transform.Find("Cus_ID").GetComponent<Text>().text = commentList[i].cus_id;
            CommentPrefab.transform.Find("Date").GetComponent<Text>().text = commentList[i].date;
            //CommentPrefab.transform.Find("Image").GetComponent<RawImage>().texture = commentList[i].image;

            if (string.IsNullOrEmpty(commentList[i].cus_name)) CommentPrefab.transform.Find("CusName").GetComponent<Text>().text = "Anonymous";
            else CommentPrefab.transform.Find("CusName").GetComponent<Text>().text = commentList[i].cus_name;

            //initialize comment rate and show stars
            if (string.IsNullOrEmpty(commentList[i].rate)) CommentPrefab.transform.Find("Rate").GetComponent<Text>().text = "5";
            else CommentPrefab.transform.Find("Rate").GetComponent<Text>().text = commentList[i].rate;
            int.TryParse(CommentPrefab.transform.Find("Rate").GetComponent<Text>().text, out int RateStarCount);
            CommentPrefab.transform.Find("Rate/Star1").gameObject.SetActive(false);
            CommentPrefab.transform.Find("Rate/Star2").gameObject.SetActive(false);
            CommentPrefab.transform.Find("Rate/Star3").gameObject.SetActive(false);
            CommentPrefab.transform.Find("Rate/Star4").gameObject.SetActive(false);
            CommentPrefab.transform.Find("Rate/Star5").gameObject.SetActive(false);
            if (RateStarCount > 0 ) CommentPrefab.transform.Find("Rate/Star1").gameObject.SetActive(true);
            if (RateStarCount > 1) CommentPrefab.transform.Find("Rate/Star2").gameObject.SetActive(true);
            if (RateStarCount > 2) CommentPrefab.transform.Find("Rate/Star3").gameObject.SetActive(true);
            if (RateStarCount > 3) CommentPrefab.transform.Find("Rate/Star4").gameObject.SetActive(true);
            if (RateStarCount > 4) CommentPrefab.transform.Find("Rate/Star5").gameObject.SetActive(true);
            float.TryParse(commentList[i].rate, out float rate);
            total_rate = total_rate + rate;

            if (string.IsNullOrEmpty(commentList[i].content)) CommentPrefab.transform.Find("Content").GetComponent<Text>().text = "Default comment";
            else CommentPrefab.transform.Find("Content").GetComponent<Text>().text = commentList[i].content;

            Instantiate(CommentPrefab, CommentViewContent.transform);
        }
        double avg_rate = total_rate / commentList.Count;
        UpdateRate(Math.Round(avg_rate, 2));
    }

    public void RefreshMenuPage(bool toggleIsOn)
    {
        if(toggleIsOn)
        {
            SpawnDishes();
            SpawnCuisines();
        }
    }
    public void RefreshPromotionPage(bool toggleIsOn)
    {
        if (toggleIsOn)
        {
            int sort = 0;
            //if (VoucherSortByAllToggle.isOn) sort = 0;
            if (VoucherSortByDiscountToggle.isOn) sort = 1;
            if (VoucherSortByStockToggle.isOn) sort = 2;
            if (VoucherSortByStartTimeToggle.isOn) sort = 3;
            if (VoucherSortByEndTimeToggle.isOn) sort = 4;
            if (VoucherSortByStartDateToggle.isOn) sort = 5;
            if (VoucherSortByEndDateToggle.isOn) sort = 6;
            int seq = 0;
            //if (SortDescendingBtn.IsActive()) seq = 1;
            if (SortAscendingBtn.IsActive()) seq = 1;
            SpawnVouchers(sort, seq);
        }
    }
    public void RefreshOrderPage(bool toggleIsOn)
    {
        if (toggleIsOn)
        {
            SpawnOrders();
        }
    }
    public void RefreshEateryPage(bool toggleIsOn)
    {
        if (toggleIsOn)
        {
            SpawnEateryProfile();
            SpawnComments();
        }
    }

    public int UpdateEateryProfile()
    {
        if (string.IsNullOrEmpty(OpenTimeInputField.text)) OpenTimeInputField.text = "00:00:00";
        if (string.IsNullOrEmpty(CloseTimeInputField.text)) CloseTimeInputField.text = "24:00:00";
        string query = "UPDATE `restaurant` SET  `name` = '"+NameInputField.text+ "', `contact_num` = '"+ContactInputField.text+ "', `postcode`= "+PostcodeInputField.text+",  `address`= '"+AddressInputField.text+ "',  `open_time` = '"+OpenTimeInputField.text+ "', `close_time` = '"+CloseTimeInputField.text+ "',  `description` = '"+ DescriptionInputField.text+ "'  WHERE `res_id` = " + UserManager.ResOrCusID;
        return MySqlManager.Update(query);
    }
    public static int UpdateRate(double avg_rate)
    {
        string query = "update restaurant set rate = " + avg_rate +" where res_id = "+ UserManager.ResOrCusID;
        return MySqlManager.Update(query);
    }
    public static int InsertCuisine()
    {
        string query = "INSERT INTO `tag` (`tag_id`, `res_id`, `name`, `type`) VALUES(NULL,"+UserManager.ResOrCusID+", '"+CuisineEditAddInputField.text+"', 0);";
        return MySqlManager.Insert(query);
    }
    public static int UpdateCuisine()
    {
        string query = "update `tag` set name = '" + CuisineEditInputField.text + "' where tag_id = " + selected_cuisine_id;
        return MySqlManager.Update(query);
    }

    public int DeleteCuisine()
    {
        string query = "DELETE FROM `tag` where res_id = " + UserManager.ResOrCusID + " and tag_id = " + selected_cuisine_id;
        return MySqlManager.Delete(query);
    }

    public int UpdateDishesWhenCuisineChange()
    {
        string query = "update dish set cuisine = '" + CuisineEditInputField.text + "' where res_id = "+ UserManager.ResOrCusID +" and cuisine = '" +cuisineName_before+"' ;";
        return MySqlManager.Update(query);
    }

    public int InsertFeedback()
    {
        string query = "Insert into `feedback` VALUES(NULL, " + UserManager.ResOrCusID +",'"+FeedbackInputField.text+"', '"+ DateTime.Now.ToString()+"')";
        return MySqlManager.Insert(query);
    }
}
