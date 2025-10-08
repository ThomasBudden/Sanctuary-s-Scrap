using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
    public GameObject player;
    public GameObject enemy;
    public EnemyScriptable[] enemyStats;
    public GameObject[] roomArray;
    public List<GameObject> enemySpawns;
    private float startTime;
    public List<GameObject> usedSpawns;
    public int clear;
    public int wave;
    public int spawnTest;
    public int randSpawn;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        wave = 0;
        int roomRand = Random.Range(0, roomArray.Length);
        Instantiate(roomArray[roomRand], new Vector3(0,0,0), Quaternion.identity);
        player.transform.position = roomArray[roomRand].transform.GetChild(0).transform.position;
        for (int i = 0; i < roomArray[roomRand].transform.GetChild(1).childCount; i++)
        {
            enemySpawns.Add(roomArray[roomRand].transform.GetChild(1).GetChild(i).gameObject);
        }
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (wave == 0)
        {
            wave = wave + 1;
            spawnTest = Random.Range((enemySpawns.Count / 2), enemySpawns.Count);
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
                    GameObject lastEnemy = Instantiate(enemy, enemySpawns[randSpawn].transform.position, Quaternion.identity);
                    lastEnemy.GetComponent<NewAiNavScript>().enemyStats = enemyStats[0];
                    usedSpawns.Add(enemySpawns[randSpawn]);
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
    }
}
