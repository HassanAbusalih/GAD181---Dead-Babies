using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager_character : MonoBehaviour
{
    public AudioSource Backgroungmusic;
    public AudioSource WalkingSFX;

    // Start is called before the first frame update
    void Start()
    {
        Backgroungmusic = gameObject.AddComponent<AudioSource>();
        WalkingSFX = gameObject.AddComponent<AudioSource>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
