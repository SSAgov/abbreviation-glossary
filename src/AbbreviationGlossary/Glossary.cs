using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Http;
using CsvHelper;
using PrimitiveExtensions;


namespace AbbreviationGlossary
{
    public class Glossary
    {
        private readonly List<GlossaryItem> _glossaryItems;

        public Glossary()
        {
            TermConverterConfiguration = new TermConverterConfig();
            _glossaryItems = new List<GlossaryItem>();
            _TermToAbbreviationDictionary = new Dictionary<string, string>();
            _AbbreviationToTermDictionary = new Dictionary<string, string>();
        }


        public ReadOnlyCollection<GlossaryItem> GlossaryItems
        {
            get
            {
                return new ReadOnlyCollection<GlossaryItem>(_glossaryItems);
            }
        }

        public void AddGlossaryItem(GlossaryItem glossaryItem)
        {

            _glossaryItems.Add(glossaryItem);

            if (glossaryItem.IsTermPreferred)
            {
                try
                {
                    _AbbreviationToTermDictionary.Add(glossaryItem.Abbreviation.ToUpper(), glossaryItem.Term);
                }
                catch (Exception ex)
                {

                    if (ex.GetType() == typeof(ArgumentException) && ex.Message.Contains("same key"))
                    {
                        throw new DuplicateNameException($"An preferred glossary item for '{glossaryItem.Abbreviation}' is already in the glossary. There can be only one primary term for an abbreviation. Consider marking IsTermPreferred to false.");
                    }
                    else
                    {
                        throw ex;
                    }
                }
            }
            if (glossaryItem.Abbreviation.IsNotNullOrEmptyString())
            {
                try
                {
                    _TermToAbbreviationDictionary.Add(glossaryItem.Term.ToUpper(), glossaryItem.Abbreviation);
                }
                catch (Exception ex)
                {
                    if (ex.GetType() == typeof(ArgumentException) && ex.Message.Contains("same key"))
                    {
                        throw new DuplicateNameException($"The term '{glossaryItem.Term}' already exists in the glossary. ");
                    }
                    else
                    {
                        throw ex;
                    }
                }
            }


        }

        public void SaveToCsvFile(string pathtoFile)
        {
            using (var writer = new StreamWriter(pathtoFile))
            using (var csv = new CsvWriter(writer))
            {
                csv.WriteRecords(_glossaryItems);

            }

        }


        private Dictionary<string, string> _TermToAbbreviationDictionary;

        public void RemoveGlossaryItem(GlossaryItem gi)
        {
            _glossaryItems.Remove(gi);
        }

        private Dictionary<string, string> _AbbreviationToTermDictionary;
        public TermConverterConfig TermConverterConfiguration { get; set; }

        public GlossaryComplianceResult CheckCompliance(string term, string abbreviatedTerm)
        {
            GlossaryComplianceResult r = new GlossaryComplianceResult();
            r.Stopwatch.Start();

            r.Term = term;
            r.Abbreviation = abbreviatedTerm;
            TermConversionResult abbreviationToTermResult = ExpandAbbreviation(r.Abbreviation);
            TermConversionResult termToAbbreviationResult = AbbreviateTerm(r.Term);

            TermConversionResult abbreviationToTermToAbbreviationResult = AbbreviateTerm(abbreviationToTermResult.Output);
            TermConversionResult termToAbbreviationToTermResult = ExpandAbbreviation(termToAbbreviationResult.Output);

            r.AbbreviationToTermResult = abbreviationToTermResult;
            r.TermToAbbreviationResult = termToAbbreviationResult;

            r.AbbreviationToTerm = abbreviationToTermResult.Output;
            r.TermToAbbreviation = termToAbbreviationResult.Output;
            r.TermToAbbreviationToTerm = termToAbbreviationToTermResult.Output;
            r.AbbreviationToTermToAbbreviation = abbreviationToTermToAbbreviationResult.Output;

            r.AbbreviationExpandsToTermCorrectly = r.AbbreviationToTerm == r.Term;
            r.TermAbbreviatesCorrectly = r.TermToAbbreviation == r.Abbreviation;

            r.PreferredTermIsUsed = r.AbbreviationExpandsToTermCorrectly
                                    && r.TermAbbreviatesCorrectly
                                    && r.AbbreviationToTerm == r.TermToAbbreviationToTerm
                                    && r.TermToAbbreviation == r.AbbreviationToTermToAbbreviation;

            r.SynonymIsUsed = !r.AbbreviationExpandsToTermCorrectly
                                   && r.TermAbbreviatesCorrectly
                                   && r.AbbreviationToTerm == r.TermToAbbreviationToTerm
                                   && r.TermToAbbreviation == r.AbbreviationToTermToAbbreviation;


            //if ((strPhysicalName == physicalToLogicalToPhysical && strPhysicalName != logicalToPhysical)
            //     || (strPhysicalName == RemoveNotFoundDelimeters(physicalToLogicalToPhysical) &&
            //         strPhysicalName != RemoveNotFoundDelimeters(logicalToPhysical)))
            //{
            //    if (logicalToPhysicalResult.NodesNotFound.Count == 0)
            //    {
            //        r.LogicalNameConvertsToPhysical = true;
            //    }
            //    if (physicalToLogicalResult.NodesNotFound.Count == 0)
            //    {
            //        r.PhysicalNameConvertsToLogical = true;
            //    }
            //}

            if (r.AbbreviationToTerm == r.TermToAbbreviationToTerm
                && (r.Abbreviation == r.TermToAbbreviation || r.TermToAbbreviation == r.AbbreviationToTermToAbbreviation)
                && r.Abbreviation != r.AbbreviationToTermToAbbreviation
                && abbreviationToTermResult.NodesNotFound.Count == 0
                && abbreviationToTermToAbbreviationResult.NodesNotFound.Count == 0)
            {
                r.CompoundAvailable = true;
            }

            if (r.Term == r.AbbreviationToTerm
   && r.AbbreviationToTerm == r.TermToAbbreviationToTerm
   && r.Abbreviation != r.TermToAbbreviation
   && r.TermToAbbreviation == r.AbbreviationToTermToAbbreviation)
            {
                r.HyphenNeededForTerm = true;

            }
            

            r.Mismatch = r.TermToAbbreviation != r.Abbreviation || r.AbbreviationToTerm != r.Term;

         
            r.Stopwatch.Stop();
            return r;
        }

        private string RemoveNotFoundDelimeters(string term)
        {
            return term.Replace(TermConverterConfiguration.DelimeterForNotFound_Left, "").Replace(TermConverterConfiguration.DelimeterForNotFound_Right, "");
        }

        public TermConversionResult ExpandAbbreviation(string abbreviation)
        {
            TermConversionResult r = new TermConversionResult();
            r.Input = abbreviation;
            r.Configuration = TermConverterConfiguration;

            if (abbreviation.IsNullOrEmptyString())
            {
                r.Output = "";
                return r;
            }
            abbreviation = abbreviation.Replace("<", "");
            abbreviation = abbreviation.Replace(">", "");

            string term = "";
            string[] nodes = abbreviation.Split(new string[] { TermConverterConfiguration.AbbreviationDelimeter }, StringSplitOptions.None);
            int i = 0;
            foreach (string node in nodes)
            {
                i++;
                if (_AbbreviationToTermDictionary.ContainsKey(node.ToUpper()))
                {
                    r.NodesFound.Add(node);
                    if (i > 1)
                    {
                        term = term + TermConverterConfiguration.TermDelimeter;
                    }
                    switch (TermConverterConfiguration.TermCaseConvention)
                    {
                        case CaseConvention.PascalCase:
                            term = term + _AbbreviationToTermDictionary[node.ToUpper()].ToTitleCase();
                            break;
                        case CaseConvention.camelCase:
                            if (i == 1)
                            {
                                term = term + _AbbreviationToTermDictionary[node.ToUpper()].ToLower();
                            }
                            else
                            {
                                term = term + _AbbreviationToTermDictionary[node.ToUpper()].ToTitleCase();
                            }
                            break;

                        case CaseConvention.lowercase:
                            term = term + _AbbreviationToTermDictionary[node.ToUpper()].ToLower();
                            break;

                        case CaseConvention.UPPERCASE:
                            term = term + _AbbreviationToTermDictionary[node.ToUpper()].ToUpper();
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    if (node.IsANumber())
                    {
                        term = term + TermConverterConfiguration.TermDelimeter + node;
                    }
                    else
                    {
                        r.NodesNotFound.Add(node);
                        term = term + TermConverterConfiguration.TermDelimeter + TermConverterConfiguration.DelimeterForNotFound_Left + node + TermConverterConfiguration.DelimeterForNotFound_Right;
                    }
                }
            }
            term = term.Trim().Replace(" ", TermConverterConfiguration.TermDelimeter);

            r.Output = term.Replace(TermConverterConfiguration.DelimeterForNotFound_Left + TermConverterConfiguration.DelimeterForNotFound_Right, "");
            return r;
        }


        public TermConversionResult AbbreviateTerm(string term)
        {
            TermConversionResult r = new TermConversionResult();
            r.Input = term;
            r.Configuration = TermConverterConfiguration;

            if (TermConverterConfiguration.DelimeterForNotFound_Left.Length > 0)
                term = term.Replace(TermConverterConfiguration.DelimeterForNotFound_Left, "");

            if (TermConverterConfiguration.DelimeterForNotFound_Right.Length > 0)
                term = term.Replace(TermConverterConfiguration.DelimeterForNotFound_Right, "");

            string termRemaining = term;
            string abbreviatedTerm = "";
            string searchString = term;
            string result = "";
            string subStringOfsearchString = "";

            do
            {
                if (_TermToAbbreviationDictionary.ContainsKey(searchString.ToUpper()))
                {
                    r.NodesFound.Add(searchString);
                    result = _TermToAbbreviationDictionary[searchString.ToUpper()];
                    switch (TermConverterConfiguration.AbbreviationCaseConvention)
                    {
                        case CaseConvention.PascalCase:
                            result = result.ToTitleCase();
                            break;
                        case CaseConvention.camelCase:
                            result = result.ToTitleCase();
                            break;
                        case CaseConvention.lowercase:
                            result = result.ToLower();
                            break;
                        case CaseConvention.UPPERCASE:
                            result = result.ToUpper();
                            break;
                        default:
                            break;
                    }

                    if (abbreviatedTerm == "")
                    {
                        if (TermConverterConfiguration.AbbreviationCaseConvention == CaseConvention.camelCase)
                        {
                            result = result.ToLower();
                        }
                        abbreviatedTerm = result;
                    }
                    else
                    {
                        abbreviatedTerm = abbreviatedTerm + TermConverterConfiguration.AbbreviationDelimeter + result;
                    }

                    termRemaining = termRemaining.ReplaceFirstOccurence(searchString, "").TrimStart();
                    if (termRemaining.StartsWith(TermConverterConfiguration.TermDelimeter))
                    {
                        termRemaining = termRemaining.ReplaceFirstOccurence(TermConverterConfiguration.TermDelimeter, "");
                    }

                    if (termRemaining.Length != 0)
                    {
                        if (termRemaining[0] == '-')
                        {
                            termRemaining = termRemaining.ReplaceFirstOccurence("-", "");
                        }
                    }
                    searchString = termRemaining;
                }
                else
                {
                    int delimeterIndex = searchString.LastIndexOf(TermConverterConfiguration.TermDelimeter);
                    if (!TermConverterConfiguration.TreatHyphenAsTermDelimeter)
                    {
                        subStringOfsearchString = searchString.Substring(0, delimeterIndex);
                    }
                    else
                    {
                        int hyphenIndex = searchString.LastIndexOf('-');

                        if (delimeterIndex == hyphenIndex)
                        {
                            subStringOfsearchString = "";
                        }
                        else if (delimeterIndex > hyphenIndex)
                        {
                            subStringOfsearchString = searchString.Substring(0, delimeterIndex);
                        }
                        else if (hyphenIndex > delimeterIndex)
                        {
                            if (hyphenIndex.Equals(0))
                            {
                                subStringOfsearchString = "";
                            }
                            else
                            {

                                subStringOfsearchString = searchString.Substring(0, hyphenIndex);

                            }
                        }
                    }


                    if (subStringOfsearchString == "")
                    {
                        if (searchString.IsANumber())
                        {
                            abbreviatedTerm = abbreviatedTerm
                                + TermConverterConfiguration.AbbreviationDelimeter
                                + searchString;
                        }
                        else
                        {
                            r.NodesNotFound.Add(searchString);
                            switch (TermConverterConfiguration.AbbreviationCaseConvention)
                            {
                                case CaseConvention.PascalCase:

                                    abbreviatedTerm = abbreviatedTerm + TermConverterConfiguration.AbbreviationDelimeter + TermConverterConfiguration.DelimeterForNotFound_Left + searchString.ToTitleCase();
                                    break;

                                case CaseConvention.camelCase:
                                    if (abbreviatedTerm == "")
                                    {
                                        abbreviatedTerm = abbreviatedTerm + TermConverterConfiguration.AbbreviationDelimeter + TermConverterConfiguration.DelimeterForNotFound_Left + searchString.ToLower();
                                    }
                                    else
                                    {
                                        abbreviatedTerm = abbreviatedTerm + TermConverterConfiguration.AbbreviationDelimeter + TermConverterConfiguration.DelimeterForNotFound_Left + searchString.ToTitleCase();
                                    }
                                    break;

                                case CaseConvention.lowercase:
                                    abbreviatedTerm = abbreviatedTerm + TermConverterConfiguration.AbbreviationDelimeter + TermConverterConfiguration.DelimeterForNotFound_Left + searchString.ToLower();
                                    break;

                                case CaseConvention.UPPERCASE:
                                    abbreviatedTerm = abbreviatedTerm + TermConverterConfiguration.AbbreviationDelimeter + TermConverterConfiguration.DelimeterForNotFound_Left + searchString.ToUpper();
                                    break;

                                default:
                                    break;
                            }


                            abbreviatedTerm = abbreviatedTerm + TermConverterConfiguration.DelimeterForNotFound_Right;
                        }

                        termRemaining = termRemaining.ReplaceFirstOccurence(searchString, "").TrimStart();
                        if (termRemaining.Length != 0)
                        {
                            if (termRemaining[0] == '-')
                            {
                                termRemaining = termRemaining.ReplaceFirstOccurence("-", "");
                            }
                        }
                        searchString = termRemaining;
                    }
                    else
                    {
                        searchString = subStringOfsearchString;
                    }
                }
            } while (termRemaining.Length > 0);

            r.Output = abbreviatedTerm;
            return r;
        }


        /// <summary>
        /// The CSV should have a header row with the following headings: Term,Abbreviation,IsTermPreferred
        /// </summary>
        /// <param name="pathToCsv"></param>
        public void LoadCsvFileFromLocal(string pathToCsv)
        {
            StreamReader rdr = new StreamReader(pathToCsv);
            LoadCsvFileFromStreamReader(rdr);
        }

  
        private void LoadCsvFileFromStreamReader(StreamReader rdr)
        {
            CsvReader csv = new CsvReader(rdr);
            var records = csv.GetRecords<GlossaryItem>();
            foreach (var item in records)
            {
                AddGlossaryItem(item);
            }
        }

        /// <summary>
        /// The CSV should have a header row with the following headings: Term,Abbreviation,IsTermPreferred
        /// </summary>
        public void LoadCsvFileFromUrl(string url)
        {
            using (var client = new HttpClient())
            {
                using (var stream = client.GetStreamAsync(url).Result)
                using (var reader = new StreamReader(stream))
                    LoadCsvFileFromStreamReader(reader);
            }
        }
    }
}
