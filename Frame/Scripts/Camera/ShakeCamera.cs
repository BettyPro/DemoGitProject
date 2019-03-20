using UnityEngine;

public class ShakeCamera : MonoBehaviour {

	public float shakeLevel = 3f; // 震动幅度
	public float setShakeTime = 0.5f; // 震动时间
	public float shakeFps = 45f; // 震动的FPS

	private bool isshakeCamera = false; // 震动标志
	private float fps;
	private float shakeTime = 0.0f;
	private float frameTime = 0.0f;
	private float shakeDelta = 0.005f;
	private Camera selfCamera;

	int testCount = 0;

	private void Start() {
		MyTimerTool.Instance.wtime.AddTimeTask (tid => {
			testCount++;
			Debug.Log(testCount);
			ShakeEffect(200);
		}, 0.1, WinterTimeUnit.Second, 3);
	}

	void ShakeEffect(int length = 1)
	{
//		for (int i = 0; i < length; i++)
//		{
			isshakeCamera = true;
			selfCamera = gameObject.GetComponent<Camera> ();
			shakeTime = setShakeTime;
			fps = shakeFps;
			frameTime = 0.03f;
			shakeDelta = 0.005f;
//		}
	}

	void OnDisable () {
		selfCamera.rect = new Rect (0.0f, 0.0f, 1.0f, 1.0f);
		isshakeCamera = false;
	}

	// Update is called once per frame
	void Update () {
		MyTimerTool.Instance.wtime.Update();
		if (isshakeCamera) {
			if (shakeTime > 0) {
				shakeTime -= Time.deltaTime;
				if (shakeTime <= 0) {
//					enabled = false;
					selfCamera.rect = new Rect (0,0, 1.0f, 1.0f);
				} else {
					frameTime += Time.deltaTime;

					if (frameTime > 1.0 / fps) {
						frameTime = 0;
//						selfCamera.rect = new Rect (shakeDelta * (-1.0f + shakeLevel * Random.value), shakeDelta * (-1.0f + shakeLevel * Random.value), 1.0f, 1.0f);
						selfCamera.rect = new Rect (shakeDelta * (-5.0f + shakeLevel * Random.value * 10), shakeDelta * (1.0f + shakeLevel * Random.value), 1.0f, 1.0f);
//						selfCamera.rect = new Rect (shakeDelta * (-1.0f + shakeLevel * Random.value), shakeDelta * (1.0f + shakeLevel * Random.value), 1.0f, 1.0f);
//						selfCamera.rect = new Rect (shakeDelta * (-1.0f + shakeLevel * Random.value), 0, 1.0f, 1.0f);//左右
//						selfCamera.rect = new Rect (0, shakeDelta * (-0.50f + shakeLevel * Random.value), 1.0f, 1.0f);//上下
					}
				}
			}
		}
	}
}