using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using OpenAi;
using UnityEngine.SceneManagement;



public class Text_area : MonoBehaviour
{
    public  TMP_Text tm;
    public TMP_Text tm2;
    public Button change;
    public Button chat_redirect;
    public string second_scene;

    // for configuration of API keys
    public Configuration configuration;


    OpenAiApi gpt = new OpenAiApi();
    // Start is called before the first frame update

    async void Start()
    {


        Configuration.GlobalConfig = OpenAiApi.ReadConfigFromUserDirectory();
        Button btn = change.GetComponent<Button>();
        btn.onClick.AddListener(change_text);


        Button chat_change = chat_redirect.GetComponent<Button>();
        chat_change.onClick.AddListener(Change_screen);
        tm.text = "20+10"; // Maths Questioning Area
        //Debug.Log(tm);
        tm2.text = "Answering Area"; // solutation area
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private async void change_text()
    {
        
        string prompt = tm.text;
        var completion = await gpt.CreateCompletion(prompt);
        Debug.Log(completion.Text);
        tm2.text = completion.Text;
    }

    void Change_screen()
    {
        SceneManager.LoadScene(second_scene);
    }
}
