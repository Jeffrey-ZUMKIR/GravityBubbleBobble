using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateformInfo : MonoBehaviour
{
    public int side;
    // Start is called before the first frame update
    void Start()
    {
        float rotZ = transform.rotation.eulerAngles.z;
        if (rotZ == 0) side = 0;
        else side = 1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
