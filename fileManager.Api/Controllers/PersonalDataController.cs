using fileManager.Api.Dtos;
using fileManager.Api.Dtos.PersonalData;
using fileManager.Api.Helper;
using fileManager.Api.IServices;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace fileManager.Api.Controllers
{
    public class PersonalDataController : BaseApiController
    {
        private readonly IPersonalDataService _service;
        public PersonalDataController(IPersonalDataService service)
        {
            _service = service;
        }

        [Produces("application/json")]
        [ProducesResponseType(typeof(ResponseDto<PaginationDto<PersonalDataGet>>), (int)HttpStatusCode.OK)]
        [HttpGet]
        public async Task<ActionResult> GetAll([FromQuery] PaginationQuery paginationQuery, [FromQuery] PersonalDataFilter filter)
        {
            var response = await _service.GetAll(paginationQuery, filter);
            return Ok(response);
        }

        [Produces("application/json")]
        [ProducesResponseType(typeof(ResponseDto<PersonalDataGet>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ResponseDto<string>), (int)HttpStatusCode.BadRequest)]
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] PersonalDataAdd form)
        {
            var response = await _service.Add(form);
            return response.Error
                ? BadRequest(response)
                : Ok(response);
        }
    }
}
