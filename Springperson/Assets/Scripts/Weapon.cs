using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] float range;
    [SerializeField] float springSpd;
    Camera cam;
    public GameObject springEnd;
    Coroutine extension = null;

    // Start is called before the first frame update
    void Start()
    {
        

        
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0) && extension == null){
            extension = StartCoroutine(Extend(springSpd, range));
        }
    }
    IEnumerator Extend(float period, float amplitude){
        float w = (1/period) * 2 * Mathf.PI;
        float time = 0;
        Vector3 startPos = springEnd.transform.localPosition;
        while(true){
            time += Time.deltaTime;
            float d = amplitude * Mathf.Sin(w * time);
            if(time >= period/2){
                springEnd.transform.localPosition = startPos;
                extension = null;
                break;
            }
            else{
                springEnd.transform.localPosition = startPos + Vector3.forward * d;
                yield return null;
            }
        }

    }

}
