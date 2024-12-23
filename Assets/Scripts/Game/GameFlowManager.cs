using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using Unity.Scripts.AI;

namespace Unity.Scripts.Game
{
    public class GameFlowManager : MonoBehaviour
    {
        public GameObject player;
        public GameObject zombiePrefab;
        public GameObject vaccinePrefab;
        public Transform[] spawnPoints;

        public int initialZombieCount = 5;
        public int vaccinesToCollect = 5;
        public float waveInterval = 5f;

        private int currentWave = 1;
        private int zombiesRemaining;
        private int vaccinesCollected = 0;
        private int playerScore = 0;

        private bool isPlayerAlive = true;

        void Start()
        {
            if (player == null)
            {
                Debug.LogError("Player reference is missing!");
                return;
            }

            StartWave();
        }

        void Update()
        {
            if (!isPlayerAlive) return;

            // Check player health
            Health playerHealth = player.GetComponent<Health>();
            if (playerHealth.CurrentHealth <= 0)
            {
                PlayerDied();
            }
        }

        void StartWave()
        {
            Debug.Log("Starting Wave: " + currentWave);

            zombiesRemaining = currentWave * initialZombieCount;
            vaccinesCollected = 0;

            SpawnZombies(zombiesRemaining);
            SpawnVaccines(vaccinesToCollect);
        }

        void SpawnZombies(int count)
        {
            for (int i = 0; i < count; i++)
            {
                Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
                GameObject zombie = Instantiate(zombiePrefab, spawnPoint.position, Quaternion.identity);
                ZombieController zombieController = zombie.GetComponent<ZombieController>();

                // Increase difficulty for higher waves
                zombieController.attackDamage += currentWave * 2;
                zombieController.moveSpeed += currentWave * 0.2f;

                zombieController.OnZombieKilled += OnZombieKilled;
            }
        }

        void SpawnVaccines(int count)
        {
            for (int i = 0; i < count; i++)
            {
                Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
                GameObject vaccine = Instantiate(vaccinePrefab, spawnPoint.position, Quaternion.identity);
                Item vaccineItem = vaccine.GetComponent<Item>();

                vaccineItem.OnVaccineCollected += OnVaccineCollected;
            }
        }

        public void OnVaccineCollected()
        {
            vaccinesCollected++;
            Debug.Log("Vaccines Collected: " + vaccinesCollected);

            if (vaccinesCollected >= vaccinesToCollect && zombiesRemaining <= 0)
            {
                StartCoroutine(NextWave());
            }
        }

        void OnZombieKilled()
        {
            zombiesRemaining--;
            playerScore += 10;
            Debug.Log("Zombie Killed. Zombies Remaining: " + zombiesRemaining);
            Debug.Log("Player Score: " + playerScore);

            if (zombiesRemaining <= 0 && vaccinesCollected >= vaccinesToCollect)
            {
                StartCoroutine(NextWave());
            }
        }

        IEnumerator NextWave()
        {
            Debug.Log("Wave completed. Next wave starts in " + waveInterval + " seconds.");
            yield return new WaitForSeconds(waveInterval);

            currentWave++;
            StartWave();
        }

        void PlayerDied()
        {
            isPlayerAlive = false;
            Debug.Log("Player Died! Final Score: " + playerScore);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Restart the game
        }
    }
}