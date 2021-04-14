using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class CbrResponse<T>
{

	public string message = "";
	public int statusCode = 0;
	public bool isError = false;
	public T data;

}
