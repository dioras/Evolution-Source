using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodRespawn : MonoBehaviour
{
    public bool dontRespAtStart;
    [Space]
    public GameObject foodHerb;
    public GameObject foodPredator;
    public GameObject foodCorpse;

    [Space]
    public bool predator;
    public bool scavenger;

    [Space]
    public float time_min;
    public float time_max;

    private GameObject curFood;

    private GameManager GM;


    private float timer;
    private float timerZone;

    private bool startDestroy;
    // Start is called before the first frame update
    void Start()
    {
      if (time_min == 0) time_min = Random.Range(0, 4f);
      if (time_max == 0) time_max = Random.Range(10, 20f);
      if (!dontRespAtStart) DoFood();
      GM = GameObject.Find("GameManager").GetComponent<GameManager>();
      timerZone = time_min;
    }

    void DoFood(){
      GameObject fd;
      if (scavenger){
        fd = foodCorpse;
      }else{
        if (predator){
          fd = foodPredator;
        }else{
          fd = foodHerb;
        }
      }

      curFood = Instantiate(fd, transform);
      //curFood.transform.localPosition = Vector3.zero;

      timer = Random.Range(time_min, time_max);
    }

    // Update is called once per frame
    void Update()
    {
      if (!startDestroy){
        if (!curFood){
          timer -= Time.deltaTime;
          if (timer < 0){
            DoFood();
          }
        }

        timerZone += Time.deltaTime;
        if (timerZone > 5){
          timerZone = 0;
          if (Vector3.Distance(transform.position, Vector3.zero) > GM.gameZone ){
            startDestroy = true;
            Destroy(gameObject, 2f);
          }
        }
      }else{
        transform.Translate(-Vector3.up*Time.deltaTime);
      }

    }
}
