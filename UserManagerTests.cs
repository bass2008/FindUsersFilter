using iCoinSoftTest.Managers;
using iCoinSoftTest.Models;
using NUnit.Framework;
using System.Linq;

namespace iCoinSoftTest.Tests
{
    [TestFixture]
    public class UserManagerTests
    {
        private const int DefaultLimit = 50;

        private const int MaxLimit = 100000;

        [Test]
        public void RunStandart()
        {
            // Arrange
            var userManager = new UserManager();
            LittleInit(userManager);

            // Act
            var ivanov = userManager.Find("ivan", DefaultLimit);
            var pavlov = userManager.Find("pav",  DefaultLimit);
            var all    = userManager.Find("@",    DefaultLimit);

            // Assert
            Assert.IsTrue(ivanov.Count() <= DefaultLimit);
            Assert.IsTrue(ivanov.Count() == 1);
            Assert.IsTrue(ivanov.First().Id == 1);

            Assert.IsTrue(pavlov.Count() <= DefaultLimit);
            Assert.IsTrue(pavlov.Count() == 1);
            Assert.IsTrue(pavlov.First().Id == 4);

            Assert.IsTrue(all.Count() <= DefaultLimit);
            Assert.IsTrue(all.Count() == 5);
        }

        [Test]
        public void RunPerformanceTest()
        {
            // Arrange
            var userManager = new UserManager();
            BigInit(userManager);

            // Act
            var users = userManager.Find("22", DefaultLimit);
            var maxUsers = userManager.Find("22", MaxLimit);
            
            // Assert
            Assert.IsTrue(users.Count() == DefaultLimit);
            Assert.IsTrue(maxUsers.Count() == 3691);
        }

        private void BigInit(UserManager userManager)
        {
            for (var id = 1; id < MaxLimit; id++)
            {
                var user = new User
                {
                    Id = id,
                    Email = $"ivanov{id}@mail.ru",
                    Login = $"ivanov{id}",
                    Phone = "+7 900 0000"
                };
                userManager.Add(user);
            }
        }

        private void LittleInit(UserManager userManager)
        {
            var lastId = 1;
            var user = new User
            {
                Id = lastId++,
                Email = "ivanov@mail.ru",
                Login = "ivanov",
                Phone = "+7 900 0000"
            };
            userManager.Add(user);

            user = new User
            {
                Id = lastId++,
                Email = "petrov@mail.ru",
                Login = "petrov",
                Phone = "+7 911 0000"
            };
            userManager.Add(user);

            user = new User
            {
                Id = lastId++,
                Email = "sergeev@mail.ru",
                Login = "sergeev",
                Phone = "+7 922 0000"
            };
            userManager.Add(user);

            user = new User
            {
                Id = lastId++,
                Email = "pavlov@mail.ru",
                Login = "pavlov",
                Phone = "+7 933 0000"
            };
            userManager.Add(user);

            user = new User
            {
                Id = lastId++,
                Email = "mrFreeman@mail.ru",
                Login = "mrFreeman",
                Phone = "+7 944 0000"
            };
            userManager.Add(user);
        }
    }
}
