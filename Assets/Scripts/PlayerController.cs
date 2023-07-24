using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    //Variables del movimiento del personaje
    /*--- Motion ---*/
    public float jumpForce = 6f;
    public float runningSpeed = 2f;
    float xInput;

    /*--- Physics and animation ---*/
    Rigidbody2D rigidBody;
    Animator animator;
    Vector3 startPosition; //Se guarda la posicion inicial cada vez que se inicia el juego

    /*--- Booleans impacts ---*/
    private const string STATE_ALIVE = "isALive";
    private const string STATE_ON_THE_GROUND = "isOnTheGround";
    
  
    public LayerMask groundMask;

    /*---------- health and mana----------*/
    [SerializeField] //Hace que podamos visualizar en unity las variables privadas
    private int healthPoints, manaPoints;

    public const int INITIAL_HEALTH = 100,
        MAX_HEALTH = 200, 
        MIN_HEALTH = 10, 
        INITIAL_MANA = 15, 
        MAX_MANA = 30, 
        MIN_MANA = 0;

    public const int SUPERJUMP_COST = 5;
    public const float SUPERJUMP_FORCE = 1.5f;

    /*---- raycast ----*/
    public float raycastGroundDistance = 1.5f;


    void Awake()//Esto se inicializa en el frame 0 del juego; antes de que arranque el frame 1 del juego.
    {
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        startPosition = this.transform.position; //Se guarda la posicion inicial cada vez que se inicia el juego
    }

    public void StartGame()
    {
        animator.SetBool(STATE_ALIVE, true);
        animator.SetBool(STATE_ON_THE_GROUND, false);

        healthPoints = INITIAL_HEALTH;
        manaPoints = INITIAL_MANA;

        Invoke("RestartPosition", 0.2f); //Retrasa la reaparicion del personaje para que no se vea la animacion del revivir.
    }

    void RestartPosition()
    {
        this.transform.position = startPosition;
        this.rigidBody.velocity = Vector2.zero;

        GameObject mainCamera = GameObject.Find("Main Camera"); //Agarramos la camara de la escena y la reseteamos a la posicion inicial
        mainCamera.GetComponent<CameraFollow>().ResetCameraPosition(); 
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate() // Tiene una ejecucion constante sin importar los bajones de fps. Es un reloj con ejecucion constante SIEMPRE
    {
        /*-------------------- Walk --------------------*/
        if (GameManager.sharedInstance.currentGameState == GameState.inGame)
        {
            if ((rigidBody.velocity.x <= runningSpeed)/*&&IsTouchingTheGround()*/)
            {                                 //Eje x           Eje y
                //rigidBody.velocity = new Vector2(runningSpeed, rigidBody.velocity.y);
                playerMotion();
            }
        }
        else
        { //If is not indicate that we are in play, We don't have move
            rigidBody.velocity = new Vector2(0, rigidBody.velocity.y);
        }
        /*-------------------- ---- --------------------*/
        Flip();
        /*-------------------- JUMP --------------------*/
        //El GetKey=si mantenes pulsado te lo toma varias veces. GetKeyDown=Te lo toma solo la pulsacion hacia abajo del ciclo
        if (Input.GetButtonDown("Jump"))//(Input.GetKeyDown(KeyCode.Space)) || (Input.GetMouseButtonDown(0)) //0=clickIzquierzo, 1=clickDerecho, 2=clickRueda
        {
            Jump(false);
        }

        if (Input.GetButtonDown("SuperJump"))
        {
            Jump(true);
        }
        /*-------------------- ---- --------------------*/

        animator.SetBool(STATE_ON_THE_GROUND, IsTouchingTheGround());


        //Hace referencia al centro del personaje
        Debug.DrawRay(this.transform.position, Vector2.down * raycastGroundDistance, Color.red); //Raycast desde el personaje para ver si esta en suelo
    }

    void Jump(bool superjump)
    {
        float jumpForceFactor = jumpForce;

        if (superjump && (manaPoints >= SUPERJUMP_COST))
        {
            manaPoints -= SUPERJUMP_COST;
            jumpForceFactor *= SUPERJUMP_FORCE;
        }

        if (IsTouchingTheGround()&&GameManager.sharedInstance.currentGameState == GameState.inGame)
        {
            GetComponent<AudioSource>().Play();
            rigidBody.AddForce(Vector2.up * jumpForceFactor, ForceMode2D.Impulse); //Force = fuerza constante. Impulse = fuerza aplicada un unico instante (En  este caso usamos impulse)
        }    
        
    }

    bool IsTouchingTheGround()
    {
        if(Physics2D.Raycast(this.transform.position, Vector2.down, raycastGroundDistance, groundMask)) //this=este objeto. transform=posicion, rotacion y escala, queremos la posicion. Vector2.down=Queremos el fector vertical hacia abajo. distancia maxima del Raycast de 2f(200centimetros) pd: es desde el centro del personaje. Que se tenga en cuenta la capa groundMask
        {   // TODO: Programar lógica de contacto con el suelo.
            return true;
        }
        else
        {   //TODO: Programar lógica de no contacto con el suelo.
            return false;
        }
    }

    void playerMotion()
    {
        xInput = Input.GetAxisRaw("Horizontal");
        Vector2 move = new Vector2(xInput * runningSpeed, rigidBody.velocity.y);
        rigidBody.velocity = move;
    }
    void Flip()
    {
        if ((xInput > 0) && (transform.localScale.x < 0))
        {
            transform.localScale = new Vector2(x: 1, y: 1);
        }
        else if((xInput < 0) && (transform.localScale.x > 0))
        {
            transform.localScale = new Vector2(x: -1, y: 1);
        }
    }

    public void Die()
    {
        float travelledDistance = GetTravelledDistance(); //Primer parametro: Accedemos a la variable que Unity la tiene ya configurada llamada "maxscore".Segundo parametro: El valor por defecto si nadie lo configuro previamente
        float previousMaxDistance = PlayerPrefs.GetFloat("maxscore", 0);   //PlayerPrefs = Almacena y accede a preferencias de jugador entre seciones del videojuego
        if (travelledDistance > previousMaxDistance)
        {
            PlayerPrefs.SetFloat("maxscore", travelledDistance);
        }
        
        this.animator.SetBool(STATE_ALIVE, false);
        GameManager.sharedInstance.GameOver();
    }

    public void CollectHealth(int points)
    {
        if (this.healthPoints >= MAX_HEALTH)
        {
            this.healthPoints = MAX_HEALTH;
        }
        if (this.healthPoints <= 0)
        {
            Die();
        }
        else
        {
            this.healthPoints += points;
        }
    }
    public void CollectMana(int points)
    {
        if (this.manaPoints >= MAX_MANA)
        {
            this.manaPoints = MAX_MANA;
        }
        else
        {
            this.manaPoints += points;
        }
    }

    public int GetHealth()
    {
        return healthPoints;
    }
    public int GetMana()
    {
        return manaPoints;
    }

    public float GetTravelledDistance()
    {
        return this.transform.position.x - startPosition.x;
    }
}
