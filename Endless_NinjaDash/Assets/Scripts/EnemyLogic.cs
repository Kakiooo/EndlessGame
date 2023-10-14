using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLogic : MonoBehaviour
{

    [SerializeField] private delegate void enemyMovement();
    [SerializeField] private static event enemyMovement enemyMovementFunction;
    [SerializeField] List<enemyMovement>movementList = new List<enemyMovement>();
    private GameObject refToPlayer;
    public float speed,decaySpeed;
    private void Awake()
    {
        refToPlayer = GameObject.Find("Player");
        speed = 5;
        decaySpeed = 2.5f;
    }
    void Start()
    {
        movementList.Add(MoveToPlayer);
        enemyMovementFunction += movementList[0];
       
        
    }

    // Update is called once per frame
    void Update()
    {
        enemyMovementFunction();
    }

    void MoveToPlayer()
    {
        transform.position = Vector2.MoveTowards(transform.position, refToPlayer.transform.position, speed * Time.deltaTime);
    }
}
