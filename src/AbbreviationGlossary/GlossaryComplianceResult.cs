using System;
using System.Diagnostics;

namespace AbbreviationGlossary
{
    public class GlossaryComplianceResult
    {
        internal GlossaryComplianceResult()
        {
            Stopwatch = new Stopwatch();
        }

        public string AbbreviationToTerm { get; internal set; }
        public string TermToAbbreviation { get; internal set; }
        public string TermToAbbreviationToTerm { get; internal set; }
        public string AbbreviationToTermToAbbreviation { get; internal set; }

        internal Stopwatch Stopwatch { get; set; }
        public TermConversionResult AbbreviationToTermResult { get; internal set; }
        public TermConversionResult TermToAbbreviationResult { get; internal set; }
     
        public bool SynonymIsUsed { get; internal set; }
        public bool PreferredTermIsUsed { get; internal set; }
        public bool TermAbbreviatesCorrectly { get; internal set; }

        /// <summary>
        /// Specfies whether or not a compound abbreviation is available for the term used. 
        /// </summary>
        public bool CompoundAvailable { get; internal set; }

        public bool HyphenNeededForTerm { get; internal set; }
        public bool AbbreviationExpandsToTermCorrectly { get; internal set; }
        public string Abbreviation { get; internal set; }
        public string Term { get; internal set; }
        public bool Mismatch { get; internal set; }
        public TimeSpan EvaluationTimeSpan { get { return Stopwatch.Elapsed; } }


    }
}