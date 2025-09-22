using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NewAiNavScript : MonoBehaviour
{
    private GameObject player;
    public int stoppingDistance;
    private GameObject body;
    private NavMeshAgent agent;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        agent = this.GetComponent<NavMeshAgent>();
        body = this.transform.GetChild(0).gameObject;

    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(this.transform.position, player.transform.position);
        Vector3 direction = player.transform.position - this.transform.position;
        RaycastHit hit;
        if (Physics.Raycast(this.transform.position, body.transform.forward, out hit))
        {
            if (hit.collider.gameObject != player)
            {
                agent.destination = this.transform.position;
            }
            else if (hit.collider.gameObject == player && distance > stoppingDistance)
            {
                agent.destination = player.transform.position;
            }
            else if (hit.collider.gameObject == player && distance < stoppingDistance - 5)
            {
                agent.destination = this.transform.position + new Vector3(2 * -direction.x, 0, 2 * -direction.z);
            }
            else if (hit.collider.gameObject == player && distance < stoppingDistance && distance > stoppingDistance - 5)
            {
                agent.destination = this.transform.position;
            }
        }
    }
}
