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
        player.transform.position = currentRoom.transform.GetChild(0).transform.position;
        for (int i = 0; i < currentRoom.transform.GetChild(1).childCount; i++)
        {
            enemySpawns.Add(roomArray[roomRand].transform.GetChild(1).GetChild(i).gameObject);
        }
        rewardSpawn = currentRoom.transform.GetChild(2).transform.gameObject;
        enemySpawning = true;
        roomClear = false;
        roomCount += 1;
        roomCountUI.text = roomCount.ToString();
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
        Destroy(currentChest.gameObject);
        currentDoors = new List<GameObject>();
        for (int i = 0; i < currentRoom.transform.GetChild(3).GetChild(3).childCount; i++)
        {
            if (currentRoom.transform.GetChild(3).GetChild(3).GetChild(i).tag == "Exit")
            {
                currentRoom.transform.GetChild(3).GetChild(3).GetChild(i).transform.GetChild(0).GetComponent<Animator>().SetTrigger("DoorOpen");
            }
        }
        EventManager.current.onFinishRoom();
    }
}
