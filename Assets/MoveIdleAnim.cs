using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveIdleAnim : MonoBehaviour

{
    public float frequancy = 1;
    private List<Animator> _animators =  new List<Animator>();
    // Start is called before the first frame update
    void Start()
    {
        foreach(Animator a in GetComponentsInChildren<Animator>())
        {
            _animators.Add(a);
        }
    }

    // Update is called once per frame
    void Update()
    {
        foreach(Animator a in _animators)
        {
            a.SetBool("moving", Mathf.FloorToInt(Time.time/frequancy) % 2 == 0);
        }
    }
}
