using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;  // TextMeshPro 사용

public class StartSceneController : MonoBehaviour
{
    // UI 요소
    public GameObject loadingPage;  // 로딩 화면 GameObject
    public TextMeshProUGUI loadingText;  // 로딩 화면의 TextMeshProUGUI 텍스트
    public GameObject mainMenu;  // 메인 메뉴 GameObject

    void Start()
    {
        // 초기 상태에서 로딩 화면 비활성화
        loadingPage.SetActive(false);
    }

    // Start 버튼 클릭 시 호출되는 함수
    public void OnStartButtonClicked()
    {
        // 메인 메뉴 숨기기
        mainMenu.SetActive(false);
        
        // 로딩 화면 활성화
        loadingPage.SetActive(true);

        // 로딩 텍스트 표시
        loadingText.text = "In game...";  // "In game..." 텍스트로 설정

        // 3초 후에 MainScene으로 이동
        Invoke("LoadMainScene", 3f);  // 3초 후 LoadMainScene 함수 호출
    }

    // 로딩이 끝난 후 MainScene으로 전환
    void LoadMainScene()
    {
        // MainScene으로 씬 전환
        SceneManager.LoadScene("MainScene");  // MainScene 씬 이름으로 변경
    }
}
