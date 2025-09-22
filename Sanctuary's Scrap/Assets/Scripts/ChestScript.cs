using System.Collections;
using System.Collections.Generic;
using TMPro;
using TMPro.Examples;
using Unity.VisualScripting;
using UnityEditor.ShaderGraph;
using UnityEngine;
using UnityEngine.UIElements;

public class ChestScript : MonoBehaviour
{
    public GameObject inputTextObj;
    public TMP_Text inputText;
    public GameObject player;
    private float playerDis;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }
    // Update is called once per frame
    void Update()
    {
        playerDis = Vector3.Distance(player.transform.position, this.transform.position);
        if (playerDis > 3)
        {
            inputText.color = new Color(0, 0, 0, 5 - playerDis);
        }
        else if (playerDis <= 3)
        {
            inputText.color = new Color(1, 0.75f, 0, 5 - playerDis);
        }

        inputTextObj.transform.LookAt(new Vector3(this.transform.position.x - (player.transform.position.x - this.transform.position.x), this.transform.position.y + 0.5f, this.transform.position.z - (player.transform.position.z - this.transform.position.z)));
    }
}
