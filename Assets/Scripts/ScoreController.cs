using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreController : MonoBehaviour
{
    [SerializeField] Text scoreValueText;
    [SerializeField] float coinRotateSpeed;


    private void Update()
    {
        transform.Rotate(new Vector3(0f, coinRotateSpeed ,0));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            /*İLK HALİ
            int scoreValue = int.Parse(scoreValueText.text);
            scoreValue += 50;
            scoreValueText.text = scoreValue.ToString();*/

            GameObject.Find("LevelManager").GetComponent<LevelManager>().AddScore(50);

            Destroy(gameObject);
        }
    }
}
