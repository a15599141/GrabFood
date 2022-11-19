using Unity.VisualScripting;
using UnityEngine;

public class UserManager : MonoBehaviour
{
    private static GameObject UserCanvasPrefab;
    public static int UserID;
    public static string UserName;
    public static int ResOrCusID;
    public static int UserType;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public static void SpawnUserCanvas(int userID, string userName , int usertype)
    {
        UserID = userID;
        UserName = userName;
        UserType = usertype;

        if (UserType == 1)
        {
            ResOrCusID = MySqlManager.GetResIdByUserID(userID);
            UserCanvasPrefab = (GameObject)Resources.Load("Prefabs/RestaurantPage");
            Instantiate(UserCanvasPrefab);
            Debug.Log("User Login: userID = " + UserID + " username = " + UserName + " usertype = " + UserType + " res_id = " + ResOrCusID);
        }
        else if (UserType == 2) 
        {
            ResOrCusID = MySqlManager.GetCusIdByUserID(userID);
            UserCanvasPrefab = (GameObject)Resources.Load("Prefabs/CustomerPage");
            Instantiate(UserCanvasPrefab);
            Debug.Log("User Login: userID = "+UserID+" username = "+UserName+ " usertype = "+ UserType + " cus_id = "+ResOrCusID);
        } 
        else TipsManager.SpawnTips("Login Error");
    }

    public void DestoryUser()
    {
        Destroy(gameObject);
    }

    public static int ResetPassword(string newPassword)
    {
        string query = "UPDATE `data`.`user` SET  `password` = '" + newPassword + "'  WHERE `user_id` = " + UserID;
        return MySqlManager.Update(query);
    }

    public static int DeleteUser()
    {
        string table = "";
        string id = "";
        if (UserType == 1)
        {
            table = "restaurant";
            id = "res_id";
        }
        if (UserType == 2)
        {
            table = "customer";
            id = "cus_id";
        }
        string query = "DELETE FROM `data`.`user` where user_id = " + UserID + " ;"+ " DELETE FROM "+ table + " where "+ id +" = " + ResOrCusID + " ;";

        return MySqlManager.Delete(query);
    }
}
