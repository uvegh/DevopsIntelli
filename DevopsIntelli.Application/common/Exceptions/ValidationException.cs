using System;
using System.Collections.Generic;
using System.Text;

namespace DevopsIntelli.Application.common.Exceptions;


public class ValidationException:Exception
{
    /// <summary>
    /// define error property
    /// </summary>
    public IDictionary<string, string[]> Errors { get; }
    public string Entity;
    public object Name;



    public ValidationException(IDictionary<string, string[]> error) : base("One or more validation errors occured") {
        Errors = error;
    
    }
    

    
}
