using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsManagerScript : MonoBehaviour
{
    public GameObject player;
    public NewPlayerMove move;
    public HitScanShootingScript shooting;
    public GameObject shopManager;
    public bool applyScrap;
    public ScrapScriptable currentScrap;
    // Start is called before the first frame update
    void Start()
    {
        move = player.GetComponent<NewPlayerMove>();
        shooting = player.GetComponent<HitScanShootingScript>();
    }
    /*
    public int relStatsCount;
    public string scrapName;
    public int scrapId;
    public int scrapRarity;
    public int scrapType;
    public string description;
    public string statsDescPos;
    public string statsDescMid;
    public string statsDescBad;
    public float fireSpeed;
    public bool fireSpeedRel;
    public int bulletDamage;
    public bool bulletDamageRel;
    public int accuracy;
    public bool accuracyRel;
    public int magSizeAdd;
    public bool magSizeAddRel;
    public int magSizeMult;
    public bool magSizeMultRel;
    public int reloadSpeed;
    public bool reloadSpeedRel;
    public int health;
    public bool healthRel;
    public int healthRecharge;
    public bool healthRechargeRel;
    public int movementSpeed;
    public bool movementSpeedRel;
    public int damageResistance;
    public bool damageResistanceRel;
    public int ability;
    public bool abilityRel;
    public int weakSpot;
    public bool weakSpotRel;
    public int critChance;
    public bool critChanceRel;
    public float dodgeChance;
    public bool dodgeChanceRel;
    public string extraInfo;
    public bool extraInfoRel;
    public float effectTime;
    public bool onEnemy;
    public bool whileReload;
    public bool onGetHit;
    public bool onKillEnemy;
    public bool diminishing;
    public string diminishStat;
    public float diminishMax;
    */
    // Update is called once per frame
    void Update()
    {
        currentScrap = shopManager.GetComponent<ShopScript>().currentScrap;
        if (applyScrap == true)
        {
            if (currentScrap.fireSpeed > 0)
            {
                shooting.shotSpeed *= (currentScrap.fireSpeed / 100);
            }
            else if (currentScrap.fireSpeed < 0)
            {
                shooting.shotSpeed += (shooting.shotSpeed * (currentScrap.fireSpeed / 100));
            }


            if (currentScrap.bulletDamage > 0)
            {
                shooting.damage = shooting.damage * (currentScrap.bulletDamage / 100);
            }
            else if (currentScrap.bulletDamage < 0)
            {
                shooting.damage += (shooting.damage * (currentScrap.bulletDamage / 100));
            }

            
            if (currentScrap.accuracy > 0)
            {
                shooting.accuracy = shooting.accuracy * (currentScrap.accuracy / 100);
            }
            else if (currentScrap.accuracy < 0)
            {
                shooting.accuracy += (shooting.accuracy * (currentScrap.accuracy / 100));
            }


            if (currentScrap.magSizeAddRel == true)
            {
                if (shooting.maxAmmo + currentScrap.magSizeAdd >= 1)
                {
                    shooting.maxAmmo += currentScrap.magSizeAdd;
                    shooting.ammoCount = shooting.maxAmmo;
                }
                else if (shooting.maxAmmo + currentScrap.magSizeAdd <= 0)
                {
                    shooting.maxAmmo = 1;
                    shooting.ammoCount = shooting.maxAmmo;
                }
            }


            if (currentScrap.magSizeMult > 0)
            {
                shooting.maxAmmo = shooting.maxAmmo * (currentScrap.magSizeMult / 100);
            }
            else if (currentScrap.magSizeMult < 0)
            {
                shooting.maxAmmo += (shooting.maxAmmo * (currentScrap.magSizeMult / 100));
            }


            if (currentScrap.reloadSpeed > 0)
            {
                shooting.reloadTime *= (currentScrap.reloadSpeed / 100);
            }
            else if (currentScrap.reloadSpeed < 0)
            {
                shooting.reloadTime += (shooting.reloadTime * (currentScrap.reloadSpeed / 100));
            }


            if (currentScrap.health > 0)
            {
                move.maxHealth *= (currentScrap.health / 100);
                move.health *= (currentScrap.health / 100);
            }
            else if (currentScrap.health < 0)
            {
                move.maxHealth += (move.maxHealth * (currentScrap.health / 100));
                if (move.health > move.maxHealth)
                {
                    move.health = move.maxHealth;
                }
            }


            // Health Recharge


            if (currentScrap.movementSpeed > 0)
            {
                move.speed *= (currentScrap.movementSpeed / 100);
            }
            else if (currentScrap.movementSpeed < 0)
            {
                move.speed += (move.speed * (currentScrap.movementSpeed / 100));
            }


            //Damage Resistance


            //Ability


            //Weak Spot


            // Crit Chance


            //Dodge Chance


            applyScrap = false;
        }
    }
}
