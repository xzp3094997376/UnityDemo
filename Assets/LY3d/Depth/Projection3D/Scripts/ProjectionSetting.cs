using UnityEngine;
using System.IO;
using System.Runtime.InteropServices;
using System;
using UnityEngine.UI;

/// <summary>
/// 3D 投影设置
/// </summary>
public class ProjectionSetting : MonoBehaviour
{


	[DllImport("user32.dll")]
	private static extern bool ShowWindowAsync(IntPtr hWnd, int nCmdShow);
	[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
	private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
	[DllImport("user32.dll")]
	private static extern bool SetWindowText(IntPtr hWnd, string lpString);


	/// <summary>
	/// 隐藏窗口。
	/// </summary>
	private const int SW_HIDE = 0;
	/// <summary>
	/// 显示窗口，标准状态。
	/// </summary>
	private const int SW_SHOWNORMAL = 1;
	/// <summary>
	/// 第二窗口的句柄（指针）。
	/// </summary>
	private static IntPtr hWnd = IntPtr.Zero;


	//是否第一次打开
	bool isFirstOpen = true;



	public enum ProjectionType
	{
		None,
		PianZhen,
		LuoYan,
	}


	public static ProjectionSetting instance;


	public ProjectionType projectionType = ProjectionType.None;


	[Header("UIPoints:")]
	public UIProjectionPoints uiPoints;

	[Header("Texture:")]
	public RenderTexture leftTexture;
	public RenderTexture rightTexture;
	public RenderTexture depthTexture;

	[Header("相机:")]
	public PolarizationCamera pLeftCam;
	public PolarizationCamera pRightCam;
	public PolarizationCamera pDepthCam;

	[Header("投影面板")]
	public GameObject uiPanel;
	public RawImage leftRawImg;
	public RawImage rightRawImg;
	public RawImage headerImg;

	[Header("相机距离:")]
	public float cameraDistance = 10;

	[Header("裸眼屏Width调整:")]
	public float widthOffset = 1920f;

	[Header("偏振瞳距:")]
	public float pianZhenEye = 0.03f;

	/// <summary>
	/// 是否开启投影模式.
	/// </summary>
	[HideInInspector]
	public bool isStartProject;



	//private ProjectionGetDepth getCamDepthCtrl;
	//public float depthScale = 0.3333333f;
	//public float depthCameraNearMove = -0.29f;


	void Awake()
	{
		DontDestroyOnLoad(gameObject);
		if (instance == null)
			instance = this;
	}



	void Start()
	{

		pLeftCam.InitStart(uiPoints, leftTexture);
		pRightCam.InitStart(uiPoints, rightTexture);
		pDepthCam.InitStart(uiPoints, depthTexture);

		ReadSettingData();
	}



	public void OpenProjection(ProjectionType _type)
	{

#if !UNITY_EDITOR	
		try
		{
			if (isFirstOpen && Display.displays.Length > 1)
			{
				Display.displays[1].Activate();

				//使用windows函数查找第二个窗口，无论多少窗口，此名称默认固定。
				hWnd = FindWindow(null, "Unity Secondary Display");
				//如果找到对应窗口，为了不混淆，可以考虑更改个名称。
				if (hWnd != IntPtr.Zero)
				{
					SetWindowText(hWnd, "LY Display");
				}

				isFirstOpen = false;
			}
			else
			{
				ShowWindowAsync(hWnd, SW_SHOWNORMAL);
			}
		}
		catch
		{
		}
#endif

		isStartProject = true;
		projectionType = _type;


		if (_type == ProjectionType.PianZhen)
		{
			pLeftCam.widthOffset = 1920;
			pRightCam.widthOffset = 1920;

			rightRawImg.texture = rightTexture;
			headerImg.gameObject.SetActive(false);

			pLeftCam.gameObject.SetActive(true);
			pRightCam.gameObject.SetActive(true);
			pDepthCam.gameObject.SetActive(false);
			

		}
		else if (_type == ProjectionType.LuoYan)
		{
			rightRawImg.texture = depthTexture;
			headerImg.gameObject.SetActive(true);

			pLeftCam.gameObject.SetActive(true);
			pRightCam.gameObject.SetActive(false);
			pDepthCam.gameObject.SetActive(true);
		}


		uiPanel.SetActive(true);


	}



	/// <summary>
	/// 关闭3D投影。
	/// </summary>
	public void CloseProjection()
	{
		if (!isFirstOpen)
		{

#if !UNITY_EDITOR
			try
			{
				ShowWindowAsync(hWnd, SW_HIDE);
			}
			catch
			{ }
#endif

		}

		isStartProject = false;
		projectionType = ProjectionType.None;

		
		pLeftCam.gameObject.SetActive(false);
		pRightCam.gameObject.SetActive(false);
		pDepthCam.gameObject.SetActive(false);

		uiPanel.SetActive(false);
	}

	public bool GetIsOpen()
	{
		return isStartProject;
	}




	void Update()
	{

#if UNITY_EDITOR
		if (Input.GetKeyDown(KeyCode.O))
		{
			OpenProjection(ProjectionType.LuoYan);
		}
		else if (Input.GetKeyDown(KeyCode.I))
		{
			OpenProjection(ProjectionType.PianZhen);
		}
		else if (Input.GetKeyDown(KeyCode.U))
		{
			CloseProjection();
		}
#endif



		if (pLeftCam != null && pDepthCam != null && pRightCam != null)
		{
			if (projectionType == ProjectionType.LuoYan)
			{
				pLeftCam.widthOffset = widthOffset;
				pDepthCam.widthOffset = widthOffset;

				pLeftCam.transform.localPosition = new Vector3(0f, 0f, -cameraDistance);
				pDepthCam.transform.localPosition = new Vector3(0f, 0f, -cameraDistance);
			}
			else if(projectionType == ProjectionType.PianZhen)
			{
				pLeftCam.widthOffset = 1920;
				pRightCam.widthOffset = 1920;

				pLeftCam.transform.localPosition = new Vector3(-pianZhenEye, 0f, -cameraDistance);
				pRightCam.transform.localPosition = new Vector3(pianZhenEye, 0f, -cameraDistance);
			}
			
		}


	}


	void OnGUI()
	{
		if (isDebug)
		{
			float startX = 20f;
			float startY = 50f;

			int indexY = 0;
			GUI.Label(new Rect(startX, startY + indexY, 150, 500), "widthOffset:" + widthOffset);
			indexY += 20;
			widthOffset = GUI.HorizontalSlider(new Rect(startX, startY + indexY, 400, 10), widthOffset, 1900, 2100f);
			//indexY += 20;

			//if (projectionType == ProjectionType.PianZhen)
			//{
			//	GUI.Label(new Rect(startX, startY + indexY, 150, 500), "eyeDis:" + eyeDis);
			//	indexY += 20;
			//	eyeDis = GUI.HorizontalSlider(new Rect(startX, startY + indexY, 400, 10), eyeDis, 0.01f, 1f);
			//}
			//else if (projectionType == ProjectionType.LuoYan)
			//{
			//	GUI.Label(new Rect(startX, startY + indexY, 150, 500), "depthScale:" + depthScale);
			//	indexY += 20;
			//	depthScale = GUI.HorizontalSlider(new Rect(startX, startY + indexY, 400, 10), depthScale, -2f, 2f);
			//	indexY += 20;

			//	GUI.Label(new Rect(startX, startY + indexY, 150, 500), "depthCameraNear:" + depthCameraNearMove);
			//	indexY += 20;
			//	depthCameraNearMove = GUI.HorizontalSlider(new Rect(startX, startY + indexY, 400, 10), depthCameraNearMove, -2f, 2f);
			//}

			if (GUI.Button(new Rect(startX + 420, startY + 20, 100, 50), "Save"))
			{
				SaveSettingData();
			}

		}
	}

	//void SetCamDis(float dis)
	//{
	//	pzParent.transform.localPosition = new Vector3(0f, 0f, -dis);
	//}



	string strDebugText = "DebugText.txt";
	public bool isDebug = false;

	void SaveSettingData()
	{
		try
		{
			File.WriteAllLines(strDebugText, new string[]
			{
				"裸眼OffsetX:" + widthOffset.ToString(),
				"OpenDebug1"
			});
		}
		catch (System.Exception)
		{
		}
	}

	void ReadSettingData()
	{
		try
		{
			string[] strReads = File.ReadAllLines(strDebugText);
			widthOffset = float.Parse(strReads[0].Split(':')[1]);
			isDebug = strReads[1].EndsWith("OpenDebug");
		}
		catch
		{
			File.WriteAllLines(strDebugText, new string[]
			{
				"裸眼OffsetX:" + widthOffset.ToString(),
				"OpenDebug1"
			});
		}
	}

}
