using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayStats : MonoBehaviour
{
    List<float> fpsList = new List<float>();
    

    misc.CheckTimer fpsTimer;
    void refreshFps(){
        fps = 1 / Time.deltaTime;
    }
    misc.CheckTimer slowTimer;
    void refreshSlowTimer(){
        maxFps = fpsList[0];
        minFps = fpsList[0];
        averageFps = 0;
        misc.forAll<float>( fpsList.ToArray(), ( fps, i)=>{
            if( fps > maxFps){
                maxFps = fps;
            }
            if( fps < minFps){
                minFps = fps;
            }
            averageFps += fps;
        });
        averageFps /= fpsList.Count;
        fpsList.Clear();
    }

    float fps;
    float maxFps = 0;
    float minFps = 0;
    float averageFps = 0;

    void Awake(){
        fpsTimer = new misc.CheckTimer( new misc.CheckTimerFunc( refreshFps ), 0.1f );
        slowTimer = new misc.CheckTimer( new misc.CheckTimerFunc( refreshSlowTimer ), 1f );
    }
    

    void Update(){
        fpsList.Add( 1 / Time.deltaTime);

        fpsTimer.checkTime();
        slowTimer.checkTime();
    }
    
    void OnGUI()
    {
        float posY = 10;
        float sizeY = 20;

        float nextPos(){
            float aux = posY;
            posY += sizeY - 5 ;
            return aux;
        }
        GUI.color = Color.black;
        GUI.Label(new Rect(10, nextPos(), 100, sizeY), "fps: " + fps.ToString());
        GUI.Label(new Rect(10, nextPos(), 100, sizeY), "max: " + maxFps.ToString());
        GUI.Label(new Rect(10, nextPos(), 100, sizeY), "min: " + minFps.ToString());
        GUI.Label(new Rect(10, nextPos(), 100, sizeY), "avg: " + averageFps.ToString());
    }
}
