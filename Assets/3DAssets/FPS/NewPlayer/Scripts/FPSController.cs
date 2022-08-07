using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FPSController : MonoBehaviour
{
    public float moveSpeed = 15;
    public float AccelerationSpeed = 30;
    public int Life = 100;
    public int Damage = 2;
    private Rigidbody rb;
    private float NextShot = 0.15f;
    public float FireDelay = 0.5f;
    private float jumpHeight = 5;
    private float height;
    private float initialSpeed;
    public GameObject SpaceShip;
    public Camera camera;
    public GameObject UIText;
    public RectTransform Reticle;
    public WeaponAnimator animator;
    private bool CanPickScene = false;
    public RectTransform LevelPicker;
    private Vector3 moveDir;
    bool CanGoInSpaceShip = false;
    bool CanEnterDunjeon = false;
    private PortalController portalController;
    private int DunjeonIndex;
    bool isZooming = false;
    private FauxGravityBody bodyAttractor;
    // Start is called before the first frame update
   private GameObject cameraComponent;
    private CustomPostProcessing customPostProcessing;



    void Start()
    {
        Time.timeScale = 1;
        rb = GetComponent<Rigidbody>();
        bodyAttractor = GetComponent<FauxGravityBody>();
        /*animator = this.gameObject.transform.GetChild(1).GetComponentInChildren<WeaponAnimator>();*/
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = true;
        initialSpeed = moveSpeed;
        
        cameraComponent = GameObject.FindGameObjectWithTag("MainCam");
        customPostProcessing = cameraComponent.GetComponent<CustomPostProcessing>();
        // En ATTENDANT DE TROUVER UNE SOLUTION : PAS D'ATMOSPHERE customPostProcessing.effects[0] = Resources.Load<PostProcessingEffect>("PlanetEffects2");
        customPostProcessing.effects[1] = Resources.Load<PostProcessingEffect>("Bloom2");
        customPostProcessing.effects[2] = Resources.Load<PostProcessingEffect>("FXAA2");

    }

    
    // Update is called once per frame
    void Update()
    {
        Move();
        Actions();
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position +transform.TransformDirection(moveDir) * moveSpeed * Time.deltaTime);
    }

    private void Move()
    {
        
        bodyAttractor.enabled = true;
        moveDir = new Vector3(-Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
    }

    private void Actions()
    {
        RayCastCheck();
        //Jump
        if (Input.GetKey(KeyCode.Space))
        {
            Jump();
        }
        //Run
        if (Input.GetKey(KeyCode.LeftShift))
        {
            moveSpeed = AccelerationSpeed;
        }
        else
        {
            moveSpeed = initialSpeed;
        }
        //Shoot
        if (Input.GetKey(KeyCode.Mouse0) && Time.time > NextShot)
        {
            animator.StartShooting();
            Shoot();
            NextShot = Time.time + FireDelay;
        }
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            animator.StopShooting();
        }
        //Zomm
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            Zoom();
        }
        //Interract
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (CanGoInSpaceShip)
            {
                GoToSpaceShip();
            }
            if (CanPickScene)
            {
                PickALevel();
            }
            if(CanEnterDunjeon)
            {
                portalController.loadingSystem.LoadLevel(DunjeonIndex);
            }
        }

    }

    private void Jump()
    {
        //rb.mass = 10;
        bodyAttractor.enabled = false;

        moveDir.y += jumpHeight;
    }
    private void Shoot()
    {
        RaycastHit hitInfo;
        if (Physics.Raycast(camera.transform.position, camera.transform.forward, out hitInfo))
        {
            EnemyControllerFPS EnemyFPS = hitInfo.transform.GetComponent<EnemyControllerFPS>();
            EnemyController Enemy = hitInfo.transform.GetComponent<EnemyController>();
            if (EnemyFPS != null)
            {
                StartCoroutine(EnemyFPS.TakeDamage(Damage));
            }
            else if (Enemy != null)
            {
                Enemy.TakeDamage(Damage);
            }
        }
    }
    private void Zoom()
    {
        if (!isZooming)
        {
            camera.fieldOfView = 40;
            Reticle.sizeDelta *= 2;
        }
        else
        {
            camera.fieldOfView = 60;
            Reticle.sizeDelta /= 2;
        }
        isZooming = !isZooming;
    }

    private void RayCastCheck()
    {
        RaycastHit hitInfo;
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hitInfo, 50f, 1 << 8))
        {
            Interact(hitInfo.transform.tag, hitInfo.transform.gameObject);
            UIText.SetActive(true);
        }
        else
        {
            UIText.SetActive(false);
            CanGoInSpaceShip = false;
            CanPickScene = false;
            CanEnterDunjeon = false;
        }
    }
    private void Interact(string InterractObjectName, GameObject interraclable)
    {
        switch (InterractObjectName)
        {
            case "Player":
                CanGoInSpaceShip = true;
                break;

            case "Portal":
                CanPickScene = true;
                break;
            case "EnemyBase":
                CanEnterDunjeon = true;
                portalController = interraclable.GetComponent<PortalController>();
                DunjeonIndex = interraclable.GetComponent<PortalController>().SceneIndex;
                break;

            default:
               
                break;
        }
    }

    private void PickALevel()
    {
        //Pause game
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        LevelPicker.gameObject.SetActive(true);
      
    }
    private void GoToSpaceShip()
    {
        Destroy(GameObject.FindGameObjectWithTag("Player"));
        Instantiate(SpaceShip, transform.position, transform.rotation);
        Destroy(gameObject);
    }
    public void TakeDamage(int amount)
    {
        Life -= amount;
        if (Life <= 0f)
        {
            Die();
        }
    }
    private void Die()
    {
        Destroy(gameObject);
    }
 

}
