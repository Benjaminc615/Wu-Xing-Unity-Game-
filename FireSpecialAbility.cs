using UnityEngine;
using System.Collections;

public class FireSpecialAbility : MonoBehaviour
{

    private Vector2 direction;

    public GameObject subfireball;
    public GameObject Player;
    [SerializeField]
    PlayerControl.WUXING wuxing;

    [SerializeField]
    GameObject FireEff;

    void Start()
    {
        Player = GameObject.Find("Warrior");
        wuxing = PlayerControl.WUXING.Fire;
        direction = Vector2.right;
    }

    public void setDirection(Vector2 dir)
    {
        direction = dir;
        GetComponent<Rigidbody2D>().velocity = direction * 7.0f;
        Vector3 mousePosition = Input.mousePosition;
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, 0));

        transform.LookAt(worldPos, Vector3.forward);
        transform.rotation = Quaternion.Euler(new Vector3(0, 180, transform.rotation.eulerAngles.z + 90));

    }

    public void setWuXing(PlayerControl.WUXING _wuxing)
    {
        wuxing = _wuxing;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Player.GetComponent<PlayerControl>().FirstPress += 1;
        }
        remote();

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag != "Player")
        {
            if (other == null)
            {
                return;
            }
            if (other.tag == "Enemy" || other.tag == "MeleeEnemy" || other.tag == "RangedEnemy" || other.tag == "MiniBoss" || other.tag == "Boss")
            {

                other.SendMessage("TakeDamage", 45, SendMessageOptions.DontRequireReceiver);
                GameObject g = GameObject.Instantiate(FireEff) as GameObject;
                g.transform.parent = other.transform;
            }
            Explode();

            

        }
    }

    void Explode()
    {
        Vector2 position = new Vector2(transform.position.x, transform.position.y);

        for (int i = 0; i < 8; i++)
        {
            GameObject pj;
            Vector2 target = new Vector2(position.x + subfireball.GetComponent<CircleCollider2D>().radius * Mathf.Cos(i * (Mathf.PI) / 4), position.y + subfireball.GetComponent<CircleCollider2D>().radius * Mathf.Sin(i * (Mathf.PI) / 4));
            Vector2 direction = target - position;
            direction.Normalize();
            Quaternion rotation = Quaternion.Euler(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 90);
            pj = GameObject.Instantiate(subfireball, target, rotation) as GameObject;
            pj.SendMessage("setDirection", direction);
        }
        Player.GetComponent<PlayerControl>().FirstPress = 0;

        Destroy(gameObject);
    }

    void remote()
    {
       
        if (Player.GetComponent<PlayerControl>().FirstPress == 3 &&Input.GetKeyDown(KeyCode.Space))// Player.GetComponent<PlayerControl>().FirstPress == false &&  Input.GetKey(KeyCode.Space))
        {
            Explode();

            Destroy(gameObject);
        
        }
    }
}
