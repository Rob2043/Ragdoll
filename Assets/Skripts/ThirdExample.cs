using UnityEngine;

public class ThirdExample : MonoBehaviour
{
    [SerializeField] private Enemy enemy;

    private void Awake()
    {
        enemy.Initializing();
    }
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
            enemy.StartRun();
        if (Input.GetKeyUp(KeyCode.F))
            enemy.StanUp();
    }
}
