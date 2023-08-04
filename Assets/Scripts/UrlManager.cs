using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UrlManager : MonoBehaviour
{
    public void Url(string url)
    {
        Application.OpenURL(url);
    }
}
