using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{
    [Header("文本文件")]
    public TextAsset textFile;
    public int index;
    [Header("UI组件")]
    public Text textLabel;

    private float textSpeed = 0.07f;


    List<string> textList = new List<string>();

    bool textFinished;  //文本是否显示完毕
    bool isTyping;  //是否在逐字显示
    void GetTextFromFile(TextAsset file)
    {
        textList.Clear();

        var lineDate = file.text.Split('\n');
        foreach (var line in lineDate)
        {
            textList.Add(line);
        }
    }

    private void Awake()
    {
        GetTextFromFile(textFile);
    }
    void OnEnable()
    {
        index = 0;
        textFinished = true;
        StartCoroutine(setTextUI());
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown && index == textList.Count)
        {
            gameObject.SetActive(false);
            return;
        }
        if (Input.anyKeyDown)
        {
            if (textFinished)
            {
                StartCoroutine(setTextUI());
            }
            else if (!textFinished)
            {
                isTyping = false;
            }
        }
    }
    IEnumerator setTextUI()
    {
        textFinished = false;   //进入文字显示状态
        textLabel.text = "";    //重置文本内容

        int word = 0;
        if (textList[index][word] == '<')
        {
            isTyping = false;
        }
        while (isTyping && word < textList[index].Length - 1)
        {
            textLabel.text += textList[index][word];
            word++;
            yield return new WaitForSeconds(textSpeed);
        }

        textLabel.text = textList[index];

        isTyping = true;
        textFinished = true;
        index++;
    }

    void Start()
    {
        gameObject.SetActive(true);
    }
}