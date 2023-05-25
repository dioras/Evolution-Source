using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameFinish : MonoBehaviour
{
    public Image[] circles;

    public int lvl_count;
    // Start is called before the first frame update
    void Start()
    {

        lvl_count = PlayerPrefs.GetInt("lvl_count");
        SetCircles();
    }

    void SetCircles(){
      int i = 0;
      while (lvl_count > 20){
        lvl_count -= 20;
      }

      while (i < 20){
        if (i < lvl_count){
          circles[i].color = Color.yellow;
        }
        i++;
      }

      if (lvl_count == 20) PlayerPrefs.SetInt("stageNum", 1);
    }
    // Update is called once per frame
    void Update()
    {

    }

    public void b_restart(){
      Application.LoadLevel(Application.loadedLevel);
    }

    public void B_Next(){
      //Application.LoadLevel("Game"+PlayerPrefs.GetInt("stageNum"));
      Application.LoadLevel("Merge");
    }
}
