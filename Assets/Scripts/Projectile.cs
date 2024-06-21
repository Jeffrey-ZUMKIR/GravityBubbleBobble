using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public GameObject prefabBubble;
    public int speed;

    public int direction = 1;

    bool hasSpawned = false;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(MovingProjectile());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            
        }
        else
        {
            if (hasSpawned) return;
            hasSpawned = true;
            GameObject instanceBubble = Instantiate(prefabBubble, GameObject.Find("BubbleParent").transform);
            
            if (other.CompareTag("Ennemi"))
            {
                instanceBubble.transform.position = other.transform.position;
                other.GetComponent<Collider2D>().enabled = false;
                other.GetComponent<Rigidbody2D>().gravityScale = 0;
                other.transform.parent = instanceBubble.transform;
                other.transform.localPosition = new Vector3(0, 0, 0);
                instanceBubble.GetComponent<BubbleBehavior>().ennemi = other.gameObject;
            }
            else
            {
                instanceBubble.transform.position = transform.position;
            }
            
            Destroy(gameObject);
        }
    }

    IEnumerator MovingProjectile()
    {
        //pendant 6 secondes
        float time = 0;
        while (time < 30)
        {
            //avancer de 0.1 unité par seconde
            transform.position += transform.right * speed * Time.deltaTime * direction;
            time += Time.deltaTime;
            yield return null;
        }
    }
}
