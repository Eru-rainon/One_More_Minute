

using UnityEngine;
using UnityEngine.AI;

public class enemyController : MonoBehaviour
{
    public float LookRadius = 10f;

    public float attackDamage;
    [SerializeField] private float AttackCooldown = 1f;
    GameObject Player;
    public float distanceToTarget;
    public Transform Target;

     public NavMeshAgent agent;
     public float walkspeed = 2.5f;
     public float runspeed = 5f;

     public float bossWalkSpeed = 5f;
     public float bossRunSpeed = 10f;
    float lastAttackTime =0f;
    
    public Animator enemyAnimator;

    enemy enemy;

    [SerializeField] private float wanderRadius = 5f;
    [SerializeField] private float WanderInterval = 3f;
    [SerializeField] private float lastWanderTime = 0f;
    void Start()
    {
        enemy = GetComponent<enemy>();
        Player = playerFinder.instance.player;
        Target = Player.transform;
        agent = GetComponent<NavMeshAgent>();

    }

    // Update is called once per frame
    void Update()
    {
        if (Target == null)
        {
            enemyAnimator.SetBool("PlayerInRange", false);
            agent.ResetPath();  // Stop movement
            return;
        }
        float distanceToTarget = Vector3.Distance(Target.position,transform.position);

        if(distanceToTarget <= LookRadius){
            
            enemyAnimator.SetBool("PlayerInRange",true);
            agent.speed = enemy.isBossEnemy?bossRunSpeed:runspeed;
            agent.SetDestination(Target.position);
            RotateTowardsTarget();
        }else{

            enemyAnimator.SetBool("PlayerInRange",false);
            agent.speed = enemy.isBossEnemy?bossWalkSpeed:walkspeed;
            Wander();
        }
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance + 0.2f){
                enemyAnimator.SetBool("Walking",false);
                enemyAnimator.SetBool("PlayerInRange",false);
            }

        if(distanceToTarget <= agent.stoppingDistance ){
           
            if(Time.time >= lastAttackTime + AttackCooldown){
                Debug.Log("Boss should attack now!");
                lastAttackTime = Time.time;
                Attack();
                RotateTowardsTarget();
                
            }

            Vector3 direction = (Target.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x,0,direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation,lookRotation,Time.deltaTime*5f);
        }
        if (enemyAnimator.GetCurrentAnimatorStateInfo(0).IsName("Idle") && distanceToTarget <= agent.stoppingDistance)
        {
            enemyAnimator.SetTrigger("Attack");
        }

    }

    public int attackCounter=0;
    void Attack(){

        if(enemy.isBossEnemy)
        {
            Debug.Log("Boss is Attacking!"); 
            if(attackCounter<2){
                enemyAnimator.SetTrigger("Attack");
                
            }
            else
            {
                enemyAnimator.SetTrigger("sAttack");
            }
        }
        else
        {
            enemyAnimator.SetTrigger("Attack");  
        }
        
    }
    public void DealDamage()
    {
        if(Target == null) return;
        float distanceToTarget = Vector3.Distance(Target.position, transform.position);
        
        attackCounter++;
        if(attackCounter>3){
            attackCounter=0;
        }
        
        PlayerHealthManager healthManager = Target.GetComponent<PlayerHealthManager>();
        
        if (healthManager != null && distanceToTarget <= 5f)
        {
            healthManager.applyDamage(attackDamage);
        }
    }

    private void Wander(){
        if(Time.time >= lastWanderTime+WanderInterval){
            Vector3 wanderDestination = getRandomWanderPoint();
            agent.SetDestination(wanderDestination);
            enemyAnimator.SetBool("Walking",true);
            lastWanderTime = Time.time;

            
        }
    }
    private Vector3 getRandomWanderPoint(){
        Vector3 randomDirection = Random.insideUnitSphere*wanderRadius;
        randomDirection += transform.position;
        NavMeshHit hit;
        if(NavMesh.SamplePosition(randomDirection,out hit, wanderRadius, NavMesh.AllAreas)){
            return hit.position;
        }else{
            return transform.position;
        }
    }

    void RotateTowardsTarget()
    {
        if (Target == null) return;

        Vector3 direction = (Target.position - transform.position).normalized;
        direction.y = 0; // Keep the rotation flat on the ground

        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        }
    }
    

    
}
