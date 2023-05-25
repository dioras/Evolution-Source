using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLables : MonoBehaviour
{

    private GameManager GM;

    public PlayerTextFinish[] ptf;
    // Start is called before the first frame update
    void Start()
    {
      GM = GameObject.Find("GameManager").GetComponent<GameManager>();

      int i = 1;
      int istart = GM.playerNum;

      while (i < 11 && istart < 21){
        ptf[i-1].StartIt(istart);

        i++;
        istart += 1;
      }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void B_Next(){
      Application.LoadLevel("Game");
    }
}
