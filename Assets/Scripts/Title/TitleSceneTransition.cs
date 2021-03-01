using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RAF.Title
{
    public class TitleSceneTransition : MonoBehaviour
    {
        public void OnClickStartButton()
        {
            SceneManager.LoadScene("SampleScene");
        }
    }
}