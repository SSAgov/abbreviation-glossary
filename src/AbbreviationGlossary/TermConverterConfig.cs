using System;
using System.Collections.Generic;
using System.Text;

namespace AbbreviationGlossary
{
    public class TermConverterConfig
    {
        private string termDelimeter;

        /// <summary>
        /// The CaseConvention to be used when converting to a logical name. The default is Pascal Case.
        /// </summary>
        public CaseConvention TermCaseConvention { get; set; }

        /// <summary>
        /// The CaseConvention to be used when converting to a physical name. The default is UPPPERCASE. 
        /// </summary>
        public CaseConvention AbbreviationCaseConvention { get; set; }

        /// <summary>
        /// The delimter character (or characters) to seperate parts of a name. The default is a space.
        /// </summary>
        public string TermDelimeter { 
            get => termDelimeter;
            set
            {
                if (value == "-")
                {
                    TreatHyphenAsTermDelimeter = true;
                }
                termDelimeter = value; 
            } }

        /// <summary>
        /// The delimter character (or characters) to seperate parts of a name. The default is an underscore.
        /// </summary>
        public string AbbreviationDelimeter { get; set; }

        /// <summary>
        /// Specifies whether or not a hyphen should also be treated as a seconary delimeter for logical names.  
        /// </summary>
        public bool TreatHyphenAsTermDelimeter { get; set; }

        /// <summary>
        /// The delimiting characater (or string) for when a conversion can't find a node in the glossary when doing a conversion. The default is a left angle bracket. 
        /// </summary>
        public string DelimeterForNotFound_Left { get; set; }

        /// <summary>
        /// The delimiting characater (or string) for when a conversion can't find a node in the glossary when doing a conversion. The default is a right angle bracket. 
        /// </summary>
        public string DelimeterForNotFound_Right { get; set; }

        public TermConverterConfig()
        {
            TermCaseConvention = CaseConvention.PascalCase;
            AbbreviationCaseConvention = CaseConvention.UPPERCASE;
            TermDelimeter = " ";
            AbbreviationDelimeter = "_";
            TreatHyphenAsTermDelimeter = true;
            DelimeterForNotFound_Left = "<";
            DelimeterForNotFound_Right = ">";
        }

    }

}
