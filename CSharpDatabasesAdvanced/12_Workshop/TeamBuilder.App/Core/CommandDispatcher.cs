﻿namespace TeamBuilder.App
{
    using System;
    using System.Linq;
    using System.Reflection;
    using TeamBuilder.App.Commands.Contracts;
    using TeamBuilder.App.Contracts;
    using TeamBuilder.Services;

    public class CommandDispatcher:ICommandDispatcher
    {
        private IServiceProvider serviceProvider;

        public CommandDispatcher(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public  string DispatchCommand(string[] commandParameters)
        {
            var commandName = commandParameters[0];

            var commandArgs = commandParameters.Skip(1).ToArray();

                ICommand command = this.ParseCommand(this.serviceProvider,commandName);

            string result;

            try
            {
                 result = command.Execute(commandArgs);
            }
            catch (IndexOutOfRangeException)
            {
                throw new InvalidOperationException(string.Format(Constants.ErrorMessages.InvalidArgumentsCount, commandName));
            }

            return result;                 
        }

        private ICommand ParseCommand(IServiceProvider serviceProvider, string commandName)
        {
            Type commandType = Assembly.GetExecutingAssembly().GetTypes()
                            .Where(t => t.GetInterfaces().Contains(typeof(ICommand)))
                            .SingleOrDefault(t => t.Name == commandName + "Command");

            if (commandType == null)
            {
                throw new InvalidOperationException(string.Format(Constants.ErrorMessages.InvalidCommand,commandName));
            }

            var constructor = commandType.GetConstructors().First();

            var constructorParameters = constructor.GetParameters()
                .Select(pi => pi.ParameterType).ToArray();

            var services = constructorParameters.Select(p => serviceProvider.GetService(p)).ToArray();

            var command = Activator.CreateInstance(commandType,services);

            return (ICommand)command;
        }
    }
}
