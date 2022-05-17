using System.Collections.Generic;
using AutoMapper;
using Commander.Data;
using Commander.DTOs;
using Commander.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace Commander.Controllers
{   
    [Route("api/[controller]")]
    [ApiController]
    public class CommandsController : ControllerBase
    {
        private readonly ICommanderRepo _repository;
        private readonly IMapper _mapper;

        public CommandsController(ICommanderRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        
        //Get api/Commands
        [HttpGet]
        public ActionResult <IEnumerable<CommandReadDto>> GetAllCommands()
        {
            var commandItems = _repository.GetAllCommands();
            var CommandReadDtoItems = _mapper.Map<IEnumerable<CommandReadDto>>(commandItems);
            return Ok(CommandReadDtoItems);
        }
        //Get api/Commands/{id}
        [HttpGet("{id}", Name = "GetCommandById")]
        public ActionResult<CommandReadDto> GetCommandById(int id)
        {
            var commandItem = _repository.GetCommandById(id);
            
            if(commandItem!=null){
                var CommandReadDtoItem = _mapper.Map<CommandReadDto>(commandItem);
                return Ok(CommandReadDtoItem);
            }
            
            else
            return NotFound();
        }
        //post api/Commands
        [HttpPost]
        public ActionResult<CommandReadDto> CreateCommand(CommandCreateDto commandCreateDto)
        {   var commandCreateDtoItem = _mapper.Map<Command>(commandCreateDto);
            _repository.CreateCommand(commandCreateDtoItem);
            _repository.savechanges();
            var commandReadDto = _mapper.Map<CommandReadDto>(commandCreateDtoItem);
            return CreatedAtRoute(nameof(GetCommandById), new{id = commandReadDto.Id}, commandReadDto);

        }

        //Put api/Commands/{id}
        [HttpPut("{id}")]
        public ActionResult UpdateCommand(int id, CommandUpdateDto commandUpdateDto)
        {
            var commandModelFromDb = _repository.GetCommandById(id);
            if (commandModelFromDb == null)
            {
                return NotFound();
            }
            _mapper.Map(commandUpdateDto, commandModelFromDb);
            _repository.UpfdateCommand(commandModelFromDb);
            _repository.savechanges();

            return NoContent();
            
        }

        //Patch api/Commands/{id}
        [HttpPatch("{id}")]
        public ActionResult PartialCommandUpdate(int id, JsonPatchDocument<CommandUpdateDto> patchDoc)
        {
            var commandModelFromDb = _repository.GetCommandById(id);
            if(commandModelFromDb==null)
            {
                return NotFound();
            }
            
            var patchCommand = _mapper.Map<CommandUpdateDto>(commandModelFromDb);
            patchDoc.ApplyTo(patchCommand, ModelState);
            if(!TryValidateModel(patchCommand))
            {
                return ValidationProblem(ModelState);
            }
            _mapper.Map(patchCommand, commandModelFromDb);
            _repository.UpfdateCommand(commandModelFromDb);
            _repository.savechanges();
            return NoContent();
        }

        //Delete api/commands/{id}
        [HttpDelete("{id}")]
        public ActionResult DeleteCommand(int id)
        {
            var commandFromDb = _repository.GetCommandById(id);
            if(commandFromDb==null)
            {
                return NotFound();
            }
            _repository.DeleteCommand(commandFromDb);
            _repository.savechanges();
            return NoContent();
        }



    }
}