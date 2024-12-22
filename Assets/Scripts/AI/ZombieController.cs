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
        public float DeathDuration = 0f;

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

        // Update is called once per frame
        void Update()
        {
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

                animator.SetTrigger("Attack");

                // if (target.TryGetComponent(out Health targetHealth))
                // {
                //     targetHealth.TakeDamage(attackDamage);
                // }
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                Debug.Log("Zombie collided with Player!");
                // Add logic here for collision handling (e.g., reducing player health).
            }
        }

        void OnDamaged(float damage, GameObject damageSource)
        {
            Debug.Log("Zombie Damaged: " + damage);
        }

        void OnDie()
        {
            animator.SetTrigger("Dead");

            navMeshAgent.enabled = false;

            Destroy(gameObject, DeathDuration);
        }
    }
}
