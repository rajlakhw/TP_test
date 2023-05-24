using Data;
using Data.Repositories;
using Services.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using ViewModels.Common;

namespace Services
{
    public class TPWordCountBreakdownBatch : ITPWordCountBreakdownBatch
    {
		private readonly IRepository<QuoteItem> quoteItemRepository;
		private readonly ITPQuotesLogic quoteService;
		private readonly ITPTimeZonesService timeZonesService;

		
        public TPWordCountBreakdownBatchModel WordCountBreakdownBatch(string LogFilePath, Org ThisOrg, JobOrder ThisJobOrder = null, Global_Settings.Enumerations.MemoryApplications MemoryApplication = Global_Settings.Enumerations.MemoryApplications.NoneOrUnknown, bool LinguisticSupplierWordCountsOnly = false, JobItem thisJobItem = null, bool ToGetWordCountsFromOneFileOnly = false)
        {
            if (LogFilePath == "")
                throw new Exception("The translation memory log file path was not supplied, so no word counts could be loaded.");
            else if (System.IO.File.Exists(LogFilePath) == false)
                throw new Exception("The translation memory log file cannot be located, so no word counts could be loaded. The log file path which could not be found was: " + LogFilePath);

            // reset word counts before adding to totals etc.
            TPWordCountBreakdownBatchModel model = new TPWordCountBreakdownBatchModel();
            model.pNewWords = 0;
            model.pFuzzyBand1Words = 0;
            model.pFuzzyBand2Words = 0;
            model.pFuzzyBand3Words = 0;
            model.pFuzzyBand4Words = 0;
            model.pLinguisticNewWords = 0;
            model.pLinguisticFuzzyBand1Words = 0;
            model.pLinguisticFuzzyBand2Words = 0;
            model.pLinguisticFuzzyBand3Words = 0;
            model.pLinguisticFuzzyBand4Words = 0;
            model.pRepetitionsWords = 0;
            model.pExactMatchWords = 0;
            model.pMatchPlusOrPerfectMatchWords = 0;
            model.pTotalCharacterCount = 0;
            int NewWordsForThisFile = 0;
            int FuzzyBand1WordsForThisFile = 0;
            int FuzzyBand2WordsForThisFile = 0;
            int FuzzyBand3WordsForThisFile = 0;
            int FuzzyBand4WordsForThisFile = 0;
            int LinguisticNewWordsForThisFile = 0;
            int LinguisticFuzzyBand1WordsForThisFile = 0;
            int LinguisticFuzzyBand2WordsForThisFile = 0;
            int LinguisticFuzzyBand3WordsForThisFile = 0;
            int LinguisticFuzzyBand4WordsForThisFile = 0;
            int RepetitionsWordsForThisFile = 0;
            int ExactMatchWordsForThisFile = 0;
            int MatchPlusOrPerfectMatchWordsForThisFile = 0;
            string FileNameForThisFile = "";
            int pNumberOfFilesAnalysed = 0;
            Global_Settings.Enumerations.MemoryApplications pMemoryFormat = new Global_Settings.Enumerations.MemoryApplications();

            FileInfo LogFileInfo = new FileInfo(LogFilePath);

            int Bands = 1;

            // if the calling function has already provided information on which memory application it comes from
            // (regardless of the actual extension of the file name) then follow that
            if (MemoryApplication != Global_Settings.Enumerations.MemoryApplications.NoneOrUnknown)
                pMemoryFormat = MemoryApplication;
            else
                // otherwise, use the file extension to determine what parsing method to use
                if ((LogFileInfo.Extension).ToLower() == ".csv")
                pMemoryFormat = Global_Settings.Enumerations.MemoryApplications.Trados2007;
            else if ((LogFileInfo.Extension).ToLower() == ".xml")
                pMemoryFormat = Global_Settings.Enumerations.MemoryApplications.Trados2014;
            else
                throw new Exception(string.Format("The file extension for the translation memory log file, {0}, is not currently supported for word-count automation.", LogFileInfo.Extension));

            // now parse the file based on the necessary rules
            if (pMemoryFormat == Global_Settings.Enumerations.MemoryApplications.Trados2007)
            {
                if (ThisOrg != null)
                {
                    {
                        var withBlock = ThisOrg;
                        if (withBlock.FuzzyBand2BottomPercentage != null/* TODO Change to default(_) if this is not a reference type */ & withBlock.FuzzyBand2BottomPercentage > 0)
                        {
                            if (withBlock.FuzzyBand4BottomPercentage != null/* TODO Change to default(_) if this is not a reference type */ & withBlock.FuzzyBand4BottomPercentage > 0)
                            {
                                // 4 bands
                                Bands = 4;
                                if (withBlock.FuzzyBand1BottomPercentage != 50 | withBlock.FuzzyBand1TopPercentage != 74 | withBlock.FuzzyBand2BottomPercentage != 75 | withBlock.FuzzyBand2TopPercentage != 84 | withBlock.FuzzyBand3BottomPercentage != 85 | withBlock.FuzzyBand3TopPercentage != 94 | withBlock.FuzzyBand4BottomPercentage != 95 | withBlock.FuzzyBand4TopPercentage != 99)
                                    throw new Exception("It is not possible to import this Trados 2007 log file, because the fuzzy match bands for this organisation do not correspond to the Trados 2007 fuzzy match bands. The fuzzy match bands cannot be adjusted in Trados 2007, though they can be adjusted if you use Trados 2009/2011.");
                            }
                            else if (withBlock.FuzzyBand3BottomPercentage != null/* TODO Change to default(_) if this is not a reference type */ & withBlock.FuzzyBand3BottomPercentage > 0)
                            {
                                // 3 bands
                                Bands = 3;
                                if (withBlock.FuzzyBand1BottomPercentage != 75 | withBlock.FuzzyBand1TopPercentage != 84 | withBlock.FuzzyBand2BottomPercentage != 85 | withBlock.FuzzyBand2TopPercentage != 94 | withBlock.FuzzyBand3BottomPercentage != 95 | withBlock.FuzzyBand3TopPercentage != 99)
                                    throw new Exception("It is not possible to import this Trados 2007 log file, because the fuzzy match bands for this organisation do not correspond to the Trados 2007 fuzzy match bands. The fuzzy match bands cannot be adjusted in Trados 2007, though they can be adjusted if you use Trados 2009/2011.");
                            }
                            else
                                // 2 bands
                                throw new Exception("It is not possible to import this Trados 2007 log file, because the fuzzy match bands for this organisation do not correspond to the Trados 2007 fuzzy match bands. The fuzzy match bands cannot be adjusted in Trados 2007, though they can be adjusted if you use Trados 2009/2011.");
                        }
                        else
                           // 1 band
                           if ((withBlock.FuzzyBand1BottomPercentage != 50 & withBlock.FuzzyBand1BottomPercentage != 75) | withBlock.FuzzyBand1TopPercentage != 99)
                            throw new Exception("It is not possible to import this Trados 2007 log file, because the fuzzy match bands for this organisation do not correspond to the Trados 2007 fuzzy match bands. The fuzzy match bands cannot be adjusted in Trados 2007, though they can be adjusted if you use Trados 2009/2011.");
                    }
                }


                Microsoft.VisualBasic.FileIO.TextFieldParser T2007CSVReader = new Microsoft.VisualBasic.FileIO.TextFieldParser(LogFilePath, System.Text.Encoding.Default);
                string[] WordCountFields;
                string[] WordCountFieldsVerified = null; // String()
                ArrayList AllWordCountFieldsFromFile = new ArrayList();
                int RowCount = 0;
                T2007CSVReader.TextFieldType = Microsoft.VisualBasic.FileIO.FieldType.Delimited;
                T2007CSVReader.Delimiters = new string[] { "," };
                T2007CSVReader.HasFieldsEnclosedInQuotes = true;

                // pull in each record in turn
                while (!T2007CSVReader.EndOfData)
                {
                    try
                    {
                        WordCountFields = T2007CSVReader.ReadFields();
                        RowCount += 1;
                        if (RowCount > 2)
                        {
                            int Count = 0;
                            string Word = "";
                            foreach (string WordCount in WordCountFields)
                            {
                                if (Regex.IsMatch(WordCount, "^[0-9]+$"))
                                {
                                    string[] oldWordCountFieldsVerified = null;
                                    if (Word != "")
                                    {
                                        oldWordCountFieldsVerified = WordCountFieldsVerified;
                                        WordCountFieldsVerified = new string[Count + 1];
                                        if (oldWordCountFieldsVerified != null)
                                            Array.Copy(oldWordCountFieldsVerified, WordCountFieldsVerified, Math.Min(Count + 1, oldWordCountFieldsVerified.Length));
                                        WordCountFieldsVerified[Count] = Word;
                                        Word = "";
                                        Count += 1;
                                    }

                                    oldWordCountFieldsVerified = WordCountFieldsVerified;
                                    WordCountFieldsVerified = new string[Count + 1];
                                    if (oldWordCountFieldsVerified != null)
                                        Array.Copy(oldWordCountFieldsVerified, WordCountFieldsVerified, Math.Min(Count + 1, oldWordCountFieldsVerified.Length));
                                    WordCountFieldsVerified[Count] = WordCount;
                                    Count += 1;
                                }
                                else
                                    Word += WordCount;
                            }
                            AllWordCountFieldsFromFile.Add(WordCountFieldsVerified);
                        }
                        else
                            // WordCountFields = T2007CSVReader.ReadFields
                            AllWordCountFieldsFromFile.Add(WordCountFields);
                    }
                    catch (Microsoft.VisualBasic.FileIO.MalformedLineException ex)
                    {
                        T2007CSVReader.Close();
                        throw new Exception(string.Format("There was an error reading word count data from file {0} due to a \"mal-formed line\" exception, so processing cannot continue. This suggests that the log file is damaged or not a valid Trados 2007 .csv log file. The specific error is: {1}", LogFileInfo.FullName, ex.Message));
                    }
                }
                T2007CSVReader.Close();

                // only now start assigning word counts, now that we know the 
                // entire file has parsed successfully as CSV
                RowCount = 0;
                foreach (string[] ThisWordCountFields in AllWordCountFieldsFromFile)
                {
                    RowCount += 1;
                    // first two rows of a Trados 2007 CSV file are column headings, so ignore them
                    if (RowCount > 2)
                    {
                        FileNameForThisFile = (ThisWordCountFields[0]).Trim();
                        pNumberOfFilesAnalysed += 1;
                        MatchPlusOrPerfectMatchWordsForThisFile = System.Convert.ToInt32((ThisWordCountFields[4]).Trim());
                        model.pMatchPlusOrPerfectMatchWords += MatchPlusOrPerfectMatchWordsForThisFile;
                        RepetitionsWordsForThisFile = System.Convert.ToInt32((ThisWordCountFields[8]).Trim());
                        model.pRepetitionsWords += RepetitionsWordsForThisFile;
                        ExactMatchWordsForThisFile = System.Convert.ToInt32((ThisWordCountFields[12]).Trim());
                        model.pExactMatchWords += ExactMatchWordsForThisFile;
                        // what we call "fuzzy" is from 3 bands in Trados - we could change this in future
                        // FuzzyBand1WordsForThisFile = CInt(Trim(ThisWordCountFields(16))) + CInt(Trim(ThisWordCountFields(20))) + CInt(Trim(ThisWordCountFields(24)))
                        if (ThisOrg.FuzzyBand1BottomPercentage == 75)
                        {
                            FuzzyBand3WordsForThisFile = System.Convert.ToInt32((ThisWordCountFields[16]).Trim());
                            model.pFuzzyBand3Words += FuzzyBand3WordsForThisFile;
                            FuzzyBand2WordsForThisFile = System.Convert.ToInt32((ThisWordCountFields[20]).Trim());
                            model.pFuzzyBand2Words += FuzzyBand2WordsForThisFile;
                            FuzzyBand1WordsForThisFile = System.Convert.ToInt32((ThisWordCountFields[24]).Trim());
                            model.pFuzzyBand1Words += FuzzyBand1WordsForThisFile;
                            // what we call "new" is the lowest fuzzy band (50%-74%) in Trados plus "actual" new words
                            // as far as Trados is concerned
                            NewWordsForThisFile = System.Convert.ToInt32((ThisWordCountFields[28]).Trim()) + System.Convert.ToInt32((ThisWordCountFields[32]).Trim());
                            model.pNewWords += NewWordsForThisFile;
                            FuzzyBand4WordsForThisFile = 0;
                            model.pFuzzyBand4Words = FuzzyBand4WordsForThisFile;
                        }
                        else
                        {
                            FuzzyBand4WordsForThisFile = System.Convert.ToInt32((ThisWordCountFields[16]).Trim());
                            model.pFuzzyBand4Words += FuzzyBand4WordsForThisFile;
                            FuzzyBand3WordsForThisFile = System.Convert.ToInt32((ThisWordCountFields[20]).Trim());
                            model.pFuzzyBand3Words += FuzzyBand3WordsForThisFile;
                            FuzzyBand2WordsForThisFile = System.Convert.ToInt32((ThisWordCountFields[24]).Trim());
                            model.pFuzzyBand2Words += FuzzyBand2WordsForThisFile;
                            FuzzyBand1WordsForThisFile = System.Convert.ToInt32((ThisWordCountFields[28]).Trim());
                            model.pFuzzyBand1Words += FuzzyBand1WordsForThisFile;
                            NewWordsForThisFile = System.Convert.ToInt32((ThisWordCountFields[32]).Trim());
                            model.pNewWords += NewWordsForThisFile;
                        }

                        if (Bands == 1)
                        {
                            FuzzyBand1WordsForThisFile = FuzzyBand2WordsForThisFile + FuzzyBand3WordsForThisFile;
                            model.pFuzzyBand1Words += FuzzyBand1WordsForThisFile;
                            FuzzyBand2WordsForThisFile = 0;
                            model.pFuzzyBand2Words = FuzzyBand2WordsForThisFile;
                            FuzzyBand3WordsForThisFile = 0;
                            model.pFuzzyBand3Words = FuzzyBand3WordsForThisFile;
                            FuzzyBand4WordsForThisFile = 0;
                            model.pFuzzyBand4Words = FuzzyBand4WordsForThisFile;
                        }

                        // add linguistic word counts
                        LinguisticFuzzyBand4WordsForThisFile = System.Convert.ToInt32((ThisWordCountFields[16]).Trim());
                        model.pLinguisticFuzzyBand4Words += LinguisticFuzzyBand4WordsForThisFile;
                        LinguisticFuzzyBand3WordsForThisFile = System.Convert.ToInt32((ThisWordCountFields[20]).Trim());
                        model.pLinguisticFuzzyBand3Words += LinguisticFuzzyBand3WordsForThisFile;
                        LinguisticFuzzyBand2WordsForThisFile = System.Convert.ToInt32((ThisWordCountFields[24]).Trim());
                        model.pLinguisticFuzzyBand2Words += LinguisticFuzzyBand2WordsForThisFile;
                        LinguisticFuzzyBand1WordsForThisFile = System.Convert.ToInt32((ThisWordCountFields[28]).Trim());
                        model.pLinguisticFuzzyBand1Words += LinguisticFuzzyBand1WordsForThisFile;
                        LinguisticNewWordsForThisFile = System.Convert.ToInt32((ThisWordCountFields[32]).Trim());
                        model.pLinguisticNewWords += LinguisticNewWordsForThisFile;

                        // create new word count breakdown for this row
                        //model.pWordCountBreakdowns.Add(new TPWordCountBreakdown { pNewWords = NewWordsForThisFile, pFuzzyBand1Words = FuzzyBand1WordsForThisFile, pFuzzyBand2Words = FuzzyBand2WordsForThisFile, pFuzzyBand3Words = FuzzyBand3WordsForThisFile, pFuzzyBand4Words = FuzzyBand4WordsForThisFile, pLinguisticNewWords = LinguisticNewWordsForThisFile, pLinguisticFuzzyBand1Words = LinguisticFuzzyBand1WordsForThisFile, pLinguisticFuzzyBand2Words = LinguisticFuzzyBand2WordsForThisFile, pLinguisticFuzzyBand3Words = LinguisticFuzzyBand3WordsForThisFile, pLinguisticFuzzyBand4Words = LinguisticFuzzyBand4WordsForThisFile, pRepetitionsWords = RepetitionsWordsForThisFile, pExactMatchWords = ExactMatchWordsForThisFile, pMatchPlusOrPerfectMatchWords = MatchPlusOrPerfectMatchWordsForThisFile, pMemoryFormat = pMemoryFormat, pFileName = FileNameForThisFile });
                    }
                }
                return model;
            }
            else if (pMemoryFormat == Global_Settings.Enumerations.MemoryApplications.Trados2009 | pMemoryFormat == Global_Settings.Enumerations.MemoryApplications.Trados2011 | pMemoryFormat == Global_Settings.Enumerations.MemoryApplications.Trados2014)
            {
                // Trados 2009-2014 XML log format

                XmlDocument Trados2009Through2014XMLDoc = new XmlDocument();
                try
                {
                    Trados2009Through2014XMLDoc.Load(LogFilePath);
                }
                catch (XmlException ex)
                {
                    throw new Exception(string.Format("There was an error reading word count data from file {0} due to an XML parsing exception, so processing cannot continue. This suggests that the log file is damaged or not a valid Trados 2009/2011/2014 .xml log file. The specific error is: {1}", LogFileInfo.FullName, ex.Message));
                }

                // pNumberOfFilesAnalysed = Trados2009Or2001XMLDoc.SelectNodes("task[@name = 'analyse']/file").Count

                // Dim BatchTotalNode As XmlNode = Trados2009Or2001XMLDoc.SelectSingleNode("task[@name = 'analyse']/batchTotal/analyse")

                string WordsOrChars = "words";
                XmlNodeList FileNodesList = Trados2009Through2014XMLDoc.SelectNodes("task[@name = 'analyse']/file/analyse");
                JobItem ThisJobItem = null/* TODO Change to default(_) if this is not a reference type */;
                JobOrder ParentJobOrder = null;
                Contact OrderContact = null;
                Org ParentOrg = null;
                var JobItemID = 0;
                ParentOrg = ThisOrg;
                ParentJobOrder = ThisJobOrder;
                foreach (XmlNode FileNode in FileNodesList)
                {
                    FileNameForThisFile = FileNode.ParentNode.Attributes["name"].Value;

                    if (JobItemID == 0 || (ThisJobItem != null && ParentOrg.Id == 79702 && FileNameForThisFile.StartsWith(JobItemID.ToString()) == true) || (ThisJobItem != null && ToGetWordCountsFromOneFileOnly == true) || (ThisJobItem != null && ParentOrg.Id == 66926) || (ThisJobItem != null && ParentJobOrder.IsMachineTranslationJobFromiPlus == true))
                    {
                        if (ThisJobItem != null && ToGetWordCountsFromOneFileOnly == true)
                        {
                            if (ThisJobItem.FileName != FileNameForThisFile.Substring(FileNameForThisFile.LastIndexOf(@"\") + 1).Replace(".sdlxliff", ""))
                                continue;
                        }

                        try
                        {
                            if (FileNode.SelectSingleNode("perfect").Attributes["words"] == null)
                                WordsOrChars = "characters";
                        }
                        catch
                        {
                        }

                        MatchPlusOrPerfectMatchWordsForThisFile = System.Convert.ToInt32((FileNode.SelectSingleNode("perfect").Attributes["" + WordsOrChars + ""].Value).Trim()) + System.Convert.ToInt32((FileNode.SelectSingleNode("inContextExact").Attributes["" + WordsOrChars + ""].Value).Trim());
                        model.pMatchPlusOrPerfectMatchWords += MatchPlusOrPerfectMatchWordsForThisFile;
                        if (FileNode.SelectSingleNode("crossFileRepeated") == null)
                            RepetitionsWordsForThisFile = System.Convert.ToInt32((FileNode.SelectSingleNode("repeated").Attributes["" + WordsOrChars + ""].Value).Trim());
                        else
                            RepetitionsWordsForThisFile = System.Convert.ToInt32((FileNode.SelectSingleNode("repeated").Attributes["" + WordsOrChars + ""].Value).Trim()) + System.Convert.ToInt32((FileNode.SelectSingleNode("crossFileRepeated").Attributes["" + WordsOrChars + ""].Value).Trim());
                        model.pRepetitionsWords += RepetitionsWordsForThisFile;
                        ExactMatchWordsForThisFile = System.Convert.ToInt32((FileNode.SelectSingleNode("exact").Attributes["" + WordsOrChars + ""].Value).Trim());
                        model.pExactMatchWords += ExactMatchWordsForThisFile;
                        // what we call "fuzzy" is from 3 bands in Trados - we could change this in future
                        // also, for T2009 and T2011 specifically, these tools support internal fuzzy matches, so add those
                        // too if that option was selected - just check for first node to see if it needs to be incorporated
                        XmlNode TestInternalFuzzyNode = FileNode.SelectSingleNode("internalFuzzy");
                        if (TestInternalFuzzyNode != null)
                        {
                            // add internal fuzzies + normal fuzzies
                            // FuzzyBand1WordsForThisFile = CInt(Trim(FileNode.SelectSingleNode("fuzzy[@min = '75']").Attributes("" & WordsOrChars & "").Value)) + CInt(Trim(FileNode.SelectSingleNode("fuzzy[@min = '85']").Attributes("" & WordsOrChars & "").Value)) + CInt(Trim(FileNode.SelectSingleNode("fuzzy[@min = '95']").Attributes("" & WordsOrChars & "").Value)) + _
                            // CInt(Trim(FileNode.SelectSingleNode("internalFuzzy[@min = '75']").Attributes("" & WordsOrChars & "").Value)) + CInt(Trim(FileNode.SelectSingleNode("internalFuzzy[@min = '85']").Attributes("" & WordsOrChars & "").Value)) + CInt(Trim(FileNode.SelectSingleNode("internalFuzzy[@min = '95']").Attributes("" & WordsOrChars & "").Value))

                            // add linguistic supplier word counts
                            try
                            {
                                LinguisticFuzzyBand1WordsForThisFile = System.Convert.ToInt32((FileNode.SelectSingleNode("fuzzy[@min = '50']").Attributes["" + WordsOrChars + ""].Value).Trim()) + System.Convert.ToInt32((FileNode.SelectSingleNode("internalFuzzy[@min = '50']").Attributes["" + WordsOrChars + ""].Value).Trim());
                                model.pLinguisticFuzzyBand1Words += LinguisticFuzzyBand1WordsForThisFile;
                                LinguisticFuzzyBand2WordsForThisFile = System.Convert.ToInt32((FileNode.SelectSingleNode("fuzzy[@min = '75']").Attributes["" + WordsOrChars + ""].Value).Trim()) + System.Convert.ToInt32((FileNode.SelectSingleNode("internalFuzzy[@min = '75']").Attributes["" + WordsOrChars + ""].Value).Trim());
                                model.pLinguisticFuzzyBand2Words += LinguisticFuzzyBand2WordsForThisFile;
                                LinguisticFuzzyBand3WordsForThisFile = System.Convert.ToInt32((FileNode.SelectSingleNode("fuzzy[@min = '85']").Attributes["" + WordsOrChars + ""].Value).Trim()) + System.Convert.ToInt32((FileNode.SelectSingleNode("internalFuzzy[@min = '85']").Attributes["" + WordsOrChars + ""].Value).Trim());
                                model.pLinguisticFuzzyBand3Words += LinguisticFuzzyBand3WordsForThisFile;
                                LinguisticFuzzyBand4WordsForThisFile = System.Convert.ToInt32((FileNode.SelectSingleNode("fuzzy[@min = '95']").Attributes["" + WordsOrChars + ""].Value).Trim()) + System.Convert.ToInt32((FileNode.SelectSingleNode("internalFuzzy[@min = '95']").Attributes["" + WordsOrChars + ""].Value).Trim());
                                model.pLinguisticFuzzyBand4Words += LinguisticFuzzyBand4WordsForThisFile;
                            }
                            catch (Exception ex)
                            {
                            }

                            if (LinguisticSupplierWordCountsOnly == false)
                            {
                                if (ThisOrg != null)
                                {
                                    {
                                        var withBlock = ThisOrg;
                                        if (withBlock.FuzzyBand2BottomPercentage != null/* TODO Change to default(_) if this is not a reference type */ & withBlock.FuzzyBand2BottomPercentage > 0)
                                        {
                                            if (withBlock.FuzzyBand4BottomPercentage != null/* TODO Change to default(_) if this is not a reference type */ & withBlock.FuzzyBand4BottomPercentage > 0)
                                            {
                                                // 4 bands
                                                if (withBlock.FuzzyBand1BottomPercentage != 50 | withBlock.FuzzyBand1TopPercentage != 74 | withBlock.FuzzyBand2BottomPercentage != 75 | withBlock.FuzzyBand2TopPercentage != 84 | withBlock.FuzzyBand3BottomPercentage != 85 | withBlock.FuzzyBand3TopPercentage != 94 | withBlock.FuzzyBand4BottomPercentage != 95 | withBlock.FuzzyBand4TopPercentage != 99)
                                                {
                                                    XmlNode FuzzyBand1Node = FileNode.SelectSingleNode("fuzzy[@min = '" + withBlock.FuzzyBand1BottomPercentage + "']");
                                                    XmlNode FuzzyBand2Node = FileNode.SelectSingleNode("fuzzy[@min = '" + withBlock.FuzzyBand2BottomPercentage + "']");
                                                    XmlNode FuzzyBand3Node = FileNode.SelectSingleNode("fuzzy[@min = '" + withBlock.FuzzyBand3BottomPercentage + "']");
                                                    XmlNode FuzzyBand4Node = FileNode.SelectSingleNode("fuzzy[@min = '" + withBlock.FuzzyBand4BottomPercentage + "']");

                                                    if (FuzzyBand1Node != null & FuzzyBand2Node != null & FuzzyBand3Node != null & FuzzyBand4Node != null)
                                                    {
                                                        FuzzyBand1WordsForThisFile = System.Convert.ToInt32((FuzzyBand1Node.Attributes["" + WordsOrChars + ""].Value).Trim()) + System.Convert.ToInt32((FileNode.SelectSingleNode("internalFuzzy[@min = '" + withBlock.FuzzyBand1BottomPercentage + "']").Attributes["" + WordsOrChars + ""].Value).Trim());
                                                        model.pFuzzyBand1Words += FuzzyBand1WordsForThisFile;
                                                        FuzzyBand2WordsForThisFile = System.Convert.ToInt32((FuzzyBand2Node.Attributes["" + WordsOrChars + ""].Value).Trim()) + System.Convert.ToInt32((FileNode.SelectSingleNode("internalFuzzy[@min = '" + withBlock.FuzzyBand2BottomPercentage + "']").Attributes["" + WordsOrChars + ""].Value).Trim());
                                                        model.pFuzzyBand2Words += FuzzyBand2WordsForThisFile;
                                                        FuzzyBand3WordsForThisFile = System.Convert.ToInt32((FuzzyBand3Node.Attributes["" + WordsOrChars + ""].Value).Trim()) + System.Convert.ToInt32((FileNode.SelectSingleNode("internalFuzzy[@min = '" + withBlock.FuzzyBand3BottomPercentage + "']").Attributes["" + WordsOrChars + ""].Value).Trim());
                                                        model.pFuzzyBand3Words += FuzzyBand3WordsForThisFile;
                                                        FuzzyBand4WordsForThisFile = System.Convert.ToInt32((FuzzyBand4Node.Attributes["" + WordsOrChars + ""].Value).Trim()) + System.Convert.ToInt32((FileNode.SelectSingleNode("internalFuzzy[@min = '" + withBlock.FuzzyBand4BottomPercentage + "']").Attributes["" + WordsOrChars + ""].Value).Trim());
                                                        model.pFuzzyBand4Words += FuzzyBand4WordsForThisFile;
                                                    }
                                                    else
                                                        throw new Exception("It is not possible to import this Trados 2009/2011/2014 log file, because the fuzzy match bands for this organisation do not correspond to the Trados fuzzy match bands. However, the fuzzy match bands can be adjusted in Trados 2009/2011/2014 via Tools->Options (for the global settings) or Project->Project Settings (for the current project only).");
                                                }
                                                else
                                                {
                                                    FuzzyBand1WordsForThisFile = System.Convert.ToInt32((FileNode.SelectSingleNode("fuzzy[@min = '50']").Attributes["" + WordsOrChars + ""].Value).Trim()) + System.Convert.ToInt32((FileNode.SelectSingleNode("internalFuzzy[@min = '50']").Attributes["" + WordsOrChars + ""].Value).Trim());
                                                    model.pFuzzyBand1Words += FuzzyBand1WordsForThisFile;
                                                    FuzzyBand2WordsForThisFile = System.Convert.ToInt32((FileNode.SelectSingleNode("fuzzy[@min = '75']").Attributes["" + WordsOrChars + ""].Value).Trim()) + System.Convert.ToInt32((FileNode.SelectSingleNode("internalFuzzy[@min = '75']").Attributes["" + WordsOrChars + ""].Value).Trim());
                                                    model.pFuzzyBand2Words += FuzzyBand2WordsForThisFile;
                                                    FuzzyBand3WordsForThisFile = System.Convert.ToInt32((FileNode.SelectSingleNode("fuzzy[@min = '85']").Attributes["" + WordsOrChars + ""].Value).Trim()) + System.Convert.ToInt32((FileNode.SelectSingleNode("internalFuzzy[@min = '85']").Attributes["" + WordsOrChars + ""].Value).Trim());
                                                    model.pFuzzyBand3Words += FuzzyBand3WordsForThisFile;
                                                    FuzzyBand4WordsForThisFile = System.Convert.ToInt32((FileNode.SelectSingleNode("fuzzy[@min = '95']").Attributes["" + WordsOrChars + ""].Value).Trim()) + System.Convert.ToInt32((FileNode.SelectSingleNode("internalFuzzy[@min = '95']").Attributes["" + WordsOrChars + ""].Value).Trim());
                                                    model.pFuzzyBand4Words += FuzzyBand4WordsForThisFile;
                                                }
                                            }
                                            else if (withBlock.FuzzyBand3BottomPercentage != null/* TODO Change to default(_) if this is not a reference type */ & withBlock.FuzzyBand3BottomPercentage > 0)
                                            {
                                                // 3 bands
                                                if (withBlock.FuzzyBand1BottomPercentage != 75 | withBlock.FuzzyBand1TopPercentage != 84 | withBlock.FuzzyBand2BottomPercentage != 85 | withBlock.FuzzyBand2TopPercentage != 94 | withBlock.FuzzyBand3BottomPercentage != 95 | withBlock.FuzzyBand3TopPercentage != 99)
                                                {
                                                    XmlNode FuzzyBand1Node = FileNode.SelectSingleNode("fuzzy[@min = '" + withBlock.FuzzyBand1BottomPercentage + "']");
                                                    XmlNode FuzzyBand2Node = FileNode.SelectSingleNode("fuzzy[@min = '" + withBlock.FuzzyBand2BottomPercentage + "']");
                                                    XmlNode FuzzyBand3Node = FileNode.SelectSingleNode("fuzzy[@min = '" + withBlock.FuzzyBand3BottomPercentage + "']");

                                                    if (FuzzyBand1Node != null & FuzzyBand2Node != null & FuzzyBand3Node != null)
                                                    {
                                                        FuzzyBand1WordsForThisFile = System.Convert.ToInt32((FuzzyBand1Node.Attributes["" + WordsOrChars + ""].Value).Trim()) + System.Convert.ToInt32((FileNode.SelectSingleNode("internalFuzzy[@min = '" + withBlock.FuzzyBand1BottomPercentage + "']").Attributes["" + WordsOrChars + ""].Value).Trim());
                                                        model.pFuzzyBand1Words += FuzzyBand1WordsForThisFile;
                                                        FuzzyBand2WordsForThisFile = System.Convert.ToInt32((FuzzyBand2Node.Attributes["" + WordsOrChars + ""].Value).Trim()) + System.Convert.ToInt32((FileNode.SelectSingleNode("internalFuzzy[@min = '" + withBlock.FuzzyBand2BottomPercentage + "']").Attributes["" + WordsOrChars + ""].Value).Trim());
                                                        model.pFuzzyBand2Words += FuzzyBand2WordsForThisFile;
                                                        FuzzyBand3WordsForThisFile = System.Convert.ToInt32((FuzzyBand3Node.Attributes["" + WordsOrChars + ""].Value).Trim()) + System.Convert.ToInt32((FileNode.SelectSingleNode("internalFuzzy[@min = '" + withBlock.FuzzyBand3BottomPercentage + "']").Attributes["" + WordsOrChars + ""].Value).Trim());
                                                        model.pFuzzyBand3Words += FuzzyBand3WordsForThisFile;
                                                    }
                                                    else
                                                        throw new Exception("It is not possible to import this Trados 2009/2011/2014 log file, because the fuzzy match bands for this organisation do not correspond to the Trados fuzzy match bands. However, the fuzzy match bands can be adjusted in Trados 2009/2011/2014 via Tools->Options (for the global settings) or Project->Project Settings (for the current project only).");
                                                }
                                                else
                                                {
                                                    FuzzyBand1WordsForThisFile = System.Convert.ToInt32((FileNode.SelectSingleNode("fuzzy[@min = '75']").Attributes["" + WordsOrChars + ""].Value).Trim()) + System.Convert.ToInt32((FileNode.SelectSingleNode("internalFuzzy[@min = '75']").Attributes["" + WordsOrChars + ""].Value).Trim());
                                                    model.pFuzzyBand1Words += FuzzyBand1WordsForThisFile;
                                                    FuzzyBand2WordsForThisFile = System.Convert.ToInt32((FileNode.SelectSingleNode("fuzzy[@min = '85']").Attributes["" + WordsOrChars + ""].Value).Trim()) + System.Convert.ToInt32((FileNode.SelectSingleNode("internalFuzzy[@min = '85']").Attributes["" + WordsOrChars + ""].Value).Trim());
                                                    model.pFuzzyBand2Words += FuzzyBand2WordsForThisFile;
                                                    FuzzyBand3WordsForThisFile = System.Convert.ToInt32((FileNode.SelectSingleNode("fuzzy[@min = '95']").Attributes["" + WordsOrChars + ""].Value).Trim()) + System.Convert.ToInt32((FileNode.SelectSingleNode("internalFuzzy[@min = '95']").Attributes["" + WordsOrChars + ""].Value).Trim());
                                                    model.pFuzzyBand3Words += FuzzyBand3WordsForThisFile;
                                                }
                                            }
                                            else
                                                                                        // 2 bands
                                                                                        if (ThisOrg.OrgGroupId == 18988)
                                            {
                                                if (withBlock.FuzzyBand1BottomPercentage != 50 | withBlock.FuzzyBand1TopPercentage != 74 | withBlock.FuzzyBand2BottomPercentage != 75 | withBlock.FuzzyBand2TopPercentage != 99)
                                                {
                                                    XmlNode FuzzyBand1Node = FileNode.SelectSingleNode("fuzzy[@min = '" + withBlock.FuzzyBand1BottomPercentage + "']");
                                                    XmlNode FuzzyBand2Node = FileNode.SelectSingleNode("fuzzy[@min = '" + withBlock.FuzzyBand2BottomPercentage + "']");
                                                    XmlNode FuzzyBand3Node = FileNode.SelectSingleNode("fuzzy[@min = '" + withBlock.FuzzyBand3BottomPercentage + "']");
                                                    XmlNode FuzzyBand4Node = FileNode.SelectSingleNode("fuzzy[@min = '" + withBlock.FuzzyBand4BottomPercentage + "']");

                                                    if (FuzzyBand1Node != null & FuzzyBand2Node != null & FuzzyBand3Node != null & FuzzyBand4Node != null)
                                                    {
                                                        FuzzyBand1WordsForThisFile = System.Convert.ToInt32((FuzzyBand1Node.Attributes["" + WordsOrChars + ""].Value).Trim()) + System.Convert.ToInt32((FileNode.SelectSingleNode("internalFuzzy[@min = '" + withBlock.FuzzyBand1BottomPercentage + "']").Attributes["" + WordsOrChars + ""].Value).Trim());
                                                        model.pFuzzyBand1Words += FuzzyBand1WordsForThisFile;
                                                        FuzzyBand2WordsForThisFile = System.Convert.ToInt32((FuzzyBand2Node.Attributes["" + WordsOrChars + ""].Value).Trim()) + System.Convert.ToInt32((FileNode.SelectSingleNode("internalFuzzy[@min = '" + withBlock.FuzzyBand2BottomPercentage + "']").Attributes["" + WordsOrChars + ""].Value).Trim()) + System.Convert.ToInt32((FuzzyBand3Node.Attributes["" + WordsOrChars + ""].Value).Trim()) + System.Convert.ToInt32((FileNode.SelectSingleNode("internalFuzzy[@min = '" + withBlock.FuzzyBand3BottomPercentage + "']").Attributes["" + WordsOrChars + ""].Value).Trim()) + System.Convert.ToInt32((FuzzyBand4Node.Attributes["" + WordsOrChars + ""].Value).Trim()) + System.Convert.ToInt32((FileNode.SelectSingleNode("internalFuzzy[@min = '" + withBlock.FuzzyBand4BottomPercentage + "']").Attributes["" + WordsOrChars + ""].Value).Trim());
                                                        model.pFuzzyBand2Words += FuzzyBand2WordsForThisFile;
                                                    }
                                                    else
                                                        throw new Exception("It is not possible to import this Trados 2009/2011/2014 log file, because the fuzzy match bands for this organisation do not correspond to the Trados fuzzy match bands. However, the fuzzy match bands can be adjusted in Trados 2009/2011/2014 via Tools->Options (for the global settings) or Project->Project Settings (for the current project only).");
                                                }
                                                else
                                                {
                                                    FuzzyBand1WordsForThisFile = System.Convert.ToInt32((FileNode.SelectSingleNode("fuzzy[@min = '50']").Attributes["" + WordsOrChars + ""].Value).Trim()) + System.Convert.ToInt32((FileNode.SelectSingleNode("internalFuzzy[@min = '50']").Attributes["" + WordsOrChars + ""].Value).Trim());
                                                    model.pFuzzyBand1Words += FuzzyBand1WordsForThisFile;
                                                    FuzzyBand2WordsForThisFile = System.Convert.ToInt32((FileNode.SelectSingleNode("fuzzy[@min = '75']").Attributes["" + WordsOrChars + ""].Value).Trim()) + System.Convert.ToInt32((FileNode.SelectSingleNode("internalFuzzy[@min = '75']").Attributes["" + WordsOrChars + ""].Value).Trim()) + System.Convert.ToInt32((FileNode.SelectSingleNode("fuzzy[@min = '85']").Attributes["" + WordsOrChars + ""].Value).Trim()) + System.Convert.ToInt32((FileNode.SelectSingleNode("internalFuzzy[@min = '85']").Attributes["" + WordsOrChars + ""].Value).Trim()) + System.Convert.ToInt32((FileNode.SelectSingleNode("fuzzy[@min = '95']").Attributes["" + WordsOrChars + ""].Value).Trim()) + System.Convert.ToInt32((FileNode.SelectSingleNode("internalFuzzy[@min = '95']").Attributes["" + WordsOrChars + ""].Value).Trim());

                                                    model.pFuzzyBand2Words += FuzzyBand2WordsForThisFile;
                                                }
                                            }
                                            else if (ThisOrg.Id == 101242)
                                            {
                                                XmlNode FuzzyBand1Node = FileNode.SelectSingleNode("fuzzy[@min = '" + withBlock.FuzzyBand1BottomPercentage + "']");
                                                XmlNode FuzzyBand2Node = FileNode.SelectSingleNode("fuzzy[@min = '" + withBlock.FuzzyBand2BottomPercentage + "']");

                                                if (FuzzyBand1Node != null & FuzzyBand2Node != null)
                                                {
                                                    FuzzyBand1WordsForThisFile = System.Convert.ToInt32((FileNode.SelectSingleNode("fuzzy[@min = '75']").Attributes["" + WordsOrChars + ""].Value).Trim()) + System.Convert.ToInt32((FileNode.SelectSingleNode("fuzzy[@min = '85']").Attributes["" + WordsOrChars + ""].Value).Trim());
                                                    model.pFuzzyBand1Words += FuzzyBand1WordsForThisFile;
                                                    // FuzzyBand1WordsForThisFile = CInt(Trim(FuzzyBand1Node.Attributes("" & WordsOrChars & "").Value)) + CInt(Trim(FileNode.SelectSingleNode("internalFuzzy[@min = '" & .FuzzyBand1BottomPercentage & "']").Attributes("" & WordsOrChars & "").Value))
                                                    // pFuzzyBand1Words += FuzzyBand1WordsForThisFile
                                                    FuzzyBand2WordsForThisFile = System.Convert.ToInt32((FuzzyBand2Node.Attributes["" + WordsOrChars + ""].Value).Trim());
                                                    model.pFuzzyBand2Words += FuzzyBand2WordsForThisFile;
                                                }
                                                else
                                                    throw new Exception("It is not possible to import this Trados 2009/2011/2014 log file, because the fuzzy match bands for this organisation do not correspond to the Trados fuzzy match bands. However, the fuzzy match bands can be adjusted in Trados 2009/2011/2014 via Tools->Options (for the global settings) or Project->Project Settings (for the current project only).");
                                            }
                                            else
                                                throw new Exception("It is not possible to import this Trados 2009/2011/2014 log file, because the fuzzy match bands for this organisation do not correspond to the Trados fuzzy match bands. However, the fuzzy match bands can be adjusted in Trados 2009/2011/2014 via Tools->Options (for the global settings) or Project->Project Settings (for the current project only).");
                                        }
                                        else
                                           // 1 band
                                           if (withBlock.FuzzyBand1BottomPercentage != 50 & withBlock.FuzzyBand1BottomPercentage != 75 | withBlock.FuzzyBand1TopPercentage != 99)
                                        {
                                            if (withBlock.FuzzyBand1TopPercentage != 99)
                                                throw new Exception("It is not possible to import this Trados 2009/2011/2014 log file, because the fuzzy match bands for this organisation do not correspond to the Trados fuzzy match bands. However, the fuzzy match bands can be adjusted in Trados 2009/2011/2014 via Tools->Options (for the global settings) or Project->Project Settings (for the current project only).");
                                            else if (withBlock.FuzzyBand1BottomPercentage < 50 | withBlock.FuzzyBand1BottomPercentage > 98)
                                                throw new Exception("It is not possible to import this Trados 2009/2011/2014 log file, because the fuzzy match bands for this organisation do not correspond to the Trados fuzzy match bands. However, the fuzzy match bands can be adjusted in Trados 2009/2011/2014 via Tools->Options (for the global settings) or Project->Project Settings (for the current project only).");
                                            else
                                                try
                                                {
                                                    FuzzyBand1WordsForThisFile = System.Convert.ToInt32((FileNode.SelectSingleNode("fuzzy[@min = '" + withBlock.FuzzyBand1BottomPercentage + "']").Attributes["" + WordsOrChars + ""].Value).Trim()) + System.Convert.ToInt32((FileNode.SelectSingleNode("internalFuzzy[@min = '" + withBlock.FuzzyBand1BottomPercentage + "']").Attributes["" + WordsOrChars + ""].Value).Trim());
                                                    model.pFuzzyBand1Words += FuzzyBand1WordsForThisFile;
                                                }
                                                catch (Exception ex)
                                                {
                                                    throw new Exception("It is not possible to import this Trados 2009/2011/2014 log file, because the fuzzy match bands for this organisation do not correspond to the Trados fuzzy match bands. However, the fuzzy match bands can be adjusted in Trados 2009/2011/2014 via Tools->Options (for the global settings) or Project->Project Settings (for the current project only).");
                                                }
                                        }
                                        else
                                               // all fuzzy in fuzzy band 1
                                               if (ThisOrg.FuzzyBand1BottomPercentage == 75)
                                        {
                                            FuzzyBand1WordsForThisFile = System.Convert.ToInt32((FileNode.SelectSingleNode("fuzzy[@min = '75']").Attributes["" + WordsOrChars + ""].Value).Trim()) + System.Convert.ToInt32((FileNode.SelectSingleNode("fuzzy[@min = '85']").Attributes["" + WordsOrChars + ""].Value).Trim()) + System.Convert.ToInt32((FileNode.SelectSingleNode("fuzzy[@min = '95']").Attributes["" + WordsOrChars + ""].Value).Trim()) + System.Convert.ToInt32((FileNode.SelectSingleNode("internalFuzzy[@min = '75']").Attributes["" + WordsOrChars + ""].Value).Trim()) + System.Convert.ToInt32((FileNode.SelectSingleNode("internalFuzzy[@min = '85']").Attributes["" + WordsOrChars + ""].Value).Trim()) + System.Convert.ToInt32((FileNode.SelectSingleNode("internalFuzzy[@min = '95']").Attributes["" + WordsOrChars + ""].Value).Trim());
                                            model.pFuzzyBand1Words += FuzzyBand1WordsForThisFile;
                                        }
                                        else if (ThisOrg.FuzzyBand1BottomPercentage == 50)
                                        {
                                            FuzzyBand1WordsForThisFile = System.Convert.ToInt32((FileNode.SelectSingleNode("fuzzy[@min = '50']").Attributes["" + WordsOrChars + ""].Value).Trim()) + System.Convert.ToInt32((FileNode.SelectSingleNode("fuzzy[@min = '75']").Attributes["" + WordsOrChars + ""].Value).Trim()) + System.Convert.ToInt32((FileNode.SelectSingleNode("fuzzy[@min = '85']").Attributes["" + WordsOrChars + ""].Value).Trim()) + System.Convert.ToInt32((FileNode.SelectSingleNode("fuzzy[@min = '95']").Attributes["" + WordsOrChars + ""].Value).Trim()) + System.Convert.ToInt32((FileNode.SelectSingleNode("internalFuzzy[@min = '50']").Attributes["" + WordsOrChars + ""].Value).Trim()) + System.Convert.ToInt32((FileNode.SelectSingleNode("internalFuzzy[@min = '75']").Attributes["" + WordsOrChars + ""].Value).Trim()) + System.Convert.ToInt32((FileNode.SelectSingleNode("internalFuzzy[@min = '85']").Attributes["" + WordsOrChars + ""].Value).Trim()) + System.Convert.ToInt32((FileNode.SelectSingleNode("internalFuzzy[@min = '95']").Attributes["" + WordsOrChars + ""].Value).Trim());
                                            model.pFuzzyBand1Words += FuzzyBand1WordsForThisFile;
                                        }
                                        else
                                            throw new Exception("It is not possible to import this Trados 2009/2011/2014 log file, because the fuzzy match bands for this organisation do not correspond to the Trados fuzzy match bands. However, the fuzzy match bands can be adjusted in Trados 2009/2011/2014 via Tools->Options (for the global settings) or Project->Project Settings (for the current project only).");
                                    }
                                }

                                // FuzzyBand1WordsForThisFile = CInt(Trim(FileNode.SelectSingleNode("fuzzy[@min = '95']").Attributes("" & WordsOrChars & "").Value)) + CInt(Trim(FileNode.SelectSingleNode("internalFuzzy[@min = '95']").Attributes("" & WordsOrChars & "").Value))
                                // pFuzzyBand1Words += FuzzyBand1WordsForThisFile
                                // FuzzyBand2WordsForThisFile = CInt(Trim(FileNode.SelectSingleNode("fuzzy[@min = '85']").Attributes("" & WordsOrChars & "").Value)) + CInt(Trim(FileNode.SelectSingleNode("internalFuzzy[@min = '85']").Attributes("" & WordsOrChars & "").Value))
                                // pFuzzyBand2Words += FuzzyBand2WordsForThisFile
                                // FuzzyBand3WordsForThisFile = CInt(Trim(FileNode.SelectSingleNode("fuzzy[@min = '75']").Attributes("" & WordsOrChars & "").Value)) + CInt(Trim(FileNode.SelectSingleNode("internalFuzzy[@min = '75']").Attributes("" & WordsOrChars & "").Value))
                                // pFuzzyBand3Words += FuzzyBand3WordsForThisFile
                                // what we call "new" is the lowest fuzzy band (50%-74%) in Trados plus "actual" new words
                                // as far as Trados is concerned
                                if (ThisOrg.FuzzyBand1BottomPercentage == 75)
                                {
                                    NewWordsForThisFile = System.Convert.ToInt32((FileNode.SelectSingleNode("fuzzy[@min = '50']").Attributes["" + WordsOrChars + ""].Value).Trim()) + System.Convert.ToInt32((FileNode.SelectSingleNode("internalFuzzy[@min = '50']").Attributes["" + WordsOrChars + ""].Value).Trim()) + System.Convert.ToInt32((FileNode.SelectSingleNode("new").Attributes["" + WordsOrChars + ""].Value).Trim());
                                    model.pNewWords += NewWordsForThisFile;
                                }
                                else
                                {
                                    NewWordsForThisFile = System.Convert.ToInt32((FileNode.SelectSingleNode("new").Attributes["" + WordsOrChars + ""].Value).Trim());
                                    model.pNewWords += NewWordsForThisFile;
                                }
                            }

                            LinguisticNewWordsForThisFile = System.Convert.ToInt32((FileNode.SelectSingleNode("new").Attributes["" + WordsOrChars + ""].Value).Trim());
                            model.pLinguisticNewWords += LinguisticNewWordsForThisFile;
                        }
                        else
                        {
                            // add linguistic supplier word counts
                            try
                            {
                                // Special fuzzy band for Ramboll org
                                if (ThisOrg.OrgGroupId == 23055 && FileNode.SelectSingleNode("fuzzy[@min = '30']") != null)
                                {
                                    LinguisticFuzzyBand1WordsForThisFile = System.Convert.ToInt32((FileNode.SelectSingleNode("fuzzy[@min = '30']").Attributes["" + WordsOrChars + ""].Value).Trim()) + System.Convert.ToInt32((FileNode.SelectSingleNode("fuzzy[@min = '51']").Attributes["" + WordsOrChars + ""].Value).Trim());
                                    model.pLinguisticFuzzyBand1Words += LinguisticFuzzyBand1WordsForThisFile;
                                    LinguisticFuzzyBand2WordsForThisFile = System.Convert.ToInt32((FileNode.SelectSingleNode("fuzzy[@min = '75']").Attributes["" + WordsOrChars + ""].Value).Trim());
                                    model.pLinguisticFuzzyBand2Words += LinguisticFuzzyBand2WordsForThisFile;
                                    LinguisticFuzzyBand3WordsForThisFile = System.Convert.ToInt32((FileNode.SelectSingleNode("fuzzy[@min = '85']").Attributes["" + WordsOrChars + ""].Value).Trim());
                                    model.pLinguisticFuzzyBand3Words += LinguisticFuzzyBand3WordsForThisFile;
                                    LinguisticFuzzyBand4WordsForThisFile = System.Convert.ToInt32((FileNode.SelectSingleNode("fuzzy[@min = '95']").Attributes["" + WordsOrChars + ""].Value).Trim());
                                    model.pLinguisticFuzzyBand4Words += LinguisticFuzzyBand4WordsForThisFile;
                                }
                                else
                                {
                                    LinguisticFuzzyBand1WordsForThisFile = System.Convert.ToInt32((FileNode.SelectSingleNode("fuzzy[@min = '50']").Attributes["" + WordsOrChars + ""].Value).Trim());
                                    model.pLinguisticFuzzyBand1Words += LinguisticFuzzyBand1WordsForThisFile;
                                    LinguisticFuzzyBand2WordsForThisFile = System.Convert.ToInt32((FileNode.SelectSingleNode("fuzzy[@min = '75']").Attributes["" + WordsOrChars + ""].Value).Trim());
                                    model.pLinguisticFuzzyBand2Words += LinguisticFuzzyBand2WordsForThisFile;
                                    LinguisticFuzzyBand3WordsForThisFile = System.Convert.ToInt32((FileNode.SelectSingleNode("fuzzy[@min = '85']").Attributes["" + WordsOrChars + ""].Value).Trim());
                                    model.pLinguisticFuzzyBand3Words += LinguisticFuzzyBand3WordsForThisFile;
                                    LinguisticFuzzyBand4WordsForThisFile = System.Convert.ToInt32((FileNode.SelectSingleNode("fuzzy[@min = '95']").Attributes["" + WordsOrChars + ""].Value).Trim());
                                    model.pLinguisticFuzzyBand4Words += LinguisticFuzzyBand4WordsForThisFile;
                                }
                            }
                            catch (Exception ex)
                            {
                            }

                            if (LinguisticSupplierWordCountsOnly == false)
                            {
                                // just look for normal fuzzies
                                if (ThisOrg != null)
                                {
                                    {
                                        var withBlock = ThisOrg;
                                        if (withBlock.FuzzyBand2BottomPercentage != null/* TODO Change to default(_) if this is not a reference type */ & withBlock.FuzzyBand2BottomPercentage > 0)
                                        {
                                            if (withBlock.FuzzyBand4BottomPercentage != null/* TODO Change to default(_) if this is not a reference type */ & withBlock.FuzzyBand4BottomPercentage > 0)
                                            {
                                                // 4 bands
                                                if (withBlock.FuzzyBand1BottomPercentage != 50 | withBlock.FuzzyBand1TopPercentage != 74 | withBlock.FuzzyBand2BottomPercentage != 75 | withBlock.FuzzyBand2TopPercentage != 84 | withBlock.FuzzyBand3BottomPercentage != 85 | withBlock.FuzzyBand3TopPercentage != 94 | withBlock.FuzzyBand4BottomPercentage != 95 | withBlock.FuzzyBand4TopPercentage != 99)
                                                {
                                                    XmlNode TestFuzzyBand1Node = FileNode.SelectSingleNode("fuzzy[@min = '" + withBlock.FuzzyBand1BottomPercentage + "']");
                                                    XmlNode TestFuzzyBand2Node = FileNode.SelectSingleNode("fuzzy[@min = '" + withBlock.FuzzyBand2BottomPercentage + "']");
                                                    XmlNode TestFuzzyBand3Node = FileNode.SelectSingleNode("fuzzy[@min = '" + withBlock.FuzzyBand3BottomPercentage + "']");
                                                    XmlNode TestFuzzyBand4Node = FileNode.SelectSingleNode("fuzzy[@min = '" + withBlock.FuzzyBand4BottomPercentage + "']");

                                                    if (TestFuzzyBand1Node != null & TestFuzzyBand2Node != null & TestFuzzyBand3Node != null & TestFuzzyBand4Node != null)
                                                    {
                                                        FuzzyBand1WordsForThisFile = System.Convert.ToInt32((TestFuzzyBand1Node.Attributes["" + WordsOrChars + ""].Value).Trim());
                                                        model.pFuzzyBand1Words += FuzzyBand1WordsForThisFile;
                                                        FuzzyBand2WordsForThisFile = System.Convert.ToInt32((TestFuzzyBand2Node.Attributes["" + WordsOrChars + ""].Value).Trim());
                                                        model.pFuzzyBand2Words += FuzzyBand2WordsForThisFile;
                                                        FuzzyBand3WordsForThisFile = System.Convert.ToInt32((TestFuzzyBand3Node.Attributes["" + WordsOrChars + ""].Value).Trim());
                                                        model.pFuzzyBand3Words += FuzzyBand3WordsForThisFile;
                                                        FuzzyBand4WordsForThisFile = System.Convert.ToInt32((TestFuzzyBand4Node.Attributes["" + WordsOrChars + ""].Value).Trim());
                                                        model.pFuzzyBand4Words += FuzzyBand4WordsForThisFile;
                                                    }
                                                    else
                                                        throw new Exception("It is not possible to import this Trados 2009/2011/2014 log file, because the fuzzy match bands for this organisation do not correspond to the Trados fuzzy match bands. However, the fuzzy match bands can be adjusted in Trados 2009/2011/2014 via Tools->Options (for the global settings) or Project->Project Settings (for the current project only).");
                                                }
                                                else
                                                {
                                                    FuzzyBand1WordsForThisFile = System.Convert.ToInt32((FileNode.SelectSingleNode("fuzzy[@min = '50']").Attributes["" + WordsOrChars + ""].Value).Trim());
                                                    model.pFuzzyBand1Words += FuzzyBand1WordsForThisFile;
                                                    FuzzyBand2WordsForThisFile = System.Convert.ToInt32((FileNode.SelectSingleNode("fuzzy[@min = '75']").Attributes["" + WordsOrChars + ""].Value).Trim());
                                                    model.pFuzzyBand2Words += FuzzyBand2WordsForThisFile;
                                                    FuzzyBand3WordsForThisFile = System.Convert.ToInt32((FileNode.SelectSingleNode("fuzzy[@min = '85']").Attributes["" + WordsOrChars + ""].Value).Trim());
                                                    model.pFuzzyBand3Words += FuzzyBand3WordsForThisFile;
                                                    FuzzyBand4WordsForThisFile = System.Convert.ToInt32((FileNode.SelectSingleNode("fuzzy[@min = '95']").Attributes["" + WordsOrChars + ""].Value).Trim());
                                                    model.pFuzzyBand4Words += FuzzyBand4WordsForThisFile;
                                                }
                                            }
                                            else if (withBlock.FuzzyBand3BottomPercentage != null/* TODO Change to default(_) if this is not a reference type */ & withBlock.FuzzyBand3BottomPercentage > 0)
                                            {
                                                // 3 bands
                                                if (withBlock.FuzzyBand1BottomPercentage != 75 | withBlock.FuzzyBand1TopPercentage != 84 | withBlock.FuzzyBand2BottomPercentage != 85 | withBlock.FuzzyBand2TopPercentage != 94 | withBlock.FuzzyBand3BottomPercentage != 95 | withBlock.FuzzyBand3TopPercentage != 99)
                                                {
                                                    var FuzzyBand1BottomPercentage = withBlock.FuzzyBand1BottomPercentage;

                                                    // Special fuzzy band for Ramboll org
                                                    if (withBlock.OrgGroupId == 23055)
                                                        FuzzyBand1BottomPercentage = 30;

                                                    XmlNode TestFuzzyBand1Node = FileNode.SelectSingleNode("fuzzy[@min = '" + FuzzyBand1BottomPercentage + "']");
                                                    XmlNode TestFuzzyBand2Node = FileNode.SelectSingleNode("fuzzy[@min = '" + withBlock.FuzzyBand2BottomPercentage + "']");
                                                    XmlNode TestFuzzyBand3Node = FileNode.SelectSingleNode("fuzzy[@min = '" + withBlock.FuzzyBand3BottomPercentage + "']");

                                                    if (TestFuzzyBand1Node != null & TestFuzzyBand2Node != null & TestFuzzyBand3Node != null)
                                                    {
                                                        if (withBlock.OrgGroupId != 23055)
                                                        {
                                                            FuzzyBand1WordsForThisFile = System.Convert.ToInt32((TestFuzzyBand1Node.Attributes["" + WordsOrChars + ""].Value).Trim());
                                                            model.pFuzzyBand1Words += FuzzyBand1WordsForThisFile;
                                                            FuzzyBand2WordsForThisFile = System.Convert.ToInt32((TestFuzzyBand2Node.Attributes["" + WordsOrChars + ""].Value).Trim());
                                                            model.pFuzzyBand2Words += FuzzyBand2WordsForThisFile;
                                                            FuzzyBand3WordsForThisFile = System.Convert.ToInt32((TestFuzzyBand3Node.Attributes["" + WordsOrChars + ""].Value).Trim());
                                                            model.pFuzzyBand3Words += FuzzyBand3WordsForThisFile;
                                                        }
                                                        else
                                                        {
                                                            short FuzzyBand4BottomPercentage = 85;
                                                            XmlNode TestFuzzyBand4Node = FileNode.SelectSingleNode("fuzzy[@min = '" + FuzzyBand4BottomPercentage + "']");
                                                            short FuzzyBand5BottomPercentage = 95;
                                                            XmlNode TestFuzzyBand5Node = FileNode.SelectSingleNode("fuzzy[@min = '" + FuzzyBand5BottomPercentage + "']");

                                                            FuzzyBand1WordsForThisFile = System.Convert.ToInt32((TestFuzzyBand1Node.Attributes["" + WordsOrChars + ""].Value).Trim());
                                                            model.pFuzzyBand1Words += FuzzyBand1WordsForThisFile;
                                                            FuzzyBand2WordsForThisFile = System.Convert.ToInt32((TestFuzzyBand2Node.Attributes["" + WordsOrChars + ""].Value).Trim());
                                                            model.pFuzzyBand2Words += FuzzyBand2WordsForThisFile;
                                                            FuzzyBand3WordsForThisFile = System.Convert.ToInt32((TestFuzzyBand3Node.Attributes["" + WordsOrChars + ""].Value).Trim());
                                                            FuzzyBand4WordsForThisFile = System.Convert.ToInt32((TestFuzzyBand4Node.Attributes["" + WordsOrChars + ""].Value).Trim());
                                                            int FuzzyBand5WordsForThisFile = System.Convert.ToInt32((TestFuzzyBand5Node.Attributes["" + WordsOrChars + ""].Value).Trim());
                                                            model.pFuzzyBand3Words += FuzzyBand3WordsForThisFile + FuzzyBand4WordsForThisFile + FuzzyBand5WordsForThisFile;
                                                        }
                                                    }
                                                    else
                                                        throw new Exception("It is not possible to import this Trados 2009/2011/2014 log file, because the fuzzy match bands for this organisation do not correspond to the Trados fuzzy match bands. However, the fuzzy match bands can be adjusted in Trados 2009/2011/2014 via Tools->Options (for the global settings) or Project->Project Settings (for the current project only).");
                                                }
                                                else
                                                {
                                                    FuzzyBand1WordsForThisFile = System.Convert.ToInt32((FileNode.SelectSingleNode("fuzzy[@min = '75']").Attributes["" + WordsOrChars + ""].Value).Trim());
                                                    model.pFuzzyBand1Words += FuzzyBand1WordsForThisFile;
                                                    FuzzyBand2WordsForThisFile = System.Convert.ToInt32((FileNode.SelectSingleNode("fuzzy[@min = '85']").Attributes["" + WordsOrChars + ""].Value).Trim());
                                                    model.pFuzzyBand2Words += FuzzyBand2WordsForThisFile;
                                                    FuzzyBand3WordsForThisFile = System.Convert.ToInt32((FileNode.SelectSingleNode("fuzzy[@min = '95']").Attributes["" + WordsOrChars + ""].Value).Trim());
                                                    model.pFuzzyBand3Words += FuzzyBand3WordsForThisFile;
                                                }
                                            }
                                            else
                                                                                        // 2 bands

                                                                                        if (ThisOrg.OrgGroupId == 18988)
                                            {
                                                if (withBlock.FuzzyBand1BottomPercentage != 50 | withBlock.FuzzyBand1TopPercentage != 74 | withBlock.FuzzyBand2BottomPercentage != 75 | withBlock.FuzzyBand2TopPercentage != 99)
                                                {
                                                    XmlNode TestFuzzyBand1Node = FileNode.SelectSingleNode("fuzzy[@min = '" + withBlock.FuzzyBand1BottomPercentage + "']");
                                                    XmlNode TestFuzzyBand2Node = FileNode.SelectSingleNode("fuzzy[@min = '" + withBlock.FuzzyBand2BottomPercentage + "']");
                                                    XmlNode TestFuzzyBand3Node = FileNode.SelectSingleNode("fuzzy[@min = '" + withBlock.FuzzyBand3BottomPercentage + "']");
                                                    XmlNode TestFuzzyBand4Node = FileNode.SelectSingleNode("fuzzy[@min = '" + withBlock.FuzzyBand4BottomPercentage + "']");

                                                    if (TestFuzzyBand1Node != null & TestFuzzyBand2Node != null & TestFuzzyBand3Node != null & TestFuzzyBand4Node != null)
                                                    {
                                                        FuzzyBand1WordsForThisFile = System.Convert.ToInt32((TestFuzzyBand1Node.Attributes["" + WordsOrChars + ""].Value).Trim());
                                                        model.pFuzzyBand1Words += FuzzyBand1WordsForThisFile;
                                                        FuzzyBand2WordsForThisFile = System.Convert.ToInt32((TestFuzzyBand2Node.Attributes["" + WordsOrChars + ""].Value).Trim()) + System.Convert.ToInt32((TestFuzzyBand3Node.Attributes["" + WordsOrChars + ""].Value).Trim()) + System.Convert.ToInt32((TestFuzzyBand4Node.Attributes["" + WordsOrChars + ""].Value).Trim());
                                                        model.pFuzzyBand2Words += FuzzyBand2WordsForThisFile;
                                                    }
                                                    else
                                                        throw new Exception("It is not possible to import this Trados 2009/2011/2014 log file, because the fuzzy match bands for this organisation do not correspond to the Trados fuzzy match bands. However, the fuzzy match bands can be adjusted in Trados 2009/2011/2014 via Tools->Options (for the global settings) or Project->Project Settings (for the current project only).");
                                                }
                                                else
                                                {
                                                    FuzzyBand1WordsForThisFile = System.Convert.ToInt32((FileNode.SelectSingleNode("fuzzy[@min = '50']").Attributes["" + WordsOrChars + ""].Value).Trim());
                                                    model.pFuzzyBand1Words += FuzzyBand1WordsForThisFile;
                                                    FuzzyBand2WordsForThisFile = System.Convert.ToInt32((FileNode.SelectSingleNode("fuzzy[@min = '75']").Attributes["" + WordsOrChars + ""].Value).Trim()) + System.Convert.ToInt32((FileNode.SelectSingleNode("fuzzy[@min = '85']").Attributes["" + WordsOrChars + ""].Value).Trim()) + System.Convert.ToInt32((FileNode.SelectSingleNode("fuzzy[@min = '95']").Attributes["" + WordsOrChars + ""].Value).Trim());
                                                    model.pFuzzyBand2Words += FuzzyBand2WordsForThisFile;
                                                }
                                            }
                                            else if (ThisOrg.Id == 101242)
                                            {
                                                XmlNode FuzzyBand1Node = FileNode.SelectSingleNode("fuzzy[@min = '" + withBlock.FuzzyBand1BottomPercentage + "']");
                                                XmlNode FuzzyBand2Node = FileNode.SelectSingleNode("fuzzy[@min = '" + withBlock.FuzzyBand2BottomPercentage + "']");

                                                if (FuzzyBand1Node != null & FuzzyBand2Node != null)
                                                {
                                                    FuzzyBand1WordsForThisFile = System.Convert.ToInt32((FileNode.SelectSingleNode("fuzzy[@min = '75']").Attributes["" + WordsOrChars + ""].Value).Trim()) + System.Convert.ToInt32((FileNode.SelectSingleNode("fuzzy[@min = '85']").Attributes["" + WordsOrChars + ""].Value).Trim());
                                                    model.pFuzzyBand1Words += FuzzyBand1WordsForThisFile;
                                                    // FuzzyBand1WordsForThisFile = CInt(Trim(FuzzyBand1Node.Attributes("" & WordsOrChars & "").Value)) + CInt(Trim(FileNode.SelectSingleNode("internalFuzzy[@min = '" & .FuzzyBand1BottomPercentage & "']").Attributes("" & WordsOrChars & "").Value))
                                                    // pFuzzyBand1Words += FuzzyBand1WordsForThisFile
                                                    FuzzyBand2WordsForThisFile = System.Convert.ToInt32((FuzzyBand2Node.Attributes["" + WordsOrChars + ""].Value).Trim());
                                                    model.pFuzzyBand2Words += FuzzyBand2WordsForThisFile;
                                                }
                                                else
                                                    throw new Exception("It is not possible to import this Trados 2009/2011/2014 log file, because the fuzzy match bands for this organisation do not correspond to the Trados fuzzy match bands. However, the fuzzy match bands can be adjusted in Trados 2009/2011/2014 via Tools->Options (for the global settings) or Project->Project Settings (for the current project only).");
                                            }
                                            else
                                                throw new Exception("It is not possible to import this Trados 2009/2011/2014 log file, because the fuzzy match bands for this organisation do not correspond to the Trados fuzzy match bands. However, the fuzzy match bands can be adjusted in Trados 2009/2011/2014 via Tools->Options (for the global settings) or Project->Project Settings (for the current project only).");
                                        }
                                        else
                                           // 1 band
                                           if (withBlock.FuzzyBand1BottomPercentage != 50 & withBlock.FuzzyBand1BottomPercentage != 75 | withBlock.FuzzyBand1TopPercentage != 99)
                                        {
                                            if (withBlock.FuzzyBand1TopPercentage != 99)
                                                throw new Exception("It is not possible to import this Trados 2009/2011/2014 log file, because the fuzzy match bands for this organisation do not correspond to the Trados fuzzy match bands. However, the fuzzy match bands can be adjusted in Trados 2009/2011/2014 via Tools->Options (for the global settings) or Project->Project Settings (for the current project only).");
                                            else if (withBlock.FuzzyBand1BottomPercentage < 50 | withBlock.FuzzyBand1BottomPercentage > 98)
                                                throw new Exception("It is not possible to import this Trados 2009/2011/2014 log file, because the fuzzy match bands for this organisation do not correspond to the Trados fuzzy match bands. However, the fuzzy match bands can be adjusted in Trados 2009/2011/2014 via Tools->Options (for the global settings) or Project->Project Settings (for the current project only).");
                                            else
                                                try
                                                {
                                                    FuzzyBand1WordsForThisFile = System.Convert.ToInt32((FileNode.SelectSingleNode("fuzzy[@min = '" + withBlock.FuzzyBand1BottomPercentage + "']").Attributes["" + WordsOrChars + ""].Value).Trim()) + System.Convert.ToInt32((FileNode.SelectSingleNode("internalFuzzy[@min = '" + withBlock.FuzzyBand1BottomPercentage + "']").Attributes["" + WordsOrChars + ""].Value).Trim());
                                                    model.pFuzzyBand1Words += FuzzyBand1WordsForThisFile;
                                                }
                                                catch (Exception ex)
                                                {
                                                    throw new Exception("It is not possible to import this Trados 2009/2011/2014 log file, because the fuzzy match bands for this organisation do not correspond to the Trados fuzzy match bands. However, the fuzzy match bands can be adjusted in Trados 2009/2011/2014 via Tools->Options (for the global settings) or Project->Project Settings (for the current project only).");
                                                }
                                        }
                                        else
                                               // all fuzzy in fuzzy band 1
                                               if (ThisOrg.FuzzyBand1BottomPercentage == 75)
                                        {
                                            FuzzyBand1WordsForThisFile = System.Convert.ToInt32((FileNode.SelectSingleNode("fuzzy[@min = '75']").Attributes["" + WordsOrChars + ""].Value).Trim()) + System.Convert.ToInt32((FileNode.SelectSingleNode("fuzzy[@min = '85']").Attributes["" + WordsOrChars + ""].Value).Trim()) + System.Convert.ToInt32((FileNode.SelectSingleNode("fuzzy[@min = '95']").Attributes["" + WordsOrChars + ""].Value).Trim());
                                            model.pFuzzyBand1Words += FuzzyBand1WordsForThisFile;
                                        }
                                        else if (ThisOrg.FuzzyBand1BottomPercentage == 50)
                                        {
                                            FuzzyBand1WordsForThisFile = System.Convert.ToInt32((FileNode.SelectSingleNode("fuzzy[@min = '50']").Attributes["" + WordsOrChars + ""].Value).Trim()) + System.Convert.ToInt32((FileNode.SelectSingleNode("fuzzy[@min = '75']").Attributes["" + WordsOrChars + ""].Value).Trim()) + System.Convert.ToInt32((FileNode.SelectSingleNode("fuzzy[@min = '85']").Attributes["" + WordsOrChars + ""].Value).Trim()) + System.Convert.ToInt32((FileNode.SelectSingleNode("fuzzy[@min = '95']").Attributes["" + WordsOrChars + ""].Value).Trim());
                                            model.pFuzzyBand1Words += FuzzyBand1WordsForThisFile;
                                        }
                                        else
                                            throw new Exception("It is not possible to import this Trados 2009/2011/2014 log file, because the fuzzy match bands for this organisation do not correspond to the Trados fuzzy match bands. However, the fuzzy match bands can be adjusted in Trados 2009/2011/2014 via Tools->Options (for the global settings) or Project->Project Settings (for the current project only).");
                                    }
                                }

                                // FuzzyBand1WordsForThisFile = CInt(Trim(FileNode.SelectSingleNode("fuzzy[@min = '75']").Attributes("" & WordsOrChars & "").Value)) + CInt(Trim(FileNode.SelectSingleNode("fuzzy[@min = '85']").Attributes("" & WordsOrChars & "").Value)) + CInt(Trim(FileNode.SelectSingleNode("fuzzy[@min = '95']").Attributes("" & WordsOrChars & "").Value))
                                // pFuzzyBand1Words += FuzzyBand1WordsForThisFile
                                // what we call "new" is the lowest fuzzy band (50%-74%) in Trados plus "actual" new words
                                // as far as Trados is concerned
                                if (ThisOrg.FuzzyBand1BottomPercentage == 75)
                                {
                                    NewWordsForThisFile = System.Convert.ToInt32((FileNode.SelectSingleNode("fuzzy[@min = '50']").Attributes["" + WordsOrChars + ""].Value).Trim()) + System.Convert.ToInt32((FileNode.SelectSingleNode("new").Attributes["" + WordsOrChars + ""].Value).Trim());
                                    model.pNewWords += NewWordsForThisFile;
                                }
                                else
                                {
                                    NewWordsForThisFile = System.Convert.ToInt32((FileNode.SelectSingleNode("new").Attributes["" + WordsOrChars + ""].Value).Trim());
                                    model.pNewWords += NewWordsForThisFile;
                                }
                            }

                            LinguisticNewWordsForThisFile = System.Convert.ToInt32((FileNode.SelectSingleNode("new").Attributes["" + WordsOrChars + ""].Value).Trim());
                            model.pLinguisticNewWords += LinguisticNewWordsForThisFile;
                        }

                        // create new word count breakdown for this node
                        //model.pWordCountBreakdowns.Add(new TPWordCountBreakdown { pNewWords = NewWordsForThisFile, pFuzzyBand1Words = FuzzyBand1WordsForThisFile, pFuzzyBand2Words = FuzzyBand2WordsForThisFile, pFuzzyBand3Words = FuzzyBand3WordsForThisFile, pFuzzyBand4Words = FuzzyBand4WordsForThisFile, pLinguisticNewWords = LinguisticNewWordsForThisFile, pLinguisticFuzzyBand1Words = LinguisticFuzzyBand1WordsForThisFile, pLinguisticFuzzyBand2Words = LinguisticFuzzyBand2WordsForThisFile, pLinguisticFuzzyBand3Words = LinguisticFuzzyBand3WordsForThisFile, pLinguisticFuzzyBand4Words = LinguisticFuzzyBand4WordsForThisFile, pRepetitionsWords = RepetitionsWordsForThisFile, pExactMatchWords = ExactMatchWordsForThisFile, pMatchPlusOrPerfectMatchWords = MatchPlusOrPerfectMatchWordsForThisFile, pMemoryFormat = pMemoryFormat, pFileName = FileNameForThisFile });


                        if (ThisJobItem != null && ToGetWordCountsFromOneFileOnly == true)
                        {
                            if (ThisJobItem.FileName == FileNameForThisFile)
                                break;
                        }
                    }

                    if ((ThisJobItem != null && ThisOrg.Id == 66926 && ParentJobOrder.ClientNotes.ToLower().Contains("machine translation") == true) | (ThisJobItem != null && ParentJobOrder.IsMachineTranslationJobFromiPlus == true))
                    {
                        model.pTotalCharacterCount = Convert.ToInt32(FileNode.SelectSingleNode("new").Attributes["characters"].Value);

                        int LowerFuzzyBandRange = Convert.ToInt32(ThisOrg.FuzzyBand1BottomPercentage);
                        if (LowerFuzzyBandRange >= 50)
                        {
                            if (LowerFuzzyBandRange >= 75)
                                model.pTotalCharacterCount += Convert.ToInt32(FileNode.SelectSingleNode("fuzzy[@max = 74]").Attributes["characters"].Value);
                            if (LowerFuzzyBandRange >= 85)
                                model.pTotalCharacterCount += Convert.ToInt32(FileNode.SelectSingleNode("fuzzy[@max = 84]").Attributes["characters"].Value);
                            if (LowerFuzzyBandRange >= 95)
                                model.pTotalCharacterCount += Convert.ToInt32(FileNode.SelectSingleNode("fuzzy[@max = 94]").Attributes["characters"].Value);
                            if (LowerFuzzyBandRange >= 100)
                                model.pTotalCharacterCount += Convert.ToInt32(FileNode.SelectSingleNode("fuzzy[@max = 99]").Attributes["characters"].Value);
                        }
                    }
                } // FileNode
                return model;
            }
            else
                throw new Exception("This translation memory log file format is not currently supported for word-count automation.");
        }


    }
}
