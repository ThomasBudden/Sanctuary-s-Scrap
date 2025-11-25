using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ShopScript : MonoBehaviour
{
    public GameObject player;
    public GameObject playerCam;
    private GameObject currentChest;
    public GameObject shopPanel;
    public GameObject statsManager;
    public bool shopRolled;
    public GameObject[] shopSlot = new GameObject[3]; //child 0 is name, child 1 is stats
    public ScrapScriptable[] scraps;
    public bool[] usedScraps;
    public List<ScrapScriptable> rarityScraps;
    public ScrapScriptable[] slotScrap = new ScrapScriptable[3];
    private int currentRarity;
    private ScrapScriptable chosenScrap;
    private int numScrapsUsed;
    public ScrapScriptable currentScrap;
    // Start is called before the first frame update
    void Start()
    {
        EventManager.current.RoomRewardInteract += StartShopping;
        EventManager.current.RoomRewardClose += ShopExit;
        EventManager.current.RoomRewardChosen += onRoomRewardChosen;
        usedScraps = new bool[scraps.Length];
    }
    public void StartShopping()
    {
        if (shopRolled == false)
        {
            RollTheShop();
            shopRolled = true;
        }
        shopPanel.SetActive(true);
    }
    public void RollTheShop()
    {
        for (int i = 0; i < usedScraps.Length; i++)
        {
            if (usedScraps[i] == true)
            {
                numScrapsUsed += 1;
            }
        }
        if (usedScraps.Length - numScrapsUsed >= shopSlot.Length)
        {
            for (int i = 0; i < shopSlot.Length; i++)
            {
                rarityScraps = new List<ScrapScriptable>();
                int rarityRand = Random.Range(1, 101);
                if (rarityRand > 94)
                {
                    currentRarity = 3;
                }
                else if (rarityRand > 84)
                {
                    currentRarity = 2;
                }
                else if (rarityRand > 59)
                {
                    currentRarity = 1;
                }
                else if (rarityRand > 0)
                {
                    currentRarity = 0;
                }
                for (int j = 0; j < scraps.Length; j++)
                {
                    if (scraps[j].scrapRarity == currentRarity)
                    {
                        rarityScraps.Add(scraps[j]);
                    }
                }
                int randScrap = Random.Range(0, rarityScraps.Count);
                for (int j = 0; j < usedScraps.Length; j++)
                {
                    if (rarityScraps[randScrap].scrapId == scraps[j].scrapId && usedScraps[j] == true)
                    {
                        i -= 1;
                    }
                    else if (rarityScraps[randScrap].scrapId == scraps[j].scrapId && usedScraps[j] == false)
                    {
                        chosenScrap = rarityScraps[randScrap];
                        shopSlot[i].transform.GetChild(0).GetComponent<TMP_Text>().text = chosenScrap.scrapName;
                        if (chosenScrap.scrapType == 0)
                        {
                            shopSlot[i].transform.GetChild(0).GetComponent<TMP_Text>().color = Color.red;
                        }
                        else if (chosenScrap.scrapType == 1)
                        {
                            shopSlot[i].transform.GetChild(0).GetComponent<TMP_Text>().color = Color.blue;
                        }
                        else if (chosenScrap.scrapType == 2)
                        {
                            shopSlot[i].transform.GetChild(0).GetComponent<TMP_Text>().color = Color.purple;
                        }
                        else if (chosenScrap.scrapType == 3)
                        {
                            shopSlot[i].transform.GetChild(0).GetComponent<TMP_Text>().color = Color.green;
                        }
                        else if (chosenScrap.scrapType == 4)
                        {
                            shopSlot[i].transform.GetChild(0).GetComponent<TMP_Text>().color = Color.yellow;
                        }
                        shopSlot[i].transform.GetChild(1).GetComponent<TMP_Text>().text = chosenScrap.description;
                        if (chosenScrap.scrapRarity == 0)
                        {
                            shopSlot[i].transform.GetChild(1).GetComponent<TMP_Text>().color = Color.black;
                        }
                        else if (chosenScrap.scrapRarity == 1)
                        {
                            shopSlot[i].transform.GetChild(1).GetComponent<TMP_Text>().color = Color.blue;
                        }
                        else if (chosenScrap.scrapRarity == 2)
                        {
                            shopSlot[i].transform.GetChild(1).GetComponent<TMP_Text>().color = Color.purple;
                        }
                        else if (chosenScrap.scrapRarity == 3)
                        {
                            shopSlot[i].transform.GetChild(1).GetComponent<TMP_Text>().color = Color.gold;
                        }
                        shopSlot[i].transform.GetChild(2).GetComponent<TMP_Text>().text = chosenScrap.statsDescPos;
                        shopSlot[i].transform.GetChild(3).GetComponent<TMP_Text>().text = chosenScrap.statsDescBad;
                        shopSlot[i].transform.GetChild(4).GetComponent<TMP_Text>().text = chosenScrap.statsDescMid;
                        usedScraps[j] = true;
                        slotScrap[i] = chosenScrap;
                    }
                }
            }
        }
        else if (usedScraps.Length - numScrapsUsed < shopSlot.Length)
        {
            Debug.Log("No shop");
        }
    }
    public void OnClick0()
    {
        currentScrap = slotScrap[0];
        statsManager.GetComponent<StatsManagerScript>().applyScrap = true;
        EventManager.current.onRoomRewardChosen();
        ShopExit();
    }
    public void OnClick1()
    {
        currentScrap = slotScrap[1];
        statsManager.GetComponent<StatsManagerScript>().applyScrap = true;
        EventManager.current.onRoomRewardChosen();
        ShopExit();
    }
    public void OnClick2()
    {
        currentScrap = slotScrap[2];
        statsManager.GetComponent<StatsManagerScript>().applyScrap = true;
        EventManager.current.onRoomRewardChosen();
        ShopExit();
    }

    public void ShopExit()
    {
        EventManager.current.onPlayerCloseMenu();
        shopPanel.SetActive(false);
        EventManager.current.onFinishRoom();
    }
    public void onRoomRewardChosen()
    {
        shopRolled = false;
        for (int i = 0;  i < usedScraps.Length; i++)
        {
            usedScraps[i] = false;
        }
    }
}
