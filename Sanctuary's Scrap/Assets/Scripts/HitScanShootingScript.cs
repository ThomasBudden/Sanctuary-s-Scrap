using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using static UnityEngine.GraphicsBuffer;
using Unity.VisualScripting;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.UIElements;
using UnityEditor.ShaderGraph;

public class HitScanShootingScript : MonoBehaviour
{
    public GameObject aimPoint;
    public GameObject playerCam;
    public GameObject weapon;
    public TMP_Text ammoCountTxt;
    public GameObject[] crosshair = new GameObject[4];
    public float shotTime;
    public float shotSpeed;
    public float shotSpeedMult;

    public float accuracy;
    public bool reduceAccuracy;

    private float bulletDiv;
    private float bulletDivMult;
    private Quaternion currentRotation;
    private Vector3 currentEulerAngles;
    public float damage;
    public float critChance;
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
    private float overChargeReloadStart = -100;
    public float overChargeReloadDuration;

    public bool canShoot;
    public bool doLines;
    public bool doSpheres;
    public List<float> timeList = new List<float>(); //time list for line render
    public List<GameObject> lineList = new List<GameObject>();
    public List<float> bTimeList = new List<float>(); //time list for bullet particals
    public List<GameObject> bulletList = new List<GameObject>();

    public GameObject abilityPanel;
    private GameObject secondaryPanel;
    public int secondary;
    public float secondaryRecharge;
    public float secondaryRechargeStart;

    public GameObject corePanel;
    public int core;
    public float coreDuration;
    public float coreDurationStart;
    public float coreRecharge;
    public float coreRechargeStart;
    // Start is called before the first frame update
    void Start()
    {
        ammoCount = maxAmmo;
        EventManager.current.CharChosen += onCharChosen;
        EventManager.current.StartRoom += onStartRoom;
        secondaryPanel = abilityPanel.transform.GetChild(0).gameObject;
    }
    public void onCharChosen()
    {
        if (StatsManagerScript.currentSecondary == 0)
        {
            secondary = 0;
            secondaryRechargeStart = (0 - secondaryRecharge);
        }

        if (StatsManagerScript.currentCore == 0)
        {
            core = 0;
            coreRechargeStart = (0 - coreRecharge);
            coreDuration = 5;
        }
    }
    // Update is called once per frame
    void Update()
    {
        //Secondary Ability
        if (secondaryRecharge + secondaryRechargeStart > Time.time)
        {
            secondaryPanel.transform.GetChild(1).gameObject.GetComponent<TMP_Text>().text = (((secondaryRecharge) + (secondaryRechargeStart - Time.time))).ToString("f1");
        }
        else if (secondaryRecharge + secondaryRechargeStart <= Time.time)
        {
            if (secondary == 0)
            {
                secondaryRecharge = (15 - reloadTime);
            }
            secondaryPanel.transform.GetChild(1).gameObject.GetComponent<TMP_Text>().text = ("Ready");
        }

        if (coreRecharge + coreRechargeStart > Time.time)
        {
            corePanel.transform.GetChild(1).gameObject.GetComponent<TMP_Text>().text = (((coreRecharge) + (coreRechargeStart - Time.time))).ToString("f1");
        }
        else if (coreRecharge + coreRechargeStart <= Time.time)
        {
            if (core == 0)
            {
                coreRecharge = (30);
            }
            corePanel.transform.GetChild(1).gameObject.GetComponent<TMP_Text>().text = ("Ready");
        }


        if (accuracy != 0)
        {
            bulletDiv = (1 / (accuracy * 40));
        }
        else if (accuracy <= 0)
        {
            bulletDiv = 0;
        }
        if (reduceAccuracy == true)
        {
            bulletDiv = 0;
            if (overChargeReloadStart + overChargeReloadDuration > (Time.time + 1) || ((1 / (accuracy * 40)) * ((overChargeReloadStart + overChargeReloadDuration) - Time.time) * StatsManagerScript.secondaryDimnish) > (1 / (accuracy * 40)))
            {
                bulletDiv += (1 / (accuracy * 40)) * (((overChargeReloadStart + overChargeReloadDuration) - Time.time) * StatsManagerScript.secondaryDimnish);
            }
            if (core == 0 && coreDuration + coreDurationStart > Time.time)
            {
                bulletDiv += (1 / (accuracy * 40)) * 1.5f;
            }

            else if ((overChargeReloadStart + overChargeReloadDuration <= Time.time || ((1 / (accuracy * 40)) * ((overChargeReloadStart + overChargeReloadDuration) - Time.time) * StatsManagerScript.secondaryDimnish) <= (1 / (accuracy * 40))) && (core == 0 && coreDuration + coreDurationStart <= Time.time))
            {
                reduceAccuracy = false;
            }
        }
        if (Input.GetMouseButton(0) && shotTime + (1 / (shotSpeed * shotSpeedMult)) < Time.time && ammoCount > 0 && reloading == false && canShoot == true)
        {
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
                    GameObject lastEnemy = hit.collider.gameObject.transform.parent.transform.gameObject;
                    int critRand = Random.Range(1, 101);
                    if (critRand > critChance)
                    {
                        lastEnemy.GetComponent<NewAiNavScript>().health -= damage;
                    }
                    else if (critRand <= critChance)
                    {
                        lastEnemy.GetComponent<NewAiNavScript>().health -= (damage * 2);
                    }
                    lastEnemy.GetComponent<NewAiNavScript>().damageTaken = true;
                }
                else if (hit.collider.gameObject.CompareTag("WeakSpot"))
                {
                    GameObject lastEnemy = hit.collider.gameObject.transform.parent.transform.gameObject.transform.parent.transform.gameObject.transform.parent.transform.gameObject;
                    int critRand = Random.Range(1, 101);
                    if (critRand > critChance)
                    {
                        lastEnemy.GetComponent<NewAiNavScript>().health -= damage;
                        Debug.Log("Weak Spot hit");
                    }
                    else if (critRand <= critChance)
                    {
                        lastEnemy.GetComponent<NewAiNavScript>().health -= (damage * 2);
                        Debug.Log("Weak Spot hit");
                    }
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
            shotTime = Time.time;
        }

        // Reloading Code
        if (((Input.GetKeyDown(KeyCode.R) && ammoCount != maxAmmo) || Input.GetMouseButtonUp(0) && ammoCount == 0) && reloading != true)
        {
            reloadStart = Time.time;
            overChargeReload = false;
            reloading = true;
        }
        if (Input.GetMouseButtonDown(1) && secondary == 0 && ammoCount != maxAmmo && secondaryRecharge + secondaryRechargeStart <= Time.time)
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
            if (overChargeReload == true)
            {
                overChargeReloadStart = Time.time;
                reduceAccuracy = true;
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

        if (Input.GetKeyDown(KeyCode.Q) && coreRecharge + coreRechargeStart <= Time.time)
        {
            if (core == 0)
            {
                coreDurationStart = Time.time;
                shotSpeedMult = 2.5f;
                reduceAccuracy = true;
            }
        }
        if (coreDurationStart + coreDuration <= Time.time && coreRecharge + coreRechargeStart <= Time.time && shotSpeedMult == 2.5f)
        {
            shotSpeedMult = 1;
            coreRechargeStart = Time.time;
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
    public void onStartRoom()
    {
        secondaryRechargeStart = -100;
        coreRechargeStart = -100;
    }
}