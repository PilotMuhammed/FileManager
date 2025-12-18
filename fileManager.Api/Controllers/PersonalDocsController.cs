using fileManager.Api.Dtos;
using fileManager.Api.Dtos.PersonalDocuments;
using fileManager.Api.Helper;
using fileManager.Api.IServices;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace fileManager.Api.Controllers
{
    public class PersonalDocsController : BaseApiController
    {
        private readonly IPersonalDocsService _service;
        public PersonalDocsController(IPersonalDocsService service)
        {
            _service = service;
        }

        [Produces("application/json")]
        [ProducesResponseType(typeof(ResponseDto<PaginationDto<PersonalDocsDTO>>), (int)HttpStatusCode.OK)]
        [HttpGet]
        public async Task<ActionResult> GetAll([FromQuery] PaginationQuery paginationQuery, [FromQuery] FilterPersonalDocs filter)
        {
            var response = await _service.GetAll(paginationQuery, filter);
            return Ok(response);
        }

        [Produces("application/json")]
        [ProducesResponseType(typeof(ResponseDto<List<PersonalDocsDTO>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ResponseDto<string>), (int)HttpStatusCode.BadRequest)]
        [HttpPost("{id}/upload")]
        public async Task<ActionResult> Post([FromForm] AddPersonalDocsWithFileDTO form)
        {
            var response = await _service.Add(form);
            return response.Error
                ? BadRequest(response)
                : Ok(response);
        }

        [Produces("application/json")]
        [ProducesResponseType(typeof(ResponseDto<PersonalDocsDTO>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ResponseDto<string>), (int)HttpStatusCode.BadRequest)]
        [HttpGet("user/{personalDataId}")]
        public async Task<ActionResult> GetByUser([FromRoute] int personalDataId)
        {
            var response = await _service.GetByUser(personalDataId);
            return Ok(response);
        }


        [Produces("application/json")]
        [ProducesResponseType(typeof(ResponseDto<string>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ResponseDto<string>), (int)HttpStatusCode.BadRequest)]
        [HttpDelete("{userId}/files/{fileId}")]
        public async Task<IActionResult> DeleteUserFile([FromRoute] int userId, [FromRoute] int fileId)
        {
            var response = await _service.DeleteUserFile(userId, fileId);
            if (response.Error)
                return BadRequest(response);
            return Ok(response);
        }
    }
}
