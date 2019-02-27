using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMaskLayer : MonoBehaviour {

    private void Awake()
    {
        CameraScale();
    }

    // Use this for initialization
    void Start ()
    {
        CameraScale();
        //int width = Screen.width;
        //int height = Screen.height;
        ////int designWidth = 960;//开发时分辨率宽
        ////int designHeight = 640;//开发时分辨率高

        //int designWidth = 855;//开发时分辨率宽
        //   int designHeight = 405;//开发时分辨率高
        //   float s1 = (float)designWidth / (float)designHeight;
        //float s2 = (float)width / (float)height;
        //if (s1 < s2)
        //{
        //    designWidth = (int)Mathf.FloorToInt(designHeight * s2);
        //}
        //else if (s1 > s2)
        //{
        //    designHeight = (int)Mathf.FloorToInt(designWidth / s2);
        //}
        //float contentScale = (float)designWidth / (float)width;
        //RectTransform rectTransform = this.transform as RectTransform;
        //if (rectTransform != null)
        //{
        //    rectTransform.sizeDelta = new Vector2(designWidth, designHeight);
        //}
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void Panel()
    {
        int width = Screen.width;
        int height = Screen.height;
        //int designWidth = 960;//开发时分辨率宽
        //int designHeight = 640;//开发时分辨率高

        int designWidth = 855;//开发时分辨率宽
        int designHeight = 405;//开发时分辨率高
        float s1 = (float)designWidth / (float)designHeight;
        float s2 = (float)width / (float)height;
        if (s1 < s2)
        {
            designWidth = (int)Mathf.FloorToInt(designHeight * s2);
        }
        else if (s1 > s2)
        {
            designHeight = (int)Mathf.FloorToInt(designWidth / s2);
        }
        float contentScale = (float)designWidth / (float)width;
        RectTransform rectTransform = this.transform as RectTransform;
        if (rectTransform != null)
        {
            rectTransform.sizeDelta = new Vector2(designWidth, designHeight);
        }
    }

    public Vector3 ScaleByOriginalHeight(float height)
    {
        //canvasScaler是UIRoot根节点挂载的CanvasScaler，referenceResolutionHeight是开发分辨率的高
        //float referenceResolutionHeight = canvasScaler.referenceResolution.y; 
        float referenceResolutionHeight = this.transform.GetComponent<CanvasScaler>().referenceResolution.y; 
        //UIRoot根节点的高度会根据CanvasScaler组件，进行高度缩放，从而产生新的基准分辨率
        float adjustReferenceResolution = GetComponent<RectTransform>().rect.height;
        float scale = adjustReferenceResolution / referenceResolutionHeight;
        return new Vector3(scale, scale, 1);
    }

    void CameraScale()
    {
        int ManualWidth = 855;
        int ManualHeight = 405;
        int manualHeight;
        if (System.Convert.ToSingle(Screen.height) / Screen.width > System.Convert.ToSingle(ManualHeight) / ManualWidth)
            manualHeight = Mathf.RoundToInt(System.Convert.ToSingle(ManualWidth) / Screen.width * Screen.height);
        else
            manualHeight = ManualHeight;
        Camera camera = GetComponent<Camera>();
        float scale = System.Convert.ToSingle(manualHeight / 405f);
        camera.fieldOfView *= scale;
    }


}
