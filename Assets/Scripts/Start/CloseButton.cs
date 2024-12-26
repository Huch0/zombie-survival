using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CloseButton : MonoBehaviour
{
    public HelpPanel helpPanel;

    // Start is called before the first frame update
    void Start()
    {
        Button button = GetComponent<Button>();

        // Button 컴포넌트가 있는지 확인
        if (button != null)
        {
            // OnClick() 이벤트에 OnButtonClick() 메서드를 추가
            button.onClick.AddListener(OnButtonClick);
            Debug.Log("HelpButton Start: Component initialized.");
        }
        else
        {
            Debug.LogError("Button 컴포넌트가 이 오브젝트에 없습니다.");
        }
    }

    public void OnButtonClick()
    {
        Debug.Log("OnButtonClick called.");

        if (helpPanel != null)
        {
            Debug.Log("HelpPanel found. Calling Hide Panel.");
            helpPanel.HidePanel();
        }
        else
        {
            Debug.LogError("HelpPanel is not assigned!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
