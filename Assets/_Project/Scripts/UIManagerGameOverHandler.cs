using Platformer;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Game Over")]
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private Button restartButton;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI gameOverScoreText;
    
    [Header("References")]
    [SerializeField] private PlayerController player;
    [SerializeField] private Health playerHealth;
    [SerializeField] private EnemySpawnManager spawnManager; // Add this reference

    private static int currentScore = 0;
    
    private void Start()
    {
        gameOverPanel.SetActive(false);
        UpdateScoreText();
        restartButton.onClick.AddListener(RestartGame);
    }

    private void OnEnable()
    {
        Enemy.OnEnemyDied += OnEnemyKilled;
        playerHealth.onDeath.AddListener(ShowGameOver);
    }

    private void OnDisable()
    {
        Enemy.OnEnemyDied -= OnEnemyKilled;
        playerHealth.onDeath.RemoveListener(ShowGameOver);
    }

    private void OnEnemyKilled()
    {
        currentScore += 100; // Adjust points per kill as needed
        UpdateScoreText();
    }

    private void UpdateScoreText()
    {
        scoreText.text = $"Score: {currentScore}";
    }

    private void ShowGameOver()
    {
        gameOverPanel.SetActive(true);
        gameOverScoreText.text = $"Final Score: {currentScore}";
    }

    public void RestartGame()
    {
        // Reset score
        currentScore = 0;
        UpdateScoreText();

        // Hide game over panel
        gameOverPanel.SetActive(false);

        // Reset enemy spawner
        if (spawnManager != null)
        {
            spawnManager.ResetSpawner();
        }

        // Reset player
        if (player != null && playerHealth != null)
        {
            player.ResetPlayer();
            playerHealth.ResetHealth();
        }
    }

}