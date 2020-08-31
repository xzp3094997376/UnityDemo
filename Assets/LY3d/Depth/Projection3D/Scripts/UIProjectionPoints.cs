using UnityEngine;
using System.Collections;

using UnityEngine.UI;

public class UIProjectionPoints : MonoBehaviour 
{

	public Transform topLeft;
	public Transform topRight;
	public Transform bottomLeft;
	public Transform bottomRight;


	public Vector3 GetTopLeftPoint()
	{
		return topLeft.position;
	}

	public Vector3 GetTopRightPoint()
	{
		return topRight.position;
	}

	public Vector3 GetBottomLeftPoint()
	{
		return bottomLeft.position;
	}

	public Vector3 GetBottomRightPoint()
	{
		return bottomRight.position;
	}




}
