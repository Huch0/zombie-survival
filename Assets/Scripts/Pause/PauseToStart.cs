using UnityEngine;
using TMPro;
using UnityEngine.UI;
using MagicPigGames;
using Unity.Scripts.AI;
using UnityEngine.SceneManagement;

public class PauseToStart : MonoBehaviour
{
    public Button quitButton; // Quit 버튼을 Inspector에서 연결

    void Start()
    {
        // Quit 버튼의 OnClick 이벤트에 메서드 연결
        if (quitButton != null)
        {
            quitButton.onClick.AddListener(OnQuitButtonPressed);
        }
        else
        {
            Debug.LogError("Quit Button is not assigned in the Inspector!");
        }
    }

    public void OnQuitButtonPressed()
    {
        Debug.Log("Quit Button Pressed"); // 디버깅용 메시지
        Time.timeScale = 1f; // 게임 재개
        Time.fixedDeltaTime = 0.02f * Time.timeScale; // fixedDeltaTime 복원
        SceneManager.LoadScene("StartScene"); // "StartScene"으로 이동
    }
}
