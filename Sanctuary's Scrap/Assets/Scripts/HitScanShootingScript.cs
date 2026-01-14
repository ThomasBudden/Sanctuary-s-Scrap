using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using static UnityEngine.GraphicsBuffer;
using Unity.VisualScripting;
using System.Linq;

public class HitScanShootingScript : MonoBehaviour
{
    public GameObject aimPoint;
    public GameObject playerCam;
    public GameObject weapon;
    public TMP_Text ammoCountTxt;
    public GameObject[] crosshair = new GameObject[4];
    public float shotTime;
    public float shotSpeed;
    public float accuracy;
    private float bulletDiv;
    private Quaternion currentRotation;
    private Vector3 currentEulerAngles;
    public float damage;
    public Transform muzzle;
    public GameObject lineTracer;
    private GameObject lastLine;
    private LineRenderer lineRenderer;
    public GameObject bullet;
    public float recoilMult;
    public int maxAmmo;
    public int ammoCount;
    private bool reloading = false;
    public float reloadTime;
    private float reloadStart;
    private bool overChargeReload;
    public bool canShoot;
    public bool doLines;
    public bool doSpheres;
    public List<float> timeList = new List<float>(); //time list for line render
    public List<GameObject> lineList = new List<GameObject>();
    public List<float> bTimeList = new List<float>(); //time list for bullet particals
    public List<GameObject> bulletList = new List<GameObject>();

    public int secondary;

    public float secondaryRecharge;
    public float secondaryRechargeStart;
    // Start is called before the first frame update
    void Start()
    {
        ammoCount = maxAmmo;
        EventManager.current.CharChosen += onCharChosen;
    }
    public void onCharChosen()
    {
        if (StatsManagerScript.currentSecondary == 0)
        {
            secondary = 0;
            secondaryRecharge = (30 - reloadTime);
            secondaryRechargeStart = (0 - secondaryRecharge);
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (accuracy != 0)
        {
            bulletDiv = (1/(accuracy * 40));
        }
        else if (accuracy <= 0)
        {
            bulletDiv = 0;
        }
        if (Input.GetMouseButton(0) && shotTime + (1 / shotSpeed) < Time.time && ammoCount > 0 && reloading == false && canShoot == true)
        {
            /*float bulletRandVert = Random.Range(-bulletDiv, bulletDiv);
            float bulletRandHori = Random.Range(-bulletDiv, bulletDiv);*/
            float bulletRand = Random.Range(-bulletDiv, bulletDiv);
            float bulletRandAngle = Random.Range(0, 90);
            float bulletRandY = (Mathf.Sin(bulletRandAngle) * bulletRand);
            float bulletRandX = (Mathf.Cos(bulletRandAngle) * bulletRand);
            currentEulerAngles = aimPoint.transform.forward + new Vector3(bulletRandX * aimPoint.transform.forward.z,bulletRandY, bulletRandX * aimPoint.transform.forward.x);
            GameObject lastBullet = Instantiate(bullet, muzzle.transform.position, muzzle.transform.rotation);
            bulletList.Add(lastBullet);
            bTimeList.Add(Time.time);
            RaycastHit hit;
            if (Physics.Raycast(aimPoint.transform.position, currentEulerAngles, out hit, 100))
            {
                Vector3 hitPoint = hit.point;
                if (hit.collider.gameObject.CompareTag("Enemy"))
                {
                    GameObject lastEnemy = hit.collider.gameObject;
                    lastEnemy.GetComponent<NewAiNavScript>().health -= damage;
                    lastEnemy.GetComponent<NewAiNavScript>().damageTaken = true;
                }
                else if (hit.collider.gameObject.CompareTag("Target"))
                {   
                    string damageNumber = damage.ToString();
                    hit.collider.gameObject.GetComponent<TargetCubeScript>().damageTxt.text = "Damage is " + damageNumber;
                    hit.collider.gameObject.GetComponent<TargetCubeScript>().dpsList.Add(damage);
                    hit.collider.gameObject.GetComponent<TargetCubeScript>().dpsTime.Add(Time.time);
                }
                //float particalAngleX = Vector2.Angle(new Vector2(muzzle.forward.x,
                //Debug.Log(particalAngleX);
                if (doSpheres == true)
                {
                    GameObject lastSphere = Instantiate(GameObject.CreatePrimitive(PrimitiveType.Sphere), hitPoint, Quaternion.identity);
                    lastSphere.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                    lastSphere.GetComponent<Renderer>().material.SetColor("_Color", Color.red);
                }
                if (doLines == true)
                {
                    lastLine = Instantiate(lineTracer, muzzle.transform.position, Quaternion.identity);
                    lineRenderer = lastLine.GetComponent<LineRenderer>();
                    lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
                    lineRenderer.widthMultiplier = 0.02f;
                    lineRenderer.startColor = Color.red;
                    lineRenderer.endColor = Color.yellow;
                    lineRenderer.SetPosition(0, muzzle.transform.position);
                    lineRenderer.SetPosition(1, hitPoint);
                    timeList.Add(Time.time);
                    lineList.Add(lastLine);
                }
            }
            else if (doLines == true)
            {
                lastBullet.transform.forward = aimPoint.transform.forward + new Vector3(bulletRandX * aimPoint.transform.forward.z, bulletRandY, bulletRandX * aimPoint.transform.forward.x);
                lastLine = Instantiate(lineTracer, muzzle.transform.position, Quaternion.identity);
                lineRenderer = lastLine.GetComponent<LineRenderer>();
                lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
                lineRenderer.widthMultiplier = 0.02f;
                lineRenderer.startColor = Color.red;
                lineRenderer.endColor = Color.yellow;
                lineRenderer.SetPosition(0, muzzle.transform.position);
                lineRenderer.SetPosition(1, (aimPoint.transform.forward * 100));
                timeList.Add(Time.time);
                lineList.Add(lastLine);
            }
            ammoCount -= 1;
            /*float recoilRandHori = Random.Range(-0.5f, 0.5f);
            playerCam.GetComponent<TurretCameraScript>().cameraVerticalRotation -= 1 * recoilMult; 
            this.transform.localEulerAngles = this.transform.localEulerAngles + new Vector3(0, (recoilRandHori * recoilMult), 0);*/
            shotTime = Time.time;
        }
        if (((Input.GetKeyDown(KeyCode.R) && ammoCount != maxAmmo) || Input.GetMouseButtonUp(0) && ammoCount == 0) && reloading != true)
        {
            reloadStart = Time.time;
            overChargeReload = false;
            reloading = true;
        }
        if (Input.GetMouseButtonDown(1) && secondary == 0 && reloading != true && ammoCount != maxAmmo && secondaryRecharge + secondaryRechargeStart <= Time.time)
        {
            overChargeReload = true;
            reloading = true;
            secondaryRechargeStart = Time.time;
        }
        else if ((reloadStart + (1 / reloadTime) < Time.time && reloading == true ) || (overChargeReload == true && reloading == true && reloadStart + (0.1 / reloadTime) < Time.time))
        {
            if (maxAmmo <= 1)
            {
                maxAmmo = 1;
            }
            ammoCount = maxAmmo;
            overChargeReload = false;
            reloading = false;
        }
        if (reloading == false)
        {
            ammoCountTxt.text = ammoCount.ToString();
        }
        else if (reloading == true)
        {
            ammoCountTxt.text = ((((1/ reloadTime) + (reloadStart - Time.time))).ToString("f1"));
        }
        for (int i = 0; i < timeList.Count; i++)
        {
            if (timeList[i] + 0.1 < Time.time)
            {
                Destroy(lineList[i].gameObject);
                lineList.Remove(lineList[i]);
                timeList.Remove(timeList[i]);
            }
        }
        for (int i = 0; i < bTimeList.Count; i++)
        {
            if (bTimeList[i] + 1 < Time.time)
            {
                Destroy(bulletList[i].gameObject);
                bulletList.Remove(bulletList[i]);
                bTimeList.Remove(bTimeList[i]);
            }
        }
        for (int i = 0; i < crosshair.Length; i++)
        {
            if (i == 0)
            {
                    crosshair[i].gameObject.transform.localPosition = new Vector3(-28 * (bulletDiv/0.025f) , 0, 0);
            }
            if (i == 1)
            {
                    crosshair[i].gameObject.transform.localPosition = new Vector3(28 * (bulletDiv / 0.025f), 0, 0);
            }
            if (i == 2)
            {
                    crosshair[i].gameObject.transform.localPosition = new Vector3(0, 28 * (bulletDiv / 0.025f), 0);
            }
            if (i == 3)
            {
                    crosshair[i].gameObject.transform.localPosition = new Vector3(0, -28 * (bulletDiv / 0.025f), 0);
            }
        }
    }
}