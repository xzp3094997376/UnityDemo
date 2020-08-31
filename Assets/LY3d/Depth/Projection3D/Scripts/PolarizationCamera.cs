using UnityEngine;
using System.Collections;
using System.Text;

public class PolarizationCamera : MonoBehaviour 
{

	private UIProjectionPoints uiPoints;
	private Camera theCamera;

	[Range(0.01f, 1f)]
	public float index = 0.1f;

	private float near = 0.2f;
	private float far = 1000f;
	private float top = 8f;
	private float bottom = -8f;
	private float left = -8f;
	private float right = 8f;

	private float offsetX = 0;
	private float offsetY = 0;

	[HideInInspector]
	public float widthOffset = 1920;


	


	// Use this for initialization
	public void InitStart(UIProjectionPoints _uiPoints, RenderTexture _texture)
	{
		theCamera = gameObject.GetComponent<Camera>();

		//theCamera.nearClipPlane = 0.03f;
		//theCamera.depth = -1;
		//theCamera.enabled = false;

		uiPoints = _uiPoints;
		theCamera.targetTexture = _texture;
	}



	// Update is called once per frame
	void Update()
	{
		//return;
		if (uiPoints == null)
			return;


		//Vector3 vCam2TopLeft = transform.position - uiPoints.topLeft.position;
		//Vector3 vCam2TopRight = transform.position - uiPoints.topRight.position;
		//Vector3 vCam2BottomLeft = transform.position - uiPoints.bottomLeft.position;
		//Vector3 vCam2BottomRight = transform.position - uiPoints.bottomRight.position;
		//Debug.DrawLine(transform.position, uiPoints.topLeft.position, Color.blue);
		//Debug.DrawLine(transform.position, uiPoints.topRight.position, Color.blue);
		//Debug.DrawLine(transform.position, uiPoints.bottomLeft.position, Color.blue);
		//Debug.DrawLine(transform.position, uiPoints.bottomRight.position, Color.blue);

		//Vector3 vIndexTopLeft = transform.position - vCam2TopLeft.normalized * vCam2TopLeft.magnitude * index;
		//Vector3 vIndexTopRight = transform.position - vCam2TopRight.normalized * vCam2TopRight.magnitude * index;
		//Vector3 vIndexBottomLeft = transform.position - vCam2BottomLeft.normalized * vCam2BottomLeft.magnitude * index;
		//Vector3 vIndexBottomRight = transform.position - vCam2BottomRight.normalized * vCam2BottomRight.magnitude * index;
		//Debug.DrawLine(vIndexTopLeft, vIndexTopRight, Color.green);
		//Debug.DrawLine(vIndexTopLeft, vIndexBottomLeft, Color.green);
		//Debug.DrawLine(vIndexBottomRight, vIndexTopRight, Color.green);
		//Debug.DrawLine(vIndexBottomRight, vIndexBottomLeft, Color.green);


		//计算rect面的法向量
		Vector3 v1 = uiPoints.topLeft.position - uiPoints.topRight.position;
		Vector3 v2 = uiPoints.topLeft.position - uiPoints.bottomLeft.position;
		Vector3 vn = new Vector3(v1.y * v2.z - v2.y * v1.z,
								 v1.z * v2.x - v2.z * v1.x,
								 v1.x * v2.y - v2.x * v1.y);
		vn = vn.normalized * -1;//方向调整
								//Debug.DrawLine(uiPoints.topLeft.position, uiPoints.topLeft.position + vn * 50f, Color.red);

		//计算夹角
		float angle1 = Vector3.Angle(vn, (transform.position - uiPoints.topLeft.position).normalized);
		near = Vector3.Distance(transform.position, uiPoints.topLeft.position) * Mathf.Cos(angle1 * Mathf.Deg2Rad);
		near = Mathf.Abs(near) * index;//取正
		//Debug.DrawLine(transform.position, transform.position + vn * near, Color.red);


		//偏移量修正
		offsetX = transform.localPosition.x * -1f * index;
		offsetY = transform.localPosition.y * -1f * index;

		top = Vector3.Distance(uiPoints.topLeft.position, uiPoints.bottomLeft.position) * 0.5f * index + offsetY;
		bottom = -Vector3.Distance(uiPoints.topLeft.position, uiPoints.bottomLeft.position) * 0.5f * index + offsetY;
		right = Vector3.Distance(uiPoints.topLeft.position, uiPoints.topRight.position) * 0.5f * index + offsetX;
		left = -Vector3.Distance(uiPoints.topLeft.position, uiPoints.topRight.position) * 0.5f * index + offsetX;

		Matrix4x4 pM4x4 = SetPolarizationM4x4(near, far, left, right, top, bottom);
		theCamera.projectionMatrix = pM4x4;

	}


	//透视矩阵
	Matrix4x4 SetPolarizationM4x4(float _near, float _far, float _left, float _right, float _top, float _bottom)
	{
		Matrix4x4 getMx4 = new Matrix4x4
		{
			m00 = 2 * _near / (_right - _left) * (1920f / widthOffset),
			m01 = 0,
			m02 = (_right + _left) / (_right - _left) * (1920f / widthOffset),
			m03 = 0,
			m10 = 0,
			m11 = 2 * _near / (_top - _bottom),
			m12 = (_top + _bottom) / (_top - _bottom),
			m13 = 0,
			m20 = 0,
			m21 = 0,
			m22 = (_far + _near) / (_far - _near) * -1,
			m23 = (2 * _far * _near) / (_far - _near) * -1,
			m30 = 0,
			m31 = 0,
			m32 = -1,
			m33 = 0
		};
		return getMx4;
	}




}
