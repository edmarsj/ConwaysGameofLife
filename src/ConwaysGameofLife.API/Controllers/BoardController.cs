using ConwaysGameofLife.API.Models;
using ConwaysGameofLife.Application.Commands;
using ConwaysGameofLife.Domain.Exceptions;
using ConwaysGameofLife.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ConwaysGameofLife.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BoardController : ApiControllerBase
    {
        private readonly IMediator _mediator;

        public BoardController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Returns a list with all boards available in the server and their ascii representation
        /// </summary>
        /// <response code="200">The list of boards</response>
        [HttpGet]
        [ProducesResponseType(typeof(ResponseModel<IEnumerable<BoardListEntry>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new ListAllBoardsCommand());

            return Ok(result);
        }

        /// <summary>
        /// Deletes a given board
        /// </summary>
        /// <param name="boardId">The id of the board</param>
        /// <response code="204">The board was deleted</response>
        /// <response code="404">The board was not found</response>
        [HttpDelete("{boardId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ResponseModel), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(Guid boardId)
        {
            await _mediator.Send(new DeleteBoardCommand(boardId));

            return NoContent();
        }

        /// <summary>
        /// Adds a new board to the server
        /// </summary>
        /// <param name="model">The information about the board to be added</param>
        /// <response code="201">The id of the newly created board</response>
        [HttpPost]
        [ProducesResponseType(typeof(ResponseModel<IEnumerable<BoardListEntry>>), StatusCodes.Status201Created)]
        public async Task<IActionResult> Post(CreateBoardRequestModel model)
        {
            if (string.IsNullOrWhiteSpace(model.BoardName))
            {
                throw new BoardNameMissingException();
            }
            if (model.InitialState == null)
            {
                throw new BoardInitialStateMissingException();
            }

            var boardId = await _mediator.Send(new AddBoardCommand(model.BoardName, model.InitialState));

            return Created(new CreateBoardResponseModel(boardId));
        }


        /// <summary>
        /// Uploads a new board to the server using a file
        /// </summary>
        /// <param name="model">The information about the board to be added</param>
        /// <response code="201">The id of the newly created board</response>
        [HttpPost("upload")]
        [ProducesResponseType(typeof(ResponseModel<IEnumerable<BoardListEntry>>), StatusCodes.Status201Created)]
        public async Task<IActionResult> PostFile([FromForm] UploadBoardRequestModel model)
        {
            if (string.IsNullOrWhiteSpace(model.BoardName))
            {
                throw new BoardNameMissingException();
            }

            if (model.BoardData == null)
            {
                throw new BoardInitialStateMissingException();
            }

            // Read file contents
            using var fileStream = model.BoardData.OpenReadStream();
            using var streamReader = new StreamReader(fileStream);
            var fileContent = await streamReader.ReadToEndAsync();
                        
            var boardId = await _mediator.Send(new UploadBoardCommand(model.BoardName, fileContent));

            return Created(new CreateBoardResponseModel(boardId));
        }     

        /// <summary>
        /// Returns the next x generations for a given board already saved in the server 
        /// </summary>
        /// <param name="boardId">The id of the board</param>
        /// <param name="outputType">If the representation of the board will be ascii or a numeric matrix. Default: ascii</param>
        /// <param name="numGenerations">The number of generations to be fetch. Default 1 </param>
        /// <response code="200">A list with the resulting generations for the request</response>
        /// <response code="404">The board was not found</response>
        [HttpGet("{boardId}/next")]
        [ProducesResponseType(typeof(ResponseModel<BoardStateResult>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseModel), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetNext(Guid boardId, [FromQuery] OutputType outputType, [FromQuery] int numGenerations = 1)
        {
            var stateResult = await _mediator.Send(new PreviewNextNStatesCommand(boardId, numGenerations));

            var result = BoardStateResult.From(stateResult.BoardName, stateResult.States, stateResult.ElapsedMilliseconds, outputType);

            return Ok(result);
        }

        /// <summary>
        /// Returns a specific generation for a given board
        /// </summary>
        /// <param name="boardId">The id of the board</param>
        /// <param name="generation">The requested generation </param>
        /// <param name="outputType">If the representation of the board will be ascii or a numeric matrix. Default: ascii</param>
        /// <response code="200">The board state</response>
        /// <response code="404">The board was not found</response>
        [HttpGet("{boardId}/generation/{generation}")]
        [ProducesResponseType(typeof(ResponseModel<BoardStateResult>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseModel), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetGeneration(Guid boardId, [Range(0, int.MaxValue)] int generation, [FromQuery] OutputType outputType)
        {
            var stateResult = await _mediator.Send(new PreviewBoardGenerationCommand(boardId, generation));

            var result = BoardStateResult.From(stateResult.BoardName, stateResult.States, stateResult.ElapsedMilliseconds, outputType);

            return Ok(result);
        }

        /// <summary>
        /// Returns the final state of a given board
        /// </summary>
        /// <param name="boardId">The id of the board</param>
        /// <param name="outputType">If the representation of the board will be ascii or a numeric matrix. Default: ascii</param>
        /// <param name="maxGenerations">The max number of generations to be processed before aborting the operation. Default is 1024</param>
        /// <response code="200">The board state</response>
        /// <response code="404">The board was not found</response>
        [HttpGet("{boardId}/final")]
        [ProducesResponseType(typeof(ResponseModel<FinalBoardStateResult>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseModel), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetFinal(Guid boardId, [FromQuery] OutputType outputType, [FromQuery] int maxGenerations = 1024)
        {
            var result = await _mediator.Send(new PreviewFinalStateCommand(boardId, maxGenerations));

            var response = BoardStateResult.From(result.BoardName, result.FinalState, outputType, result.ElapsedMilliseconds, result.NumAttempts, result.Error);

            return Ok(response);
        }
    }
}
