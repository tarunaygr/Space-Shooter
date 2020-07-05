using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Main_Menu : MonoBehaviour
{
    [SerializeField]
    GameObject CoopButton;
    [SerializeField]
    Text SinglePlayerText;
     void Awake()
    {
#if UNITY_ANDROID
        CoopButton.SetActive(false);
        SinglePlayerText.text = "New Game";
#endif

    }
    public void StartGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(1);
    }
   public void QuitGame()
    {
        Application.Quit();
    }
    public void CoopMode()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(2);
    }
}
