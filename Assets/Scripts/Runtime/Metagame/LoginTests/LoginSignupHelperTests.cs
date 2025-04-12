using System.Threading.Tasks;
using NUnit.Framework;
using OnPar.Routers;
using Unity.Template.Multiplayer.NGO.Runtime;

namespace Unity.Template.Multiplayer.NGO.Tests
{
    public class LoginSignupHelperTests
    {
        [SetUp]
        public void Setup()
        {
            // Reset handlers to default implementations before each test
            LoginAttemptEvent.LoginHandler = (username, password) =>
                OnPar.RouterHandlers.Handlers.LoginHandler(username, password);

            SignupAttemptEvent.RegisterHandler = (username, password) =>
                OnPar.RouterHandlers.Handlers.RegisterHandler(username, password);
        }

        [Test]
        public async Task LoginHelper_ReturnsTrue_OnSuccessfulLogin()
        {
            LoginAttemptEvent.LoginHandler = (username, password) =>
            {
                return Task.FromResult(new LoginRegisterResponse { success = true });
            };

            bool result = await LoginAttemptEvent.LoginHelper("test", "1234");
            Assert.IsTrue(result);
        }

        [Test]
        public async Task LoginHelper_ReturnsFalse_OnFailedLogin()
        {
            LoginAttemptEvent.LoginHandler = (username, password) =>
            {
                return Task.FromResult<LoginRegisterResponse>(null);
            };

            bool result = await LoginAttemptEvent.LoginHelper("test", "wrong");
            Assert.IsFalse(result);
        }

        [Test]
        public async Task RegisterHelper_ReturnsTrue_OnSuccessfulRegistration()
        {
            SignupAttemptEvent.RegisterHandler = (username, password) =>
            {
                return Task.FromResult(new LoginRegisterResponse { success = true });
            };

            bool result = await SignupAttemptEvent.RegisterHelper("newUser", "pass");
            Assert.IsTrue(result);
        }

        [Test]
        public async Task RegisterHelper_ReturnsFalse_OnRegistrationFailure()
        {
            SignupAttemptEvent.RegisterHandler = (username, password) =>
            {
                return Task.FromResult<LoginRegisterResponse>(null);
            };

            bool result = await SignupAttemptEvent.RegisterHelper("newUser", "pass");
            Assert.IsFalse(result);
        }
    }
}
