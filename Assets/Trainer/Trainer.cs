using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trainer : MonoBehaviour
{
    public BoxCollider2D trainerBox;
    

    void Start()
    {
        
    }

    
    void Update()
    {
       
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        Debug.Log("I can see you");
    }

}
