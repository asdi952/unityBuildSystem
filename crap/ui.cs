using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ui : MonoBehaviour
{
    void OnGUI(){
        GUI.color = Color.red;
        GUI.DrawTexture( new Rect(0, 0, 100, 100), Texture2D.whiteTexture);
    }
}
