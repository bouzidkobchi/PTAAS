﻿using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Data;
using WebApi.Data;
using WebApi.DTOs;
using WebApi.Repositories;

namespace WebApi.Controllers
{
    [Tags("pentesting methodologies")]
    [Route("api/methodologies")]
    public class PentestingMethodologyController : ControllerBase
    {
        private readonly PentestingMethodologyRepository _PentestingMethodologyRepository;

        public PentestingMethodologyController(PentestingMethodologyRepository pentestingMethodologyRepository)
        {
            _PentestingMethodologyRepository  = pentestingMethodologyRepository;
        }

        /// <summary>
        /// Retrieves a page of pentesting methodologies.
        /// </summary>
        /// <param name="pageNumber">The number of the page to retrieve. Page numbers start at 1.</param>
        /// <param name="pageSize">The number of methodologies to include on each page.</param>
        /// <returns>An IActionResult containing a list of methodologies.</returns>
        /// <response code="200">Returns the list of methodologies for the specified page.</response>
        /// <response code="400">Returns if the pageNumber or pageSize is less than 1.</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(object))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(object))]
        public IActionResult GetPage(int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                // Validate input parameters
                if (pageNumber < 1 || pageSize < 1)
                {
                    return BadRequest(new
                    {
                        Message = "Invalid page number or page size. Both should be greater than or equal to 1."
                    });
                }

                // Retrieve methodologies for the specified page
                var methodologies = _PentestingMethodologyRepository.GetPage(pageNumber, pageSize);

                // Return the list of methodologies for the specified page
                return Ok(methodologies);
            }
            catch (Exception)
            {
                // Log the exception for further investigation

                // Return a generic error response for unexpected errors
                return StatusCode(500, new
                {
                    Message = "An unexpected error occurred while retrieving pentesting methodologies. Please try again later."
                });
            }
        }



        /// <summary>
        /// Retrieves a pentesting methodology by ID.
        /// </summary>
        /// <param name="id">The ID of the pentesting methodology to retrieve.</param>
        /// <returns>An IActionResult containing the pentesting methodology if found, NotFound otherwise.</returns>
        /// <response code="200">Returns the pentesting methodology for the specified ID.</response>
        /// <response code="404">Returns if the pentesting methodology is not found.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Get(string id)
        {
            var PentestingMethodology = _PentestingMethodologyRepository.Get(id);

            if (PentestingMethodology == null)
            {
                return NotFound();
            }

            return Ok(PentestingMethodology);
        }


        /// <summary>
        /// Creates a new pentesting methodology.
        /// </summary>
        /// <param name="pentestingMethodology">The data of the pentesting methodology to create.</param>
        /// <returns>An IActionResult that represents the result of the creation operation.</returns>
        /// <response code="201">Returns when the pentesting methodology is created successfully.</response>
        /// <response code="400">Returns if the pentestingMethodology parameter is null.</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Create([Required , FromBody] MethodologyDTO pentestingMethodology)
        {
            var method = pentestingMethodology.ToPentestingMethodology();
            var methodId = _PentestingMethodologyRepository.Create(method);

            return CreatedAtAction(nameof(Get), new { id = methodId }, pentestingMethodology);
        }


        /// <summary>
        /// Deletes a pentesting methodology by ID.
        /// </summary>
        /// <param name="id">The ID of the pentesting methodology to delete.</param>
        /// <returns>An IActionResult that represents the result of the deletion operation.</returns>
        /// <response code="204">Returns when the pentesting methodology is deleted successfully.</response>
        /// <response code="404">Returns if the pentesting methodology is not found.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Delete(string id)
        {
            var PentestingMethodology = _PentestingMethodologyRepository.Get(id);

            if (PentestingMethodology == null)
            {
                return NotFound(new
                {
                    Message = $"There is no Methodology with ID = {id}."
                });
            }

            _PentestingMethodologyRepository.Delete(PentestingMethodology);

            return Ok(new
            {
                Message = $"Methodology with ID = {id} was deleted successfully."
            });
        }

        /// <summary>
        /// Retrieves the tests for a pentesting methodology.
        /// </summary>
        /// <param name="id">The ID of the pentesting methodology.</param>
        /// <param name="pageNumber">The number of the page to retrieve. Page numbers start at 1.</param>
        /// <param name="pageSize">The number of tests to include on each page.</param>
        /// <returns>An IActionResult containing the tests for the pentesting methodology if found, NotFound otherwise.</returns>
        /// <response code="200">Returns the tests for the specified pentesting methodology.</response>
        /// <response code="404">Returns if the pentesting methodology is not found.</response>
        [HttpGet("{id}/tests")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetTests(string id, int pageNumber, int pageSize)
        {
            try
            {
                // Retrieve tests for the specified pentesting methodology
                var tests = _PentestingMethodologyRepository.GetPage_Tests(id, pageNumber, pageSize);

                // Check if tests are found
                if (tests == null)
                {
                    return NotFound(new
                    {
                        Message = $"There is no Methodology with ID = {id}."
                    });
                }

                // Return the tests
                return Ok(tests);
            }
            catch (Exception)
            {
                // Log the exception or handle it appropriately
                return StatusCode(500, new
                {
                    Message = "Internal server error.",
                    //Error = ex.Message
                });
            }
        }
    }

}
