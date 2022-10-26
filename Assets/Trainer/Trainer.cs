using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trainer : MonoBehaviour
{
    public BoxCollider2D trainerbox;
    public BoxCollider2D playerbox;
    

    void Start()
    {
        
    }

    
    void Update()
    {
       
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision == null)
        Debug.Log("I can see you");
    }

}
