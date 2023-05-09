using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walk : MonoBehaviour
{
    private Animator walkAnimation;
    // Start is called before the first frame update
    void Start()
    {
        walkAnimation = GetComponent<Animator>();

    }

    // Update is called once per frame
    // void Update()
    // {
    //     if (walkAnimation != null)
    //     {
    //         if (Input.GetKey(KeyCode.W))
    //         {
    //             walkAnimation.SetTrigger("Forward");
    //         }

    //         if (Input.GetKey(KeyCode.D))
    //         {
    //             walkAnimation.SetTrigger("Right");
    //         }
    //         if (Input.GetKey(KeyCode.S))
    //         {
    //             walkAnimation.SetTrigger("Backwards");
    //         }
    //         if (Input.GetKey(KeyCode.A))
    //         {
    //             walkAnimation.SetTrigger("Left");
    //         }
    //     }
    // }

    void Update()
    {
        if (walkAnimation == null)
        {
            return;
        }

        var keyMap = new Dictionary<KeyCode, string>
    {
        { KeyCode.W, "Forward" },
        { KeyCode.D, "Right" },
        { KeyCode.S, "Backwards" },
        { KeyCode.A, "Left" }
    };

        foreach (var kvp in keyMap)
        {
            if (Input.GetKey(kvp.Key))
            {
                walkAnimation.SetBool(kvp.Value, true);
            }
            else
            {
                walkAnimation.SetBool(kvp.Value, false);
            }
        }
    }

}
