# How to start

1. Obtain an **OpenTrack API key**.
2. For the OpenTrack API to be able to reach the webhook client example app running on your **dev machine**, you need to setup a tunnel from the Internet to your **dev machine**. To do so, sign up with [ngrok](https://ngrok.com/) for free or sign in if you already have an **ngrok** account.
3. From the root folder of the solution, call `docker compose up --build`.
4. Open http://localhost:4040/ (ngrok's status page) and grab the link which looks like `https://a385293972ff.ngrok.io`.
5. Register the webhook client example app by posting your **OpenTrack API key** and the app's public address (e.g. `https://a385293972ff.ngrok.io`) to https://api.opentrack.co/v1/webhooks. Be sure to use the same value for the `secret` in this request and in the **.env** file in the root of the solution.

    ```
    curl --location --request POST 'https://api.opentrack.co/v1/webhooks' \
    --header 'Opentrack-API-Key: Your OpenTrack API key' \
    --header 'Content-Type: application/json' \
    --data-raw '{
        "url": "https://a385293972ff.ngrok.io/webhook",
        "events": [
            "container.status.updated",
            "container.itinerary.updated",
            "container.location.updated",
            "container.holds.updated",
            "container.demurrage.updated",
            "container.tracking.failed"
        ],
        "secret": "Very Secret Service",
        "name": "Your test webhook name"
    }'
    ```

6. The webhook client example app is ready to receive requests from the OpenTrack API.