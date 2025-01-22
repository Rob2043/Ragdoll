using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstExample : MonoBehaviour
{
    [SerializeField] private Enemy enemy;

    private void Update() {
        if(Input.GetKeyUp(KeyCode.F))
            enemy.Kill();
    }
}
