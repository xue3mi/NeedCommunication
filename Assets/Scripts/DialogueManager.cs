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
        public LineType lineType; // Normal, Question, Choice, Dropdown, MultiDropdown, Replace
        public QuestionType questionType; // Who, Where, Do, Complement, Criticism

        public string replaceWith;
        public bool shouldReplace; // replace sentence along with prefab
    }

    public enum LineType { Normal, Question, Choice, Dropdown, MultiDropdown, Replace }
    public enum QuestionType { None, Who, Where, Do, Complement, Criticism }

    public TextMeshProUGUI dialogueText;
    public Button nextButton;

    [Header("Whole Dialogue Panel")]
    public GameObject dialoguePanel;

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
    public TMP_Dropdown whoDropdown;
    public TMP_Dropdown whereDropdown;
    public TMP_Dropdown doDropdown;

    [Header("Choice Panel + Button Prefab")]
    public Transform choicePanel;
    public GameObject playersChoicePrefab;

    [Header("Player Prefab")]
    public GameObject playerPrefab;
    public float spawnMinX = -15f;
    public float spawnMaxX = 15f;
    public float spawnMinY = -20f;
    public float spawnMaxY = 20f;

    public List<DialogueLine> dialogueLines;
    private int currentLine = 0;
    private bool isDialogueActive = true; // if dialogue is going

    [Header("Player Answers")]
    public List<string> whoList = new List<string>();
    public List<string> whereList = new List<string>();
    public List<string> doList = new List<string>();
    public List<string> complementList = new List<string>();
    public List<string> criticismList = new List<string>();

    // store player answers for MultiDropdown
    private string selectedWho = "";
    private string selectedWhere = "";
    private string selectedDo = "";

    private PlayerData currentPlayerData;

    void Start()
    {
        HideAllInputs();
        complementButton.gameObject.SetActive(false);
        criticismButton.gameObject.SetActive(false);
        complementDropdown.gameObject.SetActive(false);
        criticismDropdown.gameObject.SetActive(false);
        whoDropdown.gameObject.SetActive(false);
        whereDropdown.gameObject.SetActive(false);
        doDropdown.gameObject.SetActive(false);

        // clicked button to show dropdown
        complementButton.onClick.AddListener(() =>
        {
            ShowDropdown(QuestionType.Complement);
            if (currentPlayerData != null) 
                currentPlayerData.SetFaceSmile();
        });

        criticismButton.onClick.AddListener(() =>
        {
            ShowDropdown(QuestionType.Criticism);
            if (currentPlayerData != null)
                currentPlayerData.SetFaceAngry();
        });

    }

    public void OpenDialogue()
    {
        if (dialoguePanel != null)
            dialoguePanel.SetActive(true);

        StartDialogue();
    }

    public void StartDialogue()
    {
        isDialogueActive = true;
        currentLine = 0;

        dialoguePanel.SetActive(true);
        dialogueText.gameObject.SetActive(true);
        nextButton.gameObject.SetActive(true);

        ShowLine();
    }

    void ShowLine()
    {
        if (!isDialogueActive) return;

        if (currentLine >= dialogueLines.Count)
        {
            EndDialogue();
            return;
        }

        DialogueLine line = dialogueLines[currentLine];

        if (!string.IsNullOrEmpty(line.replaceWith))
        {
            dialogueText.text = line.replaceWith;
        }
        else
        {
            dialogueText.text = line.text;
        }

        HideAllInputs();
        complementButton.gameObject.SetActive(false);
        criticismButton.gameObject.SetActive(false);
        complementDropdown.gameObject.SetActive(false);
        criticismDropdown.gameObject.SetActive(false);
        whoDropdown.gameObject.SetActive(false);
        whereDropdown.gameObject.SetActive(false);
        doDropdown.gameObject.SetActive(false);

        if (line.lineType == LineType.Normal)
        {
            nextButton.interactable = true;
        }
        else if (line.lineType == LineType.MultiDropdown)
        {
            whoDropdown.gameObject.SetActive(true);
            whereDropdown.gameObject.SetActive(true);
            doDropdown.gameObject.SetActive(true);

            whoDropdown.ClearOptions();
            whoDropdown.AddOptions(whoList);

            whereDropdown.ClearOptions();
            whereDropdown.AddOptions(whereList);

            doDropdown.ClearOptions();
            doDropdown.AddOptions(doList);

            whoDropdown.onValueChanged.RemoveAllListeners();
            whereDropdown.onValueChanged.RemoveAllListeners();
            doDropdown.onValueChanged.RemoveAllListeners();

            whoDropdown.onValueChanged.AddListener((i) => { selectedWho = whoList[i]; CheckAllSelected(); });
            whereDropdown.onValueChanged.AddListener((i) => { selectedWhere = whereList[i]; CheckAllSelected(); });
            doDropdown.onValueChanged.AddListener((i) => { selectedDo = doList[i]; CheckAllSelected(); });

            nextButton.interactable = false;
        }
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
                // save previous input
                activeInput.ActivateInputField();
                nextButton.interactable = !string.IsNullOrWhiteSpace(activeInput.text);

                activeInput.onValueChanged.RemoveAllListeners();
                activeInput.onValueChanged.AddListener((value) =>
                {
                    nextButton.interactable = !string.IsNullOrWhiteSpace(value);
                });
            }
        }
        else if (line.lineType == LineType.Choice)
        {
            complementButton.gameObject.SetActive(true);
            criticismButton.gameObject.SetActive(true);
            nextButton.interactable = false; // wait player to click
        }
        else if (line.lineType == LineType.Dropdown)
        {
            ShowDropdown(line.questionType);
        }
    }

    public void OnNextButton()
    {
        if (!isDialogueActive) return;

        DialogueLine line = dialogueLines[currentLine];

        if (line.isQuestion)
        {
            // handle MultiDropdown
            if (line.lineType == LineType.MultiDropdown)
            {
                // lowercase everything
                selectedWho = selectedWho.ToLower();
                selectedWhere = selectedWhere.ToLower();
                selectedDo = selectedDo.ToLower();

                Debug.Log($"Player chose MultiDropdown: {selectedWho}, {selectedWhere}, {selectedDo}");

                if (!whoList.Contains(selectedWho)) whoList.Add(selectedWho);
                if (!whereList.Contains(selectedWhere)) whereList.Add(selectedWhere);
                if (!doList.Contains(selectedDo)) doList.Add(selectedDo);
            }
            else
            {
                string answer = "";

                if (line.questionType == QuestionType.Who)
                {
                    answer = whoInputField.text.Trim();
                    selectedWho = answer.ToLower();
                    if (!whoList.Contains(selectedWho)) whoList.Add(selectedWho);
                }
                else if (line.questionType == QuestionType.Where)
                {
                    answer = whereInputField.text.Trim();
                    selectedWhere = answer.ToLower();
                    if (!whereList.Contains(selectedWhere)) whereList.Add(selectedWhere);
                }
                else if (line.questionType == QuestionType.Do)
                {
                    answer = doInputField.text.Trim();
                    selectedDo = answer.ToLower();
                    if (!doList.Contains(selectedDo)) doList.Add(selectedDo);
                }
                else if (line.questionType == QuestionType.Complement)
                {
                    // complementList
                    answer = complementInputField.text.Trim();
                    if (!string.IsNullOrEmpty(answer) && !complementList.Contains(answer.ToLower()))
                        complementList.Add(answer.ToLower());
                }
                else if (line.questionType == QuestionType.Criticism)
                {
                    // criticismList
                    answer = criticismInputField.text.Trim();
                    if (!string.IsNullOrEmpty(answer) && !criticismList.Contains(answer.ToLower()))
                        criticismList.Add(answer.ToLower());
                }

                // if empty, do not proceed
                if (string.IsNullOrEmpty(answer))
                {
                    Debug.Log("cant put empty input!");
                    return;
                }
            }
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

        whoDropdown.gameObject.SetActive(false);
        whereDropdown.gameObject.SetActive(false);
        doDropdown.gameObject.SetActive(false);
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

            criticismDropdown.gameObject.SetActive(false);
            criticismButton.gameObject.SetActive(false);
        }
        else if (type == QuestionType.Criticism)
        {
            targetDropdown = criticismDropdown;
            targetList = criticismList;

            complementDropdown.gameObject.SetActive(false);
            complementButton.gameObject.SetActive(false);
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

                // dropdown add to list
                if (currentPlayerData != null)
                {
                    string normalized = selected.ToLower();

                    if (type == QuestionType.Complement)
                    {
                        if (!currentPlayerData.chosenComplements.Contains(normalized))
                            currentPlayerData.chosenComplements.Add(normalized);
                    }
                    else if (type == QuestionType.Criticism)
                    {
                        if (!currentPlayerData.chosenCriticisms.Contains(normalized))
                            currentPlayerData.chosenCriticisms.Add(normalized);
                    }
                }

                // create a visual button (attached to choicePanel)
                CreateChoiceButton(selected);

                targetDropdown.gameObject.SetActive(false);

                currentLine++;
                ShowLine();
            });
        }
    }

    // create a PlayersChoice button under choicePanel and set its label
    private void CreateChoiceButton(string text)
    {
        if (playersChoicePrefab == null || choicePanel == null) return;

        GameObject newBtn = Instantiate(playersChoicePrefab, choicePanel);
        TextMeshProUGUI btnText = newBtn.GetComponentInChildren<TextMeshProUGUI>();
        if (btnText != null) btnText.text = text;
    }

    // after finish dialogue, reset dialogue
    private void EndDialogue()
    {
        isDialogueActive = false;

        dialoguePanel.SetActive(false);
        dialogueText.gameObject.SetActive(false);
        nextButton.gameObject.SetActive(false);

        HideAllInputs();
        complementButton.gameObject.SetActive(false);
        criticismButton.gameObject.SetActive(false);
        complementDropdown.gameObject.SetActive(false);
        criticismDropdown.gameObject.SetActive(false);

        // turn on camera WASD
        CameraController cameraController = FindFirstObjectByType<CameraController>();
        if (cameraController != null)
        {
            cameraController.EnableWASD();
        }

        SpawnNewPlayer();

        Debug.Log("Dialogue finished.");
    }

    public bool IsDialogueActive()
    {
        return isDialogueActive;
    }

    // check if all MultiDropdown selections are made
    private void CheckAllSelected()
    {
        nextButton.interactable = !string.IsNullOrEmpty(selectedWho) &&
                                  !string.IsNullOrEmpty(selectedWhere) &&
                                  !string.IsNullOrEmpty(selectedDo);
    }

    // spawn new player at random position in map
    private void SpawnNewPlayer()
    {
        if (playerPrefab == null) return;

        float x = Random.Range(spawnMinX, spawnMaxX);
        float y = Random.Range(spawnMinY, spawnMaxY);
        Vector3 spawnPos = new Vector3(x, y, 0);

        GameObject newPlayer = Instantiate(playerPrefab, spawnPos, Quaternion.identity);

        // get PlayerData
        currentPlayerData = newPlayer.GetComponent<PlayerData>();
        if (currentPlayerData == null)
        {
            currentPlayerData = newPlayer.AddComponent<PlayerData>();
        }

        // initialize SpriteRenderer
        currentPlayerData.playerRenderer = newPlayer.GetComponent<SpriteRenderer>();

        // default no face
        currentPlayerData.ClearFace();

        // only replace when prefab is empty
        if (string.IsNullOrEmpty(currentPlayerData.who) && !string.IsNullOrEmpty(selectedWho))
            currentPlayerData.who = selectedWho;

        if (string.IsNullOrEmpty(currentPlayerData.where) && !string.IsNullOrEmpty(selectedWhere))
            currentPlayerData.where = selectedWhere;

        if (string.IsNullOrEmpty(currentPlayerData.doAction) && !string.IsNullOrEmpty(selectedDo))
            currentPlayerData.doAction = selectedDo;

        // replace dialogueLines
        string finalSentence = currentPlayerData.GetSentence();
        foreach (var line in dialogueLines)
        {
            if (line.shouldReplace)
            {
                line.replaceWith = finalSentence;
            }
        }
        // add PlayerMovementAI for random movement
        PlayerMovementAI ai = newPlayer.GetComponent<PlayerMovementAI>();
        if (ai == null)
        {
            ai = newPlayer.AddComponent<PlayerMovementAI>();
        }
        ai.minX = spawnMinX;
        ai.maxX = spawnMaxX;
        ai.minY = spawnMinY;
        ai.maxY = spawnMaxY;

        Debug.Log($"New player spawned with dialogue: {finalSentence}");
    }
}
