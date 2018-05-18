using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EloomiNuGetExamples
{
    class CreateUsersFromAListAddToDepartment
    {
        public void DoExample()
        {
            // Establish API connection
            var API = new EloomiApiV2.Api(200, "secret");

            // Lets fetch the department we want the users to be part of. 
            var Department = API.Unit().Get(1234); // Getting by ID.

            // Check if OK.
            if (Department.StatusCode == "200")
            {
                // Static list of user objects. Could be anything. CSV file or a deep-dive into
                // production systems (AX, AD etc.)
                List<EloomiApiV2.Model.User> Users = new List<EloomiApiV2.Model.User>()
                {
                    new EloomiApiV2.Model.User()
                    {
                        FirstName = "User 1",
                        LastName = "Lastname 1",
                        Email = "user1@eloomi.com",
                        DepartmentID = new List<long>() {  Department.Data.Id }, // Add to one or more departments.
                    },
                    new EloomiApiV2.Model.User()
                    {
                        FirstName = "User 2",
                        LastName = "Lastname 2",
                        Email = "user2@eloomi.com",
                        DepartmentID = new List<long>() {  Department.Data.Id }, // Add to one or more departments.
                    },
                };

                // Ready to create.
                foreach( var User in Users )
                {
                    var CreateResponse = API.User().Add(User);
                    if( CreateResponse.StatusCode == "200" )
                    {
                        Console.WriteLine("Successfully created " + CreateResponse.Data.FirstName);
                    } else
                    {
                        Console.WriteLine("Something went wrong when creating " + User.FirstName);
                    }
                }


            } else
            {
                // Couldent fetch department.
                Console.WriteLine("Couldent fetch department with id 1234");
            }

        }
    }
}
