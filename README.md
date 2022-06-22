# TwitterStats

This solution demonstrates the access to the Twitter API and streaming realtime sample data.
It uses AspNetCore SignalR technique to communicate between server and client. 
It has two SignalR Hubs for demo purposes – one the real-time streaming from Twitter and another one for streaming from pre-populated file of Twitter data. <br /><br />
To connect to the Twitter you will need a bearer token that you must obtain from Twitter. You will need to update appsettings.json 
![image](https://user-images.githubusercontent.com/43709394/175111830-0982d6f6-9ac9-416a-b3b3-8ec52a960845.png)<br /><br />

The "TwitterTweetsUrl" setting should be "https://api.twitter.com/2/tweets/sample/stream?tweet.fields=entities"<br /><br />

You can configure your Visual Studio solution to start as a multiple startup projects.
![image](https://user-images.githubusercontent.com/43709394/175111944-0978c809-846d-44ea-bc5c-2842a1b28c2e.png)

