using PrimitiveExtensions;

namespace AbbreviationGlossary
{
    public class GlossaryItem
    {
        private string _term;
        private string _abbreviation;
        private bool _isTermPreferred;
        private bool _isPreferredTermValueSet;

        public GlossaryItem()
        { }


        public GlossaryItem(string term, string abbreviation, bool isPrimary)
        {
            Term = term;
            Abbreviation = abbreviation;

            IsTermPreferred = isPrimary;
        }

        public override string ToString()
        {
            return Term;
        }


        /// <summary>
        /// Represents a spelled out word or phrase
        /// </summary>
        public string Term
        {
            get => _term;
            set
            {
                if (_term.IsNullOrEmptyString())
                {
                    _term = value;
                }
                else
                {
                    throw new System.Exception("Cannot change a term or an abbreviation from the glossary. Consider removing this entry and adding a new one");
                }

            }
        }

        /// <summary>
        /// Represents a shortened form of a word or phrase 
        /// </summary>
        public string Abbreviation
        {
            get => _abbreviation;
            set
            {
                if (_abbreviation.IsNullOrEmptyString())
                {
                    _abbreviation = value;
                }
                else
                {
                    throw new System.Exception("Cannot change a term or an abbreviation from the glossary. Consider removing this entry and adding a new one");
                }
            }
        }
        /// <summary>
        /// When true, signifies that a glossary item will be used for both ExpandAbbreviation() and Abbreviate() methods. If false the glossary item will be used only for Abbreviate() methods.
        /// </summary>
        public bool IsTermPreferred { get => _isTermPreferred;
            set
            {
                if (!_isPreferredTermValueSet)
                {
                    _isPreferredTermValueSet = true;
                    _isTermPreferred = value;
                }
                else
                {
                    throw new System.Exception("Cannot change a status once set. Consider removing this entry and adding a new one");
                }
                
            }
        }
    }
}