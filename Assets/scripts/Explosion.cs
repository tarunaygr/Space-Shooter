using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    AudioSource explode;
    // Start is called before the first frame update
    void Start()
    {
        explode = GetComponent<AudioSource>();
        explode.Play();
        Destroy(gameObject, 1.75f);
    }

    
}
