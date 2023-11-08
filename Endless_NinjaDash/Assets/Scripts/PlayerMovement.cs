
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D playerRigid;
    private GameObject dashDirection,refToMouse;
    private float playerDirectionHorizontal, playerDirectionVertical, playerVelocity,dashDuration, dashChargeTime,decaySpeed;
    public float playerHealth, decayTime, dashCoolDown,value_dashCharging, dashPower;
    public bool isDuringCharging, isDashing, canDash, isDashCharged,isRestoreBar;
    private UIManager refToUIManager;

    private void Awake()
    {
        refToMouse = GameObject.Find("Mouse");
        refToUIManager = GameObject.Find("GameUI").GetComponent<UIManager>();   
        playerRigid = GetComponent<Rigidbody2D>();
        playerVelocity = 10f;
        playerHealth = 100;
        decayTime = 4;
        dashPower = 10;
        dashCoolDown = 1;
        dashDuration = 0.25f;
        canDash = true;
        dashChargeTime = 3;
        decaySpeed = 0.25f;
        value_dashCharging = dashChargeTime;
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

            playerHealth -= decayTime * Time.deltaTime;//health decay logic
            DontDestroyOnLoad(gameObject);            
            Movement();//playermovement
            DashPowerCharge();

            if (playerHealth <= 0)
            {
                //GameManager.instance.gameFlow = GameManager.GameFlow.gameEnd;
            }
           
        }
     
       
    }

    void Movement()
    {
       
        playerDirectionHorizontal = Input.GetAxis("Horizontal");
        playerDirectionVertical = Input.GetAxis("Vertical");
        playerRigid.velocity=new Vector2 (playerDirectionHorizontal * playerVelocity, playerDirectionVertical * playerVelocity);//setting velocity

    }

    private void DashPowerCharge()
    {
        if (!Input.GetKey(KeyCode.Mouse0))
        {

            dashChargeTime = 2;
            isDuringCharging = false;//use to determine if the camera need to zoom
            dashChargeTime = 2;//reset value
            if (isDashCharged)
            {
                StartCoroutine(Dash());
            }
        }
        if (Input.GetKey(KeyCode.Mouse0) && canDash)
        {
            dashPower += 10*Time.deltaTime;//adding dash power when charging for longer time
            isDashCharged = true;
            isDuringCharging = true;//use to determine if the camera need to zoom
            playerRigid.velocity = new Vector2(playerDirectionHorizontal * playerVelocity * decaySpeed, playerDirectionVertical * playerVelocity * decaySpeed);//when player accumulate dashing,velocity decreases
            dashChargeTime -= Time.deltaTime;//hold the mouse and wait for dash
            if (dashChargeTime < 0)
            {
                float maximumDashPower = 30;
                dashPower =maximumDashPower;//maximum dashpower
            }
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
        isRestoreBar=true;
        yield return new WaitForSeconds(dashCoolDown);
        isRestoreBar = false;
        canDash = true;
        dashDirection.GetComponent<SpriteRenderer>().color = cl;//reset dash direction sign after dashing
        dashPower = 10;//reset dash power

    }


}
