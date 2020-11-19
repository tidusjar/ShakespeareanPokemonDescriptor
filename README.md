# Shakespearean Pokemon Descriptor

### Pre-requsits
[.Net 5.0.100 SDK](https://dotnet.microsoft.com/download/dotnet/5.0)


### Starting up the project
You can use the `dotnet` tooling or Visual Studio.

To run the project using the `dotnet` tooling, clone the project and then navigate to the `~/src/ShakespeareanPokemonDescriptor.Api/` folder and run the following in your preferred terminal:
`dotnet run`

You can use a similar command to run the unit tests, navigate to the `~/src/` folder and run the following in your preferred terminal:
`dotnet test`


### Using the API

You can access the API documentation via `https://localhost:5001/swagger/`.

All endpoints are versioned, currently there is only one version (1.0)
Authentication API Key is present in the `appsettings.json`, this in future could be moved to a database so we are able to generate many API keys and potentially revoke them going forward.


