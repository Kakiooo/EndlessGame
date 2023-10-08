using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D playerRigid;
    private float playerDirectionHorizontal, playerDirectionVertical, playerVelocity, dashSpeed,dashCoolDown,dashDuration;
    public float playerHealth, decayTime;
    [SerializeField] private bool isDashing, canDash;

    private void Awake()
    {
        playerRigid = GetComponent<Rigidbody2D>();
        playerVelocity = 10f;
        playerHealth = 100;
        decayTime = 4;
        dashSpeed = 20;
        dashCoolDown = 1;
        dashDuration = 0.25f;
        canDash = true;


    }
    void Start()
    {

    }


    void Update()
    {
        if (isDashing)
        {
            return;
        }
        DontDestroyOnLoad(gameObject);
        Movement();//playermovement
        Health();     
        if (Input.GetKeyDown(KeyCode.Space)&&canDash)
        {
            StartCoroutine(Dash());
        }

        print(playerRigid.velocity.magnitude);


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

    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        playerRigid.velocity=new Vector2(playerDirectionHorizontal * dashSpeed,playerDirectionVertical*dashSpeed);
        yield return new WaitForSeconds(dashDuration);
        isDashing = false;
        yield return new WaitForSeconds(dashCoolDown);
        canDash = true;
    }
  

}
