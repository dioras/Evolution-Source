using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoText : MonoBehaviour
{
    public GameObject t_info;
    public Color[] ColorTemplate;
    public float speed;

    private bool doit;
    private float cordY;

    private float timer;
    // Start is called before the first frame update
    void Awake()
    {
      transform.localScale = new Vector3(1,0, 1);
    }

    // Update is called once per frame
    void Update()
    {
        if (doit){
          cordY += Time.deltaTime*speed;
          if (cordY > 1){
             cordY = 1;
             doit = false;
             timer = 0.01f;
           }
          transform.localScale = new Vector3(1,cordY, 1);
        }

        if (timer != 0){
          timer += Time.deltaTime;
          if (timer >3.7f){
            timer = 0;
            transform.localScale = new Vector3(1,0, 1);
          }
        }
    }

    public void ShowText(string txt, int clr){
        t_info.GetComponent<Text>().text = txt;
        t_info.GetComponent<Text>().color = ColorTemplate[clr];

        cordY = 0;
        timer = 0;
        doit = true;

    }
}
