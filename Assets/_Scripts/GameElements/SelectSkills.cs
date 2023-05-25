using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectSkills : MonoBehaviour
{
    public GameObject sk1;
    public GameObject sk2;
    public GameObject sk3;

    public Vector3 skills3;

    private GameManager GM;

    // Start is called before the first frame update
    void Start()
    {
      SkillManager sm = GameObject.Find("SkillManager").GetComponent<SkillManager>();
      GM = GameObject.Find("GameManager").GetComponent<GameManager>();
      GameObject pl = GM.Player;
      transform.localPosition = new Vector3(0,-300,0);
      if (!GM.gameFinished) Time.timeScale = 0;
      skills3 = sm.SkillRandom(pl);

      var s1 = Instantiate(sm.skillPrefabs[ (int)skills3.x ] , sk1.transform);
      s1.transform.localPosition = Vector3.zero;
      s1.GetComponent<OnSkill>().skillnum = (int)skills3.x;

      var s2 = Instantiate(sm.skillPrefabs[ (int)skills3.y ] , sk2.transform);
      s2.transform.localPosition = Vector3.zero;
      s2.GetComponent<OnSkill>().skillnum = (int)skills3.y;

      var s3 = Instantiate(sm.skillPrefabs[ (int)skills3.z ] , sk3.transform);
      s3.transform.localPosition = Vector3.zero;
      s3.GetComponent<OnSkill>().skillnum = (int)skills3.z;

    }

    // Update is called once per frame
    public void B_Selected()
    {
      Time.timeScale = 1;
      GameObject.Find("s_selectSkill").GetComponent<AudioSource>().Play();
      Destroy(gameObject);
    }

    public void Update(){
      if (GM.gameFinished){
        Destroy(gameObject);
      }
    }
}
