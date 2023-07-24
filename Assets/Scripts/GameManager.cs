using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*--- Como esta fuera de la clase principal de este archivo, lo que se escriba fuera puede ser usar por otro script ---*/
/*---Enumerados---*/
public enum GameState //"enum" es para enumerar y agrupar una serie de sentencias que tienen en comun para luego utilizarlas. Es como armar un array.
{
    menu, inGame, gameOver
}
/*---------------*/

public class GameManager : MonoBehaviour
{
    public GameState currentGameState = GameState.menu;

    public static GameManager sharedInstance; //"static" es que solo hay uno. "sharedInstance" es el nombre que le damos al singleton=unico en todo el proyecto. 
    
    private PlayerController controller;

    public int CollectedObject = 0;

    private void Awake()
    {
        if (sharedInstance == null)//No hay algun singleton llamado sharedInstance activado?
        {
            sharedInstance = this; //Esto hace que si hay mas de un sharedInstance se le de prioridad al primero que se ejecute.
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        controller = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if ((Input.GetButtonDown("Submit")) && (currentGameState != GameState.inGame))
        {
            StartGame();
        }
    }
    /*---------- ----------*/
    public void StartGame()
    {
            SetGameState(GameState.inGame);
    }

    public void GameOver()
    {
            SetGameState(GameState.gameOver);
    }

    public void BackToMenu()
    {
            SetGameState(GameState.menu);
    }

    private void SetGameState(GameState newGameState)
    {
        if (newGameState == GameState.menu)
        {   //TODO: Colocar la logica del juego
            MenuManager.sharedInstance.ShowMainMenu();
        }
        else if(newGameState == GameState.inGame)
        {   //TODO: Hay que preparar la escena para jugar
            LevelManager.sharedInstance.RemoveAllLevelBlocks();
            LevelManager.sharedInstance.GenerateInitialBlocks();
            controller.StartGame();

            MenuManager.sharedInstance.HideMainMenu();
        }
        else if (newGameState == GameState.gameOver)
        {//TODO: Preparar el juego apra el Game Over
            MenuManager.sharedInstance.ShowMainMenu();
        }

        this.currentGameState = newGameState;
    }

    public void CollectObject(Collectable collectable)
    {
        CollectedObject += collectable.value;
    }
}
