using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TP : MonoBehaviour
{
    public GameObject tpMirror;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Player_Movements player = collision.gameObject.GetComponent<Player_Movements>();
            if(player.isTping) return;
            collision.gameObject.transform.position = tpMirror.transform.position;
            player.OnTp();
        }
    }
}
