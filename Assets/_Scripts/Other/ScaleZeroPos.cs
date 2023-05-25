using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleZeroPos : MonoBehaviour
{
    private float timer;
    // Start is called before the first frame update
    void Start()
    {
        transform.localScale = Vector3.zero;
        Destroy(this, 4);
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
      //  if (timer > 0.01f){
          transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one , Time.deltaTime*15);
      //  }
    }
}
