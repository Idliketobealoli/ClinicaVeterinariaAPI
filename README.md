# ClinicaVeterinariaAPI
API REST de la Clinica Veterinaria para el TFG.

## Endpoints

La ruta base de la API será: 
 - https://clinicaveterinariaapi:443/
 - http://clinicaveterinariaapi:1707/

---

### 🟢 /users

| Método | Endpoint | Auth | Descripción | Status Code | Return Content |
|--------|----------|------|-------------|-------------|----------------|
| GET | / | ADMIN | Encuentra todos los usuarios de la base de datos | 200 | Lista de DTO de usuarios |
| GET | /{email} | ADMIN, VET, USER | Encuentra un usuario por su email | 200, 404 | DTO del usuario |
| GET | /short/{email} | ADMIN, VET, USER | Encuentra un usuario por su email | 200, 404 | DTO del usuario, con informacion reducida |
| POST | /register | NO | Registra al usuario en la base de datos | 200, 400 | DTO del usuario con su token |
| POST | /login | NO | Permite logarse al usuario | 200, 400 | DTO del usuario con su token |
| PUT | / | USER | Permite al usuario cambiar su contraseña | 200, 400, 404 | DTO del usuario |
| DELETE | /{email} | ADMIN, USER | Da de baja a un usuario | 200, 404 | DTO del usuario |

---

### 🟣 /vets

| Método | Endpoint | Auth | Descripción | Status Code | Return Content |
|--------|----------|------|-------------|-------------|----------------|
| GET | / | ADMIN | Encuentra todos los veterinarios activos de la base de datos | 200 | Lista de DTO de veterinarios |
| GET | /appointment | USER | Encuentra todos los veterinarios activos de la base de datos, mostrando solo su nombre, email, apellido y estado | 200 | Lista de DTOappointment de veterinarios |
| GET | /stats | ADMIN, VET | Encuentra todos los veterinarios activos de la base de datos, ordenados por la cantidad de citas asociadas a cada uno | 200 | Lista de DTOstats de veterinarios |
| GET | /me | ADMIN, VET | Devuelve la informacion del propio veterinario | 200, 404 | DTO del veterinario |
| GET | /{email} | ADMIN, VET, USER | Encuentra un veterinario por su email | 200, 404 | DTO del veterinario |
| GET | /short/{email} | ADMIN, VET, USER | Encuentra un veterinario por su email | 200, 404 | DTO del veterinario, con informacion reducida |
| GET | /appointment/{email} | ADMIN, VET, USER | Encuentra un veterinario por su email | 200, 404 | DTO del veterinario, con informacion preparada para una cita |
| POST | /register | ADMIN, VET | Registra a un nuevo veterinario en la base de datos | 200, 400 | DTO del veterinario con su token |
| POST | /login | NO | Permite logarse al veterinario | 200, 400 | DTO del veterinario con su token |
| PUT | / | ADMIN, VET | Permite al veterinario cambiar su contraseña | 200, 400, 404 | DTO del veterinario |
| DELETE | /{email} | ADMIN | Da de baja a un veterinario | 200, 404 | DTO del veterinario |

---

### 🟡 /pets

| Método | Endpoint | Auth | Descripción | Status Code | Return Content |
|--------|----------|------|-------------|-------------|----------------|
| GET | / | ADMIN, VET, USER | Encuentra todas las mascotas de la base de datos, o todas aquellas que coincidan con el query param email | 200 | Lista de DTO de mascotas con la informacion acortada |
| GET | /{id} | ADMIN, VET, USER | Encuentra una mascota por su ID | 200, 404 | DTO de la mascota |
| POST | / | ADMIN, VET | Da de alta a una mascota | 200, 400, 404 | DTO de la mascota |
| PUT | / | ADMIN, VET | Actualiza una mascota | 200, 400, 404 | DTO de la mascota |
| DELETE | /{id} | ADMIN, VET | Da de baja a una mascota | 200, 400, 404 | DTO de la mascota |

---

### 🟠 /history

| Método | Endpoint | Auth | Descripción | Status Code | Return Content |
|--------|----------|------|-------------|-------------|----------------|
| GET | / | ADMIN, VET | Encuentra todos los historiales médicos de la base de datos | 200 | Lista de DTO de historiales |
| GET | /{id} | ADMIN, VET, USER | Encuentra un historial médico por el ID de la mascota con la que está asociado | 200, 404 | DTO de historial |
| GET | /vaccinesonly/{id} | ADMIN, VET, USER | Encuentra un historial médico por el ID de la mascota con la que está asociado | 200, 404 | DTO de historial, pero solo con la informacion sobre vacunación |
| GET | /ailmentonly/{id} | ADMIN, VET, USER | Encuentra un historial médico por el ID de la mascota con la que está asociado | 200, 404 | DTO de historial, pero solo con la informacion sobre las enfermedades y tratamientos recibidos |
| PUT | /vaccine/{id} | ADMIN, VET | Añade una nueva vacuna al historial con el ID de mascota correspondiente | 200, 400, 404 | DTO de historial |
| PUT | /ailment/{id} | ADMIN, VET | Añade una nueva enfermedad y tratamiento al historial con el ID de mascota correspondiente | 200, 400, 404 | DTO de historial |

---

### 🔴 /appointments

| Método | Endpoint | Auth | Descripción | Status Code | Return Content |
|--------|----------|------|-------------|-------------|----------------|
| GET | / | ADMIN, VET, USER | Encuentra todas las citas médicas de la base de datos; opcionalmente pudiendo filtrar por email de usuario o veterinario, asi como por fecha | 200 | Lista de DTO de citas, en formato reducido |
| GET | /{id} | ADMIN, VET, USER | Encuentra una cita por su ID | 200, 404 | DTO de la cita |
| POST | / | ADMIN, VET, USER | Crea una cita | 200, 400, 404 | DTO de la cita |
| PUT | / | ADMIN, VET | Actualiza el estado una cita | 200, 400, 404 | DTO de la cita |
| DELETE | /{id} | ADMIN, VET, USER | Cancela una cita, siempre y cuando sea con mas de 24 horas de antelación | 200, 400, 404 | DTO de la cita |
