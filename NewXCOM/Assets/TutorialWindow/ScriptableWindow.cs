using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct Page
{
    public string title;
    public Image image;
    [TextArea(5, 6)] public string flavorText;
}

[CreateAssetMenu(fileName = "New Window", menuName = "Tutorial Window")]
public class ScriptableWindow : ScriptableObject
{
    public Page[] page;
}
