using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    [System.Serializable]
    public class DialogueLine
    {
        public string text;
        public bool isQuestion; // if true, show input field
        public LineType lineType; // Normal, Question, Choice, Dropdown
        public QuestionType questionType; // Who, Where, Do, Complement, Criticism
    }

    public enum LineType { Normal, Question, Choice, Dropdown }
    public enum QuestionType { None, Who, Where, Do, Complement, Criticism }

    public TextMeshProUGUI dialogueText;
    public Button nextButton;

    [Header("Input Fields")]
    public TMP_InputField whoInputField;
    public TMP_InputField whereInputField;
    public TMP_InputField doInputField;
    public TMP_InputField complementInputField;
    public TMP_InputField criticismInputField;

    [Header("Choice Buttons")]
    public Button complementButton;
    public Button criticismButton;

    [Header("Dropdowns")]
    public TMP_Dropdown complementDropdown;
    public TMP_Dropdown criticismDropdown;

    public List<DialogueLine> dialogueLines;
    private int currentLine = 0;

    [Header("Player Answers")]
    public List<string> whoList = new List<string>();
    public List<string> whereList = new List<string>();
    public List<string> doList = new List<string>();
    public List<string> complementList = new List<string>();
    public List<string> criticismList = new List<string>();

    void Start()
    {
        HideAllInputs();
        complementButton.gameObject.SetActive(false);
        criticismButton.gameObject.SetActive(false);
        complementDropdown.gameObject.SetActive(false);
        criticismDropdown.gameObject.SetActive(false);

        // clicked button to show dropdown
        complementButton.onClick.AddListener(() => ShowDropdown(QuestionType.Complement));
        criticismButton.onClick.AddListener(() => ShowDropdown(QuestionType.Criticism));

        ShowLine();
    }

    void ShowLine()
    {
        if (currentLine >= dialogueLines.Count)
        {
            dialogueText.text = "conversation ends";
            HideAllInputs();
            complementButton.gameObject.SetActive(false);
            criticismButton.gameObject.SetActive(false);
            complementDropdown.gameObject.SetActive(false);
            criticismDropdown.gameObject.SetActive(false);
            nextButton.interactable = false;
            return;
        }

        DialogueLine line = dialogueLines[currentLine];
        dialogueText.text = line.text;

        HideAllInputs();
        complementButton.gameObject.SetActive(false);
        criticismButton.gameObject.SetActive(false);
        complementDropdown.gameObject.SetActive(false);
        criticismDropdown.gameObject.SetActive(false);

        // Normal dialogue: only show text
        if (line.lineType == LineType.Normal)
        {
            nextButton.interactable = true;
        }
        // question type
        else if (line.isQuestion && line.questionType != QuestionType.None)
        {
            TMP_InputField activeInput = null;

            if (line.questionType == QuestionType.Who) activeInput = whoInputField;
            else if (line.questionType == QuestionType.Where) activeInput = whereInputField;
            else if (line.questionType == QuestionType.Do) activeInput = doInputField;
            else if (line.questionType == QuestionType.Complement) activeInput = complementInputField;
            else if (line.questionType == QuestionType.Criticism) activeInput = criticismInputField;

            if (activeInput != null)
            {
                activeInput.gameObject.SetActive(true);
                activeInput.text = "";
                activeInput.ActivateInputField();

                // only go next when theres input
                nextButton.interactable = false;

                activeInput.onValueChanged.RemoveAllListeners();
                activeInput.onValueChanged.AddListener((value) =>
                {
                    nextButton.interactable = !string.IsNullOrWhiteSpace(value);
                });
            }
        }
        // choice type
        else if (line.lineType == LineType.Choice)
        {
            complementButton.gameObject.SetActive(true);
            criticismButton.gameObject.SetActive(true);
            nextButton.interactable = false; // wait player to click
        }
        // dropdown type
        else if (line.lineType == LineType.Dropdown)
        {
            ShowDropdown(line.questionType);
        }
    }

    public void OnNextButton()
    {
        DialogueLine line = dialogueLines[currentLine];

        if (line.isQuestion)
        {
            string answer = "";

            if (line.questionType == QuestionType.Who) answer = whoInputField.text.Trim();
            else if (line.questionType == QuestionType.Where) answer = whereInputField.text.Trim();
            else if (line.questionType == QuestionType.Do) answer = doInputField.text.Trim();
            else if (line.questionType == QuestionType.Complement) answer = complementInputField.text.Trim();
            else if (line.questionType == QuestionType.Criticism) answer = criticismInputField.text.Trim();

            // if empty dont go next
            if (string.IsNullOrEmpty(answer))
            {
                Debug.Log("cant put empty input!");
                return;
            }

            if (line.questionType == QuestionType.Who) whoList.Add(answer);
            else if (line.questionType == QuestionType.Where) whereList.Add(answer);
            else if (line.questionType == QuestionType.Do) doList.Add(answer);
            else if (line.questionType == QuestionType.Complement) complementList.Add(answer);
            else if (line.questionType == QuestionType.Criticism) criticismList.Add(answer);
        }

        currentLine++;
        ShowLine();
    }

    private void HideAllInputs()
    {
        whoInputField.gameObject.SetActive(false);
        whereInputField.gameObject.SetActive(false);
        doInputField.gameObject.SetActive(false);
        complementInputField.gameObject.SetActive(false);
        criticismInputField.gameObject.SetActive(false);
    }

    // Dropdown type
    private void ShowDropdown(QuestionType type)
    {
        TMP_Dropdown targetDropdown = null;
        List<string> targetList = null;

        if (type == QuestionType.Complement)
        {
            targetDropdown = complementDropdown;
            targetList = complementList;
        }
        else if (type == QuestionType.Criticism)
        {
            targetDropdown = criticismDropdown;
            targetList = criticismList;
        }

        if (targetDropdown != null && targetList != null)
        {
            targetDropdown.ClearOptions();
            targetDropdown.AddOptions(targetList);
            targetDropdown.gameObject.SetActive(true);

            targetDropdown.onValueChanged.RemoveAllListeners();
            targetDropdown.onValueChanged.AddListener((index) =>
            {
                string selected = targetList[index];
                Debug.Log(type + " selected: " + selected);

                // after choose, hide dropdown
                targetDropdown.gameObject.SetActive(false);

                // next dialogue
                currentLine++;
                ShowLine();
            });
        }
    }
}
