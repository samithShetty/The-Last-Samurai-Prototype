using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

public class RailingController : MonoBehaviour
{
    private PathCreator pathCreator;
    public EndOfPathInstruction endOfPathInstruction;

    private void Awake() {
        pathCreator = GetComponent<PathCreator>();    
    }
}
