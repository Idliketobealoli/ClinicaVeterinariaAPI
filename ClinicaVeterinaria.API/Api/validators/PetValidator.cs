using ClinicaVeterinaria.API.Api.dto;
using System.Net.Mail;

namespace ClinicaVeterinaria.API.Api.validators
{
    // Extension functions whose putpose is to validate the information about Pets
    public static class PetValidator
    {
        public static string? Validate(this PetDTOcreate dto)
        {
            if (dto == null) return "Data must not be null.";

            else if (!dto.Name.Trim().Any())
                return "Name must not be null or blank.";

            else if (dto.Name.Trim().Length < 2)
                return "Name must not be a single letter.";

            else if (!dto.Species.Trim().Any())
                return "Species must not be null or blank.";

            else if (dto.Species.Trim().Length < 3)
                return "Species must not be less than 3 letters.";

            else if (!dto.Race.Trim().Any())
                return "Race must not be null or blank.";

            else if (dto.Race.Trim().Length < 3)
                return "Race must not be less than 3 letters.";

            else if (dto.Weight <= 0)
                return "Weight must not be equal to or lower than 0.";

            else if (dto.Size <= 0)
                return "Size must not be equal to or lower than 0.";

            else {
                if (DateOnly.TryParse(dto.Date, out DateOnly dt))
                {
                    if (dt > DateOnly.FromDateTime(DateTime.Now))
                        return "Birth date must not be in the future.";

                    else if (!MailAddress.TryCreate(dto.OwnerEmail.Trim(), out _))
                        return "Incorrect owner email address expression.";

                    else return null;
                }
                else return "Birth date must be in a valid date format.";
            }
        }

        public static string? Validate(this PetDTOupdate dto)
        {
            if (dto == null) return "Data must not be null.";

            else if (dto.Name != null && dto.Name.Trim().Length < 2)
                return "Name must not be a single letter.";

            else if (dto.Weight != null && dto.Weight <= 0)
                return "Weight must not be equal to or lower than 0.";

            else if (dto.Size != null && dto.Size <= 0)
                return "Size must not be equal to or lower than 0.";

            else return null;
        }
    }
}
