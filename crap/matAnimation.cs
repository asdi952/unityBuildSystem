using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class matAnimation : MonoBehaviour
{
    public Material mat;
    Color color;
    void Start()
    {
        color = mat.GetColor("_EmissionColor");
    }

    // Update is called once per frame
    bool block = false;
    float intensity = 0;
    float iMult = 0.1f;
    void Update()
    {
        if( Input.GetButtonDown("e")){
            if( block){
                mat.EnableKeyword("_EMISSION");
            }else{
                mat.DisableKeyword("_EMISSION");
            }
            block = !block;
        }
        if( intensity > 1)
            iMult = - iMult;
        else if( intensity < 0)
            iMult = - iMult;
        
        intensity += iMult;
        mat.SetColor("_EmissionColor", color * intensity * Time.deltaTime);
    }
}
