using UnityEngine;
using System.Collections;

public class WoodSpecialEff : MonoBehaviour
{

    SpriteRenderer spriteRenderer;
    Transform player;
    public Transform Enemy;
    [SerializeField]
    GameObject WoodEff;
    float timer;
    // Use this for initialization
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        StartCoroutine("playAnimation");
        StartCoroutine("StealLife");
        timer = 0;


    }

    IEnumerator playAnimation()
    {
        while (timer <= 4)
        {
            if (Enemy == null)
            {
                Destroy(gameObject);
            }
            spriteRenderer.flipX = !spriteRenderer.flipX;
            yield return new WaitForSeconds(0.1f);
        }

        Destroy(gameObject);

    }
    IEnumerator StealLife()
    {
        while (timer <= 4)
        {
            if (Enemy != null)
            {
            GameObject woodeff = (GameObject.Instantiate(WoodEff, Enemy.position, Quaternion.identity) as GameObject);
            woodeff.transform.parent = Enemy.transform;
            print(Enemy.name);

            Enemy.SendMessage("EffectDamage", 10);
            }

            yield return new WaitForSeconds(1f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (Enemy)
        {
            transform.localPosition = new Vector3(Enemy.transform.position.x, Enemy.transform.position.y, 1);
        }

    }
}
