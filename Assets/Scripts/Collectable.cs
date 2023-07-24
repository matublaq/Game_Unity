using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;

/*---Enumerados---*/
public enum CollectableType{
        healthPotion, manaPotion, money
}
/*----------------*/

public class Collectable : MonoBehaviour
{
    public CollectableType Type = CollectableType.money; //Por defecto todo sera una moneda

    private SpriteRenderer sprite;
    private CircleCollider2D itemCollider;
    bool hasBeenCollected = false; //Fue colectada?

    public int value = 1;


    GameObject player;

    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        itemCollider = GetComponent<CircleCollider2D>();
    }

    void Start()
    {
        player = GameObject.Find("Player"); //El GameObject es dinstinto al gameObject. El GameObject se fija en los objetos de la escena. gameObject es el objeto asociado al script
        /*GameObject player = GameObject.Find("Player"); //Como PlayerController no es un singleton, podemos acceder a el de esta forma (Accedemos por la escena)*/
    }

    void Show()
    {
        itemCollider.enabled = true;
        sprite.enabled = true;
        hasBeenCollected = false;
    }
    void Hide()
    {
        itemCollider.enabled = false;
        sprite.enabled = false;
    }
    void Collect()
    {
        Hide();
        hasBeenCollected = true;

        switch (this.Type)
        {
            case CollectableType.money:

                GameManager.sharedInstance.CollectObject(this);
                GetComponent<AudioSource>().Play();
                break;

            case CollectableType.healthPotion:

                player.GetComponent<PlayerController>().CollectHealth(this.value);

                break;

            case CollectableType.manaPotion:

                player.GetComponent<PlayerController>().CollectMana(this.value);

                break;
        }
    }

    /*
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }*/

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            Collect();
        }
    }
}
