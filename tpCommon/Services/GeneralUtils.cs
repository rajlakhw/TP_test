using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using System.Security.Cryptography;

namespace Services
{
    public static class GeneralUtils
    {
        public static DateTime GetCurrentUKTime()
        {
            var britishZone = TimeZoneInfo.FindSystemTimeZoneById("GMT Standard Time");
            return TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.Local, britishZone);
        }

        //Session extensions
        public static void Set<T>(this ISession session, string key, T value)
        {
            session.SetString(key, JsonSerializer.Serialize(value));
        }

        public static T Get<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default : JsonSerializer.Deserialize<T>(value);
        }

        public static DateTime GetCurrentGMT() => TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("GMT Standard Time"));

        public static string GetUsernameFromNetwokUsername(string username)
        {
            var res = username;
            if (string.IsNullOrEmpty(username)) { return Environment.UserName; }
            if (username.Contains(@"\"))
            {
                res = username.Remove(0, username.LastIndexOf(@"\") + 1);
            }
            return res;
        }

        public static string GetARandomInspirationalQuote()
        {
            string InspirationalQuote = "";

            //StreamReader Reader = File.OpenText(@"\\fredcpfspdm0003.global.publicisgroupe.net\SharedInfo\IT and Network Infrastructure\share plus\Inspirational quotes.txt");
            StreamReader Reader = File.OpenText(Path.Combine(Environment.CurrentDirectory, "wwwroot", "Inspirational quotes.txt"));
            List<string> AllInspirationalQuotes = new List<string>();
            while (Reader.EndOfStream == false)
            {
                AllInspirationalQuotes.Add(Reader.ReadLine());
            }
            Reader.Close();


            Random RandomPick = new Random();
            InspirationalQuote = AllInspirationalQuotes[RandomPick.Next(0, AllInspirationalQuotes.Count - 1)];

            return InspirationalQuote;
        }

        public static (DateTime startWeekDate, DateTime endWeekDate) GetStartAndEndDateOfCurrentWeek()
        {
            DateTime baseDate = DateTime.Today;
            var daysToRemove = -(int)(baseDate.DayOfWeek - 1);
            var thisWeekStart = baseDate.AddDays(daysToRemove);
            var thisWeekEnd = thisWeekStart.AddDays(7).AddSeconds(-1);
            return (thisWeekStart, thisWeekEnd);
        }

        public static (DateTime today, DateTime endWeekDateFromToday) GetTwoWeeksDatesFromToday()
        {
            DateTime today = DateTime.Today;
            var thisWeekEnd = today.AddDays(15).AddSeconds(-1);
            return (today, thisWeekEnd);
        }

        public static (DateTime today, DateTime endWeekDateFromToday) GetPreviousTwoWeeksDatesFromToday()
        {
            DateTime today = DateTime.Today.AddDays(1).AddSeconds(-1);
            var thisWeekEnd = today.AddDays(-15).AddSeconds(2);
            return (thisWeekEnd, today);
        }

        public static (DateTime startDateOfMonth, DateTime EndDateOfMonth) GetStartEndDatesOfCurrentMonth()
        {
            DateTime date = DateTime.Today;
            var firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddSeconds(-1);
            return (firstDayOfMonth, lastDayOfMonth);
        }

#nullable enable
        /// <summary>
        /// Searches for property name 'Id' in list of complex objects
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="itemId"></param>
        /// <param name="indexToInsert"></param>
        /// <returns>The same List it was called on but reordered.</returns>
        public static IList<T> MoveAndUpdateList<T>(this IList<T> list, object itemId, int indexToInsert, string propertyName = "Id")
        {
            // var item = list.FirstOrDefault(i => i.GetType().GetProperty(propertyName).GetValue(i) == itemId);
            object? item = null;

            foreach (var entry in list)
            {
                var propertyType = entry?.GetType().GetProperty(propertyName)?.PropertyType;
                var castedValue = Convert.ChangeType(itemId, Type.GetType(propertyType?.FullName));
                item = entry.GetType().GetProperty(propertyName).GetValue(entry).Equals(castedValue) ? entry : item;
            }

            if (item != null)
            {
                list.Remove((T)item);
                list.Insert(indexToInsert, (T)item);
            }

            return list;
        }

        public static String GetHoursAndMinutesIn24HoursFormat(int hours, int minutes)
        {
            var hourString = hours.ToString();
            if (hours < 10)
            {
                hourString = "0" + hourString;
            }

            var minutesString = minutes.ToString();
            if (minutes < 10)
            {
                minutesString = "0" + minutesString;
            }

            return hourString + ":" + minutesString;
        }

        /// <summary>
        /// Converts opening and closing angled brackets and ampersands into their XML entities within the specified string
        /// </summary>
        /// <param name="TextToProcess"></param>
        /// <returns></returns>
        public static string ConvertAmpersandsAndAngledBracketsForXML(string TextToProcess)
            => TextToProcess.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;");

        ///<summary>
        ///Converts vbCrLf hard returns in a string into Arial paragraph breaks recognised by Word, using OpenXML markup
        ///</summary>
        ///<remarks>Important: this assumes that the text will appear INSIDE an existing paragraph,
        ///so it closes the current paragraph and starts a new one, and assumes that at
        ///the end of the text in question, a closing paragraph markup section will already be in place</remarks>
        public static string ConvertHardReturnsForInnerWordML(string TextToProcess, int PointSize = 10)
        {
            string PointSizeStr = (2 * PointSize).ToString(); // OpenXML seems to use double the point size for the font size attribute
            return TextToProcess.Replace(Environment.NewLine, "</w:t></w:r></w:p><w:p><w:r><w:rPr><w:rFonts w:ascii=\"Arial\" w:hAnsi=\"Arial\" w:cs=\"Arial\" /><w:sz w:val=\"" + PointSizeStr + "\" /><w:szCs w:val=\"" + PointSizeStr + "\" /></w:rPr><w:t>");
        }

        //<summary>
        //Given a string (typically the name of an org or a job order), this returns
        //a "safe" version with any characters modified which might cause a problem
        //when creating a Windows file/folder with this text as the file/folder name
        //</summary>
        public static string MakeStringSafeForFileSystemPath(string StringToProcess)
        {
            StringToProcess = StringToProcess.Replace(":", "-");
            StringToProcess = StringToProcess.Replace("<", "-");
            StringToProcess = StringToProcess.Replace(">", "-");
            StringToProcess = StringToProcess.Replace(":", "-");
            StringToProcess = StringToProcess.Replace("\"\"", "-");
            StringToProcess = StringToProcess.Replace("/", "-");
            StringToProcess = StringToProcess.Replace("\"", " - ");
            StringToProcess = StringToProcess.Replace("|", "-");
            StringToProcess = StringToProcess.Replace("?", "-");
            StringToProcess = StringToProcess.Replace("*", "-");
            StringToProcess = StringToProcess.Replace("~", "-");
            //StringToProcess = StringToProcess.Replace(vbTab, " ");
            // trim leading/trailing spaces; and stop anything being more than 259 chars
            return StringToProcess.Trim();
        }


        //<summary>
        // Copies a directory, optionally including all subfolders too
        //</summary>
        public static void CopyDirectory(string SourceDirName, string DestDirName,
                                         bool CopySubDirs, bool Overwrite = false)
        {
            DirectoryInfo dirToCopy = new DirectoryInfo(SourceDirName);
            string renamedFileName = "";

            //If the source directory does not exist, throw an exception
            if (dirToCopy.Exists == false)
            {
                throw new DirectoryNotFoundException("Source directory does not exist or could not be found: " + SourceDirName);
            }

            //If the destination directory does not exist, create it
            DirectoryInfo newlyCreatedDestinationDirectoryInfo = null;

            if (Directory.Exists(DestDirName) == false)
            {
                newlyCreatedDestinationDirectoryInfo = Directory.CreateDirectory(DestDirName);
            }
            else
            {
                newlyCreatedDestinationDirectoryInfo = new DirectoryInfo(DestDirName);
            }

            // Get the file contents of the directory to copy and copy all over
            FileInfo[] FilesToCopy = dirToCopy.GetFiles();

            foreach (FileInfo fileToCopy in FilesToCopy)
            {
                //Create the path to the new copy of the file
                string tempFilePath = Path.Combine(DestDirName, fileToCopy.Name);

                //check if the length of TempFilePath is 259 or more
                if (tempFilePath.Length < 259)
                {
                    // Copy the file
                    fileToCopy.CopyTo(tempFilePath, Overwrite); // by default don't overwrite existing files
                }
                else //shorten the file name if the path is too long
                {
                    string filePathWithoutFileName = DestDirName + @"\";
                    string fileNameWithoutExtension = fileToCopy.Name.Substring(0, fileToCopy.Name.LastIndexOf("."));

                    string fileExtension = fileToCopy.Name.Substring(fileToCopy.Name.LastIndexOf("."));
                    int idealLengthOfFileName = 258 - filePathWithoutFileName.Length - fileExtension.Length;

                    tempFilePath = filePathWithoutFileName + fileNameWithoutExtension.Substring(0, idealLengthOfFileName) + fileExtension;
                    renamedFileName = fileNameWithoutExtension.Substring(0, idealLengthOfFileName) + fileExtension;

                    fileToCopy.CopyTo(tempFilePath, Overwrite);

                    //try
                    //{
                    //    HttpContext.Current.Session("ShortenedFileNameString") &= String.Format("<tr><td>Source file</td><td>{0}</td><td>{1}</td></tr>", FileToCopy.Name, RenamedFileName)
                    //}
                    //catch(Exception ex)
                    //{
                    //    // fail silently - always crashing in web service as Sessions aren't support in Web service
                    //}
                }
            }

            //If necessary, recursively copy the subdirectories
            if (CopySubDirs == true)
            {
                DirectoryInfo[] SubDirs = dirToCopy.GetDirectories();
                foreach (DirectoryInfo subDir in SubDirs)
                {
                    //Create the subdirectory
                    string tempSubDirPath = Path.Combine(DestDirName, subDir.Name);

                    //Copy the subdirectories
                    CopyDirectory(subDir.FullName, tempSubDirPath, CopySubDirs, Overwrite);
                }
            }

            //update the date/time attributes of the parent directory to match the source directory
            try
            {
                newlyCreatedDestinationDirectoryInfo.CreationTime = dirToCopy.CreationTime;
                newlyCreatedDestinationDirectoryInfo.LastAccessTime = dirToCopy.LastAccessTime;
                newlyCreatedDestinationDirectoryInfo.LastWriteTime = dirToCopy.LastWriteTime;
            }
            catch (Exception ex)
            {
                // fail silently
            }

        }

        public static string GetAllPermittedFileExtensionForExternalWebsite()
        {
            return "dotm,.xlsm,.xml,.txt,.idml,.indd,.xlsm,.ttp,.pdf,.xlf,.docx,.xls,.xlsx,.json,.wsxz,.html,.csv,.png,.pptx,.jpg,.com,.inx,.zip,.sdlxliff,.xliff,.xlf,.3dm,.3ds,.3g2,.3ga,.3gp,.3gpp,.7z,.8bi,.aa,.aac,.aae,.aax,.accdb,.ace,.acsm,.act,.adt,.ai,.aif,.aifc,.aiff,.air,.amr,.amv,.ani,.apa,.ape,.api,.apk,.apnx,.app,.arf,.art,.arw,.asc,.asf,.asm,.asp,.aspx,.asx,.au,.aup,.avi,.awb,.azw,.azw3,.b,.bak,.bas,.bash_history,.bash_profile,.bashrc,.bat,.bet,.bfc,.bik,.bin,.bluej,.bmp,.bud,.bup,.bz2,.c,.cab,.caf,.caj,.cat,.cbl,.cbr,.cbt,.cbz,.cd,.cda,.cdr,.cdt,.ced,.cel,.cer,.cfg,.cfm,.cfml,.cgi,.chm,.class,.clp,.cma,.cmd,.cmf,.cod,.com,.coti,.cpi,.cpl,.cpp,.cr2,.crw,.crx,.crypt,.cs,.csk,.csr,.css,.csv,.cue,.cur,.cvs,.d,.dao,.dat,.dav,.db,.dbf,.dbx,.dcm,.dd,.dds,.deb,.def,.dem,.deskthemepack,.dev,.dic,.dif,.dir,.dit,.divx,.djvu,.dll,.dmg,.dmp,.dng,.do,.doc,.docm,.docx,.dot,.dotx,.drv,.ds,.dtd,.dtp,.dun,.dvd,.dvsd,.dwg,.dxf,.ebd,.efx,.emf,.eml,.emz,.epc,.eps,.epub,.erb,.esp3,.exe,.exr,.f4v,.fb2,.fcpevent,.fdxt,.ffl,.ffo,.fla,.flac,.flif,.flipchart,.flo,.flt,.flv,.fm3,.fnt,.fon,.Format,.fota,.fpx,.fsproj,.fxc,.gadget,.gam,.gbr,.gcw,.ged,.gho,.gid,.gif,.gms,.gpx,.grp,.gsm,.gz,.gzip,.h,.h264,.hdr,.heic,.hex,.hiv,.hlp,.hpp,.hqx,.ht,.htm,.html,.htt,.hwp,.i5z,.ibooks,.icl,.icm,.icns,.ico,.iconpackage,.ics,.idx,.iff,.ifo,.img,.imoviemobile,.indd,.inf,.ini,.ion,.ip,.ipa,.iptheme,.ise,.iso,.ithmb,.itl,.iwb,.jad,.jar,.java,.jp2,.jpeg,.jpg,.js,.json,.jsp,.kar,.kdc,.key,.keychain,.kml,.kmz,.koz,.kv,.lit,.lnk,.log,.lrf,.lua,.m,.m2ts,.m3u,.m3u8,.m4,.m4a,.m4p,.m4r,.m4v,.mac,.map,.max,.mbp,.md,.mdb,.mdf,.mdi,.mepx,.mht,.mid,.midi,.mim,.mix,.mkv,.mlc,.mmf,.mobi,.mod,.mov,.mp2,.mp3,.mp4,.mpa,.mpc,.mpeg,.mpg,.mpga,.msg,.msi,.mswmm,.mtb,.mts,.mtw,.mxf,.nb0,.nef,.nes,.nfa,.nfi,.nfo,.nfs,.nfv,.nib,.nrw,.nt,.numbers,.nzb,.o,.obj,.odg,.odm,.odp,.ods,.odt,.ogg,.ogv,.oma,.one,.opf,.opus,.orf,.ori,.otf,.owl,.oxps,.p,.p65,.pages,.part,.pas,.pb,.pbj,.pbxuser,.pcd,.pck,.pct,.pcx,.pd,.pdb,.pdf,.pds,.pef,.pes,.pgm,.php,.pict,.pif,.pika,.pkg,.pl,.plist,.plugin,.pmd,.pnf,.png,.pol,.pps,.ppsx,.ppt,.pptm,.pptx,.prc,.prf,.prop,.ps,.psd,.pspimage,.pub,.pwn,.py,.pyw,.qb2011,.qcp,.qpr,.qt,.quickendata,.qvm,.qxd,.qxp,.ra,.raf,.ram,.rar,.rc,.rcproject,.reg,.rels,.rem,.resources,.rm,.rmvb,.rom,.rpm,.rss,.rta,.rtf,.rwl,.s19,.sav,.sb,.sb2,.scr,.sdltm,.sdlppx,.sdlxliff,.sdlrpx,.sdf,.sfw,.sh,.shs,.sit,.sitx,.sln,.sma,.snb,.sql,.sr2,.srt,.suo,.svg,.swf,.swift,.sxw,.sys,.t65,.tar,.tar.gz,.tax2012,.tax2014,.tax2016,.tcr,.tec,.tex,.tga,.tgz,.thm,.thp,.tif,.tiff,.tmp,.toast,.torrent,.trx,.ts,.ttf,.tvs,.txt,.url,.uue,.vb,.vbk,.vbp,.vbproj,.vbx,.vc,.vcd,.vcf,.vcs,.vcxproj,.veg,.vep,.vmg,.vnt,.vob,.vpj,.vproj,.vsd,.wav,.wbmp,.webloc,.webm,.webp,.wk3,.wks,.wlmp,.wma,.wmf,.wmv,.wp5,.wpd,.wpg,.wps,.wsf,.xap,.xcf,.xcodeproj,.xesc,.xfdl,.xhtml,.Xib,.xll,.xlr,.xls,.xlsb,.xlsx,.xmind,.xps,.xq,.xspf,.xt,.yml,.yuv,.zip,.zipx,.mif,.fm,.tmx,.tbulic18,.tbulic15,.tbulic16,.book,.tsv,.vsdx";
        }

        public static string CreateSaltForPasswordHashing()
        {
            //Generate the random number using the cryptographic service provider
            RNGCryptoServiceProvider RandomNumberGenerator = new RNGCryptoServiceProvider();
            Byte[] SaltByteArray = new Byte[16];
            RandomNumberGenerator.GetBytes(SaltByteArray);


            //Return a Base64 string representation of the random number
            return Convert.ToBase64String(SaltByteArray);
        }
        public static DateTime AddWorkingDaysToDate(DateTime DateToAddTo, int NumberOfWorkingDaysToAdd)
        {

            // Robert adapted this from:
            // http://www.codeproject.com/Articles/43364/Adding-Business-Days-to-a-Date

            // Knock the start date down one day if it is on a weekend.
            if (DateToAddTo.DayOfWeek == DayOfWeek.Saturday | DateToAddTo.DayOfWeek == DayOfWeek.Sunday)
                NumberOfWorkingDaysToAdd -= 1;

            for (int i = 1; i <= NumberOfWorkingDaysToAdd; i++)
            {
                switch (DateToAddTo.DayOfWeek)
                {
                    case DayOfWeek.Saturday:
                        {
                            DateToAddTo = DateToAddTo.AddDays(3);
                            break;
                        }

                    case DayOfWeek.Sunday:
                        {
                            DateToAddTo = DateToAddTo.AddDays(2);
                            break;
                        }

                    case DayOfWeek.Monday:
                    case DayOfWeek.Tuesday:
                    case DayOfWeek.Wednesday:
                    case DayOfWeek.Thursday:
                    case DayOfWeek.Friday:
                        {
                            DateToAddTo = DateToAddTo.AddDays(1);
                            break;
                        }
                }
            }

            // Check to see if the end date is on a weekend, and 
            // if so, move it ahead to Monday

            if (DateToAddTo.DayOfWeek == DayOfWeek.Saturday)
                DateToAddTo = DateToAddTo.AddDays(2);
            else if (DateToAddTo.DayOfWeek == DayOfWeek.Sunday)
                DateToAddTo = DateToAddTo.AddDays(1);

            return DateToAddTo;
        }


    }
}
