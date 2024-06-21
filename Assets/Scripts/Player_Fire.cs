using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player_Fire : MonoBehaviour
{
    public GameObject projectileParent;
    public GameObject prefabProjectile;

    float coolDown = 0.3f;
    bool canFire = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Fire()
    {
        StartCoroutine(CoolDown());
        GameObject instanceProjectile = Instantiate(prefabProjectile, projectileParent.transform);
        instanceProjectile.transform.rotation = transform.rotation;
        instanceProjectile.transform.position = transform.position;
        instanceProjectile.GetComponent<Projectile>().direction = (int)transform.localScale.x;
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if(canFire)
                Fire();
        }
    }

    IEnumerator CoolDown()
    {
        canFire = false;
        yield return new WaitForSeconds(coolDown);
        canFire = true;
    }
}
