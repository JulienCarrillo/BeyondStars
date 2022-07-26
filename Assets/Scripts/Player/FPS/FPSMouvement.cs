using UnityEngine;
using UnityEngine.UI;

public class FPSMouvement : MonoBehaviour
{

    public CharacterController controller;
    public float speed = 12f;
    public int Damage = 2;
    public float gravity = -5f;
    public float groundDistance = 0.4f;
    private float NextShot = 0.15f;
    public float FireDelay = 0.5f;
    private float jumpHeight = 3f;
    private float initialSpeed;
    public Transform groundCheck;
    public LayerMask groundMask;
    public GameObject SpaceShip;
    public Camera camera;
    public GameObject UIText;
    public RectTransform Reticle;
    private WeaponAnimator animator;
    


    Vector3 velocity;
    bool isGrounded;
    bool CanGoInSpaceShip = false;
    bool isZooming = false;

    private void Start()
    {
        animator = this.gameObject.transform.GetChild(1).GetComponentInChildren<WeaponAnimator>();

        initialSpeed = speed;
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
            animator.StartShooting();
            Shoot();
            NextShot = Time.time + FireDelay;
        }
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            animator.StopShooting();
        }

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            Zoom();
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            Run();
        }else
        {
            speed = initialSpeed;
        }

        if (Input.GetKey(KeyCode.Tab))
        {
            controller.height = 1.5f;
        }
        else
        {
            controller.height = 4;
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

    private void Run()
    {
        speed = 30f;
    }
    
    private void Jump()
    {
        velocity.y = Mathf.Sqrt(jumpHeight * -10f * gravity);
    }
    private void Zoom()
    {
        if (!isZooming)
        {
            camera.fieldOfView = 30;
            Reticle.sizeDelta *= 2;
        }
        else
        {
            camera.fieldOfView = 50;
            Reticle.sizeDelta /= 2;
        }
        isZooming = !isZooming;
    }
    private void Shoot()
    {
        RaycastHit hitInfo;
        if (Physics.Raycast(camera.transform.position, camera.transform.forward, out hitInfo))
        {
            EnemyControllerFPS Enemy = hitInfo.transform.GetComponent<EnemyControllerFPS>();
            if(Enemy != null)
            {
                Enemy.TakeDamage(Damage);
            }
        }
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
