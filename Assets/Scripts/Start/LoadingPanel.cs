using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingPanel : MonoBehaviour
{
    private CanvasGroup canvasGroup; // CanvasGroup을 사용하여 alpha 값으로 페이드 효과 주기

    private void Awake()
    {
        // CanvasGroup 참조
        canvasGroup = GetComponent<CanvasGroup>();

        // 시작 시 로딩 패널을 비활성화
        canvasGroup.alpha = 0f; // 처음에는 보이지 않도록 설정
        // gameObject.SetActive(false);

        Debug.Log("LoadingPanel Awake: Panel initialized and set to inactive.");
    }

    // 패널을 보여주는 함수
    public void ShowPanel()
    {
        Debug.Log("LoadingPanel: ShowPanel called.");

        gameObject.SetActive(true);

        // 로딩 패널을 서서히 보이게 하기 위해 alpha 값을 1로 변경
        StartCoroutine(FadeInPanel());

        // MainScene을 로드하는 시간 동안 로딩 패널이 나타나고, 그 후 바로 씬을 변경
        StartCoroutine(LoadMainScene());
    }

    // 로딩 패널을 서서히 보이게 하는 코루틴
    private IEnumerator FadeInPanel()
    {
        float duration = 1f; // 패널이 보이는 데 걸리는 시간 (1초)
        float startTime = Time.time;

        while (Time.time - startTime < duration)
        {
            float alphaValue = Mathf.Lerp(0f, 1f, (Time.time - startTime) / duration);
            canvasGroup.alpha = alphaValue;
            yield return null;
        }

        canvasGroup.alpha = 1f; // 완료 후 완전히 보이게 설정
    }

    // 로딩 패널을 서서히 숨기는 코루틴
    private IEnumerator FadeOutPanel()
    {
        float duration = 1f; // 패널이 사라지는 데 걸리는 시간 (1초)
        float startTime = Time.time;

        while (Time.time - startTime < duration)
        {
            float alphaValue = Mathf.Lerp(1f, 0f, (Time.time - startTime) / duration);
            canvasGroup.alpha = alphaValue;
            yield return null;
        }

        canvasGroup.alpha = 0f; // 완료 후 완전히 숨겨지도록 설정
        gameObject.SetActive(false);  // 패널을 비활성화시킨다.
    }

    // MainScene 로딩 코루틴
    private IEnumerator LoadMainScene()
    {
        float loadingTime = 3f; // 로딩 시간이 3초로 설정 (MainScene 로딩 시간과 맞춤)

        // 로딩 화면을 표시하고, 3초 동안 대기
        yield return new WaitForSeconds(loadingTime);

        // 씬을 비동기적으로 로드합니다.
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("MainScene");

        // 씬이 완전히 로드될 때까지 대기합니다.
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        // 로딩이 완료되면 패널을 서서히 숨깁니다.
        StartCoroutine(FadeOutPanel());
    }

    // 씬이 변경될 때마다 로딩 패널을 비활성화 시킴
    private void OnEnable()
    {
        gameObject.SetActive(true);
        canvasGroup.alpha = 0f;  // 씬 전환 시 초기화
    }

    private void OnDisable()
    {
        gameObject.SetActive(false);  // 로딩 패널이 비활성화될 때 숨김
    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("LoadingPanel Start: Component initialized.");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}