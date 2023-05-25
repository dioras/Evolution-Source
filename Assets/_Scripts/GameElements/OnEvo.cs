using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnEvo : MonoBehaviour
{

    public int evoCount;
    public GameObject effect;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Player"){
            DoEat(col.gameObject);
        }
    }


    void DoEat(GameObject trg){
            trg.GetComponent<Controll>().GetEvo(evoCount);
            Instantiate(effect, trg.transform.position+Vector3.up*0.5f, trg.transform.rotation);
            Destroy(gameObject);
    }
}
