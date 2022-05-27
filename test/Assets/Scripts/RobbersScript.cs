using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobbersScript : MonoBehaviour
{
    int moveIndex = 0;
    float speedX = 0;
    float speedZ = 0;
    float posY = 0;
    int robbersSpeed = 100; // Количество кадров за которые перемещаются разбойники
    int tailNumber = 0;

    public void MoveToNewPlace(float posX, float posZ, int tailNum){
        speedX = (posX + 2f - transform.position.x) / (float)robbersSpeed;
        speedZ = (posZ + 1f - transform.position.z) / (float)robbersSpeed;
        moveIndex = robbersSpeed;
        tailNumber = tailNum;
    }

    // Update is called once per frame
    void FixedUpdate(){
        if(moveIndex > 0){
            if(moveIndex > 0 && moveIndex < robbersSpeed/5){posY-=0.25f;}
            if(moveIndex > (4*robbersSpeed)/5 && moveIndex < robbersSpeed){posY+=0.25f;}


            transform.position = new Vector3(
                transform.position.x + speedX,
                posY,
                transform.position.z + speedZ);

            moveIndex--;
        }
        else{ speedX = 0; speedZ = 0; }
    }
}




