using System;
using Xunit;
using AbbreviationGlossary;
using System.Data;

namespace AbbreviationGlossary.Tests.ReadmeSamples
{
    public class ReadmeExamplesTests
    {
        [Fact]
        public void ReadMeExample1()
        {
            Glossary g = new Glossary();
            g.TermConverterConfiguration.DelimeterForNotFound_Left = "<";
            g.TermConverterConfiguration.DelimeterForNotFound_Right = ">";

            g.AddGlossaryItem(new GlossaryItem("Person", "PRSN", true));
            g.AddGlossaryItem(new GlossaryItem("First Name", "FNM", true));

            TermConversionResult exapndResult1 = g.ExpandAbbreviation("PRSN_FNM");
            Console.WriteLine(exapndResult1.Output);
            //Person First Name

            TermConversionResult exapndResult2 = g.ExpandAbbreviation("PRSN_PRNT_FNM");
            Console.WriteLine(exapndResult2.Output);
            //Person <PRNT> First Name
        }


        [Fact]
        public void ReadMeExample2()
        {
            Glossary g = new Glossary();
            g.AddGlossaryItem(new GlossaryItem("Person", "PRSN", true));
            g.AddGlossaryItem(new GlossaryItem("First Name", "FNM", true));

            string term = "Person First name";
            string abbreviation = "PRSN_FNM";

            GlossaryComplianceResult result = g.CheckCompliance(term, abbreviation);

            Console.WriteLine(result.PreferredTermIsUsed);
            //true

            Console.WriteLine(result.TermAbbreviatesCorrectly);
            //true

            Console.WriteLine(result.AbbreviationExpandsToTermCorrectly);
            //true
        }

    }
}