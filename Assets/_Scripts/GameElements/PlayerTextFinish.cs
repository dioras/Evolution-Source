using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerTextFinish : MonoBehaviour
{
    public Text t_num;
    public Text playerName;
    public Image bg;

    private GameManager GM;
    // Start is called before the first frame update
    public void StartIt(int leaderNum)
    {
      if (leaderNum != 0){
        GM = GameObject.Find("GameManager").GetComponent<GameManager>();
        t_num.text = leaderNum + "";
        playerName.text = GM.LeaderString[leaderNum];

        if (playerName.text ==  PlayerPrefs.GetString("Nickname") ){
          bg.color = new Color(0.75f, 0, 0.90f);
          transform.localScale *= 1.1f;
        }
      }
      //LeaderString
    }

    // Update is called once per frame
    void Update()
    {

    }
}
