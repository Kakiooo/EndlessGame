using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLogic : MonoBehaviour
{

    [SerializeField] private delegate void enemyMovement();
    [SerializeField] private static event enemyMovement enemyMovementFunction;
    [SerializeField] List<enemyMovement>movementList = new List<enemyMovement>();
    private GameObject refToPlayer,wayPoint;
    public float speed,decaySpeed,rotateAngleSpeed;
    [SerializeField] private Transform[] routes;//route of bezier curve
    [SerializeField] private int routeIndex;//how many routes need to go
    private bool isMoveInCurve;
    private float curveMoveSpeed, t_InterpolatePosition;
    private Vector2 enemyPosition;
    private void Awake()
    {
        refToPlayer = GameObject.Find("Player");
        wayPoint = GameObject.Find("CircularCenter");
        speed = 4;
        decaySpeed = 2;
        rotateAngleSpeed = 1;
        curveMoveSpeed = 0.5f;
        isMoveInCurve = true;
    }
    void Start()
    {
        movementList.Add(MoveToPlayer);
        movementList.Add(MoveInCircle);
       enemyMovementFunction += movementList[1];
     
        
    }

    // Update is called once per frame
    void Update()
    {
        enemyMovementFunction();
        if (isMoveInCurve&&CompareTag("BezierCurveEnemy"))
        {
            StartCoroutine(FollowBezierCurve(routeIndex));
        }
    }

    void MoveToPlayer()
    {
        transform.position = Vector2.MoveTowards(transform.position, refToPlayer.transform.position, speed * Time.deltaTime);
    }

    void MoveInCircle()
    {       
        wayPoint.transform.Rotate(Vector3.forward,rotateAngleSpeed);      
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
        if (routeIndex > routes.Length - 1) routeIndex = 0;//reset the route back to 0 to loop the coroutine
        
        isMoveInCurve = true;//loop the coroutine
        

    }
}
