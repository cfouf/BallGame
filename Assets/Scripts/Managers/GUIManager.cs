using TMPro;
using UnityEngine;

namespace Managers
{
    public class GUIManager : MonoBehaviour
    {
        public int Health { get; private set; }
        public int Score { get; private set; }

        [SerializeField] private TextMeshProUGUI scoreText;
        [SerializeField] private GameObject[] healthIndicators;

        private void Start()
        {
            Health = healthIndicators.Length;
            Score = 0;
        }

        public void UpdateScore(int newScore)
        {
            Score = newScore;
            scoreText.text = newScore.ToString();
        }

        public void UpdateHealth(int newHealth)
        {
            Health = newHealth;
            for (var i = 0; i < healthIndicators.Length - Health; i++)
                healthIndicators[i].SetActive(false);
        }
    }
}