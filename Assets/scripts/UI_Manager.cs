using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class UI_Manager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI Score_Text,Score_text1,Score_text2,Best_text;
    [SerializeField]
    private Sprite[] LifeSprites;
    [SerializeField]
    Image Current_sprite_1, Current_sprite_2, Current_sprite;
    public GameObject GameOverText,RestartText;
    [SerializeField]
    GameManager gamemanager;
    private int best = 0;
    [SerializeField]
    Image FireButton,PauseButton;
    [SerializeField]
    GameObject GameOverPanel;
    public GameObject Analog, Fire,Pause;
    GameObject TutorialText;

    // Start is called before the first frame update
    void Start()
    {
        TutorialText = gameObject.transform.Find("Tutorial_Text").gameObject;
        Asteroid.ast += RemoveTutText;
        GameOverText.SetActive(false);
        RestartText.SetActive(false);
        best=PlayerPrefs.GetInt("Best_Score", 0);
        if (!gamemanager.CoopMode)
        {
            Best_text.text = "Best: " + best;
        }
#if UNITY_ANDROID
        var tempColor = FireButton.color;
        tempColor.a = .5f;
        FireButton.color = tempColor;
        tempColor = PauseButton.color;
        tempColor.a = .5f;
        PauseButton.color = tempColor;
        GameOverPanel.SetActive(false);
        Analog = GameObject.FindGameObjectWithTag("Analog").gameObject;
        Fire = GameObject.Find("Fire").gameObject;
#endif
    }
    private void Awake()
    {
        gamemanager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
    }
    // Update is called once per frame
    void Update()
    {

    }
    public void SpriteUpdate(int Lives,GameObject player)
    {
#if UNITY_ANDROID
        
            Current_sprite.sprite = LifeSprites[Lives];
            if (Lives == 0)
            {
                GameOverSequenceMobile();
            }
#else
        if (!gamemanager.CoopMode)
        {


            Current_sprite.sprite = LifeSprites[Lives];
            if (Lives == 0)
            {
                GameOverSequence();
            }
        }
        else
        {
            if (player.tag=="Player1")
            {
                Current_sprite_1.sprite = LifeSprites[Lives];
                if (Lives==0)
                {
                    gamemanager.Player1died();
                }

            }
            else if(player.tag == "Player2")
            {
                Current_sprite_2.sprite = LifeSprites[Lives];
                if (Lives==0)
                {
                    gamemanager.Player2died();
                }
            }
            if (gamemanager.returnPlayer1status()&&gamemanager.returnPlayer2status())
            {
                GameOverSequence();
            }
        }
#endif
    }
    void GameOverSequenceMobile()
    {
        GameOverText.SetActive(true);
        StartCoroutine(FlickerText());
        gamemanager.GameOver();
        GameOverPanel.SetActive(true);
        Analog.SetActive(false);
        Fire.SetActive(false);
        Pause.SetActive(false);
    }
    void GameOverSequence()
    {
        GameOverText.SetActive(true);
        RestartText.SetActive(true);
        StartCoroutine(FlickerText());
        gamemanager.GameOver();

    }
    public void UpdateScore_Single_player(int Score)
    {
        Score_Text.text = "Score: " + Score;
    }
    public void UpdateScore_Coop(GameObject player,int Score)
    {
        if (player.tag=="Player1")
        {
            Score_text1.text = "Score: " + Score;
        }
        else if (player.tag=="Player2")
        {
            Score_text2.text = "Score: " + Score;
        }
    }
    IEnumerator FlickerText()
    {   while (true)
        {
            yield return new WaitForSeconds(1);
            GameOverText.GetComponent<TextMeshProUGUI>().enabled = !(GameOverText.GetComponent<TextMeshProUGUI>().enabled);
        }
    }
  
    public void CheckBestScore(int score)
    {
        if (best<score)
        {
            best = score;
            Best_text.text = "Best: " + best;
            PlayerPrefs.SetInt("Best_Score", best);
        }
    }
    public void RemoveTutText()
    {
        TutorialText.SetActive(false);
    }
}


