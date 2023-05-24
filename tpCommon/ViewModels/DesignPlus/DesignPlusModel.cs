using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data;
using Global_Settings;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace ViewModels.DesignPlus
{
    public class DesignPlusModel
    {
        private readonly IConfiguration configuration;
        private readonly GlobalVariables GlobalVars;

        public DesignPlusModel(IConfiguration configuration1)
        {
            this.configuration = configuration1;
            GlobalVars = new GlobalVariables();
            configuration1.GetSection(GlobalVariables.GlobalVars).Bind(GlobalVars);
        }
        public ClientDesignFile DesignPlusFile { get; set; }

        public string DesignPlusFolder
        {
            get
            {
                string DirPath = Path.Combine(GlobalVars.NewDesignPlusExternalFolderPath, DesignPlusFile.UploadedByClientId.ToString(),
                                             DesignPlusFile.Id.ToString());
                return DirPath;
            }

        }

        public string DesignPlusFileWithoutExtension
        {
            get
            {
                return DesignPlusFile.DocumentName.Replace(".indd", "").Replace(".idml", "");
            }

        }
        public string DesignPlusSourceFolder
        {
            get
            {
                string DirPath = Path.Combine(DesignPlusFolder, "1 Source Folder");
                return DirPath;
            }

        }

        public string DesignPlusImageFolder
        {
            get
            {
                string DirPath = Path.Combine(DesignPlusFolder, "3 Image files");
                return DirPath;
            }

        }

    }
}
