using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMenu : Singleton<UIMenu>
{
    public bool MenuOpen { get => _menuOpen; }
    private CanvasGroup menu;
    private bool _menuOpen;

    protected override void Awake()
    {
        base.Awake();
        menu = GetComponent<CanvasGroup>();
        CloseMenu();
    }

    // Update is called once per frame
    void Update()
    {
        if(!_menuOpen)
        {
            if(Input.GetButtonDown("Cancel"))
            {
                OpenMenu();
            }
        }
        else
        {
            if (Input.GetButtonDown("Cancel") || Input.GetButtonDown("Dash"))
            {
                CloseMenu();
            }

            if (Input.GetButtonDown("Confirm"))
            {
                Application.Quit();
            }
        }
    }

    void OpenMenu()
    {
        menu.alpha = 1;
        _menuOpen = true;
        Time.timeScale = 0;
    }

    void CloseMenu()
    {
        menu.alpha = 0;
        _menuOpen = false;
        Time.timeScale = 1;
    }
}
