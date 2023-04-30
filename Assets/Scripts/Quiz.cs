using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OpenAi;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine.SceneManagement;
public class Quiz : MonoBehaviour
{

    OpenAiApi gpt = new OpenAiApi();
    public string Theam="Science";
    public GameObject image_screen;
    public Button button1;
    public Button button2;
    public Button button3;
    public Button button4;
    public Button next_question;
    public Button quit;
    private int answer_option;

    Regex regex = new Regex("t([0-9])");
    async void Start()
    {
        Configuration.GlobalConfig = OpenAiApi.ReadConfigFromUserDirectory();
        button1.onClick.AddListener(option1);
        button2.onClick.AddListener(option2);
        button3.onClick.AddListener(option3);
        button4.onClick.AddListener(option4);
        next_question.onClick.AddListener(next_question_funct);
        quit.onClick.AddListener(home);
        create_question();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    async void create_question()
    {
        var question_resp = await gpt.CreateCompletion("Image of anything in " + Theam + " With 4 options ");
        string questions = question_resp.Text;
        string[] options = Regex.Replace(questions, "[0-9]{}", "").Split("\n");
       

        int random_question = (int)Random.Range(0, 3);
        answer_option = random_question;


        var image = await gpt.CreateImage(options[random_question]);
        Texture2D texture = image.Texture;
        image_screen.GetComponent<RawImage>().texture = texture;
        button1.GetComponentInChildren<TMP_Text>().text = options[0];
        button2.GetComponentInChildren<TMP_Text>().text = options[1];
        button3.GetComponentInChildren<TMP_Text>().text = options[2];
        button4.GetComponentInChildren<TMP_Text>().text = options[3];
    }
    void option1()
    {
        if(answer_option == 0)
        {
            button1.GetComponent<Image>().color = Color.green;
        }
        else
        {
            button1.GetComponent<Image>().color = Color.red;
        }
    }

    void option2()
    {
        if (answer_option == 1)
        {
            button2.GetComponent<Image>().color = Color.green;
        }
        else
        {
            button2.GetComponent<Image>().color = Color.red;
        }
    }

    void option3()
    {
        if (answer_option == 2)
        {
            button3.GetComponent<Image>().color = Color.green;
        }
        else
        {
            button3.GetComponent<Image>().color = Color.red;
        }
    }

    void option4()
    {
        if (answer_option == 3)
        {
            button4.GetComponent<Image>().color = Color.green;
        }
        else
        {
            button4.GetComponent<Image>().color = Color.red;
        }
    }
    void next_question_funct()
    {
        reset_color();
        create_question();
    }
    void reset_color()
    {
        button1.GetComponent<Image>().color = Color.white;
        button2.GetComponent<Image>().color = Color.white;
        button3.GetComponent<Image>().color = Color.white;
        button4.GetComponent<Image>().color = Color.white;
    }
    void home()
    {
        SceneManager.LoadScene("Main Scene");
    }
}
