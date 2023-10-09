using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D playerRigid;
    private GameObject indicatorDash,mouse;
    private float playerDirectionHorizontal, playerDirectionVertical, playerVelocity, dashSpeed,dashCoolDown,dashDuration,dashCharging;
    public float playerHealth, decayTime;
    [SerializeField] private bool isDashing, canDash,isDashCharged;

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
        dashCharging = 2;
        mouse = GameObject.Find("Mouse");
        indicatorDash = GameObject.Find("DashIndicator");


    }
    void Start()
    {

    }


    void Update()
    {
        mouse.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition) + new Vector3(0, 0, 10);

        if (isDashing)
        {
            return;
        }
        DontDestroyOnLoad(gameObject);
        Movement();//playermovement
        Health();     
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
        if (canDash&&isDashCharged)
        {
            StartCoroutine(Dash());
        }

        print(playerRigid.velocity.magnitude);
        DirectionIndicator();

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

    private void DirectionIndicator()
    {
        float degree= Mathf.Rad2Deg*Mathf.Atan2(mouse.transform.position.y - indicatorDash.transform.position.y, mouse.transform.position.x - indicatorDash.transform.position.x);
        //indicatorDash.transform.up = mouse.transform.position - indicatorDash.transform.position;
        indicatorDash.transform.RotateAround(transform.position, Vector3.forward, 5);
        //indicatorDash.transform.rotation = Quaternion.AngleAxis(degree, Vector3.forward);
        print(degree);
    }

    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        isDashCharged = false;//reset clicking time
        playerRigid.velocity=new Vector2(1 * dashSpeed,playerDirectionVertical*dashSpeed);
        yield return new WaitForSeconds(dashDuration);
        isDashing = false;
        yield return new WaitForSeconds(dashCoolDown);
        canDash = true;
       
    }
  

}
