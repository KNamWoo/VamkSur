using UnityEngine;

public class Result : MonoBehaviour
{
    public GameObject[] titles;
    public GameObject[] texts;
    
    public void Lose()
    {
        titles[0].SetActive(true);
        texts[0].SetActive(true);
    }

    // Update is called once per frame
    public void Win()
    {
        titles[1].SetActive(true);
        texts[1].SetActive(true);
    }
}
