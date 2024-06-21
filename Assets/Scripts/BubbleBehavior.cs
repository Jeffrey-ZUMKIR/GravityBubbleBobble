using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleBehavior : MonoBehaviour
{
    float playerSpeed;

    public int lifeTime;

    bool isPopping = false;

    public GameObject ennemi;

    public GameObject particleBubble;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(BubbleLife());
    }

    // Update is called once per frame
    void Update()
    {
        if (ennemi)
        {
            ennemi.transform.localPosition = Vector3.zero;
        }

        //léger mouvement de droite à gauche
        transform.position = new Vector3(transform.position.x + Mathf.Sin(Time.time * 2) * 0.0001f, transform.position.y, transform.position.z);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            playerSpeed = Mathf.Abs(collision.gameObject.GetComponent<Rigidbody2D>().velocity.magnitude);
            collision.gameObject.GetComponent<Player_Movements>().canBubbleJump = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            playerSpeed = 0;
            collision.gameObject.GetComponent<Player_Movements>().canBubbleJump = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            if(playerSpeed > 7)
            {
                PopChain();
                //Destroy(gameObject);
            }
        }
        else if(collision.gameObject.tag == "Spike")
        {
            PopChain();
            //Destroy(gameObject);
        }
    }

    IEnumerator BubbleLife()
    {
        yield return new WaitForSeconds(lifeTime);
        if (ennemi)
        {
            ennemi.transform.parent = GameObject.Find("EnnemiParent").transform;
            ennemi.GetComponent<Collider2D>().enabled = true;
            ennemi.GetComponent<Rigidbody2D>().gravityScale = 5;
        }
        isPopping = true;
        Instantiate(particleBubble, transform.position, Quaternion.identity);
        GetComponent<Collider2D>().enabled = false;
        GetComponent<SpriteRenderer>().enabled = false;
        AudioSource audioSource = GetComponent<AudioSource>();
        audioSource.pitch = Random.Range(1.0f, 1.3f);
        audioSource.Play();
        yield return new WaitForSeconds(0.2f);
        Destroy(gameObject);
    }

    public void PopChain()
    {
        if (!isPopping)
        {
            isPopping = true;
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 0.6f);
            foreach (Collider2D col in colliders)
            {
                if (col.gameObject.tag == "Bubble")
                {
                    StartCoroutine(popBubbleChain(col.gameObject));
                }
            }
            
        }

    }
    
    IEnumerator popBubbleChain(GameObject bubble)
    {
        yield return new WaitForSeconds(0.1f);
        if(bubble)
            bubble.GetComponent<BubbleBehavior>().PopChain();
        Instantiate(particleBubble, transform.position, Quaternion.identity);

        GetComponent<Collider2D>().enabled = false;
        GetComponent<SpriteRenderer>().enabled = false;
        AudioSource audioSource = GetComponent<AudioSource>();
        audioSource.pitch = Random.Range(1.0f, 1.3f);
        audioSource.Play();
        yield return new WaitForSeconds(0.2f);
        Destroy(gameObject);
    }
}
