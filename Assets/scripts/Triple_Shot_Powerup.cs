using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Triple_Shot_Powerup : MonoBehaviour
{
    [SerializeField]
    private float _speed=3f;
    [SerializeField]
    private int ID;
    player PowerUp;
    [SerializeField]
    AudioClip PickUp;
    private GameManager gamemanager;
    void Start()
    {
        transform.position = new Vector3(0, 8, 0);
       
    }
    private void Awake()
    {
        gamemanager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        if (transform.position.y<=-6)
        {
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!gamemanager.CoopMode)
        {


            if (collision.transform.tag == "Player")
            {
                PowerUp = collision.GetComponent<player>();
                if (PowerUp != null)
                {
                    if (ID == 0)
                        PowerUp.ActivateTripleShot();
                    else if (ID == 1)
                        PowerUp.ActivateSpeed();
                    else if (ID == 2)
                        PowerUp.ActivateShield();
                    AudioSource.PlayClipAtPoint(PickUp, transform.position);
                    Destroy(gameObject);
                }


            }
        }
        else
        {
            if (collision.tag=="Player1")

            {
                PowerUp = collision.GetComponent<player>();
                if (PowerUp != null)
                {
                    if (ID == 0)
                        PowerUp.ActivateTripleShot();
                    else if (ID == 1)
                        PowerUp.ActivateSpeed();
                    else if (ID == 2)
                        PowerUp.ActivateShield();
                    AudioSource.PlayClipAtPoint(PickUp, transform.position);
                    Destroy(gameObject);
                }
            }
            else if (collision.tag=="Player2")
            {
                PowerUp = collision.GetComponent<player>();
                if (PowerUp != null)
                {
                    if (ID == 0)
                        PowerUp.ActivateTripleShot();
                    else if (ID == 1)
                        PowerUp.ActivateSpeed();
                    else if (ID == 2)
                        PowerUp.ActivateShield();
                    AudioSource.PlayClipAtPoint(PickUp, transform.position);
                    Destroy(gameObject);
                }
            }
        }
    }
}
