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
            
            private RectTransform AppendMessage(ChatMessage message)
            {
                scroll.content.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 0);

                var item = Instantiate(message.Role == "user" ? sent : received, scroll.content);
                
                if (message.Role != "user")
                {
                    messageRect = item;
                }
                
                item.GetChild(0).GetChild(0).GetComponent<Text>().text = message.Content;
                item.anchoredPosition = new Vector2(0, -height);

                if (message.Role == "user")
                {
                    LayoutRebuilder.ForceRebuildLayoutImmediate(item);
                    height += item.sizeDelta.y;
                    scroll.content.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
                    scroll.verticalNormalizedPosition = 0;
                }

                return item;
            }

            private void SendReply()
            {
                button.interactable = false;
                SendReply(inputField.text);
            }

            public void SendReply(string input)
            {
                var message = new ChatMessage()
                {
                    Role = "user",
                    Content = input
                };
                messages.Add(message);

                openai.CreateChatCompletionAsync(new CreateChatCompletionRequest()
                {
                    Model = "gpt-4",
                    Messages = messages
                }, OnResponse, OnComplete, new CancellationTokenSource());

                AppendMessage(message);
                
                inputField.text = "";
            }
            private void OnResponse(List<CreateChatCompletionResponse> responses)
            {
                var text = string.Join("", responses.Select(r => r.Choices[0].Delta.Content));
                
                if (string.IsNullOrEmpty(text)) return;

                if (text.Contains("END_CONVO"))
                {
                    text = text.Replace("END_CONVO", "");
                    Invoke(nameof(EndConvo), 5);
                }
                
                var message = new ChatMessage()
                {
                    Role = "assistant",
                    Content = text
                };
                messages.Add(message);


                if (isDone)
                {
                    // OnReplyReceived.Invoke();
                    messageRect = AppendMessage(message);
                    isDone = false;
                }
                
                messageRect.GetChild(0).GetChild(0).GetComponent<Text>().text = text;
                LayoutRebuilder.ForceRebuildLayoutImmediate(messageRect);
                scroll.content.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
                scroll.verticalNormalizedPosition = 0;

                // Mark response handling as complete
                response = text;
                Debug.Log(response);

                // Check if all responses have been received
                
            }

            // private void OnComplete()
            // {
            //     if (responseCount < expectedResponses)
            //     {
            //         Debug.Log("OnComplete called prematurely.");
            //         return;
            //     }

            //     Debug.Log("Response handling complete.");
            //     Debug.Log(response);
            //     button.interactable = true;
            //     LayoutRebuilder.ForceRebuildLayoutImmediate(messageRect);
            //     height += messageRect.sizeDelta.y;
            //     scroll.content.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
            //     scroll.verticalNormalizedPosition = 0;
                
            //     isDone = true;
            //     response = "";
            // }
            private void OnComplete()
            {
                // Delay the execution of the completion logic
                Invoke("CompleteFinalActions", 5.0f); // Adjust the time as needed
            }

            private void CompleteFinalActions()
            {

                Debug.Log("Response handling complete. All parts received.");
                button.interactable = true;
                LayoutRebuilder.ForceRebuildLayoutImmediate(messageRect);
                height += messageRect.sizeDelta.y;
                scroll.content.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
                scroll.verticalNormalizedPosition = 0;

                isDone = true;
                response = "";
            }


            private void EndConvo()
            {
                toActivate.SetActive(false);
            }
        }
    }
