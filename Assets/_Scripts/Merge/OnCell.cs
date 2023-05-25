using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnCell : MonoBehaviour
{
    private GameManager GM;
    public int numSkin;
    public GameObject model;
    // Start is called before the first frame update

    void Awake(){
        GM = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    public void StartCell()
    {
        numSkin = PlayerPrefs.GetInt("skin"+gameObject.name);
        CreateModel();
    }

    public void CreateModel(){
      if (model) Destroy(model);
      if (numSkin >= 0){
        if (numSkin >= GM.skins.Length) numSkin = GM.skins.Length-1;

        model = Instantiate(GM.skins[numSkin], transform);
        model.transform.Rotate(Vector3.up*50);
      }
    }

    public void SetNumSkin(int n){
      numSkin = n;
      if (numSkin >= GM.skins.Length) numSkin = GM.skins.Length-1;
      PlayerPrefs.SetInt("skin"+gameObject.name, numSkin);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
