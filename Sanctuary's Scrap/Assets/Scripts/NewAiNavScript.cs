using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;
using TMPro.Examples;
using UnityEngine.UIElements;

public class NewAiNavScript : MonoBehaviour
{
    private GameObject player;
    private GameObject body;
    private NavMeshAgent agent;
    private Vector3 direction;
    private float distance;
    private bool lineOfSight;
    private float lastShotTime;
    private float hitTime;
    public bool damageTaken;
    public Material enemyMat;
    public Material hitMat;
    public int roomCount;

    public EnemyScriptable enemyStats;
    private float attackSpeed;
    private float stoppingDistance;
    private float retreatDistance;
    private float attackDistance;
    public float health;
    private float maxHealth;
    private float damage;
    private float movementSpeed;
    private float turnSpeed;
    private GameObject proj;
    private float projSize;
    private float projSpeed;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        agent = this.GetComponent<NavMeshAgent>();
        body = this.transform.GetChild(0).gameObject;
        attackSpeed = enemyStats.attackSpeed;
        stoppingDistance = enemyStats.stoppingDistance;
        retreatDistance = enemyStats.retreatDistance;
        attackDistance = enemyStats.attackDistance;
        maxHealth = enemyStats.health;
        health = enemyStats.health;
        damage = enemyStats.damage;
        movementSpeed = enemyStats.movementSpeed;
        turnSpeed = enemyStats.turnSpeed;
        proj = enemyStats.Projectile;
        projSize = enemyStats.projSize;
        projSpeed = enemyStats.projSpeed;
        this.transform.GetChild(1).gameObject.GetComponent<TMP_Text>().text = (health + " / " + maxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        direction = (player.transform.position - this.transform.position).normalized;
        distance = Vector3.Distance(this.transform.position, player.transform.position);
        //Debug.DrawLine(this.transform.position, this.transform.position + direction * 10, color: Color.red, Mathf.Infinity);
        RaycastHit hit;
        if (Physics.Raycast(this.transform.position, direction, out hit, Mathf.Infinity))
        {
            if (hit.collider.tag == "Player")
            {
                body.transform.forward = Vector3.RotateTowards(body.transform.forward, direction, turnSpeed * Time.deltaTime, turnSpeed * Time.deltaTime);
                lineOfSight = true;
                if (distance < attackDistance && lastShotTime + attackSpeed < Time.time && Vector3.Angle(body.transform.forward, direction) < 30)
                {
                    Attack();
                    lastShotTime = Time.time;
                }
            }
            else if (hit.collider.tag != "Player")
            {
                lineOfSight = false;
            }
        }

        if ((distance > stoppingDistance) || (lineOfSight == false))
        {
            Advance();
        }
        else if (distance < stoppingDistance - retreatDistance && lineOfSight == true)
        {
            Retreat();
        }
        else if (distance < stoppingDistance && distance > stoppingDistance - retreatDistance && lineOfSight == true)
        {
            Stop();
        }
        if (damageTaken == true)
        {
            TakeDamage();
        }
        if (hitTime + 0.1f < Time.time)
        {
                this.transform.GetChild(0).GetComponent<Renderer>().material = enemyMat;
        }
        this.transform.GetChild(1).transform.LookAt(new Vector3(this.transform.position.x - (player.transform.position.x - this.transform.position.x), this.transform.position.y + 0.5f, this.transform.position.z - (player.transform.position.z - this.transform.position.z)));
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
    public void Attack()
    {
        GameObject lastProj = Instantiate(proj, this.transform.position, body.transform.rotation);
        lastProj.GetComponent<BulletScript>().moveSpeed = projSpeed;
        lastProj.GetComponent<BulletScript>().damage = damage;
    }
    public void TakeDamage()
    {
        if (health > 0)
        {
            hitTime = Time.time;
            this.transform.GetChild(0).GetComponent<Renderer>().material = hitMat;
        }
        else if (health <= 0)
        {
            gameObject.SetActive(false);
            GameManagerScript.enemysActive -= 1;
            this.transform.position = new Vector3(0, -10, 0);
            health = maxHealth;
        }
        this.transform.GetChild(1).gameObject.GetComponent<TMP_Text>().text = (health + " / " + maxHealth);
        damageTaken = false;
    }
}
