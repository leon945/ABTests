using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class CbrRequest<T>
{
    public string userId;
    public string token;
    public T data;

}
