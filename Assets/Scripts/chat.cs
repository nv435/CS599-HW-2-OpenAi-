using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;
using UnityEngine.UI;
using Newtonsoft.Json;
using System.Collections.Generic;
using TMPro;


public class chat : MonoBehaviour
{
    private string api_key = "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";
    public const string API_URL = "https://api.openai.com/v1/chat/completions";
    public const string Model = "gpt-3.5-turbo";
    public int timeBetweenRequests; //time in seconds
    private float lastRequestTime;
    public GameObject Query;
    public GameObject Conversation_input;
    private string conversation = "You : ";
    private string query_string = "";
    void Start()
    {

        lastRequestTime = Time.time;

        



    }

    // Update is called once per frame
    void Update()
    {
        Conversation_input.GetComponent<TMP_Text>().text = conversation;

        if (Input.GetKeyDown(KeyCode.Return))
        {
            
            //Debug.Log(Query.GetComponent<InputField>().text);

            query_string = Query.GetComponent<TMP_InputField>().text;
            conversation += query_string + "\n";
            StartCoroutine(SendRequestToChatGpt());
            Query.GetComponent<TMP_InputField>().text = "";



        }
    }

    private IEnumerator SendRequestToChatGpt()
    {

        // Wait for the time between requests to pass
        if (Time.time - lastRequestTime < timeBetweenRequests)
        {
            yield return new WaitForSeconds(timeBetweenRequests);
        }

        // Create the request
        UnityWebRequest request = new UnityWebRequest("https://api.openai.com/v1/chat/completions", "POST");
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Authorization", "Bearer " + api_key);

        // Create the request body
        var requestBody = new
        {
            model = Model,
            messages = new[]
            {
            new { role = "user", content = query_string }
        }
        };

        string jsonRequestBody = JsonConvert.SerializeObject(requestBody);
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonRequestBody);

        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();

        // Send the request and wait for the response
        yield return request.SendWebRequest();

        // Check for errors
        if (request.result == UnityWebRequest.Result.ConnectionError ||
            request.result == UnityWebRequest.Result.ProtocolError ||
            request.result == UnityWebRequest.Result.DataProcessingError)
        {
            Debug.LogError(request.error);
        }
        else
        {
            // Parse the response to get the text
            string responseJson = request.downloadHandler.text;
            ResponseData responseData = JsonConvert.DeserializeObject<ResponseData>(responseJson);
            string text = responseData.choices[0].message.content;

            conversation += "Bot : " + text + '\n';
            conversation += " You : ";

            // Create a JSON object with the "role" and "content" keys
            string json = "{ \"messages\": [";
            json += "{\"role\": \"user\", \"content\": \"" + query_string + "\"},";
            json += "{\"role\": \"bot\", \"content\": \"" + text + "\"}";
            json += "] }";

            // Print the JSON object
            Debug.Log(json);
            Debug.Log("Response JSON: " + responseJson);

        }

        lastRequestTime = Time.time;
    }


}
[System.Serializable]
public class ResponseData
{
    public List<Choice> choices;
}

[System.Serializable]
public class Choice
{
    public MessageData message;
    public string finish_reason;
    public int index;
}

[System.Serializable]
public class MessageData
{
    public string role;
    public string content;
}