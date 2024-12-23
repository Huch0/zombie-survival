using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

using Unity.Scripts.Game;

namespace Unity.Scripts.AI
{
    [RequireComponent(typeof(Health), typeof(NavMeshAgent), typeof(CapsuleCollider))]
    public class ZombieController : MonoBehaviour
    {
        public float moveSpeed = 1f;
        public float rotationSpeed = 100f;
        public float attackRange = 2f;
        public float attackRate = 2f;
        public float attackDamage = 10f;

        private Transform target;
        private NavMeshAgent navMeshAgent;
        private Health health;
        private Animator animator;
        private float lastAttackTime;

        [Tooltip("Delay after death where the GameObject is destroyed (to allow for animation)")]
        public float DeathDuration = 10f;

        // Start is called before the first frame update
        void Start()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
            animator = GetComponent<Animator>();
            health = GetComponent<Health>();

            health.OnDie += OnDie;
            health.OnDamaged += OnDamaged;

            // Set up the CapsuleCollider for the zombie
            CapsuleCollider collider = GetComponent<CapsuleCollider>();
            // collider.isTrigger = false; // Ensure the collider isn't a trigger for physical collisions
            // collider.center = new Vector3(0, 1, 0); // Adjust to match your zombie model
            // collider.height = 2f; // Adjust based on your zombie model's height
            // collider.radius = 0.5f; // Adjust to fit the zombie's body

            Debug.Log("ZombieController initialized.");
        }

        private bool isDead = false;
        // Update is called once per frame
        void Update()
        {
            if (isDead) return;

            FindClosestTarget();

            if (target == null) return;

            float distanceToTarget = Vector3.Distance(transform.position, target.position);

            if (distanceToTarget <= attackRange)
            {
                Attack();
            }
            else
            {
                MoveTowardsTarget();
            }
        }

        private void FindClosestTarget()
        {
            GameObject[] potentialTargets = GameObject.FindGameObjectsWithTag("Player");

            float closestDistance = Mathf.Infinity;
            Transform closestTarget = null;

            foreach (GameObject obj in potentialTargets)
            {
                float distance = Vector3.Distance(transform.position, obj.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestTarget = obj.transform;
                }
            }

            target = closestTarget;
        }

        private void MoveTowardsTarget()
        {
            if (target == null) return;

            if (navMeshAgent.isOnNavMesh)
            {
                navMeshAgent.SetDestination(target.position);

                if (navMeshAgent.pathPending || navMeshAgent.remainingDistance > attackRange)
                {
                    animator.SetFloat("MoveSpeed", navMeshAgent.velocity.magnitude);
                    animator.SetBool("Walk", true);
                }
                else
                {
                    animator.SetBool("Walk", false);
                }
            }
        }

        private void Attack()
        {
            animator.SetBool("Walk", false);

            if (Time.time - lastAttackTime >= 1f / attackRate)
            {
                lastAttackTime = Time.time;

                // Trigger the attack animation
                animator.SetTrigger("Attack");

                // Start a coroutine to apply damage after the animation ends
                StartCoroutine(ApplyDamageAfterDelay(0.3f)); // Adjust delay based on animation timing
            }
        }

        private IEnumerator ApplyDamageAfterDelay(float delay)
        {
            yield return new WaitForSeconds(delay);

            // Apply damage if the target is still in range
            if (target != null && Vector3.Distance(transform.position, target.position) <= attackRange)
            {
                Health targetHealth = target.GetComponent<Health>();
                if (targetHealth != null)
                {
                    targetHealth.TakeDamage(attackDamage, gameObject);
                }

                Debug.Log("Zombie applied damage to the target!");
            }
        }



        void OnDamaged(float damage, GameObject damageSource)
        {
            Debug.Log("Zombie Damaged: " + damage);
        }
        public event System.Action OnZombieKilled; // Event to notify when a zombie is killed.

        void OnDie()
        {
            isDead = true;
            Debug.Log("Zombie Died! isDead = " + isDead);

            // Trigger the death animation
            animator.SetTrigger("Dead");

            // Stop the NavMeshAgent to prevent further movement
            navMeshAgent.enabled = false;

            // Notify listeners
            if (OnZombieKilled != null)
            {
                OnZombieKilled.Invoke();
            }

            // Start the coroutine to wait for the death animation to finish
            StartCoroutine(WaitForDeathAnimation());
        }


        private IEnumerator WaitForDeathAnimation()
        {
            // Get the length of the death animation
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            while (!stateInfo.IsName("Dead"))
            {
                stateInfo = animator.GetCurrentAnimatorStateInfo(0);
                yield return null;
            }

            // Wait for the animation to complete
            yield return new WaitForSeconds(stateInfo.length);

            // Disable the zombie game object after the animation
        }
    }
}
