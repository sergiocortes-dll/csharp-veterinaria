using veterinaria.Models;
using veterinaria.Models.Appointments;
using veterinaria.Services;

namespace veterinaria.UI;

public class Menu
{
    private readonly ClientService _clientService;
    private readonly PetService _petService;
    private readonly VetService _vetService;
    private readonly AppointmentService _appointmentService;

    public Menu(
        ClientService clientService,
        PetService petService,
        VetService vetService,
        AppointmentService appointmentService)
    {
        _clientService = clientService;
        _petService = petService;
        _vetService = vetService;
        _appointmentService = appointmentService;
    }

    public void Show()
    {
        Console.WriteLine("\nBienvenido a Veterinaria San Miguel\n");
        bool exit = false;
        while (!exit)
        {
            Console.WriteLine("\n===== Veterinaria San Miguel =====");
            Console.WriteLine("1. Gestion de Clientes");
            Console.WriteLine("2. Gestion de Mascotas");
            Console.WriteLine("3. Gestion de Veterinarios");
            Console.WriteLine("4. Gestion de Atenciones Medicas");
            Console.WriteLine("5. Historial Medico");
            Console.WriteLine("6. Consultas Avanzadas");
            Console.WriteLine("0. Salir");
            Console.Write("Seleccione una opcion: ");

            var option = Console.ReadLine();
            switch (option)
            {
                case "1": ClientsMenu(); break;
                case "2": PetsMenu(); break;
                case "3": VetsMenu(); break;
                case "4": AppointmentsMenu(); break;
                case "5": MedicalHistoryMenu(); break;
                case "6": AdvancedQueriesMenu(); break;
                case "0": 
                    exit = true;
                    Console.WriteLine("\n¡Gracias por usar el sistema! Hasta pronto");
                    break;
                default: Console.WriteLine("Opcion invalida."); break;
            }
        }
    }

    #region Gestion de Clientes

    private void ClientsMenu()
    {
        Console.WriteLine("\n--- Gestion de Clientes ---");
        Console.WriteLine("1. Registrar Cliente");
        Console.WriteLine("2. Listar Clientes");
        Console.WriteLine("3. Editar Cliente");
        Console.WriteLine("4. Eliminar Cliente");
        Console.Write("Seleccione una opcion: ");

        var option = Console.ReadLine();
        switch (option)
        {
            case "1": RegisterClient(); break;
            case "2": ListClients(); break;
            case "3": EditClient(); break;
            case "4": DeleteClient(); break;
        }
    }

    private void RegisterClient()
    {
        Console.Write("Nombre: ");
        var name = Console.ReadLine()!;
        Console.Write("Apellido: ");
        var lastName = Console.ReadLine()!;
        Console.Write("Email: ");
        var email = Console.ReadLine()!;
        Console.Write("Telefono: ");
        var phone = Console.ReadLine()!;
        Console.Write("¿Tiene seguro? (s/n): ");
        var insurance = Console.ReadLine()?.ToLower() == "s";

        var client = new Client
        {
            Name = name,
            LastName = lastName,
            Email = email,
            Phone = phone,
            Insurance = insurance
        };

        _clientService.AddClient(client);
        Console.WriteLine("Cliente registrado con exito");
    }

    private void ListClients()
    {
        var clients = _clientService.GetAllClients();
        Console.WriteLine("\n--- Lista de Clientes ---");
        if (!clients.Any())
        {
            Console.WriteLine("No hay clientes registrados.");
            return;
        }
        foreach (var c in clients)
        {
            var seguro = c.Insurance ? "Con seguro" : "Sin seguro";
            Console.WriteLine($"ID: {c.Id} | {c.Name} {c.LastName} | Tel: {c.Phone} | Email: {c.Email} | {seguro}");
        }
    }

    private void EditClient()
    {
        Console.Write("Ingrese el ID del cliente: ");
        if (int.TryParse(Console.ReadLine(), out int id))
        {
            var client = _clientService.GetClientById(id);
            if (client == null)
            {
                Console.WriteLine("Cliente no encontrado.");
                return;
            }

            Console.Write($"Nuevo nombre ({client.Name}): ");
            var newName = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(newName)) client.Name = newName;

            Console.Write($"Nuevo apellido ({client.LastName}): ");
            var newLastName = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(newLastName)) client.LastName = newLastName;

            Console.Write($"Nuevo email ({client.Email}): ");
            var newEmail = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(newEmail)) client.Email = newEmail;

            Console.Write($"Nuevo telefono ({client.Phone}): ");
            var newPhone = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(newPhone)) client.Phone = newPhone;

            _clientService.UpdateClient(client);
            Console.WriteLine("Cliente actualizado");
        }
    }

    private void DeleteClient()
    {
        Console.Write("Ingrese el ID del cliente: ");
        if (int.TryParse(Console.ReadLine(), out int id))
        {
            _clientService.DeleteClient(id);
            Console.WriteLine("Cliente eliminado");
        }
    }

    #endregion

    #region Gestion de Mascotas

    private void PetsMenu()
    {
        Console.WriteLine("\n--- Gestion de Mascotas ---");
        Console.WriteLine("1. Registrar Mascota");
        Console.WriteLine("2. Listar Mascotas");
        Console.WriteLine("3. Editar Mascota");
        Console.WriteLine("4. Eliminar Mascota");
        Console.Write("Seleccione una opcion: ");

        var option = Console.ReadLine();
        switch (option)
        {
            case "1": RegisterPet(); break;
            case "2": ListPets(); break;
            case "3": EditPet(); break;
            case "4": DeletePet(); break;
        }
    }

    private void RegisterPet()
    {
        Console.Write("Nombre de la mascota: ");
        var petName = Console.ReadLine()!;
        Console.Write("Especie (Perro/Gato/Otro): ");
        var species = Console.ReadLine()!;
        Console.Write("Raza: ");
        var breed = Console.ReadLine()!;
        Console.Write("Edad: ");
        if (!int.TryParse(Console.ReadLine(), out int age))
        {
            Console.WriteLine("Edad invalida.");
            return;
        }
        Console.Write("Peso (kg): ");
        if (!double.TryParse(Console.ReadLine(), out double weight))
        {
            Console.WriteLine("Peso invalido.");
            return;
        }
        Console.Write("¿Tiene alguna enfermedad? (s/n): ");
        var diseased = Console.ReadLine()?.ToLower() == "s";
        
        string description = "";
        if (diseased)
        {
            Console.Write("Descripcion de la enfermedad: ");
            description = Console.ReadLine()!;
        }

        ListClients();
        Console.Write("\nIngrese el ID del dueno: ");
        if (!int.TryParse(Console.ReadLine(), out int clientId))
        {
            Console.WriteLine("ID invalido.");
            return;
        }

        var pet = new Pet
        {
            PetName = petName,
            Species = species,
            Breed = breed,
            Age = age,
            Weight = weight,
            Diseased = diseased,
            Description = description,
            ClientId = clientId
        };

        _petService.AddPet(pet);
        Console.WriteLine("Mascota registrada con exito");
    }

    private void ListPets()
    {
        var pets = _petService.GetAllPets();
        Console.WriteLine("\n--- Lista de Mascotas ---");
        if (!pets.Any())
        {
            Console.WriteLine("No hay mascotas registradas.");
            return;
        }
        foreach (var p in pets)
        {
            var owner = _clientService.GetClientById(p.ClientId);
            var ownerName = owner != null ? $"{owner.Name} {owner.LastName}" : "Desconocido";
            var healthStatus = p.Diseased ? "Enfermo" : "Sano";
            Console.WriteLine($"ID: {p.Id} | {p.PetName} ({p.Species}) | Raza: {p.Breed} | Edad: {p.Age} años | Peso: {p.Weight}kg");
            Console.WriteLine($"   Dueno: {ownerName} | Estado: {healthStatus}");
            if (p.Diseased && !string.IsNullOrEmpty(p.Description))
            {
                Console.WriteLine($"   Enfermedad: {p.Description}");
            }
        }
    }

    private void EditPet()
    {
        Console.Write("Ingrese el ID de la mascota: ");
        if (int.TryParse(Console.ReadLine(), out int id))
        {
            var pet = _petService.GetPetById(id);
            if (pet == null)
            {
                Console.WriteLine("Mascota no encontrada.");
                return;
            }

            Console.Write($"Nuevo nombre ({pet.PetName}): ");
            var newName = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(newName)) pet.PetName = newName;

            Console.Write($"Nueva edad ({pet.Age}): ");
            if (int.TryParse(Console.ReadLine(), out int newAge)) pet.Age = newAge;

            Console.Write($"Nuevo peso ({pet.Weight}): ");
            if (double.TryParse(Console.ReadLine(), out double newWeight)) pet.Weight = newWeight;

            Console.Write($"¿Esta enfermo? (s/n) actual: {(pet.Diseased ? "Si" : "No")}: ");
            var diseasedInput = Console.ReadLine()?.ToLower();
            if (diseasedInput == "s" || diseasedInput == "n")
            {
                pet.Diseased = diseasedInput == "s";
                if (pet.Diseased)
                {
                    Console.Write($"Descripcion de la enfermedad ({pet.Description}): ");
                    var newDescription = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(newDescription)) pet.Description = newDescription;
                }
            }

            _petService.UpdatePet(pet);
            Console.WriteLine("Mascota actualizada");
        }
    }

    private void DeletePet()
    {
        Console.Write("Ingrese el ID de la mascota: ");
        if (int.TryParse(Console.ReadLine(), out int id))
        {
            _petService.DeletePet(id);
            Console.WriteLine("Mascota eliminada");
        }
    }

    #endregion

    #region Gestion de Veterinarios

    private void VetsMenu()
    {
        Console.WriteLine("\n--- Gestion de Veterinarios ---");
        Console.WriteLine("1. Registrar Veterinario");
        Console.WriteLine("2. Listar Veterinarios");
        Console.WriteLine("3. Editar Veterinario");
        Console.WriteLine("4. Eliminar Veterinario");
        Console.Write("Seleccione una opcion: ");

        var option = Console.ReadLine();
        switch (option)
        {
            case "1": RegisterVet(); break;
            case "2": ListVets(); break;
            case "3": EditVet(); break;
            case "4": DeleteVet(); break;
        }
    }

    private void RegisterVet()
    {
        Console.Write("Nombre: ");
        var name = Console.ReadLine()!;
        Console.Write("Apellido: ");
        var lastName = Console.ReadLine()!;
        Console.Write("Email: ");
        var email = Console.ReadLine()!;
        Console.Write("Telefono: ");
        var phone = Console.ReadLine()!;
        Console.Write("¿Es especialista? (s/n): ");
        var specialist = Console.ReadLine()?.ToLower() == "s";

        var vet = new Vet
        {
            Name = name,
            LastName = lastName,
            Email = email,
            Phone = phone,
            Specialist = specialist
        };

        _vetService.AddVet(vet);
        Console.WriteLine("Veterinario registrado con exito");
    }

    private void ListVets()
    {
        var vets = _vetService.GetAllVets();
        Console.WriteLine("\n--- Lista de Veterinarios ---");
        if (!vets.Any())
        {
            Console.WriteLine("No hay veterinarios registrados.");
            return;
        }
        foreach (var v in vets)
        {
            var tipo = v.Specialist ? "Especialista" : "Veterinario General";
            Console.WriteLine($"ID: {v.Id} | Dr(a). {v.Name} {v.LastName} | {tipo} | Tel: {v.Phone} | Email: {v.Email}");
        }
    }

    private void EditVet()
    {
        Console.Write("Ingrese el ID del veterinario: ");
        if (int.TryParse(Console.ReadLine(), out int id))
        {
            var vet = _vetService.GetVetById(id);
            if (vet == null)
            {
                Console.WriteLine("Veterinario no encontrado.");
                return;
            }

            Console.Write($"Nuevo nombre ({vet.Name}): ");
            var newName = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(newName)) vet.Name = newName;

            Console.Write($"Nuevo apellido ({vet.LastName}): ");
            var newLastName = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(newLastName)) vet.LastName = newLastName;

            Console.Write($"Nuevo email ({vet.Email}): ");
            var newEmail = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(newEmail)) vet.Email = newEmail;

            Console.Write($"Nuevo telefono ({vet.Phone}): ");
            var newPhone = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(newPhone)) vet.Phone = newPhone;

            Console.Write($"¿Es especialista? (s/n) actual: {(vet.Specialist ? "Si" : "No")}: ");
            var specialistInput = Console.ReadLine()?.ToLower();
            if (specialistInput == "s" || specialistInput == "n")
            {
                vet.Specialist = specialistInput == "s";
            }

            _vetService.UpdateVet(vet);
            Console.WriteLine("Veterinario actualizado");
        }
    }

    private void DeleteVet()
    {
        Console.Write("Ingrese el ID del veterinario: ");
        if (int.TryParse(Console.ReadLine(), out int id))
        {
            _vetService.DeleteVet(id);
            Console.WriteLine("Veterinario eliminado");
        }
    }

    #endregion

    #region Gestion de Atenciones Medicas

    private void AppointmentsMenu()
    {
        Console.WriteLine("\n--- Gestion de Atenciones Medicas ---");
        Console.WriteLine("1. Registrar Atencion Medica");
        Console.WriteLine("2. Listar Atenciones Medicas");
        Console.WriteLine("3. Editar Atencion Medica");
        Console.WriteLine("4. Eliminar Atencion Medica");
        Console.Write("Seleccione una opcion: ");

        var option = Console.ReadLine();
        switch (option)
        {
            case "1": RegisterAppointment(); break;
            case "2": ListAppointments(); break;
            case "3": EditAppointment(); break;
            case "4": DeleteAppointment(); break;
        }
    }

    private void RegisterAppointment()
    {
        ListClients();
        Console.Write("\nIngrese el ID del cliente: ");
        if (!int.TryParse(Console.ReadLine(), out int clientId))
        {
            Console.WriteLine("ID de cliente invalido.");
            return;
        }

        ListVets();
        Console.Write("\nIngrese el ID del veterinario: ");
        if (!int.TryParse(Console.ReadLine(), out int vetId))
        {
            Console.WriteLine("ID de veterinario invalido.");
            return;
        }

        Console.WriteLine("\n¿Que tipo de animal sera atendido?");
        Console.WriteLine("1. Mascota (Pet)");
        Console.WriteLine("2. Animal Silvestre (Wild)");
        Console.Write("Seleccione: ");
        var animalType = Console.ReadLine();

        int? petId = null;
        int? wildId = null;

        if (animalType == "1")
        {
            var clientPets = _petService.GetPetsByClient(clientId);
            if (!clientPets.Any())
            {
                Console.WriteLine("Este cliente no tiene mascotas registradas.");
                return;
            }
            Console.WriteLine("\n--- Mascotas del Cliente ---");
            foreach (var p in clientPets)
            {
                Console.WriteLine($"ID: {p.Id} | {p.PetName} ({p.Species})");
            }
            Console.Write("\nIngrese el ID de la mascota: ");
            if (!int.TryParse(Console.ReadLine(), out int selectedPetId))
            {
                Console.WriteLine("ID invalido.");
                return;
            }
            petId = selectedPetId;
        }
        else if (animalType == "2")
        {
            Console.WriteLine("Funcionalidad de animales silvestres no implementada aun.");
            return;
        }
        else
        {
            Console.WriteLine("Opcion invalida.");
            return;
        }

        Console.Write("\nFecha (dd/MM/yyyy) o presione Enter para hoy: ");
        var fechaInput = Console.ReadLine();
        DateTime fecha = string.IsNullOrWhiteSpace(fechaInput) ? DateTime.Now : DateTime.Parse(fechaInput);

        Console.Write("Diagnostico: ");
        var diagnosis = Console.ReadLine()!;

        Console.Write("Medicamentos: ");
        var medicaments = Console.ReadLine()!;

        var appointment = new Appointment
        {
            ClientId = clientId,
            VetId = vetId,
            PetId = petId,
            WildId = wildId,
            Date = fecha,
            Diagnosis = diagnosis,
            Medicaments = medicaments
        };

        _appointmentService.AddAppointment(appointment);
        Console.WriteLine("Atencion medica registrada con exito");
    }

    private void ListAppointments()
    {
        var appointments = _appointmentService.GetAllAppointments();
        Console.WriteLine("\n--- Lista de Atenciones Medicas ---");
        if (!appointments.Any())
        {
            Console.WriteLine("No hay atenciones registradas.");
            return;
        }
        foreach (var a in appointments)
        {
            var client = _clientService.GetClientById(a.ClientId);
            var vet = _vetService.GetVetById(a.VetId);
            
            string animalInfo = "No especificado";
            if (a.PetId.HasValue)
            {
                var pet = _petService.GetPetById(a.PetId.Value);
                animalInfo = pet != null ? $"Mascota: {pet.PetName} ({pet.Species})" : "Mascota no encontrada";
            }
            else if (a.WildId.HasValue)
            {
                animalInfo = "Animal Silvestre";
            }

            Console.WriteLine($"\n---------------------------------");
            Console.WriteLine($"ID Atencion: {a.Id} | Fecha: {a.Date:dd/MM/yyyy HH:mm}");
            Console.WriteLine($"Cliente: {client?.Name} {client?.LastName}");
            Console.WriteLine($"Veterinario: Dr(a). {vet?.Name} {vet?.LastName}");
            Console.WriteLine($"{animalInfo}");
            Console.WriteLine($"Diagnostico: {a.Diagnosis}");
            Console.WriteLine($"Medicamentos: {a.Medicaments}");
        }
        Console.WriteLine($"---------------------------------\n");
    }

    private void EditAppointment()
    {
        Console.Write("Ingrese el ID de la atencion: ");
        if (int.TryParse(Console.ReadLine(), out int id))
        {
            var appointment = _appointmentService.GetAppointmentById(id);
            if (appointment == null)
            {
                Console.WriteLine("Atencion no encontrada.");
                return;
            }

            Console.Write($"Nuevo diagnostico ({appointment.Diagnosis}): ");
            var newDiagnosis = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(newDiagnosis)) appointment.Diagnosis = newDiagnosis;

            Console.Write($"Nuevos medicamentos ({appointment.Medicaments}): ");
            var newMedicaments = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(newMedicaments)) appointment.Medicaments = newMedicaments;

            _appointmentService.UpdateAppointment(appointment);
            Console.WriteLine("Atencion actualizada");
        }
    }

    private void DeleteAppointment()
    {
        Console.Write("Ingrese el ID de la atencion: ");
        if (int.TryParse(Console.ReadLine(), out int id))
        {
            _appointmentService.DeleteAppointment(id);
            Console.WriteLine("Atencion eliminada");
        }
    }

    #endregion

    #region Historial Medico

    private void MedicalHistoryMenu()
    {
        Console.WriteLine("\n--- Historial Medico ---");
        ListPets();
        Console.Write("\nIngrese el ID de la mascota: ");
        if (int.TryParse(Console.ReadLine(), out int petId))
        {
            var pet = _petService.GetPetById(petId);
            if (pet == null)
            {
                Console.WriteLine("Mascota no encontrada.");
                return;
            }

            var appointments = _appointmentService.GetAppointmentsByPet(petId);
            Console.WriteLine($"\n================================================");
            Console.WriteLine($"  HISTORIAL MEDICO DE {pet.PetName.ToUpper()}");
            Console.WriteLine($"================================================");
            Console.WriteLine($"Especie: {pet.Species} | Raza: {pet.Breed} | Edad: {pet.Age} años | Peso: {pet.Weight}kg");
            
            var owner = _clientService.GetClientById(pet.ClientId);
            Console.WriteLine($"Dueno: {owner?.Name} {owner?.LastName}");
            
            if (pet.Diseased)
            {
                Console.WriteLine($"ADVERTENCIA: Mascota con enfermedad - {pet.Description}");
            }

            Console.WriteLine("\n");

            if (!appointments.Any())
            {
                Console.WriteLine("No hay atenciones registradas para esta mascota.");
                return;
            }

            foreach (var a in appointments.OrderByDescending(x => x.Date))
            {
                var vet = _vetService.GetVetById(a.VetId);
                var vetType = vet?.Specialist == true ? "Especialista" : "General";
                
                Console.WriteLine($"-----------------------------------------");
                Console.WriteLine($" Fecha: {a.Date:dd/MM/yyyy HH:mm}");
                Console.WriteLine($" Veterinario: Dr(a). {vet?.Name} {vet?.LastName} ({vetType})");
                Console.WriteLine($" Diagnostico: {a.Diagnosis}");
                Console.WriteLine($" Medicamentos: {a.Medicaments}");
                Console.WriteLine($"-----------------------------------------");
                Console.WriteLine();
            }
        }
    }

    #endregion

    #region Consultas Avanzadas

    private void AdvancedQueriesMenu()
    {
        Console.WriteLine("\n--- Consultas Avanzadas ---");
        Console.WriteLine("1. Mascotas de un cliente");
        Console.WriteLine("2. Veterinario con mas atenciones");
        Console.WriteLine("3. Especie de mascota mas atendida");
        Console.WriteLine("4. Cliente con mas mascotas");
        Console.WriteLine("5. Mascotas enfermas");
        Console.WriteLine("6. Veterinarios especialistas");
        Console.Write("Seleccione una opcion: ");

        var option = Console.ReadLine();
        switch (option)
        {
            case "1": ClientPetsQuery(); break;
            case "2": MostAppointmentsVetQuery(); break;
            case "3": MostAttendedSpeciesQuery(); break;
            case "4": MostPetsClientQuery(); break;
            case "5": SickPetsQuery(); break;
            case "6": SpecialistVetsQuery(); break;
        }
    }

    private void ClientPetsQuery()
    {
        ListClients();
        Console.Write("\nIngrese el ID del cliente: ");
        if (int.TryParse(Console.ReadLine(), out int clientId))
        {
            var client = _clientService.GetClientById(clientId);
            if (client == null)
            {
                Console.WriteLine("Cliente no encontrado.");
                return;
            }

            var pets = _petService.GetPetsByClient(clientId);
            Console.WriteLine($"\n================================================");
            Console.WriteLine($"  MASCOTAS DE {client.Name.ToUpper()} {client.LastName.ToUpper()}");
            Console.WriteLine($"================================================");
            
            if (!pets.Any())
            {
                Console.WriteLine("Este cliente no tiene mascotas registradas.");
                return;
            }

            Console.WriteLine($"Total de mascotas: {pets.Count()}\n");

            foreach (var p in pets)
            {
                var healthIcon = p.Diseased ? "Enfermo" : "Sano";
                Console.WriteLine($"{healthIcon} {p.PetName} - {p.Species} ({p.Breed}) - {p.Age} años - {p.Weight}kg");
                if (p.Diseased)
                {
                    Console.WriteLine($"  Enfermedad: {p.Description}");
                }
            }
        }
    }

    private void MostAppointmentsVetQuery()
    {
        Console.WriteLine("\n¿Desea filtrar por período? (s/n)");
        var filterOption = Console.ReadLine()?.ToLower();
        
        Vet? vet = null;
        int appointmentCount = 0;
        
        if (filterOption == "s")
        {
            Console.Write("Fecha inicial (dd/MM/yyyy): ");
            if (DateTime.TryParseExact(Console.ReadLine(), "dd/MM/yyyy", null, 
                System.Globalization.DateTimeStyles.None, out DateTime startDate))
            {
                Console.Write("Fecha final (dd/MM/yyyy): ");
                if (DateTime.TryParseExact(Console.ReadLine(), "dd/MM/yyyy", null, 
                    System.Globalization.DateTimeStyles.None, out DateTime endDate))
                {
                    vet = _appointmentService.GetVetWithMostAppointments(startDate, endDate);
                    
                    if (vet != null)
                    {
                        appointmentCount = _appointmentService.GetAllAppointments()
                            .Count(a => a.VetId == vet.Id && 
                                       a.Date >= startDate && 
                                       a.Date <= endDate);
                    }
                }
            }
        }
        else
        {
            // Without filter
            vet = _appointmentService.GetVetWithMostAppointments();
            
            // Count all appointments
            if (vet != null)
            {
                appointmentCount = _appointmentService.GetAllAppointments()
                    .Count(a => a.VetId == vet.Id);
            }
        }

        if (vet == null)
        {
            Console.WriteLine("No hay datos suficientes.");
            return;
        }

        var tipo = vet.Specialist ? "Especialista" : "General";
        
        Console.WriteLine($"\n================================================");
        Console.WriteLine($"   VETERINARIO CON MÁS ATENCIONES");
        if (filterOption == "s")
        {
            Console.WriteLine($"   Período específico");
        }
        else
        {
            Console.WriteLine($"   Histórico total");
        }
        Console.WriteLine($"================================================");
        Console.WriteLine($"Dr(a). {vet.Name} {vet.LastName}");
        Console.WriteLine($"Tipo: {tipo}");
        Console.WriteLine($"Total de atenciones: {appointmentCount}");
        Console.WriteLine($"Teléfono: {vet.Phone}");
        Console.WriteLine($"Email: {vet.Email}");
        Console.WriteLine($"================================================\n");
    }

    private void MostAttendedSpeciesQuery()
    {
        var species = _appointmentService.GetMostAttendedSpecies();
        if (string.IsNullOrEmpty(species))
        {
            Console.WriteLine("No hay datos suficientes.");
            return;
        }

        var count = _appointmentService.GetAllAppointments()
            .Where(a => a.PetId.HasValue)
            .Select(a => _petService.GetPetById(a.PetId.Value))
            .Count(p => p?.Species == species);

        Console.WriteLine($"\n================================================");
        Console.WriteLine($"   ESPECIE MAS ATENDIDA");
        Console.WriteLine($"================================================");
        Console.WriteLine($"Especie: {species}");
        Console.WriteLine($"Total de atenciones: {count}");
        Console.WriteLine($"================================================\n");
    }

    private void MostPetsClientQuery()
    {
        var client = _petService.GetClientWithMostPets();
        if (client == null)
        {
            Console.WriteLine("No hay datos suficientes.");
            return;
        }

        var count = _petService.GetPetsByClient(client.Id).Count();
        var seguro = client.Insurance ? "Con seguro" : "Sin seguro";
        
        Console.WriteLine($"\n================================================");
        Console.WriteLine($"   CLIENTE CON MAS MASCOTAS");
        Console.WriteLine($"================================================");
        Console.WriteLine($"{client.Name} {client.LastName}");
        Console.WriteLine($"Total de mascotas: {count}");
        Console.WriteLine($"Telefono: {client.Phone}");
        Console.WriteLine($"Email: {client.Email}");
        Console.WriteLine($"{seguro}");
        Console.WriteLine($"================================================\n");
    }

    private void SickPetsQuery()
    {
        var pets = _petService.GetAllPets().Where(p => p.Diseased).ToList();
        
        Console.WriteLine($"\n================================================");
        Console.WriteLine($"   MASCOTAS ENFERMAS");
        Console.WriteLine($"================================================");
        
        if (!pets.Any())
        {
            Console.WriteLine("No hay mascotas enfermas registradas.");
            Console.WriteLine($"================================================\n");
            return;
        }

        Console.WriteLine($"Total: {pets.Count} mascota(s)\n");

        foreach (var p in pets)
        {
            var owner = _clientService.GetClientById(p.ClientId);
            Console.WriteLine($"{p.PetName} ({p.Species} - {p.Breed})");
            Console.WriteLine($"   Dueno: {owner?.Name} {owner?.LastName}");
            Console.WriteLine($"   Enfermedad: {p.Description}");
            Console.WriteLine($"   Edad: {p.Age} años | Peso: {p.Weight}kg");
            Console.WriteLine();
        }
        Console.WriteLine($"================================================\n");
    }

    private void SpecialistVetsQuery()
    {
        var specialists = _vetService.GetAllVets().Where(v => v.Specialist).ToList();
        
        Console.WriteLine($"\n================================================");
        Console.WriteLine($"   VETERINARIOS ESPECIALISTAS");
        Console.WriteLine($"================================================");
        
        if (!specialists.Any())
        {
            Console.WriteLine("No hay veterinarios especialistas registrados.");
            Console.WriteLine($"================================================\n");
            return;
        }

        Console.WriteLine($"Total: {specialists.Count} especialista(s)\n");

        foreach (var v in specialists)
        {
            var atenciones = _appointmentService.GetAllAppointments().Count(a => a.VetId == v.Id);
            Console.WriteLine($"Dr(a). {v.Name} {v.LastName}");
            Console.WriteLine($"   {v.Phone} | {v.Email}");
            Console.WriteLine($"   Atenciones realizadas: {atenciones}");
            Console.WriteLine();
        }
        Console.WriteLine($"================================================\n");
    }

    #endregion
}