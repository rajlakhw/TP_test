using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using ViewModels.Organisation;

namespace SmartAdmin.WebUI.EndPoints
{
    public class ComponentsController : Controller
    {
        private readonly ITPJobOrderService jobOrderService;
        private readonly ITPOrgGroupsLogic groupService;
        public ComponentsController(ITPJobOrderService jobOrderService, ITPOrgGroupsLogic groupService)
        {
            this.jobOrderService = jobOrderService;
            this.groupService = groupService;
        }

        [HttpPost("api/[controller]/[action]")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllJobOrdersDataTableComponentData([FromBody] DataTables dataParams)
        {
            var data = await jobOrderService.GetAllJobOrdersForDataObjectAndType(dataParams.dataObjectId, dataParams.dataTypeId, dataParams.parameters.start, dataParams.parameters.length, dataParams.parameters.search.value, dataParams.parameters.order.FirstOrDefault().column, dataParams.parameters.order.FirstOrDefault().dir);

            int totalRecords;
            int filteredRecords;
            (totalRecords, filteredRecords) = await jobOrderService.GetAllJobOrdersCountForDataObjectAndType(dataParams.dataObjectId, dataParams.dataTypeId, dataParams.parameters.search.value);

            return Ok(new { data, recordsTotal = totalRecords, recordsFiltered = filteredRecords });
        }

        [HttpPost("api/[controller]/[action]")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllEnquiriesDataTableComponentData([FromBody] DataTables dataParams)
        {
            var data = await groupService.GetEnquiriesbOrdersForDataObjectAndType(dataParams.dataObjectId, dataParams.dataTypeId, dataParams.parameters.start, dataParams.parameters.length, dataParams.parameters.search.value, dataParams.parameters.order.FirstOrDefault().column, dataParams.parameters.order.FirstOrDefault().dir);

            int totalRecords;
            int filteredRecords;
            (totalRecords, filteredRecords) = await groupService.GetAllEnquiriesCountForDataObjectAndType(dataParams.dataObjectId, dataParams.dataTypeId, dataParams.parameters.search.value);

            return Ok(new { data, recordsTotal = totalRecords, recordsFiltered = filteredRecords });
        }
    }
}
