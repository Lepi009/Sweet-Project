using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class MoreExtensions
{
    public static void SetText(this Text text, string newText)
    {
        text.text = newText;
    }
}
