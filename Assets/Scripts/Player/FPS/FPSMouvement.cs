using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSMouvement : MonoBehaviour
{

    public CharacterController controller;
    public float speed = 12f;
    public float gravity = -5f;
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    public GameObject SpaceShip;
    private float NextShot = 0.15f;
    public float FireDelay = 0.5f;
    public int Damage = 2;
    public LineRenderer line;
    public Camera camera;
    public GameObject UIText;
    private float jumpHeight = 3f;
    public Animator animator;

    Vector3 velocity;
    bool isGrounded;
    bool CanGoInSpaceShip = false;

    private void Start()
    {
        Debug.Log(transform.GetChild(1).GetComponentInChildren<Animator>().name);
        animator = transform.GetChild(1).GetComponentInChildren<Animator>();
        Debug.Log(animator.name);
    }
    void Update()
    {
        Move();
        actions();
    }

    private void Move()
    {
        
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        float x = -Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * speed * Time.deltaTime);
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
    private void actions()
    {
        RayCastCheck();

        if (Input.GetKey(KeyCode.Space))
        {
            Jump();
        }
        if(Input.GetKey(KeyCode.Mouse0) && Time.time > NextShot)
        {
            Debug.Log("Shoot");
            Shoot();
            NextShot = Time.time + FireDelay;
        }
        //Remonter dans l'avion
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (CanGoInSpaceShip)
            {
                GoToSpaceShip();
            }
        }
    }
    
    private void Jump()
    {
        velocity.y = Mathf.Sqrt(jumpHeight * -10f * gravity);
    }
    private void Shoot()
    {
        animator.SetBool("IsShooting", true);
        line.SetPosition(0, camera.transform.position);
        RaycastHit hitInfo;
        if (Physics.Raycast(camera.transform.position, camera.transform.forward, out hitInfo))
        {
            EnemyControllerFPS Enemy = hitInfo.transform.GetComponent<EnemyControllerFPS>();
            if(Enemy != null)
            {
                Enemy.TakeDamage(Damage);
            }
        }
        animator.SetBool("IsShooting", false);


    }
    private void RayCastCheck()
    {
        RaycastHit hitInfo;
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hitInfo, 50f, 1 << 8))
        {
                Interact(hitInfo.transform.tag);
                UIText.SetActive(true);  
        }
        else
        {
            UIText.SetActive(false);
        }
    }

    private void Interact(string InterractObjectName)
    {
        switch (InterractObjectName)
        {
            case "Player":

                CanGoInSpaceShip = true;
                break;
            default:
                CanGoInSpaceShip = false;
                
                break;
        }
        
    }

    private void GoToSpaceShip()
    {
        Destroy(GameObject.FindGameObjectWithTag("Player"));
        Instantiate(SpaceShip, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
