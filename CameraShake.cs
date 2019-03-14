using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{

    public Transform CameraTran;
    public float timer = 1.3f;
    float ShakeAmount = 0.15f;
    public bool Shaking = false;
    public Vector3 originalPos;
    [SerializeField]
    GameObject EarthEff;

    void Awake()
    {
        if (CameraTran == null)
        {
            CameraTran = GetComponent(typeof(Transform)) as Transform;
        }
    }

    void OnEnable()
    {
        originalPos = CameraTran.localPosition;
    }

    // Use this for initialization
    void Start()
    {
        gameObject.GetComponent<Collider2D>().enabled = false;

    }

    // Update is called once per frame
    void Update()
    {
        // if (originalPos != CameraTran.localPosition)
        // {
        // originalPos = CameraTran.localPosition;
        //
        // }
        if (Shaking)
        {
            if (timer > 0)
            {
                CameraTran.localPosition = originalPos + Random.insideUnitSphere * ShakeAmount;
                gameObject.GetComponent<Collider2D>().enabled = true;
                timer -= Time.deltaTime;
            }
            else
            {
                gameObject.GetComponent<Collider2D>().enabled = false;
                timer = 1.3f;
                CameraTran.localPosition = originalPos;
                Shaking = false;
            }
        }
    }

    // void ShakeCamera()
    // {
    //     if (Shaking)
    //     {
    //         if (timer > 0)
    //         {
    //             CameraTran.localPosition = originalPos + Random.insideUnitSphere * ShakeAmount;
    //             gameObject.GetComponent<Collider2D>().enabled = true;
    //
    //             timer -= Time.deltaTime;
    //         }
    //         else
    //         {
    //             gameObject.GetComponent<Collider2D>().enabled = false;
    //             timer = 1.5f;
    //             CameraTran.localPosition = originalPos;
    //             Shaking = false;
    //         }
    //     }
    // }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "MeleeEnemy" || col.tag == "RangedEnemy" || col.tag == "MiniBoss" || col.tag == "Boss")
        {
            col.GetComponent<SpriteRenderer>().color = Color.red;
            col.SendMessage("TakeDamage", 20);
            if (col)
            {

                GameObject st = GameObject.Instantiate(EarthEff) as GameObject;
                st.transform.parent = col.transform;
            }
        }
    }
}
