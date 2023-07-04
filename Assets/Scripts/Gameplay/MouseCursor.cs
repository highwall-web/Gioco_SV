using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseCursor : MonoBehaviour
{

    public Texture2D cursorTexture; //immagine del puntatore
    public Vector2 hotspot = Vector2.zero;


    // Start is called before the first frame update
    void Start()
    {
        Cursor.SetCursor(cursorTexture, hotspot, CursorMode.Auto);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
