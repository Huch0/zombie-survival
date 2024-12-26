using UnityEngine;
using TMPro;
using UnityEngine.UI;
using MagicPigGames;

namespace Unity.Scripts.UI
{
    public class EndUIManager : MonoBehaviour
    {
        public TextMeshProUGUI scoreText; // 점수 텍스트
        public TextMeshProUGUI waveText;  // 웨이브 텍스트
        
        void Start()
        {
            // PlayerPrefs에서 데이터 로드
            int finalScore = PlayerPrefs.GetInt("FinalScore", 0);
            int finalWave = PlayerPrefs.GetInt("FinalWave", 0);

            // UI 업데이트
            UpdateScore(finalScore);
            UpdateWave(finalWave);
        }

        public void UpdateScore(int score)
        {
            scoreText.text = $"Final Score: {score}";
        }

        public void UpdateWave(int wave)
        {
            waveText.text = $"Final Wave: {wave}";
        }
    }
}