using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class BloodBar : MonoBehaviour {

    private Transform m_Transform;
    private Transform cube_Transform;

    private int hp = 100;
    private bool isLife = true;
    private Image m_Bar;

    public bool IsLife
    {
        get { return isLife; }
    }

    void Start()
    {
        m_Transform = gameObject.GetComponent<Transform>();
        m_Bar = m_Transform.Find("Bar").GetComponent<Image>();
    }

    void Update()
    {
        Follow();
    }

    private void Follow()
    {
        //3D坐标转换为2D坐标，WorldToScreenPoint:世界位置转换为屏幕位置
        Vector2 cubeV2Pos = RectTransformUtility.WorldToScreenPoint(Camera.main, cube_Transform.position);
        m_Transform.position = cubeV2Pos + new Vector2(0, 100);
    }

    public void SetPlayer(Transform PlayerTransform)
    {
        cube_Transform = PlayerTransform;
    }

    public void Damage(int v)
    {
        hp -= v;
        if (hp <= 0)
        {
            isLife = false;
            GameObject.Destroy(gameObject);
            GameObject.Destroy(cube_Transform.gameObject);
        }
        else
        {
            m_Bar.fillAmount = hp / 100.0f;
        }
    }
}
