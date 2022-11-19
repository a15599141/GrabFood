using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Customer : MonoBehaviour
{
    public static GameObject customer;
    public static int selected_res_id;
    public static int placed_order_res_id;

    [Header("EateryPage")]
    public static GameObject EateryViewContent;
    public static GameObject EateryDishesViewContent;
    public static ToggleGroup CuisineToggleGroup;
    public static GameObject CuisineViewContent;
    public static GameObject EateryDetail;
    public Button EateryDetailBackBtn;
    public static Text EateryDetailName;
    public static Text EateryDetailProfileName;
    public static GameObject EateryCommentViewContent;
    public static GameObject MyCommentViewContent;
    public static GameObject MyVoucherViewContent;
    public GameObject VoucherCollectView;
    public static GameObject CollectVoucherViewContent;
    public static Text Name, Rate, Sale, OpenTime, CloseTime, Contact, Postcode, Address, Description, Tag;
    public Button VoucherCollectBtn;
    public Button CloseVoucherCollectBtn;
    public static GameObject ShoppingCartViewContent;
    public static Text ShoppingCartTotalPrice, ShoppingCartTotalAmount;
    public Button PlaceOrderBtn, PayOrderBtn;
    public Text OrderDiscount, OrderDishIDs;
    public static List<int> OrderDishIDsList, OrderDishAmountsList, OrderDishCurrentStock;
    public static Text CurrentPlacedOrderID;
    public GameObject PayFinishedView;
    public GameObject PayFinishedRate;
    public Text PayFinishedComment;
    public Button CommentSubmitBtn;
    public InputField CommentInputField;
    public Button EateryCommentRefreshBtn;

    [Header("MyVoucherPage")]
    public static ToggleGroup VoucherSortToggleGroup;
    public Toggle VoucherSortByAllToggle,
              VoucherSortByStartTimeToggle, VoucherSortByEndTimeToggle,
              VoucherSortByStartDateToggle, VoucherSortByEndDateToggle,
              VoucherSortByDiscountToggle, VoucherSortByTypeToggle;
    public Button SortAscendingBtn, SortDescendingBtn;

    [Header("MePage")]
    public Text MyName, MyPhone, MyEmail, MyPostcode, MyAddress, MyFavorRes, MyFavorDish;
    public Text MyImage;
    public Button MyProfileEditBtn, MyProfileEditOkBtn;
    public InputField MyNameInputField, MyPhoneInputField, MyEmailInputField, MyPostcodeInputField, MyAddressInputField, MyFavorResInputField, MyFavorDishInputField;
    public Button LogoutBtn;
    public static ToggleGroup MeToggleGroup;
    public Button MyCommentRefreshBtn;
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
        ObjectInitialize();

        RefreshEateryPage(true);
        RefreshMyVoucherPage(true);
        RefreshMyPage(true);
        LogoutBtn.onClick.AddListener(() =>
        {
            Logout();
        });
        EateryDetailBackBtn.onClick.AddListener(() =>
        {
            // clear cart
            if(string.IsNullOrEmpty(CurrentPlacedOrderID.text))
            {
                for (int i = 0; i < ShoppingCartViewContent.transform.childCount; i++)
                {
                    Destroy(ShoppingCartViewContent.transform.GetChild(i).gameObject);
                }
            }
            EateryDetail.SetActive(false);
        });
        SortAscendingBtn.onClick.AddListener(() =>
        {
            int sort = 0;
            if (VoucherSortByAllToggle.isOn) sort = 0;
            if (VoucherSortByDiscountToggle.isOn) sort = 1;
            if (VoucherSortByTypeToggle.isOn) sort = 2;
            if (VoucherSortByStartTimeToggle.isOn) sort = 3;
            if (VoucherSortByEndTimeToggle.isOn) sort = 4;
            if (VoucherSortByStartDateToggle.isOn) sort = 5;
            if (VoucherSortByEndDateToggle.isOn) sort = 6;

            int seq = 0;
            SpawnMyVouchers(sort, seq);

        });
        SortDescendingBtn.onClick.AddListener(() =>
        {
            int sort = 0;
            if (VoucherSortByAllToggle.isOn) sort = 0;
            if (VoucherSortByDiscountToggle.isOn) sort = 1;
            if (VoucherSortByTypeToggle.isOn) sort = 2;
            if (VoucherSortByStartTimeToggle.isOn) sort = 3;
            if (VoucherSortByEndTimeToggle.isOn) sort = 4;
            if (VoucherSortByStartDateToggle.isOn) sort = 5;
            if (VoucherSortByEndDateToggle.isOn) sort = 6;

            int seq = 1;
            SpawnMyVouchers(sort, seq);
        });
        VoucherCollectBtn.onClick.AddListener(() =>
        {
            SpawnCollectVouchers();
            VoucherCollectView.SetActive(true);
        });
        CloseVoucherCollectBtn.onClick.AddListener(() =>
        {
            VoucherCollectView.SetActive(false);
        });

        MyProfileEditBtn.onClick.AddListener(() =>
        {
            MyNameInputField.text = MyName.text;
            MyPhoneInputField.text = MyPhone.text;
            MyEmailInputField.text = MyEmail.text;
            MyPostcodeInputField.text = MyPostcode.text;
            MyAddressInputField.text = MyAddress.text;
            MyFavorResInputField.text = MyFavorRes.text;
            MyFavorDishInputField.text = MyFavorDish.text;
        });
        MyProfileEditOkBtn.onClick.AddListener(() =>
        {
            if (string.IsNullOrEmpty(MyNameInputField.text)) TipsManager.SpawnTips("Name cannot be empty");
            else if (string.IsNullOrEmpty(MyPhoneInputField.text)) TipsManager.SpawnTips("Phone number cannot be empty");
            else if (string.IsNullOrEmpty(MyPostcodeInputField.text)) TipsManager.SpawnTips("Postcode cannot be empty");
            else if (string.IsNullOrEmpty(MyAddressInputField.text)) TipsManager.SpawnTips("Address cannot be empty");
            else
            {
                if (UpdateMyProfile() > 0)
                {
                    MyNameInputField.gameObject.SetActive(false);
                    MyPhoneInputField.gameObject.SetActive(false);
                    MyEmailInputField.gameObject.SetActive(false);
                    MyPostcodeInputField.gameObject.SetActive(false);
                    MyAddressInputField.gameObject.SetActive(false);
                    MyFavorResInputField.gameObject.SetActive(false);
                    MyFavorDishInputField.gameObject.SetActive(false);
                    SpawnMyProfile();
                    TipsManager.SpawnTips("My profile updated");
                    MyProfileEditBtn.gameObject.SetActive(true);
                    MyProfileEditOkBtn.gameObject.SetActive(false);
                }
                else TipsManager.SpawnTips("My profile updated error");
            }
        });

        PlaceOrderBtn.onClick.AddListener(() =>
        {
            if (ShoppingCartViewContent.transform.childCount > 0)
            {
                int correctOrderDishCount = 0;
                for (int i = 0; i < ShoppingCartViewContent.transform.childCount; i++)
                {
                    int.TryParse(ShoppingCartViewContent.transform.GetChild(i).Find("dish_id").GetComponent<Text>().text, out int dish_id);
                    string dishName = ShoppingCartViewContent.transform.GetChild(i).Find("Name").GetComponent<Text>().text;
                    int.TryParse(ShoppingCartViewContent.transform.GetChild(i).Find("Amount").GetComponent<Text>().text, out int amount);
                    int current_stock = MySqlManager.GetDishStockByID(dish_id);
                    OrderDishCurrentStock.Add(current_stock);
                    if (current_stock<=0)
                    {
                        TipsManager.SpawnTips("'" + dishName + "' sold out");
                        ShoppingCartViewContent.transform.GetChild(i).Find("Stock").GetComponent<Text>().text = "0";
                    }
                    else if (MySqlManager.DishIsOffShelved(dish_id))
                    {
                        TipsManager.SpawnTips("'" + dishName + "' is off shelved");
                    }
                    else if (amount > current_stock)
                    {
                         TipsManager.SpawnTips("'" + dishName + "' stock not enough, auto adjusted");
                         ShoppingCartViewContent.transform.GetChild(i).Find("Amount").GetComponent<Text>().text = current_stock.ToString();
                         OrderDishAmountsList[OrderDishIDsList.IndexOf(dish_id)] = current_stock;
                    }
                    else
                    {
                        correctOrderDishCount++;
                        Debug.Log("dish placed into order!");
                    }
                }
                if (correctOrderDishCount == ShoppingCartViewContent.transform.childCount)
                {
                    if (InsertOrder() > 0)
                    {
                        TipsManager.SpawnTips("Order placed£¡");
                        placed_order_res_id = selected_res_id;
                        UpdateDishStockAndSale();
                        UpdateResSale();
                        int currentPlacedOrderID = MySqlManager.GetCurrentPlacedOrderID(selected_res_id, UserManager.ResOrCusID);
                        if (currentPlacedOrderID > 0)
                        {
                            CurrentPlacedOrderID.text = currentPlacedOrderID.ToString();
                        }
                        PlaceOrderBtn.gameObject.SetActive(false);
                        PayOrderBtn.gameObject.SetActive(true);
                    }
                    else TipsManager.SpawnTips("Connection error");
                }
            }
            else TipsManager.SpawnTips("Your cart is empty");
        });
        PayOrderBtn.onClick.AddListener(() =>
        {
            Debug.Log("Try pay");
            int.TryParse(CurrentPlacedOrderID.text, out int orderID);
            if (UpdateOrderState(orderID) > 0)
            {
                CurrentPlacedOrderID.text = "";
                ShoppingCartTotalAmount.text = "0";
                ShoppingCartTotalPrice.text = "0";
                OrderDishIDsList.Clear();
                OrderDishAmountsList.Clear();
                OrderDishCurrentStock.Clear();
                for (int i = 0; i < ShoppingCartViewContent.transform.childCount; i++)
                {
                    Destroy(ShoppingCartViewContent.transform.GetChild(i).gameObject);
                }
                PlaceOrderBtn.gameObject.SetActive(true);
                PayOrderBtn.gameObject.SetActive(false);
                PayFinishedView.gameObject.SetActive(true);
                SpawnEateryDishes();

            }
            else TipsManager.SpawnTips("Connection error");
        });
        CommentSubmitBtn.onClick.AddListener(() =>
        {
            int rate = 0;
            if (PayFinishedRate.transform.Find("Star1").gameObject.active) rate++;
            if (PayFinishedRate.transform.Find("Star2").gameObject.active) rate++;
            if (PayFinishedRate.transform.Find("Star3").gameObject.active) rate++;
            if (PayFinishedRate.transform.Find("Star4").gameObject.active) rate++;
            if (PayFinishedRate.transform.Find("Star5").gameObject.active) rate++;
            PayFinishedRate.GetComponent<Text>().text = rate.ToString();
            if (InsertComment()>0)
            {
                CommentInputField.text = "";
                PayFinishedView.gameObject.SetActive(false);
                TipsManager.SpawnTips("Review submitted");
                SpawnEateryComments();
            }
            else TipsManager.SpawnTips("Connection error");
        });

        EateryCommentRefreshBtn.onClick.AddListener(() =>
        {
            SpawnEateryComments();
        });
        MyCommentRefreshBtn.onClick.AddListener(() =>
        {
            SpawnMyComments();
        });

        ResetPasswordBtn.onClick.AddListener(() =>
        {
            if (string.IsNullOrEmpty(NewPasswordInputField.text) || string.IsNullOrEmpty(ConfirmNewPasswordInputField.text))
            {
                TipsManager.SpawnTips("Passwords cannot be empty");
            }
            else if (NewPasswordInputField.text.Contains(" ") || ConfirmNewPasswordInputField.text.Contains(" "))
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
            if (string.IsNullOrEmpty(FeedbackInputField.text)) TipsManager.SpawnTips("Please tell us somethings");
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

        VoucherSortByAllToggle.onValueChanged.AddListener(RefreshMyVoucherPage);
        VoucherSortByDiscountToggle.onValueChanged.AddListener(RefreshMyVoucherPage);
        VoucherSortByTypeToggle.onValueChanged.AddListener(RefreshMyVoucherPage);
        VoucherSortByStartTimeToggle.onValueChanged.AddListener(RefreshMyVoucherPage);
        VoucherSortByEndTimeToggle.onValueChanged.AddListener(RefreshMyVoucherPage);
        VoucherSortByStartDateToggle.onValueChanged.AddListener(RefreshMyVoucherPage);
        VoucherSortByEndDateToggle.onValueChanged.AddListener(RefreshMyVoucherPage);
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void Logout()
    {
        GameObject LoginPagePrefab = (GameObject)Resources.Load("Prefabs/LoginPage");
        Instantiate(LoginPagePrefab);
        Destroy(gameObject);
    }

    public void ObjectInitialize()  // Assign objects
    {
        customer = gameObject;
        EateryDetailName = customer.transform.Find("EateryPage/EateryDetail/Name").GetComponent<Text>();
        EateryDetailProfileName = customer.transform.Find("EateryPage/EateryDetail/ProfileView/Name").GetComponent<Text>();

        // initialize toggle group
        CuisineToggleGroup = transform.Find("ToggleGroups").Find("CuisineToggleGroup").GetComponent<ToggleGroup>();
        CuisineViewContent = transform.Find("EateryPage/EateryDetail/MenuView/CuisineView/Viewport/Content").gameObject;
        VoucherSortToggleGroup = transform.Find("ToggleGroups").Find("VoucherSortToggleGroup").GetComponent<ToggleGroup>();
        MeToggleGroup = transform.Find("ToggleGroups").Find("MeToggleGroup").GetComponent<ToggleGroup>();

        // initialize Eatery Detail page, for customer click on any eatery and navigate to the eatery
        EateryDetail = transform.Find("EateryPage").Find("EateryDetail").gameObject;
        //public static Text Name, Rate, Sale, OpenTime, CloseTime, Contact, Postcode, Address, Description, Tag;
        Name = customer.transform.Find("EateryPage/EateryDetail/ProfileView/Name").GetComponent<Text>();
        Rate = customer.transform.Find("EateryPage/EateryDetail/ProfileView/Rate").GetComponent<Text>();
        Sale = customer.transform.Find("EateryPage/EateryDetail/ProfileView/Sale").GetComponent<Text>();
        OpenTime = customer.transform.Find("EateryPage/EateryDetail/ProfileView/OpenTime").GetComponent<Text>();
        CloseTime = customer.transform.Find("EateryPage/EateryDetail/ProfileView/CloseTime").GetComponent<Text>();
        Contact = customer.transform.Find("EateryPage/EateryDetail/ProfileView/Contact").GetComponent<Text>();
        Postcode = customer.transform.Find("EateryPage/EateryDetail/ProfileView/Postcode").GetComponent<Text>();
        Address = customer.transform.Find("EateryPage/EateryDetail/ProfileView/Address").GetComponent<Text>();
        Description = customer.transform.Find("EateryPage/EateryDetail/ProfileView/Description").GetComponent<Text>();
        Tag = customer.transform.Find("EateryPage/EateryDetail/ProfileView/Tag").GetComponent<Text>();

        // initialize shopping cart 
        ShoppingCartViewContent = customer.transform.Find("EateryPage/EateryDetail/MenuView/ShoppingCart/ShoppingCartView/Viewport/Content").gameObject;
        ShoppingCartTotalPrice = customer.transform.Find("EateryPage/EateryDetail/MenuView/ShoppingCart/TotalPrice").GetComponent<Text>();
        ShoppingCartTotalAmount = customer.transform.Find("EateryPage/EateryDetail/MenuView/ShoppingCart/TotalAmountImage/TotalAmount").GetComponent<Text>();
        OrderDishIDsList = new List<int>();
        OrderDishAmountsList = new List<int>();
        OrderDishCurrentStock = new List<int>();
        CurrentPlacedOrderID = customer.transform.Find("EateryPage/EateryDetail/MenuView/ShoppingCart/CurrentPlacedOrderID").GetComponent<Text>();
    }
    public static void SpawnEateries(int SORT = 0, int SEQUENCE = 1)
    {
        EateryViewContent = customer.transform.Find("EateryPage/EateryView/Viewport/Content").gameObject;
        //clear all eateries
        if (EateryViewContent.transform.childCount > 0)
        {
            for (int i = 0; i < EateryViewContent.transform.childCount; i++)
            {
                Destroy(EateryViewContent.transform.GetChild(i).gameObject);
            }
        }

        // read eateries from database and spawn on scene
        List<RestaurantStruct> eateryList = MySqlManager.GetAllRes(SORT, SEQUENCE);
        if (eateryList.Count == 0) TipsManager.SpawnTips("No eateries found");
        GameObject EateryPrefab = (GameObject)Resources.Load("Prefabs/Eatery");
        for (int i = 0; i < eateryList.Count; i++)
        {
            EateryPrefab.transform.Find("res_id").GetComponent<Text>().text = eateryList[i].res_id;
            EateryPrefab.transform.Find("user_id").GetComponent<Text>().text = eateryList[i].user_id;
            EateryPrefab.transform.Find("liscense_num").GetComponent<Text>().text = eateryList[i].liscense_num;
            EateryPrefab.transform.Find("Name").GetComponent<Text>().text = eateryList[i].name;

            if (string.IsNullOrEmpty(eateryList[i].rate)) EateryPrefab.transform.Find("Rate").GetComponent<Text>().text = "5.0";
            else EateryPrefab.transform.Find("Rate").GetComponent<Text>().text = eateryList[i].rate;

            if (string.IsNullOrEmpty(eateryList[i].sale)) EateryPrefab.transform.Find("Sale").GetComponent<Text>().text = "0";
            else EateryPrefab.transform.Find("Sale").GetComponent<Text>().text = eateryList[i].sale;

            float.TryParse(eateryList[i].max_discount, out float max_discount); // get the largest discount to display to the customer
            if (max_discount > 0)
            {
                EateryPrefab.transform.Find("Max_Discount").GetComponent<Text>().text = max_discount * 100 + "%";
                EateryPrefab.transform.Find("Max_Discount").gameObject.SetActive(true);
                EateryPrefab.transform.Find("DiscountLabel").gameObject.SetActive(true);
            }
            else
            {
                EateryPrefab.transform.Find("Max_Discount").GetComponent<Text>().text = "";
                EateryPrefab.transform.Find("Max_Discount").gameObject.SetActive(false);
                EateryPrefab.transform.Find("DiscountLabel").gameObject.SetActive(false);
            }

            EateryPrefab.transform.Find("Tag").GetComponent<Text>().text = eateryList[i].tag;
            //EateryPrefab.transform.Find("Image").GetComponent<RawImage>().texture = eateryList[i].image;
            Instantiate(EateryPrefab, EateryViewContent.transform);
        }
    }
    public static void SpawnCuisines()
    {
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
        List<TagStruct> tagList = MySqlManager.GetAllTagByResID(selected_res_id);
        //if (tagList.Count == 0) TipsManager.SpawnTips("No cuisine found");
        for (int i = 0; i < tagList.Count; i++)
        {
            CuisinePrefab.transform.Find("Label").GetComponent<Text>().text = tagList[i].name;
            CuisinePrefab.GetComponent<Toggle>().group = CuisineToggleGroup;
            Instantiate(CuisinePrefab, CuisineViewContent.transform);
        }
    }
    public static void SpawnEateryDishes(int SORT = 0, int SEQUENCE = 0, string CUISINE = "")
    {
        EateryDishesViewContent = customer.transform.Find("EateryPage/EateryDetail/MenuView/DishesView/Viewport/Content").gameObject;

        // clear all dish
        if (EateryDishesViewContent.transform.childCount > 0)
        {
            for (int i = 0; i < EateryDishesViewContent.transform.childCount; i++)
            {
                Destroy(EateryDishesViewContent.transform.GetChild(i).gameObject);
            }
        }

        EateryDetailProfileName.text = EateryDetailName.text;

        // read dishes from database and spawn on scene
        List<DishStruct> dishList = MySqlManager.GetAllDishesByResID(selected_res_id,SORT,SEQUENCE,CUISINE);
        if (dishList.Count == 0) TipsManager.SpawnTips("No dish found");
        GameObject DishPrefab = (GameObject)Resources.Load("Prefabs/Dish");
        for (int i = 0; i < dishList.Count; i++)
        {
            DishPrefab.transform.Find("Dish_ID").GetComponent<Text>().text = dishList[i].dish_id;
            DishPrefab.transform.Find("ResID").GetComponent<Text>().text = dishList[i].res_id;
            DishPrefab.transform.Find("Name").GetComponent<Text>().text = dishList[i].name;

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
            Instantiate(DishPrefab, EateryDishesViewContent.transform);
        }
    }
    public static void SpawnDishInShoppingCart(int dish_id)
    {
        ShoppingCartViewContent = customer.transform.Find("EateryPage/EateryDetail/MenuView/ShoppingCart/ShoppingCartView/Viewport/Content").gameObject;

        GameObject DishInCartPrefab = (GameObject)Resources.Load("Prefabs/DishInCart");

        DishStruct dish = MySqlManager.GetDishByDishID(dish_id);
        if (string.IsNullOrEmpty(dish.dish_id)) TipsManager.SpawnTips("Connection error");
        else
        {
            DishInCartPrefab.transform.Find("dish_id").GetComponent<Text>().text = dish.dish_id;
            int.TryParse(dish.dish_id, out int dishID);
            OrderDishIDsList.Add(dishID);

            DishInCartPrefab.transform.Find("Name").GetComponent<Text>().text = dish.name;
            DishInCartPrefab.transform.Find("Price").GetComponent<Text>().text = dish.price;

            int.TryParse(dish.stock, out int stock);
            stock = stock - 1;
            DishInCartPrefab.transform.Find("Stock").GetComponent<Text>().text = stock.ToString();

            DishInCartPrefab.transform.Find("Sale").GetComponent<Text>().text = dish.sale;

            DishInCartPrefab.transform.Find("Image/Text").GetComponent<Text>().text = dish.image;
            DishInCartPrefab.transform.Find("Amount").GetComponent<Text>().text = "1";
            OrderDishAmountsList.Add(1);

            // update total price in cart
            float.TryParse(ShoppingCartTotalPrice.text, out float total_price);
            float.TryParse(dish.price, out float price);
            total_price = total_price + price;
            ShoppingCartTotalPrice.text = total_price.ToString();

            // update amount in cart
            int.TryParse(ShoppingCartTotalAmount.text, out int total_amount);
            total_amount = total_amount + 1;
            ShoppingCartTotalAmount.text = total_amount.ToString();

            Instantiate(DishInCartPrefab, ShoppingCartViewContent.transform);
        }

    }
    public static void SpawnEateryComments()
    {
        EateryCommentViewContent = customer.transform.Find("EateryPage/EateryDetail/CommentView/Viewport/Content").gameObject;
        // clear all comments
        if (EateryCommentViewContent.transform.childCount > 0)
        {
            for (int i = 0; i < EateryCommentViewContent.transform.childCount; i++)
            {
                Destroy(EateryCommentViewContent.transform.GetChild(i).gameObject);
            }
        }
        // read dishes from database and spawn on scene
        List<CommentStruct> commentList = MySqlManager.GetAllCommentsByResID(selected_res_id);
        //if (commentList.Count == 0) TipsManager.SpawnTips("No comment found");
        GameObject CommentPrefab = (GameObject)Resources.Load("Prefabs/Comment");
        for (int i = 0; i < commentList.Count; i++)
        {
            customer.transform.Find("EateryPage/EateryDetail/CommentCount").GetComponent<Text>().text = "(" + commentList.Count.ToString() + ")";
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
            if (RateStarCount > 0) CommentPrefab.transform.Find("Rate/Star1").gameObject.SetActive(true);
            if (RateStarCount > 1) CommentPrefab.transform.Find("Rate/Star2").gameObject.SetActive(true);
            if (RateStarCount > 2) CommentPrefab.transform.Find("Rate/Star3").gameObject.SetActive(true);
            if (RateStarCount > 3) CommentPrefab.transform.Find("Rate/Star4").gameObject.SetActive(true);
            if (RateStarCount > 4) CommentPrefab.transform.Find("Rate/Star5").gameObject.SetActive(true);

            if (string.IsNullOrEmpty(commentList[i].content)) CommentPrefab.transform.Find("Content").GetComponent<Text>().text = "Default comment";
            else CommentPrefab.transform.Find("Content").GetComponent<Text>().text = commentList[i].content;

            Instantiate(CommentPrefab, EateryCommentViewContent.transform);
        }
    }
    public static void SpawnEateryProfile()
    {
        RestaurantStruct res = MySqlManager.GetResByResID(selected_res_id);
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
    public static void SpawnMyVouchers(int SORT = 0, int SEQUENCE = 0)
    {
        MyVoucherViewContent = customer.transform.Find("MyVoucherPage/VoucherView/Viewport/Content").gameObject;
        // clear all vouchers
        if (MyVoucherViewContent.transform.childCount > 0)
        {
            for (int i = 0; i < MyVoucherViewContent.transform.childCount; i++)
            {
                Destroy(MyVoucherViewContent.transform.GetChild(i).gameObject);
            }
        }
        // read vouchers from database and spawn on scene
        List<VoucherListStruct> voucherList = MySqlManager.GetAllVouchersByCusID(UserManager.ResOrCusID, SORT, SEQUENCE);
        //if (voucherList.Count == 0) TipsManager.SpawnTips("No voucher found");
        GameObject VoucherPrefab = (GameObject)Resources.Load("Prefabs/Voucher");
        for (int i = 0; i < voucherList.Count; i++)
        {
            customer.transform.Find("MyVoucherPage/VoucherCount").GetComponent<Text>().text = "(" + voucherList.Count.ToString() + ")";
            VoucherPrefab.transform.Find("Purpose").GetComponent<Text>().text = "2";
            VoucherPrefab.transform.Find("VoucherList_ID").GetComponent<Text>().text = voucherList[i].voucherList_id;
            VoucherPrefab.transform.Find("Voucher_ID").GetComponent<Text>().text = voucherList[i].voucher_id;
            VoucherPrefab.transform.Find("Res_ID").GetComponent<Text>().text = voucherList[i].res_id;
            VoucherPrefab.transform.Find("Res_Name").GetComponent<Text>().text = voucherList[i].res_name;
            VoucherPrefab.transform.Find("Name").GetComponent<Text>().text = voucherList[i].name;

            if (string.IsNullOrEmpty(voucherList[i].discount)) VoucherPrefab.transform.Find("Discount").GetComponent<Text>().text = "1";
            else VoucherPrefab.transform.Find("Discount").GetComponent<Text>().text = voucherList[i].discount;

            if (string.IsNullOrEmpty(voucherList[i].start_time)) VoucherPrefab.transform.Find("StartTime").GetComponent<Text>().text = "00:00:00";
            else VoucherPrefab.transform.Find("StartTime").GetComponent<Text>().text = voucherList[i].start_time;
            if (string.IsNullOrEmpty(voucherList[i].end_time)) VoucherPrefab.transform.Find("EndTime").GetComponent<Text>().text = "23:59:59";
            else VoucherPrefab.transform.Find("EndTime").GetComponent<Text>().text = voucherList[i].end_time;

            if (string.IsNullOrEmpty(voucherList[i].start_date)) VoucherPrefab.transform.Find("StartDate").GetComponent<Text>().text = "inf";
            else VoucherPrefab.transform.Find("StartDate").GetComponent<Text>().text = voucherList[i].start_date;
            if (string.IsNullOrEmpty(voucherList[i].end_date)) VoucherPrefab.transform.Find("EndDate").GetComponent<Text>().text = "inf";
            else VoucherPrefab.transform.Find("EndDate").GetComponent<Text>().text = voucherList[i].end_date;
            
            VoucherPrefab.transform.Find("Type").GetComponent<Text>().text = voucherList[i].type;
            //VoucherPrefab.transform.Find("Image").GetComponent<RawImage>().texture = voucherList[i].image;
            Instantiate(VoucherPrefab, MyVoucherViewContent.transform);
        }
    }
    public static void SpawnCollectVouchers(int SORT = 0, int SEQUENCE = 0)
    {
        CollectVoucherViewContent = customer.transform.Find("EateryPage/EateryDetail/MenuView/VoucherCollectView/Viewport/Content").gameObject;
        // clear all vouchers
        if (CollectVoucherViewContent.transform.childCount > 0)
        {
            for (int i = 0; i < CollectVoucherViewContent.transform.childCount; i++)
            {
                Destroy(CollectVoucherViewContent.transform.GetChild(i).gameObject);
            }
        }
        // read vouchers from database and spawn on scene
        List<VoucherStruct> voucherList = MySqlManager.GetAllAvailableVouchersByResID(selected_res_id, SORT, SEQUENCE);
        if(voucherList.Count == 0) TipsManager.SpawnTips("No voucher found");
        GameObject VoucherPrefab = (GameObject)Resources.Load("Prefabs/Voucher");
        for (int i = 0; i < voucherList.Count; i++)
        {
            customer.transform.Find("MyVoucherPage/VoucherCount").GetComponent<Text>().text = "(" + voucherList.Count.ToString() + ")";
            VoucherPrefab.transform.Find("Purpose").GetComponent<Text>().text = "3";
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

            VoucherPrefab.transform.Find("Type").GetComponent<Text>().text = voucherList[i].type;
            //VoucherPrefab.transform.Find("Image").GetComponent<RawImage>().texture = voucherList[i].image;
            Instantiate(VoucherPrefab, CollectVoucherViewContent.transform);
        }
    }
    public void SpawnMyProfile()
    {
        CustomerStruct cus = MySqlManager.GetCusByCusID(UserManager.ResOrCusID);
        if (String.IsNullOrEmpty(cus.cus_id)) TipsManager.SpawnTips("Failed to load my profile");
        else
        {
            MyName.text = cus.name;
            MyPhone.text = cus.phone_num;
            MyEmail.text = cus.email_address;
            MyPostcode.text = cus.postcode;
            MyAddress.text = cus.address;
            MyFavorRes.text = cus.favor_res;
            MyFavorDish.text = cus.favor_dish;
            MyImage.text = cus.image;
        }
    }
    public static void SpawnMyComments()
    {
        MyCommentViewContent = customer.transform.Find("MePage/CommentView/Viewport/Content").gameObject;
        // clear all comments
        if (MyCommentViewContent.transform.childCount > 0)
        {
            for (int i = 0; i < MyCommentViewContent.transform.childCount; i++)
            {
                Destroy(MyCommentViewContent.transform.GetChild(i).gameObject);
            }
        }
        // read dishes from database and spawn on scene
        List<CommentStruct> commentList = MySqlManager.GetAllCommentsByCusID(UserManager.ResOrCusID);
        //if (commentList.Count == 0) TipsManager.SpawnTips("No comment found");
        GameObject CommentPrefab = (GameObject)Resources.Load("Prefabs/Comment");
        for (int i = 0; i < commentList.Count; i++)
        {
            customer.transform.Find("MePage/CommentCount").GetComponent<Text>().text = "(" + commentList.Count.ToString() + ")";
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
            if (RateStarCount > 0) CommentPrefab.transform.Find("Rate/Star1").gameObject.SetActive(true);
            if (RateStarCount > 1) CommentPrefab.transform.Find("Rate/Star2").gameObject.SetActive(true);
            if (RateStarCount > 2) CommentPrefab.transform.Find("Rate/Star3").gameObject.SetActive(true);
            if (RateStarCount > 3) CommentPrefab.transform.Find("Rate/Star4").gameObject.SetActive(true);
            if (RateStarCount > 4) CommentPrefab.transform.Find("Rate/Star5").gameObject.SetActive(true);

            if (string.IsNullOrEmpty(commentList[i].content)) CommentPrefab.transform.Find("Content").GetComponent<Text>().text = "Default comment";
            else CommentPrefab.transform.Find("Content").GetComponent<Text>().text = commentList[i].content;

            Instantiate(CommentPrefab, MyCommentViewContent.transform);
        }
    }
    public void RefreshEateryPage(bool isToggleOn)
    {
        if(isToggleOn)
        {
            SpawnEateries();
        }
    }
    public void RefreshMyVoucherPage(bool isToggleOn)
    {
        if (isToggleOn)
        {
            int sort = 0;
            //if (VoucherSortByAllToggle.isOn) sort = 0;
            if (VoucherSortByDiscountToggle.isOn) sort = 1;
            if (VoucherSortByTypeToggle.isOn) sort = 2;
            if (VoucherSortByStartTimeToggle.isOn) sort = 3;
            if (VoucherSortByEndTimeToggle.isOn) sort = 4;
            if (VoucherSortByStartDateToggle.isOn) sort = 5;
            if (VoucherSortByEndDateToggle.isOn) sort = 6;
            int seq = 0;
            //if (SortDescendingBtn.IsActive()) seq = 1;
            if (SortAscendingBtn.IsActive()) seq = 1;
            SpawnMyVouchers(sort, seq);
        }
    }
    public void RefreshMyPage(bool isToggleOn)
    {
        if (isToggleOn)
        {
            SpawnMyProfile();
            SpawnMyComments();
        }
    }
    public int UpdateMyProfile()
    {
        string query = "UPDATE `customer` SET  `phone_num` = '"+ MyPhoneInputField.text+"', `email_address` = '"+ MyEmailInputField.text+"', `name` = '"+ MyNameInputField.text+ "', `favor_dish` = '"+ MyFavorDishInputField.text+ "', `favor_res` = '"+ MyFavorResInputField.text+"', `postcode` = '"+ MyPostcodeInputField.text+"', `address` = '"+ MyAddressInputField.text+"', `image` = '"+MyImage.text+"' WHERE `cus_id` = "+ UserManager.ResOrCusID;
        return MySqlManager.Update(query);
    }
    public int InsertOrder()
    {
        string dish_ids="", dish_amounts="";
        for(int i=0;i< OrderDishIDsList.Count;i++)
        {
            dish_ids += OrderDishIDsList[i]+"|";
            dish_amounts += OrderDishAmountsList[i] + "|";
        }
        float discount = 0;
        int.TryParse(ShoppingCartTotalAmount.text,out int total_amount);
        float.TryParse(ShoppingCartTotalPrice.text, out float total_price);
        int state = 0; // order palced
        string query = "INSERT INTO `order` VALUES (NULL," + selected_res_id + ", '" + Name.text + "', " + UserManager.ResOrCusID+ ",'" + MyName.text + "' ,'" + dish_ids + "','" + dish_amounts + "', " + total_amount + "," + discount + ", " + total_price + ","+ state+",'"+ DateTime.Now.ToString() + "');";
        return MySqlManager.Insert(query);
    }
    public int InsertComment()
    {
        int.TryParse(PayFinishedRate.GetComponent<Text>().text, out int rate);
        string query = "INSERT INTO `comment` VALUES(NULL,"+ selected_res_id+", "+UserManager.ResOrCusID+", NULL, '"+ MyName.text + "',"+ rate+" , '"+ CommentInputField.text+ "','"+ DateTime.Now.ToString()+"', NULL);";
        return MySqlManager.Insert(query);
    }
    public int UpdateDishStockAndSale()
    {
        string query = "";
        for (int i = 0; i< OrderDishIDsList.Count;i++)
        {
            int stock = OrderDishCurrentStock[i] - OrderDishAmountsList[i];
            if (stock < 0) stock = 0;
            int sale = MySqlManager.GetDishSaleByDishID(OrderDishIDsList[i])+ OrderDishAmountsList[i];
            query = query + "UPDATE dish SET stock = " + stock + ", sale = "+sale+ " WHERE dish_id = " + OrderDishIDsList[i] + "; ";
        }
        return MySqlManager.Update(query);
    }
    public int UpdateResSale()
    {
        int.TryParse(Sale.text, out int current_sale);
        int.TryParse(ShoppingCartTotalAmount.text, out int amount);
        int sale = current_sale + amount;
        string query = "UPDATE restaurant SET  sale = " + sale + " WHERE res_id = " + placed_order_res_id + "; ";
        return MySqlManager.Update(query);
    }
    public int UpdateOrderState(int orderID)
    {
        string query = "UPDATE `order` SET state = " + 1 + " WHERE order_id = " + orderID + "; ";
        return MySqlManager.Update(query);
    }
    public int InsertFeedback()
    {
        string query = "Insert into `feedback` values(NULL, " + UserManager.ResOrCusID + ",'" + FeedbackInputField.text + "', '" + DateTime.Now.ToString() + "')";
        return MySqlManager.Insert(query);
    }
}