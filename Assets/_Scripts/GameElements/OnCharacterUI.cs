using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnCharacterUI : MonoBehaviour
{
    private GameObject Player;
    // Start is called before the first frame update
    void Awake()
    {
        transform.parent.transform.gameObject.GetComponent<Controll>().SetCharacterUI(gameObject);
        transform.parent = null;
    }

    // Update is called once per frame
    void Update()
    {
    }
}
