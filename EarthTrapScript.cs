using UnityEngine;
using System.Collections;

public class EarthTrapScript : MonoBehaviour {

    [SerializeField]
    Collider2D leftSide;
    [SerializeField]
    Collider2D Center;
    [SerializeField]
    Collider2D rightSide;

    Vector2 LeftForce;
    Vector2 RightForce;

    public GameObject[] images;

    GameObject player;
    float damageTimer;
    float imageTimer;

	// Use this for initialization
	void Start ()
    {
        player = GameObject.Find("Warrior");
        LeftForce = new Vector2(11.0f, 0.0f);
        RightForce = new Vector2(-11.0f, 0.0f);
    }

    // Update is called once per frame
    void Update ()
    {
        if (damageTimer != 0.0f)
        {
            damageTimer += Time.deltaTime;

            if (damageTimer >= 1.0f)
                damageTimer = 0.0f;
        }

        imageTimer += Time.deltaTime;

        if (imageTimer >= 0.0f && imageTimer < 1.0f)
        {
            for (int i = 0; i < images.Length; i++)
            {
                images[i].GetComponent<SpriteRenderer>().enabled = false;
            }
        }
        else if (imageTimer >= 1.0f && imageTimer < 2.0f)
        {
            for (int i = 0; i < images.Length; i++)
            {
                images[i].GetComponent<SpriteRenderer>().enabled = true;
            }
        }
        else if (imageTimer >= 2.0f)
        {
            imageTimer = 0.0f;
        }

        if ((leftSide.IsTouching(player.GetComponent<Collider2D>())  && player.GetComponent<PlayerControl>().wuXing == PlayerControl.WUXING.Metal) ||
            (Center.IsTouching(player.GetComponent<Collider2D>())    && player.GetComponent<PlayerControl>().wuXing == PlayerControl.WUXING.Metal) ||
            (rightSide.IsTouching(player.GetComponent<Collider2D>()) && player.GetComponent<PlayerControl>().wuXing == PlayerControl.WUXING.Metal))
            Destroy(gameObject);
        else if (Center.IsTouching(player.GetComponent<Collider2D>()))
        {
            if (damageTimer == 0.0f && player.GetComponent<PlayerControl>().wuXing != PlayerControl.WUXING.Wood)
            {
                player.GetComponent<PlayerControl>().TakeDamage(25, PlayerControl.WUXING.Earth, gameObject);
                damageTimer += Time.deltaTime;
            }
        }
        else if (leftSide.IsTouching(player.GetComponent<Collider2D>()))
        {
            if (damageTimer == 0.0f && player.GetComponent<PlayerControl>().wuXing != PlayerControl.WUXING.Wood)
            {
                player.GetComponent<PlayerControl>().TakeDamage(5, PlayerControl.WUXING.Earth,gameObject);
                damageTimer += Time.deltaTime;
            }
            if (player.GetComponent<PlayerControl>().wuXing != PlayerControl.WUXING.Earth)
                player.GetComponent<PlayerControl>().PlayerRB.AddForce(LeftForce);
        }
        else if (rightSide.IsTouching(player.GetComponent<Collider2D>()))
        {
            if (damageTimer == 0.0f && player.GetComponent<PlayerControl>().wuXing != PlayerControl.WUXING.Wood)
            {
                player.GetComponent<PlayerControl>().TakeDamage(5, PlayerControl.WUXING.Earth,gameObject);
                damageTimer += Time.deltaTime;
            }
            if (player.GetComponent<PlayerControl>().wuXing != PlayerControl.WUXING.Earth)
                player.GetComponent<PlayerControl>().PlayerRB.AddForce(RightForce);
        }

    }
}
