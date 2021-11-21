using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    public void GOTO_StartScene()
    {
        SceneManager.LoadScene("StartScene");
    }
    public void GOTO_CharacterGenerationScene()
    {
        SceneManager.LoadScene("CharacterCustomizationScene");
    }
    public void GOTO_TutorialScene()
    {
        SceneManager.LoadScene("TutorialScene");
    }

    public void GameExit()
    {
        Application.Quit();
    }
}