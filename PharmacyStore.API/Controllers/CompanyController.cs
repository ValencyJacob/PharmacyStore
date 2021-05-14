using AutoMapper;
using BusinessLogic.Repository.IRepository;
using Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.DTOs;
using PharmacyStore.API.Services.IServices;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PharmacyStore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly ICompanyRepository _repository;
        private readonly ILoggerService _loggerService;
        private readonly IMapper _mapper;

        public CompanyController(ICompanyRepository repository, ILoggerService loggerService, IMapper mapper)
        {
            _repository = repository;
            _loggerService = loggerService;
            _mapper = mapper;
        }

        /// <summary>
        /// Get all companies.
        /// </summary>
        /// <returns>Companie list</returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetObjectsListAsync()
        {
            try
            {
                var objList = await _repository.GetAllAsync();
                var response = _mapper.Map<IList<CompanyDTO>>(objList);

                _loggerService.LogInfo($"The operation <get company list> was successful.");

                return Ok(response);
            }
            catch (Exception ex)
            {
                _loggerService.LogError($"Error. The operation <get company list> failed. Info: {ex.Message}");
                return StatusCode(500, "Server error.");
            }
        }

        /// <summary>
        /// Get company by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Company</returns>
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetObjectByIdAsync(int id)
        {
            try
            {
                var obj = await _repository.GetByIdAsync(id);

                if(obj == null)
                {
                    _loggerService.LogWarn($"Company with Id: {id} was not found.");
                    return NotFound();
                }

                var response = _mapper.Map<CompanyDTO>(obj);

                _loggerService.LogInfo($"The operation <get company by id: {obj.Id}> was successful.");

                return Ok(response);
            }
            catch(Exception ex)
            {
                _loggerService.LogError($"Error. The operation <get company by id: {id}> failed. Info: {ex.Message}");
                return StatusCode(500, "Server error.");
            }
        }

        /// <summary>
        /// Create company.
        /// </summary>
        /// <param name="modelDTO"></param>
        /// <returns>Add company</returns>
        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] CompanyDTO modelDTO)
        {
            try
            {
                if(modelDTO == null)
                {
                    _loggerService.LogWarn($"Empty request was submitted");
                    return BadRequest(ModelState);
                }

                if (!ModelState.IsValid)
                {
                    _loggerService.LogWarn($"Empty request was submitted");
                    return BadRequest(ModelState);
                }

                var obj = _mapper.Map<Company>(modelDTO);

                var response = await _repository.CreateAsync(obj);

                if (!response)
                {
                    _loggerService.LogError($"Company creation failed.");
                    return StatusCode(500, "Server error.");
                }

                _loggerService.LogInfo($"The operation <create company> was successful.");

                return Created("Create", new { response });
            }
            catch(Exception ex)
            {
                _loggerService.LogError($"Error. The operation <create company> failed. Info: {ex.Message}");
                return StatusCode(500, "Server error.");
            }
        }

        /// <summary>
        /// Update Company
        /// </summary>
        /// <param name="modelDTO"></param>
        /// <param name="id"></param>
        /// <returns>Update Company</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] CompanyDTO modelDTO)
        {
            try
            {
                if (id == modelDTO.Id)
                {
                    if (modelDTO == null)
                    {
                        _loggerService.LogWarn($"Empty request was submitted");
                        return BadRequest(ModelState);
                    }

                    if (!ModelState.IsValid)
                    {
                        _loggerService.LogWarn($"Empty request was submitted");
                        return BadRequest(ModelState);
                    }

                    var obj = _mapper.Map<Company>(modelDTO);

                    var response = await _repository.UpdateAsync(obj);

                    _loggerService.LogInfo($"The operation <update company> was successful.");

                    return Ok(response);
                }

                _loggerService.LogError($"Company not found to update");
                return NotFound();
            }
            catch (Exception ex)
            {
                _loggerService.LogError($"Error. The operation <update company> failed. Info: {ex.Message}");
                return StatusCode(500, "Server error.");
            }
        }

        /// <summary>
        /// Delete the company by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Delete the company by Id</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            try
            {
                if(id == 0)
                {
                    return BadRequest();
                }

                var obj = await _repository.GetByIdAsync(id);

                if(obj != null)
                {
                    await _repository.DeleteAsync(obj);

                    _loggerService.LogInfo($"The operation <delete company> was successful.");

                    return NoContent();
                }

                _loggerService.LogWarn($"The operation <delete company> failed. Object not found.");

                return NotFound();
            }
            catch(Exception ex)
            {
                _loggerService.LogError($"Error. The operation <delete company> failed. Info: {ex.Message}");
                return StatusCode(500, "Server error.");
            }
        }
    }
}
