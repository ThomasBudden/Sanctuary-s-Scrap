using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TargetCubeScript : MonoBehaviour
{
    public TMP_Text damageTxt; //Text for damage
    public TMP_Text dpsTxt;
    public List<float> dpsList = new List<float>();
    public List<float> dpsTime = new List<float>();
    private Vector3 startPos;
    public float speed;
    private float dps;
    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = Vector3.Lerp(this.transform.position, startPos, speed * Time.deltaTime);
        dps = 0;
        for (int i = 0; i < dpsTime.Count; i++)
        {
            dps += dpsList[i];
            if (dpsTime[i] + 1 < Time.time)
            {
                dpsTime.Remove(dpsTime[i]);
                dpsList.Remove(dpsList[i]);
            }
        }
        dpsTxt.text = ("DPS is " + dps.ToString());
    }
}
