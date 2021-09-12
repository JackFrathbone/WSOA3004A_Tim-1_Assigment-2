using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageIndicator : MonoBehaviour
{
    
    [SerializeField] Image blood;
    [SerializeField] Image red;
    [SerializeField] Image healthUp;
    [SerializeField] Image powerUp;


    [Header("Max Alphas")]
    [Range(0, 1)] [SerializeField] float maxBloodAlpha = 1;
    [Range(0, 1)] [SerializeField] float maxRedAlpha = 1;

    [Range(0, 1)] [SerializeField] float maxHealthAlpha = 0.43f;
    [Range(0, 1)] [SerializeField] float maxPowerUpAlpha = 0.8f;
    [SerializeField] float time = 1f;


    
    // Start is called before the first frame update
    void Start()
    {
        ChangeAlpha(0, blood);
        ChangeAlpha(0, red);
        blood.gameObject.SetActive(false);
        red.gameObject.SetActive(false);
        healthUp.gameObject.SetActive(false);
        powerUp.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.P)){
            Damage();
        }
    }

    void ChangeAlpha(float alpha, Image image){
        Color newColor = image.color;
        newColor.a = alpha;
        image.color = newColor;

    }
    public void Damage(){
        StartCoroutine(IndicateDamage(time));
    }

    public void GainHealth(){
        StartCoroutine(HealthUp(time));
    }

    public void IndicatePowerUp(){
        StartCoroutine(PowerUp(time));
    }



    IEnumerator HealthUp(float period){
        healthUp.gameObject.SetActive(true);
        float time = 0;
        float w = (1/period) * 2 * Mathf.PI;
        while(true){
            time += Time.fixedDeltaTime;
            float alpha = Mathf.Sin(w * time);
            
            if(time >= period/2){
                ChangeAlpha(0, healthUp);
                healthUp.gameObject.SetActive(false);
                break;
            }
            else{
                ChangeAlpha(alpha * maxHealthAlpha, healthUp);
                yield return new WaitForFixedUpdate();
            }
        }
    }

    IEnumerator PowerUp(float period){
        powerUp.gameObject.SetActive(true);
        float time = 0;
        float w = (1/period) * 2 * Mathf.PI;
        while(true){
            time += Time.fixedDeltaTime;
            float alpha = Mathf.Sin(w * time);
            
            if(time >= period/2){
                ChangeAlpha(0, powerUp);
                powerUp.gameObject.SetActive(false);
                break;
            }
            else{
                ChangeAlpha(alpha * maxPowerUpAlpha, powerUp);
                yield return new WaitForFixedUpdate();
            }
        }
    }

    IEnumerator IndicateDamage(float period){
        blood.gameObject.SetActive(true);
        red.gameObject.SetActive(true);
        float time = 0;
        float w = (1/period) * 2 * Mathf.PI;
        while(true){
            time += Time.fixedDeltaTime;
            float alpha = Mathf.Sin(w * time);
            
            if(time >= period/2){
                ChangeAlpha(0, blood);
                ChangeAlpha(0, red);
                blood.gameObject.SetActive(false);
                red.gameObject.SetActive(false);
                break;
            }
            else{
                ChangeAlpha(alpha * maxBloodAlpha, blood);
                ChangeAlpha(alpha * maxRedAlpha, red);
                yield return new WaitForFixedUpdate();
            }
        }
    }

    
}
