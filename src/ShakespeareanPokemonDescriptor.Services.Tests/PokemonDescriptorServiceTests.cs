using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using NUnit.Framework;
using ShakespeareanPokemonDescriptor.FunTranslationsApi.Models;
using ShakespeareanPokemonDescriptor.PokeApi;
using ShakespeareanPokemonDescriptor.PokeApi.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ShakespeareanPokemonDescriptor.Services.Tests
{
    public class PokemonDescriptorServiceTests
    {
        private PokemonDescriptorService Service { get; set; }
        private Mock<IPokemonApiClient> PokemonApiClientMock { get; set; }
        private Mock<ICacheService> CacheServiceMock { get; set; }
        private Mock<ITranslatorApiClient> TranslatorApiClientMock { get; set; }

        [SetUp]
        public void Setup()
        {
            PokemonApiClientMock = new Mock<IPokemonApiClient>();
            TranslatorApiClientMock = new Mock<ITranslatorApiClient>();
            CacheServiceMock = new Mock<ICacheService>();
            Service = new PokemonDescriptorService(PokemonApiClientMock.Object, TranslatorApiClientMock.Object, new NullLogger<PokemonDescriptorService>(), CacheServiceMock.Object);
        }

        [Test]
        public async Task Valid_Test()
        {
            SetupBasicDataMocks("Charmander");
            TranslatorApiClientMock.Setup(x => x.Translate(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(new TranslationResult
            {
                Content = new TranslationContent
                {
                    Text = "Shakespearing Charmander Description"
                }
            });
            var result = await Service.Describe("Charmander", "en-GB", CancellationToken.None);
            Assert.That(result, Is.EqualTo("Shakespearing Charmander Description"));
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
            SetupBasicDataMocks("Mewto");

            var result = await Service.Describe("Mewto", input, CancellationToken.None);
            TranslatorApiClientMock.Verify(x => x.Translate("English", It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task Translate_API_Error()
        {
            SetupBasicDataMocks("Charmander");

            var result = await Service.Describe("Charmander", "en-GB", CancellationToken.None);
            Assert.That(result, Is.EqualTo(string.Empty));
            TranslatorApiClientMock.Verify(x => x.Translate(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
        }


        private void SetupBasicDataMocks(string pokemonName)
        {
            CacheServiceMock.Setup(x => x.GetOrAddAsync(CacheKeys.PokemonSearchKey + pokemonName, It.IsAny<Func<Task<PokemonSearchResult>>>(), It.IsAny<DateTime>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new PokemonSearchResult
                {
                    Id = 1
                });

            CacheServiceMock.Setup(x => x.GetOrAddAsync(CacheKeys.PokemonSpeciesKey + 1, It.IsAny<Func<Task<PokemonSpeciesSearchResult>>>(), It.IsAny<DateTime>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new PokemonSpeciesSearchResult
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