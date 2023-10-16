using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BezierRoute : MonoBehaviour
{
    [SerializeField] private Transform[] wayPoints;
    public Vector2 bezierPosition;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnDrawGizmos()
    {
        for (float i = 0; i <= 1; i += 0.05f)
        {
            bezierPosition = Mathf.Pow(1 - i, 3) * wayPoints[0].position +
                             3 * Mathf.Pow(1 - i, 2) * i * wayPoints[1].position +
                             3 * (1 - i) * Mathf.Pow(i, 2) * wayPoints[2].position +
                             Mathf.Pow(i, 3) * wayPoints[3].position;
            Gizmos.DrawSphere(bezierPosition, 0.5f);
        }
    }

}
