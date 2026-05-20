using System.Collections;
using UnityEngine;

public class TestScripts : NewMonoBehaviour,IAnimation
{
    public IEnumerator PlayAnimationRoutine()
    {

        yield return new WaitForSeconds(1f);
        Debug.Log("PlayAnimationRoutine");
    }

    public void Test()
    {
        Debug.Log("Test");
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        StartCoroutine(PlayAnimationRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
