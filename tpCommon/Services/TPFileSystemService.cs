using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Data;
using Data.Repositories;
using Global_Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Services.Interfaces;
using ViewModels.FileSystem;

namespace Services
{
    public class TPFileSystemService : ITPFileSystemService
    {
        private readonly IRepository<SharedDoc> repository;
        private readonly IConfiguration configuration;

        public TPFileSystemService(IRepository<SharedDoc> repository, IConfiguration configuration)
        {
            this.repository = repository;
            this.configuration = configuration;
        }

        public async Task<IEnumerable<DownloadbleFile>> GetDownloadableDirectoryContents(string absolutePath, int dataObjectId, int dataTypeId, int sortOrder = 0)
        {
            var sharedDocsList = new List<DownloadbleFile>();
            // skip whole operation if the supplied directory does not exist
            // (do not display an error about this for security reasons, just
            // return an empty collection)
            if (Directory.Exists(absolutePath))
            {
                var DirToBrowse = new DirectoryInfo(absolutePath);
                foreach (var fileToList in DirToBrowse.GetFiles())
                {
                    // filter out our description metafiles
                    if (fileToList.Extension != ".tpfiledesc")
                    {
                        // see if there is a metafile with a description
                        var FileDescription = "";
                        if (File.Exists(fileToList.FullName + ".tpfiledesc"))
                        {
                            FileDescription = File.ReadAllText(fileToList.FullName + ".tpfiledesc", System.Text.Encoding.UTF8);
                        }
                        sharedDocsList.Add(new DownloadbleFile()
                        {
                            FileName = fileToList.Name,
                            LastModifiedGMTDateTime = fileToList.LastWriteTime,
                            AbsoluteSystemFilePath = fileToList.FullName,
                            FileSizeBytes = fileToList.Length,
                            TranslatePlusDescription = FileDescription
                        });
                    }
                }

                var databaseDocs = await repository.All().Where(x => sharedDocsList.Select(x => x.FileName).Contains(x.FileName)
                    && x.DataObjectId == dataObjectId && x.DataObjectTypeId == dataTypeId).ToListAsync();

                foreach (var dbDoc in databaseDocs)
                {
                    var downloadebleDoc = sharedDocsList.FirstOrDefault(x => x.FileName == dbDoc.FileName);
                    if(downloadebleDoc != null)
                        downloadebleDoc.FileName = dbDoc.ClientFileName;
                }
            }
            // sort based on any specific criteria

            return sharedDocsList;
        }




        ///<summary>
        ///  Path to the "Key Client Info" network directory (as expressed in a format accessible to the current
        ///  application) where important documentation about this org is stored
        ///</summary>
        ///<returns>An empty string if the folder cannot be located</returns>
        ///<remarks>If no orders have yet been created for this org, then there will most probably
        ///not be a "Key Client Info" folder yet, in which case this will return an empty string</remarks>
        public string GetNetworkKeyClientInfoDirectoryPath(string jobServerLocation, int orgId)
        {
            // in case of any file system errors, return an empty string
            // if key network job folders cannot be found
            try
            {
                // find the first matching directory within the org folder
                // which starts with the order ID, regardless of what comes after it
                string OrgDirSearchPattern = orgId.ToString() + "*";
                string OrgDirPath = string.Empty;
                var DirInfo = new DirectoryInfo(jobServerLocation);
                // find org folder first (key client info folder should then appear within that)
                var MatchingOrgDirs = DirInfo.GetDirectories(OrgDirSearchPattern, SearchOption.TopDirectoryOnly);

                if (MatchingOrgDirs.Count() == 0)
                {
                    string newOrgDirSearchPattern = orgId.ToString();
                    var newMatchingOrgDirs = DirInfo.GetDirectories(newOrgDirSearchPattern, SearchOption.TopDirectoryOnly);
                    if (newMatchingOrgDirs.Count() == 0)
                        return "";
                    else
                        OrgDirPath = newMatchingOrgDirs[0].FullName;
                }
                // no org folder found, so don't bother searching further
                else
                    OrgDirPath = MatchingOrgDirs[0].FullName;

                // now look for the key client info folder within the org folder
                string ExpectedKeyClientInfoPath = Path.Combine(OrgDirPath, "Key Client Info");

                if (Directory.Exists(ExpectedKeyClientInfoPath))
                    return ExpectedKeyClientInfoPath;
                else
                    return "";
            }
            catch (System.Exception ex)
            {
                return "";
            }
        }
        public string UnMapTPNetworkPath(string pathToUnMap)
        {
            
            try
            {
                var config = new GlobalVariables();
                configuration.GetSection(GlobalVariables.GlobalVars).Bind(config);

                string UnmappedPath = pathToUnMap;
                UnmappedPath.Replace("J:", config.LondonJobDriveBaseDirectoryPathForApp);     
                UnmappedPath.Replace("K:", config.SofiaJobDriveBaseDirectoryPathForApp);
                UnmappedPath.Replace("Q:", config.InternalJobDriveBaseDirectoryPathForApp);      
                UnmappedPath.Replace("S:", @"\\fredcpfspdm0003.global.publicisgroupe.net\SharedInfo");            
                UnmappedPath.Replace("L:", config.ParisJobDriveBaseDirectoryPathForApp);
                return UnmappedPath;
            }
            catch (System.Exception ex)
            {
                return "";
            }
        }
        ///<summary>
        ///  Path to the Enquiry network directory (as expressed in a format accessible to the current
        ///  application)
        ///</summary>
        ///<returns>/remarks>
        public string GetEnquiryDirectoryPath(string quoteServerLocation, int orgId, int enquiryId)
        {
            // in case of any file system errors, return an empty string
            // if key network job folders cannot be found
            try
            {
                // find the first matching directory within the org folder
                // which starts with the order ID, regardless of what comes after it
                string OrgDirSearchPattern = orgId.ToString() + "*";
                string OrgDirPath = string.Empty;
                var DirInfo = new DirectoryInfo(quoteServerLocation);
                // find org folder first (enquiryfolder should then appear within that)
                var MatchingOrgDirs = DirInfo.GetDirectories(OrgDirSearchPattern, SearchOption.TopDirectoryOnly);

                if (MatchingOrgDirs.Count() == 0)
                {
                    string newOrgDirSearchPattern = orgId.ToString();
                    var newMatchingOrgDirs = DirInfo.GetDirectories(newOrgDirSearchPattern, SearchOption.TopDirectoryOnly);
                    if (newMatchingOrgDirs.Count() == 0)
                        return "";
                    else
                        OrgDirPath = newMatchingOrgDirs[0].FullName;
                }
                // no org folder found, so don't bother searching further
                else
                    OrgDirPath = MatchingOrgDirs[0].FullName;


                string EnqDirSearchPattern = enquiryId.ToString() + "*";
                string EnqDirPath = string.Empty;
                var OrgDirInfo = new DirectoryInfo(OrgDirPath);
                var MatchingEnqDirs = OrgDirInfo.GetDirectories(EnqDirSearchPattern, SearchOption.TopDirectoryOnly);

                if (MatchingEnqDirs.Count() == 0)
                {
                    string newEnqDirSearchPattern = enquiryId.ToString();
                    var newMatchingEnqDirs = OrgDirInfo.GetDirectories(newEnqDirSearchPattern, SearchOption.TopDirectoryOnly);
                    if (newMatchingEnqDirs.Count() == 0)
                        return "";
                    else
                        EnqDirPath = newMatchingEnqDirs[0].FullName;
                }
                // no org folder found, so don't bother searching further
                else
                    EnqDirPath = MatchingEnqDirs[0].FullName;


                if (Directory.Exists(EnqDirPath))
                    return EnqDirPath;
                else
                    return "";
            }
            catch (System.Exception ex)
            {
                return "";
            }
        }
    }
}
