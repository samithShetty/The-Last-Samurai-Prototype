using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RubbleController : MonoBehaviour
{
    [SerializeField] float lifespan; // Seconds until rubble is deleted

    // Start is called before the first frame update
    void Start()
    {  
        StartCoroutine(DeleteRubble());
    }

    IEnumerator DeleteRubble() 
    {
        yield return new WaitForSeconds(lifespan);
        Destroy(gameObject);
    }

}
