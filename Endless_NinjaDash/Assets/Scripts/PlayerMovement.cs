using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D playerRigid;
    private float playerDirectionHorizontal, playerDirectionVertical, playerVelocity;

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
        Movement();
        
        //testing
    }

    void Movement()
    {
        playerDirectionHorizontal = Input.GetAxis("Horizontal");
        playerDirectionVertical = Input.GetAxis("Vertical");
        playerRigid.velocity=new Vector2 (playerDirectionHorizontal * playerVelocity, playerDirectionVertical * playerVelocity);
        Debug.Log(playerRigid.velocity);
    }

}
