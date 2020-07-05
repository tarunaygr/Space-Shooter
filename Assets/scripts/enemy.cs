using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy : MonoBehaviour
{   [SerializeField]
    private float speed = 4f;
    [SerializeField]
    GameObject Laser;
    [SerializeField]
    player Player_script, Player_2_script, Player_1_script;
    Animator Explosion;
    private bool IsShot;
    AudioSource ExplosionAudio;
    laser Laser_script;
    [SerializeField]
    private float rof;
    private float canfire = -1;
    GameManager gamemanager;
    Vector3 screenBounds;
    float objwidth, objheight;
   public static int destroyedCount = 0;
    private Spawn_Manager sm;
    private void Awake()
    {
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
        objwidth = GetComponent<SpriteRenderer>().bounds.size.x * transform.localScale.x / 2;
        objheight = GetComponent<SpriteRenderer>().bounds.size.y * transform.localScale.y / 2;
    }
    // Start is called before the first frame update
    void Start()
    {
        sm = GameObject.Find("SpawnManager").GetComponent<Spawn_Manager>();
        destroyedCount = 0;
        rof = Random.Range(1f, 1.5f);
        canfire = rof + Time.time;
        gamemanager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
        transform.position = new Vector3(Random.Range(-6.3f, 6.3f), 8f, 0);
        if (!gamemanager.CoopMode)
        {
            Player_script = GameObject.Find("player").GetComponent<player>();
        }
        else
        {
            if (!gamemanager.returnPlayer1status())
            {
                Player_1_script = GameObject.Find("Player 1").GetComponent<player>();
            }
            if (!gamemanager.returnPlayer2status())
            {
                Player_2_script = GameObject.Find("Player 2").GetComponent<player>();
            }    
                
            
        }
        Explosion = this.GetComponent<Animator>();
        IsShot = false;
        ExplosionAudio = GetComponent<AudioSource>();
      
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * Time.deltaTime * speed);
        if(!IsShot&&transform.position.y<screenBounds.y-objheight-.25)
        {
            transform.position = new Vector3(Random.Range(screenBounds.x+objwidth,-screenBounds.x-objwidth), -screenBounds.y+2, 0);
        }
        if (Time.time>canfire&&!IsShot&&sm.EnemyFire)
        {
            firelaser();
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {   if (!gamemanager.CoopMode)
        {
            if (other.gameObject.tag == "laser"&&!other.gameObject.GetComponent<laser>().isenemylaser)
            {
             
                Destroy(other.gameObject);
                if (Player_script != null)
                {
                    Player_script.UpdateScore();
                    Explosion.SetTrigger("EnemyDestroyed");
                    gameObject.GetComponent<BoxCollider2D>().enabled = false;
                    IsShot = true;
                    ExplosionAudio.Play();
                    Destroy(gameObject, 2.5f);
                }
              

            }
            else if (other.gameObject.tag == "Player")
            {
                player player_object = other.gameObject.GetComponent<player>();
                if (player_object != null)
                {
                    Debug.Log("Player damage");
                    player_object.damage();
                    Explosion.SetTrigger("EnemyDestroyed");
                    gameObject.GetComponent<BoxCollider2D>().enabled = false;
                    IsShot = true;
                    ExplosionAudio.Play();
                    Destroy(gameObject, 2.5f);
                }


            }
        }
        else
        {
            if (other.tag=="laser")
            {
               
                if (other.transform.parent.tag =="Player_1_lasers")
                {
                    Player_1_script.UpdateScore();
                    Explosion.SetTrigger("EnemyDestroyed");
                    gameObject.GetComponent<BoxCollider2D>().enabled = false;
                    IsShot = true;
                    ExplosionAudio.Play();
                    Destroy(gameObject, 2.5f);
                    Destroy(other.gameObject);
                }
                else if (other.transform.parent.tag== "Player_2_lasers")
                {
                    Player_2_script.UpdateScore();
                    Explosion.SetTrigger("EnemyDestroyed");
                    gameObject.GetComponent<BoxCollider2D>().enabled = false;
                    IsShot = true;
                    ExplosionAudio.Play();
                    Destroy(gameObject, 2.5f);
                    Destroy(other.gameObject);
                }
              

            }
            else if (other.tag=="Player1")
            {
                Debug.Log("Player 1 Damaged");
                Player_1_script.damage();
                Explosion.SetTrigger("EnemyDestroyed");
                gameObject.GetComponent<BoxCollider2D>().enabled = false;
                IsShot = true;
                ExplosionAudio.Play();
                Destroy(gameObject, 2.5f);
            }
            else if (other.tag=="Player2")
            {
                Debug.Log("Player 2 Damaged");
                Player_2_script.damage();
                Explosion.SetTrigger("EnemyDestroyed");
                gameObject.GetComponent<BoxCollider2D>().enabled = false;
                IsShot = true;
                ExplosionAudio.Play();
                Destroy(gameObject, 2.5f);
            }           




        }
        
        
    }

        
    void firelaser()
    {
        canfire = Time.time + rof;
        GameObject Fired = (GameObject)Instantiate(Laser, transform.position + new Vector3(0, -1, 0), Quaternion.identity);
        Fired.GetComponent<laser>().SetEnemy();
       // Debug.Break();
    }
}
