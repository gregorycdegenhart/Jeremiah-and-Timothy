using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AnimationEventScript : MonoBehaviour
{
    public PlayerMovement Playermovement;

    public void Reload()
    {
        SceneManager.LoadScene("GameOverScreen");
    }

}

