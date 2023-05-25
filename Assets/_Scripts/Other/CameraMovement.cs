using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    // Start is called before the first frame update
    public float speed;
    public Vector3 vectorCorrection;

    //public float minZ;

    public GameObject hero;

    public float view;
    private bool doview;
    private float correct;
    void Start()
    {
        //hero = GameObject.Find("Hero");
        //view = 60;

        GetComponent<Camera>().fieldOfView = view + (PlayerPrefs.GetInt("s_zoom")*0.3f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (hero) transform.position = Vector3.Lerp(transform.position, hero.transform.position + vectorCorrection, Time.fixedDeltaTime*speed);

         if (doview)
          {
              Camera.main.fieldOfView  += Time.fixedDeltaTime*correct*7;
              if (correct == 1)
              {
                  if (Camera.main.fieldOfView  > view)
                  {
                      Camera.main.fieldOfView  = view;
                      doview = false;
                  }
              }
              else
              {
                  if (Camera.main.fieldOfView  < view)
                  {
                      Camera.main.fieldOfView  = view;
                      doview = false;
                  }
              }
          }

    }

    public void SetFieldView(float x, float c)
    {
        view += x;
        if (view > 25) view = 25;
        correct = c;
        doview = true;
    }
}
