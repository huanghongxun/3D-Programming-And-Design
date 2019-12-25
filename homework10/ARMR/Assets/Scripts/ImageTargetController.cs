using UnityEngine;
using Vuforia;

public class ImageTargetController : MonoBehaviour
{
    public GameObject anime;
    private bool floating = false;
    private bool running = false;

    // Start is called before the first frame update
    void Start()
    {
        var animator = anime.GetComponent<Animator>();
        foreach (var virtualButton in GetComponentsInChildren<VirtualButtonBehaviour>())
        {
            virtualButton.RegisterOnButtonPressed(vb =>
            {
                switch (vb.VirtualButtonName)
                {
                    case "FloatButton":
                        animator.SetBool("Float", floating = !floating);
                        Debug.Log("Floating: " + floating);
                        break;
                    case "JumpButton":
                        animator.SetTrigger("Jump");
                        Debug.Log("Jump");
                        break;
                    case "RunButton":
                        running = !running;
                        animator.SetFloat("MoveSpeed", running ? 10 : 0);
                        Debug.Log("Running: " + running);
                        break;
                }
            });
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
