using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

public class EnemyPoolManager : MonoBehaviour
{
    public static EnemyPoolManager ePInstance;
    public GameObject enemy;
    public int ePSize = 30;
    private List<GameObject> enemyPool;
    public GameObject startPlat;
    void Awake()
    {
        ePInstance = this;
        enemyPool = new List<GameObject>();
        for (int i = 0; i < ePSize; i++)
        {
            GameObject enemyPoolSpawn = Instantiate(enemy);
            enemyPoolSpawn.SetActive(false);
            enemyPool.Add(enemyPoolSpawn);
        }
        startPlat.SetActive(false);
    }

    public GameObject GetEnemy()
    {
        foreach (GameObject enemy in enemyPool)
        {
            if (!enemy.activeInHierarchy)
                return enemy;
        }
        // Optionally expand pool:
        GameObject newEnemy = Instantiate(enemy);
        newEnemy.SetActive(false);
        enemyPool.Add(newEnemy);
        return newEnemy;
    }
}
