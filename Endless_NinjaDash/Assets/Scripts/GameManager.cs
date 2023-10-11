using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;//creating singleton
    public enum GameFlow {gameStart,gamePause,gameEnd}//setting game flow
    public GameFlow gameFlow;
    private PlayerMovement refToPlayerScript;
    public Camera refToCM;
    private CinemachineVirtualCamera refToVirtual;
    private float zoomingTime;
    
    private void Awake()
    {
        instance = this;//connect to this script
        gameFlow = GameFlow.gameStart;
        refToCM=GameObject.Find("Main Camera").GetComponent<Camera>();
        refToVirtual = GameObject.Find("Virtual Camera").GetComponent<CinemachineVirtualCamera>();
        refToPlayerScript = GameObject.Find("Player").GetComponent<PlayerMovement>();
        zoomingTime = 2;
    }

    void Start()
    {

    }


    void Update()
    {

    }

    void CameraMovement()
    {

    }
}
