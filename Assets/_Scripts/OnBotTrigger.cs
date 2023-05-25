using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnBotTrigger : MonoBehaviour
{
    private GameObject Player;
    private Controll controll;

    private float timer;

    private bool predator;
    private bool scavenger;
    private bool camouflage;
    private float timerCamouflage;

    private bool startit;

    private int poisonRnd;
    // Start is called before the first frame update
    void Start()
    {
        Player = transform.parent.transform.gameObject;
        controll = Player.GetComponent<Controll>();
        poisonRnd = Random.Range(2,8);

        if (!controll.Bot ){
           Destroy(gameObject);
        }else{
          predator = controll.predator;
          scavenger = controll.scavenger;
          camouflage = controll.scavenger;
          startit = true;
        }

    }

    // Update is called once per frame
    void Update()
    {
      timer -= Time.deltaTime;

      if (timer < 0){
        timer = 3;
        predator = controll.predator;
        scavenger = controll.scavenger;
      }
    }



    private void OnTriggerStay(Collider col)
    {
      if (startit){
              if (col.tag == "Food"){
                  if (col.GetComponent<OnFood>().predator){
                    if(predator) controll.BotTarget(col.gameObject, 20);
                  }else if (col.GetComponent<OnFood>().scavenger){
                    if(scavenger) controll.BotTarget(col.gameObject, 20);
                  }else{
                    if(!predator) controll.BotTarget(col.gameObject, 20);
                  }
              }else if (col.tag == "Evo"){
                  controll.BotTarget(col.gameObject, 25);
              }
      }
    }

    private void OnTriggerEnter(Collider col)
    {
      if (col.tag == "Player"){
          Controll cntrlEnemy = col.gameObject.GetComponent<Controll>();

          if (predator){
            if (!cntrlEnemy.hidden){
              if (controll.CheckKill(col.gameObject) ){
                  if (cntrlEnemy.poison){
                    if (controll.foodCur < 30 || controll.GM.totalPlayers < poisonRnd){
                      controll.BotTarget(col.gameObject, 30);
                    }
                  }else{
                      controll.BotTarget(col.gameObject, 30);
                  }
              }
            }
          }

          if (!cntrlEnemy.hidden && cntrlEnemy.predator){
            if (controll){
              if (!controll.CheckKill(col.gameObject) ){
                int rnd = Random.Range(0,2);
                if (rnd == 0){
                  controll.BotFear();
                  controll.BotTarget(col.gameObject, 30);
                }
              }
            }
          }
      }

    }
}
