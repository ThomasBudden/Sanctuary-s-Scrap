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
    public GameObject[] room0Array;
    public GameObject[] roomArray;
    public GameObject currentRoom;
    public List<GameObject> currentDoors;
    public List<GameObject> enemySpawns;
    private float startTime;
    public List<GameObject> usedSpawns;
    public int clear;
    public int wave;
    public int spawnTest;
    public int randSpawn;
    public static int enemysActive;
    public bool enemySpawning;
    public GameObject rewardSpawn;
    public GameObject rewardChest;
    public GameObject currentChest;
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
        EventManager.current.CharChosen += onCharChosen;
        StartUp();
        startTime = Time.time;
    }

    public void StartUp()
    {
        charPanel.SetActive(true);
        EventManager.current.onPlayerOpenMenu();
        StartRoomSpawn();
    }
    private void onCharChosen()
    {
        charPanel.SetActive(false);
        EventManager.current.onPlayerCloseMenu();
    }

    public void StartRoomSpawn()
    {
        currentRoom = Instantiate(room0Array[0], new Vector3(0, 0, 0), Quaternion.identity);
        player.transform.position = currentRoom.transform.GetChild(0).transform.position;
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
        player.transform.position = currentRoom.transform.GetChild(0).transform.position;
    }
    void Update()
    {
        if (charPanel.activeInHierarchy == true)
        {
            EventManager.current.onPlayerOpenMenu();
        }
        if (wave < 2 && enemysActive <= 0 && enemySpawning == true)
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
            nextRoomRandScrap1 = Random.Range(0, 4);
            nextRoom1 = nextRoomRandScrap1;
        }
        nextRoomRandScrap2 = Random.Range(0, 4);
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
