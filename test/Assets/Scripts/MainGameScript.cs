using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainGameScript : MonoBehaviour
{
    string[] playerColors = {"Red", "Blue", "Green", "Orange"};

    [SerializeField]GameObject gameField;
    Player [] players;



    void Start(){
        CreatePlayers();
        Player player = players[0];
        gameField.GetComponent<GameField>().BuildingStartConstractionActive(players);
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
