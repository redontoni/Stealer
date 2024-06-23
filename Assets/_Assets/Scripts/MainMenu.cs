using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();    
    }

    public void StartButton()
    {
        animator.SetBool("ChangeScene", true);
    }

    public void ChangeScene()
    {
        SceneManager.LoadScene("GameScene");
    }

}
