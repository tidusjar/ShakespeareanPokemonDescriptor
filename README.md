# Shakespearean Pokemon Descriptor

A API that will take a Pokemon name (Or PokeApi ID) and apply a shakespearean translation to the pokemons description.
Also supports multiple languages, if there is no description for your preferred language it will fall back to English. If you do supply a language other than english there will be no shakespearean translation for this currently.

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
Authentication API Key is present in the `appsettings.json`.

On swagger ensure you press the [ Authorize ] button and enter the valid API key in.



### Improvements going forward

* Change the cache implimentation from MemoryCache to a distributed cache e.g. Redis. This would allow us to scale the application out
* Abstract the common API code e.g. error handling into some abstract base class
* Change the API key from config and move it to some sort of data store, or switch to using some provider like oidc/OAuth2
* Create integration tests and e2e API tests
* Change any response strings that come from the API to use Resources that can be translated going forward
* If the project scope increases it might be worth looking at implimenting something like the Mediator pattern to hanlde the incoming requests
* Investigate into containerization for this API or possibly use some serverless compute e.g. Azure Functions/AWS Lambda
