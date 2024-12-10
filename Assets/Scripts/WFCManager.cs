using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WFCManager : MonoBehaviour
{
    public Button startBtn;
    public Button reloadBtn;
    public WaveFunctionCollapse waveFunction;

    void Start()
    {
        startBtn.onClick.AddListener(waveFunction.StartWaveFunctionCollapse);
        reloadBtn.onClick.AddListener(ReloadScene);
    }

    void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
