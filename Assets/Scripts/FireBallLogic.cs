using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBallLogic : MonoBehaviour
{
    Rigidbody rb;
    float FireballeSpeed = 8;
    float lifetime = 5;
    Collider m_Collider;

    [SerializeField] ParticleSystem m_Explosion;
    [SerializeField] ParticleSystem FireBall;
   


    void Start()
    {
        
        m_Collider = GetComponent<Collider>();
        rb = GetComponent<Rigidbody>();
        if (rb)
        {
            rb.velocity = transform.forward * FireballeSpeed;
        }
    }
     void Update()
    {
        lifetime -= Time.deltaTime;
        if(lifetime < 0)
        {
            Destroy(gameObject);
        }
    }



    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            PlayerLogic playerLogic = other.gameObject.GetComponent<PlayerLogic>();
            if(playerLogic)
            {
                playerLogic.Die();
            }
           

            FireBall.Stop(true);
            m_Explosion.Play(true);
            rb.velocity = Vector3.zero;
            m_Collider.enabled = false;
           
        }
    }
}
