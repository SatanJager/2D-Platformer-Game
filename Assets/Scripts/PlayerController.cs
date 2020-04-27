using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private float mySpeedX;
    [SerializeField] float speed; //private oluyor ama unity içinden ulaşılıyor
    [SerializeField] float jumpPower;
    private Rigidbody2D myBody;
    private Vector3 defaultLocalScale;
    public bool onGround; //player zemin üzerinde mi değil mi?
    private bool canDoubleJump;
    [SerializeField] GameObject arrow;
    [SerializeField] bool attacked;
    [SerializeField] float currentAttackTimer;
    [SerializeField] float defaultAttackTimer;
    private Animator myAnimator;
    [SerializeField] int arrowNumber; //atılacak ok sayısı
    [SerializeField] Text arrowNumberText;
    [SerializeField] AudioClip dieMusic; //ölme sesi
    [SerializeField] GameObject winPanel, losePanel;


    // Start is called before the first frame update
    void Start()
    {
        myBody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        defaultLocalScale = transform.localScale;
        attacked = false;
        arrowNumberText.text = arrowNumber.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(Input.GetAxis("Horizontal"));
        //Debug.Log("Frame Mantığı");

        mySpeedX = Input.GetAxis("Horizontal"); // -1 ve 1 arasında sağ ve sol ok tuşuna basılma süreine bağlı olarak değerler gelecek
        myAnimator.SetFloat("Speed", Mathf.Abs(mySpeedX)); //Idle-Run Animasyonu arasında geçişi yakalaması için kontrol

        myBody.velocity = new Vector2(mySpeedX * speed, myBody.velocity.y);

        #region Player'ın yüzünün sağ ve sola dönmesi
        if (mySpeedX >0)
        {
            transform.localScale = new Vector3(defaultLocalScale.x, defaultLocalScale.y, defaultLocalScale.z);
        }
        else if (mySpeedX <0)
        {
            transform.localScale = new Vector3(-defaultLocalScale.x, defaultLocalScale.y, defaultLocalScale.z);
        }
        #endregion

        #region Player zıplamasının kontrol edilmesi
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //Debug.Log("Boşluk tuşuna basıldı");
            if (onGround == true)
            {
                myBody.velocity = new Vector2(myBody.velocity.x, jumpPower);
                canDoubleJump = true;
                myAnimator.SetTrigger("Jump");
            }
            else
            {
                if (canDoubleJump == true)
                {
                    myBody.velocity = new Vector2(myBody.velocity.x, jumpPower);
                    canDoubleJump = false;
                }
            }
        }
        #endregion

        #region Player'ın ok atmasının kontrolü
        if (Input.GetKeyDown(KeyCode.LeftControl) && arrowNumber > 0)
        {
            if (attacked == false)
            {
                attacked = true;
                myAnimator.SetTrigger("Attack");
                Invoke("Fire", 0.5f);
            }
        }
        #endregion

        #region Arka arkaya ok atma kontrolü
        if (attacked == true)
        {
            currentAttackTimer -= Time.deltaTime;       //1000/40 = 25 ms => 0.025 sn
        }
        else
        {
            currentAttackTimer = defaultAttackTimer;
        }

        if (currentAttackTimer <= 0)
        {
            attacked = false;
        }
        #endregion
    }

    void Fire()
    {
        GameObject okumuz = Instantiate(arrow, transform.position, Quaternion.identity);
        okumuz.GetComponent<Rigidbody2D>().velocity = new Vector2(5f, 0f);
        okumuz.transform.parent = GameObject.Find("Arrows").transform;

        if (transform.localScale.x > 0)
        {
            okumuz.GetComponent<Rigidbody2D>().velocity = new Vector2(5f, 0f);
        }
        else
        {
            Vector3 okumuzScale = okumuz.transform.localScale;
            okumuz.transform.localScale = new Vector3(-okumuzScale.x, okumuzScale.y, okumuzScale.z);
            okumuz.GetComponent<Rigidbody2D>().velocity = new Vector2(-5f, 0f);
        }

        arrowNumber--;
        arrowNumberText.text = arrowNumber.ToString();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log(collision.gameObject.name);
        if (collision.gameObject.CompareTag("Enemy"))
        {
            GetComponent<TimeControl>().enabled = false;
            Die();
        }
        else if (collision.gameObject.CompareTag("Finish"))
        {
            Destroy(collision.gameObject);
            GetComponent<TimeControl>().enabled = false;
            StartCoroutine(Wait(true));
        }
    }

    public void Die()
    {
        GameObject.Find("SoundController").GetComponent<AudioSource>().clip = null;
        GameObject.Find("SoundController").GetComponent<AudioSource>().PlayOneShot(dieMusic);
        myAnimator.SetFloat("Speed", 0);
        myAnimator.SetTrigger("Die");
        myBody.constraints = RigidbodyConstraints2D.FreezeAll;
        enabled = false;  //PlayerController scriptini disable yapıyoruz.

        StartCoroutine(Wait(false));

    }

    IEnumerator Wait(bool win)
    {
        yield return new WaitForSecondsRealtime(2f);
        Time.timeScale = 0;
        if (win == true)
        {
            winPanel.SetActive(true); //winpanel.active = true;
        }
        else
        {
            losePanel.SetActive(true);       //losePanel.active = true;
        }

        
    }

}
