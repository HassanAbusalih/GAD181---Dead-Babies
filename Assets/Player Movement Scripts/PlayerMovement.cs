using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb2D;
    
    
    private float speed = 3.0f;
    Vector2 movementofplayer;


    // Start is called before the first frame update
  

    // Update is called once per frame
    void Update()
    {
        movementofplayer.x = Input.GetAxisRaw("Horizontal");
        movementofplayer.y = Input.GetAxisRaw("Vertical");
        
    }

    private void FixedUpdate()
    {
        rb2D.MovePosition(rb2D.position + movementofplayer * speed * Time.fixedDeltaTime);
        
    }
}
