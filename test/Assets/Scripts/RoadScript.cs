using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class RoadScript : MonoBehaviour, IPointerClickHandler{
    float rotation;
    Player player;
    bool roadExisting = false;
    GameObject road;
    public GameObject roadPrefab_Orange;
    public GameObject roadPrefab_Green;
    public GameObject roadPrefab_Blue;
    public GameObject roadPrefab_Red;
    public bool wasPicked = false;
    [SerializeField] int firstBuilding;
    [SerializeField] int secondBuilding;

    public Player GetPlayer(){
        return player;
    }
    public bool GetRoadExisting(){
       return roadExisting;
    }

    public int GetBuildPlace1(){return firstBuilding;}
    public int GetBuildPlace2(){return secondBuilding;}

    public void SetRoad(ref Player plyr){  // Player in param
        roadExisting = true;
        player = plyr;
        ////                                    ТАК ЖЕ КАК С ПОСЕЛЕНИЯМИ
         switch(plyr.GetColor()){
            case "Red":
                road = Instantiate(roadPrefab_Red, new Vector3(transform.position.x, 0, transform.position.z), transform.rotation);
                break;
            case "Blue":
                road = Instantiate(roadPrefab_Blue, new Vector3(transform.position.x, 0, transform.position.z), transform.rotation);
                break;
            case "Green":
                road = Instantiate(roadPrefab_Green, new Vector3(transform.position.x, 0, transform.position.z), transform.rotation);
                break;
            case "Orange":
                road = Instantiate(roadPrefab_Orange, new Vector3(transform.position.x, 0, transform.position.z), transform.rotation);
                break;
        }
    }

    public void RoadInit(int fB, int sB){
        firstBuilding = fB;
        secondBuilding = sB;
    }

    public void OnPointerClick(PointerEventData eventData){wasPicked = true;}
}
