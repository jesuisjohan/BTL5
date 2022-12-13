using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugController : MonoBehaviour
{
    private bool showConsole = true;
    private string input;

    public static DebugCommand PSYCHO_MODE;
    public static DebugCommand STOP_ENEMY;

    public List<object> commands;

    private void Awake()
    {
        PSYCHO_MODE = new DebugCommand("psycho_mode",
            "enhance shooting speed of the player",
            "psycho_mode",
            () =>
            {
                var players = FindObjectsOfType<PlayerShooting>();

                foreach (var p in players)
                {
                    if (!p.IsOwner && !SingleModeManager.Instance.isPlaying) continue;
                    p.ReduceCoolDown(10000);
                }
            });

        STOP_ENEMY = new DebugCommand("stop_enemy",
            "stop enemy in single play mode",
            "stop_enemy",
            () =>
            {
                if (SingleModeManager.Instance.isPlaying)
                {
                    var boss = SingleModeManager.Instance.boss;
                    var bossController = boss.GetComponent<BossController>();
                    var bossShooting = boss.GetComponent<PlayerShooting>();

                    bossController.IsMoving = false;
                }
            });

        commands = new List<object>
        {
            STOP_ENEMY, PSYCHO_MODE
        };
    }

    void Start()
    {
        showConsole = false;
    }

    void Update()
    {
        // if (Input.GetKeyDown(KeyCode.Slash))
        // {
        //     showConsole = !showConsole;
        // }

        if (Input.GetKeyDown(KeyCode.Backslash))
        {
            Debug.Log("Enter");

            OnReturn();
        }
    }

    private void OnGUI()
    {
        if (!showConsole) return;

        float y = 0f;
        GUI.Box(new Rect(0, y, Screen.width, 30), "");

        input = GUI.TextField(new Rect(10f, y + 5f, Screen.width - 20f, 20f), input);
    }

    public void OnReturn()
    {
        if (showConsole)
        {
            HandleInput();
            input = "";
        }
    }

    private void HandleInput()
    {
        for (int i = 0; i < commands.Count; i++)
        {
            var commandBase = commands[i] as DebugCommandBase;
            if (input.Contains(commandBase._commandId))
            {
                if (commands[i] as DebugCommand != null)
                {
                    ((commands[i] as DebugCommand)).Invoke();
                }
            }
        }
    }
}