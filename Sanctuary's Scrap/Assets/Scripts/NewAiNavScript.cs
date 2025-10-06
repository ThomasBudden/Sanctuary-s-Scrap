using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NewAiNavScript : MonoBehaviour
{
    private GameObject player;
    private GameObject body;
    private NavMeshAgent agent;
    private Vector3 direction;
    public EnemyScriptable enemyStats;
    private int range;
    private float attackSpeed;
    private int stoppingDistance;
    private int retreatDistance;
    private int health;
    private int damage;
    private int movementSpeed;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        agent = this.GetComponent<NavMeshAgent>();
        body = this.transform.GetChild(1).gameObject;
        range = enemyStats.range;
        attackSpeed = enemyStats.attackSpeed;
        stoppingDistance = enemyStats.stoppingDistance;
        retreatDistance = enemyStats.retreatDistance;
        health = enemyStats.health;
        damage = enemyStats.damage;
        movementSpeed = enemyStats.movementSpeed;
}

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(this.transform.position, player.transform.position);
        direction = player.transform.position - this.transform.position;
        if (distance > stoppingDistance)
        {
            Advance();
        }
        else if (distance < stoppingDistance - retreatDistance)
        {
            Retreat();
        }
        else if (distance < stoppingDistance && distance > stoppingDistance - retreatDistance)
        {
            Stop();
        }
    }
    public void Advance()
    {
        agent.destination = player.transform.position;
    }
    public void Retreat()
    {
        agent.destination = this.transform.position + new Vector3(2 * -direction.x, 0, 2 * -direction.z);
    }
    public void Stop()
    {
        agent.destination = this.transform.position;
    }
}
