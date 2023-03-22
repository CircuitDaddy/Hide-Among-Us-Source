using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
public class RegionSelection : MonoBehaviour
{
    public static RegionSelection instance;
    public Dropdown RegionDropDown;

    public static List<string> PhotonRegions = new List<string>()
        {
            "Asia",
            "Australia",
            "Canada, East",
            "Europe",
            "India",
            "Japan",
            "Russia",
            "Russia, East",
            "South America",
            "South Korea",
            "USA, East",
            "USA, West"
        };

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }
    private void Start()
    {
        AddRegionToDropDown(RegionDropDown);
    }

    public static int ConvertCodeToRegion(string value)
    {
        switch (value)
        {
            case "asia":
                return 0;

            case "au":
                return 1;

            case "cae":
                return 2;

            case "eu":
                return 3;
            case "in":
                return 4;

            case "jp":
                return 5;

            case "ru":
                return 6;

            case "rue":
                return 7;

            case "sa":
                return 8;

            case "kr":
                return 9;

            case "us":
                return 10;

            case "usw":
                return 11;

        }
        return 0;
    }

    public static string ConvertRegionToCode(int value)
    {
        switch (value)
        {
            case 0:
                return "asia";

            case 1:
                return "au";

            case 2:
                return "cae";

            case 3:
                return "eu";
            case 4:
                return "in";

            case 5:
                return "jp";

            case 6:
                return "ru";

            case 7:
                return "rue";

            case 8:
                return "sa";

            case 9:
                return "kr";

            case 10:
                return "us";

            case 11:
                return "usw";
        }
        return "";
    }
    public void AddRegionToDropDown(Dropdown regionList)
    {
        regionList.AddOptions(PhotonRegions);
    }
    public void ChangeRegion()
    {
        PhotonNetwork.Disconnect();

        if (PhotonNetwork.ConnectToRegion(ConvertRegionToCode(RegionDropDown.value)))
        {
            PhotonNetwork.ConnectUsingSettings();
        }
    }
}
