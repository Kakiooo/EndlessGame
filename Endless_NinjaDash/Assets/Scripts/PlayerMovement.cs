using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D playerRigid;
    private float playerDirectionHorizontal,playerDirectionVertical,playerVelocity;
    public float playerHealth, decayTime;

    private void Awake()
    {
        playerRigid = GetComponent<Rigidbody2D>();
        playerVelocity = 10f;
        playerHealth = 100;
        decayTime = 4;
        
    }
    void Start()
    {
        
    }


    void Update()
    {
        DontDestroyOnLoad(gameObject);
        Movement();//playermovement
        Health();
    }

    void Movement()
    {
        playerDirectionHorizontal = Input.GetAxis("Horizontal");
        playerDirectionVertical = Input.GetAxis("Vertical");
        playerRigid.velocity=new Vector2 (playerDirectionHorizontal * playerVelocity, playerDirectionVertical * playerVelocity);//setting velocity
        transform.position = new Vector2(Mathf.Clamp(transform.position.x, -8.5f, 8.5f), Mathf.Clamp(transform.position.y, -4.3f, 4.3f));//limit movement
    }
    void Health()
    {
       if (GameManager.instance.gameFlow == GameManager.GameFlow.gameStart)
        {
            playerHealth -= decayTime*Time.deltaTime;//health decay logic
        }
    }   
    void Dash()
    {

    }

  

}
