using System;
using Xunit;
using AbbreviationGlossary;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Reflection;

namespace AbbreviationGlossary.Tests.Unit
{
    public class GlossaryTests
    {
        [Fact]
        public void LoadGlossaryFromCsvUrl()
        {
            Glossary glossary = new Glossary();
            string url = "https://github.com/SSAgov/abbreviation-glossary/raw/master/src/AbbreviationGlossary.Tests/glossary.csv";
            glossary.LoadCsvFileFromUrl(url);
            Assert.True(glossary.GlossaryItems.Count > 10);
        }


        [Fact]
        public void LoadGlossaryFromCsvFile()
        {
            string baseDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string filePath = Path.Combine(baseDir, "glossary.csv");

            Glossary glossary = new Glossary();
            glossary.LoadCsvFileFromLocal(filePath);
            Assert.True(glossary.GlossaryItems.Count > 10);
        }

        [Fact]
        public void SaveGlossaryToCsvFile()
        {
            string baseDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string filePath = Path.Combine(baseDir, "glossary-output.csv");
            Glossary glossary = TestData.GetTestGlossary();
            glossary.SaveToCsvFile(filePath);
        }

        [Fact]
        public void AddDuplicateTermsToEmptyGlossary_ExpectExceptions()
        {
            Glossary glossary = new Glossary();
            glossary.AddGlossaryItem(new GlossaryItem("Hearing Office", "HOFC", true));
            Assert.Throws<DuplicateNameException>(() => glossary.AddGlossaryItem(new GlossaryItem("Hearing Office", "HO", true)));
        }

        [Fact]
        public void AddDuplicate1AbbreviationsToEmptyGlossary_ExpectExceptions()
        {
            Glossary glossary = new Glossary();
            glossary.AddGlossaryItem(new GlossaryItem("Happy Otters", "HO", true));
            Assert.Throws<DuplicateNameException>(() => glossary.AddGlossaryItem(new GlossaryItem("Hearing Office", "HO", true)));
        }

        [Fact]
        public void AddMultipleSynonymousAbbreviationsAndOne1ToEmptyGlossary_ExpectNoExceptions()
        {
            Glossary glossary = new Glossary();
            glossary.AddGlossaryItem(new GlossaryItem("Happy Octopus", "HO", false));
            glossary.AddGlossaryItem(new GlossaryItem("Happy Otters", "HO", false));
            glossary.AddGlossaryItem(new GlossaryItem("Hearing Office", "HO", true));
        }

        [Fact]
        public void RemoveGlossaryItem_ExpectNoExceptions()
        {
            Glossary glossary = new Glossary();
            GlossaryItem gi = new GlossaryItem("Happy Octopus", "HO", false);
            glossary.AddGlossaryItem(gi);
            glossary.AddGlossaryItem(new GlossaryItem("Happy Otters", "HO", false));
            glossary.AddGlossaryItem(new GlossaryItem("Hearing Office", "HO", true));
            Assert.Equal(3, glossary.GlossaryItems.Count);
            glossary.RemoveGlossaryItem(gi);
            Assert.Equal(2, glossary.GlossaryItems.Count);
        }

        [Fact]
        public void ChangeGlossaryitem_ExpectException()
        {
            GlossaryItem gi = new GlossaryItem();
            gi.Abbreviation = "test";
            gi.Term = "term";
            bool threwException = false;
            try
            {
                gi.Abbreviation = "changed";
            }
            catch (Exception)
            {
                threwException = true;
            }
            Assert.True(threwException);
        }



        [Fact]
        public void ChangeGlossaryitemStatus_ExpectException()
        {
            GlossaryItem gi = new GlossaryItem();
            gi.Abbreviation = "test";
            gi.Term = "term";
            gi.IsTermPreferred = false;

            bool threwException = false;
            try
            {
                gi.IsTermPreferred = true;
            }
            catch (Exception)
            {
                threwException = true;
            }
            Assert.True(threwException);
        }

        [Fact]
        public void ChangeGlossaryitemStatus_ExpectNoException()
        {
            GlossaryItem gi = new GlossaryItem();
            gi.Abbreviation = "test";
            gi.Term = "term";
            gi.IsTermPreferred = true;

            bool exceptionThrown = false;
            try
            {
                gi.IsTermPreferred = false;
            }
            catch (Exception)
            {
                exceptionThrown = true;
            }
            Assert.True(exceptionThrown);
        }

      
    }
}