using ClinicaVeterinaria.API.Api.dto;
using ClinicaVeterinaria.API.Api.errors;
using ClinicaVeterinaria.API.Api.model;
using ClinicaVeterinaria.API.Api.repositories;
using ClinicaVeterinaria.API.Api.services;
using ClinicaVeterinaria.API.Api.services.bcrypt;
using Moq;

namespace ClinicaVeterinaria.TEST.Api.services
{
    [TestClass]
    public class VetServiceTest
    {
        private Mock<VetRepository> Repo;
        private Mock<AppointmentRepository> ARepo;
        private VetService Service;
        private List<Vet> List;
        private List<VetDTO> ListDTO;
        private Vet Entity;
        private Vet EntityLogin;
        private VetDTO DTO;
        private VetDTOshort DTOShort;
        private VetDTOappointment DTOappointment;
        private VetDTOandToken DTOandToken;
        private VetDTOregister DTOregister;
        private VetDTOloginOrChangePassword DTOupdate;
        private VetDTOloginOrChangePassword DTOlogin;

        [TestInitialize]
        public void Init()
        {
            Repo = new Mock<VetRepository>();
            ARepo = new Mock<AppointmentRepository>();
            Service = new(Repo.Object, ARepo.Object);
            Entity = new(
                "test", "testeado", "uwu@gmail.com",
                "123456789", "uwu1234", Role.VET, "qwerty", true);
            EntityLogin = new(
                "test", "testeado", "uwu@gmail.com",
                "123456789", CipherService.Encode("uwu1234"), Role.VET, "qwerty", true);
            DTO = new(
                "test", "testeado", "uwu@gmail.com",
                "123456789", Role.VET, "qwerty", true);
            DTOShort = new("test", "testeado");
            DTOandToken = new(DTO, "");
            List = new List<Vet>() { Entity };
            ListDTO = new List<VetDTO>() { DTO };
            DTOregister = new(
                "test", "testeado2", "email2@gmail.com",
                "987654321", "uwu1234", "uwu1234",
                Roles.ToString(Role.VET), "qwerty");
            DTOupdate = new("uwu@gmail.com", "1234uwu");
            DTOlogin = new("uwu@gmail.com", "uwu1234");
            DTOappointment = new("test", "testeado2", "email2@gmail.com", true);
        }

        [TestMethod]
        public void FindAllOK()
        {
            Repo.Setup(x => x.FindAll()).ReturnsAsync(List, new TimeSpan(100));

            var res = Service.FindAll();
            res.Wait();

            Assert.IsNotNull(res.Result);
            CollectionAssert.AllItemsAreNotNull(res.Result);
            Assert.AreEqual(ListDTO.Count, res.Result.Count);
        }

        [TestMethod]
        public void FindAllNF()
        {
            Repo.Setup(x => x.FindAll()).ReturnsAsync(new(), new TimeSpan(100));

            var res = Service.FindAll();
            res.Wait();

            Assert.IsNotNull(res.Result);
            CollectionAssert.AllItemsAreNotNull(res.Result);
            Assert.AreEqual(new List<UserDTO>().Count, res.Result.Count);
            CollectionAssert.AreEqual(new List<UserDTO>(), res.Result);
        }

        [TestMethod]
        public void FindByEmailOk()
        {
            Repo.Setup(x => x.FindByEmail(It.IsAny<string>())).ReturnsAsync(Entity, new TimeSpan(100));

            var res = Service.FindByEmail("uwu@gmail.com");
            res.Wait();

            Assert.IsTrue(res.Result._isSuccess);
            Assert.IsNotNull(res.Result._successValue);
            Assert.IsNull(res.Result._errorValue);
            Assert.AreEqual(DTO.Name, res.Result._successValue.Name);
        }

        [TestMethod]
        public void FindByEmailNF()
        {
            Repo.Setup(x => x.FindByEmail(It.IsAny<string>())).ReturnsAsync(null, new TimeSpan(100));

            var res = Service.FindByEmail("uwu@gmail.com");
            res.Wait();

            Assert.IsFalse(res.Result._isSuccess);
            Assert.IsNull(res.Result._successValue);
            Assert.IsNotNull(res.Result._errorValue);
            Assert.AreEqual
                ($"Vet with email uwu@gmail.com not found.", res.Result._errorValue);
        }

        [TestMethod]
        public void FindByEmailShortOk()
        {
            Repo.Setup(x => x.FindByEmail(It.IsAny<string>())).ReturnsAsync(Entity, new TimeSpan(100));

            var res = Service.FindByEmailShort("uwu@gmail.com");
            res.Wait();

            Assert.IsTrue(res.Result._isSuccess);
            Assert.IsNotNull(res.Result._successValue);
            Assert.IsNull(res.Result._errorValue);
            Assert.AreEqual(DTOShort.Name, res.Result._successValue.Name);
        }

        [TestMethod]
        public void FindByEmailShortNF()
        {
            Repo.Setup(x => x.FindByEmail(It.IsAny<string>())).ReturnsAsync(null, new TimeSpan(100));

            var res = Service.FindByEmailShort("uwu@gmail.com");
            res.Wait();

            Assert.IsFalse(res.Result._isSuccess);
            Assert.IsNull(res.Result._successValue);
            Assert.IsNotNull(res.Result._errorValue);
            Assert.AreEqual(
                $"Vet with email uwu@gmail.com not found.", res.Result._errorValue);
        }

        [TestMethod]
        public void FindByEmailAppointmentOk()
        {
            Repo.Setup(x => x.FindByEmail(It.IsAny<string>())).ReturnsAsync(Entity, new TimeSpan(100));

            var res = Service.FindByEmailAppointment("uwu@gmail.com");
            res.Wait();

            Assert.IsTrue(res.Result._isSuccess);
            Assert.IsNotNull(res.Result._successValue);
            Assert.IsNull(res.Result._errorValue);
            Assert.AreEqual(DTOappointment.Name, res.Result._successValue.Name);
        }

        [TestMethod]
        public void FindByEmailAppointmentNF()
        {
            Repo.Setup(x => x.FindByEmail(It.IsAny<string>())).ReturnsAsync(null, new TimeSpan(100));

            var res = Service.FindByEmailAppointment("uwu@gmail.com");
            res.Wait();

            Assert.IsFalse(res.Result._isSuccess);
            Assert.IsNull(res.Result._successValue);
            Assert.IsNotNull(res.Result._errorValue);
            Assert.AreEqual($"Vet with email uwu@gmail.com not found.", res.Result._errorValue);
        }

        [TestMethod]
        public void RegisterOk()
        {
            Repo.Setup(x => x.FindByEmail(It.IsAny<string>())).ReturnsAsync(null, new TimeSpan(100));
            Repo.Setup(x => x.FindBySSNum(It.IsAny<string>())).ReturnsAsync(null, new TimeSpan(100));
            Repo.Setup(x => x.Create(It.IsAny<Vet>())).ReturnsAsync(Entity, new TimeSpan(100));

            var res = Service.Register(DTOregister, null);
            res.Wait();

            Assert.IsTrue(res.Result._isSuccess);
            Assert.IsNotNull(res.Result._successValue);
            Assert.IsNull(res.Result._errorValue);
            Assert.AreEqual(DTOandToken.Token, res.Result._successValue.Token);
        }

        [TestMethod]
        public void RegisterError()
        {
            Repo.Setup(x => x.FindByEmail(It.IsAny<string>())).ReturnsAsync(Entity, new TimeSpan(100));
            Repo.Setup(x => x.FindBySSNum(It.IsAny<string>())).ReturnsAsync(Entity, new TimeSpan(100));

            var res = Service.Register(DTOregister, null);
            res.Wait();

            Assert.IsFalse(res.Result._isSuccess);
            Assert.IsNull(res.Result._successValue);
            Assert.IsNotNull(res.Result._errorValue);
            Assert.AreEqual(
                "Cannot use either that email or that Social Security number.", res.Result._errorValue);
        }

        [TestMethod]
        public void LoginOk()
        {
            Repo.Setup(x => x.FindByEmail(It.IsAny<string>())).ReturnsAsync(EntityLogin, new TimeSpan(100));

            var res = Service.Login(DTOlogin, null);
            res.Wait();

            Assert.IsTrue(res.Result._isSuccess);
            Assert.IsNotNull(res.Result._successValue);
            Assert.IsNull(res.Result._errorValue);
            Assert.AreEqual(DTOandToken.Token, res.Result._successValue.Token);
        }

        [TestMethod]
        public void LoginError()
        {
            Repo.Setup(x => x.FindByEmail(It.IsAny<string>())).ReturnsAsync(null, new TimeSpan(100));

            var res = Service.Login(DTOlogin, null);
            res.Wait();

            Assert.IsFalse(res.Result._isSuccess);
            Assert.IsNull(res.Result._successValue);
            Assert.IsNotNull(res.Result._errorValue);
            Assert.AreEqual("Incorrect email or password.", res.Result._errorValue);
        }

        [TestMethod]
        public void ChangePasswordOk()
        {
            Repo.Setup(x => x.UpdatePassword(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(Entity, new TimeSpan(100));

            var res = Service.ChangePassword(DTOupdate);
            res.Wait();

            Assert.IsTrue(res.Result._isSuccess);
            Assert.IsNotNull(res.Result._successValue);
            Assert.IsNull(res.Result._errorValue);
            Assert.AreEqual(DTO.Name, res.Result._successValue.Name);
        }

        [TestMethod]
        public void ChangePasswordError()
        {
            Repo.Setup(x => x.UpdatePassword(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(null, new TimeSpan(100));

            var res = Service.ChangePassword(DTOupdate);
            res.Wait();

            Assert.IsFalse(res.Result._isSuccess);
            Assert.IsNull(res.Result._successValue);
            Assert.IsNotNull(res.Result._errorValue);
            Assert.AreEqual($"Vet with email {DTOupdate.Email} not found.", res.Result._errorValue);
        }

        [TestMethod]
        public void DeleteOk()
        {
            Repo.Setup(x => x.SwitchActivity(It.IsAny<string>())).ReturnsAsync(Entity, new TimeSpan(100));

            var res = Service.Delete("uwu@gmail.com", false);
            res.Wait();

            Assert.IsTrue(res.Result._isSuccess);
            Assert.IsNotNull(res.Result._successValue);
            Assert.IsNull(res.Result._errorValue);
            Assert.AreEqual(DTO.Name, res.Result._successValue.Name);
        }

        [TestMethod]
        public void DeleteError()
        {
            Repo.Setup(x => x.SwitchActivity(It.IsAny<string>())).ReturnsAsync(null, new TimeSpan(100));

            var res = Service.Delete("uwu@gmail.com", false);
            res.Wait();

            Assert.IsFalse(res.Result._isSuccess);
            Assert.IsNull(res.Result._successValue);
            Assert.IsNotNull(res.Result._errorValue);
            Assert.AreEqual($"Vet with email uwu@gmail.com not found.", res.Result._errorValue);
        }
    }
}
