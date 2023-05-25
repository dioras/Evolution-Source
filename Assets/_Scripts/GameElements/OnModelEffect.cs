using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnModelEffect : MonoBehaviour
{
    public int types;
    public GameObject Tooth;
    public GameObject Tail;
    public GameObject tailEffect;
    public GameObject Poison;
    public GameObject Wings;
    public GameObject jump;
    private Vector3 startScale;

    public Texture[] tex;
    public SkinnedMeshRenderer body;
    // Start is called before the first frame update
    void Start()
    {
      startScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void effect_predator(bool st){
    /*  if (st){
        Tooth.active = true;
        body.material.SetTexture("_MainTex", tex[2]);
      }else{
        Tooth.active = false;
        body.material.SetTexture("_MainTex", tex[0]);
      }*/
    }

    public void effect_longNeck(){
/*
      if (types< 2){
        transform.localEulerAngles = new Vector3(-25, 0 , 0);
      }
      if (types == 2){
        //transform.localScale *= 1.6f;
      }*/
    }

    public void effect_biggest(){
    /*  if (types < 2){
        transform.localScale *= 1.8f;
      }
      if (types == 2){
        transform.localScale *= 1.6f;
      }*/
    }

    public void DoBig(){
      transform.parent.transform.localScale += Vector3.one*0.15f;
    }

    public void effect_tailDrop(bool st){
      if (st){
      //  Tail.transform.localScale = new Vector3(1, 2, 1);
      //  body.material.SetTexture("_MainTex", tex[1]);
      }else{
      //  Tail.transform.localScale = new Vector3(1, 0, 1);
      //  body.material.SetTexture("_MainTex", tex[0]);
        Destroy(Instantiate(tailEffect, transform.position, transform.rotation), 3);
      }
    }

    public void effect_poison(){
    //  Poison.active = true;
    }

    public void effect_jump(){
    //  jump.active = true;
    //  Wings.active = true;
    }

    public void effect_flight(){
    //  Wings.active = true;
    //  transform.localPosition += Vector3.up*1;
    }



    public void effect_hidden(bool st, bool pred){
      /*
      if (st){
        body.material.SetTexture("_MainTex", tex[3]);
      }else{
        if (pred){
          body.material.SetTexture("_MainTex", tex[2]);
        }else{
          body.material.SetTexture("_MainTex", tex[0]);
        }
      }*/
    }


}
