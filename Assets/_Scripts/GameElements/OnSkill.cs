using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnSkill : MonoBehaviour
{
    public int skillnum;
    // Start is called before the first frame update
    public void B_SelectSkill()
    {
      GameObject pl = GameObject.Find("GameManager").GetComponent<GameManager>().Player;

      if (pl) GameObject.Find("SkillManager").GetComponent<SkillManager>().ApplySkill(skillnum, pl);

      transform.parent.transform.parent.GetComponent<SelectSkills>().B_Selected();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
