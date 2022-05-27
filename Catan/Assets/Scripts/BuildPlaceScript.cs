using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class BuildPlaceScript : MonoBehaviour, IPointerClickHandler{
    [SerializeField] private int placeNumber;
    public bool wasPicked = false;
    [SerializeField] private int buildType = 0;
    GameObject building;
    Player player;
    public GameObject settlementPrefab_Red;
    public GameObject settlementPrefab_Green;
    public GameObject settlementPrefab_Blue;
    public GameObject settlementPrefab_Orange;



    public GameObject cityPrefab;
    public GameObject harborPrefab;
    public GameObject forestHarborPrefab;
    public GameObject rockHarborPrefab;
    public GameObject meadowHarborPrefab;
    public GameObject clayHarborPrefab;
    public GameObject ryeHarborPrefab;
    GameObject harborObj;
    Resourse harbor;



    public int GetBuildType(){ return buildType; }
    public void SetPlaceNumber(int num){ placeNumber = num; }
    public int GetPlaceNumber(){ return placeNumber; }
    public void InitHarbor(Resourse res){
        harbor = res;
        switch(res.type){
            case "Tree":
                harborObj = Instantiate(forestHarborPrefab, new Vector3(transform.position.x, 0, transform.position.z), Quaternion.identity);
                break;
            case "Clay":
                harborObj = Instantiate(clayHarborPrefab, new Vector3(transform.position.x, 0, transform.position.z), Quaternion.identity);
                break;
            case "Rock":
                harborObj = Instantiate(rockHarborPrefab, new Vector3(transform.position.x, 0, transform.position.z), Quaternion.identity);
                break;
            case "Meadow":
                harborObj = Instantiate(meadowHarborPrefab, new Vector3(transform.position.x, 0, transform.position.z), Quaternion.identity);
                break;
            case "Rye":
                harborObj = Instantiate(ryeHarborPrefab, new Vector3(transform.position.x, 0, transform.position.z), Quaternion.identity);
                break;
            case "All":
                harborObj = Instantiate(harborPrefab, new Vector3(transform.position.x, 0, transform.position.z), Quaternion.identity);
                break;
        }

    }

    public void SetBuildType(int type, ref Player plyr){
        buildType = type;   //  1 - settlement     2 - city

        switch(plyr.GetColor()){
            case "Red":
                building = Instantiate(settlementPrefab_Red, new Vector3(transform.position.x, 0, transform.position.z), Quaternion.identity);
                break;
            case "Blue":
                building = Instantiate(settlementPrefab_Blue, new Vector3(transform.position.x, 0, transform.position.z), Quaternion.identity);
                break;
            case "Green":
                building = Instantiate(settlementPrefab_Green, new Vector3(transform.position.x, 0, transform.position.z), Quaternion.identity);
                break;
            case "Orange":
                building = Instantiate(settlementPrefab_Orange, new Vector3(transform.position.x, 0, transform.position.z), Quaternion.identity);
                break;
        }

        player = plyr;

    }


    public void OnPointerClick(PointerEventData eventData){wasPicked = true;}
}
