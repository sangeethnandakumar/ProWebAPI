# ProWebAPI
ASP.NET Core API with all proper standards

### Standards Implemented
| Name | Description
| ------ | ------
| Standard Response | Implementation of a standard response pattern for DTO and internal
| Versioning | Supports versioned endpoints 'api/v1/' using API versioningccc
| Environment Variable | Dynamic environment based appsettings and configurations
| OData v4 | Support for endpoints with OData for easy client manipulation

# Swagger
Install Swagger
```
Swashbuckle.AspNetCore
```
Setup DI
```csharp
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ProWebAPI", Version = "v1" });
                c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ProWebAPI v1"));
            }
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
```

# Versioning
Install Nuget library
```nuget
    Microsoft.AspNetCore.Mvc.Versioning
```
Add to dependency container
```csharp
   services.AddApiVersioning(options =>
            {
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.ReportApiVersions = true;
            });
```
Decorate the controllers base on the need
```csharp
    [ApiController]
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class StudentsController : ControllerBase
    {
        [HttpGet]
        [MapToApiVersion("1.0")]
        [Route("Success")]
        public Response Success()
        {
        }
        
        [HttpGet]
        [MapToApiVersion("2.0")]
        [Route("Success")]
        public Response Success20()
        {
        }

        [HttpGet]
        [Route("Failure")]
        public Response Failure()
        {
        }
               
        [HttpPost]
        [Route("Submit")]
        public Response Submit(User user)
        {
        }
    }
```

# Request Validator
Install Nuget library
```nuget
    FluentValidation.AspNetCore
```
Register validators on DI
```csharp
 services.AddFluentValidation(x => x.RegisterValidatorsFromAssemblyContaining<Startup>());
```
On the 'RequestDto' Namespace, Add the validators along with dto
```csharp
namespace ProWebAPI.RequestDtos
{
    public class User
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
    }

    public class UserValidator : AbstractValidator<User>
    {
        public UserValidator()
        {
            RuleFor(x => x.FirstName).NotNull().NotEmpty();
            RuleFor(x => x.LastName).NotNull().NotEmpty();
            endpoints.Select().Count().Filter().OrderBy().MaxTop(100).Expand();
        }
    }
}
```
Build a ValidationFilter
```csharp
namespace ProWebAPI.Filters
{
    public class ValidationFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.ModelState.IsValid)
            {
                var errorResponse = new ErrorResponse
                {
                    ErrorCode = ErrorCodes.ERR01.ToString(),
                    Message = "Data submitted is not in correct format",
                    Status = ResponseStatus.WARNING.ToString()
                };

                var errorModalState = context.ModelState
                    .Where(x => x.Value.Errors.Count > 0)
                    .ToDictionary(kv => kv.Key, kv => kv.Value.Errors.Select(x => x.ErrorMessage))
                    .ToArray();

                foreach (var error in errorModalState)
                {
                    foreach (var subError in error.Value)
                    {
                        errorResponse.Info.Add($"{error.Key}: {subError}");
                    }
                }

                context.Result = new BadRequestObjectResult(errorResponse);
                return;
            }
            await next();
        }
    }
}
```
Register the filter and turn off [ApiController] auto 400 Bad Request intercept
```csharp
  public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers()
                .ConfigureApiBehaviorOptions(options =>
                {
                    options.SuppressModelStateInvalidFilter = true;
                });
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ProWebAPI", Version = "v1" });
                c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
            });
            services.AddApiVersioning(options =>
            {
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.ReportApiVersions = true;
            });
            services.AddFluentValidation(x => x.RegisterValidatorsFromAssemblyContaining<Startup>());
            services.AddMvc(options =>
            {
                options.Filters.Add<ValidationFilter>();
            });
        }
```

# AppSettings.json
Configure appsettings for databases
```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning",
      "Microsoft.Hosting.Lifetime": "Information"
    }
  },
  "DataConnections": {
    "DatabaseOne": {
      "IsStandardSecurity": false,
      "Server": "DESKTOP//SQLSERVER",
      "Catalogue": "MyDatabase",
      "Username": "sangee",
      "Password": "password@123"
    },
    "DatabaseTwo": {
      "IsStandardSecurity": false,
      "Server": "DESKTOP//SQLSERVER",
      "Catalogue": "MyDatabase",
      "Username": "sangee",
      "Password": "password@123"
    },
    "DatabaseThree": {
      "IsStandardSecurity": false,
      "Server": "DESKTOP//SQLSERVER",
      "Catalogue": "MyDatabase",
      "Username": "sangee",
      "Password": "password@123"
    }
  }
}
```
Create 2 ENVIRONMENT configurations
```
appsettings.Development.json
appsettings.Production.json
```

# OData v4
To support OData Install the NuGetpackages
```
Microsoft.AspNetCore.Mvc.NewtonsoftJson
Microsoft.AspNetCore.OData
OData.Swagger
```
Add to service DI for OData
```csharp
   services.AddControllers().AddNewtonsoftJson();
   ..
   ...
   ....
   .....
   services.AddOData();
   services.AddOdataSwaggerSupport();
```
Configure OData
```csharp
 app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.EnableDependencyInjection();
                endpoints.Select().Count().Filter().OrderBy().MaxTop(100);
            });
```
Decorate the Action Methord
```csharp
        [HttpGet]
        [MapToApiVersion("2.0")]
        [Route("Success")]
        [EnableQuery]
        public IActionResult Success20()
        {
            var listOfData = new List<Student>();
            listOfData.Add(new Student { Name = "Student A", Age = 10 });
            listOfData.Add(new Student { Name = "Student B", Age = 11 });
            listOfData.Add(new Student { Name = "Student C", Age = 12 });
            listOfData.Add(new Student { Name = "Student D", Age = 13 });
            return Ok(listOfData);
        }
```

Query by URL
```url
https://localhost:44351/api/v2/Students/Success?   $expand=data($filter= age gt 11;    $orderby=Name desc;  $select=Name)
```
