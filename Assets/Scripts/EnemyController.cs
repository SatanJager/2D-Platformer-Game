using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] bool onGround; //enemy zemin üzerinde mi değil mi?
    [SerializeField] float speed; //enemy hız
    private float width; //enemy sprite eni
    private Rigidbody2D myBody;
    [SerializeField] LayerMask engel; //Layer: engel
    private static int totalEnemyNumber = 0;

    // Start is called before the first frame update
    void Start()
    {
        totalEnemyNumber++;
        //Debug.Log("Düşman ismi: " + gameObject.name + " oluştu." + "Oyundaki Toplam Düşman sayısı: " + totalEnemyNumber); //consolda enemy sayısını görüyoruz.
        width = GetComponent<SpriteRenderer>().bounds.extents.x;
        myBody = GetComponent<Rigidbody2D>();
    }

    //5 * (2, 2, 2) = (10, 10, 10) --> Vektörü çarparsam --> (1,0,0) (0,1,0) (0,0,1)
    
    // Update is called once per frame
    void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position + (transform.right * width/2), Vector2.down, 2f, engel);
        if (hit.collider != null)
        {
            onGround = true;
        }
        else
        {
            onGround = false;
        }

        Flip();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector3 enemyRealPosition = transform.position + (transform.right * width / 2);
        Gizmos.DrawLine(enemyRealPosition, enemyRealPosition + new Vector3(0, -2f, 0));
    }

    void Flip()
    {
        if (!onGround)
        {
            transform.eulerAngles += new Vector3(0, 180, 0);
        }
        myBody.velocity = new Vector2(transform.right.x * speed, 0f);
    }
}
