using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlateformeParent : MonoBehaviour
{
    PlatformEffector2D[] platforms;
    // Start is called before the first frame update
    void Start()
    {
        platforms = FindObjectsOfType<PlatformEffector2D>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void RotatePlatforms(int rot, int side)
    {
        foreach (PlatformEffector2D platform in platforms)
        {
            platform.rotationalOffset += rot;
            if(platform.GetComponent<PlateformInfo>().side == side)
            {
                platform.enabled = true;
            }
            else
            {
                platform.enabled = false;
            }
            
        }
    }
}
