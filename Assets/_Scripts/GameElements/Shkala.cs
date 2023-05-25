using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shkala : MonoBehaviour
{
    public Image skal_evo;
    public Image skal_food;
    public Text t_fooRate;
    public Image i_status;
    public Image i_scavenger;
    public Sprite s_predator;
    public Sprite s_herbivore;

    public Controll heroControll;

    private float timer;
    // Start is called before the first frame update
    void Start()
    {
      ChangeFoodIcons(false, false);
    }

    public void ChangeFoodRate(int count){
        t_fooRate.text = count+"";
    }

    public void ChangeFoodIcons(bool predator, bool scavenger){
      if (scavenger){
          i_scavenger.enabled = true;
      }else{
          i_scavenger.enabled = false;
      }


        if (predator){
            i_status.sprite = s_predator;
        }else{
            i_status.sprite = s_herbivore;
        }
    }

    // Update is called once per frame
    void Update()
    {/*
        timer -= Time.deltaTime;

        if (timer < 0 ){
          timer = 1;
          SetFood(heroControll.foodCur);
          SetEvo(heroControll.evoCur);
        }*/
    }

    public void SetFood(float rate){
      skal_food.fillAmount = rate/100f;
    }

    public void SetEvo(float rate){
      skal_evo.fillAmount = rate/100f;
    }
}
