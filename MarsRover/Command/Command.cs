using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace MarsRover.Command
{
    public class Command : ICommand
    {
        #region Fields
        private readonly IServiceProvider _serviceProvider;
        private readonly IEnumerable<CommandExecuter> _commandExecuters;
        #endregion

        #region Ctor
        public Command(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _commandExecuters = GetCommandExecutersInAssembly();
        }
        #endregion

        #region Methods

        /// <summary>
        /// Invoke executers according to command
        /// </summary>
        /// <param name="commands"></param>
        public void SendCommand(string command)
        {
            // Find an Executer to execute command
            var commandExecuter = _commandExecuters.FirstOrDefault(x => x.IsCommandBelongsToThisExecuter(command));
            if (commandExecuter == null)
                throw new Exception($"This ({command}) input is not valid. Please check your input. (You can consider test inputs)");

            commandExecuter.Execute(command);
        }

        /// <summary>
        /// Gets command executers (all classes that inherits from CommandExecuter)
        /// </summary>
        /// <returns></returns>
        private IEnumerable<CommandExecuter> GetCommandExecutersInAssembly()
        {
            return Assembly.GetExecutingAssembly()
                .DefinedTypes
                .Where(type => type.IsSubclassOf(typeof(CommandExecuter)) && !type.IsAbstract)
                .Select(x => Activator.CreateInstance(x, _serviceProvider) as CommandExecuter)
                .ToList();
        }

        #endregion
    }
}
