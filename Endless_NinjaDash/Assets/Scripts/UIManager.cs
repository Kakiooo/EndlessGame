using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cinemachine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI refToTextMesh;
    public RectTransform ui_healthBar;
    private PlayerMovement refToPlayerScript;
    private GameObject indicatorDash, mouse, direction,refToPlayer;
    private CinemachineVirtualCamera refToVirtualCM;
    public float duration, totalTime,shakeIntensity=2;
    public int num_eliminated;
    public bool isEnemyDestroied;
    [SerializeField] CinemachineBasicMultiChannelPerlin shakeProperties;


    private void Awake()
    {
        ui_healthBar = GameObject.Find("HealthBar").GetComponent<RectTransform>();
        refToPlayerScript = GameObject.Find("Player").GetComponent<PlayerMovement>();
        mouse = GameObject.Find("Mouse");
        indicatorDash = GameObject.Find("DashIndicator");
        direction = GameObject.Find("direction");
        refToPlayer = GameObject.Find("Player");
        refToVirtualCM = GameObject.Find("Virtual Camera").GetComponent<CinemachineVirtualCamera>();
        shakeProperties = refToVirtualCM.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        totalTime = 3;
        refToTextMesh = GameObject.Find("Text_Enemy").GetComponent<TextMeshProUGUI>();
        //duration = totalTime;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       
        if (GameManager.instance.gameFlow==GameManager.GameFlow.gameStart)
        {
            refToTextMesh.text = ""+num_eliminated;
            mouse.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition) + new Vector3(0, 0, 10);
            indicatorDash.transform.position = refToPlayer.transform.position; //dash direction indicator follow the player
            DirectionIndicator();
            CameraEffect();
            ui_healthBar.sizeDelta -= new Vector2(refToPlayerScript.decayTime*4, 0)*Time.deltaTime;//scalling health bar according to players health decay and time
        }
    }

    private void DirectionIndicator()
    {
        float degree = Mathf.Rad2Deg * Mathf.Atan2(mouse.transform.position.y - indicatorDash.transform.position.y, mouse.transform.position.x - indicatorDash.transform.position.x);
        //indicatorDash.transform.up = mouse.transform.position - indicatorDash.transform.position;
        indicatorDash.transform.localRotation = Quaternion.AngleAxis(degree, Vector3.forward);
    }

    private void CameraEffect()
    {
        float zoomInTime = 2;
        float zoomOutTime = 4f;
        if (refToPlayerScript.isDuringCharging)//when dashing...
        {
            refToVirtualCM.m_Lens.OrthographicSize-= zoomInTime * Time.deltaTime;//zoom in virtual camera when player is dashing
            if (refToVirtualCM.m_Lens.OrthographicSize <= 6)
            {
                refToVirtualCM.m_Lens.OrthographicSize=6;//when zoom in camera limit the size of virtual camera
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

    public void CameraShake()
    {
        
        shakeProperties.m_FrequencyGain = 1;
        shakeProperties.m_AmplitudeGain = 3;
    }
    public void CameraStopShake()
    {
        shakeProperties.m_AmplitudeGain = 0;
        print("Works");
    }


}
