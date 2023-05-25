using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnFood : MonoBehaviour
{

    public int eatCount;

    public bool predator;
    public bool scavenger;

    public GameObject[] foods;

    // Start is called before the first frame update
    void Start()
    {
      Instantiate(foods[Random.Range(0, foods.Length)], transform);
    }

    // Update is called once per frame
    void Update()
    {

    }
/*
    private void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Player"){
            DoEat(col.gameObject);
        }
    }


    void DoEat(GameObject trg){
      if (predator){
        if (trg.GetComponent<Controll>().predator){
            trg.GetComponent<Controll>().GetFood(eatCount);
            Destroy(gameObject);
        }
      }else if(scavenger){
          if (trg.GetComponent<Controll>().scavenger){
              trg.GetComponent<Controll>().GetFood(eatCount);
              Destroy(gameObject);
          }
      }else{
        if (!trg.GetComponent<Controll>().predator){
            trg.GetComponent<Controll>().GetFood(eatCount);
            Destroy(gameObject);
          }
      }
    }
    */
}
