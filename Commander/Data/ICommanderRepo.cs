using System.Collections.Generic;
using Commander.DTOs;
using Commander.Models;

namespace Commander.Data    
{
    public interface ICommanderRepo
    {
        bool savechanges();
        IEnumerable<Command> GetAllCommands();
        
        Command GetCommandById(int id);

        void CreateCommand(Command cmd);

        void UpfdateCommand(Command cmd);

        void DeleteCommand(Command cmd);
    }
}