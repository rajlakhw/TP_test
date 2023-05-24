using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Data;
using Services.Interfaces;
using Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SmartAdmin.WebUI.Controllers
{
    public class TPEndClientController : Controller
    {
        //public IActionResult Index()
        //{
        //    return View();
        //}

        private readonly int[] EmployeesIDsToAccess = new int[] { 1345, 1337, 475, 447, 185, 393, 1191, 1245, 1358, 31, 706, 760, 1229, 1269, 1298, 1305, 1308, 1324, 1352 };

        private IConfiguration Configuration;


        private ITPEmployeesService tpservice;

        private ITPEndClient tpendclientservice;


        public TPEndClientController(IConfiguration _configuration, ITPEmployeesService _empservice, ITPEndClient _tpendclientservice)
        {
            Configuration = _configuration;
            tpservice = _empservice;
            tpendclientservice = _tpendclientservice;
        }

        public async Task<IActionResult> Insert(string TextBoxValue, string ItemType, int endclientID)
        {
            string username = HttpContext.User.Identity.Name;
            username = GeneralUtils.GetUsernameFromNetwokUsername(username);
            //int EmployeeCurrentlyLoggedIn = await tpservice.IdentifyCurrentUserID<int>(LogonUserName: username);
            Employee EmployeeCurrentlyLoggedIn = await tpservice.IdentifyCurrentUser<Employee>(LogonUserName: username);

            string returnMessage = "";
            if (Int32.Parse(ItemType) == 1)
            {
                returnMessage = "NoItem";
                return RedirectToAction("TPConfig", new { showText = returnMessage });
            }
            else if (int.Parse(ItemType) == 2)
            {
                if (TextBoxValue == null)
                {
                    returnMessage = "NoName";
                    return RedirectToAction("TPConfig", new { showText = returnMessage });
                }
            }
            else
            {
                if (Int32.Parse(ItemType) == 18 || Int32.Parse(ItemType) == 19 || Int32.Parse(ItemType) == 20)
                {
                    if (endclientID == 0)
                    {
                        returnMessage = "NoEndClient";
                        return RedirectToAction("TPConfig", new { showText = returnMessage });
                    }
                    else
                    {
                        if (TextBoxValue == null)
                        {
                            returnMessage = "NoName";
                            return RedirectToAction("TPConfig", new { showText = returnMessage });
                        }
                    }
                }

                if (endclientID == 0)
                {
                    var result = await tpendclientservice.GetEndClientDetails<EndClient>(TextBoxValue);
                    if (result == null)
                    {
                        await tpendclientservice.InsertNewEndClient(TextBoxValue, EmployeeCurrentlyLoggedIn.Id);
                        returnMessage = "insertednewEC";
                    }
                    else
                    {
                        returnMessage = "alreadyExistsEC";
                    }

                }
                else
                {
                    var result = await tpendclientservice.GetEndClientDataDetails<EndClient>(endclientID, TextBoxValue, Int32.Parse(ItemType));
                    if (result == null)
                    {
                        await tpendclientservice.InsertNewEndClientData(TextBoxValue, endclientID, Int32.Parse(ItemType), EmployeeCurrentlyLoggedIn.Id);
                        returnMessage = "insertednewEC";
                    }
                    else
                    {
                        returnMessage = "alreadyExistsEC";
                    }


                }
            }
            //await tpendclientservice.InsertNewEndClient(TextBoxValue, EmployeeCurrentlyLoggedInID);


            return RedirectToAction("TPConfig", new { showText = returnMessage });

        }
        public async Task<IActionResult> TPConfig(string showText)
        {

            string username = HttpContext.User.Identity.Name;
            username = GeneralUtils.GetUsernameFromNetwokUsername(username);
            //int EmployeeCurrentlyLoggedIn = await tpservice.IdentifyCurrentUserID<int>(LogonUserName: username);
            Employee EmployeeCurrentlyLoggedIn = await tpservice.IdentifyCurrentUser<Employee>(LogonUserName: username);
            //Employee EmployeeCurrentlyLoggedIn = await tpservice.IdentifyCurrentUserById(185); // Arian

            if (EmployeeCurrentlyLoggedIn.AccessLevel == 3 || EmployeeCurrentlyLoggedIn.AccessLevel == 4 || EmployeesIDsToAccess.Contains(EmployeeCurrentlyLoggedIn.Id))
            {
                var model = new ViewModels.TPEndClient.TPEndClientList();
                if (showText == "insertednewEC")
                {
                    model.JavascriptToRun = "ShowSuccessPopup()";
                }
                else if (showText == "alreadyExistsEC")
                {
                    model.JavascriptToRun = "ShowErrorPopup()";
                }
                else if (showText == "NoItem")
                {
                    model.JavascriptToRun = "ShowNoItemPopup()";
                }
                else if (showText == "NoEndClient")
                {
                    model.JavascriptToRun = "ShowNoEndClient()";
                }
                else if (showText == "NoName")
                {
                    model.JavascriptToRun = "ShowNoTextPopup()";
                }


                var EndClientListNames = await tpendclientservice.GetAllEndClients();
                model.EndClientList = new SelectList(EndClientListNames, "ID", "Name");
                model.EndClientNames = EndClientListNames;

                return View(model);
            }
            else
            {
                return Redirect("/Page/Locked");
            }




        }
    }
}
