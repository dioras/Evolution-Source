using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnArrow : MonoBehaviour
{
    public GameObject Player;
    public GameObject prnt;
    public bool doit;

    void Start(){
      transform.position = Vector3.up*999;
    }
    // Start is called before the first frame update
    public void StartIt(GameObject pr, GameObject pl)
    {
      prnt = pr;
      Player = pl;
      doit = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (doit){
          if (Player && prnt){
            if (Vector3.Distance( prnt.transform.position, Player.transform.position) < 18){
              transform.position = prnt.transform.position;
              transform.LookAt(Player.transform);
            }else{
              transform.position = Vector3.up*999;
            }
          }else{
            Destroy(gameObject);
          }
        }
    }
}
