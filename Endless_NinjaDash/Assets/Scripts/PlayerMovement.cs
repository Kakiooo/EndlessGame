using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D playerRigid;
    private float playerDirectionHorizontal,playerDirectionVertical,playerVelocity,xPosition,yPosition;

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
        Movement();//playermovement        
    }

    void Movement()
    {
        playerDirectionHorizontal = Input.GetAxis("Horizontal");
        playerDirectionVertical = Input.GetAxis("Vertical");
        playerRigid.velocity=new Vector2 (playerDirectionHorizontal * playerVelocity, playerDirectionVertical * playerVelocity);
        transform.position = new Vector2(Mathf.Clamp(transform.position.x, -8.5f, 8.5f), Mathf.Clamp(transform.position.y, -4.5f, 4.5f));
    }

}
