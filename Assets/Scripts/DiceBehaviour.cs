using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceBehaviour : MonoBehaviour
{
    public int face;

    private GameManager gameManager;

    private void Start()
    {
        gameManager = GameManager.Instance;
    }
}
