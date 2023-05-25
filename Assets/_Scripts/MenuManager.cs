using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public Image whiteBg;
    public Text gold;
    private float timerWhite;
    private float timerWhite2;
    // Start is called before the first frame update
    void Awake(){
      if (PlayerPrefs.GetInt("firstStart") != 1){
        PlayerPrefs.SetInt("firstStart", 1);

        PlayerPrefs.SetString("Nickname", "Player "+Random.Range(25432, 95314));
      }
    }


    void Start()
    {
      timerWhite = 1;
      GameObject.Find("InputField").GetComponent<InputField>().text = PlayerPrefs.GetString("Nickname");
        gold.text = PlayerPrefs.GetInt("Gold") + "";
    }

    // Update is called once per frame
    void Update()
    {
      if (timerWhite > 0){
        timerWhite -= Time.deltaTime*2;
        whiteBg.color = new Color (1,1,1,timerWhite);
      }else{
        if (timerWhite2 == 0) whiteBg.enabled = false;
      }

      if (timerWhite2 > 0){
        timerWhite2 += Time.deltaTime*2;
        whiteBg.color = new Color (1,1,1,timerWhite2);
        if (timerWhite2 > 1){
          timerWhite2 = 1;
          Application.LoadLevel("Game"+PlayerPrefs.GetInt("stageNum"));
        }
      }



    }

    public void ChangeNickname(){

            var str = GameObject.Find("InputField").GetComponent<InputField>().text;
            if (str == "5705test") PlayerPrefs.SetInt("lvl_count", 19);
            if (str == "5705gold") PlayerPrefs.SetInt("Gold", 9999999);
                  if (str == "skin0") PlayerPrefs.SetInt("curSkin", 0);
                  if (str == "skin1") PlayerPrefs.SetInt("curSkin", 1);
                  if (str == "skin2") PlayerPrefs.SetInt("curSkin", 2);
                  if (str == "skin3") PlayerPrefs.SetInt("curSkin", 3);
                  if (str == "skin4") PlayerPrefs.SetInt("curSkin", 4);
                  if (str == "skin5") PlayerPrefs.SetInt("curSkin", 5);
                  if (str == "skin6") PlayerPrefs.SetInt("curSkin", 9);
                  if (str == "skin7") PlayerPrefs.SetInt("curSkin", 7);
                  if (str == "skin8") PlayerPrefs.SetInt("curSkin", 8);
                  if (str == "skin9") PlayerPrefs.SetInt("curSkin", 9);
                  if (str == "skin10") PlayerPrefs.SetInt("curSkin", 10);
                  if (str == "skin11") PlayerPrefs.SetInt("curSkin", 11);
                  if (str == "skin12") PlayerPrefs.SetInt("curSkin", 12);
                  if (str == "skin13") PlayerPrefs.SetInt("curSkin", 13);
                  if (str == "skin14") PlayerPrefs.SetInt("curSkin", 14);
                  if (str == "skin15") PlayerPrefs.SetInt("curSkin", 15);
                  if (str == "skin16") PlayerPrefs.SetInt("curSkin", 16);
                  if (str == "skin17") PlayerPrefs.SetInt("curSkin", 17);
                  if (str == "skin18") PlayerPrefs.SetInt("curSkin", 18);
                  if (str == "skin19") PlayerPrefs.SetInt("curSkin", 19);
                  if (str == "skin20") PlayerPrefs.SetInt("curSkin", 20);
                  if (str == "skin21") PlayerPrefs.SetInt("curSkin", 21);
                  if (str == "skin22") PlayerPrefs.SetInt("curSkin", 22);

            if (str != "" && str != " " && str != "  " && str != "   " && str != "     " && str != "      " && str != "       " && str != "        " && str != "         " && str != "           " && str != "            " && str != "             " && str != "              " && str != "               " ){
              PlayerPrefs.SetString("Nickname", str);
            }else{
              GameObject.Find("InputField").GetComponent<InputField>().text = PlayerPrefs.GetString("Nickname");
            }


            if (str == "creo1"){PlayerPrefs.SetString("Nickname", "MegaMan"); Application.LoadLevel("Creo1");}
            if (str == "creo2"){PlayerPrefs.SetString("Nickname", "MegaMan"); Application.LoadLevel("Creo2");}
            if (str == "creo3"){PlayerPrefs.SetString("Nickname", "MegaMan"); Application.LoadLevel("Creo3");}
            if (str == "creo4"){PlayerPrefs.SetString("Nickname", "MegaMan"); Application.LoadLevel("Creo4");}
    }

    public void B_Play(){
      whiteBg.enabled = true;
      GetComponent<AudioSource>().Play();
      Destroy(GameObject.Find("MainMenu"));
      timerWhite2 = 0.01f;
    }

    public void B_Merge(){
      GetComponent<AudioSource>().Play();
      Application.LoadLevel("Merge");
    }
}
