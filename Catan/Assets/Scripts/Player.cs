using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player{// : MonoBehaviour{
    string color;

    public Player(string clr){
        color = clr;
    }
    public string GetColor(){return color;}

    public static bool operator ==(Player player1, Player player2)
    {
        return player1.color == player2.color;
    }
        public static bool operator !=(Player player1, Player player2)
    {
        return player1.color != player2.color;
    }

}
