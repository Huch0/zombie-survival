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
                zombie.PauseZombie();
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
            foreach (ZombieController zombie in zombies)
            {
                zombie.ResumeZombie();
            }

            Time.timeScale = 1f; // 게임 재개
            Time.fixedDeltaTime = 0.02f * Time.timeScale; // fixedDeltaTime 복원
            isPaused = false;
            pauseMenuPanel.SetActive(false); // Pause 메뉴 비활성화
            // AudioListener.pause = false; // 소리 재개 (선택 사항)
        }

        // UI 버튼을 통해 Resume 호출
        public void OnResumeButtonPressed()
        {
            ResumeGame();
        }

        // UI 버튼을 통해 Quit 호출
        public void OnQuitButtonPressed()
        {
            // 메인 메뉴로 이동 (씬 이름은 프로젝트에 따라 수정)
            UnityEngine.SceneManagement.SceneManager.LoadScene("StartScene");
        }
    }
}