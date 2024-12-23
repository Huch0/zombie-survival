using UnityEngine;
using TMPro;
using UnityEngine.UI;
using MagicPigGames;

namespace Unity.Scripts.UI
{
    public class PlayerUIManager : MonoBehaviour
    {
        public TextMeshProUGUI scoreText;
        public TextMeshProUGUI waveText;
        public ProgressBar healthBar;
        public TextMeshProUGUI bulletsText;
        public TextMeshProUGUI vaccinesText;
        public TextMeshProUGUI zombiesText;

        public void UpdateHealth(float currentHealth, float maxHealth)
        {
            healthBar.SetProgress(currentHealth / maxHealth);
        }

        public void UpdateBullets(int remainingBullets, int totalBullets)
        {
            bulletsText.text = $"Bullets: {remainingBullets} / {totalBullets}";
        }

        public void UpdateVaccines(int remainingVaccines, int totalVaccines)
        {
            vaccinesText.text = $"Vaccines: {remainingVaccines}/{totalVaccines}";
        }

        public void UpdateZombies(int zombiesRemaining, int totalZombies)
        {
            zombiesText.text = $"Zombies: {zombiesRemaining}/{totalZombies}";
        }

        public void UpdateScore(int score)
        {
            scoreText.text = $"Score: {score}";
        }

        public void UpdateWave(int wave)
        {
            waveText.text = $"Wave: {wave}";
        }
    }
}