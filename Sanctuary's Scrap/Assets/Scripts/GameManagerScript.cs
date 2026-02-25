using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;
using UnityEngine.UI;

public class GameManagerScript : MonoBehaviour
{
    public GameObject player;
    public bool charSelect;
    public GameObject charPanel;
    public TMP_Text roomCountUI;

    public GameObject enemy;
    public EnemyScriptable[] enemyStats;
    public int enemyToSpawn;

    public WaveTypeScriptable[] waveStats;
    public int waveRand;
    public int numbEnemy1Used;
    public int numbEnemy2Used;
    public int numbEnemy3Used;
    public int numbEnemy4Used;
    public int numbEnemy5Used;

    public GameObject[] room0Array;
    public GameObject[] roomArray;
    public GameObject currentRoom;
    public List<GameObject> currentDoors;
    public List<GameObject> enemySpawns;
    private float startTime;
    public bool[] usedSpawns;
    public int wave;
    public WaveTypeScriptable currentWave;
    public int spawnTest;
    public int randSpawn;
    public static int enemiesActive;
    public bool enemySpawning;
    public GameObject rewardSpawn;
    public GameObject rewardChest;
    public GameObject currentChest;
    public GameObject currentPlayerSpawn;
    public bool roomClear;
    public int roomCount;

    public int nextRoom1;
    public int nextRoom2;
    public int nextRoomRandScrap1;
    public int nextRoomRandScrap2;

    public static int thisRoomRandScrap;

    public float shopProb;

    void Start()
    {
        EventManager.current.RoomRewardChosen += OnRoomRewardChosen;
        EventManager.current.StartRoom += RoomSpawn;
        EventManager.current.CharChosen += OnCharChosen;
        EventManager.current.StartEnemySpawn += OnStartEnemySpawn;
        EventManager.current.EnemyKilled += OnEnemyKilled;
        StartUp();
        startTime = Time.time;
    }

    public void StartUp()
    {
        charPanel.SetActive(true);
        EventManager.current.onPlayerOpenMenu();
        StartRoomSpawn();
    }
    private void OnCharChosen()
    {
        charPanel.SetActive(false);
        EventManager.current.onPlayerCloseMenu();
    }

    public void StartRoomSpawn()
    {
        currentRoom = Instantiate(room0Array[0], new Vector3(0, 0, 0), Quaternion.identity);
        currentPlayerSpawn = currentRoom.transform.GetChild(0).gameObject;
        player.transform.position = currentPlayerSpawn.transform.position;
        rewardSpawn = currentRoom.transform.GetChild(2).transform.gameObject;
        enemySpawning = false;
        roomClear = false;
        SpawnReward();
        roomCount = 0;
        thisRoomRandScrap = -1;
        roomCountUI.text = roomCount.ToString();
    }
    public void RoomSpawn()
    {
        Destroy(currentRoom);
        if (enemySpawns.Count != 0)
        {
            enemySpawns.Clear();
        }
        wave = 0;
        int roomRand = Random.Range(0, roomArray.Length);
        currentRoom = Instantiate(roomArray[roomRand], new Vector3(0, 0, 0), Quaternion.identity);
        for (int i = 0; i < currentRoom.transform.GetChild(1).childCount; i++)
        {
            enemySpawns.Add(roomArray[roomRand].transform.GetChild(1).GetChild(i).gameObject);
        }
        rewardSpawn = currentRoom.transform.GetChild(2).transform.gameObject;
        enemySpawning = true;
        roomClear = false;
        roomCount += 1;
        if (NewPlayerMove.doorChosen == 1)
        {
            thisRoomRandScrap = nextRoom1;
        }
        else if (NewPlayerMove.doorChosen == 2)
        {
            thisRoomRandScrap = nextRoom2;
        }
        roomCountUI.text = roomCount.ToString();
        currentPlayerSpawn = currentRoom.transform.GetChild(0).gameObject;
        player.GetComponent<CharacterController>().enabled = false;
        player.transform.position = currentPlayerSpawn.transform.position;
        player.GetComponent<CharacterController>().enabled = true;
        //Debug.Log("Player spawn location = " + currentPlayerSpawn.transform.position);
        Debug.Log(("This room rand scrap = ") +thisRoomRandScrap.ToString());
        wave = 0;
        EventManager.current.onStartEnemySpawn();
    }
    void Update()
    {
        if (charPanel.activeInHierarchy == true)
        {
            EventManager.current.onPlayerOpenMenu();
        }


        /*if (wave < 2 && enemysActive <= 0 && enemySpawning == true)
        {
            wave = wave + 1;
            usedSpawns = new List<GameObject>();
            if ((enemySpawns.Count / 2) + (roomCount - 1) < enemySpawns.Count)
            {
                spawnTest = (enemySpawns.Count / 2) + (roomCount - 1);
            }
            else if ((enemySpawns.Count / 2) + (roomCount - 1) >= enemySpawns.Count)
            {
                spawnTest = enemySpawns.Count;
            }
            for (int i = 0; i < spawnTest; i++)
            {
                clear = 0;
                randSpawn = Random.Range(0, enemySpawns.Count);
                if (usedSpawns.Count != 0)
                {
                    for (int j = 0; j < usedSpawns.Count; j++)
                    {
                        if (enemySpawns[randSpawn] != usedSpawns[j])
                        {
                            clear = clear + 1;
                        }
                    }
                }
                else if (usedSpawns.Count == 0)
                {
                    clear = 0;
                }
                if (clear == usedSpawns.Count && usedSpawns.Count != spawnTest)
                {
                    //GameObject lastEnemy = Instantiate(enemy, enemySpawns[randSpawn].transform.position, Quaternion.identity);
                    GameObject lastEnemy = EnemyPoolManager.ePInstance.GetEnemy();
                    lastEnemy.transform.position = enemySpawns[randSpawn].transform.position;
                    lastEnemy.transform.rotation = Quaternion.identity;
                    lastEnemy.SetActive(true);
                    int enemyTypeRand = Random.Range(0, enemyStats.Length);
                    lastEnemy.GetComponent<NewAiNavScript>().enemyStats = enemyStats[enemyTypeRand];
                    lastEnemy.gameObject.transform.localScale = new Vector3(enemyStats[enemyTypeRand].size, enemyStats[enemyTypeRand].size, enemyStats[enemyTypeRand].size);
                    lastEnemy.GetComponent<NewAiNavScript>().roomCount = roomCount;
                    usedSpawns.Add(enemySpawns[randSpawn]);
                    enemysActive += 1;
                }
                else if (clear < usedSpawns.Count)
                {
                    i = i - 1;
                }
                else if (usedSpawns.Count == spawnTest)
                {
                    break;
                }
            }
        }
        else if (wave == 2 && enemysActive == 0)
        {
            enemySpawning = false;
            roomClear = true;
            SpawnReward();
            wave = wave + 1;
        }*/
    }
    public void OnStartEnemySpawn()
    {
        wave += 1;
        waveRand = Random.Range(0, (waveStats.Length - 1));
        currentWave = waveStats[waveRand];
        usedSpawns = new bool[enemySpawns.Count];
        enemiesActive = currentWave.totalEnemyCount;
        numbEnemy1Used = 0;
        numbEnemy2Used = 0;
        numbEnemy3Used = 0;
        numbEnemy4Used = 0;
        numbEnemy5Used = 0;

        for (int i = 0; i < currentWave.totalEnemyCount; i++) // for the number of enemies in the first wave
        {
            if (currentWave.enemy1Count > numbEnemy1Used)
            {
                enemyToSpawn = 1;
                numbEnemy1Used += 1;
            }
            else if (currentWave.enemy2Count > numbEnemy2Used)
            {
                enemyToSpawn = 2;
                numbEnemy2Used += 1;
            }
            else if (currentWave.enemy3Count > numbEnemy2Used)
            {
                enemyToSpawn = 3;
                numbEnemy2Used += 1;
            }
            else if (currentWave.enemy4Count > numbEnemy4Used)
            {
                enemyToSpawn = 4;
                numbEnemy2Used += 1;
            }
            else if (currentWave.enemy5Count > numbEnemy5Used)
            {
                enemyToSpawn = 5;
                numbEnemy2Used += 1;
            }
            OnRandSpawn();
        }
    }
    public void OnRandSpawn()
    {
        randSpawn = Random.Range(0, (enemySpawns.Count - 1)); // pick a random spawn point
        OnRandSpawnCheck();
    }
    public void OnRandSpawnCheck()
    {
        for (int j = 0; j < usedSpawns.Length; j++)
        {
            if (j == randSpawn)
            {
                if (usedSpawns[j] == true)
                {
                    OnRandSpawn();
                    break;
                }
                else if(usedSpawns[j] != true)
                {
                    OnSpawnEnemy();
                    break;
                }
            }
        }
    }
    public void OnSpawnEnemy()
    {
        GameObject lastEnemy = EnemyPoolManager.ePInstance.GetEnemy();
        lastEnemy.transform.position = enemySpawns[randSpawn].transform.position;
        lastEnemy.transform.rotation = Quaternion.identity;
        lastEnemy.SetActive(true);
        lastEnemy.GetComponent<NewAiNavScript>().enemyStats = enemyStats[enemyToSpawn - 1];
        lastEnemy.GetComponent<NewAiNavScript>().OnEnemyActivated();
        usedSpawns[randSpawn] = true;
    }

    public void OnEnemyKilled()
    {
        enemiesActive -= 1;
        if (enemiesActive <= 0)
        {
            if (wave == 1)
            {
                OnStartEnemySpawn();
            }
            else if (wave >= 2)
            {
                enemySpawning = false;
                roomClear = true;
                SpawnReward();
            }
        }
    }

    public void SpawnReward()
    {
        currentChest = Instantiate(rewardChest, rewardSpawn.transform.position, Quaternion.identity);
    }
    public void OnRoomRewardChosen()
    {
        int nextRoomRand = Random.Range(0, 100);
        if (nextRoomRand >= shopProb)
        {
            nextRoom1 = 5;
        }
        if (nextRoomRand < shopProb)
        {
            nextRoomRandScrap1 = Random.Range(0, 5);
            nextRoom1 = nextRoomRandScrap1;
        }
        nextRoomRandScrap2 = Random.Range(0, 5);
        if (nextRoomRandScrap2 == nextRoomRandScrap1)
        {
            if (nextRoom1 == 4 || nextRoom1 == 5)
            {
                nextRoom2 = 0;
            }
            else if (nextRoom1 != 4)
            {
                nextRoom2 = nextRoom1 + 1;
            }
        }
        else
        {
            nextRoom2 = nextRoomRandScrap2;
        }


        Destroy(currentChest.gameObject);
        currentDoors = new List<GameObject>();
        for (int i = 0; i < currentRoom.transform.GetChild(3).GetChild(3).childCount; i++)
        {
            if (currentRoom.transform.GetChild(3).GetChild(3).GetChild(i).tag == "Exit")
            {
                currentRoom.transform.GetChild(3).GetChild(3).GetChild(i).transform.GetChild(0).GetComponent<Animator>().SetTrigger("DoorOpen");
                Collider[] doorTriggers = currentRoom.transform.GetChild(3).GetChild(3).GetChild(i).GetComponents<Collider>();
                doorTriggers[1].enabled = true;
                if (i == 0)
                {                  
                    currentRoom.transform.GetChild(3).GetChild(3).GetChild(i).GetChild(1).GetComponent<TMP_Text>().text = (nextRoom1).ToString();
                }
                if (i == 1)
                {
                    currentRoom.transform.GetChild(3).GetChild(3).GetChild(i).GetChild(1).GetComponent<TMP_Text>().text = (nextRoom2).ToString();
                }
            }
        }
        EventManager.current.onFinishRoom();
    }
}
