using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D playerRigid;
    private float playerDirection,playerVelocity;

    private void Awake()
    {
        playerRigid = GetComponent<Rigidbody2D>();
        playerVelocity = 10f;
    }
    void Start()
    {
        
    }


    void Update()
    {   
        HorizontalMove();
        //testing
    }

    void HorizontalMove()
    {
        playerDirection = Input.GetAxis("Horizontal");
        playerRigid.velocity=new Vector2 (playerDirection*playerVelocity,0);
     
    }
}
