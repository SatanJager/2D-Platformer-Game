using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowController : MonoBehaviour
{
    [SerializeField] GameObject effect;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!(collision.gameObject.tag == "Player"))
        {
            Destroy(gameObject);
            if (collision.gameObject.CompareTag("Enemy"))
            {
                Destroy(collision.gameObject);

                GameObject.Find("LevelManager").GetComponent<LevelManager>().AddScore(100);

                Instantiate(effect, collision.gameObject.transform.position, Quaternion.identity);
            }
        }
    }
    private void OnBecameInvisible()
    {
        Destroy(gameObject);   
    }

}
