using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System;
using UnityEngine;

public class DebugCommandBase
{
    public string _commandId { get; private set; }
    public string _commandDescription { get; private set; }
    public string _commandFormat { get; private set; }

    public DebugCommandBase(string commandID, string commandDescription, string commandFormat)
    {
        _commandId = commandID;
        _commandDescription = commandDescription;
        _commandFormat = commandFormat;
    }
}

public class DebugCommand : DebugCommandBase
{
    public Action command;

    public DebugCommand(string commandId, string commandDescription, string commandFormat, Action action)
        : base(commandId, commandDescription, commandFormat)
    {
        this.command = command;
    }

    public void Invoke()
    {
        command.Invoke();
    }
}