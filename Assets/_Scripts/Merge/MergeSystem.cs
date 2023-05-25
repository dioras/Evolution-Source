using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MergeSystem : MonoBehaviour
{
    private GameManager GM;
    public GameObject skinRespawn;
    public GameObject b_buy;
    public Text gold;
    public GameObject merge_effect;
    public GameObject new_effect;
    public GameObject tutorial;
    public Text t_speed;
    public Text t_grow;
    [Space]
    public GameObject model;
    public GameObject curCellObj;

    public int pickSkin;
    public OnCell curCell;

    //private Metrica metrica;


    private int bestSkin;
    // Start is called before the first frame update
    void Awake(){
      if (PlayerPrefs.GetInt("firstSettings") != 1){
        PlayerPrefs.SetInt("curSkin", 0);

        PlayerPrefs.SetInt("skin1", 0);
        PlayerPrefs.SetInt("skin2", 0);
        PlayerPrefs.SetInt("skin3", -1);
        PlayerPrefs.SetInt("skin4", -1);
        PlayerPrefs.SetInt("skin5", -1);
        PlayerPrefs.SetInt("skin6", -1);
        PlayerPrefs.SetInt("skin7", -1);
        PlayerPrefs.SetInt("skin8", -1);
        PlayerPrefs.SetInt("skin9", -1);
        PlayerPrefs.SetInt("skin10", -1);
        PlayerPrefs.SetInt("skin11", -1);
        PlayerPrefs.SetInt("skin12", -1);
        PlayerPrefs.SetInt("skin13", -1);
        PlayerPrefs.SetInt("skin14", -1);
        PlayerPrefs.SetInt("skin15", -1);
        PlayerPrefs.SetInt("skin16", -1);
        PlayerPrefs.SetInt("skin17", -1);
        PlayerPrefs.SetInt("skin18", -1);
        PlayerPrefs.SetInt("skin19", -1);
        PlayerPrefs.SetInt("skin20", -1);

        PlayerPrefs.SetInt("firstSettings", 1);
      }else{
        tutorial.active = false;
      }

          GM = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    void Start()
    {
      // if (GameObject.Find("AppMetrica")){
      //   metrica = GameObject.Find("AppMetrica").GetComponent<Metrica>();
      // }

    //  PlayerPrefs.SetInt("Gold", 50000);

      gold.text = PlayerPrefs.GetInt("Gold") + "";

      RefreshCells(false);
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Input.GetMouseButtonDown(0)){
          if (Physics.Raycast(ray, out hit, 100) ){
            if (hit.collider.tag == "Respawn"){
              if (hit.collider.gameObject.GetComponent<OnCell>().numSkin > -1){
                model = hit.collider.gameObject.GetComponent<OnCell>().model;
                pickSkin = hit.collider.gameObject.GetComponent<OnCell>().numSkin;
                curCell = hit.collider.gameObject.GetComponent<OnCell>();
                curCellObj = hit.collider.gameObject;
                  tutorial.active = false;
              }
            }

            if (hit.collider.tag == "Button"){
              if (hit.collider.name == "b_play"){
                GetComponent<AudioSource>().Play();
                 Application.LoadLevel("Game"+PlayerPrefs.GetInt("stageNum"));
                 hit.collider.gameObject.AddComponent<ButtonClickEffect>();
               }
              if (hit.collider.name == "b_buy"){
                 TryBuy(500);
                 hit.collider.gameObject.AddComponent<ButtonClickEffect>();
               }
            }
          }
        }

        if (Input.GetMouseButton(0)){
          if (model){
            if (Physics.Raycast(ray, out hit, 100) ){
              model.transform.position = hit.point;
            }
          }
        }

        if (Input.GetMouseButtonUp(0)){
          if (Physics.Raycast(ray, out hit, 100) ){
            if (hit.collider.tag == "Respawn" && model){
              if (hit.collider.gameObject.GetComponent<OnCell>().numSkin == pickSkin && curCellObj != hit.collider.gameObject){
                hit.collider.gameObject.GetComponent<OnCell>().SetNumSkin(pickSkin+1);
                curCell.SetNumSkin(-1);
                Instantiate(merge_effect, hit.collider.gameObject.transform.position+(Vector3.up*0.5f), merge_effect.transform.rotation);
                GameObject.Find("s_levelUp").GetComponent<AudioSource>().Play();
              }else{
                int tempNum = pickSkin;
                curCell.SetNumSkin(hit.collider.gameObject.GetComponent<OnCell>().numSkin);
                hit.collider.gameObject.GetComponent<OnCell>().SetNumSkin(pickSkin);
              }
            }

            if (hit.collider.tag == "Delete" && model){
              if (curCell.numSkin != PlayerPrefs.GetInt("curSkin")) curCell.SetNumSkin(-1);
              GameObject.Find("s_kill").GetComponent<AudioSource>().Play();
            }
          }
          RefreshCells(false);
          GetComponent<AudioSource>().Play();

          model = null;
          pickSkin = -1;
          curCell = null;
          curCellObj = null;
        }


    }

    void TryBuy(int g){
      if (PlayerPrefs.GetInt("Gold") >= g){
        PlayerPrefs.SetInt("Gold", PlayerPrefs.GetInt("Gold") - g);
        gold.text = PlayerPrefs.GetInt("Gold") + "";

        RefreshCells(true);
        GameObject.Find("s_selectSkill").GetComponent<AudioSource>().Play();
      }
    }

    void RefreshCells(bool makenew){
      bestSkin = 0;
      bool empty = false;
      foreach(Transform tr in transform){

        if (tr.GetComponent<OnCell>().numSkin == -1){
          if (makenew){

            Instantiate(new_effect, tr.position+(Vector3.up*0.5f), new_effect.transform.rotation);
            makenew = false;
            int cs = PlayerPrefs.GetInt("curSkin") - 3;
            if (cs > 5) cs -= 1;
            if (cs > 10) cs -= 1;
            if (cs > 13) cs -= 1;
            if (cs > 15) cs -= 1;

            if (cs < 0) cs = 0;
            tr.GetComponent<OnCell>().SetNumSkin(cs);
          }else{
            empty = true;
          }
        }

        tr.GetComponent<OnCell>().StartCell();

        if (tr.GetComponent<OnCell>().numSkin > bestSkin){
           bestSkin = tr.GetComponent<OnCell>().numSkin;
           if (bestSkin > PlayerPrefs.GetInt("curSkin") ){
              PlayerPrefs.SetInt("curSkin", bestSkin);
              //if (metrica) metrica.skin_unlock("hero", bestSkin+"", "common", "specific_currency");
            }
         }
      }

      foreach(Transform tr in skinRespawn.transform){
        Destroy(tr.gameObject);
      }

      int csn = PlayerPrefs.GetInt("curSkin");

      var mdl = Instantiate(GM.skins[csn], skinRespawn.transform);
      mdl.transform.localScale *= 1.7f;

      b_buy.active = empty;

      t_speed.text = "+" + (5+(csn*1)) + "%";
      t_grow.text = "+" + (3+(csn*2)) + "%";

    }

    public void b_home(){
      Application.LoadLevel("Menu");
    }
}
