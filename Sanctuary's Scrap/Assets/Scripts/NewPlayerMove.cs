using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NewPlayerMove : MonoBehaviour
{
    public CharacterController controller; //This allows me to control the character controller component
    public float speed;
    public float gravity = -9.81f;
    Vector3 velocity;

    public Transform groundCheck;
    public float groundDistance;
    public bool isGrounded;
    public bool isGrounded2;
    public LayerMask groundMask;
    public float jumpHeight;
    public bool canMove;
    public float jumpTime;

    [SerializeField] private bool nearChest;
    public bool shopping;
    public GameObject currentChest;

    public GameObject enemy;
    private GameObject lastEnemy;
    public EnemyScriptable enemy1;
    public TMP_Text healthCount;
    public float health;
    public float maxHealth;
    public float regenAmount;

    public bool dead;

    [SerializeField] private bool nearExit;
    public bool roomFinished;

    public int mobility;
    public float mobilityRecharge;
    public float mobilityRechargeStart;
    public float mobilityActiveTime;
    public float mobilityActiveTimeStart;
    public GameObject abilityPanel;
    private GameObject mobilityPanel;

    public static int doorChosen;


    // Start is called before the first frame update
    void Start()
    {
        canMove = true;
        EventManager.current.PlayerOpenMenu += OnOpenMenu;
        EventManager.current.PlayerCloseMenu += OnCloseMenu;
        EventManager.current.FinishRoom += OnFinishRoom;
        EventManager.current.StartRoom += OnStartRoom;
        EventManager.current.RoomRewardChosen += OnRoomRewardChosen;

        mobilityPanel = abilityPanel.transform.GetChild(1).gameObject;
    }
    public void onCharChosen()
    {
        if (StatsManagerScript.currentMobility == 0)
        {
            mobility = 0;
            mobilityRechargeStart = -100;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Secondary Ability
        if (mobilityRecharge + mobilityRechargeStart > Time.time)
        {
            mobilityPanel.transform.GetChild(1).gameObject.GetComponent<TMP_Text>().text = (((mobilityRecharge) + (mobilityRechargeStart - Time.time))).ToString("f1");
        }
        else if (mobilityRecharge + mobilityRechargeStart <= Time.time)
        {
            if (mobility == 0)
            {
                mobilityRecharge = (10 - ((speed - 10) / 2));
            }
            mobilityPanel.transform.GetChild(1).gameObject.GetComponent<TMP_Text>().text = ("Ready");
        }

        // Jumping
        isGrounded = Physics.CheckBox(groundCheck.position, new Vector3(0.3535533906f, 0.1f, 0.3535533906f), Quaternion.identity, groundMask);
        isGrounded2 = Physics.CheckBox(groundCheck.position, new Vector3(0.3535533906f, 0.75f, 0.3535533906f), Quaternion.identity, groundMask);
        if (jumpTime + 0.1f > Time.time)
        {
            isGrounded = false;
        }

        // Walking
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D))
        {
            x = 0;
        }
        if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.S))
        {
            z = 0;
        }
        Vector3 move = transform.right * x + transform.forward * z;
        move = Vector3.ClampMagnitude(move, 1f);
        if (canMove == true)
        {
            controller.Move(move * speed * Time.deltaTime);
        }
        if (isGrounded == true)
        {
            velocity.y = 0;
        }
        else if (isGrounded == false)
        {
            velocity.y += gravity * Time.deltaTime;
        }
        if (Input.GetButtonDown("Jump") && isGrounded2)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            jumpTime = Time.time;
        }
        if (canMove == true)
        {
            controller.Move(velocity * Time.deltaTime);
        }

        // Shopping
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (nearChest == true)
            {
                EventManager.current.onRoomRewardInteract();
                EventManager.current.onPlayerOpenMenu();
                shopping = true;
            }
            if (nearExit == true && roomFinished == true)
            {
                EventManager.current.onStartRoom();
            }
        }
        else if (nearChest == false && shopping == true)
        {
            EventManager.current.onRoomRewardClose();
            EventManager.current.onPlayerCloseMenu();
            shopping = false;
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            EventManager.current.onPlayerOpenMenu();
            EventManager.current.onPlayerOpenDebugMenu();
        }

        // Health
        if (health > 0)
        {
            healthCount.text = (health + "/" + maxHealth);
        }
        else if (health <= 0 && dead != true)
        {
            Debug.Log("KILLED");
            dead = true;
        }

        // Mobility
        if (mobility == 0 && Input.GetKeyDown(KeyCode.LeftShift) && mobilityRechargeStart + mobilityRecharge <= Time.time && canMove == true)
        {
            float mX = Input.GetAxis("Horizontal");
            float mZ = Input.GetAxis("Vertical");
            Vector3 mMove = transform.right * x + transform.forward * z;
            mMove = Vector3.ClampMagnitude(mMove, 1f);

            mobilityActiveTime = (0.5f);
            mobilityActiveTimeStart = Time.time;
            mobilityRechargeStart = Time.time;
        }
        if (mobilityActiveTimeStart + mobilityActiveTime > Time.time)
        {
            controller.Move(move * (100 * ((mobilityActiveTime + mobilityActiveTimeStart - Time.time))) * Time.deltaTime);
        }
    }
    private void OnOpenMenu()
    {
        canMove = false;
        GameObject playerCam = this.gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject;
        playerCam.GetComponent<TurretCameraScript>().lockedCursor = false;
        Cursor.visible = true;
        this.GetComponent<HitScanShootingScript>().canShoot = false;
    }
    private void OnCloseMenu()
    {
        canMove = true;
        GameObject playerCam = this.gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject;
        playerCam.GetComponent<TurretCameraScript>().lockedCursor = true;
        Cursor.visible = true;
        this.GetComponent<HitScanShootingScript>().canShoot = true;
    }
    private void OnFinishRoom()
    {
        roomFinished = true;
    }
    private void OnStartRoom()
    {
        if (health + regenAmount <= maxHealth)
        {
            health += regenAmount;
        }
        else if (health + regenAmount > maxHealth)
        {
            health = maxHealth;
        }
        mobilityRechargeStart = -100;
        roomFinished = false;
        nearExit = false;
    }
    private void OnRoomRewardChosen()
    {
        nearChest = false;
        shopping = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == ("Chest"))
        {
            nearChest = true;
            currentChest = other.gameObject;
        }
        else if (other.gameObject.tag == ("Exit"))
        {
            nearExit = true;
            doorChosen = other.gameObject.GetComponent<ExitDoorScript>().doorID;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == ("Chest"))
        {
            nearChest = false;
            currentChest = null;
        }
        else if (other.gameObject.tag == ("Exit"))
        {
            nearExit = false;
        }
    }
}
