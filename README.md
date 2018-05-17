# eloomi API nuget package

The eloomi API NuGet package makes it easy to interact with the eloomi API in an object based approach. The package can be found here: https://www.nuget.org/packages/Eloomi.Eloomi.Api and the API documentation here: https://api.eloomi.com/apidoc

Currently the package supports the following endpoints
* Users
* Units (Departments)
* Teams
* Courses
* Goals


# Initialisation 

The initialisation of the NuGet package requires the **Client ID** and **Client Secret** which you have recieved from eloomi. If you do not yet have an id/secret pair, please feel free to contact your customer representive.

**Standard connection**
The most standarized way of connecting to the API is with the following code

```c#
    using EloomiApiV2;
    namespace EloomiApiTest 
    {
        class Program
        {
            static void Main(string[] args)
            {
                // Fill correct details
                int ClientID = 200;
                string ClientSecret = "JG4m3eKWGMPKLYJL7DV8dquDjKgXYsGlZO2VL5i4";
    
                // Initialize the API Wrapper
                var API = new Api(ClientID, ClientSecret);
            }
        }
    }
```
 **Proxy connection**
 In some cases it might be neccesary to proxy all calls to the services. This can be done by filling in a **System.Net.IWebProxy** object as the last parameter of the API() Initialiser.
 ```c#
using EloomiApiV2;
namespace EloomiApiTest 
{
    class Program
    {
        static void Main(string[] args)
        {
            // Fill correct details
            int ClientID = 200;
            string ClientSecret = "JG4m3eKWGMPKLYJL7DV8dquDjKgXYsGlZO2VL5i4";

            var Proxy = new System.Net.WebProxy("0.0.0.0");
            Proxy.Credentials = new System.Net.NetworkCredential("Username","password");

            // Initialize the API Wrapper
            var API = new Api(ClientID, ClientSecret, Proxy);
        }
    }
}
```
**Debugging**
If debugging is set to true, output will be made to the console *(Console.WriteLine)* with details of the JSON posted and recieved. Debugging is turned on by setting the API.Debug() method as true.
 ```c#
using EloomiApiV2;
namespace EloomiApiTest 
{
    class Program
    {
        static void Main(string[] args)
        {
            // Fill correct details
            int ClientID = 200;
            string ClientSecret = "JG4m3eKWGMPKLYJL7DV8dquDjKgXYsGlZO2VL5i4";
            // Initialize the API Wrapper
            var API = new Api(ClientID, ClientSecret);
            API.Debug(true); // Setting debug as true.
        }
    }
}
```

After successfull authentication, you will use the "API" variable to manipulate the api forwardly, so store it accordingly. Endpoint adapters are accessed by injecting them through the API variable:
 ```c#
            API.User(); // Returns the User endpoint adapter
            API.Unit(); // Returns the Department/Unit endpoint adapter
            API.Team(); // Returns the Team endpoint adapter
            API.Goal(); // Returns the Goal endpoint adapter
            API.Course(); // Returns the Course endpoint adapter
```

Each of these adapters will contain methods to work with the given endpoints. 

# Users endpoint

With the users endpoint it is possible to do actions on the user objects in eloomi. A user will always have a unique numeric ID, but it is also possible when creating or updating the user object, to fill in a **"employee_id"** this can be anything, but most probably your internal id for the given employee. The API offers various methods to filter, 
select and update based on this as well.

**Custom attributes notice**
Custom attributes are possible but must be registered in the system before usage. Therefor please contact your eloomi representive before utilizing them since it may throw exceptions if not allowed.

**User model**
The most used parameters on the user-model is as the following.
 ```c#
            EloomiApiV2.Model.User NewUser = new EloomiApiV2.Model.User()
            {
                FirstName = "Firstname",
                LastName = "Surname",
                Initials = "Mr",
                Level = "Senior",
                Title = "CEO",
                Phone = "00000000",
                MobilePhone = "00000000",
                Email = "testuser@eloomi.com",
                Activate = true, // Should the user recieve an activation email upon creation?
                BirthDay = DateTime.Now.AddYears(-25),
                Company = "Company",
                EndOfEmployment = DateTime.MaxValue,
                CustomAttributes =
                {
                    { "custom","random" }
                },
                Gender = "M"

            };
```
## Get user/users
**Get all users**
 ```c#
            var Users = API.User().Get();

            if( Users.StatusCode == "200" )
            {
                foreach (var User in Users.Data)
                {
                    Console.WriteLine("My name is: " + User.FirstName);
                }
            }
```

**Get a user by eloomi ID**
 ```c#
            var User = API.User().Get(150); // ID = 150

            if( User.StatusCode == "200" )
            {
                Console.WriteLine("My name is: " + User.Data.FirstName);
            } else
            {
                // User probably doesent exist.
                Console.WriteLine(User.StatusCode + " - " + User.Message);
            }
```

**Get a user by employee id**
 ```c#
            var User = API.User().GetByEmployeeID("EMPLOYEE1234"); // employee_id = EMPLOYEE1234

            if( User.StatusCode == "200" )
            {
                Console.WriteLine("My name is: " + User.Data.FirstName);
            } else
            {
                // User probably doesent exist.
                Console.WriteLine(User.StatusCode + " - " + User.Message);
            }
```

## Add user
**Add a user to eloomi**
When adding a user to eloomi please note that FirstName, LastName and one of Email or UserName is required fields. Rest is optional.

 ```c#
            // Create a user object from model
            var User = new EloomiApiV2.Model.User()
            {
                FirstName = "Test",
                LastName = "User",
                Email = "testuser@eloomi.com",
		           EmployeeID = "EMPLOYEE1234"
            };

            // Add through the API user adapter
            var AddResponse = API.User().Add(User);

            // Check response
            if(AddResponse.StatusCode == "200" )
            {
                Console.WriteLine("Successfully added: " + AddResponse.Data.FirstName);
            } else
            {
                Console.WriteLine(AddResponse.StatusCode + " - " + AddResponse.Message);
            }
```

## Update user
As when getting users, you may also update users either with eloomi ID or employee_id.

**Update user with eloomi ID**
When calling the Update adapter method, the package will look for the Id attribute on the user, and update accordingly.
 ```c#
            // User object with valid Id parameter
            var User = new EloomiApiV2.Model.User()
            {
                Id = 150,
                FirstName = "Test",
                LastName = "User",
                Email = "testuser@eloomi.com"
            };

            // Update through the API user adapter
            var UpdateResponse = API.User().Update(User);

            // Check response
            if(UpdateResponse .StatusCode == "200" )
            {
                Console.WriteLine("Successfully updated: " + UpdateResponse .Data.FirstName);
            } else
            {
                // User probably doesent exist.
                Console.WriteLine(UpdateResponse .StatusCode + " - " + UpdateResponse .Message);
            }
```

**Update user with own employee id**
When calling the UpdateByEmployeeID method, the package will look for the EmployeeID attribute on the user, and update accordingly.

 ```c#
            // User object with valid EmployeeID parameter
            var User = new EloomiApiV2.Model.User()
            {
                EmployeeID = "EMPLOYEE1234",
                FirstName = "Test",
                LastName = "User",
                Email = "testuser@eloomi.com"
            };

            // Update through the API user adapter
            var UpdateResponse = API.User().UpdateByEmployeeID(User);

            // Check response
            if(UpdateResponse .StatusCode == "200" )
            {
                Console.WriteLine("Successfully updated: " + UpdateResponse .Data.FirstName);
            } else
            {
                // User probably doesent exist.
                Console.WriteLine(UpdateResponse .StatusCode + " - " + UpdateResponse .Message);
            }
```

## Delete user
As when getting and updating users, you may also delete users either with eloomi ID or employee_id. We do generally not encourage the use of "Delete" but rather EndOfEmploymentAt which will automatically deactivate the given user.

**Delete with eloomi ID**
When calling the Update adapter method, the package will look for the Id attribute on the user, and update accordingly.
 ```c#
            // User object with valid Id parameter
            var User = new EloomiApiV2.Model.User()
            {
                Id = 150,
            };

            // Update through the API user adapter
            var DeleteResponse = API.User().Delete(User);

            // Check response
            if(DeleteResponse .StatusCode == "200" )
            {
                Console.WriteLine("Successfully deleted user");
            } else
            {
                // User probably doesent exist.
                Console.WriteLine(DeleteResponse .StatusCode + " - " + DeleteResponse .Message);
            }
```

**Delete with employee ID**
When calling the Update adapter method, the package will look for the Id attribute on the user, and update accordingly.
 ```c#
            var DeleteResponse = API.User().DeleteByEmployeeID("EMPLOYEE1234");

            // Check response
            if(DeleteResponse.StatusCode == "200" )
            {
                Console.WriteLine("Successfully deleted user");
            } else
            {
                // User probably doesent exist.
                Console.WriteLine(DeleteResponse.StatusCode + " - " + DeleteResponse.Message);
            }
```

# Units endpoint (Departments)
With the units endpoint it is possible to do actions on the unit/department objects in eloomi. A unit will always have a unique numeric ID, but it is also possible when creating or updating the unit object, to fill in a **"department_code"** this can be anything, but most probably your internal id for the given department. The API offers various methods to filter,  select and update based on this as well.

**Custom attributes notice**
Custom attributes are possible but must be registered in the system before usage. Therefor please contact your eloomi representive before utilizing them since it may throw exceptions if not allowed.

**Unit model**
The most used parameters on the unit-model is as the following.
 ```c#
            EloomiApiV2.Model.Unit Unit = new EloomiApiV2.Model.Unit()
            {
                Name = "Test department",
                Code = "Department code",

                // If using eloomi ID's
                Leaders = new List<int>(),
                Users = new List<int>(),

                // If using own ID's
                LeadersCode = new List<string>(),
                CustomAttributes =
                {
                    { "attribute","value" }
                }
            };
```
## Get unit/units
**Get all users**
 ```c#
            var Units = API.Unit().Get();

            if (Units.StatusCode == "200")
            {
                foreach (var Unit in Units.Data)
                {
                    Console.WriteLine("Department name is: " + Unit.Name);
                }
            }
```

**Get a unit by eloomi ID**
 ```c#
            var Unit = API.Unit().Get(150); // ID = 150

            if (Unit.StatusCode == "200")
            {
                Console.WriteLine("Unit name is: " + Unit.Data.Name);
            }
            else
            {
                // Unit probably doesent exist.
                Console.WriteLine(Unit.StatusCode + " - " + Unit.Message);
            }
```

**Get a unit by unit code**
 ```c#
            var Unit = API.Unit().GetByUnitCode("UNIT1234"); // code = UNIT1234

            if (Unit.StatusCode == "200")
            {
                Console.WriteLine("Unit name is: " + Unit.Data.Name);
            }
            else
            {
                // Unit probably doesent exist.
                Console.WriteLine(Unit.StatusCode + " - " + Unit.Message);
            }
```

## Add unit
**Add a user to eloomi**
When adding a unit to eloomi please note that Name is a required field. Rest is optional.

**Add to top level**
If you want to create a unit which resides at the top-most level of the organizational hierachy, follow this example
 ```c#
            // Create a unit object from model
            var Unit = new EloomiApiV2.Model.Unit()
            {
                Name = "Test department",
                Code = "UNIT1234",
            };

            // Add through UNIT adapter
            var AddResponse = API.Unit().Add(Unit);

            // Check response
            if (AddResponse.StatusCode == "200")
            {
                Console.WriteLine("Successfully added: " + AddResponse.Data.Name);
            }
            else
            {
                Console.WriteLine(AddResponse.StatusCode + " - " + AddResponse.Message);
            }
```

**Create a unit as a child of another unit**
If you would like the unit to be a child of another unit, this can be achieved by either using the other units "CODE" or "ID".

 ```c#
            var Unit = new EloomiApiV2.Model.Unit()
            {
                Name = "Test department",
                Code = "UNIT2345",
                // If using code
                ParentCode = "UNIT1234",
                // If using ID
                ParentId  = 150
            };
```

## Update unit
You may update units by either code or ID in the same way as when updating users

**Update unit with eloomi ID**
When calling the Update adapter method, the package will look for the Id attribute on the unit, and update accordingly.

 ```c#
            // Update the unit through ID
            var UpdateResponse = API.Unit().Update(Unit);


            // Check response
            if (UpdateResponse.StatusCode == "200")
            {
                Console.WriteLine("Successfully updated: " + UpdateResponse.Data.Name);
            }
            else
            {
                Console.WriteLine(UpdateResponse.StatusCode + " - " + UpdateResponse.Message);
            }
```

**Update unit with own code**
When calling the UpdateByUnitCode method, the package will look for the "Code" attribute on the unit, and update accordingly.

 ```c#
            // Update the unit through CODE
            var UpdateResponse = API.Unit().UpdateByUnitCode(Unit);

            // Check response
            if (UpdateResponse.StatusCode == "200")
            {
                Console.WriteLine("Successfully updated: " + UpdateResponse.Data.Name);
            }
            else
            {
                Console.WriteLine(UpdateResponse.StatusCode + " - " + UpdateResponse.Message);
            }
```

**Add users to unit**
You can add and remove users from the unit with the "Users" list.
 ```c#
            var UnitToUpdate = API.Unit().Get(150);
            if( UnitToUpdate.StatusCode == "200" )
            {
                UnitToUpdate.Data.Users.Add(120);
                UnitToUpdate.Data.Users.Add(130);
                UnitToUpdate.Data.Users.Add(140);

                API.Unit().Update(UnitToUpdate.Data);
            }
```

**Add Leaders to unit**
You can add and remove leaders from the unit with the "Leaders" list.
 ```c#
            var UnitToUpdate = API.Unit().Get(150);
            if( UnitToUpdate.StatusCode == "200" )
            {
                UnitToUpdate.Data.Leaders.Add(120);
                UnitToUpdate.Data.Leaders.Add(130);
                UnitToUpdate.Data.Leaders.Add(140);

                API.Unit().Update(UnitToUpdate.Data);
            }
```

## Delete unit
Deleting units can be done either with eloomi ID or through own unit code

**Delete with eloomi ID**
When calling the Update adapter method, the package will look for the Id attribute on the user, and update accordingly.
 ```c#
            // Delete the unit through ID
            var UpdateResponse = API.Unit().Delete(Unit);

            // Check response
            if (DeleteResponse.StatusCode == "200")
            {
                Console.WriteLine("Successfully deleted unit");
            }
            else
            {
                Console.WriteLine(DeleteResponse.StatusCode + " - " + DeleteResponse.Message);
            }
```

**Delete with employee ID**
When calling the Update adapter method, the package will look for the Id attribute on the user, and update accordingly.
 ```c#
            // Delete the unit through CODE
            var DeleteResponse = API.Unit().DeleteByUnitCode("UNIT1234");

            // Check response
            if (DeleteResponse.StatusCode == "200")
            {
                Console.WriteLine("Successfully deleted unit");
            }
            else
            {
                Console.WriteLine(DeleteResponse.StatusCode + " - " + DeleteResponse.Message);
            }
```

# Teams endpoint


**Custom attributes notice**
Custom attributes are possible but must be registered in the system before usage. Therefor please contact your eloomi representive before utilizing them since it may throw exceptions if not allowed.

**Team model**
The most used parameters on the team-model is as the following.
 ```c#
            EloomiApiV2.Model.Team TeamModel = new EloomiApiV2.Model.Team()
            {
                Name = "Team name",
                Description = "Team description",
                Users = new List<int>(),
                Leaders = new List<int>(),
                CustomAttributes =
                {
                    { "key","value" }
                },
            };
```
## Get team/teams
**Get all users**
 ```c#
            var Teams= API.Team().Get();

            if (Teams.StatusCode == "200")
            {
                foreach (var Team in Teams.Data)
                {
                    Console.WriteLine("Team name is: " + Team.Name);
                }
            }
```

**Get a unit by eloomi ID**
 ```c#
            var Team = API.Team().Get(150); // ID = 150

            if (Team.StatusCode == "200")
            {
                Console.WriteLine("Unit name is: " + Team.Data.Name);
            }
            else
            {
                // Unit probably doesent exist.
                Console.WriteLine(Team.StatusCode + " - " + Team.Message);
            }
```


## Add team
**Add a user to eloomi**
When adding a team to eloomi please note that Name is a required field. Rest is optional.
 ```c#
            var NewTeam = new EloomiApiV2.Model.Team()
            {
                Name = "Test team",
                Description = "Awesome team!"
            };

            var AddResponse = API.Team().Add(NewTeam);

            if( AddResponse.StatusCode == "200" )
            {
                Console.WriteLine("Added team: " + AddResponse.Data.Name);
            } else
            {
                Console.WriteLine(AddResponse.StatusCode + " - " + AddResponse.Message);
            }
```


## Update team
You may update teams directly and this way also add people to the team.

 ```c#
            var UpdateTeam = new EloomiApiV2.Model.Team()
            {
                Id = 150,
                Name = "Update team",
                Description = "Awesome updated team!"
            };

            var UpdateResponse = API.Team().Update(UpdateTeam);

            if (UpdateResponse.StatusCode == "200")
            {
                Console.WriteLine("Updated team: " + UpdateResponse.Data.Name);
            }
            else
            {
                Console.WriteLine(UpdateResponse.StatusCode + " - " + UpdateResponse.Message);
            }
```

**Add users to team**
You can add and remove users from the team with the "Users" list.
 ```c#
            var TeamToUpdate = API.Team().Get(150);
            if (TeamToUpdate.StatusCode == "200")
            {
                TeamToUpdate.Data.Users.Add(120);
                TeamToUpdate.Data.Users.Add(130);
                TeamToUpdate.Data.Users.Add(140);

                API.Team().Update(TeamToUpdate.Data);
            }
```

**Add Leaders to team**
You can add and remove leaders from the team with the "Leaders" list.
 ```c#
            var TeamToUpdate = API.Team().Get(150);
            if (TeamToUpdate.StatusCode == "200")
            {
                TeamToUpdate.Data.Leaders.Add(120);
                TeamToUpdate.Data.Leaders.Add(130);
                TeamToUpdate.Data.Leaders.Add(140);

                API.Team().Update(TeamToUpdate.Data);
            }
```

## Delete team
Deleting teams are done using the eloomi ID

 ```c#
            var DeleteTeam = new EloomiApiV2.Model.Team()
            { Id = 150};

            var DeleteResponse = API.Team().Delete(DeleteTeam);

            if (DeleteResponse.StatusCode == "200")
            {
                Console.WriteLine("Deleted team");
            }
            else
            {
                Console.WriteLine(DeleteResponse.StatusCode + " - " + DeleteResponse.Message);
            }
```
