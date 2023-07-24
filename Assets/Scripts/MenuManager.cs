using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{ //El singleton lleva static porque dice que va a estar activa durante todo el programa (Un unico menuManager para todo el programa)
    public static MenuManager sharedInstance;
    public Canvas menuCanvas;


    void Awake()
    {
        if(sharedInstance == null)
        {
            sharedInstance = this;
        }
    }
    /*---------- Show and hide a menu ----------*/
    public void ShowMainMenu()
    {
        menuCanvas.enabled = true; //El enable activa en este casi el canvas
    }
    public void HideMainMenu()
    {
        menuCanvas.enabled = false; //Aqui activamos el ocultamiento del canvas
    }
    /*---------- -------------------- ----------*/
    
    public void ExitGame()
    { //Acceder a sintaxis de los metodos que dependen de una plataforma. Se usa el "#"
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false; //Parar la emulacion de la aplicacion
        #else
            Application.Quit();
        #endif
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
