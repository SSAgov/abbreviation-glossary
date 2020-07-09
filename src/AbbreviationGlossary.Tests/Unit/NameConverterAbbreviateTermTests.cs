using System;
using Xunit;
using AbbreviationGlossary;
using System.Data;
using System.Collections.Generic;

namespace AbbreviationGlossary.Tests.Unit
{
    public class Glossary_AbbreviateTests
    {


        [Fact]
        public void NoAbbreviationDelimeter_camelCase_EmptyGlossary()
        {
            Glossary nsg = new Glossary();
            nsg.TermConverterConfiguration.AbbreviationDelimeter = "";
            nsg.TermConverterConfiguration.AbbreviationCaseConvention = CaseConvention.camelCase;
            nsg.TermConverterConfiguration.DelimeterForNotFound_Left = "";
            nsg.TermConverterConfiguration.DelimeterForNotFound_Right = "";

            string term = "Person First Name";
            string abbreviation = "personFirstName";
            string convertedAbbreviation = nsg.AbbreviateTerm(term).Output;
            Assert.Equal(abbreviation, convertedAbbreviation);
        }

        [Fact]
        public void NoAbbreviationDelimeter_camelCase_EmptyGlossary_ExclamationNotFound()
        {
            Glossary nsg = new Glossary();
            nsg.TermConverterConfiguration.AbbreviationDelimeter = "";
            nsg.TermConverterConfiguration.AbbreviationCaseConvention = CaseConvention.camelCase;
            nsg.TermConverterConfiguration.DelimeterForNotFound_Left = "!";
            nsg.TermConverterConfiguration.DelimeterForNotFound_Right = "!";

            string term = "Person First Name";
            string abbreviation = "!person!!First!!Name!";
            string convertedAbbreviation = nsg.AbbreviateTerm(term).Output;
            Assert.Equal(abbreviation, convertedAbbreviation);
        }

        [Fact]
        public void NoAbbreviationDelimeter_camelCase_Glossary_ExclamationNotFound()
        {
            Glossary nsg = TestData.GetTestGlossary();

            nsg.TermConverterConfiguration.AbbreviationDelimeter = "";
            nsg.TermConverterConfiguration.AbbreviationCaseConvention = CaseConvention.camelCase;
            nsg.TermConverterConfiguration.DelimeterForNotFound_Left = "!";
            nsg.TermConverterConfiguration.DelimeterForNotFound_Right = "!";

            string term = "Person First Cool Name";
            string abbreviation = "prsnFrst!Cool!Nm";
            string convertedAbbreviation = nsg.AbbreviateTerm(term).Output;
            Assert.Equal(abbreviation, convertedAbbreviation);
        }

        [Fact]
        public void NoAbbreviationDelimeter_camelCase_Glossary_TermNotInGlossary()
        {
            Glossary nsg = TestData.GetTestGlossary();

            nsg.TermConverterConfiguration.AbbreviationDelimeter = "";
            nsg.TermConverterConfiguration.AbbreviationCaseConvention = CaseConvention.camelCase;
            nsg.TermConverterConfiguration.DelimeterForNotFound_Left = "";
            nsg.TermConverterConfiguration.DelimeterForNotFound_Right = "";

            string term = "Person First Cool Name";
            string abbreviation = "prsnFrstCoolNm";
            string convertedAbbreviation = nsg.AbbreviateTerm(term).Output;


            Assert.Equal(abbreviation, convertedAbbreviation);
        }

        [Fact]
        public void NoAbbreviationDelimeter_camelCase_Glossary()
        {
            Glossary nsg = TestData.GetTestGlossary();

            nsg.TermConverterConfiguration.AbbreviationDelimeter = "";
            nsg.TermConverterConfiguration.AbbreviationCaseConvention = CaseConvention.camelCase;
            nsg.TermConverterConfiguration.DelimeterForNotFound_Left = "";
            nsg.TermConverterConfiguration.DelimeterForNotFound_Right = "";

            string term = "Person First Name";
            string abbreviation = "prsnFnm";
            string convertedAbbreviation = nsg.AbbreviateTerm(term).Output;
            Assert.Equal(abbreviation, convertedAbbreviation);
        }


        [Fact]
        public void Given_PRSN_Expect_Person()
        {
            Glossary nsg = TestData.GetTestGlossary();

            nsg.TermConverterConfiguration.TermDelimeter = "|";
            nsg.TermConverterConfiguration.AbbreviationCaseConvention = CaseConvention.lowercase;

            string term = "Person|First|Name";
            string abbreviation = "prsn_frst_nm";
            string convertedAbbreviation = nsg.AbbreviateTerm(term).Output;
            Assert.Equal(abbreviation, convertedAbbreviation);
        }
        [Fact]
        public void ld_pipe_pd_pipe()
        {
            Glossary nsg = TestData.GetTestGlossary();

            nsg.TermConverterConfiguration.TermDelimeter = "|";
            nsg.TermConverterConfiguration.AbbreviationDelimeter = "|";
            nsg.TermConverterConfiguration.AbbreviationCaseConvention = CaseConvention.lowercase;

            string term = "Person|First|Name";
            string abbreviation = "prsn|frst|nm";
            string convertedAbbreviation = nsg.AbbreviateTerm(term).Output;
            Assert.Equal(abbreviation, convertedAbbreviation);
        }


        [Fact]
        public void no_delimter_camelCase()
        {
            //Arrange
            Glossary nsg = TestData.GetTestGlossary();
            nsg.TermConverterConfiguration.AbbreviationDelimeter = "";
            nsg.TermConverterConfiguration.TermDelimeter = "|";
            nsg.TermConverterConfiguration.AbbreviationCaseConvention = CaseConvention.camelCase;

            //Act
            string term = "Hearing|Office|Code";
            string abbreviatedTerm = "hrgOfcCd";
            string convertedAbbreviatedTerm = nsg.AbbreviateTerm(term).Output;

            //Assert
            Assert.Equal(abbreviatedTerm, convertedAbbreviatedTerm);
        }

        [Fact]
        public void HyphenDelimeter_Output()
        {
            //Arrange
            Glossary nsg = TestData.GetTestGlossary();
            nsg.TermConverterConfiguration.AbbreviationDelimeter = "-";
            nsg.TermConverterConfiguration.TermDelimeter = "-";
            nsg.TermConverterConfiguration.AbbreviationCaseConvention = CaseConvention.lowercase;

            //Act
            string term = "Hearing-Office-Code";
            string abbreviatedTerm = "hrg-ofc-cd";
            string convertedAbbreviatedTerm = nsg.AbbreviateTerm(term).Output;

            //Assert
            Assert.Equal(abbreviatedTerm, convertedAbbreviatedTerm);
        }

        [Fact]
        public void HyphenDelimeter_BadInput_Output()
        {
            //Arrange
            Glossary nsg = TestData.GetTestGlossary();
            nsg.TermConverterConfiguration.AbbreviationDelimeter = "-";
            nsg.TermConverterConfiguration.TermDelimeter = "-";
            nsg.TermConverterConfiguration.AbbreviationCaseConvention = CaseConvention.lowercase;

            //Act
            string term = "nopenope-Office-Code";
            string abbreviatedTerm = "<nopenope>-ofc-cd";
            string convertedAbbreviatedTerm = nsg.AbbreviateTerm(term).Output;

            //Assert
            Assert.Equal(abbreviatedTerm, convertedAbbreviatedTerm);
        }





        [Fact]
        public void Given_Hearing_OfficeCode_Expect_HRG_OCD()
        {
            //Arrange
            Glossary nsg = TestData.GetTestGlossary();

            nsg.TermConverterConfiguration.TreatHyphenAsTermDelimeter = true;

            //Act
            string abbreviation = "HRG_OCD";
            string term = "Hearing-Office Code";
            string convertedabbreviation = nsg.AbbreviateTerm(term).Output;

            //Assert
            Assert.Equal(abbreviation, convertedabbreviation);
        }

        [Fact]
        public void Given_Hearing_Office_Code_Expect_HRG_OFC_CD()
        {
            //Arrange
            Glossary nsg = TestData.GetTestGlossary();
            nsg.TermConverterConfiguration.TreatHyphenAsTermDelimeter = true;

            //Act
            string abbreviatedTerm = "HRG_OFC_CD";
            string term = "Hearing-Office-Code";
            TermConversionResult result = nsg.AbbreviateTerm(term);
            string convertedAbbreviationResult = result.Output;

            //Assert
            Assert.Equal(abbreviatedTerm, convertedAbbreviationResult);
        }

        [Fact]
        public void Given_HearingOffice_Code_Expect_HOFC_CD()
        {
            //Arrange
            Glossary nsg = TestData.GetTestGlossary();

            nsg.TermConverterConfiguration.TreatHyphenAsTermDelimeter = true;

            //Act
            string abbreviation = "HOFC_CD";
            string term = "Hearing Office-Code";
            TermConversionResult result = nsg.AbbreviateTerm(term);
            string convertedabbreviation = result.Output;

            //Assert
            Assert.Equal(abbreviation, convertedabbreviation);
            Assert.Equal(2, result.NodesFound.Count);
        }



    }
}