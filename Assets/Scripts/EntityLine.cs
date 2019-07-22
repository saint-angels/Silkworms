using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityLine : MonoBehaviour
{
    [SerializeField] private LineRenderer line = null;


    public void Init(Vector3 point1, Vector3 point2)
    {
        line.SetPosition(0, point1);
        line.SetPosition(1, point2);
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
