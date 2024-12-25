using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartButton : MonoBehaviour
{
    // UI 요소
    public GameObject loadingPanel;
    public GameObject mainMenu;  // 메인 메뉴 GameObject

    public LoadingPanel loadingPanelScript;  // 로딩 화면 GameObject

    void Start()
    {
        // LoadingPanel 스크립트를 찾음
        loadingPanelScript = loadingPanel.GetComponent<LoadingPanel>();

        Button button = GetComponent<Button>();

        // Button 컴포넌트가 있는지 확인
        if (button != null)
        {
            // OnClick() 이벤트에 OnButtonClick() 메서드를 추가
            button.onClick.AddListener(OnButtonClick);
            Debug.Log("StartButton Start: Component initialized.");
        }
        else
        {
            Debug.LogError("Button 컴포넌트가 이 오브젝트에 없습니다.");
        }
    }

    // Start 버튼 클릭 시 호출되는 함수
    public void OnButtonClick()
    {
        Debug.Log("Start button clicked!");

       // 메인 메뉴 숨기기
        mainMenu.SetActive(false);
        
        // 로딩 화면 활성화 및 표시
        loadingPanelScript.ShowPanel();
    }
}
