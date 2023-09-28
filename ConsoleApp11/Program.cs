class Program
{
    static List<User> users = new List<User>();
    static List<Person> persons = new List<Person>();
    static User authenticatedUser;

    static ChiefDoctor chiefDoctor = new ChiefDoctor();

    class User
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
    }

    class Person
    {
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string ContactInfo { get; set; }
    }

    class Patient : Person
    {
        public List<Doctor> AttendingDoctors { get; set; } = new List<Doctor>();
        public List<Diagnosis> Diagnoses { get; set; } = new List<Diagnosis>();
        public List<Medication> Medications { get; set; } = new List<Medication>();
        public List<PatientAssessment> Assessments { get; set; } = new List<PatientAssessment>();
        public List<Notification> Notifications { get; set; } = new List<Notification>();
    }

    class Doctor : Person
    {
        public string Specialty { get; set; }
        public List<Patient> Patients { get; set; } = new List<Patient>();
    }

    class ChiefDoctor : Doctor
    {
        public List<Diagnosis> PossibleDiagnoses { get; set; } = new List<Diagnosis>();

        public void AddPossibleDiagnosis(Diagnosis diagnosis)
        {
            PossibleDiagnoses.Add(diagnosis);
        }

        public void ViewPossibleDiagnoses()
        {
            Console.WriteLine("Possible Diagnoses:");
            foreach (var diagnosis in PossibleDiagnoses)
            {
                Console.WriteLine($"{diagnosis.Name} - Diagnosed by {diagnosis.DiagnosingDoctor.Name} on {diagnosis.DiagnosisTime}");
            }
        }
    }

    class Diagnosis
    {
        public string Name { get; set; }
        public Doctor DiagnosingDoctor { get; set; }
        public DateTime DiagnosisTime { get; set; }
    }

    class Medication
    {
        public string Name { get; set; }
        public string Dosage { get; set; }
        public DateTime EndDate { get; set; }
    }

    class PatientAssessment
    {
        public Patient Patient { get; set; }
        public DateTime AssessmentDate { get; set; }
        public string AssessmentNotes { get; set; }
        public int HeartRate { get; set; }
        public int BloodPressure { get; set; }
    }

    class Notification
    {
        public string Message { get; set; }
        public Patient Patient { get; set; }
    }

    static void Main(string[] args)
    {
        users.Add(new User { Username = "doctor1", Password = "password1", Role = "Doctor" });
        users.Add(new User { Username = "doctor2", Password = "password2", Role = "Doctor" });
        users.Add(new User { Username = "chiefdoctor", Password = "chiefpass", Role = "ChiefDoctor" });
        users.Add(new User { Username = "admin", Password = "adminpass", Role = "Admin" });

        var doctor1 = new Doctor { Name = "Dr. Smith", Specialty = "General Practitioner" };
        var doctor2 = new Doctor { Name = "Dr. Johnson", Specialty = "Surgeon" };

        chiefDoctor.AddPossibleDiagnosis(new Diagnosis { Name = "Flu", DiagnosingDoctor = chiefDoctor, DiagnosisTime = DateTime.Now });
        chiefDoctor.AddPossibleDiagnosis(new Diagnosis { Name = "Common Cold", DiagnosingDoctor = chiefDoctor, DiagnosisTime = DateTime.Now });

        var patient1 = new Patient
        {
            Name = "Patient1",
            DateOfBirth = new DateTime(1980, 1, 15),
            Gender = "Male",
            ContactInfo = "Phone: 123-456-7890",
        };
        patient1.AttendingDoctors.Add(doctor1);

        var patient2 = new Patient
        {
            Name = "Patient2",
            DateOfBirth = new DateTime(1995, 5, 25),
            Gender = "Female",
            ContactInfo = "Phone: 987-654-3210",
        };
        patient2.AttendingDoctors.Add(doctor2);

        persons.Add(doctor1);
        persons.Add(doctor2);
        persons.Add(chiefDoctor);
        persons.Add(patient1);
        persons.Add(patient2);

        while (true)
        {
            Console.Clear();
            Console.WriteLine("Welcome to the Medical Records System.");
            Console.WriteLine("Please log in.");

            Console.Write("Username: ");
            string username = Console.ReadLine();

            Console.Write("Password: ");
            string password = Console.ReadLine();

            authenticatedUser = AuthenticateUser(username, password);

            if (authenticatedUser != null)
            {
                Console.WriteLine($"Logged in as {authenticatedUser.Role}.");
                MainMenu();
            }
            else
            {
                Console.WriteLine("Invalid username or password. Please try again.");
                Console.ReadLine();
            }
        }
    }

    static User AuthenticateUser(string username, string password)
    {
        return users.FirstOrDefault(user => user.Username == username && user.Password == password);
    }

    static void MainMenu()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("Main Menu:");
            Console.WriteLine("1. View Patient Information");
            Console.WriteLine("2. Add Medical Analysis");
            Console.WriteLine("3. Add Diagnosis");
            Console.WriteLine("4. Edit Patient Information");
            Console.WriteLine("5. Add Medication");
            Console.WriteLine("6. Check Patient's Critical Condition");
            Console.WriteLine("7. View Possible Diagnoses (Chief Doctor)");
            Console.WriteLine("8. Create Notification");
            Console.WriteLine("9. Admin View All Patient Data");
            Console.WriteLine("10. Exit");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    ViewPatientInfo();
                    break;
                case "2":
                    AddMedicalAnalysis();
                    break;
                case "3":
                    AddDiagnosis();
                    break;
                case "4":
                    EditPatientInfo();
                    break;
                case "5":
                    AddMedication();
                    break;
                case "6":
                    CheckCriticalCondition();
                    break;
                case "7":
                    if (authenticatedUser.Role == "ChiefDoctor")
                        chiefDoctor.ViewPossibleDiagnoses();
                    else
                        Console.WriteLine("Access denied. You are not the Chief Doctor.");
                    break;
                case "8":
                    CreateNotification();
                    break;
                case "9":
                    if (authenticatedUser.Role == "Admin")
                        AdminViewAllPatientData();
                    else
                        Console.WriteLine("Access denied. You are not an Admin.");
                    break;
                case "10":
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Invalid choice. Please select again.");
                    break;
            }
        }
    }

    static void ViewPatientInfo()
    {
        Console.WriteLine("Enter patient name: ");
        string patientName = Console.ReadLine();

        var patient = persons.OfType<Patient>().FirstOrDefault(p => p.Name == patientName);

        if (patient != null)
        {
            Console.WriteLine($"Patient Information for {patient.Name}:");
            Console.WriteLine($"Date of Birth: {patient.DateOfBirth}");
            Console.WriteLine($"Gender: {patient.Gender}");
            Console.WriteLine($"Contact Info: {patient.ContactInfo}");

            Console.WriteLine("Attending Doctors:");
            foreach (var doctor in patient.AttendingDoctors)
            {
                Console.WriteLine($"- Dr. {doctor.Name} ({doctor.Specialty})");
            }

            Console.WriteLine("Diagnoses:");
            foreach (var diagnosis in patient.Diagnoses)
            {
                Console.WriteLine($"- {diagnosis.Name} (Diagnosed by Dr. {diagnosis.DiagnosingDoctor.Name} on {diagnosis.DiagnosisTime})");
            }

            Console.WriteLine("Medications:");
            foreach (var medication in patient.Medications)
            {
                Console.WriteLine($"- {medication.Name} ({medication.Dosage}) - End Date: {medication.EndDate}");
            }
        }
        else
        {
            Console.WriteLine("Patient not found.");
        }

        Console.WriteLine("Press Enter to continue...");
        Console.ReadLine();
    }

    static void AddMedicalAnalysis()
    {
        Console.WriteLine("Enter patient name: ");
        string patientName = Console.ReadLine();

        var patient = persons.OfType<Patient>().FirstOrDefault(p => p.Name == patientName);

        if (patient != null)
        {
            Console.WriteLine($"Adding Medical Analysis for {patient.Name}:");
            Console.Write("Enter analysis details: ");
            string analysisDetails = Console.ReadLine();

            Console.WriteLine("Medical Analysis added successfully.");
        }
        else
        {
            Console.WriteLine("Patient not found.");
        }

        Console.WriteLine("Press Enter to continue...");
        Console.ReadLine();
    }

    static void AddDiagnosis()
    {
        Console.WriteLine("Enter patient name: ");
        string patientName = Console.ReadLine();

        var patient = persons.OfType<Patient>().FirstOrDefault(p => p.Name == patientName);

        if (patient != null)
        {
            Console.WriteLine($"Adding Diagnosis for {patient.Name}:");

            if (authenticatedUser.Role != "ChiefDoctor")
            {
                Console.WriteLine("Access denied. Only Chief Doctor can add diagnoses.");
                Console.WriteLine("Press Enter to continue...");
                Console.ReadLine();
                return;
            }

            Console.WriteLine("Available Diagnoses:");
            foreach (var diagnosis in chiefDoctor.PossibleDiagnoses)
            {
                Console.WriteLine($"- {diagnosis.Name}");
            }

            Console.Write("Enter diagnosis name: ");
            string diagnosisName = Console.ReadLine();

            var possibleDiagnosis = chiefDoctor.PossibleDiagnoses.FirstOrDefault(diagnosis => diagnosis.Name == diagnosisName);

            if (possibleDiagnosis != null)
            {
                var diagnosis = new Diagnosis
                {
                    Name = diagnosisName,
                    DiagnosingDoctor = chiefDoctor,
                    DiagnosisTime = DateTime.Now
                };

                patient.Diagnoses.Add(diagnosis);

                Console.WriteLine($"Diagnosis '{diagnosisName}' added successfully by Chief Doctor {chiefDoctor.Name}.");
            }
            else
            {
                Console.WriteLine("Invalid diagnosis name.");
            }
        }
        else
        {
            Console.WriteLine("Patient not found.");
        }

        Console.WriteLine("Press Enter to continue...");
        Console.ReadLine();
    }

    static void EditPatientInfo()
    {
        Console.WriteLine("Enter patient name: ");
        string patientName = Console.ReadLine();

        var patient = persons.OfType<Patient>().FirstOrDefault(p => p.Name == patientName);

        if (patient != null)
        {
            Console.WriteLine($"Editing Patient Information for {patient.Name}:");
            Console.Write("Enter new contact info: ");
            string newContactInfo = Console.ReadLine();

            patient.ContactInfo = newContactInfo;

            Console.WriteLine("Patient Information updated successfully.");
        }
        else
        {
            Console.WriteLine("Patient not found.");
        }

        Console.WriteLine("Press Enter to continue...");
        Console.ReadLine();
    }

    static void AddMedication()
    {
        Console.WriteLine("Enter patient name: ");
        string patientName = Console.ReadLine();

        var patient = persons.OfType<Patient>().FirstOrDefault(p => p.Name == patientName);

        if (patient != null)
        {
            Console.WriteLine($"Adding Medication for {patient.Name}:");
            Console.Write("Enter medication name: ");
            string medicationName = Console.ReadLine();

            Console.Write("Enter dosage: ");
            string dosage = Console.ReadLine();

            Console.Write("Enter end date (yyyy-MM-dd): ");
            if (DateTime.TryParse(Console.ReadLine(), out DateTime endDate))
            {
                var medication = new Medication
                {
                    Name = medicationName,
                    Dosage = dosage,
                    EndDate = endDate
                };

                patient.Medications.Add(medication);

                Console.WriteLine($"Medication '{medicationName}' added successfully.");
            }
            else
            {
                Console.WriteLine("Invalid date format.");
            }
        }
        else
        {
            Console.WriteLine("Patient not found.");
        }

        Console.WriteLine("Press Enter to continue...");
        Console.ReadLine();
    }

    static void CheckCriticalCondition()
    {
        Console.WriteLine("Enter patient name: ");
        string patientName = Console.ReadLine();

        var patient = persons.OfType<Patient>().FirstOrDefault(p => p.Name == patientName);

        if (patient != null)
        {
            Console.WriteLine($"Checking Critical Condition for {patient.Name}...");

            bool isCritical = patient.Assessments.Any(assessment => assessment.HeartRate > 120 || assessment.BloodPressure > 140);

            if (isCritical)
            {
                Console.WriteLine("Critical condition detected! Notify the medical team.");
            }
            else
            {
                Console.WriteLine("Patient is in stable condition.");
            }
        }
        else
        {
            Console.WriteLine("Patient not found.");
        }

        Console.WriteLine("Press Enter to continue...");
        Console.ReadLine();
    }

    static void CreateNotification()
    {
        Console.WriteLine("Enter patient name: ");
        string patientName = Console.ReadLine();

        var patient = persons.OfType<Patient>().FirstOrDefault(p => p.Name == patientName);

        if (patient != null)
        {
            Console.WriteLine($"Creating Notification for {patient.Name}:");
            Console.Write("Enter notification message: ");
            string notificationMessage = Console.ReadLine();

            var notification = new Notification
            {
                Message = notificationMessage,
                Patient = patient
            };

            patient.Notifications.Add(notification);

            Console.WriteLine("Notification created successfully.");
        }
        else
        {
            Console.WriteLine("Patient not found.");
        }

        Console.WriteLine("Press Enter to continue...");
        Console.ReadLine();
    }

    static void AdminViewAllPatientData()
    {
        Console.WriteLine("Enter patient name: ");
        string patientName = Console.ReadLine();

        var patient = persons.OfType<Patient>().FirstOrDefault(p => p.Name == patientName);

        if (patient != null)
        {
            Console.WriteLine($"Admin Viewing All Data for {patient.Name}:");

            Console.WriteLine($"Date of Birth: {patient.DateOfBirth}");
            Console.WriteLine($"Gender: {patient.Gender}");
            Console.WriteLine($"Contact Info: {patient.ContactInfo}");

            Console.WriteLine("Attending Doctors:");
            foreach (var doctor in patient.AttendingDoctors)
            {
                Console.WriteLine($"- Dr. {doctor.Name} ({doctor.Specialty})");
            }

            Console.WriteLine("Diagnoses:");
            foreach (var diagnosis in patient.Diagnoses)
            {
                Console.WriteLine($"- {diagnosis.Name} (Diagnosed by Dr. {diagnosis.DiagnosingDoctor.Name} on {diagnosis.DiagnosisTime})");
            }

            Console.WriteLine("Medications:");
            foreach (var medication in patient.Medications)
            {
                Console.WriteLine($"- {medication.Name} ({medication.Dosage}) - End Date: {medication.EndDate}");
            }
        }
        else
        {
            Console.WriteLine("Patient not found.");
        }

        Console.WriteLine("Press Enter to continue...");
        Console.ReadLine();
    }
}