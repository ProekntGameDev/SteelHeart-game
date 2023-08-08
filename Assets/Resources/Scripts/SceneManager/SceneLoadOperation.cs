using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class SceneLoadOperation : MonoBehaviour
{
    public readonly string Name;
    public readonly AsyncOperation Operation;

    public SceneLoadOperation(AsyncOperation operation, string name)
    {
        Name = name;
        
        Operation = operation;
    
    }
}