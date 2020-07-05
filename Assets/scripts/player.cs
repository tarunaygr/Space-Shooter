using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour
{
    Spawn_Manager _spawn;
    public bool isplayer1 = false;
    public bool isplayer2 = false;
    [SerializeField]
    private float firerate = .25f, horizontalinput, verticalinput, speed = 8f,canfire;
    [SerializeField]
    GameObject LaserPrefab,Explosion;
    [SerializeField]
    private int _lives = 3,SpeedMultiplier=2;
    [SerializeField]
    private GameObject ShieldVisulaizer;
    [SerializeField]
    private bool Triple_shot_active = false, Speed_Active = false, Shield_Active = false;
    private int _score = 0;
    UI_Manager Ui_manager;
    [SerializeField]
    GameObject Left_Engine, Right_Engine;
    [SerializeField]
    AudioSource LaserFire;
    GameManager gamemanager;
    GameObject FiredLaser1, FiredLaser2, FiredLaser3;
    [SerializeField]
    GameObject Laser_Container;
    Animator turningAnimation;
    Spawn_Manager spawnmanager;
    [SerializeField]
    GameObject PlayerInstance;
    [SerializeField]
    FloatingJoystick js;
    public float horizontal;
    float objwidth, objheight;
    Vector3 screenBounds;
    private bool timeSlowed;
    private float Speeduptime;

    // Start is called before the first frame update
    private void Awake()
    {
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
        objwidth = GetComponent<SpriteRenderer>().bounds.size.x*transform.localScale.x/2;
        objheight = GetComponent<SpriteRenderer>().bounds.size.y*transform.localScale.y/2;
    }
    void Start()
    {
        Speeduptime = 2f;
        firerate =0.25f;
        spawnmanager = GameObject.Find("SpawnManager").GetComponent<Spawn_Manager>();
        _spawn = GameObject.Find("SpawnManager").GetComponent<Spawn_Manager>();
        ShieldVisulaizer.SetActive(false);
        Ui_manager = GameObject.Find("Canvas").GetComponent < UI_Manager > ();
        Ui_manager.SpriteUpdate(_lives,gameObject);
        Left_Engine.SetActive(false);
        Right_Engine.SetActive(false);
        turningAnimation = this.GetComponent<Animator>();
        gamemanager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
        if (!gamemanager.CoopMode)
        {
            transform.position = new Vector3(0, -1.75f, 0);
            Ui_manager.UpdateScore_Single_player(_score);
        }
        else
        {
            Ui_manager.UpdateScore_Coop(PlayerInstance,_score);
        }
        js = GameObject.Find("Floating Joystick").GetComponent<FloatingJoystick>();
        horizontal = js.Horizontal;
    }

    // Update is called once per frame
    void Update()
    {
        if (isplayer1)
        {
            movement();
            Player1animation();
        }
        else if (isplayer2)
        {
            player2movement();
            Player2animation();
            if (Time.time > canfire)
            {
                if (Input.GetKeyDown(KeyCode.Keypad1))
                {
                    FireLaser();
                }
            }
        }
        if (timeSlowed)
        {
            Time.timeScale += (1f / Speeduptime) * Time.unscaledDeltaTime;
            Time.timeScale = Mathf.Clamp(Time.timeScale, 0f, 1f);
        }
        
        
    }
    void player2movement()
    {
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, screenBounds.x + objwidth, -screenBounds.x - objwidth), Mathf.Clamp(transform.position.y, screenBounds.y + objheight, -screenBounds.y - objheight), 0);
        if (Speed_Active == false)
        {
            if (Input.GetKey(KeyCode.Keypad8))
            {
                transform.Translate(Vector3.up * speed * Time.unscaledDeltaTime);
            }
            if (Input.GetKey(KeyCode.Keypad4))
            {
                transform.Translate(Vector3.left * speed * Time.unscaledDeltaTime);

            }
            if (Input.GetKey(KeyCode.Keypad6))
            {
                transform.Translate(Vector3.right * speed * Time.unscaledDeltaTime);
            }
            if (Input.GetKey(KeyCode.Keypad5))
            {
                transform.Translate(Vector3.down * speed * Time.unscaledDeltaTime);
            }
        }
        else
        {
            if (Input.GetKey(KeyCode.Keypad8))
            {
                transform.Translate(Vector3.up * speed * Time.deltaTime*SpeedMultiplier);
            }
            if (Input.GetKey(KeyCode.Keypad4))
            {
                transform.Translate(Vector3.left * speed * Time.deltaTime*SpeedMultiplier);
            }
            if (Input.GetKey(KeyCode.Keypad6))
            {
                transform.Translate(Vector3.right * speed * Time.deltaTime*SpeedMultiplier);
            }
            if (Input.GetKey(KeyCode.Keypad5))
            {
                transform.Translate(Vector3.down * speed * Time.deltaTime*SpeedMultiplier);
            }

        }
    }
    void movement()
    {
#if UNITY_ANDROID
        transform.position = new Vector3(Mathf.Clamp(transform.position.x,screenBounds.x+objwidth, -screenBounds.x - objwidth), Mathf.Clamp(transform.position.y, screenBounds.y + objheight, -screenBounds.y ), 0);
        horizontalinput = js.Horizontal;
        verticalinput = js.Vertical;
        Vector3 direction = new Vector3(horizontalinput, verticalinput, 0);
        if (Speed_Active == false)
            transform.Translate(direction * speed * Time.unscaledDeltaTime);
        else
            transform.Translate(direction * speed * SpeedMultiplier * Time.unscaledDeltaTime);

#else
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -9.05f, 9.05f), Mathf.Clamp(transform.position.y, -3.4f, 4.19f), 0);
        horizontalinput = CrossPlatformInputManager.GetAxis("Horizontal");
        verticalinput = CrossPlatformInputManager.GetAxis("Vertical");
        Vector3 direction = new Vector3(horizontalinput, verticalinput, 0);
        if (Speed_Active == false)
            transform.Translate(direction * speed * Time.unscaledDeltaTime);
        else
            transform.Translate(direction * speed * SpeedMultiplier * Time.unscaledDeltaTime);
#endif
    }

    public void FireLaser()
    {
        if (Time.time > canfire)
        {
            canfire = Time.time + firerate;





#if UNITY_ANDROID
            // if(CrossPlatformInputManager.GetButtonDown("Fire")|| CrossPlatformInputManager.GetButtonDown("Fire"))
            //{
            if (Triple_shot_active == true)
            {
                Instantiate(LaserPrefab, transform.position + new Vector3(1.1f, -1.0f, 0), Quaternion.identity);
                Instantiate(LaserPrefab, transform.position + new Vector3(0, 0.8f, 0), Quaternion.identity);
                Instantiate(LaserPrefab, transform.position + new Vector3(-1.1f, -1.0f, 0), Quaternion.identity);
                //  FiredLaser1.transform.parent = Laser_Container.transform;
                //  FiredLaser2.transform.parent = Laser_Container.transform;
                //  FiredLaser3.transform.parent = Laser_Container.transform;
                LaserFire.Play();
            }
            else
            {
                Instantiate(LaserPrefab, transform.position + new Vector3(0, 0.8f, 0), Quaternion.identity);
                // FiredLaser1.transform.parent = Laser_Container.transform;
            }
            LaserFire.Play();
            //}
#else
            if (Input.GetKeyDown(KeyCode.Space))
                {
                if (Triple_shot_active == true)
                {
                    FiredLaser1 = Instantiate(LaserPrefab, transform.position + new Vector3(1.1f, -1.0f, 0), Quaternion.identity);
                    FiredLaser2 = Instantiate(LaserPrefab, transform.position + new Vector3(0, 0.8f, 0), Quaternion.identity);
                    FiredLaser3 = Instantiate(LaserPrefab, transform.position + new Vector3(-1.1f, -1.0f, 0), Quaternion.identity);
                    FiredLaser1.transform.parent = Laser_Container.transform;
                    FiredLaser2.transform.parent = Laser_Container.transform;
                    FiredLaser3.transform.parent = Laser_Container.transform;
                    LaserFire.Play();
                }
                else
                {
                    FiredLaser1 = Instantiate(LaserPrefab, transform.position + new Vector3(0, 0.8f, 0), Quaternion.identity);
                    FiredLaser1.transform.parent = Laser_Container.transform;
                }
                LaserFire.Play();
            }


#endif


        }
    }
    public void damage()
    {   if (Shield_Active == false)
        {
            if (_lives>0)
            {
                _lives--;
            }
            
            if (_lives==2)
            {
                Left_Engine.SetActive(true);
            }
            if (_lives==1)
            {
                Right_Engine.SetActive(true);
            }
            Ui_manager.SpriteUpdate(_lives,gameObject);
        }
        else
        {
            Shield_Active = false;
            ShieldVisulaizer.SetActive(false);
        }
        Debug.Log("You took damage");
        if (_lives==0)
        {
            _spawn.Death();
            Instantiate(Explosion, transform.position, Quaternion.identity);
            Debug.Log("You died");
            Ui_manager.CheckBestScore(_score);
            Destroy(gameObject);
        
            
           
        }
    }
    public void ActivateTripleShot()
    {
        Triple_shot_active = true;
        StartCoroutine("TripleShotPowerDown");
       
    }

    private IEnumerator TripleShotPowerDown()
    {
        yield return new WaitForSeconds(5);
        Triple_shot_active = false;
    }
    public void ActivateSpeed()
    {
        Time.timeScale = 0.6f;
        timeSlowed = false;
        StartCoroutine("SpeedPowerDown");
    }
    IEnumerator SpeedPowerDown()
    {
        yield return new WaitForSeconds(5);
        timeSlowed = true;

    }

    public void ActivateShield()
    {
        Shield_Active = true;
        ShieldVisulaizer.SetActive(true);
    }
    public void UpdateScore()
    {
        _score+=10;
        if (_score==100)
        {
            spawnmanager.EnableEnemyFire();
            StartCoroutine(Delay());
        }
        if (_score==150||_score==550||_score==1000)
        {
            spawnmanager.UpdateSpawnRate();
            StartCoroutine(Delay());
        }
        if (!gamemanager.CoopMode)
        {   
            Ui_manager.UpdateScore_Single_player(_score);
        }
        else
        {
          Ui_manager.UpdateScore_Coop(gameObject, _score);
            
        }
    }
    void Player2animation()
    {
        if (Input.GetKeyDown(KeyCode.Keypad4))
        {
            turningAnimation.SetBool("Move_Left", true);
        }
        if (Input.GetKeyUp(KeyCode.Keypad4))
        {
            turningAnimation.SetBool("Move_Left", false);
        }
        if (Input.GetKeyDown(KeyCode.Keypad6))
        {
            turningAnimation.SetBool("Move_Right", true);
        }
        if (Input.GetKeyUp(KeyCode.Keypad6))
        {
            turningAnimation.SetBool("Move_Right", false);
        }
    }
    void Player1animation()
    {
#if UNITY_ANDROID
        if(horizontalinput<0)
        {
            turningAnimation.SetBool("Move_Left", true);
            turningAnimation.SetBool("Move_Right", false);
        }
        if(horizontalinput>0)
        {
            turningAnimation.SetBool("Move_Right", true);
            turningAnimation.SetBool("Move_Left", false);
        }
        if(horizontalinput==0)
        {
            turningAnimation.SetBool("Move_Left", false);
            turningAnimation.SetBool("Move_Right", false);
        }
#else
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {   
            turningAnimation.SetBool("Move_Left", true);
        }
        if (Input.GetKeyUp(KeyCode.A)|| Input.GetKeyUp(KeyCode.LeftArrow))
        {  
            turningAnimation.SetBool("Move_Left", false);
        }
        if (Input.GetKeyDown(KeyCode.D)|| Input.GetKeyDown(KeyCode.RightArrow))
        {
            turningAnimation.SetBool("Move_Right", true);
        }
        if (Input.GetKeyUp(KeyCode.D)|| Input.GetKeyUp(KeyCode.RightArrow))
        {
            turningAnimation.SetBool("Move_Right", false);
        }
#endif
    }
    IEnumerator Delay()
    {
        yield return new WaitForSeconds(3f);
    }
}



