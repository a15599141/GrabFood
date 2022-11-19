public struct DishStruct
{
    public string dish_id;
    public string res_id;
    public string name;
    public string price;
    public string stock;
    public string sale;
    public string state;
    public string tag;
    public string cuisine;
    public string serve_type;
    public string description;
    public string image;
}

public struct RestaurantStruct
{
    public string res_id;
    public string user_id;
    public string liscense_num;
    public string name;
    public string rate;
    public string sale;
    public string contact_num;
    public string postcode;
    public string address;
    public string open_time;
    public string close_time;
    public string description;
    public string tag;
    public string max_discount;
    public string image;
}
public struct CustomerStruct
{
    public string cus_id;
    public string user_id;
    public string liscense_num;
    public string phone_num;
    public string email_address;
    public string name;
    public string favor_dish;
    public string favor_res;
    public string postcode;
    public string address;
    public string image;
}
public struct VoucherStruct
{
    public string voucher_id;
    public string res_id;
    public string res_name;
    public string name;
    public string discount;
    public string stock;
    public string state;
    public string type;
    public string start_time;
    public string end_time;
    public string start_date;
    public string end_date;
    public string image;
}
public struct VoucherListStruct
{
    public string voucherList_id;
    public string voucher_id;
    public string cus_id;
    public string res_id;
    public string res_name;
    public string name;
    public string discount;
    public string type;
    public string state;
    public string start_time;
    public string end_time;
    public string start_date;
    public string end_date;
}
public struct CommentStruct
{
    public string comment_id;
    public string res_id;
    public string cus_id;
    public string cus_name;
    public string rate;
    public string content;
    public string date;
    public string image;
}

public struct TagStruct
{
    public string tag_id;
    public string res_id;
    public string name;
    public string type;
}

public struct OrderStruct
{
    public string cus_name;
    public string total_amount;
    public string total_price;
    public string state;
}