using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    public float health;
    public float maxHealth;
    private float healthTime;
    public float healthSpeed;
    public float regenAmount;
    public float lastHitTime;
    public float regenDelay;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(health == 0)
        {
            death();
        }
        if (healthTime + healthSpeed < Time.time && (health + regenAmount) < maxHealth && lastHitTime + regenDelay < Time.time)
        {
            health += regenAmount;
            healthTime = Time.time;
        }
        else if(healthTime + healthSpeed < Time.time && (health + regenAmount) >= maxHealth && health != maxHealth)
        {
            health = maxHealth;
        }
    }

    void death()
    {
        if (this.gameObject.tag == "Enemy")
        {
            Destroy(this.gameObject);
        }
    }
}
