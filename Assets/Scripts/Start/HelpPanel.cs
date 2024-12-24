using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class HelpPanel : MonoBehaviour
{
    private RectTransform panel;

    private void Awake()
    {
        // HelpPanel 참조
        panel = GetComponent<RectTransform>();

        // 시작 시 패널을 비활성화 상태로 설정
        panel.localScale = Vector3.zero; // 크기를 0으로 설정
        gameObject.SetActive(false);

        Debug.Log("HelpPanel Awake: Panel initialized and set to inactive.");
    }

    // 패널을 보여주는 함수
    public void ShowPanel()
    {
        Debug.Log("ShowPanel called.");

        gameObject.SetActive(true);
        Debug.Log("HelpPanel is now active.");

        // DOTween을 사용해 패널을 부드럽게 크기를 키운다 (애니메이션 효과)
        panel.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack).OnStart(() =>
        {
            Debug.Log("Animation started: Panel scaling up.");
        }).OnComplete(() =>
        {
            Debug.Log("Animation complete: Panel fully scaled.");
        });
    }

    // 패널을 닫는 함수
    public void HidePanel()
    {
        Debug.Log("HidePanel called.");

        // DOTween을 사용해 패널을 부드럽게 크기를 줄인다 (애니메이션 효과)
        panel.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InBack).OnStart(() =>
        {
            Debug.Log("Animation started: Panel scaling down.");
        }).OnComplete(() =>
        {
            Debug.Log("Animation complete: Panel fully scaled down.");
            gameObject.SetActive(false);
        });
    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("HelpPanel Start: Component initialized.");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}