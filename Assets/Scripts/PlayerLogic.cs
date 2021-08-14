using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerID
{
    _P1,
    _P2
}

public class PlayerLogic : MonoBehaviour
{
    [SerializeField]
    PlayerID m_playerID;
    CharacterController m_characterController;

    Vector3 m_movement;
    Vector3 m_heightMovement;
    Vector3 m_DashMovement;

    float m_horizontalInput;
    float m_verticalInput;

    float m_movementSpeed = 5.0f;
    float m_DashDistance = 5.0f;
    bool m_dash;
    bool m_jump;
    float m_jumpHeight = 0.35f;
    float m_gravity = 0.981f;


    Animator m_animator;

    [SerializeField]
    Transform m_fireballSpawn;
    [SerializeField]
    GameObject m_fireball;

    bool m_isCastingFireball = false;


    Vector3 SpawnPos;
    Vector3 SpawnRot;

    bool m_isDead;
    const float MAX_Respawn_Time = 2.0f;
    float RespawnTimer = MAX_Respawn_Time;


    //Events
    public delegate void PlayerDeath(int playerNum);
    public static event PlayerDeath onPLayerDeath;




    // Start is called before the first frame update
    void Start()
    {
        m_characterController = GetComponent<CharacterController>();
        m_animator = GetComponent<Animator>();

        SpawnPos = transform.position;
        SpawnRot = transform.forward;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_isDead) { return; }   // if dead then player cannot access controls

        m_horizontalInput = Input.GetAxis("Horizontal" + m_playerID);
        m_verticalInput = Input.GetAxis("Vertical" + m_playerID);

        if (Input.GetButtonDown("Jump" + m_playerID) && m_characterController && m_characterController.isGrounded)
        {
            m_jump = true;
            
        }

        if (m_characterController.isGrounded)
        {
            m_animator.SetBool("Grounded", true);
        }


        if (Input.GetButtonDown("Fire1" + m_playerID) && m_animator)
        {
            m_animator.SetTrigger("CastFireball");
            m_isCastingFireball = true;
        }
    }

    private void FixedUpdate()
    {

        if (m_isDead)
        {
            RespawnTimer -= Time.deltaTime;
            if(RespawnTimer <= 0)
            {
                Respawn();
            }
            return; 
        } // if dead then player cannot access controls

        // Jump Logic
        if (m_jump)
        {
            m_heightMovement.y = m_jumpHeight;
            m_jump = false;
            m_animator.SetTrigger("Jump");
        }

        m_heightMovement.y -= m_gravity * Time.deltaTime;

        m_movement = new Vector3(m_horizontalInput, 0, m_verticalInput) * m_movementSpeed * Time.deltaTime;
     

        // Animator BlendTree float
        if (m_animator)
        {
            m_animator.SetFloat("MovementInput", Mathf.Max(Mathf.Abs(m_horizontalInput), Mathf.Abs(m_verticalInput)));
        }

        // Rotate towards movement direction
        if (m_movement != Vector3.zero)
        {
            transform.forward = m_movement.normalized;
        }
        // assigning all vector values to be zero while in fireball casting state
        if (m_isCastingFireball)
        {
            m_movement = Vector3.zero;
        }

        m_characterController.Move(m_heightMovement + m_movement);

        if (m_characterController.isGrounded)
        {
            m_heightMovement.y = 0;
        }
    }

    public void SetCastingFireballState(bool isCasting)
    {
        m_isCastingFireball = isCasting;
    }

    public void ReleaseFireball()
    {
        Instantiate(m_fireball, m_fireballSpawn.transform.position, transform.rotation);
    }

    public void Die()
    {
        if (m_animator)
        {
            m_animator.SetTrigger("Die");
            m_isDead = true; // Tracks Death State
        }
        if (m_characterController)
        {
            m_characterController.enabled = false;
        }
    }

    public void Respawn()
    {
        m_characterController.enabled = false;
        transform.position = SpawnPos;
        transform.forward = SpawnRot;
        m_characterController.enabled = true;


        RespawnTimer = MAX_Respawn_Time;

        m_isDead = false;

        if (m_animator)
        {
            m_animator.SetTrigger("Respawn");
        }

        if(onPLayerDeath != null)
        {
            onPLayerDeath(getPLayerNum());
        }
    }

    int getPLayerNum()
    {
        if(m_playerID == PlayerID._P1)
        {
            return 1;
        }
       else if (m_playerID == PlayerID._P2)
        {
            return 2;
        }
        return 0;
    }
}
