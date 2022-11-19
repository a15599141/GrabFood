using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MySql.Data.MySqlClient;
using System;

public class MySqlManager : MonoBehaviour
{
    private static MySqlConnection mySqlConnection;
    // IP address
    private static string host = "gz-cynosdbmysql-grp-jfqeugr7.sql.tencentcdb.com";
    // port number
    private static string port = "24690";
    // connection username
    private static string userName = "root";
    // connection password
    private static string password = "tencent_comp9900";
    // database name
    private static string databaseName = "data";

    public readonly static string mySqlString = string.Format("Database={0};Data Source={1};User Id={2};Password={3};port={4}",
                                                                databaseName, host, userName, password, port);

    public void Start()
    {

    }

    /// <summary>
    /// open database
    /// </summary>
    public static void OpenSql()
    {
        try
        {
            mySqlConnection = new MySqlConnection(mySqlString);
            mySqlConnection.Open();
            Debug.Log("<color=#00FF00>" + "Server connected!" + "</color>");
        }
        catch (Exception e)
        {
            throw new Exception("Server connection failded£¬please check MySQL is well connected" + e.Message.ToString());
        }
    }

    /// <summary>
    /// close database
    /// </summary>
    public static void CloseSql()
    {
        mySqlConnection = new(mySqlString);
        if (mySqlConnection != null)
        {
            mySqlConnection.Close();
            mySqlConnection.Dispose();
            mySqlConnection = null;
            Debug.Log("<color=#00FF00>" + "Connection pending" + "</color>");
        }
    }

    public static MySqlDataReader Select(string query) // for selection query, return data reader for result extraction
    {
        Debug.Log("Run query: " + query);
        MySqlCommand cmd = new(query, mySqlConnection);//create a new command 
        MySqlDataReader reader = cmd.ExecuteReader();//read command result
        return reader;
    }
    public static int Insert(string query) // for Insertion query, return affected row counts 
    {
        int affectRowCount= 0;
        try
        {
            OpenSql();
            Debug.Log("Run query: " + query);
            MySqlCommand cmd = new(query, mySqlConnection);//create a new command 
            affectRowCount = cmd.ExecuteNonQuery();// row num after insert
            Debug.Log("Insert affect row count: " + affectRowCount);
        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
        }
        CloseSql();
        return affectRowCount;
    }
    public static int Update(string query) // for update query,  return affected row counts 
    {
        int affectRowCount = 0;
        try
        {
            OpenSql();
            Debug.Log("Run query: " + query);
            MySqlCommand cmd = new(query, mySqlConnection);//create a new command 
            affectRowCount = cmd.ExecuteNonQuery();// row num after update
            Debug.Log("Update affect row count: " + affectRowCount);
        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
        }
        CloseSql();
        return affectRowCount;
    }
    public static int Delete(string query) // for update query,  return affected row counts 
    {
        int affectRowCount = 0;
        try
        {
            OpenSql();
            Debug.Log("Run query: " + query);
            MySqlCommand cmd = new(query, mySqlConnection);//create a new command 
            affectRowCount = cmd.ExecuteNonQuery();// row num after update
            Debug.Log("Delete affect row count: " + affectRowCount);
        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
        }
        CloseSql();
        return affectRowCount;
    }

    public static bool UsernameIsExist(string username)
    {
        bool isExist = false;
        try
        {
            OpenSql();
            string query = "select 1 from user where username = '"+ username + "' limit 1";
            MySqlDataReader reader = Select(query);//read command result
            if (reader.HasRows)
            {
                reader.Read();
                Debug.Log("Query result: " + reader.GetString(0));
                if (reader.GetInt32(0) == 1) isExist = true;

            }
        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
        }
        CloseSql();
        return isExist;
    }
    public static int LoginCheck(string username, string password) // return bool 
    {
        int checkResult = -1;
        if (UsernameIsExist(username))
        {
            try
            {
                OpenSql();
                string query = "select password from user where username = '" + username + "'";
                MySqlDataReader reader = Select(query);//read command result
                if (reader.HasRows)
                {
                    reader.Read();
                    Debug.Log("Query result: " + reader.GetString(0));
                    if (!reader.GetString(0).Equals(password)) // then check the password
                    {
                        checkResult = 1; // pw incorrect
                    }
                    else checkResult = 2; // pw correct
                }
                else checkResult = 0;
            }
            catch (Exception e)
            {
                Debug.Log(e.ToString());
            }
        }
        CloseSql();
        return checkResult;
    }
    public static bool ResLicenseCheck(int licenseNum) // return bool 
    {
        //bool isValid = false;
        try
        {
            OpenSql();
            string query = "select state from license where license_num = " + licenseNum;
            MySqlDataReader reader = Select(query);//read command result
            if (reader.HasRows)
            {
                reader.Read();
                Debug.Log("Query result: " + reader.GetInt32(0));
                if (reader.GetInt32(0) == 1) // then check license state 
                {
                    //isValid = true;
                }
            }
        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
        }
        CloseSql();
        return true;
    }
    public static bool isDishNameExist(string dishName) // check whether a dish name exist of a certain resturant 
    {
        bool isExist = false;
        try
        {
            OpenSql();
            string query = "select 1 from dish where res_id = " + UserManager.ResOrCusID + " and name = '" + dishName + "' limit 1";
            MySqlDataReader reader = Select(query);//read command result
            if (reader.HasRows)
            {
                reader.Read();
                Debug.Log("Query result: " + reader.GetString(0));
                if (reader.GetInt32(0) == 1) isExist = true;

            }
        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
        }
        CloseSql();
        return isExist;
    }
    public static bool isVoucherNameExist(string voucherName) // check whether a dish name exist of a certain resturant 
    {
        bool isExist = false;
        try
        {
            OpenSql();
            string query = "select 1 from voucher where res_id = " + UserManager.ResOrCusID + " and name = '" + voucherName + "' limit 1";
            MySqlDataReader reader = Select(query);//read command result
            if (reader.HasRows)
            {
                reader.Read();
                Debug.Log("Query result: " + reader.GetString(0));
                if (reader.GetInt32(0) == 1) isExist = true;

            }
        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
        }
        CloseSql();
        return isExist;
    }
    public static bool isCusAlreadyOwnVoucher(int voucherID)
    {
        bool isOwn = false;
        try
        {
            OpenSql();
            string query = "select 1 from voucher_list where voucher_id = " + voucherID + " and cus_id = '" + UserManager.ResOrCusID + "' limit 1";
            MySqlDataReader reader = Select(query);//read command result
            if (reader.HasRows)
            {
                reader.Read();
                Debug.Log("Query result: " + reader.GetString(0));
                if (reader.GetInt32(0) == 1) isOwn = true;
            }
        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
        }
        CloseSql();
        return isOwn;
    }
    public static bool DishIsOffShelved(int dishID)
    {
        bool isOffShelved = false;
        try
        {
            OpenSql();
            string query = "SELECT state FROM dish where dish_id =  " + dishID;
            MySqlDataReader reader = Select(query);
            if (reader.HasRows)
            {
                reader.Read();
                Debug.Log("Query result: " + reader.GetString(0));
                if (reader.GetInt32(0) == 0) isOffShelved = true;
            }
        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
        }
        CloseSql();
        return isOffShelved;
    }
    public static bool DishIsSoldOut(int dishID)
    {
        bool isSoldOut = false;
        try
        {
            OpenSql();
            string query = "SELECT stock FROM dish where dish_id =  " + dishID;
            MySqlDataReader reader = Select(query);
            if (reader.HasRows)
            {
                reader.Read();
                Debug.Log("Query result: " + reader.GetString(0));
                if (reader.GetInt32(0) <= 0) isSoldOut = true;
            }
        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
        }
        CloseSql();
        return isSoldOut;
    }
    public static int DishIsAvaliable(int dishID)  // 0 connection error, 1 stock not enough, 2 dish off shelved
    {
        int isAvaliable = 0;
        try
        {
            OpenSql();
            string query = "SELECT stock, state FROM dish where dish_id =  " + dishID;
            MySqlDataReader reader = Select(query);
            if (reader.HasRows)
            {
                reader.Read();
                Debug.Log("Query result: " + reader.GetString(0));
                int stock, state;
                stock = reader.GetInt32(0);
                state = reader.GetInt32(1);
                if (stock <= 0) isAvaliable = 1;
                else if (state == 0) isAvaliable = 2;
                else isAvaliable = 3;
            }
        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
        }
        CloseSql();
        return isAvaliable;
    }
    public static int CheckCuisineExsit(int resID, string tagName)
    {
        int result = 0;
        try
        {
            OpenSql();
            string query = "select 1 from `tag` where res_id = " + resID + " and name = '" + tagName + "'";
            MySqlDataReader reader = Select(query);//read command result
            if (reader.HasRows)
            {
                reader.Read();
                Debug.Log("Query result: " + reader.GetString(0));
                result = 1;
            }
            else result = 2;
        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
        }
        CloseSql();
        return result;
    }
    public static int GetDishStockByID(int dishID) // return current of the dish
    {
        int stock = 0;
        try
        {
            OpenSql();
            string query = "SELECT stock FROM dish where dish_id =  " + dishID;
            MySqlDataReader reader = Select(query);
            if (reader.HasRows)
            {
                reader.Read();
                Debug.Log("Query result: " + reader.GetString(0));
                stock = reader.GetInt32(0);
            }
        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
        }
        CloseSql();
        return stock;
    }
    public static int GetUserTypeByUsername(string username) // return usertype 
    {
        int usertype = 0;
        try
        {
            OpenSql();
            string query = "select type from user where username = '" + username + "'";
            MySqlDataReader reader = Select(query);//read command result
            if (reader.HasRows)
            {
                reader.Read();
                Debug.Log("Query result: " + reader.GetString(0));
                usertype = reader.GetInt32(0);
            }
        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
        }


        CloseSql();
        return usertype;
    }
    public static int GetUserIdByRowNum(int RowNum)
    {
        int userID = 0;
        try
        {
            OpenSql();
            string query = "SELECT user_id FROM user LIMIT " + RowNum + ",1;";
            MySqlDataReader reader = Select(query);
            if (reader.HasRows)
            {
                reader.Read();
                Debug.Log("Query result: " + reader.GetString(0));
                userID = reader.GetInt32(0);
            }
        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
        }
        CloseSql();
        return userID;
    }
    public static int GetUserIdByUsername(string Username)
    {
        int userID = 0;
        try
        {
            OpenSql();
            string query = "SELECT user_id FROM user where username = '" + Username + "';";
            MySqlDataReader reader = Select(query);
            if (reader.HasRows)
            {
                reader.Read();
                Debug.Log("Query result: " + reader.GetString(0));
                userID = reader.GetInt32(0);
            }
        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
        }
        CloseSql();
        return userID;
    }
    public static int GetUserIdOfLastRow()
    {
        int userID = 0;
        try
        {
            OpenSql();
            string query = "select user_id from user order by user_id desc limit 1;";
            MySqlDataReader reader = Select(query);
            if (reader.HasRows)
            {
                reader.Read();
                Debug.Log("Query result: " + reader.GetString(0));
                userID = reader.GetInt32(0);
            }
        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
        }
        CloseSql();
        return userID;
    }
    public static int GetResIdByUserID(int userID)
    {
        int ResID = 0;
        try
        {
            OpenSql();
            string query = "SELECT res_id FROM restaurant where user_id =  " + userID;
            MySqlDataReader reader = Select(query);
            if (reader.HasRows)
            {
                reader.Read();
                Debug.Log("Query result: " + reader.GetString(0));
                ResID = reader.GetInt32(0);
            }
        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
        }
        CloseSql();
        return ResID;
    }
    public static int GetCusIdByUserID(int userID)
    {
        int CusID = 0;
        try
        {
            OpenSql();
            string query = "SELECT cus_id FROM customer where user_id =  " + userID;
            MySqlDataReader reader = Select(query);
            if (reader.HasRows)
            {
                reader.Read();
                Debug.Log("Query result: " + reader.GetString(0));
                CusID = reader.GetInt32(0);
            }
        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
        }
        CloseSql();
        return CusID;
    }
    public static float GetAvailableMaxDiscountByResID(int resID)
    {
        float max_discount = 0;
        try
        {
            OpenSql();
            string query = "select MAX(discount) from voucher where state = 1 and res_id = " + resID;
            MySqlDataReader reader = Select(query);
            if (reader.HasRows)
            {
                reader.Read();
                Debug.Log("Query result: " + reader.GetString(0));
                max_discount = reader.GetFloat(0);
            }
        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
        }
        CloseSql();
        return max_discount;
    }
    public static float GetDishPriceByDishID(int dishID)
    {
        float price = 0;
        try
        {
            OpenSql();
            string query = "select price from dish where dish_id = " + dishID;
            MySqlDataReader reader = Select(query);
            if (reader.HasRows)
            {
                reader.Read();
                Debug.Log("Query result: " + reader.GetString(0));
                price = reader.GetFloat(0);
            }
        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
        }
        CloseSql();
        return price;
    }
    public static int GetDishSaleByDishID(int dishID)
    {
        int sale = 0;
        try
        {
            OpenSql();
            string query = "select sale from dish where dish_id = " + dishID;
            MySqlDataReader reader = Select(query);
            if (reader.HasRows)
            {
                reader.Read();
                Debug.Log("Query result: " + reader.GetString(0));
                sale = reader.GetInt32(0);
            }
        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
        }
        CloseSql();
        return sale;
    }
    public static int GetCurrentPlacedOrderID(int res_id, int cusID)
    {
        int id = 0;
        try
        {
            OpenSql();
            string query = "select order_id from `order` where res_id = " + res_id + " and cus_id = "+ cusID+ " and state = 0";
            MySqlDataReader reader = Select(query);
            if (reader.HasRows)
            {
                reader.Read();
                Debug.Log("Query result: " + reader.GetString(0));
                id = reader.GetInt32(0);
            }
        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
        }
        CloseSql();
        return id;
    }


    public static List<DishStruct> GetAllDishesByResID(int resID, int sort = 0, int sequence = 0, string cuisine = "")
    {
        List<DishStruct> dishList = new();
        try
        {
            OpenSql();
            string ORDER_BY = " ORDER BY state ";
            if (sort == 1) ORDER_BY = " ORDER BY price ";
            if (sort == 2) ORDER_BY = " ORDER BY stock ";
            if (sort == 3) ORDER_BY = " ORDER BY sale ";
            if (sort == 4) ORDER_BY = " ORDER BY serve_type ";

            string CUISINE = " and cuisine = '" + cuisine + "' "; 
            if (string.IsNullOrEmpty(cuisine)|| cuisine.Equals("All")) CUISINE = "";

            string ASC_OR_DESC = "DESC";
            if (sequence == 1) ASC_OR_DESC = "ASC";

            string query = "SELECT * FROM dish where res_id =  " + resID + CUISINE + ORDER_BY + ASC_OR_DESC;
            MySqlDataReader reader = Select(query);
            int selectCount = 0;
            while(reader.Read())
            {
                if (reader.HasRows)
                {
                    DishStruct dish = new()
                    {
                        dish_id = reader["dish_id"].ToString(),
                        res_id = reader["res_id"].ToString(),
                        name = reader["name"].ToString(),
                        price = reader["price"].ToString(),
                        stock = reader["stock"].ToString(),
                        sale = reader["sale"].ToString(),
                        state = reader["state"].ToString(),
                        tag = reader["tag"].ToString(),
                        cuisine = reader["cuisine"].ToString(),
                        serve_type = reader["serve_type"].ToString(),
                        description = reader["description"].ToString(),
                        image = reader["image"].ToString()
                    };
                    selectCount++;
                    dishList.Add(dish);
                }
            }
            Debug.Log("Query Result: " + selectCount+ " dishes found");
            reader.Close();
        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
        }
        CloseSql();
        return dishList;
    }
    public static List<VoucherStruct> GetAllVouchersByResID(int resID, int sort = 0, int sequence = 0) // return all vouchers by resID with specified sequence
    {
        List<VoucherStruct> voucherList = new();
        try
        {
            OpenSql();
            string ORDER_BY = " ORDER BY voucher_id ";
            if (sort == 1) ORDER_BY = " ORDER BY discount ";
            if (sort == 2) ORDER_BY = " ORDER BY stock ";
            if (sort == 3) ORDER_BY = " ORDER BY start_time ";
            if (sort == 4) ORDER_BY = " ORDER BY end_time ";
            if (sort == 5) ORDER_BY = " ORDER BY start_date ";
            if (sort == 6) ORDER_BY = " ORDER BY end_date ";

            string ASC_OR_DESC = "DESC";
            if (sequence==1) ASC_OR_DESC = "ASC";

            string query = "SELECT * FROM voucher where res_id =  " + resID + ORDER_BY + ASC_OR_DESC;

            MySqlDataReader reader = Select(query);
            int selectCount = 0;
            while (reader.Read())
            {
                if (reader.HasRows)
                {
                    VoucherStruct voucher = new()
                    {
                        voucher_id = reader["voucher_id"].ToString(),
                        res_id = reader["res_id"].ToString(),
                        res_name = reader["res_name"].ToString(),
                        name = reader["name"].ToString(),
                        discount = reader["discount"].ToString(),
                        stock = reader["stock"].ToString(),
                        state = reader["state"].ToString(),
                        type = reader["type"].ToString(),
                        start_time = reader["start_time"].ToString(),
                        end_time = reader["end_time"].ToString(),
                        start_date = reader["start_date"].ToString(),
                        end_date = reader["end_date"].ToString(),
                        image = reader["image"].ToString()
                    };
                    voucherList.Add(voucher);
                    selectCount++;
                }
            }
            Debug.Log("Query Result: " + selectCount + " vouchers found");
            reader.Close();
        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
        }
        CloseSql();
        return voucherList;
    }
    public static List<VoucherStruct> GetAllAvailableVouchersByResID(int resID, int sort = 0, int sequence = 0) // return all vouchers by resID with specified sequence
    {
        List<VoucherStruct> voucherList = new();
        try
        {
            OpenSql();
            string ORDER_BY = " ORDER BY voucher_id ";
            if (sort == 1) ORDER_BY = " ORDER BY discount ";
            if (sort == 2) ORDER_BY = " ORDER BY stock ";
            if (sort == 3) ORDER_BY = " ORDER BY start_time ";
            if (sort == 4) ORDER_BY = " ORDER BY end_time ";
            if (sort == 5) ORDER_BY = " ORDER BY start_date ";
            if (sort == 6) ORDER_BY = " ORDER BY end_date ";

            string ASC_OR_DESC = "DESC";
            if (sequence == 1) ASC_OR_DESC = "ASC";

            string query = "SELECT * FROM voucher where state=1 and res_id =  " + resID + ORDER_BY + ASC_OR_DESC;

            MySqlDataReader reader = Select(query);
            int selectCount = 0;
            while (reader.Read())
            {
                if (reader.HasRows)
                {
                    VoucherStruct voucher = new()
                    {
                        voucher_id = reader["voucher_id"].ToString(),
                        res_id = reader["res_id"].ToString(),
                        res_name = reader["res_name"].ToString(),
                        name = reader["name"].ToString(),
                        discount = reader["discount"].ToString(),
                        stock = reader["stock"].ToString(),
                        state = reader["state"].ToString(),
                        type = reader["type"].ToString(),
                        start_time = reader["start_time"].ToString(),
                        end_time = reader["end_time"].ToString(),
                        start_date = reader["start_date"].ToString(),
                        end_date = reader["end_date"].ToString(),
                        image = reader["image"].ToString()
                    };
                    voucherList.Add(voucher);
                    selectCount++;
                }
            }
            Debug.Log("Query Result: " + selectCount + " vouchers found");
            reader.Close();
        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
        }
        CloseSql();
        return voucherList;
    }
    public static VoucherStruct GetMaxAvailableVouchersByResID(int resID) // return a available vouchers wiht max discount value by specify resID 
    {
        VoucherStruct voucher = new();
        try
        {
            OpenSql();
            string query = "SELECT * FROM voucher where res_id =  " + resID+ " and state = 1 and stock > 0 order by discount desc limit 1";

            MySqlDataReader reader = Select(query);
            int selectCount = 0;
            while (reader.Read())
            {
                if (reader.HasRows)
                {
                    voucher = new()
                    {
                        voucher_id = reader["voucher_id"].ToString(),
                        res_id = reader["res_id"].ToString(),
                        res_name = reader["res_name"].ToString(),
                        name = reader["name"].ToString(),
                        discount = reader["discount"].ToString(),
                        stock = reader["stock"].ToString(),
                        state = reader["state"].ToString(),
                        type = reader["type"].ToString(),
                        start_time = reader["start_time"].ToString(),
                        end_time = reader["end_time"].ToString(),
                        start_date = reader["start_date"].ToString(),
                        end_date = reader["end_date"].ToString(),
                        image = reader["image"].ToString()
                    };
                    selectCount++;
                }
            }
            Debug.Log("Query Result: " + selectCount + " vouchers found");
            reader.Close();
        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
        }
        CloseSql();
        return voucher;
    }
    public static List<VoucherListStruct> GetAllVouchersByCusID(int cusID, int sort = 0, int sequence = 0) // return all vouchers by resID with specified sequence
    {
        List<VoucherListStruct> voucherList = new();
        try
        {
            OpenSql();
            string ORDER_BY = " ORDER BY voucherList_id ";
            if (sort == 1) ORDER_BY = " ORDER BY discount ";
            if (sort == 2) ORDER_BY = " ORDER BY type ";
            if (sort == 3) ORDER_BY = " ORDER BY start_time ";
            if (sort == 4) ORDER_BY = " ORDER BY end_time ";
            if (sort == 5) ORDER_BY = " ORDER BY start_date ";
            if (sort == 6) ORDER_BY = " ORDER BY end_date ";

            string ASC_OR_DESC = "DESC";
            if (sequence == 1) ASC_OR_DESC = "ASC";

            string query = "SELECT * FROM voucher_list where cus_id =  " + cusID + ORDER_BY + ASC_OR_DESC;

            MySqlDataReader reader = Select(query);
            int selectCount = 0;
            while (reader.Read())
            {
                if (reader.HasRows)
                {
                    VoucherListStruct voucher = new()
                    {
                        voucherList_id = reader["voucherList_id"].ToString(),
                        voucher_id = reader["voucher_id"].ToString(),
                        res_id = reader["res_id"].ToString(),
                        res_name = reader["res_name"].ToString(),
                        name = reader["name"].ToString(),
                        discount = reader["discount"].ToString(),
                        type = reader["type"].ToString(),
                        start_time = reader["start_time"].ToString(),
                        end_time = reader["end_time"].ToString(),
                        start_date = reader["start_date"].ToString(),
                        end_date = reader["end_date"].ToString()
                    };
                    voucherList.Add(voucher);
                    selectCount++;
                }
            }
            Debug.Log("Query Result: " + selectCount + " vouchers found");
            reader.Close();
        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
        }
        CloseSql();
        return voucherList;
    }
    public static List<CommentStruct> GetAllCommentsByResID(int resID)
    {
        List<CommentStruct> commentList = new();
        try
        {
            OpenSql();
            string query = "SELECT * FROM comment where res_id =  " + resID;
            MySqlDataReader reader = Select(query);
            int selectCount = 0;
            while (reader.Read())
            {
                if (reader.HasRows)
                {
                    CommentStruct comment = new()
                    {
                        comment_id = reader["comment_id"].ToString(),
                        res_id = reader["res_id"].ToString(),
                        cus_id = reader["cus_id"].ToString(),
                        cus_name = reader["cus_name"].ToString(),
                        rate = reader["rate"].ToString(),
                        content = reader["content"].ToString(),
                        date = reader["date"].ToString(),
                        image = reader["image"].ToString()
                    };
                    commentList.Add(comment);
                    selectCount++;
                }
            }
            Debug.Log("Query Result: " + selectCount + " comments found");
            reader.Close();
        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
        }
        CloseSql();
        return commentList;
    }
    public static List<CommentStruct> GetAllCommentsByCusID(int cusID)
    {
        List<CommentStruct> commentList = new();
        try
        {
            OpenSql();
            string query = "SELECT * FROM comment where cus_id =  " + cusID;
            MySqlDataReader reader = Select(query);
            int selectCount = 0;
            while (reader.Read())
            {
                if (reader.HasRows)
                {
                    CommentStruct comment = new()
                    {
                        comment_id = reader["comment_id"].ToString(),
                        res_id = reader["res_id"].ToString(),
                        cus_id = reader["cus_id"].ToString(),
                        cus_name = reader["cus_name"].ToString(),
                        rate = reader["rate"].ToString(),
                        content = reader["content"].ToString(),
                        date = reader["date"].ToString(),
                        image = reader["image"].ToString()
                    };
                    commentList.Add(comment);
                    selectCount++;
                }
            }
            Debug.Log("Query Result: " + selectCount + " comments found");
            reader.Close();
        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
        }
        CloseSql();
        return commentList;
    }
    public static RestaurantStruct GetResByResID(int resID)
    {
        RestaurantStruct res = new();
        try
        {
            OpenSql();
            string query = "SELECT * FROM restaurant where res_id =  " + resID;
            MySqlDataReader reader = Select(query);
            int selectCount = 0;
            while (reader.Read())
            {
                if (reader.HasRows)
                {
                    res = new()
                    {
                        res_id = reader["res_id"].ToString(),
                        user_id = reader["user_id"].ToString(),
                        liscense_num = reader["liscense_num"].ToString(),
                        name = reader["name"].ToString(),
                        rate = reader["rate"].ToString(),
                        sale = reader["sale"].ToString(),
                        contact_num = reader["contact_num"].ToString(),
                        postcode = reader["postcode"].ToString(),
                        address = reader["address"].ToString(),
                        open_time = reader["open_time"].ToString(),
                        close_time = reader["close_time"].ToString(),
                        description = reader["description"].ToString(),
                        tag = reader["tag"].ToString(),
                        image = reader["image"].ToString()
                    };
                    selectCount++;
                }
            }
            Debug.Log("Query Result: " + selectCount + " restaurant found");
            reader.Close();
        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
        }
        CloseSql();
        return res;
    }
    public static CustomerStruct GetCusByCusID(int cusID)
    {
        CustomerStruct cus = new();
        try
        {
            OpenSql();
            string query = "SELECT * FROM customer where cus_id =  " + cusID;
            MySqlDataReader reader = Select(query);
            int selectCount = 0;
            while (reader.Read())
            {
                if (reader.HasRows)
                {
                    cus = new()
                    {
                        cus_id = reader["cus_id"].ToString(),
                        user_id = reader["user_id"].ToString(),
                        phone_num = reader["phone_num"].ToString(),
                        email_address = reader["email_address"].ToString(),
                        name = reader["name"].ToString(),
                        favor_dish = reader["favor_dish"].ToString(),
                        favor_res = reader["favor_res"].ToString(),
                        postcode = reader["postcode"].ToString(),
                        address = reader["address"].ToString(),
                        image = reader["image"].ToString()
                    };
                    selectCount++;
                }
            }
            Debug.Log("Query Result: " + selectCount + " customer found");
            reader.Close();
        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
        }
        CloseSql();
        return cus;
    }
    public static List<RestaurantStruct> GetAllRes(int sort = 0, int sequence = 0) // return all restaurant with specified sequence
    {
        List<RestaurantStruct> resList = new();
        try
        {
            OpenSql();
            string ORDER_BY = " ORDER BY res_id ";
            if (sort == 1) ORDER_BY = " ORDER BY rate ";
            if (sort == 2) ORDER_BY = " ORDER BY sale ";
            if (sort == 3) ORDER_BY = " ORDER BY postcode ";
            if (sort == 4) ORDER_BY = " ORDER BY open_time ";
            if (sort == 5) ORDER_BY = " ORDER BY close_time ";

            string ASC_OR_DESC = "DESC";
            if (sequence == 1) ASC_OR_DESC = "ASC";

            string query = "SELECT * FROM restaurant "  + ORDER_BY + ASC_OR_DESC;

            MySqlDataReader reader = Select(query);
            int selectCount = 0;
            while (reader.Read())
            {
                if (reader.HasRows)
                {
                    RestaurantStruct res = new()
                    {
                        res_id = reader["res_id"].ToString(),
                        user_id = reader["user_id"].ToString(),
                        liscense_num = reader["liscense_num"].ToString(),
                        name = reader["name"].ToString(),
                        rate = reader["rate"].ToString(),
                        sale = reader["sale"].ToString(),
                        contact_num = reader["contact_num"].ToString(),
                        postcode = reader["postcode"].ToString(),
                        address = reader["address"].ToString(),
                        open_time = reader["open_time"].ToString(),
                        close_time = reader["close_time"].ToString(),
                        description = reader["description"].ToString(),
                        tag = reader["tag"].ToString(),
                        max_discount = reader["max_discount"].ToString(),
                        image = reader["image"].ToString()
                    };
                    resList.Add(res);
                    selectCount++;
                }
            }
            Debug.Log("Query Result: " + selectCount + " restaurants found");
            reader.Close();
        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
        }
        CloseSql();
        return resList;
    }
    public static DishStruct GetDishByDishID(int dishID)
    {
        DishStruct dish = new();
        try
        {
            OpenSql();
            string query = "SELECT * FROM dish where dish_id =  " + dishID;
            MySqlDataReader reader = Select(query);
            int selectCount = 0;
            while (reader.Read())
            {
                if (reader.HasRows)
                {
                    dish = new()
                    {
                        dish_id = reader["dish_id"].ToString(),
                        res_id = reader["res_id"].ToString(),
                        name = reader["name"].ToString(),
                        price = reader["price"].ToString(),
                        stock = reader["stock"].ToString(),
                        sale = reader["sale"].ToString(),
                        state = reader["state"].ToString(),
                        tag = reader["tag"].ToString(),
                        cuisine = reader["cuisine"].ToString(),
                        serve_type = reader["serve_type"].ToString(),
                        description = reader["description"].ToString(),
                        image = reader["image"].ToString()
                    };
                    selectCount++;
                }
            }
            Debug.Log("Query Result: " + selectCount + " dishes found");
            reader.Close();
        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
        }
        CloseSql();
        return dish;
    }
    public static List<TagStruct> GetAllTag()
    {
        List<TagStruct> tagList = new();
        try
        {
            OpenSql();

            string query = "SELECT * FROM tag " ;

            MySqlDataReader reader = Select(query);
            int selectCount = 0;
            while (reader.Read())
            {
                if (reader.HasRows)
                {
                    TagStruct res = new()
                    {
                        tag_id = reader["tag_id"].ToString(),
                        res_id = reader["res_id"].ToString(),
                        name = reader["name"].ToString(),
                        type = reader["type"].ToString()
                    };
                    tagList.Add(res);
                    selectCount++;
                }
            }
            Debug.Log("Query Result: " + selectCount + " tags found");
            reader.Close();
        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
        }
        CloseSql();
        return tagList;
    }
    public static List<TagStruct> GetAllTagByResID(int resID)
    {
        List<TagStruct> tagList = new();
        try
        {
            OpenSql();

            string query = "SELECT * FROM tag where res_id = "+ resID;

            MySqlDataReader reader = Select(query);
            int selectCount = 0;
            while (reader.Read())
            {
                if (reader.HasRows)
                {
                    TagStruct res = new()
                    {
                        tag_id = reader["tag_id"].ToString(),
                        res_id = reader["res_id"].ToString(),
                        name = reader["name"].ToString(),
                        type = reader["type"].ToString()
                    };
                    tagList.Add(res);
                    selectCount++;
                }
            }
            Debug.Log("Query Result: " + selectCount + " tags found");
            reader.Close();
        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
        }
        CloseSql();
        return tagList;
    }
    public static List<OrderStruct> GetAllOrderByResID(int resID)
    {
        List<OrderStruct> orderList = new();
        try
        {
            OpenSql();

            string query = "SELECT * FROM `order` where res_id =  " + resID;

            MySqlDataReader reader = Select(query);
            int selectCount = 0;
            while (reader.Read())
            {
                if (reader.HasRows)
                {
                    OrderStruct order = new()
                    {
                        cus_name = reader["cus_name"].ToString(),
                        total_price = reader["total_price"].ToString(),
                        total_amount = reader["total_amount"].ToString(),
                        state = reader["state"].ToString(),
                    };
                    orderList.Add(order);
                    selectCount++;
                }
            }
            Debug.Log("Query Result: " + selectCount + " order found");
            reader.Close();
        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
        }
        CloseSql();
        return orderList;
    }
}

