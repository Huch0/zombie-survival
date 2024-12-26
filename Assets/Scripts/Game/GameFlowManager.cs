using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using Unity.Scripts.AI;
using Unity.Scripts.UI;
using Unity.Scripts.Gameplay;

namespace Unity.Scripts.Game
{
    public class GameFlowManager : MonoBehaviour
    {
        public GameObject player;
        public PlayerCharacterController playerCharacterController;
        public GameObject zombiePrefab;

        public List<GameObject> currentZombies = new List<GameObject>();
        public GameObject vaccinePrefab;
        public List<GameObject> currentVaccines = new List<GameObject>();

        public Transform[] zombieSpawnPoints;
        public Transform[] vaccineSpawnPoints;

        public PlayerUIManager playerUIManager;

        public int initialZombieCount = 5;
        public int vaccinesToCollect = 5;
        public float waveInterval = 5f;

        private int currentWave = 1;
        private int zombiesRemaining;
        private int zombiesToKill;
        private int vaccinesCollected = 0;
        private int playerScore = 0;
        private int maxWave = 2;

        private bool isPlayerAlive = true;

        void Start()
        {
            Debug.Log($"GameFlowManager Initialized: maxWave = {maxWave}");
            if (player == null)
            {
                Debug.LogError("Player reference is missing!");
                return;
            }

            playerCharacterController = player.GetComponent<PlayerCharacterController>();

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

            zombiesToKill = currentWave * initialZombieCount;
            vaccinesCollected = 0;

            SpawnZombies(zombiesToKill);
            SpawnVaccines(vaccinesToCollect);

            zombiesRemaining = zombiesToKill;

            playerUIManager.UpdateWave(currentWave);
            playerUIManager.UpdateZombies(zombiesRemaining, zombiesToKill);
            playerUIManager.UpdateVaccines(vaccinesCollected, vaccinesToCollect);
        }

        void SpawnZombies(int count)
        {
            for (int i = 0; i < count; i++)
            {
                Transform spawnPoint = zombieSpawnPoints[Random.Range(0, zombieSpawnPoints.Length)];
                GameObject zombie = Instantiate(zombiePrefab, spawnPoint.position, Quaternion.identity);
                ZombieController zombieController = zombie.GetComponent<ZombieController>();

                // Increase difficulty for higher waves
                zombieController.attackDamage += currentWave * 2;
                zombieController.moveSpeed += currentWave * 0.2f;

                zombieController.OnZombieKilled += OnZombieKilled;

                currentZombies.Add(zombie);
            }
        }

        void SpawnVaccines(int count)
        {
            for (int i = 0; i < count; i++)
            {
                Transform spawnPoint = vaccineSpawnPoints[Random.Range(0, vaccineSpawnPoints.Length)];
                GameObject vaccine = Instantiate(vaccinePrefab, spawnPoint.position, Quaternion.identity);
                Item vaccineItem = vaccine.GetComponent<Item>();

                vaccineItem.OnVaccineCollected += OnVaccineCollected;

                currentVaccines.Add(vaccine);
            }
        }

        public void OnVaccineCollected()
        {
            vaccinesCollected++;
            Debug.Log("Vaccines Collected: " + vaccinesCollected);

            playerUIManager.UpdateVaccines(vaccinesCollected, vaccinesToCollect);

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

            playerUIManager.UpdateZombies(zombiesRemaining, zombiesToKill);
            playerUIManager.UpdateScore(playerScore);

            if (zombiesRemaining <= 0 && vaccinesCollected >= vaccinesToCollect)
            {
                StartCoroutine(NextWave());
            }
        }

        IEnumerator NextWave()
        {
            if (currentWave >= maxWave)
            {
                Debug.Log("Maximum wave reached! Ending the game.");
                EndGame(); // 게임 종료 로직
                yield break; // 코루틴 종료
            }

            Debug.Log("Wave completed. Next wave starts in " + waveInterval + " seconds.");
            yield return new WaitForSeconds(waveInterval);

            currentWave++;
            ClearWave();
            StartWave();
        }

        void ClearWave()
        {
            foreach (GameObject zombie in currentZombies)
            {
                Destroy(zombie);
            }

            foreach (GameObject vaccine in currentVaccines)
            {
                Destroy(vaccine);
            }
        }

        void EndGame()
        {
            // 게임 종료 로직 구현
            Debug.Log("Game Over! Final Wave: " + currentWave);
            // 마우스 커서 활성화
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            playerUIManager.SaveFinalStats(playerScore, currentWave);
            SceneManager.LoadScene("GameClearScene"); // 엔드 씬으로 이동 (필요 시)
        }

        public void OnShoot()
        {
            int ammoCapacity = playerCharacterController.gun.ammoCapacity;
            int remainingAmmo = playerCharacterController.gun.currentAmmo;

            playerUIManager.UpdateBullets(remainingAmmo, ammoCapacity);
        }

        public void OnPlayerHit()
        {
            playerUIManager.UpdateHealth(playerCharacterController.health.CurrentHealth, playerCharacterController.health.MaxHealth);
        }

        void PlayerDied()
        {
            isPlayerAlive = false;
            Debug.Log("Player Died! Final Score: " + playerScore);

            // 마우스 커서 활성화
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            
            // 게임 오버 로직
            SceneManager.LoadScene("GameOverScene"); // 패배 씬으로 이동
        }
    }
}