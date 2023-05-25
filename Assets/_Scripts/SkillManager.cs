using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{

    public GameObject SelectSkills;
    public string[] skillDescription;
    public GameObject[] skillPrefabs;

    private int skmax;
    private GameObject canvas;

    private InfoText infoText;
    // Start is called before the first frame update
    void Awake()
    {

       skmax = skillPrefabs.Length;
       canvas = GameObject.Find("Canvas");
       infoText = GameObject.Find("InfoText").GetComponent<InfoText>();
    }

    // Update is called once per frame
    public void ShowSkills()
    {
      Instantiate(SelectSkills, canvas.transform);//.transform.localPosition = Vector3.zero;
    }

    public Vector3 SkillRandom(GameObject g){
      Vector3 skills3 = new Vector3(-1,-1,-1);
      Controll controll = g.GetComponent<Controll>();
      int tryer = 999;

      int sknum = -1;
      while (skills3.x < 0){ // 1 skill --------------------------------
        sknum = Random.Range(0, skmax);
        sknum = this.skchecked(sknum, controll);

        if (sknum >= 0){
          skills3 += new Vector3(1+sknum, 0, 0);
        }

        tryer -= 1;
        if (tryer < 0) skills3 += new Vector3(1+999, 0, 0);
      }

      sknum = -1;
      while (skills3.y < 0){ // 2 skill -------------------------------
        sknum = Random.Range(0, skmax);

        if ( sknum == (int)skills3.x || skillDescription[(int)skills3.x] == skillDescription[sknum] ){
            sknum = -1;
        }else{
            sknum = this.skchecked(sknum, controll);
        }

        if (sknum >= 0){
          skills3 += new Vector3(0, 1+sknum, 0);
        }

        tryer -= 1;
        if (tryer < 0) skills3 += new Vector3(0, 1+999, 0);
      }

      sknum = -1;
      while (skills3.z < 0){ // 3 skill --------------------------------
        sknum = Random.Range(0, skmax);

        if ( sknum == (int)skills3.x || sknum == (int)skills3.y ){
            sknum = -1;
        }else{
            sknum = this.skchecked(sknum, controll);
        }

        if (sknum >= 0){
          skills3 += new Vector3( 0, 0, 1+sknum );
        }

        tryer -= 1;
        if (tryer < 0){ skills3 += new Vector3(0, 0, 1+999); Debug.Log("ASFASFA");}
      }




      return skills3;
    }

    public int skchecked(int sk, Controll controll){
      int sknum = sk;

      if (sknum == 0){ // if predator
        if (controll.predator) sknum = -1;
      }
      if (sknum == 1){ // if scavender
        if (controll.scavenger) sknum = -1;
      }
      if (sknum == 2){ // if herd
        if (!controll.predator) sknum = -1;
      }
      if (sknum == 3){ // if foodrate low
        if (controll.foodRate >= -1) sknum = -1;
      }
      if (sknum == 6){ // if long neck already
        if (controll.longNeck) sknum = -1;
      }
      if (sknum == 7){ // if long neck already
        if (controll.biggest) sknum = -1;
      }
      if (sknum == 8){ // if droptail
        if (controll.tailDropping) sknum = -1;
      }
      if (sknum == 9){ // if tracking
        if (controll.tracking) sknum = -1;
      }
      if (sknum == 10){ // if posion
        if (controll.poison) sknum = -1;
      }
      if (sknum == 11){ // if flight
        if (controll.flight) sknum = -1;
      }
      if (sknum == 12){ // if camouflage
        if (controll.camouflage) sknum = -1;
      }
      if (sknum == 13){ // if jump
        if (controll.jump || !controll.predator) sknum = -1;
      }

      return sknum;
    }

    public void ApplySkill(int skillnum, GameObject g){
      Controll controll = g.GetComponent<Controll>();

      if (skillnum == 0){ // predator
        //controll.s_predator();
        controll.s_evolution();
      }

      if (skillnum == 1){ // scavenger
        //controll.s_scavenger();
        controll.s_evolution();
      }

      if (skillnum == 2){ // herbivore
        //controll.s_herbivore();
        controll.s_swift();
      }

      if (skillnum == 3){ // FAT
        //controll.s_fat();
        controll.s_evolution();
      }

      if (skillnum == 4){ // SWIFT SPEED
        controll.s_swift();
      }

      if (skillnum == 5){ // EVOLUTION FAST
        controll.s_evolution();
      }

      if (skillnum == 6){ // LONG NECK
        controll.s_longNeck();
      }

      if (skillnum == 7){ // LONG NECK
      //  controll.s_biggest();
        controll.s_swift();
      }

      if (skillnum == 8){ // drop tail
        controll.s_droptail();
      }

      if (skillnum == 9){ // tracking
        controll.s_tracking();
      }

      if (skillnum == 10){ // poison
        //controll.s_poison();
          controll.s_swift();
      }

      if (skillnum == 11){ // flight
        //controll.s_flight();
          controll.s_evolution();
      }

      if (skillnum == 12){ // camouflage
        controll.s_camouflage();
      }

      if (skillnum == 13){ // jump
        controll.s_jump();
      }





      if (!controll.Bot){
        infoText.ShowText( skillDescription[skillnum], 0 );
      }
    }
}
