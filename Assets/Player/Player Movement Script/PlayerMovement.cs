using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb2D;
    public Animator anim;
    
    
    private float speed = 3.0f;
    Vector2 movementOfPlayer;


    // Start is called before the first frame update
  

    // Update is called once per frame
    void Update()
    {
        movementOfPlayer.x = Input.GetAxisRaw("Horizontal");
        movementOfPlayer.y = Input.GetAxisRaw("Vertical");
        PlayerAnimation();
    }

    private void FixedUpdate()
    {
        rb2D.MovePosition(rb2D.position + movementOfPlayer * speed * Time.fixedDeltaTime);
        
    }
    void PlayerAnimation()
    {
        anim.SetFloat("ySpeed", movementOfPlayer.y);
        anim.SetFloat("xSpeed", Mathf.Abs(movementOfPlayer.x));
        if(movementOfPlayer.x < 0)
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
        else if (movementOfPlayer.x > 0)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }    
    }

}
