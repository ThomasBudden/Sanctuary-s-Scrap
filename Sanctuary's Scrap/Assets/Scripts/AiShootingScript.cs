using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Windows;
using static UnityEngine.GraphicsBuffer;

public class AiShootingScript : MonoBehaviour
{
    private GameObject player;
    private float currentRotY;
    private float CurrentRotX;
    public float horizontalAngle;
    public float verticalAngle;
    private Quaternion targetRotation;
    private float timeCount;
    public GameObject bullet;
    public float bulletDiv;
    private Vector3 currentEulerAngles;
    private Quaternion currentRotation;
    private GameObject lastBullet;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {

        Vector3 targetDir = player.transform.position - transform.position;
        Quaternion lookDir = Quaternion.LookRotation(targetDir);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, lookDir, 10);
        RaycastHit hit;
        if (Physics.Raycast(this.transform.position, this.transform.forward, out hit, 100))
        {
            if (hit.collider.gameObject == player && timeCount + 2 < Time.time)
            {
                timeCount = Time.time;
                float randX = Random.Range(-bulletDiv, bulletDiv);
                float randY = Random.Range(-bulletDiv, bulletDiv);
                float randZ = Random.Range(-bulletDiv, bulletDiv);
                currentEulerAngles = new Vector3(randX, randY, randZ) + this.transform.rotation.eulerAngles;
                currentRotation.eulerAngles = currentEulerAngles;
                lastBullet = Instantiate(bullet, this.transform.position, this.transform.rotation);
                lastBullet.GetComponent<BulletScript>().moveSpeed = 50f;
                lastBullet.GetComponent<BulletScript>().damage = 10f;
                //lastBullet.GetComponent<BoxCollider>().excludeLayers = 7;
            }
        }
    }
}
