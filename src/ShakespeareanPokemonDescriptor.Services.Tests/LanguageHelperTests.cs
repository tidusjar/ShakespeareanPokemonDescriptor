using NUnit.Framework;
using System.Collections.Generic;

namespace ShakespeareanPokemonDescriptor.Services.Tests
{
    [TestFixture]
    public class LanguageHelperTests
    {
        [TestCaseSource(nameof(LanguageInput))]
        public string TwoCharacterLanguageCodeTests(string input)
        {
            return LanguageHelper.TwoCharacterLanguageCode(input);
        }

        private static IEnumerable<TestCaseData> LanguageInput
        {
            get
            {
                yield return new TestCaseData("en-US").Returns("en").SetName("TwoCharacterLanguageCode_Simple_Code");
                yield return new TestCaseData(null).Returns("en").SetName("TwoCharacterLanguageCode_NullLanguage");
                yield return new TestCaseData("").Returns("en").SetName("TwoCharacterLanguageCode_EmptyLanguage");
                yield return new TestCaseData("jp").Returns("jp").SetName("TwoCharacterLanguageCode_No_Seperator");
            }
        }
    }
}
