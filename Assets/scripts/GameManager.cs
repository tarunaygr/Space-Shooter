using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public bool CoopMode = false;
    private bool IsGameOver = false;
    private bool player1dead=false, player2dead=false;
    public GameObject pause_panel;
    private int AudioState = 1;
    Text AudioButtonText,BGText;

    Scene currentscene;
    GameObject FireButton,Analog;
 
    public GameObject AudioToggleButton,BGMButton,PauseButton;
    AudioSource BG_audio;
    float currentTimeScale;
    bool TutTextState;
    public GameObject TutText;
    private void Start()
    {
#if UNITY_ANDROID
        FireButton = GameObject.Find("Fire").gameObject;
        Analog = GameObject.FindGameObjectWithTag("Analog").gameObject;
#endif
        pause_panel.SetActive(false);   
        AudioButtonText = AudioToggleButton.transform.Find("Text").GetComponent<Text>();
        BGText = BGMButton.transform.Find("Text").GetComponent<Text>();
        CheckSFXstate();
        BG_audio = GameObject.Find("Audio_Manager").transform.GetChild(0).GetComponent<AudioSource>();
        BG_audio.ignoreListenerVolume = true;
        CheckBGState();
        
    }
    public void GameOver()
    {
        IsGameOver = true;

    }
    
    void RestartGame()
    {
        currentscene = SceneManager.GetActiveScene();
        if (Input.GetKeyDown(KeyCode.R)&&IsGameOver)
        {
            Asteroid.ast = null;
            SceneManager.LoadScene(currentscene.name);
        }
        else if(Input.GetKeyDown(KeyCode.Escape)&&IsGameOver)
        {
            Asteroid.ast = null;
            SceneManager.LoadScene(0);
        }
    }
    private void Update()
    {

#if UNITY_ANDROID
#else
        RestartGame();
        Pause();
#endif
    }
    public void Pause()
    {
#if UNITY_ANDROID
        FireButton.SetActive(false);
        Analog.SetActive(false);
        currentTimeScale = Time.timeScale;
        Time.timeScale = 0;
        TutTextState = TutText.activeSelf;
        TutText.SetActive(false);
        pause_panel.SetActive(true);
        PauseButton.SetActive(false);
#else
        if (Input.GetKeyDown(KeyCode.Escape) && !IsGameOver)
        {
            Time.timeScale = 0;
            pause_panel.SetActive(true);
        }
#endif
    }
    public void Resume()
    {
#if UNITY_ANDROID
        FireButton.SetActive(true);
        Analog.SetActive(true);
        PauseButton.SetActive(true);
        TutText.SetActive(TutTextState);
#endif
        Time.timeScale = currentTimeScale;
        pause_panel.SetActive(false);

        }
    public void Player2died()
    {
        player2dead = true;
    }
    public void Player1died()
    {
        player1dead = true;
    }
    public bool returnPlayer1status()
    {
        return player1dead;
    }

    public bool returnPlayer2status()
    {
        return player2dead;
    }
    public void main_menu()
    {
        Asteroid.ast = null;
        SceneManager.LoadScene(0);
    }
    public void RestartLevel()
    {
        Time.timeScale = 1;
        Asteroid.ast = null;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
   
    public void SFXToggle()
    {
  
        
        if (PlayerPrefs.GetInt("SFX")==0)
        {
            AudioListener.volume = 1;
            AudioButtonText.text = "SFX: ON";
            PlayerPrefs.SetInt("SFX", 1);

        }
           else
        {
            AudioListener.volume = 0;
            PlayerPrefs.SetInt("SFX", 0);
            AudioButtonText.text = "SFX: OFF";
        }

    }
    private void CheckSFXstate()
    {
        if(PlayerPrefs.GetInt("SFX",1)==1)
        {
            AudioButtonText.text = "SFX: ON";
            AudioListener.volume = 1;
        }
        else
        {
            AudioButtonText.text = "SFX: OFF";
            AudioListener.volume = 0;
        }
    }
    public void BGToggle()
    {

        if (PlayerPrefs.GetInt("BG_music") == 1)
        {
            BGText.text = "Music: OFF";
            PlayerPrefs.SetInt("BG_music", 0);
            BG_audio.Pause();

        }
        else
        {
            PlayerPrefs.SetInt("BG_music", 1);
            BGText.text = "Music: ON";
            BG_audio.Play();
        }

    }
    private void CheckBGState()
    {
        if (PlayerPrefs.GetInt("BG_music", 1) == 0)
        {
            BGText.text = "Music: OFF";
            BG_audio.Stop();

        }
        else
        {
            BGText.text = "Music: ON";
        }
    }
}

