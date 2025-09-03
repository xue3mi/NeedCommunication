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
        public bool isQuestion;
        public string questionType; // who / where / do
    }

    public TextMeshProUGUI dialogueText;
    public Button nextButton;

    [Header("Input Fields")]
    public TMP_InputField whoInputField;
    public TMP_InputField whereInputField;
    public TMP_InputField doInputField;

    public TMP_InputField complementInputField;
    public TMP_InputField criticismInputField;

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
        ShowLine();
    }

    void ShowLine()
    {
        if (currentLine >= dialogueLines.Count)
        {
            dialogueText.text = "conversation ends";
            HideAllInputs();
            nextButton.interactable = false;
            return;
        }

        DialogueLine line = dialogueLines[currentLine];
        dialogueText.text = line.text;

        HideAllInputs();

        if (line.isQuestion)
        {
            TMP_InputField activeInput = null;

            if (line.questionType == "who") activeInput = whoInputField;
            else if (line.questionType == "where") activeInput = whereInputField;
            else if (line.questionType == "do") activeInput = doInputField;
            else if (line.questionType == "complement") activeInput = complementInputField;
            else if (line.questionType == "criticism") activeInput = criticismInputField;


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
        else
        {
            nextButton.interactable = true; // dialogue doesnt get affected, can go next
        }
    }

    public void OnNextButton()
    {
        DialogueLine line = dialogueLines[currentLine];

        if (line.isQuestion)
        {
            string answer = "";

            if (line.questionType == "who") answer = whoInputField.text.Trim();
            else if (line.questionType == "where") answer = whereInputField.text.Trim();
            else if (line.questionType == "do") answer = doInputField.text.Trim();
            else if (line.questionType == "complement") answer = complementInputField.text.Trim();
            else if (line.questionType == "criticism") answer = criticismInputField.text.Trim();


            // if empty dont go next
            if (string.IsNullOrEmpty(answer))
            {
                Debug.Log("cant put empty input!");
                return;
            }

            if (line.questionType == "who") whoList.Add(answer);
            else if (line.questionType == "where") whereList.Add(answer);
            else if (line.questionType == "do") doList.Add(answer);
            else if (line.questionType == "complement") complementList.Add(answer);
            else if (line.questionType == "criticism") criticismList.Add(answer);
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
}
