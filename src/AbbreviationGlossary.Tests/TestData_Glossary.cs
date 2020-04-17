using System;
using Xunit;
using AbbreviationGlossary;
using System.Data;
using System.Collections.Generic;

namespace AbbreviationGlossary.Tests
{
    public  static partial class TestData
    {
        public static Glossary GetTestGlossary()
        {
            Glossary g = new Glossary();
            g.AddGlossaryItem(new GlossaryItem("Person","PRSN",true));
            g.AddGlossaryItem(new GlossaryItem("First","FRST",true));
            g.AddGlossaryItem(new GlossaryItem("Name","NM",true));
            g.AddGlossaryItem(new GlossaryItem("First Name","FNM",true));
            g.AddGlossaryItem(new GlossaryItem("Hearing", "HRG",true));
            g.AddGlossaryItem(new GlossaryItem("Office","OFC",true));
            g.AddGlossaryItem(new GlossaryItem("Code", "CD", true));
            g.AddGlossaryItem(new GlossaryItem("Hearing Office","HOFC",true) );
            g.AddGlossaryItem(new GlossaryItem("Hearing Office Code","HOCD",true) );
            g.AddGlossaryItem(new GlossaryItem("Office Code","OCD",true) );
            return g;
        }
    }
}
