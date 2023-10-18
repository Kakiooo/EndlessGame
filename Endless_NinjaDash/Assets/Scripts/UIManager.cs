using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cinemachine;

public class UIManager : MonoBehaviour
{
    public RectTransform ui_healthBar;
    private PlayerMovement refToPlayerScript;
    private GameObject indicatorDash, mouse, direction,refToPlayer;
    private CinemachineVirtualCamera refToVirtualCM;


    private void Awake()
    {
        ui_healthBar = GameObject.Find("HealthBar").GetComponent<RectTransform>();
        refToPlayerScript = GameObject.Find("Player").GetComponent<PlayerMovement>();
        mouse = GameObject.Find("Mouse");
        indicatorDash = GameObject.Find("DashIndicator");
        direction = GameObject.Find("direction");
        refToPlayer = GameObject.Find("Player");
        refToVirtualCM = GameObject.Find("Virtual Camera").GetComponent<CinemachineVirtualCamera>();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       
        if (GameManager.instance.gameFlow==GameManager.GameFlow.gameStart)
        {
            mouse.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition) + new Vector3(0, 0, 10);
            indicatorDash.transform.position = refToPlayer.transform.position; //dash direction indicator follow the player
            DirectionIndicator();
            cameraEffect();
            ui_healthBar.sizeDelta -= new Vector2(refToPlayerScript.decayTime*4, 0)*Time.deltaTime;
        }
    }

    private void DirectionIndicator()
    {
        float degree = Mathf.Rad2Deg * Mathf.Atan2(mouse.transform.position.y - indicatorDash.transform.position.y, mouse.transform.position.x - indicatorDash.transform.position.x);
        //indicatorDash.transform.up = mouse.transform.position - indicatorDash.transform.position;
        indicatorDash.transform.localRotation = Quaternion.AngleAxis(degree, Vector3.forward);
        print(degree);
    }

    private void cameraEffect()
    {
        float zoomInTime = 2;
        float zoomOutTime = 4f;
        if (refToPlayerScript.isDuringCharging)//when dashing...
        {
            refToVirtualCM.m_Lens.OrthographicSize-= zoomInTime * Time.deltaTime;//zoom in virtual camera when player is dashing
            if (refToVirtualCM.m_Lens.OrthographicSize <= 5)
            {
                refToVirtualCM.m_Lens.OrthographicSize=5;//when zoom in camera limit the size of virtual camera
            }
        }
        else //when is not dashing...
        { 
            refToVirtualCM.m_Lens.OrthographicSize += zoomOutTime * Time.deltaTime;//zoom out virtual camera when player is not dashing
            if (refToVirtualCM.m_Lens.OrthographicSize >=8)
            {
                refToVirtualCM.m_Lens.OrthographicSize =8;//when zoom out camera limit the size of virtual camera
            }
        }

    }


}
