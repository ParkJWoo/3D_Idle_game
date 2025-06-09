using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class StageSelectButton : MonoBehaviour
{
   public void LoadNormalStage()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void LoadAdvancedStage()
    {
        SceneManager.LoadScene("AdvancedGameScene");
    }
}
