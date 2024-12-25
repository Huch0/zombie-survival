using UnityEngine;
using TMPro;
using UnityEngine.UI;
using MagicPigGames;
using Unity.Scripts.AI;

namespace Unity.Scripts.UI
{
    public class PauseMenuUI : MonoBehaviour
    {
        public GameObject pauseMenuPanel; // Inspector에서 Pause 메뉴 패널 연결
        private bool isPaused = false;   // 일시정지 상태를 확인

        void Update()
        {
            // ESC 키 입력으로 일시정지 토글
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Debug.Log("ESC key pressed");
                TogglePause();
            }
        }

        public void TogglePause()
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }

        void PauseGame()
        {
            ZombieController[] zombies = FindObjectsOfType<ZombieController>();
            Debug.Log("Pausing " + zombies.Length + " zombies.");

            foreach (ZombieController zombie in zombies)
            {
                UnityEngine.AI.NavMeshAgent agent = zombie.GetComponent<UnityEngine.AI.NavMeshAgent>();

                if (agent != null && agent.isOnNavMesh) // NavMeshAgent가 활성화되고 NavMesh에 배치된 경우
                {
                    zombie.PauseZombie(); // 정상적으로 좀비를 멈춤
                }
                else
                {
                    Debug.LogWarning($"{zombie.gameObject.name} is not on a valid NavMesh or has no NavMeshAgent.");
                }
            }

            Time.timeScale = 0f; // 게임 멈춤
            Time.fixedDeltaTime = 0.02f * Time.timeScale; // fixedDeltaTime 조정
            isPaused = true;
            pauseMenuPanel.SetActive(true); // Pause 메뉴 활성화
            // AudioListener.pause = true; // 소리도 멈추기 (선택 사항)
        }


        void ResumeGame()
        {
            // 모든 좀비 재개
            ZombieController[] zombies = FindObjectsOfType<ZombieController>();
            Debug.Log("Resuming " + zombies.Length + " zombies.");
            
            foreach (ZombieController zombie in zombies)
            {
                UnityEngine.AI.NavMeshAgent agent = zombie.GetComponent<UnityEngine.AI.NavMeshAgent>();

                if (agent != null && agent.isOnNavMesh) // NavMeshAgent가 활성화되고 NavMesh에 배치된 경우
                {
                    zombie.ResumeZombie(); // 정상적으로 좀비를 재개
                }
                else
                {
                    Debug.LogWarning($"{zombie.gameObject.name} is not on a valid NavMesh or has no NavMeshAgent.");
                }
            }

            Time.timeScale = 1f; // 게임 재개
            Time.fixedDeltaTime = 0.02f * Time.timeScale; // fixedDeltaTime 복원
            isPaused = false;
            pauseMenuPanel.SetActive(false); // Pause 메뉴 비활성화
            // AudioListener.pause = false; // 소리 재개 (선택 사항)
        } 
    }
}