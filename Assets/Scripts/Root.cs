using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Root : MonoBehaviour
{
    [SerializeField] private ConfigManager configManager = null;
    [SerializeField] private CameraController cameraController = null;
    [SerializeField] private UIManager uiManager = null;
    [SerializeField] private PlayerInput playerInput = null;
    [SerializeField] private GridManager grid = null;
    
    private static Root _instance;

    private void Awake()
    {
        _instance = this;
    }

    private void Start()
    {
        cameraController.Init();
        uiManager.Init();
        grid.Init();
    }


    public static GridManager GridManager => _instance.grid;
    public static ConfigManager ConfigManager => _instance.configManager;
    public static CameraController CameraController => _instance.cameraController;
    public static UIManager UIManager => _instance.uiManager;
    public static PlayerInput PlayerInput => _instance.playerInput;
}
