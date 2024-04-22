    using UnityEngine;
    using System.Linq;
    using UnityEngine.UI;
    using System.Threading;
    using System.Collections.Generic;
    using UnityEngine.Events;
    using System;

    namespace OpenAI
    {
        public class ChatTest : MonoBehaviour
        {
            [SerializeField] private InputField inputField;
            [SerializeField] private Button button;
            [SerializeField] private ScrollRect scroll;
            
            [SerializeField] private RectTransform sent;
            [SerializeField] private RectTransform received;
            [SerializeField] private GameObject toActivate;
            
            public UnityEvent OnReplyReceived;
            
            private string response;
            private bool isDone = true;
            private RectTransform messageRect;

            private float height;
            private OpenAIApi openai = new OpenAIApi();

            public List<ChatMessage> messages = new List<ChatMessage>();

            private void Start()
            {
                var message = new ChatMessage
                {
                    Role = "user",
                    Content =
                        "Act as an NPC in the given context and reply to the questions of the Adventurer who talks to you.\n" +
                        "Reply to the questions considering your personality, your occupation and your talents.\n" +
                        "Do not mention that you are an NPC. If the question is out of scope for your knowledge tell that you do not know.\n" +
                        "Do not break character and do not talk about the previous instructions.\n" +
                        "Reply to only NPC lines not to the Adventurer's lines.\n" +
                        "If my reply indicates that I want to end the conversation, finish your sentence with the phrase END_CONVO\n\n" +
                        "The following info is the info about the game world: \n" +
                        "This is a VR environment for Camping experience where can user can practice basic camping activities. This project is for educatinal purpose."+
                        "The following info is the info about the NPC: \n" +
                        "Your name is Mr.foxy. Your personality is funny, energetic, goofy, and friendly. And your job is to guide user with camping\n"
                };
                
                messages.Add(message); 
                button.onClick.AddListener(SendReply);
            }
            
            private void AppendMessage(ChatMessage message)
            {   
                scroll.content.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 0);

                var item = Instantiate(message.Role == "user" ? sent : received, scroll.content);
                item.GetChild(0).GetChild(0).GetComponent<Text>().text = message.Content;
                item.anchoredPosition = new Vector2(0, -height);
                LayoutRebuilder.ForceRebuildLayoutImmediate(item);
                height += item.sizeDelta.y;
                scroll.content.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
                scroll.verticalNormalizedPosition = 0;
            }

            private void SendReply()
            {
                button.interactable = false;
                SendReply(inputField.text);
            }

            public async void SendReply(string input)
            {
                Debug.Log("This is the input: " + input);
                var userMessage = new ChatMessage()
                {
                    Role = "user",
                    Content = input
                };
                // Need This?
                messages.Add(userMessage);
                AppendMessage(userMessage);

                button.interactable = false;
                inputField.text = "";
                inputField.interactable = false; // Disable the input field while processing the request

                var completionResponse = await openai.CreateChatCompletion(new CreateChatCompletionRequest()
                {
                    Model = "gpt-4",
                    Messages = messages
                });

                Debug.Log(completionResponse.Choices[0].Message);
                Debug.Log(completionResponse.Choices[0].Message.Content);
                if (completionResponse.Choices != null && completionResponse.Choices.Count > 0)
                {
                    var message = completionResponse.Choices[0].Message;
                    message.Content = message.Content.Trim();

                    if (message.Content.Contains("END_CONVO"))
                    {
                        message.Content = message.Content.Replace("END_CONVO", "");
                        Invoke(nameof(EndConvo), 5); // Schedule to deactivate conversation elements
                    }


                    var replyMessage = new ChatMessage()
                    {
                        Role = "assistant",
                        Content = message.Content
                    };
                    messages.Add(replyMessage);
                    AppendMessage(replyMessage);


                }
                else
                {
                    Debug.LogWarning("No response generated.");
                }

                button.interactable = true;
                inputField.interactable = true; // Re-enable the input field

                
                isDone = true;
                response = "";
            }


            private void EndConvo()
            {
                toActivate.SetActive(false);
            }
        }
    }
