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
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _repository;
        private readonly ILoggerService _loggerService;
        private readonly IMapper _mapper;

        public ProductController(IProductRepository repository, ILoggerService loggerService, IMapper mapper)
        {
            _repository = repository;
            _loggerService = loggerService;
            _mapper = mapper;
        }

        /// <summary>
        /// Get all products
        /// </summary>
        /// <returns>Product List</returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetObjectsListAsync()
        {
            try
            {
                var objectList = await _repository.GetAllAsync();
                var response = _mapper.Map<IList<ProductDTO>>(objectList);

                _loggerService.LogInfo($"The operation <get product list> was successful.");

                return Ok(response);
            }
            catch (Exception ex)
            {
                _loggerService.LogError($"Error. The operation <get product list> failed. Info: {ex.Message}");
                return StatusCode(500, "Server error.");
            }
        }

        /// <summary>
        /// Get product by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Get product by Id</returns>
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetObjectByIdAsync(int id)
        {
            try
            {
                if(id != 0)
                {
                    var obj = await _repository.GetByIdAsync(id);

                    if (obj == null)
                    {
                        _loggerService.LogWarn($"Product with Id: {id} was not found.");
                        return NotFound();
                    }

                    var response = _mapper.Map<ProductDTO>(obj);

                    _loggerService.LogInfo($"The operation <get product by id: {obj.Id}> was successful.");

                    return Ok(response);
                }

                _loggerService.LogInfo($"The operation <get product by Id> was failed. Object not found.");

                return NotFound();
            }
            catch (Exception ex)
            {
                _loggerService.LogError($"Error. The operation <get product by Id> failed. Info: {ex.Message}");
                return StatusCode(500, "Server error.");
            }
        }

        /// <summary>
        /// Create product
        /// </summary>
        ///  <param name="modelDTO"></param>
        /// <returns>Create product</returns>
        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] ProductDTO modelDTO)
        {
            try
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

                var obj = _mapper.Map<Product>(modelDTO);

                var response = await _repository.CreateAsync(obj);

                if (!response)
                {
                    _loggerService.LogError($"Product creation failed.");
                    return StatusCode(500, "Server error.");
                }

                _loggerService.LogInfo($"The operation <create product. Id: {modelDTO.Id}> was successful.");

                return Created("Create", new { response });
            }
            catch (Exception ex)
            {
                _loggerService.LogError($"Error. The operation <create product> failed. Info: {ex.Message}");
                return StatusCode(500, "Server error.");
            }
        }

        /// <summary>
        /// Update product
        /// </summary>
        /// <param name="id"></param>
        /// <param name="modelDTO"></param>
        /// <returns>update product</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] ProductDTO modelDTO)
        {
            try
            {
                if(id == modelDTO.Id)
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

                    var obj = _mapper.Map<Product>(modelDTO);

                    var response = await _repository.UpdateAsync(obj);

                    _loggerService.LogInfo($"The operation <update product. Id: {id}> was successful.");

                    return Ok(response);
                }

                _loggerService.LogWarn($"The operation <update product> failed. Object not found.");

                return NotFound();
            }
            catch (Exception ex)
            {
                _loggerService.LogError($"Error. The operation <update product> failed. Info: {ex.Message}");
                return StatusCode(500, "Server error.");
            }
        }

        //otedl_mn Monitoring18

        /// <summary>
        /// Delete product
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Delete product</returns>
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

                if(obj == null)
                {
                    _loggerService.LogWarn($"The operation <delete product> failed. Object not found.");
                    return NotFound();
                }

                await _repository.DeleteAsync(obj);

                _loggerService.LogInfo($"The operation <delete product. Id: {id}> was successful.");

                return NoContent();
            }
            catch (Exception ex)
            {
                _loggerService.LogError($"Error. The operation <delete product> failed. Info: {ex.Message}");
                return StatusCode(500, "Server error.");
            }
        }
    }
}
