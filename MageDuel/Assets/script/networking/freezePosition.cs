using UnityEngine;
using System.Collections;

public class freezePosition : MonoBehaviour {

    Vector3 stopPos;
	// Use this for initialization
	void Start () {
	
	}
	void OnEnable()
    {
        StartCoroutine(delay(7.0f));
    }
    IEnumerator delay(float _time)
    {
        yield return new WaitForSeconds(_time);
        GetComponent<freezePosition>().enabled = false;
    }
	// Update is called once per frame
	void Update () {

        transform.position = stopPos;
	}
    public void setPosition(Vector3 pos)
    {
        stopPos = pos;
    }
}
