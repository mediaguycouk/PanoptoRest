# PanoptoRest

## First Token
Open in Visual Studio and look at

`FirstToken/PanoptoRest.Tests/AuthTokenShould.cs`
and
`FirstToken/PanoptoRest.Auth/Token.cs`

The Token.cs file is based from Paul Redmond https://community.panopto.com/discussion/1208/rest-api-authentication-issue with some additional comments for people creating a token for the first time.

Run the XUnit test in debug mode if you wish to step through the code. You will have need to

* Create an API key
* Create a new user

You will also need to have a Panopto server to run against, and have it's URL

This program simply uses the API key, username and password to request a token. The token is returned as TokenResult.cs. This takes the returned access key, type and expires information then create a DateTime of when it was created and when it expires. If it errors it will return an Error and ErrorDescription.
