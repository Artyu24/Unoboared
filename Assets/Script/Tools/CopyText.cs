using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CopyText : MonoBehaviour
{
    [SerializeField] private Text textToCopy;
    private Text ourText;

    private void Start()
    {
        ourText = GetComponent<Text>();
    }

    void Update()
    {
        ourText.text = textToCopy.text;
    }
}
