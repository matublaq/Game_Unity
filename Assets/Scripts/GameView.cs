using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameView : MonoBehaviour
{
    public TextMeshProUGUI coinsText, scoreText, maxScoreText;

    private PlayerController controller;

    // Start is called before the first frame update
    void Start()
    {
        controller = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.sharedInstance.currentGameState == GameState.inGame)
        {
            int coins = GameManager.sharedInstance.CollectedObject;
            float score = controller.GetTravelledDistance();
            float maxScore = PlayerPrefs.GetFloat("maxscore", 0); //Accedemos a los datos en distintas seciones con el "PlayerPrefs"

            coinsText.text = coins.ToString(); //"ToString()" convierte el valor a string
            scoreText.text = "Score: " + score.ToString("f1"); //"f1" es para que muestre solo un decimal (3.1416... = 3.1)
            maxScoreText.text = "Max score: " + maxScore.ToString("f1");
        }
    }
}
