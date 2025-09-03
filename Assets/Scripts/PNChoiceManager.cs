using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ChoiceManager : MonoBehaviour
{
    public Button complementButton;
    public Button criticismButton;

    public TMP_Dropdown complementDropdown;
    public TMP_Dropdown criticismDropdown;

    public List<string> complementList = new List<string>();
    public List<string> criticismList = new List<string>();

    void Start()
    {
        complementButton.onClick.AddListener(() => ShowDropdown("complement"));
        criticismButton.onClick.AddListener(() => ShowDropdown("criticism"));

        complementDropdown.gameObject.SetActive(false);
        criticismDropdown.gameObject.SetActive(false);
    }

    void ShowDropdown(string type)
    {
        TMP_Dropdown targetDropdown = null;
        List<string> targetList = null;

        if (type == "complement")
        {
            targetDropdown = complementDropdown;
            targetList = complementList;
        }
        else if (type == "criticism")
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

                // hide after choose dropdown
                targetDropdown.gameObject.SetActive(false);
            });
        }
    }
}
