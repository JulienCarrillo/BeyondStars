using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public bool throttle => Input.GetKey(KeyCode.Space);
    public float pitchPower, rollPower, yawPower, enginePower;
    public GameObject Bullet;
    public int damage = 12;
    private float NextShot = 0.15f;
    public float FireDelay = 0.5f;
    public GameObject[] BulletsSpawner;
    public LineRenderer line;
    public GameObject FX;
    LayerMask layer = 1 << 8;
    public float Life = 100f;
    private float activeRoll, activePitch, activeYaw;
    private float enginePowerMax;
    public GameObject[] prefabList;
    public GameObject Missile;
    public Image heathBar;
    //UI
    public GameObject ShootingText;
    public GameObject LandingText;

    private float m_timeRemainingLock = 3;
    public float timeRemainingLock = 0;
    EnemyController target;
    private bool CanShootMissile = false;

    private bool CanLand = false;
    private bool IsLanding = false;
    private bool CanTakeOff = false;
    private Transform m_targetPosition;

    public bool CanChangeScene = false;
    public string NextScene = null;

    //Changement joueur 
    public GameObject FPSPlayer;


    Rigidbody m_Rigidbody;
    private AnimationController animationController;

  
    void Start()
    {
      
        
        heathBar = GameObject.FindGameObjectWithTag("HeathBar").GetComponent<Image>();
        Cursor.lockState = CursorLockMode.Locked;
        //to hide the curser
        Cursor.visible = false;
        m_Rigidbody = GetComponent<Rigidbody>();
        animationController = GetComponent<AnimationController>();
        enginePowerMax = enginePower;
    }
    void Update()
    {
        Actions();
    }

    private void Actions()
    {
        heathBar.fillAmount = Life / 100;
        Throttle();
        LokingTarget();
        CheckIfLanding();

        if (Input.GetKey(KeyCode.Mouse0) && Time.time > NextShot)
        {
            Attack();
            
            NextShot = Time.time + FireDelay;
        }
        if (Input.GetKeyDown(KeyCode.Mouse1) && CanShootMissile)
        {
            ShootMissile(target);
        }
        if(Input.GetKeyDown(KeyCode.E) )
        {
            if (CanLand)
            {
                Land();
            }else if (CanTakeOff) 
            {
                TakeOfShip();
            }else if (CanChangeScene)
            {
                ChangeScene(NextScene);
            }
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (IsLanding)
            {
                GameObject ActualShipGFX = GameObject.FindGameObjectWithTag("Player");

                Instantiate(ActualShipGFX, transform.position + new Vector3(0,4,0), transform.rotation);
                Instantiate(FPSPlayer, transform.position + new Vector3(10,4,0), transform.rotation);
                
                Destroy(gameObject);
            }
        }
        if (Input.GetKey(KeyCode.LeftShift))
        {
            if(enginePower >= 2f)
            {
                enginePower -= 2f;

            }
        }else
        {
            if (enginePower < enginePowerMax )
            {
                enginePower += 20f;

            }
        }

        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            animationController.StopAttackAnimation();
        }

    }
    private void CheckIfLanding()
    {

        RaycastHit hitInfo;
        line.SetPosition(0, transform.position);

        if (Physics.Raycast(transform.position,-Vector3.up, out hitInfo, 50f, 1<<9))
        {
            //Trigger UI
            LandingText.SetActive(true);
            CanLand = true;
            line.SetPosition(1, hitInfo.point);
            m_targetPosition = hitInfo.transform;
        }
        else
        {
            CanLand = false;
            if (!IsLanding)
            {
                LandingText.SetActive(false);
            }
        }
    }
    private void Land()
    {
        StartCoroutine(animationController.StartLandingAnimation());
        IsLanding = true;
        m_Rigidbody.useGravity = true;
        m_Rigidbody.isKinematic = false;
        m_Rigidbody.mass = 10f;
        m_Rigidbody.AddForce(Vector3.up, ForceMode.Impulse);
        enginePower = 0;
        transform.rotation = Quaternion.identity;
        m_Rigidbody.freezeRotation = true;
        LandingText.GetComponent<Text>().text = "Take off (E)";
        LandingText.SetActive(true);

        CanTakeOff = true;
    }

    private void TakeOfShip()
    {
        m_Rigidbody.useGravity= false;
        m_Rigidbody.isKinematic = true;
        m_Rigidbody.mass = 1f;
        m_Rigidbody.AddForce(-Vector3.up, ForceMode.Impulse);
        enginePower = enginePowerMax;
        m_Rigidbody.freezeRotation = false;
        IsLanding = false;
        LandingText.SetActive(false);
        animationController.StopLandingAnimation();
        LandingText.GetComponent<Text>().text = "Land (E)";
    }

    private void Throttle()
    {
        if(!IsLanding)
        { 
            if (throttle)
            {
                transform.position += transform.forward * enginePower * Time.deltaTime;

                activePitch = Input.GetAxisRaw("Mouse Y") * pitchPower * 2 * Time.deltaTime;
                activeRoll = Input.GetAxisRaw("Horizontal") * rollPower * Time.deltaTime;
                activeYaw = Input.GetAxisRaw("Mouse X") * yawPower * Time.deltaTime;
                transform.Rotate(activePitch * pitchPower * Time.deltaTime,
                    activeYaw * yawPower * Time.deltaTime,
                    activeRoll * rollPower * Time.deltaTime, Space.Self);
            }
            else
            {
                transform.position += transform.forward * enginePower / 4 * Time.deltaTime;
                activePitch = Input.GetAxisRaw("Mouse Y") * pitchPower / 1.5f * Time.deltaTime;
                activeRoll = Input.GetAxisRaw("Horizontal") * rollPower / 1.5f * Time.deltaTime;
                activeYaw = Input.GetAxisRaw("Mouse X") * yawPower / 1.5f * Time.deltaTime;
                transform.Rotate(activePitch * pitchPower * Time.deltaTime,
                    activeYaw * yawPower * Time.deltaTime,
                    activeRoll * rollPower * Time.deltaTime, Space.Self);
            }
        }
    }
    private void Attack()
    {
        foreach (GameObject BulletSpawn in BulletsSpawner)
        {
            animationController.StartAttackAnimation();
            GameObject NewBullet = Instantiate(Bullet, BulletSpawn.transform.position, BulletSpawn.transform.rotation);
            NewBullet.GetComponent<BulletController>().IsPlayerTarget = false;
            NewBullet.GetComponent<BulletController>().Damage = damage;
            NewBullet.GetComponent<Rigidbody>().AddForce(transform.forward * 100000f);
            Destroy(NewBullet, 1.0f);
        }
    }
    private void LokingTarget()
    {
        RaycastHit hitInfo;
        if (Physics.Raycast(transform.position, transform.forward, out hitInfo, Mathf.Infinity, layer))
        {
            m_timeRemainingLock -= Time.deltaTime;
            if (m_timeRemainingLock <= 0)
            {
                ShootingText.SetActive(true);
                CanShootMissile = true;
                target = hitInfo.transform.gameObject.GetComponentInParent<EnemyController>();
            }
        }
        else
        {
            ShootingText.SetActive(false);
            m_timeRemainingLock = timeRemainingLock;
            CanShootMissile = false;
        }
    }
    
    private void ShootMissile(EnemyController target)
    {
        GameObject NewMissile = Instantiate(Missile,transform.position, transform.rotation);
        NewMissile.GetComponent<MissileController>().target = target;
        CanShootMissile = false;
    }
    public void TakeDamage(int damage)
    {
        
        Life -= damage;
        
        if (Life <= 50)
        {
            prefabList[0].SetActive(false);
            prefabList[1].SetActive(true);
        }
        if (Life <= 0)
        {
            die();
        }
    }
    private void ChangeScene(string NextScene)
    {
        SceneManager.LoadScene(NextScene);
    }
    
    private void die()
    {
        prefabList[1].SetActive(false);
        prefabList[0].SetActive(false);
        prefabList[2].SetActive(true);
        FX.SetActive(true);
        pitchPower = rollPower = yawPower = enginePower = 0;
        m_Rigidbody.useGravity = true;

    }

   /* private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.name);
        die();
    }*/
}
