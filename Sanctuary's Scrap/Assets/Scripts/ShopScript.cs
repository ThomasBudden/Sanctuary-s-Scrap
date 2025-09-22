using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShopScript : MonoBehaviour
{
    public GameObject player;
    public GameObject playerCam;
    private GameObject currentChest;
    public GameObject shopPanel;
    public GameObject statsManager;
    public bool shopping;
    public bool shopRolled;
    public GameObject[] shopSlot = new GameObject[3]; //child 0 is name, child 1 is stats
    public string[] names = new string[5];
    public string[] stats = new string[5];
    private bool[] cardUsed = new bool[5];
    public int[] slotCard = new int[3];
    private int numCardsUsed;
    public int currentCard;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        shopping = player.GetComponent<NewPlayerMove>().shopping;
        if (shopping == true)
        {
            if (shopRolled == false)
            {
                RollTheShop();
                shopRolled = true;
            }
            shopPanel.SetActive(true);
            player.GetComponent<NewPlayerMove>().canMove = false;
            playerCam.GetComponent<TurretCameraScript>().lockedCursor = false;
            Cursor.visible = true;
            player.GetComponent<HitScanShootingScript>().canShoot = false;
        }
    }
    public void RollTheShop()
    {
        for (int i = 0; i < cardUsed.Length; i++)
        {
            if (cardUsed[i] == true)
            {
                numCardsUsed += 1;
            }
        }
        if (cardUsed.Length - numCardsUsed >= shopSlot.Length)
        {
            for (int i = 0; i < shopSlot.Length; i++)
            {
                int randCard = Random.Range(0, stats.Length);
                if (cardUsed[randCard] == false)
                {
                    shopSlot[i].transform.GetChild(0).GetComponent<TMP_Text>().text = names[randCard];
                    shopSlot[i].transform.GetChild(1).GetComponent<TMP_Text>().text = stats[randCard];
                    cardUsed[randCard] = true;
                    slotCard[i] = randCard;
                    
                }
                else if (cardUsed[randCard] == true)
                {
                    i -= 1;
                }
            }
        }
        else if (cardUsed.Length - numCardsUsed < shopSlot.Length)
        {
            Debug.Log("No shop");
        }
    }
    public void OnClick0()
    {
        currentCard = slotCard[0];
        statsManager.GetComponent<StatsManagerScript>().applyCard = true;
        ShopExit();
    }
    public void OnClick1()
    {
        currentCard = slotCard[1];
        statsManager.GetComponent<StatsManagerScript>().applyCard = true;
        ShopExit();
    }
    public void OnClick2()
    {
        currentCard = slotCard[2];
        statsManager.GetComponent<StatsManagerScript>().applyCard = true;
        ShopExit();
    }

    public void ShopExit()
    {
        player.GetComponent<NewPlayerMove>().shopping = false;
        shopPanel.SetActive(false);
        player.GetComponent<NewPlayerMove>().canMove = true;
        playerCam.GetComponent<TurretCameraScript>().lockedCursor = true;
        Cursor.visible = false;
        player.GetComponent<HitScanShootingScript>().canShoot = true;
        shopRolled = false;
    }
}
