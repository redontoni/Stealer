using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton : MonoBehaviour
{
    public static Singleton instance;
    public GameObject player;
    public PlayerController playerController;
    public AnimationClip animationToControlPickLeft;
    public AnimationClip animationToControlPickRight;
    public AnimationClip animationToControlPickDownLeft;
    public AnimationClip animationToControlPickDownRight;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

}
