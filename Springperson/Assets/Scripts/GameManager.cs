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

    [SerializeField] HeartSpawner [] hearts;
    [SerializeField] int playerNoDamageTime;
    [SerializeField] DamageIndicator damageIndicator;
    [SerializeField] PlayerScoreDisplay scoreDisplay;
    [SerializeField] Image health1, health2, health3;
    
    

    [SerializeField] float powerUpTime = 10f;
    [SerializeField] float PowerUpSpawnFrequency = 30f;
    [SerializeField] float powerUpChance = 10;
    [SerializeField] int maxHearts = 1; // maximum number of hearts which may be spawned at one time
    [SerializeField] TextMeshProUGUI scoreText;

    [SerializeField] GameObject restartScreen;
    [SerializeField] TextMeshProUGUI endScoreText;
    [SerializeField] TextMeshProUGUI highscoreText;

    [SerializeField] CameraController cameraController;
    

    bool isPoweredUp;
    bool gameOver;
    float time;

    public bool IsPoweredUp { get => isPoweredUp; set => isPoweredUp = value; }
    public bool GameOver { get => gameOver; set => gameOver = value; }
    public int PlayerHealth { get => _playerHealth; set => _playerHealth = value; }
    public int MaxHearts { get => maxHearts; set => maxHearts = value; }

    private void Update() {
        
    }
    void Start()
    {
        time = 0;
        isPoweredUp = false;
        //isPoweredUp = true;
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
            if(CheckSpawnedHearts() == 0){
                SpawnHeart(maxHearts);
            }
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

    public void SpawnPowerUp(){
        if(CheckSpawnedHearts() == hearts.Length){
            return;
        }
        if(!isPoweredUp && CheckSpawnedPowerUps() == 0){
            int random = (int) Random.Range(0, powerUpChance);
            if(random == 0){
                while(true){

                    int spawner = (int) Random.Range(0, hearts.Length);
                    if(hearts[spawner].Empty){
                        hearts[spawner].SpawnPowerUp();
                        break;
                    }
                }
                
                
            }

        }
    }

    public void SpawnHeart(int num){
        if(num > hearts.Length || CheckSpawnedHearts() >= num){
            Debug.Log("too many hearts");
            return;
        }
        else{
            if(_playerHealth < 3){
                
                if(num == 1){
                    if(CheckSpawnedHearts() < num){
                        int random = Random.Range(0, hearts.Length);
                        hearts[random].SpawnHeart();
                    }
                    
                }
                else if(num > 1){
                    List<int> numbers = new List<int>();
                    for(int loop = 0; loop < num; loop++){
                        if(CheckSpawnedHearts() >= num){
                            return;
                        }
                        else{
                            while(true){
                                int random = Random.Range(0, hearts.Length);
                                if(!numbers.Contains(random) && hearts[random].Empty){
                                    hearts[random].SpawnHeart();
                                    numbers.Add(random);
                                    break;
                                }
                        
                            }
                        }
                    
                    }
                }
            }
        }
    }

    public int CheckSpawnedHearts(){
        int count = 0;
        for(int loop = 0; loop< hearts.Length; loop++){
            if(!hearts[loop].Empty){
                count++;
            }
        }
        return count;
    }
    public int CheckSpawnedPowerUps(){
        int count = 0;
        for(int loop = 0; loop< hearts.Length; loop++){
            if(!hearts[loop].Empty && hearts[loop].PowerUpSpawned){
                count++;
            }
        }
        return count;
    }
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
        StopAllCoroutines();

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

    public void StartPowerUp(){
        StartCoroutine(PowerUpPlayer(powerUpTime));
    }

    IEnumerator PowerUpPlayer(float time){
        isPoweredUp = true;
        damageIndicator.IndicatePowerUp();
        float t = 0;
        while(true){
            t += Time.deltaTime;
            if(t >= time){
                break;
            }
            else{
                isPoweredUp = false;
                yield return null;
            }
        }
    }
}
