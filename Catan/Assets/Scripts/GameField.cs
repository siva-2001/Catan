using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameField : MonoBehaviour
{
    private float thDistanceBetweenTilesBy_X = 8.5596f;
    private float thDistanceBetweenTilesBy_Z = 4.9419f;
    private int[] tileTipes = {1, 2, 2, 2, 3, 3, 3, 3, 4, 4, 4, 5, 5, 5, 5, 6, 6, 6, 6};
    private int[] tileNumbers = {11, 4, 8, 12, 2, 6, 10, 11, 9, 5, 3, 9, 3, 10, 8, 4, 6, 5};

    // Prefabs
    public GameObject[] prefabTiles;
    public GameObject prefabTile;

    public GameObject prefabBorder;
    public GameObject robbersPrefab;
    public GameObject placePrefab;
    public GameObject roadPrefab;

    // GameObjects
    [HideInInspector] public List<GameObject> tails = new List<GameObject>();
    [HideInInspector] public List<GameObject> buildPlaces = new List<GameObject>();
    [HideInInspector] public List<GameObject> roadPlaces = new List<GameObject>();
    GameObject robbers;

    Player nowMovingPlayer;

    //      Состояния активности обработчика нажатия на игровые объекты
    private bool constructionIsPossible = false;
    private bool roadConstructionIsPossible = false;
    private bool robbersCanMoves = false;

    //      Костыли для стартовой расстановки поселений
    List<int> startPlaceNumbers = new List<int>();
    Player [] players;
    bool theStartingLineUpOfSettlementIsUnderway = false;
    bool theStartingLineUpOfRoadIsUnderway = false;
    int movePlayerInd = 0;


    void Start(){
        InitField();
    }

    void Update(){
        CheckTheClick();
        //  Методы стартовой расстоновки поселений и дорог
        SetStartSettlement();
        SetStartRoad();

    }

    //     ___________________________     PRIVATE METHODS    ________________________________
    //     ___________________________________________________________________________________

    void InitField(){
        //      Генерация бортов поля
        Instantiate(prefabBorder, new Vector3(19.972f, 0, 0), Quaternion.Euler(0, 0, 0));
        Instantiate(prefabBorder, new Vector3(9.986f, 0, 17.296f), Quaternion.Euler(0, 300, 0));
        Instantiate(prefabBorder, new Vector3(-9.986f, 0, 17.296f), Quaternion.Euler(0, 240, 0));
        Instantiate(prefabBorder, new Vector3(-19.972f, 0, 0), Quaternion.Euler(0, 180, 0));
        Instantiate(prefabBorder, new Vector3(-9.986f, 0, -17.296f), Quaternion.Euler(0, 120, 0));
        Instantiate(prefabBorder, new Vector3(9.986f, 0, -17.296f), Quaternion.Euler(0, 60, 0));

        //      Генерация гексов
        var rnd = new System.Random();
        int tailNumInd = 0;
        float x = -2 * thDistanceBetweenTilesBy_X;
        for(int i = 1; i < 6; i++){
            int k = 5 - Math.Abs(3 - i);
            float z = -thDistanceBetweenTilesBy_Z * (k-1);
            for(int j = 0; j < k; j++){
                while(true){
                    int resourseNumber = rnd.Next(0, 19);
                    if( tileTipes[resourseNumber] != 0 ){
                        GameObject tail = Instantiate(prefabTile, new Vector3(x, 0, z), Quaternion.identity); // создание гекса
                        if (tileTipes[resourseNumber] == 1){
                            robbers = Instantiate(robbersPrefab, new Vector3(x+2f, 0, z+1f), Quaternion.identity);  // установка бандитов в пустыню или...
                            tail.GetComponent<Tail>().InitTail(0, tailNumInd, tileTipes[resourseNumber]-1); // ...присвоение номера гексу
                        }
                        else{
                            tail.GetComponent<Tail>().InitTail(tileNumbers[tailNumInd], tailNumInd, tileTipes[resourseNumber]-1); // ...присвоение номера гексу
                            tailNumInd++;
                        }
                        tails.Add(tail);                                // добавление тайла в лист тайлов игрового поля
                        tileTipes[resourseNumber] = 0;
                        break;
                    }
                }
                z += thDistanceBetweenTilesBy_Z * 2;
            }
            x += thDistanceBetweenTilesBy_X;
        }



        //      Генерация мест для строительства дорог и зданий
        List<GameObject> firstBuildPlaceRow = new List<GameObject>();
        List<GameObject> secondBuildPlaceRow = new List<GameObject>();
        x = -2.67f * thDistanceBetweenTilesBy_X;
        int placeNumber = 0;
        for(int i = 0; i < 12; i++){
            int k = 6 - Math.Abs(3 - ((i+1) / 2));      //___________код генерации мест построек
            float z = -thDistanceBetweenTilesBy_Z * (k-1);
            secondBuildPlaceRow = firstBuildPlaceRow.GetRange(0, firstBuildPlaceRow.Count);
            firstBuildPlaceRow.Clear();
            for(int j = 0; j < k; j++){
                    //___________код генерации мест построек
                    GameObject place = Instantiate(placePrefab, new Vector3(x, 0, z), Quaternion.identity);
                    place.GetComponent<BuildPlaceScript>().SetPlaceNumber(placeNumber++);
                    buildPlaces.Add(place);
                    z += thDistanceBetweenTilesBy_Z * 2;
                    //___________ создание рядов, на основе которых происходит генерация дорог
                    firstBuildPlaceRow.Add(place);
            }
            //                  Генерация дорог
            if(secondBuildPlaceRow.Count != 0){
                if(secondBuildPlaceRow.Count == firstBuildPlaceRow.Count){
                    //      Генерация дорог при одинаковом количестве узлов в рядах
                    for(int ind = 0; ind < firstBuildPlaceRow.Count; ind++){
                        float x1 = firstBuildPlaceRow[ind].GetComponent<Transform>().position.x;
                        float z1 = firstBuildPlaceRow[ind].GetComponent<Transform>().position.z;
                        float x2 = secondBuildPlaceRow[ind].GetComponent<Transform>().position.x;
                        float z2 = secondBuildPlaceRow[ind].GetComponent<Transform>().position.z;
                        GameObject road = Instantiate(roadPrefab,
                                                      new Vector3((x2+x1)/2, 0, (z2+z1)/2),
                                                      Quaternion.Euler(0, 90, 0));
                        road.GetComponent<RoadScript>().RoadInit(firstBuildPlaceRow[ind].GetComponent<BuildPlaceScript>().GetPlaceNumber(),
                                                                 secondBuildPlaceRow[ind].GetComponent<BuildPlaceScript>().GetPlaceNumber());
                        roadPlaces.Add(road);
                    }
                }
                //      Генерация дорог при большем количестве узлов во впереди идущем ряду
                if(secondBuildPlaceRow.Count == firstBuildPlaceRow.Count - 1){
                    for(int ind = 0; ind < firstBuildPlaceRow.Count; ind++){
                        if(ind >= 1){
                            float x1 = firstBuildPlaceRow[ind].GetComponent<Transform>().position.x;
                            float z1 = firstBuildPlaceRow[ind].GetComponent<Transform>().position.z;
                            float x2 = secondBuildPlaceRow[ind-1].GetComponent<Transform>().position.x;
                            float z2 = secondBuildPlaceRow[ind-1].GetComponent<Transform>().position.z;


                            GameObject road = Instantiate(roadPrefab,
                                                        new Vector3((x2+x1)/2, 0, (z2+z1)/2),
                                                        Quaternion.Euler(0, 30, 0));
                            road.GetComponent<RoadScript>().RoadInit(firstBuildPlaceRow[ind].GetComponent<BuildPlaceScript>().GetPlaceNumber(),
                                                                    secondBuildPlaceRow[ind-1].GetComponent<BuildPlaceScript>().GetPlaceNumber());
                            roadPlaces.Add(road);
                        }
                        if(ind < firstBuildPlaceRow.Count - 1){
                            float x1 = firstBuildPlaceRow[ind].GetComponent<Transform>().position.x;
                            float z1 = firstBuildPlaceRow[ind].GetComponent<Transform>().position.z;
                            float x2 = secondBuildPlaceRow[ind].GetComponent<Transform>().position.x;
                            float z2 = secondBuildPlaceRow[ind].GetComponent<Transform>().position.z;


                            GameObject road = Instantiate(roadPrefab,
                                                        new Vector3((x2+x1)/2, 0, (z2+z1)/2),
                                                        Quaternion.Euler(0, -30, 0));
                            road.GetComponent<RoadScript>().RoadInit(firstBuildPlaceRow[ind].GetComponent<BuildPlaceScript>().GetPlaceNumber(),
                                                                    secondBuildPlaceRow[ind].GetComponent<BuildPlaceScript>().GetPlaceNumber());
                            roadPlaces.Add(road);
                        }
                    }
                }
                //      Генерация дорог при большем количестве узлов во втором ряду
                if(secondBuildPlaceRow.Count == firstBuildPlaceRow.Count + 1){
                    for(int ind = 0; ind < firstBuildPlaceRow.Count; ind++){
                            float x1 = firstBuildPlaceRow[ind].GetComponent<Transform>().position.x;
                            float z1 = firstBuildPlaceRow[ind].GetComponent<Transform>().position.z;
                            float x2 = secondBuildPlaceRow[ind].GetComponent<Transform>().position.x;
                            float z2 = secondBuildPlaceRow[ind].GetComponent<Transform>().position.z;
                            float x3 = secondBuildPlaceRow[ind+1].GetComponent<Transform>().position.x;
                            float z3 = secondBuildPlaceRow[ind+1].GetComponent<Transform>().position.z;

                            GameObject road = Instantiate(roadPrefab,
                                                        new Vector3((x2+x1)/2, 0, (z2+z1)/2),
                                                        Quaternion.Euler(0, 30, 0));
                            GameObject road1 = Instantiate(roadPrefab,
                                                        new Vector3((x3+x1)/2, 0, (z3+z1)/2),
                                                        Quaternion.Euler(0, -30, 0));
                            road.GetComponent<RoadScript>().RoadInit(firstBuildPlaceRow[ind].GetComponent<BuildPlaceScript>().GetPlaceNumber(),
                                                                    secondBuildPlaceRow[ind].GetComponent<BuildPlaceScript>().GetPlaceNumber());
                            road1.GetComponent<RoadScript>().RoadInit(firstBuildPlaceRow[ind].GetComponent<BuildPlaceScript>().GetPlaceNumber(),
                                                                    secondBuildPlaceRow[ind+1].GetComponent<BuildPlaceScript>().GetPlaceNumber());
                            roadPlaces.Add(road1);
                            roadPlaces.Add(road);

                    }
                }
            }
            x += (thDistanceBetweenTilesBy_X/3.0f) * (i%2 + 1f);
        }

        //      Генерация гаваней
        {
            buildPlaces[1].GetComponent<BuildPlaceScript>().InitHarbor(new Resourse("All"));
            buildPlaces[2].GetComponent<BuildPlaceScript>().InitHarbor(new Resourse("All"));
            buildPlaces[4].GetComponent<BuildPlaceScript>().InitHarbor(new Resourse("All"));
            buildPlaces[6].GetComponent<BuildPlaceScript>().InitHarbor(new Resourse("All"));
            buildPlaces[21].GetComponent<BuildPlaceScript>().InitHarbor(new Resourse("All"));
            buildPlaces[27].GetComponent<BuildPlaceScript>().InitHarbor(new Resourse("All"));
            buildPlaces[50].GetComponent<BuildPlaceScript>().InitHarbor(new Resourse("All"));
            buildPlaces[53].GetComponent<BuildPlaceScript>().InitHarbor(new Resourse("All"));

            buildPlaces[7].GetComponent<BuildPlaceScript>().InitHarbor(new Resourse("Meadow"));
            buildPlaces[11].GetComponent<BuildPlaceScript>().InitHarbor(new Resourse("Meadow"));

            buildPlaces[15].GetComponent<BuildPlaceScript>().InitHarbor(new Resourse("Clay"));
            buildPlaces[20].GetComponent<BuildPlaceScript>().InitHarbor(new Resourse("Clay"));

            buildPlaces[37].GetComponent<BuildPlaceScript>().InitHarbor(new Resourse("Tree"));
            buildPlaces[42].GetComponent<BuildPlaceScript>().InitHarbor(new Resourse("Tree"));

            buildPlaces[38].GetComponent<BuildPlaceScript>().InitHarbor(new Resourse("Rock"));
            buildPlaces[43].GetComponent<BuildPlaceScript>().InitHarbor(new Resourse("Rock"));

            buildPlaces[48].GetComponent<BuildPlaceScript>().InitHarbor(new Resourse("Rye"));
            buildPlaces[52].GetComponent<BuildPlaceScript>().InitHarbor(new Resourse("Rye"));
        }

    }

    void CheckTheClick(){
    // _________________________________________________________________________________________________
    //                      Обработка нажатий на игровые объекты, вызывается каждый кадр в методе Update

        if (constructionIsPossible){
            foreach(GameObject place in buildPlaces){
                if(place.GetComponent<BuildPlaceScript>().wasPicked){
                    place.GetComponent<BuildPlaceScript>().SetBuildType(1, ref nowMovingPlayer);
                    foreach(GameObject _place in buildPlaces){_place.SetActive(false);}
                    place.GetComponent<BuildPlaceScript>().wasPicked = false;
                    constructionIsPossible = false;
                    break;
                }
            }
        }
        if (roadConstructionIsPossible){
            foreach(GameObject road in roadPlaces){
                if(road.GetComponent<RoadScript>().wasPicked){
                    road.GetComponent<RoadScript>().SetRoad(ref nowMovingPlayer);
                    foreach(GameObject _road in roadPlaces){_road.SetActive(false);}
                    road.GetComponent<RoadScript>().wasPicked = false;
                    roadConstructionIsPossible = false;
                    break;
                }
            }
        }
        if(robbersCanMoves){
            foreach(GameObject tail in tails){
                if(tail.GetComponent<Tail>().wasPicked && tail.GetComponent<Tail>().number != 0){
                    robbersCanMoves = false;
                    robbers.GetComponent<RobbersScript>().MoveToNewPlace(tail.GetComponent<Transform>().position.x,
                                                                         tail.GetComponent<Transform>().position.z,
                                                                         tail.GetComponent<Tail>().number);
                    tail.GetComponent<Tail>().wasPicked = false;
                }
            }
        }
    }

    void SetStartSettlement(){
        if(theStartingLineUpOfSettlementIsUnderway && !roadConstructionIsPossible){
            if(movePlayerInd < players.Length) nowMovingPlayer = players[movePlayerInd%players.Length];
            else nowMovingPlayer = players[players.Length-(movePlayerInd%players.Length)-1];
            constructionIsPossible = true;
            List<int> forbiddenPlaces = new List<int>();
            foreach(GameObject place in buildPlaces){
                int _placeNum = place.GetComponent<BuildPlaceScript>().GetPlaceNumber();
                if(startPlaceNumbers.Exists(x => x == _placeNum)){
                    forbiddenPlaces.Add(_placeNum);
                    foreach(GameObject road in roadPlaces){
                        if(road.GetComponent<RoadScript>().GetBuildPlace1() == _placeNum){
                            forbiddenPlaces.Add(road.GetComponent<RoadScript>().GetBuildPlace2());
                        }
                        if(road.GetComponent<RoadScript>().GetBuildPlace2() == _placeNum){
                            forbiddenPlaces.Add(road.GetComponent<RoadScript>().GetBuildPlace1());
                        }
                    }
                }
            }



            foreach(GameObject place in buildPlaces){
                if(!forbiddenPlaces.Exists(x => x == place.GetComponent<BuildPlaceScript>().GetPlaceNumber())){
                    place.SetActive(true);
                }
                place.GetComponent<BuildPlaceScript>().wasPicked = false;
            }
            theStartingLineUpOfRoadIsUnderway = true;
            theStartingLineUpOfSettlementIsUnderway = false;
        }
    }

    void SetStartRoad(){
        if(theStartingLineUpOfRoadIsUnderway && !constructionIsPossible){
            int newBuilding = 0;
            foreach(GameObject place in buildPlaces){
                if(!startPlaceNumbers.Exists(x => x == place.GetComponent<BuildPlaceScript>().GetPlaceNumber()) &&
                    place.GetComponent<BuildPlaceScript>().GetBuildType() != 0){
                    newBuilding = place.GetComponent<BuildPlaceScript>().GetPlaceNumber();
                    startPlaceNumbers.Add(newBuilding);
                }
            }
            roadConstructionIsPossible = true;

            foreach(GameObject road in roadPlaces){
                 if(road.GetComponent<RoadScript>().GetBuildPlace1() == newBuilding ||
                    road.GetComponent<RoadScript>().GetBuildPlace2() == newBuilding){
                     road.SetActive(true);
                 }
             }

            if(movePlayerInd < players.Length * 2 - 1){
                movePlayerInd++; theStartingLineUpOfSettlementIsUnderway = true;
            }
            theStartingLineUpOfRoadIsUnderway = false;
        }
    }

    //  _____________________________      PUBLIC METHODS      _______________________________
    //  ______________________________________________________________________________________

    public void BuildingStartConstractionActive(Player [] plyrs){
    //  ЗАПУСКАЕТ СТАРТОВУЮ РАССТАНОВКУ ПОСЕЛЕНИЙ И ДОРОГ

        players = plyrs;
        theStartingLineUpOfSettlementIsUnderway = true;
    }

    public void SetMovingPlayer(ref Player plyr){nowMovingPlayer = plyr;}

    //______________________________________________________________________________________________________
    //      Ниже идёт активация и отмена отображения мест для строительства и передвижения разбойников.
    //      Изменение состояний возможности передвижения разбойников и строительства передаются полю
    //      от главного игрового объекта за счёт данных публичных методов.

    public void BuildingConstractionActive(){
        constructionIsPossible = true;
        foreach(GameObject place in buildPlaces){
            bool theirIsRoadOfTheCurrentPlayer = false;
            List<int> adjacentBuildPlaces = new List<int>();

            //      Определяются соседние для текущего перекрёстка перекрёстки и ведёт ли к нему дорога текущего игрока
            foreach(GameObject _road in roadPlaces){
                if(_road.GetComponent<RoadScript>().GetBuildPlace1() == place.GetComponent<BuildPlaceScript>().GetPlaceNumber()){
                    adjacentBuildPlaces.Add(_road.GetComponent<RoadScript>().GetBuildPlace2());
                    if(_road.GetComponent<RoadScript>().GetRoadExisting()){
                        if(_road.GetComponent<RoadScript>().GetPlayer() == nowMovingPlayer) theirIsRoadOfTheCurrentPlayer = true;
                    }
                }
                if(_road.GetComponent<RoadScript>().GetBuildPlace2() == place.GetComponent<BuildPlaceScript>().GetPlaceNumber()){
                    adjacentBuildPlaces.Add(_road.GetComponent<RoadScript>().GetBuildPlace1());
                    if(_road.GetComponent<RoadScript>().GetRoadExisting()){
                        if(_road.GetComponent<RoadScript>().GetPlayer() == nowMovingPlayer) theirIsRoadOfTheCurrentPlayer = true;
                    }
                }
            }

            //      Если дорога текущего игрока ведёт к данному перекрёстку и соседние перекрёстки пусты перекрёсток становится активен
            if(theirIsRoadOfTheCurrentPlayer){
                bool constructionInPlaceIsPossible = true;
                foreach(GameObject _place in buildPlaces){
                    foreach(int ind in adjacentBuildPlaces){
                        if(ind == _place.GetComponent<BuildPlaceScript>().GetPlaceNumber() &&
                           _place.GetComponent<BuildPlaceScript>().GetBuildType() != 0){constructionInPlaceIsPossible = false; break;}
                    }
                }
                place.SetActive(constructionInPlaceIsPossible);
            }
            place.GetComponent<BuildPlaceScript>().wasPicked = false;
        }
    }

    public void RoadConstractionActive(){
        roadConstructionIsPossible = true;
        foreach(GameObject road in roadPlaces){
            road.GetComponent<RoadScript>().wasPicked = false;
            if( road.GetComponent<RoadScript>().GetRoadExisting() && road.GetComponent<RoadScript>().GetPlayer() == nowMovingPlayer){
                int buildPlaceNum1 = road.GetComponent<RoadScript>().GetBuildPlace1();
                int buildPlaceNum2 = road.GetComponent<RoadScript>().GetBuildPlace2();
                foreach(GameObject adjacentRoad in roadPlaces){
                    if((adjacentRoad.GetComponent<RoadScript>().GetBuildPlace1() == buildPlaceNum1 ||
                        adjacentRoad.GetComponent<RoadScript>().GetBuildPlace1() == buildPlaceNum2 ||
                        adjacentRoad.GetComponent<RoadScript>().GetBuildPlace2() == buildPlaceNum1 ||
                        adjacentRoad.GetComponent<RoadScript>().GetBuildPlace2() == buildPlaceNum2) &&
                        !adjacentRoad.GetComponent<RoadScript>().GetRoadExisting()){
                        adjacentRoad.SetActive(true);
                    }
                }
            }
        }
    }

    public void RobbersActive(){
            // Активация возможности передвижения разбойников текущим игроком

        robbersCanMoves = true;
        foreach(GameObject tail in tails){
            tail.GetComponent<Tail>().wasPicked = false;
        }
    }

    public void CancelClick(){
    //_________________________________________________________________________________________________________________
    //                                                                      ОТМЕНА АКТИВАЦИИ РЕАГИРОВАНИЯ НА ВСЕ НАЖАТИЯ

        robbersCanMoves=false;
        constructionIsPossible=false;
        roadConstructionIsPossible=false;
        foreach(GameObject road in roadPlaces){road.SetActive(false);}
        foreach(GameObject place in buildPlaces){place.SetActive(false);}
    }
}



























