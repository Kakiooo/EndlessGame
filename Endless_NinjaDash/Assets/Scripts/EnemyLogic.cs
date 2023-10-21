using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLogic : MonoBehaviour
{

    [SerializeField] private delegate void enemyMovement();
    [SerializeField] private static event enemyMovement enemyMovementFunction;
    [SerializeField] List<enemyMovement>movementList = new List<enemyMovement>();
    private GameObject refToPlayer;
    private Transform circularCenter;
    private float speed,decaySpeed,rotateAngleSpeed,originalSpeed,decayRotateAngleSpeed,originalAngleSpeed;
    private UIManager refToUiManager;
    [SerializeField] private Transform[] routes;//route of bezier curve
    [SerializeField] private int routeIndex;//how many routes need to go
    private bool isMoveInCurve;
    [SerializeField] private bool enemyIsEliminated,notMoving;
    private float curveMoveSpeed, t_InterpolatePosition,shakeTimer;
    private Vector2 enemyPosition;
    private void Awake()
    {
        refToPlayer = GameObject.Find("Player");
        refToUiManager = GameObject.Find("GameUI").GetComponent<UIManager>();
        speed = 4;
        originalSpeed = speed;       
        decaySpeed = 2;
        rotateAngleSpeed = 15;
        curveMoveSpeed = 0.5f;
        isMoveInCurve = true;
        if(transform.parent != null) { circularCenter = transform.parent.GetComponent<Transform>(); }
        shakeTimer=0.5f;


    }
    void Start()
    {
        movementList.Add(MoveToPlayer);
        movementList.Add(MoveInCircle);
        enemyMovementFunction += movementList[1];

        refToUiManager.CameraStopShake();
    }

    // Update is called once per frame
    void Update()
    {
        print(enemyIsEliminated);
        if (isMoveInCurve&&CompareTag("BezierCurveEnemy"))
        {
            StartCoroutine(FollowBezierCurve(routeIndex));
        }
        if (CompareTag("CircularEnemy")&&!notMoving)
        {
            MoveInCircle();
        }
        if (CompareTag("DirectEnemy")&&!notMoving)
        {
            MoveToPlayer();
        }

        CameraShakingLogic(); 

    }

    void MoveToPlayer()
    {
        if (Input.GetMouseButton(0) && refToPlayer.GetComponent<PlayerMovement>().canDash)
        {
            speed = decaySpeed;//when dashing enemy moves slower
        }
        else speed = originalSpeed;//reset to normal speed when player is not dashing

        transform.position = Vector2.MoveTowards(transform.position, refToPlayer.transform.position, speed * Time.deltaTime);
    }

    void MoveInCircle()
    {
        circularCenter.transform.Rotate(Vector3.forward,rotateAngleSpeed*Time.deltaTime);      
    }

    private IEnumerator FollowBezierCurve(int routeNumber)//make enemy move along bezier curve
    {
        isMoveInCurve = false;//control the coroutine to start
        Vector2 p0 = routes[routeNumber].GetChild(0).position;
        Vector2 p1 = routes[routeNumber].GetChild(1).position;
        Vector2 p2 = routes[routeNumber].GetChild(2).position;
        Vector2 p3 = routes[routeNumber].GetChild(3).position;
        //setting waypoints
        
        while (t_InterpolatePosition < 1)
        {
            t_InterpolatePosition += Time.deltaTime * curveMoveSpeed;//interpolating point moving
            enemyPosition = Mathf.Pow(1 - t_InterpolatePosition, 3) * p0 +
                            3 * Mathf.Pow(1 - t_InterpolatePosition, 2) * t_InterpolatePosition * p1 +
                            3 * (1 - t_InterpolatePosition) * Mathf.Pow(t_InterpolatePosition, 2) * p2 +
                            Mathf.Pow(t_InterpolatePosition, 3) * p3;//Formula of cubic bezier curve   
            transform.position = enemyPosition;//enemy follow the position of interpolating point
            yield return new WaitForEndOfFrame();
        }

        t_InterpolatePosition = 0;//reset position of interpolating point
        routeIndex++;//change to another bezier curve route
        if (CompareTag("BezierCurveEnemy")&&routeIndex > routes.Length - 1) routeIndex = 0;//reset the route back to 0 to loop the coroutine
        
        isMoveInCurve = true;//loop the coroutine      

    }

    private void OnTriggerEnter2D(Collider2D collision)//when enemy enter player
    {
        if (refToPlayer.GetComponent<PlayerMovement>().isDashing == true&&collision.gameObject.tag=="Player")//condintion for player to eliminate enemies
        {
            enemyIsEliminated = true;
        }
        else if(collision.gameObject.tag == "Player")
        {
            refToPlayer.GetComponent<PlayerMovement>().playerHealth -= 10;
            refToUiManager.ui_healthBar.sizeDelta -= new Vector2(40, 0)*Time.deltaTime;//enemy damage to player
        }
    }
    private void OnTriggerStay2D(Collider2D collision)//when enemy is over player
    {
        if (refToPlayer.GetComponent<PlayerMovement>().isDashing == true && collision.gameObject.tag == "Player")//condintion for player to eliminate enemies
        {
            enemyIsEliminated=true;        
        }
        else if (collision.gameObject.tag == "Player")
        {
            refToPlayer.GetComponent<PlayerMovement>().playerHealth -= 10;
            refToUiManager.ui_healthBar.sizeDelta -= new Vector2(40, 0)*Time.deltaTime;//enemy damage to player
        }
    }

    private void CameraShakingLogic()
    {

        if (shakeTimer > 0 && enemyIsEliminated)
        {
            shakeTimer -= Time.deltaTime;
            refToUiManager.CameraShake();
        }
        else if (shakeTimer < 0)
        {
            refToUiManager.CameraStopShake();
            notMoving = true;
            Destroy(gameObject);
        }
    }
    private void OnDestroy()
    {     
        refToPlayer.GetComponent<PlayerMovement>().playerHealth += 10;
        refToUiManager.ui_healthBar.sizeDelta+=new Vector2(40,0);//restore player health bar when enemy is eliminated      
        enemyIsEliminated=false;
        shakeTimer = 0.5f;
    }
}
