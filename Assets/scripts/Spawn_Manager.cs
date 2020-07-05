using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn_Manager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private GameObject Enemy;
    [SerializeField]
    private GameObject EnemySpawn;
    [SerializeField]
    private GameObject[] PowerUps;
    GameManager gamemanager;
    public bool Alive = true;
    private Vector3 location1,location2;
    int spawnrate = 1;

    public bool EnemyFire = false;

    void Awake()
    {
        spawnrate = 1;
        gamemanager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
        Asteroid.ast += StartSpawning;
        Debug.Log("subscribed");

    }
    private void Start()
    {

    }
    public void StartSpawning()
    {
        StartCoroutine("SpawnRoutine");
        StartCoroutine("SpawnPowerUp");
    }

    // Update is called once per frame
    void Update()
    {
        location1 = new Vector3(Random.Range(-6.3f, 6.3f), 8, 0);
        location2= new Vector3(Random.Range(-6.3f, 6.3f), 8, 0);

    }
    IEnumerator SpawnRoutine()
    {
        if (!gamemanager.CoopMode)
        {


            while (Alive == true)
            {
                yield return new WaitForSeconds(2);
                for (int i = 0; i <spawnrate; i++)
                {
                    if (Alive&& GameObject.FindGameObjectsWithTag("Enemy").Length<4)
                    {
                        GameObject newenemy1 = Instantiate(Enemy, location1, Quaternion.identity);
                        newenemy1.transform.parent = EnemySpawn.transform;
                    }
                    yield return new WaitForSeconds(1);
                }
            }
        }
        else
        {
            while (!gamemanager.returnPlayer2status() || !gamemanager.returnPlayer1status())
            {
                yield return new WaitForSeconds(2);
                for (int i = 0; i < spawnrate; i++)
                {
                    if (!gamemanager.returnPlayer2status() || !gamemanager.returnPlayer1status())
                    {
                        GameObject newenemy1 = Instantiate(Enemy, location1, Quaternion.identity);
                        newenemy1.transform.parent = EnemySpawn.transform;
                        GameObject newenemy2 = Instantiate(Enemy, location2, Quaternion.identity);
                        newenemy2.transform.parent = EnemySpawn.transform;
                    }
                }
            }
        }
    }
    public void Death()
    {
        Alive = false;
    }
    IEnumerator SpawnPowerUp()
    {   if (!gamemanager.CoopMode)
        {
            while (Alive == true)
            {
                yield return new WaitForSeconds(Random.Range(20, 41));
                int Random_PowerUp = Random.Range(0, 3);
                if (Alive)
                    Instantiate(PowerUps[Random_PowerUp], location2, Quaternion.identity);
            }
        }
        else
        {
            while (!gamemanager.returnPlayer2status() || !gamemanager.returnPlayer1status())
            {
                yield return new WaitForSeconds(Random.Range(20, 41));
                int Random_PowerUp = Random.Range(0, 3);
                if (!gamemanager.returnPlayer2status() || !gamemanager.returnPlayer1status())
                    Instantiate(PowerUps[Random_PowerUp], location2, Quaternion.identity);
            }
        }
    }
  public void UpdateSpawnRate()
    { 
        spawnrate++; 
    }
    public void EnableEnemyFire()
    {
        EnemyFire = true;
    }

}
       
