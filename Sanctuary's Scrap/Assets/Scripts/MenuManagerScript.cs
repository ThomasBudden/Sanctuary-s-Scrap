using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuManagerScript : MonoBehaviour
{
    [SerializeField] public Scene gameScene;
    public void OnPlayButtonPress()
    {
        SceneManager.LoadScene(1);
    }
}
