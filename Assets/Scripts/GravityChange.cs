using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class GravityChange : MonoBehaviour
{
    public Player_Movements player;
    public PlateformeParent plateformeParent;

    public List<float> gravity = new List<float>(4) { 0, 90, 180, 270};

    Rigidbody2D[] rigidbodies;

    bool isRotating = false;

    public int side = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RotateMap(int rot)
    {
        if(isRotating) return;
        if(rot == 0) return;

        GetComponent<AudioSource>().Play();

        if(rot == 90 || rot == -90)
        {
            if (side == 0) side = 1;
            else if(side == 1) side = 0;
        }

        player.SetSimulatedRb(false);
        isRotating = true;
        StartCoroutine(RotateOverTime(rot, 0.3f));
        player.RotateWithMap(rot, 0.3f);
        rigidbodies = FindObjectsOfType<Rigidbody2D>();
        foreach (Rigidbody2D rb in rigidbodies)
        {
            rb.simulated = false;
        }
        
        //plateformeParent.RotatePlatforms(rot * -1, side);
        
    }

    IEnumerator RotateOverTime(int rot, float duration)
    {
        float finalRot = transform.rotation.eulerAngles.z + rot;

        // S'assurer que l'angle reste entre 0 et 360
        if (finalRot < 0) finalRot += 360;
        if (finalRot > 360) finalRot -= 360;

        yield return new WaitForSeconds(0.1f);

        float time = 0;
        float currentAngle = transform.rotation.eulerAngles.z;
        float smoothVelocity = 0.5f;

        while (Mathf.Abs(Mathf.DeltaAngle(currentAngle, finalRot)) > 0.1f)
        {
            time += Time.deltaTime;
            currentAngle = Mathf.SmoothDampAngle(currentAngle, finalRot, ref smoothVelocity, duration);
            transform.rotation = Quaternion.Euler(0, 0, currentAngle);
            yield return null;
        }

        // S'assurer que la rotation finale est exacte
        yield return new WaitForSeconds(0.1f);
        transform.rotation = Quaternion.Euler(0, 0, finalRot);
        isRotating = false;
        
        player.SetSimulatedRb(true);
        foreach (Rigidbody2D rb in rigidbodies)
        {
            if(rb)
                rb.simulated = true;
        }
    }

    public void OnRotDec90(InputAction.CallbackContext ctx) => RotateMap(/*-9*/0);
    public void OnRotAdd90(InputAction.CallbackContext ctx) => RotateMap(/*9*/0);
    public void OnRotDec180(InputAction.CallbackContext ctx) => RotateMap(-180);
    public void OnRotAdd180(InputAction.CallbackContext ctx) => RotateMap(180);
}
