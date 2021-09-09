using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    [SerializeField] PlayerScoreDisplay scoreDisplay;
    [SerializeField] GameObject health1, health2, health3;
    [SerializeField] TextMeshProUGUI scoreText;

    [SerializeField] GameObject restartScreen;
    [SerializeField] TextMeshProUGUI endScoreText;
    [SerializeField] TextMeshProUGUI highscoreText;

    [SerializeField] CameraController cameraController;

    bool isInvincible;

    public bool IsInvincible { get => isInvincible; set => isInvincible = value; }

    void Start()
    {
        isInvincible = true;
    }
    public void AddToScoreTotal(int i)
    {
        playerScore += i;
        scoreDisplay.ShowScore(i);
        scoreText.text = "Score: " + playerScore.ToString();
    }

    public void PlayerLoseHealth()
    {
        if (_playerCanBeDamaged)
        {
            StartCoroutine(PlayerNoDamagePeriod());

            _playerHealth--;

            switch (_playerHealth)
            {
                case 0:
                    health1.SetActive(false);
                    health2.SetActive(false);
                    health3.SetActive(false);
                    EndLevel();
                    break;
                case 1:
                    health1.SetActive(true);
                    health2.SetActive(false);
                    health3.SetActive(false);
                    break;

                case 2:
                    health1.SetActive(true);
                    health2.SetActive(true);
                    health3.SetActive(false);
                    break;
                case 3:
                    health1.SetActive(true);
                    health2.SetActive(true);
                    health3.SetActive(true);
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

        switch (_playerHealth)
        {
            case 1:
                health1.SetActive(true);
                health2.SetActive(false);
                health3.SetActive(false);
                break;

            case 2:
                health1.SetActive(true);
                health2.SetActive(true);
                health3.SetActive(false);
                break;
            case 3:
                health1.SetActive(true);
                health2.SetActive(true);
                health3.SetActive(true);
                break;
            default:
                Debug.Log("The player health number is outside its range, you fucked up");
                break;
        }
    }

    public void EndLevel()
    {
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
