using UnityEngine;
using System.Collections;
using System;

public class PlayerControl : MonoBehaviour
{
    public GameObject Player;
    public Rigidbody2D PlayerRB;
    public float MovingSpeed;
    [SerializeField]
    private float hp = 100.0f;
    float oldMaxHPCount = 100;
    public float hpCount = 100;
    private const float HPOriginal = 100.0f;
    [SerializeField]
    private int energy;
    public bool IsAbleToAttack;
    public float damage;
    [SerializeField]
    private float damageOriginal = 25;
    [SerializeField]
    public bool isAlive;

    public LayerMask WhatIsGround;
    public LayerMask WhatIsPlatform;
    public float GroundPointRadius;
    public float JumpForce;
    public bool isGround;

    public bool isJumpping = false;

    public GameObject Melee;
    private Vector4 MelColor = new Vector4(1, 1, 1, 1);
    public LayerMask WhatIsEnemy;
    public LayerMask WhatIsBoss;

    public enum WUXING { Fire, Earth, Metal, Water, Wood, }
    public GameObject[] WuXingProject; //0:Fire, 1：Earth, 2：Metal, 3：Water, 4：Wood
    public WUXING wuXing;

    float ShotTimer = 0;
    private bool ShotInterval = true;

    [SerializeField]
    GameObject FireEff;
    [SerializeField]
    GameObject WoodEff;
    [SerializeField]
    GameObject WaterEff;
    [SerializeField]
    GameObject MetalEff;
    [SerializeField]
    GameObject EarthEff;

    private bool waterProtect = false;

    public GameObject[] SpecialAbility;
    public int FirstPress = 0;
    public bool isdodge; //waterspecialability
    public bool shield; //metalspecialability
    int prevEnergy;
    int metalSA;


    bool stunned;
    float stunDuration;
    float stunTimer;

    public AudioClip WalkAudio;
    public AudioClip SwitchAudio;
    public AudioClip JumpAudio;
    public AudioClip TouchingGroundAudio;
    public AudioClip ShootAudio;
    public AudioClip DamagedAudio;
    public AudioClip DeathAudio;
    public AudioClip MeleeAudio;
    float AudioTimer;
    public bool AudioPlaying;
    public ArmorConfig.ArmorItem[] armorItemList;
    private GameObject ArmorSYSTEM;
    private float playerArmor = 0;
    private AudioSource audioSource;
    public int RoomNumber;
    public string BradEMPType;

    public Animator anim;
    public RuntimeAnimatorController[] ElementSkin;

    public float Hp
    {
        get { return hp; }
        set { hp = value; }
    }

    public int Energy
    {
        get { return energy; }
        set { energy = value; }
    }

    void Start()
    {
        //DontDestroyOnLoad(transform.gameObject);
        Time.timeScale = 1;
        Player.layer = 12;
        isAlive = true;
        PlayerPrefs.SetString("SceneToRelive", UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        //PlayerTransform = this.transform;
        PlayerRB = GetComponent<Rigidbody2D>();
        GetComponent<Animator>().runtimeAnimatorController = ElementSkin[0];
        FirstPress = 0;
        ArmorSYSTEM = GameObject.Find("Armor System");
        damage = 25.0f;
        ArmorSYSTEM.SendMessage("PlayerRelive", gameObject);
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        FirstPress = 0;
        AudioTimer = 0;
        waterProtect = false;
        AudioPlaying = false;
        stunned = false;
        playerArmor = 0;
    }

    void SetRoomNumber(int _roomNumber)
    {
        RoomNumber = _roomNumber;
    }

    void PlayerReset(Vector3 playerPosition)
    {
        Player.layer = 12;
        FirstPress = 0;
        damage = 25.0f;
        ArmorSYSTEM.SendMessage("PlayerRelive", gameObject);
        audioSource = GetComponent<AudioSource>();
        FirstPress = 0;
        AudioTimer = 0;
        waterProtect = false;
        AudioPlaying = false;
        transform.position = playerPosition;
        transform.rotation = Quaternion.identity;
        oldMaxHPCount = hpCount = hp = 100;
        Energy = 0;
        isAlive = true;
        stunned = false;
        isJumpping = false;
        this.enabled = true;
        anim.SetBool("Dead", false);
        playerArmor = 0;
        GetComponent<SpriteRenderer>().color = Color.white;
    }

    public float GetDamage()
    {
        return damage;
    }

    void ArmorIni(object[] _armorItemList)
    {
        armorItemList = (ArmorConfig.ArmorItem[])_armorItemList;
    }
    void ArmorEff()
    {
        float hpArmorEff = 0;
        float damageArmorEff = 0;
        float playerArmorEff = 0;
        for (int i = 0; i < armorItemList.Length; i++)
        {
            if (armorItemList[i] != null && armorItemList[i].TYPE != ArmorConfig.ArmorType.NULL)
            {
                hpArmorEff += armorItemList[i].HP;
                damageArmorEff += armorItemList[i].DAMAGE;
                playerArmorEff += armorItemList[i].ARMOR;
            }
        }
        if (oldMaxHPCount != hpCount)
            hp += hpCount - oldMaxHPCount;

        oldMaxHPCount = hpCount;
        hpCount = HPOriginal + hpArmorEff;
        damage = damageArmorEff + damageOriginal;
        playerArmor = playerArmorEff;
        ArmorConfig.ArmorItem tmp = new ArmorConfig.ArmorItem();
        tmp.HP = (int)hpCount;
        tmp.DAMAGE = (int)damage;
        tmp.ARMOR = playerArmor;
        //ArmorSYSTEM.SendMessage("shouArmorVaul", tmp,SendMessageOptions.DontRequireReceiver);
    }
    // Update is called once per frame
    void Update()
    {
        if (hp <= 0)
            hp = 0;
        ShotTimer += Time.deltaTime;
        if (ShotTimer >= .7f)
        {
            ShotInterval = true;
            ShotTimer = 0;
        }
        //print(Input.GetAxis("Mouse ScrollWheel"));
        if (shield)
        {
            MetalEff.SetActive(true);
        }
        if (wuXing != WUXING.Metal || Energy <= 0)
        {
            MetalEff.SetActive(false);
        }


        if (energy <= 0)
            energy = 0;
        ArmorEff();
        GameObject[] emps = GameObject.FindGameObjectsWithTag("BradEMP");
        for (int i = 0; i < emps.Length; i++)
            if (!emps[i].GetComponent<Collider2D>().IsTouching(GetComponent<Collider2D>()))
                BradEMPType = "none";
        audioSource.volume = SoundInfo.soundMute ? 0 : SoundInfo.soundVolume;
        //print(Input.GetAxis("Mouse ScrollWheel"));
    }

    void FixedUpdate()
    {
        if (isAlive)
        {
            if (!shield)
            {
                if (IsAbleToAttack)
                {
                    float timer = 0;
                    timer += Time.deltaTime;
                    if (timer < 0.1f)
                    {
                        Energy += 1;
                        if (Energy >= 5)
                            Energy = 5;
                        IsAbleToAttack = false;
                    }

                }
            }
            else
                IsAbleToAttack = false;
            if (!stunned)
            {
                MelColor.w = 0;
                Melee.GetComponent<SpriteRenderer>().color = MelColor;
                Melee.GetComponent<Collider2D>().enabled = false;

                // always face to cursor
                FaceTotheCursor();

                //Movement
                MovementInput();

            }
            else if (stunned)
            {
                stunTimer += Time.deltaTime;
                GetComponent<SpriteRenderer>().color = Color.cyan;

                if (stunTimer >= stunDuration)
                {
                    GetComponent<SpriteRenderer>().color = Color.white;
                    stunned = false;
                    stunTimer = 0;
                }
            }

            SFX();
            if (Hp <= 0)
            {
                Hp = 0;
                isAlive = false;
            }
        }
        else if (!isAlive)
        {
            Player.layer = 25;

        }
    }

    private void FaceTotheCursor()
    {
        Vector2 MousePosition = Camera.main.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y));

        if (GetComponent<Transform>().position.x - MousePosition.x <= 0)
        {
            //face to right
            Player.GetComponent<SpriteRenderer>().flipX = false;
            Melee.GetComponent<SpriteRenderer>().flipX = false;
            Vector2 pos = Melee.GetComponent<Transform>().position;
            pos.x = PlayerRB.position.x + 0.4f;
            Melee.GetComponent<Transform>().position = pos;
            Melee.GetComponent<BoxCollider2D>().offset = new Vector2(0.1f, 0f);
        }
        else
        {
            //face to left

            Player.GetComponent<SpriteRenderer>().flipX = true;
            Melee.GetComponent<SpriteRenderer>().flipX = true;
            Vector2 pos = Melee.GetComponent<Transform>().position;
            pos.x = PlayerRB.position.x - 0.5f;
            Melee.GetComponent<Transform>().position = pos;
            Melee.GetComponent<BoxCollider2D>().offset = new Vector2(-0.1f, 0f);
        }
    }

    void MovementInput()
    {
        anim.SetFloat("MovingSpeed", Mathf.Abs(Input.GetAxis("Horizontal")));

        // press w to jump
        if (Input.GetKey(KeyCode.W))
        {
            if (PlayerRB.velocity.y == 0)
            {
                isJumpping = false;
            }
            if (isGround && !isJumpping)
            {
                PlayerRB.AddForce(new Vector2(0, JumpForce));

                isGround = false;
                isJumpping = true;
                anim.SetBool("IsGrounded", false);
            }
        }


        if (!isJumpping)
        {
            anim.SetBool("IsGrounded", true);
        }
        else
        {
            anim.SetBool("IsGrounded", false);
        }

        // press d toward to right
        if (Input.GetAxisRaw("Horizontal") > 0)
        {
            transform.Translate(Vector2.right * MovingSpeed * Time.deltaTime);
        }

        // press a toward to right
        if (Input.GetAxisRaw("Horizontal") < 0)
        {
            transform.Translate(Vector2.left * MovingSpeed * Time.deltaTime);
        }

        // left click to melee
        if (Input.GetMouseButtonDown(0))
        {
            if (isJumpping)
            {

                switch (wuXing)
                {
                    case WUXING.Metal:
                        Melee.GetComponent<SpriteRenderer>().color = Color.gray;
                        break;
                    case WUXING.Wood:
                        Melee.GetComponent<SpriteRenderer>().color = Color.green;
                        break;
                    case WUXING.Water:
                        Melee.GetComponent<SpriteRenderer>().color = Color.blue;
                        break;
                    case WUXING.Fire:
                        Melee.GetComponent<SpriteRenderer>().color = Color.red;
                        break;
                    case WUXING.Earth:
                        Melee.GetComponent<SpriteRenderer>().color = new Color(182 / 255f, 110 / 255f, 19f / 255f, 1);
                        break;
                    default:
                        break;
                }
            }
            anim.SetTrigger("Melee");
            Melee.GetComponent<Collider2D>().enabled = true;
            PlayerSFX(MeleeAudio);



        }


        if ((Input.GetMouseButtonDown(1)))
            if (Energy > 0)
            {
                if (ShotInterval)
                {
                    ShotInterval = false;
                    Energy -= 1;
                    anim.SetTrigger("RangedAttack");
                    Vector2 target = Camera.main.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
                    Vector2 position = new Vector2(transform.position.x, transform.position.y);
                    Vector2 direction = target - position;
                    direction.Normalize();
                    Quaternion rotation = Quaternion.Euler(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 90);
                    GameObject pj;
                    switch (wuXing)
                    {
                        case WUXING.Fire:
                            pj = GameObject.Instantiate(WuXingProject[0], position, rotation) as GameObject;
                            break;
                        case WUXING.Earth:
                            pj = GameObject.Instantiate(WuXingProject[1], position, rotation) as GameObject;
                            break;
                        case WUXING.Metal:
                            pj = GameObject.Instantiate(WuXingProject[2], position, rotation) as GameObject;
                            break;
                        case WUXING.Water:
                            pj = GameObject.Instantiate(WuXingProject[3], position, rotation) as GameObject;
                            break;
                        case WUXING.Wood:
                            pj = GameObject.Instantiate(WuXingProject[4], position, rotation) as GameObject;
                            break;
                        default:
                            pj = GameObject.Instantiate(WuXingProject[0], position, rotation) as GameObject;
                            break;
                    }
                    pj.SendMessage("setDirection", direction);
                    PlayerSFX(ShootAudio);
                }
            }
        if ((Input.GetKeyDown(KeyCode.Space)))
        {
            if (Energy == 5)
            {
                Energy -= 5;
                Vector2 target = Camera.main.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
                Vector2 position = new Vector2(transform.position.x, transform.position.y);
                Vector2 direction = target - position;
                direction.Normalize();
                Quaternion rotation = Quaternion.Euler(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 90);
                GameObject pj;

                switch (wuXing)
                {
                    case WUXING.Metal:
                        Energy = 5;
                        shield = true;
                        StartCoroutine("ShieldArmor");
                        break;
                    case WUXING.Wood:
                        pj = GameObject.Instantiate(SpecialAbility[1], position, rotation) as GameObject;
                        pj.GetComponent<WoodSpecialAbility>().Growing = true;
                        break;
                    case WUXING.Water:

                        pj = GameObject.Instantiate(SpecialAbility[2], position, rotation) as GameObject;
                        pj.GetComponent<WaterSpecialAbility>().player = gameObject;
                        isdodge = true;
                        break;
                    case WUXING.Fire:
                        if (FirstPress == 0)
                        {
                            FirstPress = 1;
                            pj = GameObject.Instantiate(SpecialAbility[0], position, rotation) as GameObject;
                            pj.GetComponent<FireSpecialAbility>().Player = gameObject;
                            pj.SendMessage("setDirection", direction);
                        }
                        break;
                    case WUXING.Earth:
                        pj = Camera.main.gameObject;
                        pj.GetComponent<CameraShake>().originalPos = Camera.main.gameObject.transform.localPosition;
                        pj.GetComponent<CameraShake>().Shaking = true;
                        break;
                    default:
                        break;

                }
                PlayerSFX(ShootAudio);
            }

        }


        //switch elements by numbers
        if (!ElementWheelBehavior.Instance.Turning)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                PlayerSFX(1.2f, 1, SwitchAudio);
                GetComponent<Animator>().runtimeAnimatorController = ElementSkin[0];
                WaterEff.SendMessage("DisableWaterEffect");
                wuXing = WUXING.Fire;
                ElementWheelBehavior.Instance.ChangedElement();
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                PlayerSFX(1.2f, 1, SwitchAudio);
                GetComponent<Animator>().runtimeAnimatorController = ElementSkin[1];
                WaterEff.SendMessage("DisableWaterEffect");
                wuXing = WUXING.Earth;
                ElementWheelBehavior.Instance.ChangedElement();
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                PlayerSFX(1.2f, 1, SwitchAudio);
                GetComponent<Animator>().runtimeAnimatorController = ElementSkin[2];
                wuXing = WUXING.Metal;
                WaterEff.SendMessage("DisableWaterEffect");
                waterProtect = true;
                ElementWheelBehavior.Instance.ChangedElement();
            }
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                PlayerSFX(1.2f, 1, SwitchAudio);
                GetComponent<Animator>().runtimeAnimatorController = ElementSkin[3];
                wuXing = WUXING.Water;
                WaterEff.SendMessage("EnableWaterEffect");
                ElementWheelBehavior.Instance.ChangedElement();
            }
            if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                PlayerSFX(1.2f, 1, SwitchAudio);
                GetComponent<Animator>().runtimeAnimatorController = ElementSkin[4];
                wuXing = WUXING.Wood;
                WaterEff.SendMessage("DisableWaterEffect");
                ElementWheelBehavior.Instance.ChangedElement();
            }

            //switch elements by scrollwheel
            //if (Input.GetAxis("Mouse ScrollWheel") > 0.05f)
            //{
            //    //if (wuXing == WUXING.Wood)
            //    //{
            //    //    wuXing = 0;
            //    //}
            //    //else
            //    //    wuXing += 1;
            //    //ElementWheelBehavior.Instance.ChangedElement();
            //}
            //if (Input.GetAxis("Mouse ScrollWheel") < -0.05f)
            //{
            //    //if (wuXing == WUXING.Fire)
            //    //{
            //    //    wuXing = (PlayerControl.WUXING)4;
            //    //}
            //    //else
            //    //    wuXing -= 1;
            //    //ElementWheelBehavior.Instance.ChangedElement();
            //}
        }
        //if (Input.GetAxis("Mouse ScrollWheel") < -0.05f)
        //{
        //    if (wuXing == WUXING.Fire)
        //    {
        //        wuXing = (PlayerControl.WUXING)4;
        //    }
        //    else
        //        wuXing -= 1;
        //    ElementWheelBehavior.Instance.ChangedElement();
        //}
        //}
    }




    //obj0 is damage obj1 is WUXING for SendMessage
    public void TakeDamage(object[] obj)
    {
        float damage = (float)obj[0];
        WUXING enemyElement = (WUXING)obj[1];
        GameObject enemy = (GameObject)obj[2];
        TakeDamage(damage, enemyElement, enemy);
    }

    public void AddEnergy(int _count)
    {
        energy += _count;
    }

    public void TakeDamage(float damage, WUXING enemyElement, GameObject _enemy)
    {
        damage = damage * (1 - (float)playerArmor / 100);

        if (wuXing == WUXING.Water && waterProtect)
        {
            Energy -= 1;
            waterProtect = false;
            WaterEff.SendMessage("DisableWaterEffect");
            return;
        }
        if (isdodge)   //Water Special Abilbty : have 50% dodge enemy's attack
        {
            System.Random rand = new System.Random();
            if (rand.Next(1, 100) <= 50)
            {
                return;
            }
        }
        switch (enemyElement)
        {
            case PlayerControl.WUXING.Fire:                 //Enemy  is Fire
                {
                    switch (wuXing)
                    {
                        case PlayerControl.WUXING.Fire:     //Player is Fire & Enemy is Fire
                            {
                                PlayerSFX(DamagedAudio);
                                Hp -= damage;
                                System.Random rand = new System.Random();
                                if (rand.Next(1, 100) <= 10)
                                {
                                    GameObject g = GameObject.Instantiate(FireEff) as GameObject;
                                    g.transform.parent = gameObject.transform;
                                }
                            }
                            break;
                        case PlayerControl.WUXING.Earth:    //Player is Earth & Enemy is Fire
                            {
                                PlayerSFX(DamagedAudio);
                                Hp -= (damage - (damage * 0.5f));
                                System.Random rand = new System.Random();
                                if (rand.Next(1, 100) <= 10)
                                {
                                    GameObject g = GameObject.Instantiate(FireEff) as GameObject;
                                    g.transform.parent = gameObject.transform;
                                }
                            }
                            break;
                        case PlayerControl.WUXING.Metal:    //Player is Metal & Enemy is Fire
                            {
                                PlayerSFX(DamagedAudio);
                                Hp -= (damage + (damage * 0.5f));
                                System.Random rand = new System.Random();
                                if (rand.Next(1, 100) <= 10)
                                {
                                    GameObject g = GameObject.Instantiate(FireEff) as GameObject;
                                    g.transform.parent = gameObject.transform;
                                }
                            }
                            break;
                        case PlayerControl.WUXING.Water:    //Player is Water & Enemy is Fire
                            {
                                PlayerSFX(DamagedAudio);
                                Hp -= (damage - (damage * 0.5f));
                                System.Random rand = new System.Random();
                                if (rand.Next(1, 100) <= 10)
                                {
                                    GameObject g = GameObject.Instantiate(FireEff) as GameObject;
                                    g.transform.parent = gameObject.transform;
                                }
                            }
                            break;
                        case PlayerControl.WUXING.Wood:     //Player is Wood & Enemy is Fire
                            {
                                PlayerSFX(DamagedAudio);
                                Hp -= (damage + (damage * 0.5f));
                                System.Random rand = new System.Random();
                                if (rand.Next(1, 100) <= 10)
                                {
                                    GameObject g = GameObject.Instantiate(FireEff) as GameObject;
                                    g.transform.parent = gameObject.transform;
                                }
                            }
                            break;
                    }
                }
                break;
            case PlayerControl.WUXING.Earth:              //Enemy is Earth
                {
                    switch (wuXing)
                    {
                        case PlayerControl.WUXING.Fire:   //Player is Fire & Enemy is Earth
                            {
                                PlayerSFX(DamagedAudio);
                                Hp -= (damage + (damage * 0.5f));
                                System.Random rand = new System.Random();
                                if (rand.Next(1, 100) <= 10)
                                {
                                    GameObject g = GameObject.Instantiate(FireEff) as GameObject;
                                    g.transform.parent = gameObject.transform;
                                }
                            }
                            break;
                        case PlayerControl.WUXING.Earth:  //Player is Earth & Enemy is Earth
                            {
                                PlayerSFX(DamagedAudio);
                                Hp -= (damage);
                                System.Random rand = new System.Random();
                                if (rand.Next(1, 100) <= 10)
                                {
                                    GameObject st = GameObject.Instantiate(EarthEff) as GameObject;
                                    st.transform.parent = gameObject.transform;
                                }
                            }
                            break;
                        case PlayerControl.WUXING.Metal:  //Player is Metal & Enemy is Earth
                            {
                                PlayerSFX(DamagedAudio);
                                Hp -= (damage - (damage * 0.5f));
                                System.Random rand = new System.Random();
                                if (rand.Next(1, 100) <= 10)
                                {
                                    GameObject st = GameObject.Instantiate(EarthEff) as GameObject;
                                    st.transform.parent = gameObject.transform;
                                }
                            }
                            break;
                        case PlayerControl.WUXING.Water:  //Player is Water & Enemy is Earth
                            {
                                PlayerSFX(DamagedAudio);
                                Hp -= (damage + (damage * 0.5f));
                                System.Random rand = new System.Random();
                                if (rand.Next(1, 100) <= 10)
                                {
                                    GameObject st = GameObject.Instantiate(EarthEff) as GameObject;
                                    st.transform.parent = gameObject.transform;
                                }
                            }
                            break;
                        case PlayerControl.WUXING.Wood:  //Player is Wood & Enemy is Earth
                            {
                                PlayerSFX(DamagedAudio);
                                Hp -= (damage - (damage * 0.5f));
                                System.Random rand = new System.Random();
                                if (rand.Next(1, 100) <= 10)
                                {
                                    GameObject st = GameObject.Instantiate(EarthEff) as GameObject;
                                    st.transform.parent = gameObject.transform;
                                }

                            }
                            break;
                    }
                }
                break;
            case PlayerControl.WUXING.Metal:              //Enemy is Metal
                {
                    switch (wuXing)
                    {
                        case PlayerControl.WUXING.Fire:   //Player is Fire & Enemy is Metal
                            {
                                PlayerSFX(DamagedAudio);
                                Hp -= (damage - (damage * 0.5f));
                                System.Random rand = new System.Random();
                                if (rand.Next(1, 100) <= 50)
                                {
                                    //Reflect Effect
                                }
                            }
                            break;
                        case PlayerControl.WUXING.Earth:  //Player is Earth & Enemy is Metal
                            {
                                PlayerSFX(DamagedAudio);
                                Hp -= (damage + (damage * 0.5f));
                                System.Random rand = new System.Random();
                                if (rand.Next(1, 100) <= 50)
                                {
                                    //Reflect Effect
                                }
                            }
                            break;
                        case PlayerControl.WUXING.Metal:  //Player is Metal & Enemy is Metal
                            {
                                PlayerSFX(DamagedAudio);
                                Hp -= (damage);
                                System.Random rand = new System.Random();
                                if (rand.Next(1, 100) <= 50)
                                {
                                    //Reflect Effect
                                }
                            }
                            break;
                        case PlayerControl.WUXING.Water:  //Player is Water & Enemy is Metal
                            {
                                PlayerSFX(DamagedAudio);
                                Hp -= (damage - (damage * 0.5f));
                                System.Random rand = new System.Random();
                                if (rand.Next(1, 100) <= 50)
                                {
                                    //Reflect Effect
                                }
                            }
                            break;
                        case PlayerControl.WUXING.Wood:  //Player is Wood & Enemy is Metal
                            {
                                PlayerSFX(DamagedAudio);
                                Hp -= (damage + (damage * 0.5f));
                                System.Random rand = new System.Random();
                                if (rand.Next(1, 100) <= 50)
                                {
                                    //Reflect Effect
                                }
                            }
                            break;
                    }
                }
                break;
            case PlayerControl.WUXING.Water:             ////////////////////////////////////Enemy is Water
                {
                    switch (wuXing)
                    {
                        case PlayerControl.WUXING.Fire:  //Player is Fire & Enemy is Water
                            {
                                PlayerSFX(DamagedAudio);
                                Hp -= (damage + (damage * 0.5f));
                                System.Random rand = new System.Random();
                                if (rand.Next(1, 100) <= 30)
                                {
                                    //Water Effect
                                }
                            }
                            break;
                        case PlayerControl.WUXING.Earth:  //Player is Earth & Enemy is Water
                            {
                                PlayerSFX(DamagedAudio);
                                Hp -= (damage - (damage * 0.5f));
                                System.Random rand = new System.Random();
                                if (rand.Next(1, 100) <= 30)
                                {
                                    //Water Effect
                                }
                            }
                            break;
                        case PlayerControl.WUXING.Metal:  //Player is Metal & Enemy is Water
                            {
                                PlayerSFX(DamagedAudio);
                                Hp -= (damage + (damage * 0.5f));
                                System.Random rand = new System.Random();
                                if (rand.Next(1, 100) <= 30)
                                {
                                    //Water Effect
                                }
                            }
                            break;
                        case PlayerControl.WUXING.Water:  //Player is Water & Enemy is Water
                            {
                                PlayerSFX(DamagedAudio);
                                Hp -= (damage);
                                System.Random rand = new System.Random();
                                if (rand.Next(1, 100) <= 30)
                                {
                                    //Water Effect
                                }
                            }
                            break;
                        case PlayerControl.WUXING.Wood:  //Player is Wood & Enemy is Water
                            {
                                PlayerSFX(DamagedAudio);
                                Hp -= (damage + (damage * 0.5f));
                                System.Random rand = new System.Random();
                                if (rand.Next(1, 100) <= 30)
                                {
                                    //Water Effect
                                }
                            }
                            break;
                    }
                }
                break;
            case PlayerControl.WUXING.Wood:               ////////////////////////////////////////////////////////////////////////Enemy is Wood
                {
                    switch (wuXing)
                    {
                        case PlayerControl.WUXING.Fire:   //Player is Fire & Enemy is Wood
                            {
                                PlayerSFX(DamagedAudio);
                                Hp -= (damage - (damage * 0.5f));
                                // if (_enemy.GetComponent<MeleeEnemyScript>().health >= 75)
                                // {
                                //     System.Random rand = new System.Random();
                                //     if (rand.Next(1, 100) <= 5)
                                //     {
                                //         _enemy.GetComponent<MeleeEnemyScript>().AddHP(10);
                                //     }
                                // }
                                //
                                // else if (_enemy.GetComponent<MeleeEnemyScript>().health >= 50 && _enemy.GetComponent<MeleeEnemyScript>().health < 75)
                                // {
                                //     System.Random rand = new System.Random();
                                //     if (rand.Next(1, 100) <= 10)
                                //     {
                                //         _enemy.GetComponent<MeleeEnemyScript>().AddHP(15);
                                //     }
                                // }
                                // else if (_enemy.GetComponent<MeleeEnemyScript>().health >= 25 && _enemy.GetComponent<MeleeEnemyScript>().health < 50)
                                // {
                                //     System.Random rand = new System.Random();
                                //     if (rand.Next(1, 100) <= 15)
                                //     {
                                //         _enemy.GetComponent<MeleeEnemyScript>().AddHP(20);
                                //     }
                                // }
                                // else if (_enemy.GetComponent<MeleeEnemyScript>().health < 25)
                                // {
                                //     System.Random rand = new System.Random();
                                //     if (rand.Next(1, 100) <= 20)
                                //     {
                                //         _enemy.GetComponent<MeleeEnemyScript>().AddHP(25);
                                //     }
                                // }
                            }
                            break;
                        case PlayerControl.WUXING.Earth:  //Player is Earth & Enemy is Wood
                            {
                                PlayerSFX(DamagedAudio);
                                Hp -= (damage + (damage * 0.5f));
                                // if (_enemy.GetComponent<MeleeEnemyScript>().health >= 75)
                                // {
                                //     System.Random rand = new System.Random();
                                //     if (rand.Next(1, 100) <= 5)
                                //     {
                                //         _enemy.GetComponent<MeleeEnemyScript>().AddHP(10);
                                //     }
                                // }
                                //
                                // else if (_enemy.GetComponent<MeleeEnemyScript>().health >= 50 && _enemy.GetComponent<MeleeEnemyScript>().health < 75)
                                // {
                                //     System.Random rand = new System.Random();
                                //     if (rand.Next(1, 100) <= 10)
                                //     {
                                //         _enemy.GetComponent<MeleeEnemyScript>().AddHP(15);
                                //     }
                                // }
                                // else if (_enemy.GetComponent<MeleeEnemyScript>().health >= 25 && _enemy.GetComponent<MeleeEnemyScript>().health < 50)
                                // {
                                //     System.Random rand = new System.Random();
                                //     if (rand.Next(1, 100) <= 15)
                                //     {
                                //         _enemy.GetComponent<MeleeEnemyScript>().AddHP(20);
                                //     }
                                // }
                                // else if (_enemy.GetComponent<MeleeEnemyScript>().health < 25)
                                // {
                                //     System.Random rand = new System.Random();
                                //     if (rand.Next(1, 100) <= 20)
                                //     {
                                //         _enemy.GetComponent<MeleeEnemyScript>().AddHP(25);
                                //     }
                                // }
                            }
                            break;
                        case PlayerControl.WUXING.Metal:  //Player is Metal & Enemy is Wood
                            {
                                PlayerSFX(DamagedAudio);
                                Hp -= (damage - (damage * 0.5f));
                                // if (_enemy.GetComponent<MeleeEnemyScript>().health >= 75)
                                // {
                                //     System.Random rand = new System.Random();
                                //     if (rand.Next(1, 100) <= 5)
                                //     {
                                //         _enemy.GetComponent<MeleeEnemyScript>().AddHP(10);
                                //     }
                                // }
                                //
                                // else if (_enemy.GetComponent<MeleeEnemyScript>().health >= 50 && _enemy.GetComponent<MeleeEnemyScript>().health < 75)
                                // {
                                //     System.Random rand = new System.Random();
                                //     if (rand.Next(1, 100) <= 10)
                                //     {
                                //         _enemy.GetComponent<MeleeEnemyScript>().AddHP(15);
                                //     }
                                // }
                                // else if (_enemy.GetComponent<MeleeEnemyScript>().health >= 25 && _enemy.GetComponent<MeleeEnemyScript>().health < 50)
                                // {
                                //     System.Random rand = new System.Random();
                                //     if (rand.Next(1, 100) <= 15)
                                //     {
                                //         _enemy.GetComponent<MeleeEnemyScript>().AddHP(20);
                                //     }
                                // }
                                // else if (_enemy.GetComponent<MeleeEnemyScript>().health < 25)
                                // {
                                //     System.Random rand = new System.Random();
                                //     if (rand.Next(1, 100) <= 20)
                                //     {
                                //         _enemy.GetComponent<MeleeEnemyScript>().AddHP(25);
                                //     }
                                // }
                            }
                            break;
                        case PlayerControl.WUXING.Water:  //Player is Water & Enemy is Wood
                            {
                                PlayerSFX(DamagedAudio);
                                Hp -= (damage + (damage * 0.5f));

                                //  if (_enemy.GetComponent<MeleeEnemyScript>().health >= 75)
                                //  {
                                //      System.Random rand = new System.Random();
                                //      if (rand.Next(1, 100) <= 5)
                                //      {
                                //          _enemy.GetComponent<MeleeEnemyScript>().AddHP(10);
                                //      }
                                //  }
                                //
                                //  else if (_enemy.GetComponent<MeleeEnemyScript>().health >= 50 && _enemy.GetComponent<MeleeEnemyScript>().health < 75)
                                //  {
                                //      System.Random rand = new System.Random();
                                //      if (rand.Next(1, 100) <= 10)
                                //      {
                                //          _enemy.GetComponent<MeleeEnemyScript>().AddHP(15);
                                //      }
                                //  }
                                //  else if (_enemy.GetComponent<MeleeEnemyScript>().health >= 25 && _enemy.GetComponent<MeleeEnemyScript>().health < 50)
                                //  {
                                //      System.Random rand = new System.Random();
                                //      if (rand.Next(1, 100) <= 15)
                                //      {
                                //          _enemy.GetComponent<MeleeEnemyScript>().AddHP(20);
                                //      }
                                //  }
                                //  else if (_enemy.GetComponent<MeleeEnemyScript>().health < 25)
                                //  {
                                //      System.Random rand = new System.Random();
                                //      if (rand.Next(1, 100) <= 20)
                                //      {
                                //          _enemy.GetComponent<MeleeEnemyScript>().AddHP(25);
                                //      }
                                //  }
                            }
                            break;
                        case PlayerControl.WUXING.Wood:  //Player is Wood & Enemy is Wood
                            {
                                PlayerSFX(DamagedAudio);
                                Hp -= (damage);
                                //  if (_enemy.GetComponent<MeleeEnemyScript>().health >= 75)
                                //  {
                                //      System.Random rand = new System.Random();
                                //      if (rand.Next(1, 100) <= 5)
                                //      {
                                //          _enemy.GetComponent<MeleeEnemyScript>().AddHP(10);
                                //      }
                                //  }
                                //  if (_enemy.GetComponent<MeleeEnemyScript>().health >= 50 && _enemy.GetComponent<MeleeEnemyScript>().health < 75)
                                //  {
                                //      System.Random rand = new System.Random();
                                //      if (rand.Next(1, 100) <= 10)
                                //      {
                                //          _enemy.GetComponent<MeleeEnemyScript>().AddHP(15);
                                //      }
                                //  }
                                //  if (_enemy.GetComponent<MeleeEnemyScript>().health >= 25 && _enemy.GetComponent<MeleeEnemyScript>().health < 50)
                                //  {
                                //      System.Random rand = new System.Random();
                                //      if (rand.Next(1, 100) <= 15)
                                //      {
                                //          _enemy.GetComponent<MeleeEnemyScript>().AddHP(20);
                                //      }
                                //  }
                                //  if (_enemy.GetComponent<MeleeEnemyScript>().health < 25)
                                //  {
                                //      System.Random rand = new System.Random();
                                //      if (rand.Next(1, 100) <= 20)
                                //      {
                                //          _enemy.GetComponent<MeleeEnemyScript>().AddHP(25);
                                //      }
                                //  }
                            }
                            break;
                    }
                }
                break;
            default:
                PlayerSFX(DamagedAudio);
                hp -= damage;
                break;
        }
        if (hp > 0)
        {
            PlayerSFX(1, 1, DamagedAudio);
            anim.SetTrigger("GettingDam");
        }
        else if (hp <= 0)
        {
            hp = 0;
            foreach (GameObject g in GameObject.FindGameObjectsWithTag("Burn"))
            {
                Destroy(g);
            }

            if (isAlive)
            {
                PlayerSFX(1, 1, DeathAudio);
                anim.SetBool("Dead", true);
                gameObject.layer = 24;
            }
        }
    }
    void PlayerSFX(AudioClip audioClip)
    {
        PlayerSFX(1, 1, audioClip);
    }
    void PlayerSFX(float pitch, float volume, AudioClip audioClip)
    {
        if (!SoundInfo.soundMute)
        {
            float soundVolumeIndex = PlayerPrefs.GetFloat("SoundVolume", 1);
            audioSource.pitch = pitch;
            audioSource.PlayOneShot(audioClip, volume * soundVolumeIndex);
        }
    }

    private bool AbleToAttack()
    {
        if (Melee.GetComponent<Collider2D>().IsTouchingLayers(WhatIsEnemy))
        {
            print("true");
            return true;
        }
        else
        {
            print("false");
            return false;
        }
    }
    void ElementAttack(MeleeEnemyScript enemy)
    {
        switch (wuXing)
        {
            case WUXING.Metal:
                break;
            case WUXING.Wood:
                GameObject woodeff = (GameObject.Instantiate(WoodEff) as GameObject);
                woodeff.transform.parent = enemy.transform;
                woodeff.SendMessage("SetPlayer", transform);
                break;
            case WUXING.Water:
                break;
            case WUXING.Fire:
                (GameObject.Instantiate(FireEff) as GameObject).transform.parent = enemy.transform;
                break;
            case WUXING.Earth:
                break;
            default:
                break;
        }
    }

    //wood effect life steal
    public void AddHP(int _HP)
    {
        Hp += _HP;
        if (Hp > hpCount)
            Hp = hpCount;
    }

    void SFX()
    {
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A))
            if (PlayerRB.velocity.y == 0)
            {
                AudioTimer += Time.deltaTime;
                if (AudioTimer > 0.1f && AudioTimer <= 0.5f && isGround && AudioPlaying == false)
                {
                    audioSource.Stop();
                    PlayerSFX(UnityEngine.Random.Range(1f, 1.4f), UnityEngine.Random.Range(.1f, .2f), WalkAudio);

                    AudioPlaying = true;
                }
                if (AudioTimer > 0.5f)
                {
                    AudioTimer = 0;
                    audioSource.Stop();
                    AudioPlaying = false;
                }
            }
        if (Input.GetKey(KeyCode.W) && PlayerRB.velocity.y == 0)
        {
            audioSource.Stop();
            PlayerSFX(UnityEngine.Random.Range(0.9f, 1f), UnityEngine.Random.Range(0.35f, 0.65f), JumpAudio);
            AudioPlaying = false;
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.transform.tag == "ArmorItem")
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                ArmorConfig.ArmorItem tmp = ArmorSYSTEM.GetComponent<ArmorConfig>().armorItem[int.Parse(other.transform.parent.name)];
                int weithCount = tmp.WEIGHT;
                for (int i = 0; i < armorItemList.Length; i++)
                {
                    if (armorItemList[i] != null && armorItemList[i].TYPE != tmp.TYPE)
                    {
                        weithCount += armorItemList[i].WEIGHT;
                    }
                }
                if (weithCount <= 15)
                {
                    other.SendMessageUpwards("GetArmorItem", armorItemList);
                }
            }
        }
    }
    void GetArmorItem(ArmorConfig.ArmorItem item)
    {
        switch ((ArmorConfig.ArmorType)item.TYPE)
        {
            case ArmorConfig.ArmorType.HELMET:
                armorItemList[0] = item;
                break;
            case ArmorConfig.ArmorType.COTHES:
                armorItemList[1] = item;
                break;
            case ArmorConfig.ArmorType.GLOVE:
                armorItemList[2] = item;
                break;
            case ArmorConfig.ArmorType.SHOSE:
                armorItemList[3] = item;
                break;
            case ArmorConfig.ArmorType.TROUSERS:
                armorItemList[4] = item;
                break;
            case ArmorConfig.ArmorType.SHIELD:
                armorItemList[5] = item;
                break;
            default:
                break;
        }
        ArmorSYSTEM.SendMessage("ChangeSlot", armorItemList);
    }

    public void Stun(float duration)
    {
        stunned = true;
        stunDuration = duration;
        stunTimer = 0;
    }

    IEnumerator ShieldArmor()
    {
        waterProtect = true;
        yield return new WaitForSeconds(5f);
        MetalEff.SetActive(false);
        shield = false;
        waterProtect = false;
    }
    public void EffectDamage(float _damage)
    {
        hp -= _damage;
    }

    IEnumerator StunEff()  //just slow down the moving speed
    {
        MovingSpeed = 1.5f;
        yield return new WaitForSeconds(3f);
        MovingSpeed = 3;
    }
    void SetStun()
    {
        StartCoroutine(StunEff());
    }
}
