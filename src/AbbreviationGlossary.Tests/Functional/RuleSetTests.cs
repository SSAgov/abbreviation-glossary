using System;
using Xunit;
using AbbreviationGlossary;
using System.Data;
using System.Collections.Generic;
using System.Linq;

namespace AbbreviationGlossary.Tests.Functional
{
    public class RuleSetTests
    {
        [Fact]
        public void FullTest()
        {
            Glossary g = TestData.GetTestGlossary();

            string term = "Hearing Office Code";
            string abbreviation = "HOCD";

            GlossaryComplianceResult result = g.CheckCompliance(term, abbreviation);

            Assert.Equal(result.Term, term);
            Assert.Equal(result.Abbreviation, abbreviation);
            Assert.Equal(term, result.AbbreviationToTerm);
            Assert.Equal(abbreviation, result.TermToAbbreviation);
            Assert.Equal(result.TermToAbbreviation, result.AbbreviationToTermToAbbreviation);
            Assert.Equal(result.AbbreviationToTerm, result.TermToAbbreviationToTerm);
            Assert.True(result.PreferredTermIsUsed);
            Assert.False(result.CompoundAvailable);
            Assert.False(result.Mismatch);
            Assert.False(result.HyphenNeededForTerm);
            Assert.True(result.TermAbbreviatesCorrectly);
            Assert.True(result.AbbreviationExpandsToTermCorrectly);
            Assert.True(result.EvaluationTimeSpan.TotalMilliseconds > 0);
         

        }

        [Fact]
        public void FullTest2()
        {
            Glossary g = new Glossary();
            g.AddGlossaryItem(new GlossaryItem("Person", "PRSN", true));
            g.AddGlossaryItem(new GlossaryItem("First Name", "FNM", true));

            string logicalName = "Person First Name";
            string physicalName = "PRSN_FNM";

            GlossaryComplianceResult result = g.CheckCompliance(logicalName, physicalName);

            Console.WriteLine( g.ExpandAbbreviation("PRSN_FNM").Output);
            Assert.Equal(result.Term, logicalName);
            Assert.Equal(result.Abbreviation, physicalName);
            Assert.Equal(logicalName, result.AbbreviationToTerm);
            Assert.Equal(physicalName, result.TermToAbbreviation);
            Assert.Equal(result.TermToAbbreviation, result.AbbreviationToTermToAbbreviation);
            Assert.Equal(result.AbbreviationToTerm, result.TermToAbbreviationToTerm);
            Assert.True(result.PreferredTermIsUsed);
            Assert.False(result.CompoundAvailable);
            Assert.False(result.Mismatch);
            Assert.False(result.HyphenNeededForTerm);
            Assert.True(result.TermAbbreviatesCorrectly);
            Assert.True(result.AbbreviationExpandsToTermCorrectly);
            Assert.True(result.EvaluationTimeSpan.TotalMilliseconds > 0);
        }
    }
}
