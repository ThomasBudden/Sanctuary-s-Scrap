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
    public GameObject body;
    public GameObject head;
    private NavMeshAgent agent;
    private Vector3 direction;
    private float distance;
    private bool lineOfSight;
    private float lastShotTime;
    private float hitTime;
    public bool damageTaken;
    public Material enemyMat;
    public Material hitMat;
    public GameObject model;

    public EnemyScriptable enemyStats;
    public int enemyTypeId;
    public float attackSpeed;
    public float stoppingDistance;
    public float retreatDistance;
    public float attackDistance;
    public float health;
    public float maxHealth;
    public float damage;
    public float movementSpeed;
    public float turnSpeed;
    public GameObject proj;
    public float projSize;
    public float projSpeed;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        agent = this.GetComponent<NavMeshAgent>();
    }

    public void OnEnemyActivated()
    {
        model = Instantiate(enemyStats.enemyBody, this.transform.position, Quaternion.identity);
        model.transform.parent = this.transform;
        enemyTypeId = enemyStats.enemyTypeId;
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
        model.transform.GetChild(0).gameObject.GetComponent<TMP_Text>().text = (health + " / " + maxHealth);
        body = model.transform.GetChild(1).gameObject;
        head = model.transform.GetChild(2).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        direction = (player.transform.position - this.transform.position).normalized;
        distance = Vector3.Distance(this.transform.position, player.transform.position);
        Debug.DrawLine(this.transform.position, this.transform.position + direction * 10, color: Color.red, Mathf.Infinity);
        RaycastHit hit;
        if (Physics.Raycast(this.transform.position, direction, out hit, Mathf.Infinity))
        {
            if (hit.collider.tag == "Player")
            {
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
            body.transform.GetChild(0).GetComponent<Renderer>().material = enemyMat;
            head.transform.GetChild(0).GetComponent<Renderer>().material = enemyMat;
        }
        head.transform.forward = Vector3.RotateTowards(head.transform.forward, direction, turnSpeed * Time.deltaTime, 0.0f);
        body.transform.LookAt(new Vector3(body.transform.position.x - (body.transform.position.x - player.transform.position.x), body.transform.position.y, body.transform.position.z - (body.transform.position.z - player.transform.position.z)));
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
        GameObject lastProj = Instantiate(proj, head.transform.position, head.transform.rotation);
        lastProj.GetComponent<BulletScript>().moveSpeed = projSpeed;
        lastProj.GetComponent<BulletScript>().damage = damage;
    }
    public void TakeDamage()
    {
        if (health > 0)
        {
            hitTime = Time.time;
            body.transform.GetChild(0).GetComponent<Renderer>().material = hitMat;
            head.transform.GetChild(0).GetComponent<Renderer>().material = hitMat;
        }
        else if (health <= 0)
        {
            EventManager.current.onEnemyKilled();
            gameObject.SetActive(false);
            Destroy(this.transform.GetChild(0).transform.gameObject);
            this.transform.position = new Vector3(0, -10, 0);
            health = maxHealth;
        }
        model.transform.GetChild(0).gameObject.GetComponent<TMP_Text>().text = (health + " / " + maxHealth);
        damageTaken = false;
    }
}
