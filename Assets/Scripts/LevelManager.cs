using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LevelManager : MonoBehaviour
{

    public static LevelManager sharedInstance;

    public List<LevelBlock> allTheLevelBlocks = new List<LevelBlock>();  //Todos los bloques que se pueden generar
    public List<LevelBlock> currentLevelBlocks = new List<LevelBlock>(); //Los bloques que estan generados
    public Transform levelStartPosition; //Donde se genera el primer bloque

    void Awake()
    {
        if(sharedInstance == null)
        {
            sharedInstance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        GenerateInitialBlocks();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /*----- Creation and destructions blocks -----*/
    public void AddLevelBlock()
    {
        int randomIdx = Random.Range(0, allTheLevelBlocks.Count); //Numero aleatorio entre la cantidad de estructura de bloques que tenemos .El "Count" cuenta la cantidad de elementos que hay

        LevelBlock block;

        Vector3 spawnPosition = Vector3.zero; //La posicion donde se van a espaunear los bloques

        if (currentLevelBlocks.Count == 0) //Si no hay bloques generados, vamos a generar un bloque simple asi arrancas en una plataforma facil
        {
            block = Instantiate(allTheLevelBlocks[0]); //"Instantiate" es el proceso de crear un nuevo objeto
            spawnPosition = levelStartPosition.position;
        }
        else
        {
            block = Instantiate(allTheLevelBlocks[randomIdx]);
            spawnPosition = currentLevelBlocks[currentLevelBlocks.Count - 1].endPoint.position;//En el ultimo elemento espawneado, en el exitPint, que se genere la proxima estructura/bloque
        }

        block.transform.SetParent(this.transform, false); //Todos los bloques que se generen, quedaran como hijos del LevelManager. El false es para que las transformaciones que tenga el padre, no las tenga el hijo

        Vector3 correction = new Vector3(spawnPosition.x - block.startPoint.position.x, spawnPosition.y - block.startPoint.position.y, 0); //Unimos los escenarios
        block.transform.position = correction;
        currentLevelBlocks.Add(block);
    }
    public void RemoveLevelBlock()
    {
        LevelBlock oldBlock = currentLevelBlocks[0];
        currentLevelBlocks.Remove(oldBlock); //Elimina el oldBlock de la lista
        Destroy(oldBlock.gameObject);        //Elimina de la pantalla
    }
    public void RemoveAllLevelBlocks()
    {
        while (currentLevelBlocks.Count > 0)
        {
            RemoveLevelBlock();
        }
    }
    public void GenerateInitialBlocks()
    {
        for(int i=0; i<2; i++)
        {
            AddLevelBlock();
        }
    }

}
