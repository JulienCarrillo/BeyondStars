using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyControllerFPS : MonoBehaviour
{
  
    //base 
    public float Life = 50f;
    public float lookRadius = 100f;
    public GameObject Bullet;
    public GameObject BulletSpawn;
    public int damage = 1;
    public LayerMask targetLayer;

    //IA
    Transform target;
    NavMeshAgent agent;
    //patrolling;
    public Vector3 walkpoint;
    bool walkPointSet =true;
    public float walkPointRange;
    float distanceToWalkPoint;
    private float currentDelay = 5;
    public float timeRemainingAnimation = 5;
    //Attacking 
    public float timeBetwennAttacks;
    bool CoolDowned = false;
    public float timeRemainingGetHit = 2;
    private float currentDelayHit = 5;

    //States
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;
    //Animator
    FPSEnemyAnimator EnemyAnimator;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        EnemyAnimator = GetComponentInChildren<FPSEnemyAnimator>();

        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);
        walkpoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);
        walkPointSet = true;

    }
    void Update()
    {

        target = GameObject.FindGameObjectWithTag("PlayerFPS").transform;

        playerInSightRange = Physics.CheckSphere(transform.position, sightRange,targetLayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange,targetLayer);


        if (!playerInSightRange && !playerInAttackRange) Patrolling();
        if (playerInSightRange && !playerInAttackRange) ChasePlayer();
        if (playerInSightRange && playerInAttackRange) AttackPlayer();
    }

    private void Patrolling()
    {

        EnemyAnimator.StopShooting();
        EnemyAnimator.StopRunning();
        EnemyAnimator.StartWalking();
            
        if(walkPointSet)
            agent.SetDestination(walkpoint);
        

        distanceToWalkPoint = Vector3.Distance(walkpoint,transform.position);
      /*  Debug.Log(distanceToWalkPoint);*/
        if (distanceToWalkPoint < 1f)
        {
            walkPointSet = false;
                
        }
    }

    private void SearchWalkPoint()
    {
        agent.SetDestination(transform.position);
        currentDelay -= Time.deltaTime;
        EnemyAnimator.StopWalking();

        if (currentDelay <= 0)
        {
            float randomZ = Random.Range(-walkPointRange, walkPointRange);
            float randomX = Random.Range(-walkPointRange, walkPointRange);
            walkpoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);
           
            walkPointSet = true;
            currentDelay = timeRemainingAnimation;
        }
    }

    private void ChasePlayer()
    {
        EnemyAnimator.StartWalking();
        EnemyAnimator.StopShooting();
        EnemyAnimator.StartRunning();
        Debug.Log("Chase");

        agent.SetDestination(target.position);
        transform.LookAt(target);
    }
    private void AttackPlayer()
    {

        EnemyAnimator.StartShooting();
        Debug.Log("Attack");

        //immobilise enemy pour shoot;
        agent.SetDestination(transform.position);
        transform.LookAt(target);
        if (!CoolDowned)
        {
            //Launch attack 
            Debug.Log("AttackPlayer");
            GameObject NewBullet = Instantiate(Bullet, BulletSpawn.transform.position, BulletSpawn.transform.rotation);
            NewBullet.GetComponent<BulletController>().Damage = damage;
            NewBullet.GetComponent<Rigidbody>().AddForce(BulletSpawn.transform.forward * 1000f);
            Destroy(NewBullet, 1.0f);
            CoolDowned = true;
            Invoke(nameof(CoolDown), timeBetwennAttacks);
        }
    }
    private void CoolDown()
    {
        CoolDowned = false;
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
    public IEnumerator TakeDamage(float amount)
    {
        EnemyAnimator.StartGetHit();
        agent.SetDestination(target.position);
        Life -= amount;

        if(Life <= 0f)
        {
            Die();
        }
        yield return new WaitForSeconds(1);
        EnemyAnimator.StopGetHit();
    }

    private void Die()
    {
        EnemyAnimator.Dying();
        Destroy(gameObject,3f);
    }
}
