using System;
using Xunit;
using AbbreviationGlossary;
using System.Data;

namespace AbbreviationGlossary.Tests.Unit
{
    public class Glossary_ExpandAbbreviatedTermTests
    {
        [Fact]
        public void Given_PRSN_Expect_Person()
        {
            Glossary nsg = TestData.GetTestGlossary();
            
            nsg.TermConverterConfiguration.TermDelimeter = "|";

            string term = "Person|First|Name";
            string abbreviation = "PRSN_FNM";
            string convertedName = nsg.ExpandAbbreviation(abbreviation).Output;
            Assert.Equal(term, convertedName);
        }
        
        [Fact]
        public void Given_NOTINGLOSSARY_Expect_NOTINGLOSSARY()
        {
            Glossary nsg = TestData.GetTestGlossary();
            
            TermConversionResult r = nsg.ExpandAbbreviation("NOTINGLOSSARY");
            string term = r.Output;
            Assert.Empty(r.NodesFound);
            Assert.Single(r.NodesNotFound);
            Assert.Equal("NOTINGLOSSARY", r.Input);
            Assert.Equal("<NOTINGLOSSARY>", term);
        }

        [Fact]
        public void Given_NOT_IN_GLOSSARY_Expect_NOT_IN_GLOSSARY()
        {
            Glossary nsg = TestData.GetTestGlossary();

            TermConversionResult r = nsg.ExpandAbbreviation("NOT_IN_GLOSSARY");
            string term = r.Output;
            Assert.Empty(r.NodesFound);
            Assert.Equal(3,r.NodesNotFound.Count);
            Assert.Equal("NOT_IN_GLOSSARY", r.Input);
            Assert.Equal("<NOT> <IN> <GLOSSARY>", term);
        }

        [Fact]
        public void Given_HOCD_Expect_HearingOfficeCode()
        {
            //Arrange
            Glossary nsg = TestData.GetTestGlossary();
            
            //Act
            string abbreviation = "HOCD";
            string term = "Hearing Office Code";
            string convertedterm = nsg.ExpandAbbreviation(abbreviation).Output;

            //Assert
            Assert.Equal(term, convertedterm);
        }

        [Fact]
        public void Given_HOFC_CD_Expect_HearingOfficeCode()
        {
            //Arrange
            Glossary nsg = TestData.GetTestGlossary();
            

            //Act
            string abbreviation = "HOFC_CD";
            string term = "Hearing Office Code";
            string convertedterm = nsg.ExpandAbbreviation(abbreviation).Output;


            //Assert
            Assert.Equal(term, convertedterm);
        }

        [Fact]
        public void case_lower_delimeter_space()
        {
            //Arrange
            Glossary nsg = TestData.GetTestGlossary();
            
            nsg.TermConverterConfiguration.TermCaseConvention = CaseConvention.lowercase;

            //Act
            string abbreviation = "HRG_OCD";
            string term = "hearing office code";
            string convertedterm = nsg.ExpandAbbreviation(abbreviation).Output;

            //Assert
            Assert.Equal(term, convertedterm);
        }
        [Fact]
        public void case_UPPER_delimeter_space()
        {
            //Arrange
            Glossary nsg = TestData.GetTestGlossary();
            
            nsg.TermConverterConfiguration.TermCaseConvention = CaseConvention.UPPERCASE;

            //Act
            string abbreviation = "HRG_OCD";
            string term = "HEARING OFFICE CODE";
            string convertedterm = nsg.ExpandAbbreviation(abbreviation).Output;

            //Assert
            Assert.Equal(term, convertedterm);
        }

        [Fact]
        public void case_Pascal_delimeter_space()
        {
            //Arrange
            Glossary nsg = TestData.GetTestGlossary();
            
            nsg.TermConverterConfiguration.TermCaseConvention = CaseConvention.PascalCase;

            //Act
            string abbreviation = "HRG_OCD";
            string term = "Hearing Office Code";
            string convertedterm = nsg.ExpandAbbreviation(abbreviation).Output;

            //Assert
            Assert.Equal(term, convertedterm);
        }


        [Fact]
        public void case_camel_delimeter_none()
        {
            //Arrange
            Glossary nsg = TestData.GetTestGlossary();
            
            nsg.TermConverterConfiguration.TermCaseConvention = CaseConvention.camelCase;
            nsg.TermConverterConfiguration.TermDelimeter = "";

            //Act
            string abbreviation = "HRG_OCD";
            string term = "hearingOfficeCode";
            string convertedterm = nsg.ExpandAbbreviation(abbreviation).Output;

            //Assert
            Assert.Equal(term, convertedterm);
        }

        [Fact]
        public void Given_HRG_OFC_CD_Expect_HearingOfficeCode()
        {
            //Arrange
            Glossary nsg = TestData.GetTestGlossary();
            
            //Act
            string abbreviation = "HRG_OFC_CD";
            string term = "Hearing Office Code";
            string convertedterm = nsg.ExpandAbbreviation(abbreviation).Output;

            //Assert
            Assert.Equal(term, convertedterm);
        }
    }
}