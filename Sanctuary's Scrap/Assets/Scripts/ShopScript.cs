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
    public bool shopRolled;
    public GameObject[] shopSlot = new GameObject[3]; //child 0 is name, child 1 is stats
    public ScrapScriptable[] scraps;
    public bool[] usedScraps;
    public List<ScrapScriptable> rarityScraps;
    public ScrapScriptable[] slotScrap = new ScrapScriptable[3];
    public ScrapScriptable[] charScraps;
    private int currentRarity;
    private int randScrap;
    private ScrapScriptable chosenScrap;
    private int numScrapsUsed;
    private int numScrapType;
    public bool[] scrapsInType;
    public ScrapScriptable currentScrap;
    public bool roomIsShop;

    public int _debugFlag;

    void Start()
    {
        EventManager.current.RoomRewardInteract += StartShopping;
        EventManager.current.RoomRewardClose += ShopExit;
        EventManager.current.RoomRewardChosen += onRoomRewardChosen;
        EventManager.current.CharChosen += onCharChosen;
        usedScraps = new bool[scraps.Length];
        scrapsInType = new bool[scraps.Length];
    }
    public void StartShopping()
    {
        if (shopRolled == false)
        {
            numScrapType = 0;
            RollTheShop();
            shopRolled = true;
        }
        shopPanel.SetActive(true);
    }
    public void RollTheShop()
    {
        Debug.Log(GameManagerScript.thisRoomRandScrap);
        for (int i = 0; i < usedScraps.Length; i++) // checking the number of Scraps that have been used
        {
            if (usedScraps[i] == true) // for the number of different scraps, if it is checked as true in the usedScraps array, the number of Scraps that have been used is increased by 1
            {
                numScrapsUsed += 1;
            }
        }
        for (int i = 0; i < scraps.Length; i++) // checking and making an array of the scraps that are in the rooms scrap type
        {
            Debug.Log("Cheking Type");
            if (scraps[i].scrapType == GameManagerScript.thisRoomRandScrap) // if i is the same type as the room, the corosponding boolen in the scrapsOfType array is checked as true and the number of scraps in the type is increased by 1
            {
                numScrapType += 1;
                scrapsInType[i] = true;
                roomIsShop = false;
            }
            else if (GameManagerScript.thisRoomRandScrap == -1) // if the room type if -1, all scraps are set as scrap in type
            {
                scrapsInType[i] = true;
                numScrapType += 1;
            }
            else if (GameManagerScript.thisRoomRandScrap == 5) // if the room type is 5, the room is a shop
            {
                roomIsShop = true;
            }
            else if (scraps[i].scrapType != GameManagerScript.thisRoomRandScrap) // if i is not the same type as the room, the corosponding boolen in the scrapsOfType array is checked as false and the number of scraps in the type is reduced by 1
            {
                scrapsInType[i] = false;
                numScrapType -= 1;
            }
        }
        if (usedScraps.Length - numScrapsUsed >= shopSlot.Length) // if the number of avalible scraps is greater or equal to the number of slots in the shop
        {
            for (int i = 0; i < shopSlot.Length; i++) // for every slots in the shop
            {
                Debug.Log("doing rarity");
                rarityScraps = new List<ScrapScriptable>(); // create a new list to hold scraps that qualify
                int rarityRand = Random.Range(1, 101); // pick a random number
                if (rarityRand > 94) // check what rarity that makes it
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
                for (int j = 0; j < scraps.Length; j++) // for the number of scraps
                {
                    Debug.Log("checking rarity and type");
                    if (scraps[j].scrapRarity == currentRarity && scrapsInType[j] == true) // check if the scrap qualifies by being the correct rarity and type. Then add it to the rarityScrap list
                    {
                        rarityScraps.Add(scraps[j]);
                        Debug.Log(rarityScraps.Count);
                    }
                }


                //Up to this point we have a list with only the possible scraps that can be used
                if (rarityScraps.Count <= 0)
                {
                    i -= 1;
                }
                else if (rarityScraps.Count >= 1)
                {
                    randScrap = Random.Range(0, rarityScraps.Count - 1); // pick a random scrap from the scraps of the correct rarity and type
                    for (int j = 0; j < usedScraps.Length; j++) // for the number of scraps
                    {
                        Debug.Log("Checking if used" + "J=" + j + "randScrap =" + randScrap);
                        if (rarityScraps[randScrap].scrapId == scraps[j].scrapId && usedScraps[j] == true) // check whether it was already been selected in this roll of the shop. It if has, go back to the rarity selection
                        {
                            Debug.Log("Used" + i);
                            i -= 1;
                        }
                        /*else if (rarityScraps.Count < shopSlot.Length)
                        {
                            Debug.Log("Rarity scraps too small");
                            if (currentRarity != 0)
                            {
                                currentRarity -= 1;
                                i -= 1;
                                Debug.Log("Current Rarity " + currentRarity);
                            }
                        }*/
                        else if (rarityScraps[randScrap].scrapId == scraps[j].scrapId && usedScraps[j] == false) // check whether it has not been selected in this roll
                        {
                            Debug.Log("Giving slots");
                            chosenScrap = rarityScraps[randScrap];
                            Debug.Log(chosenScrap);
                            Debug.Log(rarityScraps.Count);
                            Debug.Log(i);
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
                _debugFlag += 1;
              
                if (_debugFlag > 1000)
                {
                    Debug.LogError(_debugFlag + "is full... now breaking");
                    return;
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
        for (int i = 0; i < usedScraps.Length; i++)
        {
            usedScraps[i] = false;
        }
    }
    public void onCharChosen()
    {
        for (int i = 0; i < charScraps.Length; i++)
        {
            if (i == (CharSelectScript.currentChar))
            {
                statsManager.GetComponent<StatsManagerScript>().scrapIsCharSelect = true;
                currentScrap = charScraps[i];
                statsManager.GetComponent<StatsManagerScript>().applyScrap = true;
            }
        }
    }
}
