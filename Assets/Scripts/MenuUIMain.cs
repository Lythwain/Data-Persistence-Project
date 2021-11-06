using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif
public class MenuUIMain : MonoBehaviour
{
    private void Start()
    {
        UpdateNameField();
    }
    private bool CheckPlayerNameEntered()
    {
        GameObject obj = GameObject.Find("Name");
        if (obj == null)
        {
            Debug.Log("UI component (Name) not found.");
            return false;
        }
        UnityEngine.UI.InputField input = obj.GetComponent<InputField>();
        if(input.text.Length == 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
    public void Play()
    {
        if(!CheckPlayerNameEntered())
        {
            return;
        }
        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }
    public void Exit()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit(); // original code to quit Unity player
#endif
    }
    public void EnterName()
    {
        GameObject obj = GameObject.Find("Name");
        
        if(obj == null)
        {
            Debug.Log("UI component (Name) not found.");
            return;
        }
        UnityEngine.UI.InputField input = obj.GetComponent<InputField>();
        DataManager.Instance.playerName = input.text;
    }
    public void UpdateNameField()
    {
        GameObject obj = GameObject.Find("Name");

        if (obj == null)
        {
            Debug.Log("UI component (Name) not found.");
            return;
        }
        UnityEngine.UI.InputField input = obj.GetComponent<InputField>();
        input.text = DataManager.Instance.playerName;
    }
}
