using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectLocation : MonoBehaviour
{
    // Start is called before the first frame update
    public int stage;
    public int maxStage;
    public string[] stageNames;
    public GameObject b_play;
    public GameObject lockObjects;

    public Transform stagesVisual;
    public Text stageName;
    public Text t_count;

    private int lvl_count;
    void Start()
    {
      lvl_count = PlayerPrefs.GetInt("lvl_count");
      SetStage();
    }

    // Update is called once per frame
    void SetStage()
    {
      stagesVisual.position = new Vector3(stage * 100, 0,0);
      stageName.text = stageNames[stage];

      if (lvl_count >= stage * 20){
        b_play.active = true;
        lockObjects.active = false;

        PlayerPrefs.SetInt("stageNum", stage);
      }else{
        b_play.active = false;
        lockObjects.active = true;
        t_count.text = lvl_count + " / " + (stage*20);
      }

    }

    public void b_arrow(int step){
      stage += step;
      if (stage > maxStage){
        stage = 0;
      }
      if (stage < 0){
        stage = maxStage;
      }
      SetStage();
      GameObject.Find("MenuManager").GetComponent<AudioSource>().Play();
    }
}
