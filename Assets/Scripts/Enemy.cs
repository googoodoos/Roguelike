using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour { 

    private Vector2 targetPosition;
    private Transform player;
    private Rigidbody2D rigidbody;

    public float smoothing = 5;
    public int lossFood = 10;
    public AudioClip attackAudio;

    private BoxCollider2D collider;
    private Animator animator;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rigidbody = GetComponent<Rigidbody2D>();
        targetPosition = transform.position;
        GameManager.Instance.enemyList.Add(this);
        collider = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        rigidbody.MovePosition(Vector2.Lerp(transform.position, targetPosition, smoothing * Time.deltaTime));
    }
	public void Move()
    {
        Vector2 offset = player.position - transform.position;
        if (offset.magnitude < 1.1f)
        {
            //攻击
            animator.SetTrigger("Attack");
            AudioManager.Instance.RandomPlay(attackAudio);
            player.SendMessage("TakeDamage", lossFood);
        }
        else
        {
            float x = 0, y = 0;
            if (Mathf.Abs(offset.y) > Mathf.Abs(offset.x))
            {
                //y轴移动
                if (offset.y < 0)
                {
                    y = -1;
                }
                else
                {
                    y = 1;
                }
            }
            else
            {
                //x轴移动
                if (offset.x < 0)
                {
                    x = -1;
                }
                else
                {
                    x = 1;
                }
            }
            collider.enabled = false;
            RaycastHit2D hit = Physics2D.Linecast(targetPosition, targetPosition + new Vector2(x, y));
            Debug.Log(targetPosition.y);


            collider.enabled = true;
            if (hit.transform == null)
            {
                targetPosition += new Vector2(x, y);
            }
            else
            {
                if (hit.collider.tag == "food" || hit.collider.tag == "soda")
                {
                    targetPosition += new Vector2(x, y);
                }
                else
                {
                    if(hit.collider.tag == "wall") {
                        animator.SetTrigger("Attack");
                        hit.collider.SendMessage("TakeDamage");
                        AudioManager.Instance.RandomPlay(attackAudio);
                    }
                    
                }
            }
        }
    }

}