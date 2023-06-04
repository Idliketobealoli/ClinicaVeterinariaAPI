namespace ClinicaVeterinaria.API.Api.dto
{
    public class PetDTOshort
    {
        public PetDTOshort
            (
            Guid id, string name,
            string race, string species, string sex
            )
        {
            Id = id;
            Name = name;
            Race = race;
            Species = species;
            Sex = sex;
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Race { get; set; }
        public string Species { get; set; }
        public string Sex { get; set; }
    }

    public class PetDTO
    {
        public PetDTO
            (
            Guid id, string name,
            string race, string species, string sex,
            string birthDate, double weight, double size,
            HistoryDTO history,UserDTOshort ownerDTO,
            bool isActive
            )
        {
            Id = id;
            Name = name;
            Race = race;
            Species = species;
            Sex = sex;
            BirthDate = birthDate;
            Weight = weight;
            Size = size;
            History = history;
            OwnerDTO = ownerDTO;
            IsActive = isActive;
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Race { get; set; }
        public string Species { get; set; }
        public string Sex { get; set; }
        public string BirthDate { get; set; }
        public double Weight { get; set; }
        public double Size { get; set; }
        public HistoryDTO History { get; set; }
        public UserDTOshort OwnerDTO { get; set; }
        public bool IsActive { get; set; }
    }

    public class PetDTOcreate
    {
        public string Name { get; set; }
        public string Species { get; set; }
        public string Race { get; set; }
        public double Weight { get; set; }
        public double Size { get; set; }
        public string Sex { get; set; }
        public string Date { get; set; }
        public string OwnerEmail { get; set; }

        public PetDTOcreate(
            string name, string species, string race,
            double weight, double size, string sex,
            string date, string ownerEmail
            )
        {
            Name = name;
            Species = species;
            Race = race;
            Weight = weight;
            Size = size;
            Sex = sex;
            Date = date;
            OwnerEmail = ownerEmail;
        }
    }

    public class PetDTOupdate
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public double? Weight { get; set; }
        public double? Size { get; set; }

        public PetDTOupdate(
            Guid id, string? name, double? weight,
            double? size
            )
        {
            Id = id;
            Name = name;
            Weight = weight;
            Size = size;
        }
    }
}