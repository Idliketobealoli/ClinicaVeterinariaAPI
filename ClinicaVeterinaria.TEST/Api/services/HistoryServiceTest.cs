using ClinicaVeterinaria.API.Api.dto;
using ClinicaVeterinaria.API.Api.model;
using ClinicaVeterinaria.API.Api.repositories;
using ClinicaVeterinaria.API.Api.services;
using Moq;

namespace ClinicaVeterinaria.TEST.Api.services
{
    [TestClass]
    public class HistoryServiceTest
    {
        private Mock<HistoryRepository> HistoryRepo;
        private Mock<VaccineRepository> VaccineRepo;
        private Mock<AilmentTreatmentRepository> AilmentRepo;
        private HistoryService Service;
        private List<History> ListHistory;
        private List<HistoryDTO> ListDTO;
        private HashSet<VaccineDTO> VaccinesSet;
        private HashSet<AilmentTreatmentDTO> ailmentTreatmentSet;
        private VaccineDTO Vaccine;
        private AilmentTreatmentDTO AT;
        private History EntityHistory;
        private HistoryDTO DTO;

        [TestInitialize]
        public void Init()
        {
            HistoryRepo = new Mock<HistoryRepository>();
            VaccineRepo = new Mock<VaccineRepository>();
            AilmentRepo = new Mock<AilmentTreatmentRepository>();
            Service = new(HistoryRepo.Object, VaccineRepo.Object, AilmentRepo.Object);
            EntityHistory = new(
                Guid.Parse("d24e89f2-c97a-4e0d-ab07-f392a8ea5fd4"));
            Vaccine = new("Vacuna1", DateOnly.FromDateTime(DateTime.Now).ToString());
            VaccinesSet = new HashSet<VaccineDTO>() { Vaccine };
            AT = new("Diabetes", "Inyecciones de insulina");
            ailmentTreatmentSet = new HashSet<AilmentTreatmentDTO>() { AT };
            DTO = new(
                Guid.Parse("d24e89f2-c97a-4e0d-ab07-f392a8ea5fd4"), VaccinesSet, ailmentTreatmentSet);
            ListHistory = new List<History>() { EntityHistory };
            ListDTO = new List<HistoryDTO>() { DTO };
        }

        [TestMethod]
        public void FindAllOk()
        {
            HistoryRepo.Setup(x => x.FindAll()).ReturnsAsync(ListHistory, new TimeSpan(100));

            var res = Service.FindAll();
            res.Wait();

            Assert.IsNotNull(res.Result);
            CollectionAssert.AllItemsAreUnique(res.Result);
            Assert.AreEqual(ListDTO.Count, res.Result.Count);
        }

        [TestMethod]
        public void FindAllNF()
        {
            HistoryRepo.Setup(x => x.FindAll()).ReturnsAsync(new(), new TimeSpan(100));

            var res = Service.FindAll();
            res.Wait();

            Assert.IsNotNull(res.Result);
            CollectionAssert.AllItemsAreNotNull(res.Result);
            Assert.AreEqual(new List<HistoryDTO>().Count, res.Result.Count);
            CollectionAssert.AreEqual(new List<HistoryDTO>(), res.Result);
        }

        [TestMethod]
        public void FindByPetIdOk()
        {
            HistoryRepo.Setup(x => x.FindByPetId(It.IsAny<Guid>())).ReturnsAsync(EntityHistory, new TimeSpan(100));

            var res = Service.FindByPetId(Guid.Parse("d24e89f2-c97a-4e0d-ab07-f392a8ea5fd4"));
            res.Wait();

            Assert.IsTrue(res.Result._isSuccess);
            Assert.IsNotNull(res.Result._successValue);
            Assert.IsNull(res.Result._errorValue);
            Assert.AreEqual(DTO.PetId, res.Result._successValue.PetId);
        }

        [TestMethod]
        public void FindByPetIdNF()
        {
            HistoryRepo.Setup(x => x.FindByPetId(It.IsAny<Guid>())).ReturnsAsync(null, new TimeSpan(100));

            var res = Service.FindByPetId(Guid.Empty);
            res.Wait();

            Assert.IsFalse(res.Result._isSuccess);
            Assert.IsNull(res.Result._successValue);
            Assert.IsNotNull(res.Result._errorValue);
            Assert.AreEqual("History with PetId 00000000-0000-0000-0000-000000000000 not found.", res.Result._errorValue);
        }

        [TestMethod]
        public void FindByPetIdVaccinesOnlyOk()
        {
            HistoryRepo.Setup(x => x.FindByPetId(It.IsAny<Guid>())).ReturnsAsync(EntityHistory, new TimeSpan(100));

            var res = Service.FindByPetIdVaccinesOnly(Guid.Parse("d24e89f2-c97a-4e0d-ab07-f392a8ea5fd4"));
            res.Wait();

            Assert.IsTrue(res.Result._isSuccess);
            Assert.IsNotNull(res.Result._successValue);
            Assert.IsNull(res.Result._errorValue);
        }

        [TestMethod]
        public void FindByPetIdVaccinesOnlyNF()
        {
            HistoryRepo.Setup(x => x.FindByPetId(It.IsAny<Guid>())).ReturnsAsync(null, new TimeSpan(100));

            var res = Service.FindByPetIdVaccinesOnly(Guid.Empty);
            res.Wait();

            Assert.IsFalse(res.Result._isSuccess);
            Assert.IsNull(res.Result._successValue);
            Assert.IsNotNull(res.Result._errorValue);
            Assert.AreEqual("History with PetId 00000000-0000-0000-0000-000000000000 not found.", res.Result._errorValue);
        }

        [TestMethod]
        public void FindByPetIdAilmTreatOnlyOk()
        {
            HistoryRepo.Setup(x => x.FindByPetId(It.IsAny<Guid>())).ReturnsAsync(EntityHistory, new TimeSpan(100));

            var res = Service.FindByPetIdAilmTreatOnly(Guid.Parse("d24e89f2-c97a-4e0d-ab07-f392a8ea5fd4"));
            res.Wait();

            Assert.IsTrue(res.Result._isSuccess);
            Assert.IsNotNull(res.Result._successValue);
            Assert.IsNull(res.Result._errorValue);
        }

        [TestMethod]
        public void FindByPetIdAilmTreatOnlyNF()
        {
            HistoryRepo.Setup(x => x.FindByPetId(It.IsAny<Guid>())).ReturnsAsync(null, new TimeSpan(100));

            var res = Service.FindByPetIdAilmTreatOnly(Guid.Empty);
            res.Wait();

            Assert.IsFalse(res.Result._isSuccess);
            Assert.IsNull(res.Result._successValue);
            Assert.IsNotNull(res.Result._errorValue);
            Assert.AreEqual("History with PetId 00000000-0000-0000-0000-000000000000 not found.", res.Result._errorValue);
        }

        [TestMethod]
        public void AddVaccineOk()
        {
            HistoryRepo.Setup(x => x.FindByPetId(It.IsAny<Guid>())).ReturnsAsync(EntityHistory, new TimeSpan(100));

            var res = Service.AddVaccine(Guid.NewGuid(),
                new VaccineDTO("VacunaAdd", DateOnly.FromDateTime(DateTime.Now).ToString()));

            Assert.IsTrue(res.Result._isSuccess);
            Assert.IsNotNull(res.Result._successValue);
            Assert.IsNull(res.Result._errorValue);
        }

        [TestMethod]
        public void AddVaccineError()
        {
            HistoryRepo.Setup(x => x.FindByPetId(It.IsAny<Guid>())).ReturnsAsync(null, new TimeSpan(100));

            var res = Service.AddVaccine(Guid.Empty,
                new VaccineDTO("VacunaAdd", DateOnly.FromDateTime(DateTime.Now).ToString()));

            Assert.IsFalse(res.Result._isSuccess);
            Assert.IsNull(res.Result._successValue);
            Assert.IsNotNull(res.Result._errorValue);
            Assert.AreEqual("History with PetId 00000000-0000-0000-0000-000000000000 not found.", res.Result._errorValue);
        }

        [TestMethod]
        public void AddAilmentTreatmentOk()
        {
            HistoryRepo.Setup(x => x.FindByPetId(It.IsAny<Guid>())).ReturnsAsync(EntityHistory, new TimeSpan(100));

            var res = Service.AddAilmentTreatment(Guid.NewGuid(), AT);

            Assert.IsTrue(res.Result._isSuccess);
            Assert.IsNotNull(res.Result._successValue);
            Assert.IsNull(res.Result._errorValue);
        }

        [TestMethod]
        public void AddAilmentTreatmentError()
        {
            HistoryRepo.Setup(x => x.FindByPetId(It.IsAny<Guid>())).ReturnsAsync(null, new TimeSpan(100));

            var res = Service.AddAilmentTreatment(Guid.Empty, AT);

            Assert.IsFalse(res.Result._isSuccess);
            Assert.IsNull(res.Result._successValue);
            Assert.IsNotNull(res.Result._errorValue);
            Assert.AreEqual("History with PetId 00000000-0000-0000-0000-000000000000 not found.", res.Result._errorValue);
        }
    }
}