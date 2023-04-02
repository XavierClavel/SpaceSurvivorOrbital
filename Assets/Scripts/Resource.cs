using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum type { violet, orange, green }

public class Resource : MonoBehaviour
{
    [SerializeField] type resourceType;
    int health = 3;

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "OrangeBullet")
        {
            health -= 1;
            if (health <= 0) Break();
        }
    }


    void Break()
    {
        SoundManager.instance.PlaySfx(transform, sfx.breakResource);
        switch (resourceType)
        {
            case type.violet:
                PlayerController.instance.IncreaseViolet();
                break;

            case type.orange:
                PlayerController.instance.IncreaseOrange();
                break;

            case type.green:
                PlayerController.instance.IncreaseGreen();
                break;
        }
        Destroy(gameObject);
    }
}
