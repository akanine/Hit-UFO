using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Com.HitGame;

namespace Com.HitGame
{
    public class myFactory : System.Object
    {
        public int Round = 0; 
        public int state = 0;  // 0-end 1-begin
        public int Score = 0;
        public int usefri = 0;
        public int GameState = 1;// 1-准备 2-进行 3-结束
        public int LoseNum = 0;

        public List<GameObject> fris = new List<GameObject>();


        private static myFactory _instance;
        public static myFactory GetInstance()
        {
            if (_instance == null)
            {
                _instance = new myFactory();
            }
            return _instance;
        }

        public void launch_fri(float g, float delay, Vector3 speed, Color color,
            Vector3 size, Vector3 position)
        {
            if (fris.Count == 0)
            {
                GameObject fri = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                Factory fr = fri.AddComponent<Factory>();
                fr.setting(g, fri, fris, delay, speed, color, size, position);
            }
            else
            {
                GameObject fri = fris[0];
                fris.RemoveAt(0);
                Factory fr = fri.GetComponent<Factory>();
                fr.setting(g, fri, fris, delay, speed, color, size, position);
            }
        }

        public class Factory : MonoBehaviour
        {
            public List<GameObject> fris;
            public GameObject fri;
            public Camera cam;
            public float delay;
            public Vector3 speed;
            public float g;
            public int state = 0;
            public Color color;
            public Vector3 size;
            public Vector3 position;


            public void setting(float g, GameObject fri, List<GameObject> fris, float delay,
                Vector3 speed, Color color, Vector3 size, Vector3 position)
            {
                this.fri = fri;
                this.fris = fris;
                this.delay = delay;
                this.speed = speed;
                this.g = g;
                this.color = color;
                this.size = size;
                this.position = position;
                StartCoroutine(launch());
            }

            public IEnumerator launch()
            {
                yield return new WaitForSeconds(delay);
                this.transform.position = position;
                this.transform.localScale = size;
                this.GetComponent<Renderer>().material.color = color;
                state = 1;
            }

            void Update()
            {
                if (state == 1)
                {
                    speed = new Vector3(speed.x, speed.y - g, speed.z);
                    this.transform.position = this.transform.position + speed;
                    if (this.transform.position.y <= 0f)
                    {

                        myFactory.GetInstance().LoseNum++;
                        this.transform.position = new Vector3(0f, 0f, -2f);
                        state = 0;
                        fris.Add(fri);
                        myFactory.GetInstance().usefri++;

                    }
                }
                if (Input.GetMouseButtonDown(0))
                {
                    Vector3 mp = Input.mousePosition;
                    cam = Camera.main;
                    Ray ray = cam.ScreenPointToRay(mp);
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit))
                    {
                        if (hit.collider.gameObject == this.gameObject)
                        {

                            this.transform.position = new Vector3(0f, 0f, -2f);
                            state = 0;
                            fris.Add(fri);
                            if (myFactory.GetInstance().Round == 1)
                            {
                                myFactory.GetInstance().Score++;
                                myFactory.GetInstance().usefri++;
                            }
                            else if (myFactory.GetInstance().Round == 2)
                            {
                                myFactory.GetInstance().Score += 2;
                                myFactory.GetInstance().usefri++;
                            }
                            else if (myFactory.GetInstance().Round == 3)
                            {
                                myFactory.GetInstance().Score += 3;
                                myFactory.GetInstance().usefri++;
                            }

                        }
                    }
                }
            }

        }
    }
}

public class GameModel : MonoBehaviour
{
    public Vector3 camera_pos;
    public Quaternion camera_qua;
    public Camera cam;
    public Vector3 Move;
    public float g;  //gravity
    public Color whiteColor;
    public Color blueColor;
    public Color RedColor;
    public Vector3 white_size;
    public Vector3 blue_size;
    public Vector3 red_size;
    public Vector3 white_speed;
    public Vector3 blue_speed;
    public Vector3 red_speed;


    public Vector3 left_pos;
    public Vector3 right_pos;

    void Start()
    {
        camera_pos = new Vector3(0f, 0f, 0f);
        camera_qua = Quaternion.Euler(340f, 0f, 0f);
        cam = Camera.main;
        cam.transform.position = camera_pos;
        cam.transform.rotation = camera_qua;


        whiteColor = new Color(0.9f, 0.9f, 0.9f);
        blueColor = new Color(0f, 0f, 0.7f);
        RedColor = new Color(0.7f, 0f, 0f);

        white_size = new Vector3(1.2f, 0.5f, 1.2f);
        blue_size = new Vector3(1f, 0.2f, 1f);
        red_size = new Vector3(1f, 0.2f, 1f);

        white_speed = new Vector3(0f, 0.1f, 0.25f);
        blue_speed = new Vector3(0f, 0.1f, 0.25f);
        red_speed = new Vector3(0f, 0.1f, 0.4f);

        Move = new Vector3(0.05f, 0f, 0f);

        g = 0.001f;

        left_pos = new Vector3(-1f, 0f, 0f);
        right_pos = new Vector3(1f, 0f, 0f);

    }


    void Update()
    {
        if (myFactory.GetInstance().LoseNum >= 3)
        {
            myFactory.GetInstance().GameState = 3;
        }

        if (myFactory.GetInstance().Round == 1 && myFactory.GetInstance().state == 0 && myFactory.GetInstance().GameState != 3)
        {
            myFactory.GetInstance().state = 1;
            myFactory.GetInstance().GameState = 2;

            Invoke("Round1", 0f);
        }

        if (myFactory.GetInstance().Round == 1 && myFactory.GetInstance().usefri == 6 && myFactory.GetInstance().GameState == 2)
        {
            myFactory.GetInstance().Round = 2;
            myFactory.GetInstance().usefri = 0;
            Invoke("Round2", 1f);
        }

        if (myFactory.GetInstance().Round == 2 && myFactory.GetInstance().usefri == 6 && myFactory.GetInstance().GameState == 2)
        {
            myFactory.GetInstance().Round = 3;
            myFactory.GetInstance().usefri = 0;
            Invoke("Round3", 1f);
        }


    }

    void Round1()
    {
        for (int i = 1; i <= 6; i++)
        {
            float time = i;
            float Randkey = Random.Range(-4f, 4f);
            if (i % 2 == 0)
            {
                myFactory.GetInstance().launch_fri(g, time * 2, white_speed + Move * Randkey, whiteColor, white_size, left_pos);

            }
            else
            {
                myFactory.GetInstance().launch_fri(g, time * 2, white_speed - Move * Randkey, whiteColor, white_size, right_pos);

            }
        }
    }

    void Round2()
    {
        for (int i = 1; i <= 6; i++)
        {
            float time = i;
            float Randkey = Random.Range(-4f, 4f);
            if (i % 2 == 0)
            {
                myFactory.GetInstance().launch_fri(g, time * 2, blue_speed + Move * Randkey, blueColor, blue_size, left_pos);

            }
            else
            {
                myFactory.GetInstance().launch_fri(g, time * 2, blue_speed - Move * Randkey, blueColor, blue_size, right_pos);

            }
        }
    }

    void Round3()
    {
        for (int i = 1; i <= 6; i++)
        {
            float time = i;
            float Randkey = Random.Range(-4f, 4f);
            if (i % 2 == 0)
            {
                myFactory.GetInstance().launch_fri(g, time * 2, red_speed + Move * Randkey, RedColor, red_size, left_pos);

            }
            else
            {
                myFactory.GetInstance().launch_fri(g, time * 2, red_speed + Move * Randkey, RedColor, red_size, right_pos);

            }
        }

    }
}






