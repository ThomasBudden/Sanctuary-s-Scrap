using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DebugManager : MonoBehaviour
{
    public GameObject debugPanel;
    private TMP_InputField inputField;
    public string command;
    public int commandStep;
    public string commandState;
    public string holder;
    public GameObject enemy;
    public EnemyScriptable[] enemyStatArray = new EnemyScriptable[2];
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        EventManager.current.PlayerOpenDebugMenu += OpenMenu;
        EventManager.current.PlayerCloseDebugMenu += CloseMenu;
        inputField = debugPanel.transform.GetChild(0).gameObject.GetComponent<TMP_InputField>();
    }
    private void OpenMenu()
    {
        debugPanel.SetActive(true);
        commandStep = 0;
    }
    private void CloseMenu()
    {
        debugPanel.SetActive(false);
    }
    public void InputButtonPress()
    {
        command = inputField.text;
        if (commandStep == 0 && command == "Spawn")
        {
            commandStep = 1;
            commandState = "Spawn";
            inputField.text = "";
        }
        else if (commandStep == 1 && commandState == "Spawn")
        {
            for (int i = 0; i < enemyStatArray.Length; i++)
            {
                if (enemyStatArray[i].name == command)
                {
                    commandStep = 2;
                    commandState = "Spawn";
                    holder = command;
                    inputField.text = "";
                }
            }
        }
    }
}
