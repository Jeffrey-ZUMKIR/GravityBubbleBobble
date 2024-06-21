using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player_Movements : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rb;
    private Collider2D collider;

    public Transform groundCheck;
    public LayerMask groundLayer;
    public LayerMask platformLayer;

    private float direction;
    public float speed;
    public float jumpForce;

    public float coyoteTime = 0.2f; // Durée du coyote time
    public float bubbleJumpTime = 0.2f;
    private float coyoteTimeCounter;
    private float coyoteTimeCounterBubble;

    public bool isGrounded;
    public bool onPlatform = false;
    public bool isFallingThrough = false;
    public bool isTping = false;
    public bool isJumping = false;

    public bool canBubbleJump = false;

    public float maxSpeed;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        collider = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckGrounded();

        Move();

        if (Mathf.Abs(rb.velocity.magnitude) > maxSpeed)
        {
            maxSpeed = rb.velocity.magnitude;
        }

        if(isGrounded)
            coyoteTimeCounter = coyoteTime;
        else
            coyoteTimeCounter -= Time.deltaTime;

        if (canBubbleJump)
            coyoteTimeCounterBubble = bubbleJumpTime;
        else
            coyoteTimeCounterBubble -= Time.deltaTime;
    }

    //Fonction pour déplacer le joueur avec la vitesse donnée
    public void Move()
    {
        rb.velocity = new Vector2(direction * speed, rb.velocity.y);
        if(direction != 0)
        {
            anim.SetBool("isRunning", true);
            if(direction > 0)
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
            else
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
        }
        else
        {
            anim.SetBool("isRunning", false);
        }

    }

    //Fonction pour sauter
    public void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }

    public void OnMove(InputAction.CallbackContext ctx) => direction = ctx.ReadValue<float>();

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (isGrounded || coyoteTimeCounter > 0)
            {
                Jump();
            }
            else if (canBubbleJump || coyoteTimeCounterBubble > 0)
            {
                Debug.Log("Jumping from bubble");
                Jump();
            }
                
        }
    }

    public void OnFallThrough(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            if(onPlatform)
            {
                collider.enabled = false;
                StartCoroutine(EnableCollider());
                /*Physics2D.IgnoreLayerCollision(8, 9, true);
                StartCoroutine(ResetFallThrough());*/
            }
        }
    }

    IEnumerator EnableCollider()
    {
        yield return new WaitForSeconds(0.2f);
        collider.enabled = true;
    }

    void CheckGrounded()
    {
        // Vérifie si le personnage est au sol en utilisant un cercle de chevauchement
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer);

        if(!isGrounded)
        {
            isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.1f, platformLayer);
            onPlatform = isGrounded;
        }
    }

    public void OnTp()
    {
        isTping = true;
        StartCoroutine(Teleport());
    }

    IEnumerator Teleport()
    {
        yield return new WaitForSeconds(0.1f);
        isTping = false;
    }

    public void SetSimulatedRb(bool value) => rb.simulated = value;

    public void RotateWithMap(int rot, float duration)
    {
        StartCoroutine(Rotate(duration));
    }

    IEnumerator Rotate(float duration)
    {
        yield return new WaitForSeconds(0.2f);
        float time = 0;
        float currentAngle = transform.rotation.eulerAngles.z;
        float smoothVelocity = 0.5f;

        while (Mathf.Abs(Mathf.DeltaAngle(currentAngle, 0)) > 0.1f)
        {
            time += Time.deltaTime;
            currentAngle = Mathf.SmoothDampAngle(currentAngle, 0, ref smoothVelocity, duration);
            transform.rotation = Quaternion.Euler(0, 0, currentAngle);
            yield return null;
        }

        // S'assurer que la rotation finale est exacte
        yield return new WaitForSeconds(0.1f);

        transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Spike")
        {
            //reload scene
            UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        }
    }
}
