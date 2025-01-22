using System;
using UnityEngine;

public class Mover : MonoBehaviour
{
    [SerializeField,Range(0,20)] private float _speed;
    [SerializeField] private Vector3 _direction;
    private bool isEnable;

    private void Update() {
        if(isEnable == false)
            return;
        transform.Translate(_direction * _speed * Time.deltaTime);
    }

    public void Enable() => isEnable = true;
    public void Disable() => isEnable = false;
}

