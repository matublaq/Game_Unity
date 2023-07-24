using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; //Player que vamos a seleccionar
    
    public Vector3 offset = new Vector3( 0.2f, 0, -10); //Posicion de la camara
    public float dampingtime = 0.3f; //Es lo que la camara tarda en seguir al personaje (Efecto cine)
    public Vector3 velocity = Vector3.zero;

    void Awake()
    {
        Application.targetFrameRate = 60; //Forzamos a que vaya a 60fps
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        MoveCamera(true);
    }

    public void ResetCameraPosition()
    {
        MoveCamera(false);
    }
    void MoveCamera(bool smooth) //Efecto cine true o false
    { //target=player
        Vector3 destination = new Vector3(target.position.x + offset.x, target.position.y + offset.y, offset.z);
        if (smooth == true)
        {
            this.transform.position = Vector3.SmoothDamp(
                this.transform.position, //Posicion inicial de la camara actual
                destination,             //Objetivo a donde queremos ir
                ref velocity,            //"ref"= Podes pasar por referencia a orto script, que se calcule, y luego lo devuelva y usarlo. (En este caso usaremos un script de unity (SmoothDamp())) 
                dampingtime);            //Tiempo de la animacion (SmoothTime)           
        }
        else
        {
            this.transform.position = destination;
        }
    }
}
