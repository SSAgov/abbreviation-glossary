using System;
using Xunit;
using AbbreviationGlossary;
using System.Data;
using System.Collections.Generic;
using System.Linq;

namespace AbbreviationGlossary.Tests.Unit
{
    public class StandardsComplianceTests
    {
        [Fact]
        public void RunRuleAgainstObject_Column_NoClassWord()
        {
            Glossary nsg = TestData.GetTestGlossary();
            DataTable dt = new DataTable();
            string logicalName = "Hearing Office Donkey Monkey";
            string physicalName = "HOFC";
          
            GlossaryComplianceResult nsResult = nsg.CheckCompliance(logicalName, physicalName);

            Assert.Equal(2, nsResult.TermToAbbreviationResult.NodesNotFound.Count);
            Assert.Empty(nsResult.AbbreviationToTermResult.NodesNotFound);
            Assert.Single(nsResult.AbbreviationToTermResult.NodesFound);
            Assert.Single(nsResult.TermToAbbreviationResult.NodesFound);
            Assert.False(nsResult.CompoundAvailable);
            Assert.False(nsResult.HyphenNeededForTerm);
            Assert.False(nsResult.TermAbbreviatesCorrectly);
            Assert.False(nsResult.AbbreviationExpandsToTermCorrectly);

        }

        [Fact]
        public void RunRuleAgainstObject_Column_ClassWord()
        {
            Glossary nsg = TestData.GetTestGlossary();
            DataTable dt = new DataTable();
            string logicalName = "Hearing Office Code";
            string physicalName = "HOFC";

            GlossaryComplianceResult nsResult = nsg.CheckCompliance(logicalName, physicalName);
            
        }

        [Fact]
        public void RunRuleAgainstObject_Column_HyphenNeeded()
        {

            Glossary nsg = TestData.GetTestGlossary();
            DataTable dt = new DataTable();
            string logicalName = "Hearing Office";
            string physicalName = "HRG_OFC";
          
            GlossaryComplianceResult nsResult = nsg.CheckCompliance(logicalName, physicalName);

            Assert.Empty(nsResult.TermToAbbreviationResult.NodesNotFound);
            Assert.Empty(nsResult.AbbreviationToTermResult.NodesNotFound);
            Assert.Equal(2, nsResult.AbbreviationToTermResult.NodesFound.Count);
            Assert.Single(nsResult.TermToAbbreviationResult.NodesFound);
            Assert.True(nsResult.CompoundAvailable);
            Assert.True(nsResult.HyphenNeededForTerm);
        }
    }
}
