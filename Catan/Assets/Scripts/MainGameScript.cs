using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainGameScript : MonoBehaviour
{
    string[] playerColors = {"Red", "Blue", "Green", "Orange"};

    [SerializeField]GameObject gameField;
    Player [] players;
    [SerializeField] int nowMovingPlayerIndex = 0;

    [SerializeField] bool changePlayer = false;
    [SerializeField] bool buildSettlement = false;
    [SerializeField] bool buildRoad = false;
    [SerializeField] bool moveRobbers = false;


    void Start(){
        CreatePlayers();
        Player player = players[0];
        gameField.GetComponent<GameField>().BuildingStartConstractionActive(players);
    }

    void Update(){
         CheckChangePlayer();
         CheckBuildSettlement();
         CheckBuildRoad();
         CheckRobbers();
    }

//  внешние функции-заглушки интерфейса

    void CheckBuildSettlement(){
        if(buildSettlement){
            gameField.GetComponent<GameField>().BuildingConstractionActive();
            buildSettlement = false;
        }
    }
    void CheckBuildRoad(){
        if(buildRoad){
            gameField.GetComponent<GameField>().RoadConstractionActive();
            buildRoad = false;
        }
    }
    void CheckRobbers(){
        if(moveRobbers){
            gameField.GetComponent<GameField>().RobbersActive();
            moveRobbers = false;
        }
    }
    void CheckChangePlayer(){
        if(changePlayer){
            nowMovingPlayerIndex = (nowMovingPlayerIndex + 1) % players.Length;
            Player nowMovingPlayer = players[nowMovingPlayerIndex];
            gameField.GetComponent<GameField>().SetMovingPlayer(ref nowMovingPlayer);
            changePlayer = false;
        }
    }



    void CreatePlayers(){
        List<Player> _players = new List<Player>();
        for(int i = 0; i < 4; i++){
            _players.Add(new Player(playerColors[i]));
        }
        //players = new int[] {_players[0],_players[1],_players[2]};
        players = new Player[] {_players[0],_players[1],_players[2], _players[3]};
    }
}
