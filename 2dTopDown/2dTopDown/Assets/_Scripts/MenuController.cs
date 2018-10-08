using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    public GameObject magicMenu;

    private void Awake()
    {
        magicMenu.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            magicMenu.SetActive(!magicMenu.activeSelf);
        }
    }
}
