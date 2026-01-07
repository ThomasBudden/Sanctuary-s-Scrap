using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class CharSelectScript : MonoBehaviour
{
    public static int currentChar;
    public void charSelect0()
    {
        currentChar = 0;
        buttonPressed();
    }
    public void charSelect1()
    {
        currentChar = 1;
        buttonPressed();
    }
    public void charSelect2()
    {
        currentChar = 2;
        buttonPressed();
    }
    public void buttonPressed()
    {
        EventManager.current.onCharChosen();
    }
}
