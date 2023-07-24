using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float runningSpeed = 1.5f;

    public int enemyDamage = 20;


    Rigidbody2D rigidBody;

    public bool facingRight = false; //Por defecto el enemigo mira hacia la izquierda proque esta dibujado asi, por eso el false

    private Vector3 startPosition;

    void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        startPosition = this.transform.position;
    }

    // Start is called before the first frame update
    void Start()
    {
        this.transform.position = startPosition;
    }

    // Update is called once per frame
    void FixedUpdate() //En vez de usar Update usamos FizedUpdate porque como vamos a alterar las fisicas del juego, puede que se experimente un bajon de fps y no queremos que eso altere el juego. Esto se va a ejecutar 30 veces en 1 segundo real
    {
        float currentRunningSpeed = runningSpeed;

        /*------ Flip ------*/
        if (facingRight)
        {
            currentRunningSpeed = runningSpeed;
            this.transform.eulerAngles = new Vector3(0, 180, 0);
        }
        else
        {
            currentRunningSpeed = -runningSpeed;
            this.transform.eulerAngles = Vector3.zero;
        }
        /*------ ---- ------*/

        if (GameManager.sharedInstance.currentGameState == GameState.inGame)
        {
            rigidBody.velocity = new Vector2(currentRunningSpeed, rigidBody.velocity.y);

        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "coin")
        {
            return;
        }
        if (collision.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerController>().CollectHealth(-enemyDamage);
            return;
        }
        //Si colisiona con cualquier otra cosa, que se de vuelta
        facingRight = !facingRight;


        Debug.Log(collision.tag);
    }
}
