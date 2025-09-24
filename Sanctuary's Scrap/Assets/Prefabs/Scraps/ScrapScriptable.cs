using NUnit.Framework;
using UnityEngine;
using static UnityEngine.InputSystem.LowLevel.InputStateHistory;

[CreateAssetMenu(fileName = "NewScrapData", menuName = "Scrap Data", order = 1)]
public class ScrapScriptable : ScriptableObject
{
    public string scrapName;
    public int scrapId;
    public int scrapRarity;
    public int scrapType;
    public int fireSpeed;
    public int bulletDamage;
    public int accuracy;
    public int magSize;
    public int reloadSpread;
    public int speed;
    public int health;
    public int healthRecharge;
    public int movementSpeed;
    public int damageResistance;
    public int ability;
    public int weakSpot;
    public int critChance;

    void Start()
    {

    }
}
