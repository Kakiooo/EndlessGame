
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D playerRigid;
    private GameObject dashDirection,refToMouse;
    private float playerDirectionHorizontal, playerDirectionVertical, playerVelocity, dashPower, dashCoolDown,dashDuration;
    public float playerHealth, decayTime,dashCharging;
    [SerializeField] private bool isDashing, canDash,isDashCharged;

    private void Awake()
    {
        refToMouse = GameObject.Find("Mouse");
        playerRigid = GetComponent<Rigidbody2D>();
        playerVelocity = 10f;
        playerHealth = 100;
        decayTime = 4;
        dashPower = 20;
        dashCoolDown = 1;
        dashDuration = 0.25f;
        canDash = true;
        dashCharging = 2;
        dashDirection = GameObject.Find("direction");


    }
    void Start()
    {

    }

    private void FixedUpdate()
    {
        refToMouse.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition) + new Vector3(0, 0, 10);
        if (isDashing)
        {
            return;
        }
        if (GameManager.instance.gameFlow == GameManager.GameFlow.gameStart)
        {
                      
            DontDestroyOnLoad(gameObject);            
            Movement();//playermovement
            Health();
            if (!Input.GetKey(KeyCode.Mouse0))
            {
                dashCharging = 2;
            }
            if (Input.GetKey(KeyCode.Mouse0))
            {
                playerRigid.velocity = new Vector2(0, 0);//when player dashing,movement is disfunctional
                dashCharging -= Time.deltaTime;//hold the mouse and wait for dash
                if (dashCharging < 0)
                {
                    isDashCharged = true;
                    dashCharging = 2;//reset value
                }
            }
            if (canDash && isDashCharged)
            {
                StartCoroutine(Dash());
            }
            if (playerHealth <= 0)
            {
                //GameManager.instance.gameFlow = GameManager.GameFlow.gameEnd;
            }
        }
     
        print(playerRigid.velocity.magnitude);
    }

    void Movement()
    {
       
        playerDirectionHorizontal = Input.GetAxis("Horizontal");
        playerDirectionVertical = Input.GetAxis("Vertical");
        playerRigid.velocity=new Vector2 (playerDirectionHorizontal * playerVelocity, playerDirectionVertical * playerVelocity);//setting velocity
        //transform.position = new Vector2(Mathf.Clamp(transform.position.x, -8.5f, 8.5f), Mathf.Clamp(transform.position.y, -4.3f, 4.3f));//limit movement

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
        Vector3 direction = (refToMouse.transform.position - transform.position).normalized;
        Color cl = dashDirection.GetComponent<SpriteRenderer>().color;
        dashDirection.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0);//hide dash direction sign when dashing 
        canDash = false;
        isDashing = true;
        isDashCharged = false;//reset clicking time
        playerRigid.AddForce(direction * dashPower, ForceMode2D.Impulse);
        yield return new WaitForSeconds(dashDuration);
        isDashing = false;
        yield return new WaitForSeconds(dashCoolDown);
        canDash = true;
        dashDirection.GetComponent<SpriteRenderer>().color = cl;//reset dash direction sign after dashing


    }
  

}
