using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public int Life = 100;
    public float speed = 50f;
    public float AttackRange = 100f;
    public float MaxFollowingRange = 500f;
    public float MinFollowingRange = 200f;
    public GameObject Bullet;
    public GameObject[] BulletsSpawner;
    public GameObject[] prefabList;
    public int damage = 1;
    public float NextShot = 0.15f;
    public float FireDelay = 0.5f;
    private List<GameObject> TargetsNavigation;
    private GameObject m_targetPoint;
    private bool IsNavigateState = true;
    public float altitude;
    private float randPositionX = 0;
    private float randPositionZ = 0;
    private float attackSpeed = 50f;
    private bool isDead = false;
    public Rigidbody rb;


    int index = 0;
    private GameObject Player;

    void Start()
    {
        rb = GetComponentInChildren<Rigidbody>();
        TargetsNavigation = new List<GameObject>();

        for (int i = 0; i <= 5; i++)
        {
            randPositionX = Random.Range(-500, 500);
            randPositionZ = Random.Range(-500, 500);

            m_targetPoint = new GameObject("target" + i);
            m_targetPoint.transform.position = new Vector3(randPositionX, altitude, randPositionZ);
            TargetsNavigation.Add(m_targetPoint);
        }
        
    }
    void Update()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        if (!isDead)
        {
            if (!IsNavigateState)
            {
                AttackState();
            }
            else
            {
                NavigateState();
            }
        }
    }

    public void NavigateState()
    {
        GameObject target = TargetsNavigation[index];
        if (Vector3.Distance(target.transform.position, transform.position) > 5f)
        {
            transform.LookAt(target.transform);
            transform.position += transform.forward * speed * Time.deltaTime;  
        }
        else if (index < TargetsNavigation.Count - 1)
        {
            index++;
        }
        else
        { 
            index = 0;
        }
    }

   

    public void AttackState()
    {
        float DistanceBetweenPlayer = Vector3.Distance(Player.transform.position, transform.position);
       
        Vector3 direction = (Player.transform.position - transform.position);
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, direction.y, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        transform.LookAt(Player.transform);
        transform.position += transform.forward * attackSpeed * Time.deltaTime;
        
        if (DistanceBetweenPlayer < AttackRange && Time.time > NextShot)
        {
            /*attackSpeed = attackSpeed / 1.5f;*/
            Attack();
            NextShot = Time.time + FireDelay;
        }
        if (DistanceBetweenPlayer > MaxFollowingRange)
        {
            IsNavigateState = true;
        }
    }
    private void Attack()
    {
        foreach(GameObject BulletSpawn in BulletsSpawner)
        {
            GameObject NewBullet = Instantiate(Bullet, BulletSpawn.transform.position, BulletSpawn.transform.rotation);
            NewBullet.GetComponent<BulletController>().Damage = damage;
            NewBullet.GetComponent<Rigidbody>().AddForce(BulletSpawn.transform.forward * 100000f);
            Destroy(NewBullet, 1.0f);
        }
       
    }
    private void OnTriggerEnter(Collider other)
    {
        if (Player)
        {
            IsNavigateState = false;
        }
    }

    public void TakeDamage(int damage)
    {
        if (Life <= 0)
        {
            Die();
        }else if (Life <= 50 && Life > 0)
        {
            prefabList[0].SetActive(false);
            prefabList[1].SetActive(true);
        }
      
         Life -= damage;

        IsNavigateState = false;
    }
    void Die()
    {
        isDead = true;
        prefabList[1].SetActive(false);
        prefabList[2].SetActive(true);
        Destroy(gameObject,5f);
    }
}
