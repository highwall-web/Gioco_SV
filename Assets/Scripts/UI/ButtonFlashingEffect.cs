using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonFlashingEffect : MonoBehaviour
{
    public float flashSpeed = 5f; //velocità di lampeggiamento
    public Color flashColor = Color.red; //colore del lampeggio

    private Button button;
    private Color normalColor;
    private bool isFlashing = false;


    // Start is called before the first frame update
    void Start()
    {
        button = GetComponent<Button>();
        normalColor = button.colors.normalColor;
    }

    // Update is called once per frame
    void Update()
    {
        if(isFlashing){
            Color targetColor = Color.Lerp(normalColor, flashColor, Mathf.PingPong(Time.time * flashSpeed, 1f));
            button.image.color = targetColor;
        }
    }

    public void StartFlashing(){
        isFlashing = true;
    }

    public void StopFlashing(){
        isFlashing = false;
        Color transparent = normalColor;
        transparent.a = 0f;
        button.image.color = transparent; //ripristina il colore normale
    }
}
