# GiG.Core.ObjectMapping.AutoMapper

This Library provides an API to register an object mapper in your application using AutoMapper.

## Basic Usage

The below code needs to be added to the `Startup.cs`. This will register an the Object Mapper, together with AutoMapper and any assemblies containing Profiles.

```csharp
public void ConfigureServices(IServiceCollection services)
{
    services.AddObjectMapper(typeof(ProfileTypeFromAssembly1), typeof(ProfileTypeFromAssembly2));
}
```

Now you can inject AutoMapper at runtime into your services/controllers:

```csharp
public class EmployeesController
{
    private readonly IObjectMapper _mapper;

    public EmployeesController(IObjectMapper mapper) => _mapper = mapper;

    [ProducesResponseType((int)HttpStatusCode.Created)]
    [ProducesResponseType((int)HttpStatusCode.Conflict)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult<EmployeeResponse>> Post(EmployeeRequest request)
    {
	    var model = _mapper.Map<Employee>(request);
            ...
            ...
    }
}
```