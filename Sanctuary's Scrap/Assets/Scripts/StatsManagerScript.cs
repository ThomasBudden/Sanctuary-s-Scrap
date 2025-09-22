using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsManagerScript : MonoBehaviour
{
    public GameObject player;
    public GameObject shopManager;
    public bool applyCard;
    public int currentCard;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        currentCard = shopManager.GetComponent<ShopScript>().currentCard;
        if (applyCard == true)
        {
            if(currentCard == 0) //spray'n'pray (x500% fire speed, +30 mag size, -75% damage, -35% accuracy, -30% reload speed)
            {
                player.GetComponent<HitScanShootingScript>().shotSpeed = (player.GetComponent<HitScanShootingScript>().shotSpeed / 5f);
                player.GetComponent<HitScanShootingScript>().maxAmmo += 30;
                player.GetComponent<HitScanShootingScript>().ammoCount = player.GetComponent<HitScanShootingScript>().maxAmmo;
                player.GetComponent<HitScanShootingScript>().damage -= (player.GetComponent<HitScanShootingScript>().damage * 0.75f);
                player.GetComponent<HitScanShootingScript>().accuracy -= (player.GetComponent<HitScanShootingScript>().accuracy * 0.35f);
                player.GetComponent<HitScanShootingScript>().reloadTime += (player.GetComponent<HitScanShootingScript>().reloadTime * 0.3f);
            }
            else if(currentCard == 1) //large caliber (x1000% damage, x500% accuracy, x110% movement speed, -90% fire speed, -5 mag size)
            {
                player.GetComponent<HitScanShootingScript>().damage *= 10f;
                player.GetComponent<HitScanShootingScript>().accuracy *= 5f;
                player.GetComponent<NewPlayerMove>().speed *= 1.1f;
                player.GetComponent<HitScanShootingScript>().shotSpeed += (player.GetComponent<HitScanShootingScript>().shotSpeed * 0.9f);
                player.GetComponent<HitScanShootingScript>().maxAmmo -= 5;
            }
            else if(currentCard == 2) //grippy grip (x750% accuracy, x125% reload speed, -15% movement speed)
            {
                player.GetComponent<HitScanShootingScript>().accuracy *= 7.5f;
                player.GetComponent<HitScanShootingScript>().reloadTime -= (player.GetComponent<HitScanShootingScript>().reloadTime * 0.25f);
                player.GetComponent<NewPlayerMove>().speed -= (player.GetComponent<NewPlayerMove>().speed * 0.15f);
            }
            else if(currentCard == 3) //heavy armour (x500% heath, +2 heath recharge speed, -15% reload speed, -20% movement speed)
            {
                float lastHealth = player.GetComponent<HealthManager>().maxHealth;
                player.GetComponent<HealthManager>().maxHealth *= 5;
                player.GetComponent<HealthManager>().health += (player.GetComponent<HealthManager>().maxHealth - lastHealth);
                player.GetComponent<HealthManager>().regenAmount += 2;
                player.GetComponent<HitScanShootingScript>().reloadTime += (player.GetComponent<HitScanShootingScript>().reloadTime * 0.15f);
                player.GetComponent<NewPlayerMove>().speed -= (player.GetComponent<NewPlayerMove>().speed * 0.2f);
            }
            else if(currentCard == 4) //light frame (x120% reload speed, x135% movement speed, -20% accuracy, -30% health)
            {
                player.GetComponent<HitScanShootingScript>().reloadTime -= (player.GetComponent<HitScanShootingScript>().reloadTime * 0.2f);
                player.GetComponent<NewPlayerMove>().speed += (player.GetComponent<NewPlayerMove>().speed * 0.35f);
                player.GetComponent<HitScanShootingScript>().accuracy -= (player.GetComponent<HitScanShootingScript>().accuracy * 0.2f);
                player.GetComponent<HealthManager>().maxHealth -= (player.GetComponent<HealthManager>().maxHealth * 0.3f);
            }
            applyCard = false;
        }
    }
}
