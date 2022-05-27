using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Tail : MonoBehaviour, IPointerClickHandler
{
    public int number = 0;
    public int tailIndex;
    public bool wasPicked = false;
    private Resourse resourse;
    public GameObject[] prefabNumbers;
    public GameObject numberObject;
    public Material[] materials;



    public Resourse GetResourse(){return resourse;}
    public void OnPointerClick(PointerEventData eventData){wasPicked = true;}

    public void InitTail(int num, int tailInd, int res){
        number = num;
        if(num != 0){numberObject = Instantiate(prefabNumbers[(number-2)-(number/7)], new Vector3(transform.position.x - 2f, -0.2f, transform.position.z - 1f), Quaternion.Euler(0, 30, 0));}
        gameObject.GetComponent<MeshRenderer>().material = materials[res];

        tailIndex = tailInd;
        switch(res){
            case 1:
                resourse = new Resourse("Tree");
                break;
            case 2:
                resourse = new Resourse("Clay");
                break;
            case 3:
                resourse = new Resourse("Rock");
                break;
            case 4:
                resourse = new Resourse("Rye");
                break;
            case 5:
                resourse = new Resourse("Meadow");
                break;
            case 0:
                resourse = null;
                break;
        }
    }
}
