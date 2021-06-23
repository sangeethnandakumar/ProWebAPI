# ProWebAPI
ASP.NET Core API with all proper standards

### Standards Implemented
| Name | Description
| ------ | ------
| Swagger | Implementation of Swagger OpenAPI standards
| Standard Response | Implementation of a standard response pattern for DTO and internal
| Versioning | Supports versioned endpoints 'api/v1/' using API versioningccc
| Environment Variable | Dynamic environment based appsettings and configurations
| OData v4 | Support for endpoints with OData for easy client manipulation
| Global Exception Handler | Enables standard error response for any 500 server errors
| Response Casing | Pascal casing with Newtonsoft
| OAuth v2 | Add support for OAuth from other repo (No imp here)
| Redis Cache | Implementing caching with Redis

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

# Standard Resoponse for API
Standard response
```csharp
namespace ProWebAPI.Modal
{
    public enum ResponseStatus
    {
        SUCCESS,
        WARNING,
        FAILED
    }

    public enum ErrorCodes
    {
        //RESERVED
        ERR01, // => Invalid request validation (400)

        ERR02, // => Unsupported OData Query (400)
        ERR03, // => Unhandled exception on server (500)

        //CUSTOM CODES
        ERR04
    }

    public class Response
    {
        protected ResponseStatus ResponseStatus { get; set; }
        public string Status { get; set; }
    }

    public class SuccessResponse<T> : Response
    {
        public T Data { get; set; }

        public SuccessResponse()
        {
            ResponseStatus = ResponseStatus.SUCCESS;
            Status = ResponseStatus.SUCCESS.ToString();
        }
    }

    public class ErrorResponse : Response
    {
        public string ErrorCode { get; set; }
        public string Message { get; set; }
        public List<string> Info { get; set; }

        public ErrorResponse()
        {
            ResponseStatus = ResponseStatus.FAILED;
            Status = ResponseStatus.FAILED.ToString();
            ErrorCode = ErrorCodes.ERR01.ToString();
            Info = new List<string>();
        }
    }

    public class Dto<T>
    {
        public bool IsSuccess { get; set; }
        public string Error { get; set; }
        public T Data { get; set; }
    }
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
            RuleFor(x => x.Age).NotNull().GreaterThan(0).LessThan(150).NotEmpty();
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
 services.AddControllers(options =>
            {
                options.Filters.Add<ValidationFilter>();
            })
             .ConfigureApiBehaviorOptions(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });
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
                endpoints.Select().Count().Filter().OrderBy().MaxTop(100).Expand();
            });
```
Override [EnableQuery] attribute with [EnableOData] to catch errors and return standard response
```csharp
namespace ProWebAPI.Attributes
{
    public class EnableODataAttribute : EnableQueryAttribute
    {
        public override void ValidateQuery(HttpRequest request, ODataQueryOptions queryOptions)
        {
            try
            {
                base.ValidateQuery(request, queryOptions);
            }
            catch (ODataException ex)
            {
                var errorResponse = new ErrorResponse
                {
                    ErrorCode = ErrorCodes.ERR02.ToString(),
                    Message = "Unsupported OData query",
                    Info = new List<string>() { ex.Message },
                    Status = ResponseStatus.WARNING.ToString()
                };

                request.HttpContext.Response.ContentType = "application/json";
                request.HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                request.HttpContext.Response.WriteAsJsonAsync(errorResponse);
                return;
            }
            catch (Exception ex)
            {
                var errorResponse = new ErrorResponse
                {
                    ErrorCode = ErrorCodes.ERR03.ToString(),
                    Message = "Something went wrong",
                    Status = ResponseStatus.FAILED.ToString(),
                    Info = new List<string>() { "An operation on the server resulted in failure" }
                };
                request.HttpContext.Response.ContentType = "application/json";
                request.HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
                request.HttpContext.Response.WriteAsJsonAsync(errorResponse);
                return;
            }
        }
    }
}
```
Decorate the Action Methord
```csharp
        [HttpGet]
        [MapToApiVersion("2.0")]
        [Route("Success")]
        [EnableOData]
        public IActionResult Success20()
        {
            var listOfData = new List<Student>();
            listOfData.Add(new Student { Name = "Student A", Age = 10 });
            listOfData.Add(new Student { Name = "Student B", Age = 11 });
            listOfData.Add(new Student { Name = "Student C", Age = 12 });
            listOfData.Add(new Student { Name = "Student D", Age = 13 });
            return Ok(new SuccessResponse<List<Student>>
            {
                Data = listOfData
            });
        }
```

Query by URL
```url
https://localhost:44351/api/v2/Students/Success?   $expand=data(  $filter= age gt 11; $orderby=Name desc; $select=Name  )
```

# Global Exception Handler
Create the Middleware
```csharp
namespace ProWebAPI.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception)
            {
                //Write exception log here
                await HandleExceptionAsync(context);
            }
        }

        private Task HandleExceptionAsync(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            var serverError = new ErrorResponse
            {
                ErrorCode = ErrorCodes.ERR03.ToString(),
                Message = "Something went wrong",
                Status = ResponseStatus.FAILED.ToString(),
                Info = new List<string>() { "An operation on the server resulted in failure" }
            };
            return context.Response.WriteAsJsonAsync(serverError);
        }
    }
}
```
Create an extension methord for app builder
```csharp
namespace ProWebAPI.Extensions
{
    public static class ExceptionExtensions
    {
        public static void UseGlobalExceptionHandler(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionMiddleware>();
        }
    }
}
```
Register the middleware
```csharp
app.UseGlobalExceptionHandler();
```

# Response Casing
Allow member casing (which will be in pascal case for props) so that OData query result won't conflict with casing
```csharp
 services.AddControllers()
            .AddNewtonsoftJson(options =>
            {
                options.UseMemberCasing();
            })
```

# Redis Cache
