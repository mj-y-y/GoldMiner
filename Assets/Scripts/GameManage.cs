using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public enum State//状态
{
    Rock,//摇摆
    Stretch,//拉伸
    Shorten//缩短
}

public class GameManage : MonoBehaviour {
    private State state;
    private Vector3 dir;
    private Transform rope;
    private Transform ropeCord;
    private float length;
    private int grade;
    private float times;

    public Text timeText;
    public Text gradeText;
    public State GetState
    {
        set { state = value; }
        get { return state ; }
    }
	void Start () {
        state = State.Rock;
        dir = Vector3.back;
        rope = transform.GetChild(0);
        ropeCord = rope.GetChild(0);
        length = 1;
        times = 30;
        grade = 0;
	}
	
	void Update () {
        if (times <= 0) { times = 0; UnityEditor.EditorApplication.isPlaying = false; return; }
        if (state == State.Rock)
        {
            Rock();
            if (Input.GetMouseButtonDown(0)) state = State.Stretch;
        }
        else if (state == State.Stretch)
        {
            Stretch();
        }
        else if (state == State.Shorten)
        {
            Shorten();
        }
        times -= Time.deltaTime;
        timeText.text = ((int)times).ToString();
        gradeText.text = grade.ToString();
	}

    private void Rock()
    {
        if (rope.localRotation.z <= -0.5f)
            dir = Vector3.forward;
        else if (rope.localRotation.z >= 0.5f)
            dir = Vector3.back;
        rope.Rotate(dir * 60 * Time.deltaTime);
    }
    private void Stretch()
    {
        if (length >= 7.5f) { state = State.Shorten; return; }
        length += Time.deltaTime;
        rope.localScale = new Vector3(rope.localScale.x, length, rope.localScale.z);
        ropeCord.localScale = new Vector3(ropeCord.localScale.x, 1 / length, ropeCord.localScale.z);
    }
    private void Shorten()
    {
        if (length <= 1)
        {
            length = 1;
            state = State.Rock;
            if (0 != ropeCord.childCount)
            {
                grade += GetGrade(ropeCord.GetChild(0).tag);
                Destroy(ropeCord.GetChild(0).gameObject);
            }
            ropeCord.GetComponent<Collider2D>().enabled = true;
            return;
        }
        length -= Time.deltaTime;
        rope.localScale = new Vector3(rope.localScale.x, length, rope.localScale.z);
        ropeCord.localScale = new Vector3(ropeCord.localScale.x, 1 / length, ropeCord.localScale.z);
    }

    private int GetGrade(string tag)//每次得到的分数
    {
        int num = 0;
        switch (tag)
        {
            case "rock":
                num = 50;
                break;
            case "goldsma":
                num = 100;
                break;
            case "goldmid":
                num = 500;
                break;
            case "goldbig":
                num = 1000;
                break;
            case "mouse":
                num = -1000;
                break;
            case "diamond":
                num = 600;
                break;
        }
        return num;
    }
}
