# Athlos Gree WebView Helper
This package provides a helpful component to streamline integration with Athlos via a Gree WebView.

## Instructions
1. Attach the AthlosGreeWebView component to any game object within the same scene as the Gree WebView.
2. Set the `Web View` variable to the Gree WebView in your scene.
3. Follow the common instructions as documented [here](https://athlos.readme.io/docs).

## Options

### Initialisation Settings
The component provides access to the standard initialisation settings for the Gree WebView via the inspector. These include:

| Common | Android | iOS | Editor |
|--|--|--|--|
|Transparent|Force Dark Mode|Enable WKWebView|Separated|
|Zoom||Content Mode||
|UA||Allow Link Preview||

### WebView Callbacks
The component requires priority when receiving callbacks from the webview to initialise properly but provides access to the same callbacks via UnityEvents. Simply add listeners to the following events (all `Action<string>`):
* OnCallback
* OnError
* OnHTTPError
* OnStarted
* OnLoaded
* OnHooked

Refer to Gree's official documentation for when these callbacks are invoked.