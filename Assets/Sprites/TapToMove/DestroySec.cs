using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroySec : MonoBehaviour
{
    public float timeDestroy;
    // Start is called before the first frame update
    void Awake(){
    }

    void Start()
    {

      Destroy(gameObject, timeDestroy);

    }

    // Update is called once per frame
    void Update()
    {
    }
}
