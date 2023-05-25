using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShkalaFoodRed : MonoBehaviour
{
    public Image skala_lerp;
    private Image thisSkala;
    // Start is called before the first frame update
    void Start()
    {
      thisSkala = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
      thisSkala.fillAmount = Mathf.Lerp(thisSkala.fillAmount, skala_lerp.fillAmount, Time.deltaTime);
    }
}
