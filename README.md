# NileCat
WPF app that displays images of cats using a web api.
## Web api
- URLS for images are fetched from a web api I made with flask (python web framework)
- The api is hosted on repl (a free online code editor)
- The repl url is https://replit.com/@PelvisResley/nilecat
- The api host however is https://nilecat--pelvisresley.repl.co/
- Changing the api url is supported. 
- Any host whose endpoint "/api/cat-img-v1" returns a string with the url of an image will work
## Features
- On startup, it will try to fetch an image of a cat
- The cat button will fetch a new cat instantly
- The app periodically fetches new images of cats
- The window will try to pop up when a new image is fetched
- The pop up interval is changeable
- The pop up feature can be disabled or enabled
- The server url from where the image urls can be changed
## Workaround caveat
To keep the web api simple and consistently working, it returns strings which are WPF app resource URIs, meaning they refer to images embedded in the app. The api used to return http urls but the urls are since unavailable, so I did this as a workaround.