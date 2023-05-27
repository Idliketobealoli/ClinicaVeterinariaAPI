using ClinicaVeterinaria.API.Api.dto;
using ClinicaVeterinaria.API.Api.errors;
using ClinicaVeterinaria.API.Api.model;
using ClinicaVeterinaria.API.Api.repositories;
using ClinicaVeterinaria.API.Api.services;
using Moq;

namespace ClinicaVeterinaria.TEST.Api.services
{
    [TestClass]
    public class AppointmentServiceTest
    {
        private Mock<AppointmentRepository> AppointmentRepo;
        private Mock<PetRepository> PetRepo;
        private Mock<UserRepository> UserRepo;
        private Mock<VetRepository> VetRepo;
        private AppointmentService Service;
        private List<Appointment> ListAppointments;
        private List<AppointmentDTO> ListDTO;
        private Appointment EntityAppointment;
        private AppointmentDTO DTO;
        private UserDTOshort UserDto;
        private User UserTest;
        private PetDTOshort PetDtO;
        private Pet PetTest;
        private VetDTOappointment VetDtO;
        private Vet VetTest;

        [TestInitialize]
        public void Init()
        {
            AppointmentRepo = new Mock<AppointmentRepository>();
            PetRepo = new Mock<PetRepository>();
            UserRepo = new Mock<UserRepository>();
            VetRepo = new Mock<VetRepository>();
            Service = new AppointmentService(AppointmentRepo.Object, PetRepo.Object, UserRepo.Object, VetRepo.Object);
            EntityAppointment = new Appointment("prueba@prueba.com", DateTime.Now, DateTime.Now.AddHours(1),
                Guid.Parse("7e2809eb-a756-4515-9646-aca4d58f6a01"), "Dato", "vet@vet.com");
            UserDto = new("Sebastian", "Mendoza");
            UserTest = new("Sebastian", "Mendoza", "sebs@mendoza.com", "000000000", "prueba");
            PetDtO = new(Guid.Parse("84ee5eff-afee-4c61-b835-3574af2d5c60"), "Danko", "Labrador", "Perro", "MALE");
            PetTest = new(Guid.Parse("e214c069-4b3e-4b8b-9179-bb6428ed3e52"), "Danko", "Labrador", "Perro", 50, 10.3,
                Sex.MALE, DateOnly.Parse("2004-10-15"), "sebs@mendoza.com");
            VetDtO = new("Daniel", "Rodriguez", "daro@mail.com");
            VetTest = new("Daniel", "Rodriguez", "daro@mail.com", "000000000", "prueba", Role.VET, "Animales exótico");
            DTO = new(UserDto, DateTime.Now, DateTime.Now, PetDtO, "Dato", State.PENDING, VetDtO);
            ListAppointments = new List<Appointment>() { EntityAppointment };
            ListDTO = new List<AppointmentDTO>() { DTO };
        }

        [TestMethod]
        public void FindAllOk()
        {
            AppointmentRepo.Setup(x => x.FindAll()).ReturnsAsync(ListAppointments, new TimeSpan(100));
            PetRepo.Setup(x => x.FindById(It.IsAny<Guid>())).ReturnsAsync(PetTest, new TimeSpan(100));

            var res = Service.FindAll();
            res.Wait();

            Assert.IsNotNull(res.Result);
            CollectionAssert.AllItemsAreNotNull(res.Result);
            Assert.AreEqual(ListDTO.Count, res.Result.Count);
        }

        [TestMethod]
        public void FindAllNF()
        {
            AppointmentRepo.Setup(x => x.FindAll()).ReturnsAsync(new(), new TimeSpan(100));

            var res = Service.FindAll();
            res.Wait();

            Assert.IsNotNull(res.Result);
            Assert.AreEqual(new List<AppointmentDTO>().Count, res.Result.Count);
            CollectionAssert.AreEqual(new List<AppointmentDTO>(), res.Result);
        }

        [TestMethod]
        public void FindByIdOk()
        {
            AppointmentRepo.Setup(x => x.FindById(It.IsAny<Guid>())).ReturnsAsync(EntityAppointment, new TimeSpan(100));
            UserRepo.Setup(x => x.FindByEmail(It.IsAny<string>())).ReturnsAsync(UserTest, new TimeSpan(100));
            PetRepo.Setup(x => x.FindById(It.IsAny<Guid>())).ReturnsAsync(PetTest, new TimeSpan(100));
            VetRepo.Setup(x => x.FindByEmail(It.IsAny<string>())).ReturnsAsync(VetTest, new TimeSpan(100));

            var res = Service.FindById(EntityAppointment.Id);
            res.Wait();

            Assert.IsTrue(res.Result._isSuccess);
            Assert.IsNotNull(res.Result._successValue);
            Assert.IsNull(res.Result._errorValue);
            Assert.AreEqual(DTO.Pet.Name, res.Result._successValue.Pet.Name);
        }

        [TestMethod]
        public void FindByIdNF()
        {
            AppointmentRepo.Setup(x => x.FindById(It.IsAny<Guid>())).ReturnsAsync(null, new TimeSpan(100));
            var res = Service.FindById(EntityAppointment.Id);
            res.Wait();

            Assert.IsFalse(res.Result._isSuccess);
            Assert.IsNull(res.Result._successValue);
            Assert.IsNotNull(res.Result._errorValue);
            Assert.AreEqual($"Appointment with id {EntityAppointment.Id} not found.", res.Result._errorValue);
        }

        [TestMethod]
        public void FindByIdUserNF()
        {
            AppointmentRepo.Setup(x => x.FindById(It.IsAny<Guid>())).ReturnsAsync(EntityAppointment, new TimeSpan(100));
            UserRepo.Setup(x => x.FindByEmail(It.IsAny<string>())).ReturnsAsync(null, new TimeSpan(100));

            var res = Service.FindById(EntityAppointment.Id);
            res.Wait();

            Assert.IsFalse(res.Result._isSuccess);
            Assert.IsNull(res.Result._successValue);
            Assert.IsNotNull(res.Result._errorValue);
            Assert.AreEqual($"User with email {EntityAppointment.UserEmail} not found.", res.Result._errorValue);
        }

        [TestMethod]
        public void FindByIdPetNF()
        {
            AppointmentRepo.Setup(x => x.FindById(It.IsAny<Guid>())).ReturnsAsync(EntityAppointment, new TimeSpan(100));
            UserRepo.Setup(x => x.FindByEmail(It.IsAny<string>())).ReturnsAsync(UserTest, new TimeSpan(100));
            PetRepo.Setup(x => x.FindById(It.IsAny<Guid>())).ReturnsAsync(null, new TimeSpan(100));

            var res = Service.FindById(EntityAppointment.Id);
            res.Wait();

            Assert.IsFalse(res.Result._isSuccess);
            Assert.IsNull(res.Result._successValue);
            Assert.IsNotNull(res.Result._errorValue);
            Assert.AreEqual($"Pet with id {EntityAppointment.PetId} not found.", res.Result._errorValue);
        }

        [TestMethod]
        public void FindByIdVetNF()
        {
            AppointmentRepo.Setup(x => x.FindById(It.IsAny<Guid>())).ReturnsAsync(EntityAppointment, new TimeSpan(100));
            UserRepo.Setup(x => x.FindByEmail(It.IsAny<string>())).ReturnsAsync(UserTest, new TimeSpan(100));
            PetRepo.Setup(x => x.FindById(It.IsAny<Guid>())).ReturnsAsync(PetTest, new TimeSpan(100));
            VetRepo.Setup(x => x.FindByEmail(It.IsAny<string>())).ReturnsAsync(null, new TimeSpan(100));

            var res = Service.FindById(EntityAppointment.Id);
            res.Wait();

            Assert.IsFalse(res.Result._isSuccess);
            Assert.IsNull(res.Result._successValue);
            Assert.IsNotNull(res.Result._errorValue);
            Assert.AreEqual($"Vet with email {EntityAppointment.VetEmail} not found.", res.Result._errorValue);
        }

        [TestMethod]
        public void CreateOk()
        {
            UserRepo.Setup(x => x.FindByEmail(It.IsAny<string>())).ReturnsAsync(UserTest, new TimeSpan(100));
            VetRepo.Setup(x => x.FindByEmail(It.IsAny<string>())).ReturnsAsync(VetTest, new TimeSpan(100));
            AppointmentRepo.Setup(x => x.FindAll()).ReturnsAsync(new(), new TimeSpan(100));
            PetRepo.Setup(x => x.FindById(It.IsAny<Guid>())).ReturnsAsync(PetTest, new TimeSpan(100));
            AppointmentRepo.Setup(x => x.Create(It.IsAny<Appointment>()))
                .ReturnsAsync(EntityAppointment, new TimeSpan(100));

            var res = Service.Create(EntityAppointment);
            res.Wait();

            Assert.IsTrue(res.Result._isSuccess);
            Assert.IsNotNull(res.Result._successValue);
            Assert.IsNull(res.Result._errorValue);
            Assert.AreEqual(DTO.Pet.Name, res.Result._successValue.Pet.Name);
        }

        [TestMethod]
        public void CreateBadRequest()
        {
            UserRepo.Setup(x => x.FindByEmail(It.IsAny<string>())).ReturnsAsync(UserTest, new TimeSpan(100));
            VetRepo.Setup(x => x.FindByEmail(It.IsAny<string>())).ReturnsAsync(VetTest, new TimeSpan(100));
            AppointmentRepo.Setup(x => x.FindAll()).ReturnsAsync(ListAppointments, new TimeSpan(100));
            PetRepo.Setup(x => x.FindById(It.IsAny<Guid>())).ReturnsAsync(PetTest, new TimeSpan(100));
            AppointmentRepo.Setup(x => x.Create(It.IsAny<Appointment>()))
                .ReturnsAsync(EntityAppointment, new TimeSpan(100));

            var res = Service.Create(EntityAppointment);
            res.Wait();

            Assert.IsFalse(res.Result._isSuccess);
            Assert.IsNull(res.Result._successValue);
            Assert.IsNotNull(res.Result._errorValue);
            Assert.AreEqual("Incorrect data for the new appointment.", res.Result._errorValue.Message);
        }

        [TestMethod]
        public void DeleteOk()
        {
            AppointmentRepo.Setup(x => x.FindById(It.IsAny<Guid>())).ReturnsAsync(EntityAppointment, new TimeSpan(100));
            UserRepo.Setup(x => x.FindByEmail(It.IsAny<string>())).ReturnsAsync(UserTest, new TimeSpan(100));
            PetRepo.Setup(x => x.FindById(It.IsAny<Guid>())).ReturnsAsync(PetTest, new TimeSpan(100));
            VetRepo.Setup(x => x.FindByEmail(It.IsAny<string>())).ReturnsAsync(VetTest, new TimeSpan(100));
            AppointmentRepo.Setup(x => x.Delete(It.IsAny<Guid>())).ReturnsAsync(EntityAppointment, new TimeSpan(100));

            var res = Service.Delete(EntityAppointment.Id);
            res.Wait();

            Assert.IsTrue(res.Result._isSuccess);
            Assert.IsNotNull(res.Result._successValue);
            Assert.IsNull(res.Result._errorValue);
            Assert.AreEqual(DTO.Pet.Name, res.Result._successValue.Pet.Name);
        }

        [TestMethod]
        public void DeleteBadRequest()
        {
            AppointmentRepo.Setup(x => x.FindById(It.IsAny<Guid>())).ReturnsAsync(EntityAppointment, new TimeSpan(100));
            UserRepo.Setup(x => x.FindByEmail(It.IsAny<string>())).ReturnsAsync(UserTest, new TimeSpan(100));
            PetRepo.Setup(x => x.FindById(It.IsAny<Guid>())).ReturnsAsync(PetTest, new TimeSpan(100));
            VetRepo.Setup(x => x.FindByEmail(It.IsAny<string>())).ReturnsAsync(VetTest, new TimeSpan(100));
            AppointmentRepo.Setup(x => x.Delete(It.IsAny<Guid>())).ReturnsAsync(null, new TimeSpan(100));

            var res = Service.Delete(EntityAppointment.Id);
            res.Wait();

            Assert.IsFalse(res.Result._isSuccess);
            Assert.IsNull(res.Result._successValue);
            Assert.IsNotNull(res.Result._errorValue);
            Assert.AreEqual($"Could not delete Appointment with id {EntityAppointment.Id}.", res.Result._errorValue.Message);
        }

        [TestMethod]
        public void DeleteAppointmentNF()
        {
            AppointmentRepo.Setup(x => x.FindById(It.IsAny<Guid>())).ReturnsAsync(null, new TimeSpan(100));

            var res = Service.Delete(EntityAppointment.Id);
            res.Wait();

            Assert.IsFalse(res.Result._isSuccess);
            Assert.IsNull(res.Result._successValue);
            Assert.IsNotNull(res.Result._errorValue);
            Assert.AreEqual($"Appointment with id {EntityAppointment.Id} not found.", res.Result._errorValue.Message);
        }

        [TestMethod]
        public void DeleteUserNF()
        {
            AppointmentRepo.Setup(x => x.FindById(It.IsAny<Guid>())).ReturnsAsync(EntityAppointment, new TimeSpan(100));
            UserRepo.Setup(x => x.FindByEmail(It.IsAny<string>())).ReturnsAsync(null, new TimeSpan(100));
            PetRepo.Setup(x => x.FindById(It.IsAny<Guid>())).ReturnsAsync(null, new TimeSpan(100));
            VetRepo.Setup(x => x.FindByEmail(It.IsAny<string>())).ReturnsAsync(null, new TimeSpan(100));

            var res = Service.Delete(EntityAppointment.Id);
            res.Wait();

            Assert.IsFalse(res.Result._isSuccess);
            Assert.IsNull(res.Result._successValue);
            Assert.IsNotNull(res.Result._errorValue);
            Assert.AreEqual($"User with email {EntityAppointment.UserEmail} not found.", res.Result._errorValue.Message);
        }

        [TestMethod]
        public void DeletePetNF()
        {
            AppointmentRepo.Setup(x => x.FindById(It.IsAny<Guid>())).ReturnsAsync(EntityAppointment, new TimeSpan(100));
            UserRepo.Setup(x => x.FindByEmail(It.IsAny<string>())).ReturnsAsync(UserTest, new TimeSpan(100));
            PetRepo.Setup(x => x.FindById(It.IsAny<Guid>())).ReturnsAsync(null, new TimeSpan(100));
            VetRepo.Setup(x => x.FindByEmail(It.IsAny<string>())).ReturnsAsync(null, new TimeSpan(100));

            var res = Service.Delete(EntityAppointment.Id);
            res.Wait();

            Assert.IsFalse(res.Result._isSuccess);
            Assert.IsNull(res.Result._successValue);
            Assert.IsNotNull(res.Result._errorValue);
            Assert.AreEqual($"Pet with id {EntityAppointment.PetId} not found.", res.Result._errorValue.Message);
        }

        [TestMethod]
        public void DeleteVetNF()
        {
            AppointmentRepo.Setup(x => x.FindById(It.IsAny<Guid>())).ReturnsAsync(EntityAppointment, new TimeSpan(100));
            UserRepo.Setup(x => x.FindByEmail(It.IsAny<string>())).ReturnsAsync(UserTest, new TimeSpan(100));
            PetRepo.Setup(x => x.FindById(It.IsAny<Guid>())).ReturnsAsync(PetTest, new TimeSpan(100));
            VetRepo.Setup(x => x.FindByEmail(It.IsAny<string>())).ReturnsAsync(null, new TimeSpan(100));

            var res = Service.Delete(EntityAppointment.Id);
            res.Wait();

            Assert.IsFalse(res.Result._isSuccess);
            Assert.IsNull(res.Result._successValue);
            Assert.IsNotNull(res.Result._errorValue);
            Assert.AreEqual($"Vet with email {EntityAppointment.VetEmail} not found.", res.Result._errorValue.Message);
        }
    }
}