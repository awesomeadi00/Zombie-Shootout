using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    public static ObjectPooler SharedInstance;                        //We have a variable of this class type called SharedInstance
    private List<GameObject> pooledObjects;                            //We create a list of all the objects we want to pool into
    [SerializeField] private GameObject objectToPool;                 //This is a gameobject reference to select objects from the pool    
    [SerializeField] private int amountToPool;                        //This is the size of the objects we want to pool. 

    //Awake is called when the script object is initialised, regardless of whether or not the script is enabled.
    //Hence, when the spawn manager object is initialized, then it will awaken the object pooler (aka 'this script')
    void Awake()
    {
        SharedInstance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        //Loop through list of pooled objects, deactivating them and adding them to the list 
        pooledObjects = new List<GameObject>();
        for (int i = 0; i < amountToPool; i++)
        {
            GameObject obj = (GameObject)Instantiate(objectToPool);
            obj.SetActive(false);
            pooledObjects.Add(obj);
            obj.transform.SetParent(this.transform); //Sets the object as children of Spawn Manager
        }
    }

    //This function gets all objects from the pooledObjects list and if they aren't active, we return that object. 
    public GameObject GetPooledObject()
    {
        //For as many objects as are in the pooledObjects list
        for (int i = 0; i < pooledObjects.Count; i++)
        {
            //If the pooled objects is NOT active in the scene, return that object 
            if (!pooledObjects[i].activeInHierarchy)
            {
                return pooledObjects[i];
            }
        }
        //Otherwise, return null   
        return null;
    }

}
