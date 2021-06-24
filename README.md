# How to start

1. Obtain an **OpenTrack API key**.
2. For the OpenTrack API to be able to reach the webhook client example app running on your **dev machine**, you need to setup a tunnel from the Internet to your **dev machine**. To do so, this project uses **ngrok**.
3. From the root folder of the solution, call `docker compose up --build`.
4. Open http://localhost:4040/ (**ngrok**'s status page) and grab the link which looks like `https://a385293972ff.ngrok.io`.
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

5. The webhook client example app is ready to receive requests from the OpenTrack API.

# Notes

* If not signed in, **nrok**'s session and consequently the example app's link will only work for 2 hours. [Sign in](https://dashboard.ngrok.com/login), grab your **ngrok** authtoken [here](https://dashboard.ngrok.com/get-started/your-authtoken), and then paste it into the **.env** file in the root of the solution.
* Every time the **ngrok**'s container is restarted (e.g. after calling `docker compose up --build`), **ngrok** generates a new random public link. So for the webhook client example app to continue receiving requests from **OpenTrack API** after being restarted, you need to grab a newly generated public link from [**ngrok**'s status page](http://localhost:4040/) and update your webhook by following the instruction [here](https://dashboard.opentrack.co/api/beta/docs/developer#operation/updateWebhook).
