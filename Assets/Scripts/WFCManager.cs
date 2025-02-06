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

    public Button bgColorBtn;
    public Button cubeColorBtn;
    public GameObject bgColorPicker;
    public GameObject cubeColorPicker;
    private bool ifBGColorBtnActivated = false;
    private bool ifCubeColorBtnActivated = false;

    private FlexibleColorPicker bgFlexibleColorPicker;
    private FlexibleColorPicker cubeFlexibleColorPicker;
    public Material[] cubeMats;

    void Start()
    {
        startBtn.onClick.AddListener(waveFunction.StartWaveFunctionCollapse);
        reloadBtn.onClick.AddListener(ReloadScene);
        bgColorBtn.onClick.AddListener(ActivateBGColorPicker);
        cubeColorBtn.onClick.AddListener(ActivateCubeColorPicker);

        bgFlexibleColorPicker = bgColorPicker.GetComponent<FlexibleColorPicker>();
        cubeFlexibleColorPicker = cubeColorPicker.GetComponent<FlexibleColorPicker>();
    }

    void Update()
    {
        foreach (Material mat in cubeMats)
        {
            mat.color = cubeFlexibleColorPicker.color;
        }

        Camera.main.backgroundColor = bgFlexibleColorPicker.color;
    }

    void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void ActivateBGColorPicker()
    {
        if (!ifBGColorBtnActivated)
        {
            bgColorPicker.SetActive(true);
            cubeColorPicker.SetActive(false);
            ifBGColorBtnActivated = true;
        }
        else
        {
            bgColorPicker.SetActive(false);
            ifBGColorBtnActivated = false;
        }
    }

    void ActivateCubeColorPicker()
    {
        if (!ifCubeColorBtnActivated)
        {
            cubeColorPicker.SetActive(true);
            bgColorPicker.SetActive(false);
            ifCubeColorBtnActivated = true;
        }
        else
        {
            cubeColorPicker.SetActive(false);
            ifCubeColorBtnActivated = false;
        }
    }
}
