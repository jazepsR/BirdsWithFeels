using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class setTargetGameObjectEnabled : MonoBehaviour
{
    public GameObject GameObjectToDisable;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DisableGameObject()
    {
        GameObjectToDisable.gameObject.SetActive(false);
    }
    public void EnableGameObject()
    {
        GameObjectToDisable.gameObject.SetActive(true);
    }
}
