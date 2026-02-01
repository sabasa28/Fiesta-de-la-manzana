using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public enum Option
{ 
    A,
    B,
    C,
    D
}

[Serializable]
public struct OptionData
{
    public string text;
    public Sprite sprite;
}

[CreateAssetMenu(fileName = "Question", menuName = "Scriptable Objects/Question")]
public class Question : ScriptableObject
{
    public string questionText = "";
    public Sprite questionImage;
    public OptionData[] options;
    public Option correctOption;
}
