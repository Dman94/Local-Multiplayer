using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLogic : MonoBehaviour
{
    Transform m_CamerTarget;
    GameObject[] Players;

    Vector3 CameraPos;

    [SerializeField] Transform Ground;

    // Start is called before the first frame update
    void Start()
    {
        Players = GameObject.FindGameObjectsWithTag("Player");   
        
    }

    // Update is called once per frame
    void Update()
    {
        float PlayerDistance = Vector3.Distance(Players[0].transform.position, Players[1].transform.position);
        float PlayerDistanceHalfPoint = PlayerDistance / 2;
     


        float CameraDistance = Vector3.Distance(transform.position, Ground.position);

        Debug.Log(CameraDistance);

        Debug.Log(PlayerDistanceHalfPoint);
    }

    private void OnDrawGizmos()
    {
        Color color = new Color(1, 0, 0, 0.25f);
        Gizmos.DrawSphere(CameraPos, 1f);
    }
}
