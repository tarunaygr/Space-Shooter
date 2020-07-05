using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{   [SerializeField]
    private float _speed = 20f;
    [SerializeField]
    GameObject Explosion;
    public static Action ast; 
    public Spawn_Manager sm;

    // Start is called before the first frame update


    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.back * _speed * Time.deltaTime);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag=="laser")
        {
            Instantiate(Explosion, transform.position, Quaternion.identity);
            Destroy(collision.gameObject);
            ast?.Invoke();
            Destroy(gameObject);
        }
    }
}
