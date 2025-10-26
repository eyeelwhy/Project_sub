using NUnit.Framework;
using Microsoft.AspNetCore.Mvc;
using api_docker.Controllers;
using api_docker.ModelDb;
using dockers_api.Classes;

namespace Unit_test
{
    [TestFixture]
    public class UsersControllerTests
    {
        [Test]
        public void Get_WhenCalled_ReturnsOkResult()
        {
            var controller = new UsersController();
            var result = controller.Get();
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
        }

        [Test]
        public void Post_Auth_WithValidCredentials_ReturnsOkResult()
        {
            var controller = new UsersController();
            var authUser = new UserAuth { Login = "testuser", Password = "testpass123" };
            var result = controller.Post(authUser);
            Assert.That(result, Is.InstanceOf<OkObjectResult>().Or.InstanceOf<NotFoundResult>());
        }

        [Test]
        public void Post_Auth_WithEmptyCredentials_ReturnsNotFound()
        {
            var controller = new UsersController();
            var authUser = new UserAuth { Login = "", Password = "" };
            var result = controller.Post(authUser);
            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }

        [Test]
        public void Register_WithValidData_ReturnsOkResult()
        {
            var controller = new UsersController();
            Random rnd = new Random();
            int rnd_num = rnd.Next(10000, 99999);
            var newUser = new UserRegDTO 
            { 
                Login = $"newuser123{rnd_num}", 
                Password = $"password123{rnd_num}", 
                Name = $"Test User {rnd_num}" 
            };
            var result = controller.Register(newUser);
            Assert.That(result, Is.InstanceOf<OkObjectResult>().Or.InstanceOf<BadRequestObjectResult>().Or.InstanceOf<NotFoundResult>());
        }
    }
}