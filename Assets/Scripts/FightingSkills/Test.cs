using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{

    public CharacterController controller;
    public GameObject animationController;
    // Start is called before the first frame update
    void Start()
    {
        UnityEngine.Debug.Log(animationController.transform.position);
        UnityEngine.Debug.Log(transform.position);
        UnityEngine.Debug.Log(animationController.transform.position- transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Testing() {

        UnityEngine.Debug.Log(animationController.transform.position);
        UnityEngine.Debug.Log(transform.position);
        UnityEngine.Debug.Log(animationController.transform.position- transform.position);
        controller.Move(animationController.transform.position - transform.position);
    }
}
