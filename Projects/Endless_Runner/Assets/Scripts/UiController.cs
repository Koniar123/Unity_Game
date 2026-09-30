using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UiController : MonoBehaviour
{
    Player player;
    Text distanceText;

    GameObject results;
    Text finalDistanceText;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        distanceText = GameObject.Find("DistanceText").GetComponent<Text>();

        finalDistanceText = GameObject.Find("FinalDistanceText").GetComponent<Text>();
        results = GameObject.Find("Results");
        results.SetActive(false);
    }
    void Start()
    {
        
    }

    void Update()
    {
        int distance = Mathf.FloorToInt(player.distance);
        distanceText.text = distance.ToString() + " m";

        AchievementService.UnlockDistanceMilestone(distance);

        if (player.isDead)
        {
            results.SetActive(true);
            finalDistanceText.text = distance.ToString() + " m";
        }
    }

    public void Quit()
    {
        SceneManager.LoadScene("MainMenu");

    }

    public void Restart()
    {
        SceneManager.LoadScene("SampleScene");
    }

}
