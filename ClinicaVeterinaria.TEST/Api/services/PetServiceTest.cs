using ClinicaVeterinaria.API.Api.dto;
using ClinicaVeterinaria.API.Api.errors;
using ClinicaVeterinaria.API.Api.model;
using ClinicaVeterinaria.API.Api.repositories;
using ClinicaVeterinaria.API.Api.services;
using Moq;

namespace ClinicaVeterinaria.TEST.Api.services
{
    [TestClass]
    public class PServiceTest
    {
        private Mock<PetRepository> Repo;
        private Mock<UserRepository> URepo;
        private Mock<HistoryRepository> HRepo;
        private Mock<VaccineRepository> VRepo;
        private Mock<AilmentTreatmentRepository> ARepo;
        private PetService Service;
        private List<Pet> List;
        private List<PetDTO> ListDTO;
        private Pet Entity;
        private PetDTO DTO;
        private PetDTOcreate DTOcreate;
        private PetDTOupdate DTOupdate;
        private History History;
        private HistoryDTO HistoryDTO;
        private User User;
        private UserDTOshort UserDTO;
        private Vaccine Vaccine;

        [TestInitialize]
        public void Init()
        {
            Repo = new Mock<PetRepository>();
            URepo = new Mock<UserRepository>();
            HRepo = new Mock<HistoryRepository>();
            VRepo = new Mock<VaccineRepository>();
            ARepo = new Mock<AilmentTreatmentRepository>();
            Service = new(Repo.Object, URepo.Object, HRepo.Object, VRepo.Object, ARepo.Object);
            Entity = new(
                Guid.NewGuid(), "test", "testeado", "cat",
                10.0, 15.0, Sex.FEMALE, DateOnly.FromDateTime(DateTime.Now), "uwu@gmail.com", true);
            History = new(Entity.Id);
            HistoryDTO = new(Entity.Id, new(), new());
            User = new("owner", "test", "uwu@gmail.com", "y", "z", true);
            UserDTO = new("owner", "test");
            DTO = new(
                Entity.Id, "test", "testeado", "cat",
                "FEMALE", DateOnly.FromDateTime(DateTime.Now).ToString(), 10.0, 15.0, HistoryDTO, UserDTO, true);
            DTOcreate = new(
                "test", "testeado", "cat",
                10.0, 15.0, "FEMALE", DateOnly.FromDateTime(DateTime.Now).ToString(), "uwu@gmail.com");
            DTOupdate = new(Entity.Id, "test", 2.0, 3.0);
            List = new List<Pet>() { Entity };
            ListDTO = new List<PetDTO>() { DTO };
            Vaccine = new(Entity.Id, "qwerty", DateOnly.FromDateTime(DateTime.Now));
        }

        [TestMethod]
        public void FindAllOK()
        {
            Repo.Setup(x => x.FindAll()).ReturnsAsync(List, new TimeSpan(100));

            var res = Service.FindAll(null);
            res.Wait();

            Assert.IsNotNull(res.Result);
            CollectionAssert.AllItemsAreNotNull(res.Result);
            Assert.AreEqual(ListDTO.Count, res.Result.Count);
        }

        [TestMethod]
        public void FindAllNF()
        {
            Repo.Setup(x => x.FindAll()).ReturnsAsync(new(), new TimeSpan(100));

            var res = Service.FindAll(null);
            res.Wait();

            Assert.IsNotNull(res.Result);
            CollectionAssert.AllItemsAreNotNull(res.Result);
            Assert.AreEqual(new List<UserDTO>().Count, res.Result.Count);
            CollectionAssert.AreEqual(new List<UserDTO>(), res.Result);
        }

        [TestMethod]
        public void FindByIdOk()
        {
            Repo.Setup(x => x.FindById(It.IsAny<Guid>())).ReturnsAsync(Entity, new TimeSpan(100));
            URepo.Setup(x => x.FindByEmail(It.IsAny<string>())).ReturnsAsync(User, new TimeSpan(100));

            var res = Service.FindById(Entity.Id);
            res.Wait();

            Assert.IsTrue(res.Result._isSuccess);
            Assert.IsNotNull(res.Result._successValue);
            Assert.IsNull(res.Result._errorValue);
            Assert.AreEqual(DTO.Name, res.Result._successValue.Name);
        }

        [TestMethod]
        public void FindByIdNF()
        {
            Repo.Setup(x => x.FindById(It.IsAny<Guid>())).ReturnsAsync(null, new TimeSpan(100));

            var res = Service.FindById(Entity.Id);
            res.Wait();

            Assert.IsFalse(res.Result._isSuccess);
            Assert.IsNull(res.Result._successValue);
            Assert.IsNotNull(res.Result._errorValue);
            Assert.AreEqual($"Pet with id {Entity.Id} not found.", res.Result._errorValue);
        }

        [TestMethod]
        public void FindByIdOwnerNF()
        {
            Repo.Setup(x => x.FindById(It.IsAny<Guid>())).ReturnsAsync(Entity, new TimeSpan(100));
            URepo.Setup(x => x.FindByEmail(It.IsAny<string>())).ReturnsAsync(null, new TimeSpan(100));

            var res = Service.FindById(Entity.Id);
            res.Wait();

            Assert.IsFalse(res.Result._isSuccess);
            Assert.IsNull(res.Result._successValue);
            Assert.IsNotNull(res.Result._errorValue);
            Assert.AreEqual($"User with email {Entity.OwnerEmail} not found.", res.Result._errorValue);
        }

        [TestMethod]
        public void CreateOk()
        {
            URepo.Setup(x => x.FindByEmail(It.IsAny<string>())).ReturnsAsync(User, new TimeSpan(100));
            Repo.Setup(x => x.Create(It.IsAny<Pet>())).ReturnsAsync(Entity, new TimeSpan(100));
            HRepo.Setup(x => x.Create(It.IsAny<History>())).ReturnsAsync(History, new TimeSpan(100));

            var res = Service.Create(DTOcreate);
            res.Wait();

            Assert.IsTrue(res.Result._isSuccess);
            Assert.IsNotNull(res.Result._successValue);
            Assert.IsNull(res.Result._errorValue);
            Assert.AreEqual(DTO.Name, res.Result._successValue.Name);
        }

        [TestMethod]
        public void CreateError()
        {
            URepo.Setup(x => x.FindByEmail(It.IsAny<string>())).ReturnsAsync(null, new TimeSpan(100));

            var res = Service.Create(DTOcreate);
            res.Wait();

            Assert.IsFalse(res.Result._isSuccess);
            Assert.IsNull(res.Result._successValue);
            Assert.IsNotNull(res.Result._errorValue);
            Assert.AreEqual($"Owner with email {Entity.OwnerEmail} not found.",res.Result._errorValue.Message);
        }

        [TestMethod]
        public void UpdateOk()
        {
            Repo.Setup(x => x.Update(It.IsAny<PetDTOupdate>())).ReturnsAsync(Entity, new TimeSpan(100));
            URepo.Setup(x => x.FindByEmail(It.IsAny<string>())).ReturnsAsync(User, new TimeSpan(100));

            var res = Service.Update(DTOupdate);
            res.Wait();

            Assert.IsTrue(res.Result._isSuccess);
            Assert.IsNotNull(res.Result._successValue);
            Assert.IsNull(res.Result._errorValue);
            Assert.AreEqual(DTO.Name, res.Result._successValue.Name);
        }

        [TestMethod]
        public void UpdateError()
        {
            Repo.Setup(x => x.Update(It.IsAny<PetDTOupdate>())).ReturnsAsync(null, new TimeSpan(100));

            var res = Service.Update(DTOupdate);
            res.Wait();

            Assert.IsFalse(res.Result._isSuccess);
            Assert.IsNull(res.Result._successValue);
            Assert.IsNotNull(res.Result._errorValue);
            Assert.AreEqual($"Pet with id {DTOupdate.Id} not found.",res.Result._errorValue);
        }

        [TestMethod]
        public void UpdateError2()
        {
            Repo.Setup(x => x.Update(It.IsAny<PetDTOupdate>())).ReturnsAsync(Entity, new TimeSpan(100));
            URepo.Setup(x => x.FindByEmail(It.IsAny<string>())).ReturnsAsync(null, new TimeSpan(100));

            var res = Service.Update(DTOupdate);
            res.Wait();

            Assert.IsFalse(res.Result._isSuccess);
            Assert.IsNull(res.Result._successValue);
            Assert.IsNotNull(res.Result._errorValue);
            Assert.AreEqual($"User with email {Entity.OwnerEmail} not found.", res.Result._errorValue);
        }

        [TestMethod]
        public void DeleteOk()
        {
            Repo.Setup(x => x.FindById(It.IsAny<Guid>())).ReturnsAsync(Entity, new TimeSpan(100));
            URepo.Setup(x => x.FindByEmail(It.IsAny<string>())).ReturnsAsync(User, new TimeSpan(100));
            VRepo.Setup(x => x.FindAll()).ReturnsAsync(new List<Vaccine>() { Vaccine }, new TimeSpan(100));
            VRepo.Setup(x => x.Delete(It.IsAny<Guid>())).ReturnsAsync(Vaccine, new TimeSpan(100));
            HRepo.Setup(x => x.FindByPetId(It.IsAny<Guid>())).ReturnsAsync(History, new TimeSpan(100));
            HRepo.Setup(x => x.Delete(It.IsAny<Guid>())).ReturnsAsync(History, new TimeSpan(100));
            Repo.Setup(x => x.Delete(It.IsAny<Guid>(), false)).ReturnsAsync(Entity, new TimeSpan(100));

            var res = Service.Delete(Entity.Id);
            res.Wait();

            Assert.IsTrue(res.Result._isSuccess);
            Assert.IsNotNull(res.Result._successValue);
            Assert.IsNull(res.Result._errorValue);
            Assert.AreEqual(DTO.Name, res.Result._successValue.Name);
        }

        [TestMethod]
        public void DeleteError()
        {
            Repo.Setup(x => x.FindById(It.IsAny<Guid>())).ReturnsAsync(null, new TimeSpan(100));

            var res = Service.Delete(Entity.Id);
            res.Wait();

            Assert.IsFalse(res.Result._isSuccess);
            Assert.IsNull(res.Result._successValue);
            Assert.IsNotNull(res.Result._errorValue);
            Assert.AreEqual(
                new PetErrorNotFound($"Pet with id {Entity.Id} not found.").Message,
                res.Result._errorValue.Message);
        }

        [TestMethod]
        public void DeleteError2()
        {
            Repo.Setup(x => x.FindById(It.IsAny<Guid>())).ReturnsAsync(Entity, new TimeSpan(100));
            URepo.Setup(x => x.FindByEmail(It.IsAny<string>())).ReturnsAsync(null, new TimeSpan(100));

            var res = Service.Delete(Entity.Id);
            res.Wait();

            Assert.IsFalse(res.Result._isSuccess);
            Assert.IsNull(res.Result._successValue);
            Assert.IsNotNull(res.Result._errorValue);
            Assert.AreEqual(
                new UserErrorNotFound($"User with email {Entity.OwnerEmail} not found.").Message,
                res.Result._errorValue.Message);
        }

        [TestMethod]
        public void DeleteError3()
        {
            Repo.Setup(x => x.FindById(It.IsAny<Guid>())).ReturnsAsync(Entity, new TimeSpan(100));
            URepo.Setup(x => x.FindByEmail(It.IsAny<string>())).ReturnsAsync(User, new TimeSpan(100));
            VRepo.Setup(x => x.FindAll()).ReturnsAsync(new List<Vaccine>() { Vaccine }, new TimeSpan(100));
            VRepo.Setup(x => x.Delete(It.IsAny<Guid>())).ReturnsAsync(Vaccine, new TimeSpan(100));
            HRepo.Setup(x => x.FindByPetId(It.IsAny<Guid>())).ReturnsAsync(History, new TimeSpan(100));
            HRepo.Setup(x => x.Delete(It.IsAny<Guid>())).ReturnsAsync(History, new TimeSpan(100));
            Repo.Setup(x => x.Delete(It.IsAny<Guid>(), false)).ReturnsAsync(null, new TimeSpan(100));

            var res = Service.Delete(Entity.Id);
            res.Wait();

            Assert.IsFalse(res.Result._isSuccess);
            Assert.IsNull(res.Result._successValue);
            Assert.IsNotNull(res.Result._errorValue);
            Assert.AreEqual(
                new PetErrorBadRequest($"Could not delete Pet with id {Entity.Id}.").Message,
                res.Result._errorValue.Message);
        }
    }
}
