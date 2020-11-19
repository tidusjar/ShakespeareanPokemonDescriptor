using Moq;
using NUnit.Framework;
using ShakespeareanPokemonDescriptor.PokeApi;
using ShakespeareanPokemonDescriptor.PokeApi.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ShakespeareanPokemonDescriptor.Services.Tests
{
    public class PokemonDescriptorServiceTests
    {
        private PokemonDescriptorService Service { get; set; }
        private Mock<IPokemonApiClient> PokemonApiClientMock { get; set; }

        [SetUp]
        public void Setup()
        {
            PokemonApiClientMock = new Mock<IPokemonApiClient>();
            Service = new PokemonDescriptorService(PokemonApiClientMock.Object);
        }

        [Test]
        public async Task InvalidPokemonName_Api_Returns_Null()
        {
            var result = await Service.Describe("some silly name", "en", CancellationToken.None);
            Assert.That(result, Is.EqualTo(string.Empty));
        }

        [Test]
        public async Task InvalidPokemonSpeciesName_Api_Returns_Null()
        {
            PokemonApiClientMock.Setup(x => x.SearchPokemon(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(new PokemonSearchResult
            {
                Id = 1
            });

            var result = await Service.Describe("Charmander", "en", CancellationToken.None);
            Assert.That(result, Is.EqualTo(string.Empty));
        }

        [TestCase(null, TestName = "NullRequestLanguage_Should_DefaultTo_En")]
        [TestCase("", TestName = "EmptyRequestLanguage_Should_DefaultTo_En")]
        public async Task EmptyRequestLanguage_Should_DefaultTo_En(string input)
        {
            SetupBasicDataMocks();

            var result = await Service.Describe("some silly name", input, CancellationToken.None);
            Assert.That(result, Is.EqualTo("English"));
        }


        private void SetupBasicDataMocks()
        {
            PokemonApiClientMock.Setup(x => x.SearchPokemon(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(new PokemonSearchResult
            {
                Id = 1
            });

            PokemonApiClientMock.Setup(x => x.GetSpecies(It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync(new PokemonSpeciesSearchResult
            {
                Results = new List<PokemonSpeciesFlavourResult>
                {
                    new PokemonSpeciesFlavourResult
                    {
                        Language = new LanguageResult
                        {
                            Name = "jp"
                        },
                        Text = "NotEnglish"
                    },
                    new PokemonSpeciesFlavourResult
                    {
                        Language = new LanguageResult
                        {
                            Name = "en"
                        },
                        Text = "English"
                    },
                }
            });
        }
    }
}