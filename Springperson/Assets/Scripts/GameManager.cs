using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;  

public class GameManager : Singleton<GameManager>
{
    public int playerScore;

    //This number is used to stop more than x amount of enemies being on the map at once
    public int totalEnemy;
    public int currentEnemy;

    private int _playerHealth = 3;
    private bool _playerCanBeDamaged = true;

    //Determines the player invulnerability time after taking damage
    [SerializeField] int playerNoDamageTime;
    [SerializeField] DamageIndicator damageIndicator;
    [SerializeField] PlayerScoreDisplay scoreDisplay;
    [SerializeField] Image health1, health2, health3;
    
    

    [SerializeField] int maxHearts;
    List<Image> _hearts = new List<Image>();
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] Image heartImage;

    [SerializeField] GameObject healthDisplay;
    [SerializeField] GameObject restartScreen;
    [SerializeField] TextMeshProUGUI endScoreText;
    [SerializeField] TextMeshProUGUI highscoreText;

    [SerializeField] CameraController cameraController;

    bool isInvincible;
    bool gameOver;

    public bool IsInvincible { get => isInvincible; set => isInvincible = value; }
    public bool GameOver { get => gameOver; set => gameOver = value; }
    public int PlayerHealth { get => _playerHealth; set => _playerHealth = value; }

    void Start()
    {
        //isInvincible = true;
        gameOver = false;
    }
    public void AddToScoreTotal(int i)
    {
        playerScore += i;
        SoundBoard.instance.ScoreUp();
        scoreDisplay.ShowScore(i);
        scoreText.text = "Score: " + playerScore.ToString();
    }

    public void PlayerLoseHealth()
    {
        if (_playerCanBeDamaged)
        {
            StartCoroutine(PlayerNoDamagePeriod());
            SoundBoard.instance.DamageSound();
            _playerHealth--;
            damageIndicator.Damage();
            switch (_playerHealth)
            {
                case 0:
                    health1.enabled = false;
                    health2.enabled = false;
                    health3.enabled = false;
                    EndLevel();
                    break;
                case 1:
                    health1.enabled = true;
                    health2.enabled = false;
                    health3.enabled = false;
                    break;

                case 2:
                    health1.enabled = true;
                    health2.enabled = true;
                    health3.enabled = false;
                    break;
                case 3:
                    health1.enabled = true;
                    health2.enabled = true;
                    health3.enabled = true;
                    break;
                default:
                    Debug.Log("The player health number is outside its range, you fucked up");
                    EndLevel();
                    break;
            }
        }

    }

    //For when we add powerups
    public void PlayerAddHealth()
    {
        _playerHealth++;
        damageIndicator.GainHealth();
        SoundBoard.instance.HealSound();
        switch (_playerHealth)
        {
            case 1:
                health1.enabled = true;
                health2.enabled = false;
                health3.enabled = false;
                break;

            case 2:
                health1.enabled = true;
                health2.enabled = true;
                health3.enabled = false;
                break;
            case 3:
                health1.enabled = true;
                health2.enabled = true;
                health3.enabled = true;
                break;
            default:
                Debug.Log("The player health number is outside its range, you fucked up");
                break;
        }
    }

    public void EndLevel()
    {
        SoundBoard.instance.DeathSound();
        gameOver = true;
        restartScreen.SetActive(true);
        endScoreText.text = playerScore.ToString();
        cameraController.DisableCameraControl();
        Time.timeScale = 0f;

        //For setting the highscore, uses Playerprefs
        if (PlayerPrefs.HasKey("Highscore"))
        {
            if(PlayerPrefs.GetInt("Highscore", 0) < playerScore)
            {
                PlayerPrefs.SetInt("Highscore", playerScore);
            }
        }
        else
        {
            PlayerPrefs.SetInt("Highscore", playerScore);
        }

        highscoreText.text = PlayerPrefs.GetInt("Highscore", 0).ToString();
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    IEnumerator PlayerNoDamagePeriod()
    {
        _playerCanBeDamaged = false;
        yield return new WaitForSeconds(playerNoDamageTime);
        _playerCanBeDamaged = true;
    }
}
