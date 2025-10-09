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
    public float groundDistance = 0.4f;
    bool isGrounded;
    public LayerMask groundMask;
    public float jumpHeight;
    public bool canMove;

    [SerializeField] private bool nearChest;
    public bool shopping;
    public GameObject currentChest;

    public GameObject enemy;
    private GameObject lastEnemy;
    public EnemyScriptable enemy1;
    public TMP_Text healthCount;
    public float health;
    public float maxHealth;


    // Start is called before the first frame update
    void Start()
    {
        canMove = true;
        EventManager.current.PlayerOpenMenu += OnOpenMenu;
        EventManager.current.PlayerCloseMenu += OnCloseMenu;
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

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
        velocity.y += gravity * Time.deltaTime;
        if (canMove == true)
        {
            controller.Move(velocity * Time.deltaTime);
        }

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        if (nearChest == true && Input.GetKeyDown(KeyCode.E))
        {
            EventManager.current.onRoomRewardInteract();
            EventManager.current.onPlayerOpenMenu();
            shopping = true;
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
        if (health > 0)
        {
            healthCount.text = (health + "/ " + maxHealth);
        }
        else if (health <= 0)
        {
            Debug.Log("KILLED");
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
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == ("Chest"))
        {
            nearChest = true;
            currentChest = other.gameObject;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == ("Chest"))
        {
            nearChest = false;
            currentChest = null;
        }
    }
}
