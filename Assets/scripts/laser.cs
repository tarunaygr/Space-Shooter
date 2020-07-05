using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class laser : MonoBehaviour
{
    public GameObject Lasers_collision;
    [SerializeField]
    private float player_speed = 9f,enemy_speed=4f;
    [SerializeField]
    public bool isenemylaser=false;
    player Player_script;
    private Vector3 screenBounds;

    // Start is called before the first frame update
    void Start()
    {
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
    }

    // Update is called once per frame
    void Update()
    {
        movement();

    }

   
  


    void movement()
    {
        if (!isenemylaser)
        {
            transform.Translate(Vector2.up * Time.unscaledDeltaTime * player_speed);
            if (transform.position.y > -screenBounds.y+5)
            {

                Destroy(gameObject);
            }
        }
        else
        {
            transform.Translate(Vector2.down * Time.deltaTime * enemy_speed);
            if (transform.position.y < screenBounds.y-0.5)
            {

                Destroy(gameObject);
            }
        }
    }
        public void SetEnemy()
    {
       isenemylaser = true;
        Debug.Log("Isenemycalled");
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isenemylaser==true && collision.transform.tag=="Player")
        {
            Player_script = collision.GetComponent<player>();
            Player_script.damage();
            Destroy(gameObject);
        }
    }
}
