using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMap : MonoBehaviour {

    //定义枚举类型
    public enum HorizontalAlignment { left, center, right };
    public enum VerticalAlignment { top, middle, bottom };
    public enum ScreenDimensions { pixels, screen_percentage };

    //定义枚举类型的变量
    public HorizontalAlignment horizontalAlignment = HorizontalAlignment.left;
    public VerticalAlignment verticalAlignment = VerticalAlignment.top;
    public ScreenDimensions dimensions = ScreenDimensions.pixels;

    public int width = 50;
    public int height = 50;
    public float xOffset = 0.0f;
    public float yOffset = 0.0f;
    public bool update = true;

    private int hSize, vSize, hLoc, vLoc;

    void Start()
    {
        AdjustCamera();
    }

    //游戏运行时，每一帧都调用此函数
    void Update()
    {
        AdjustCamera();
    }

    //游戏对象初始化时，调用此函数
    void AdjustCamera()
    {
        if (dimensions == ScreenDimensions.screen_percentage)            //调节视图为指定百分比大小
        {
            hSize = (int)(width * 0.01f * Screen.width);
            vSize = (int)(height * 0.01f * Screen.height);
        }
        else                                 //调节视图为指定像素大小
        {
            hSize = height;
            vSize = width;
        }

        if (horizontalAlignment == HorizontalAlignment.left)               //水平方向上是左对齐
        {
            hLoc = (int)(xOffset * 0.01f * Screen.width);
        }
        else if (horizontalAlignment == HorizontalAlignment.right)         //水平方向上是右对齐
        {
            hLoc = (int)((Screen.width - hSize) - (xOffset * 0.01f * Screen.width));
        }
        else                                                             //水平方向上是居中
        {
            hLoc = (int)((Screen.width - hSize) * 0.5f - (xOffset * 0.01f * Screen.width));
        }

        if (verticalAlignment == VerticalAlignment.top)                     //垂直方向为顶端
        {
            vLoc = (int)((Screen.height - vSize) - (yOffset * 0.01f * Screen.height));
        }
        else if (verticalAlignment == VerticalAlignment.bottom)             //垂直方向为底端
        {
            vLoc = (int)(yOffset * 0.01f * Screen.height);
        }
        else                                  //垂直方向为居中
        {
            vLoc = (int)((Screen.height - vSize) * 0.5f - (yOffset * 0.01f * Screen.height));
        }

        this.GetComponent<Camera>().pixelRect = new Rect(hLoc, vLoc, hSize, vSize);
    }
}
