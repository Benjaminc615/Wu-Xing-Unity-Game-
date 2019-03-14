using UnityEngine;
using System.Collections;

public class WoodSeedScript : MonoBehaviour
{
    public GameObject miniBoss;
    public GameObject boss;
    public GameObject player;
    BoxCollider2D Collider;
    Transform trans;
    SpriteRenderer sr;
    [SerializeField]
    Sprite woodSeed;
    float duration;

    // Use this for initialization
    void Start ()
    {
        player = GameObject.Find("Warrior");
        Collider = GetComponent<BoxCollider2D>();
        trans = GetComponent<Transform>();
        sr = GetComponent<SpriteRenderer>();
        trans.localScale = new Vector3(trans.localScale.x, 0.1f);
        Collider.enabled = false;

        duration = 0.0f;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (player.GetComponent<PlayerControl>().Hp <= 0)
            Destroy(gameObject);

        if (duration < 2.5f)
            duration += Time.deltaTime;


        if (player.GetComponent<Collider2D>().IsTouching(Collider))
        {
            player.GetComponent<PlayerControl>().TakeDamage(15.0f, PlayerControl.WUXING.Wood, gameObject);

            if (boss != null)
            boss.GetComponent<BossScript>().GainLife(30.0f);
            else if (miniBoss != null)
            miniBoss.GetComponent<MiniBossScript>().GainLife(30.0f);
            Destroy(gameObject);
        }

        if (duration >= 2.5f)
        {
            sr.sprite = woodSeed;
            Collider.enabled = true;
            trans.localScale = new Vector3(0.1f, 0.08f);
            //trans.position = new Vector3(trans.position.x, 0.5f);
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "PlayerProjectile")
            Destroy(gameObject);
    }
}
