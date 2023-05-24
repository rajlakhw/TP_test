using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Services;
using Services.Interfaces;
using ViewModels.DesignPlus;
using System.IO;
using IDServerRef;


namespace flowPlusExternal.Controllers
{
    public class designPlusController : Controller
    {
        private readonly ITPDesignPlusService designPlusService;

        public designPlusController(ITPDesignPlusService _designPlusService)
        {
            this.designPlusService = _designPlusService;
        }

        [HttpGet]
        [Route("[controller]/[action]/{designPlusFileId}")]
        public async Task<IActionResult> DesignPlusEditing(int designPlusFileId)
        {
            var thisDesignPlusFile = await designPlusService.GetDesignPlusFileDetails(designPlusFileId);

            string FirstImageFilePath = Path.Combine(thisDesignPlusFile.DesignPlusImageFolder, thisDesignPlusFile.DesignPlusFileWithoutExtension + ".png");

            byte[] imgBytes = System.IO.File.ReadAllBytes(FirstImageFilePath);
            string base64String = Convert.ToBase64String(imgBytes);

            var base64 = Convert.ToBase64String(imgBytes);
            var dataUri = "data:image/png" + ";base64, ";
            string imgSrc = dataUri + base64String;


            DesignPlusViewModel designPlusView = new DesignPlusViewModel()
            {

                FirstPageImageByte64String = imgSrc,
                FirstPageImageMapTextFilePath = Path.Combine(thisDesignPlusFile.DesignPlusImageFolder, thisDesignPlusFile.DesignPlusFileWithoutExtension + "1.txt")

            };
            return View(designPlusView);
        }

        //[HttpPost]
        //[Route("[controller]/[action]")]
        //public async Task<IActionResult> GetTextFromTextBox()
        //{
        //    IDSPScriptArg[] ScriptArgs = new IDSPScriptArg[1];

        //    ScriptArgs[0] = new IDSPScriptArg();
        //    ScriptArgs[0].name = "sourceFile";
        //    ScriptArgs[0].value = "\\\\10.196.48.130\\ExtranetBase\\design plus\\Contacts\\114354\\27593\\4 Delivery file\\InDesign 2023 file.indd";


        //    //RUN SCRIPT PARAMETERS
        //    //build the RunScript parameters
        //    RunScriptParameters ParamsToRun = new RunScriptParameters();
        //    ParamsToRun.scriptArgs = ScriptArgs;
        //    string MyScriptFile = "\\\\fredcpfspdm0005\\ExtranetBase\\design plus\\Adobe InDesign CC Server TP scripts\\TPDesignPlusScriptsLIVE\\testScript.js";
        //    StreamReader Reader = System.IO.File.OpenText(MyScriptFile);
        //    string ScriptToRun = Reader.ReadToEnd();
        //    Reader.Close();

        //    ParamsToRun.scriptText = ScriptToRun;
        //    ParamsToRun.scriptLanguage = "javascript";


        //    // run the script on the server 
        //    string errorString = "";
        //    IDServerRef.Data results = new IDServerRef.Data();
        //    IDServerRef.ServicePortTypeClient InDesignProxy = new IDServerRef.ServicePortTypeClient();


        //    RunScriptRequest thisRunRequest = new RunScriptRequest(1, ParamsToRun);
        //    //InDesignProxy.Url = "http://fredcpappdm0001:12345";
        //    //        //RunScript runs the javascript command to convert the InDesign file to SWF file
        //    //        Dim TimeBeforeScript As DateTime = TPTimeZonesLogic.GetCurrentGMT
        //    RunScriptResponse runScriptResponse = new RunScriptResponse();
        //    DateTime TimeBeforeScript = DateTime.Now;
        //    int errNum = 0;
        //    runScriptResponse = await InDesignProxy.RunScriptAsync(thisRunRequest);

        //    TimeSpan SecondsTaken = DateTime.Now - TimeBeforeScript;
        //    errNum = runScriptResponse.errorNumber;
        //    errorString = runScriptResponse.errorString;

        //    results = runScriptResponse.scriptResult;

        //    string textFrameContents = results.data.ToString();
        //    return Ok(textFrameContents);
        //}


        [HttpPost]
        [Route("[controller]/[action]")]
        public async Task<IActionResult> GetTextFromTextBox()
        {
            //var test = System.Runtime.InteropServices.Marshal.BindToMoniker("configuration_12345");


            //var myApp = (InDesignServer.Application)COMc

            //var myApp = new InDesignServer.Application();

            var myApp = (InDesignServer.Application)System.Runtime.InteropServices.Marshal.BindToMoniker("configuration_12345");
            object myScript = "\\\\fredcpfspdm0005\\ExtranetBase\\design plus\\Adobe InDesign CC Server TP scripts\\TPDesignPlusScriptsLIVE\\testScript.js";
            var result = myApp.DoScript(myScript, InDesignServer.idScriptLanguage.idJavascript);

            //string textFrameContents = results.data.ToString();
            //return Ok(textFrameContents);
            return Ok();
        }
    }
}
