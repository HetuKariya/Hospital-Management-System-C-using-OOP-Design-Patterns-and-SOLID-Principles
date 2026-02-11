using System;
using System.Collections.Generic;
using System.Linq;

namespace HospitalManagementSystem
{
    // ==================== ENTITIES (Models) ====================
    
    public enum AppointmentStatus
    {
        Scheduled,
        Completed,
        Cancelled
    }

    public enum PatientType
    {
        General,
        Premium,
        Emergency
    }

    public class Patient
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string ContactNumber { get; set; }
        public string MedicalHistory { get; set; }
        public PatientType Type { get; set; }

        public Patient(int id, string name, int age, string contactNumber, PatientType type)
        {
            Id = id;
            Name = name;
            Age = age;
            ContactNumber = contactNumber;
            Type = type;
            MedicalHistory = "";
        }
    }

    public class Doctor
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Specialization { get; set; }
        public bool IsAvailable { get; set; }
        public List<string> AvailableTimeSlots { get; set; }

        public Doctor(int id, string name, string specialization)
        {
            Id = id;
            Name = name;
            Specialization = specialization;
            IsAvailable = true;
            AvailableTimeSlots = new List<string>();
        }
    }

    public class Appointment
    {
        public int Id { get; set; }
        public Patient Patient { get; set; }
        public Doctor Doctor { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string TimeSlot { get; set; }
        public AppointmentStatus Status { get; set; }

        public Appointment(int id, Patient patient, Doctor doctor, DateTime date, string timeSlot)
        {
            Id = id;
            Patient = patient;
            Doctor = doctor;
            AppointmentDate = date;
            TimeSlot = timeSlot;
            Status = AppointmentStatus.Scheduled;
        }
    }

    // ==================== INTERFACES (SOLID - Interface Segregation & Dependency Inversion) ====================
    
    public interface IRepository<T> where T : class
    {
        void Add(T entity);
        T GetById(int id);
        List<T> GetAll();
        void Update(T entity);
        void Delete(int id);
    }

    public interface INotificationService
    {
        void SendNotification(string recipient, string message);
    }

    public interface IBillingStrategy
    {
        decimal CalculateBill(int durationMinutes, PatientType patientType);
    }

    public interface IAppointmentObserver
    {
        void Update(Appointment appointment);
    }

    // ==================== DESIGN PATTERN: SINGLETON ====================
    // Hospital Logger - Single instance throughout application
    
    public sealed class HospitalLogger
    {
        private static HospitalLogger _instance;
        private static readonly object _lock = new object();
        private List<string> _logs;

        private HospitalLogger()
        {
            _logs = new List<string>();
        }

        public static HospitalLogger Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                        {
                            _instance = new HospitalLogger();
                        }
                    }
                }
                return _instance;
            }
        }

        public void Log(string message)
        {
            string logEntry = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {message}";
            _logs.Add(logEntry);
            Console.WriteLine(logEntry);
        }

        public List<string> GetLogs()
        {
            return new List<string>(_logs);
        }
    }

    // ==================== DESIGN PATTERN: REPOSITORY ====================
    // SOLID: Single Responsibility - Each repository handles one entity
    
    public class PatientRepository : IRepository<Patient>
    {
        private List<Patient> _patients = new List<Patient>();
        private int _nextId = 1;

        public void Add(Patient entity)
        {
            entity.Id = _nextId++;
            _patients.Add(entity);
            HospitalLogger.Instance.Log($"Patient added: {entity.Name}");
        }

        public Patient GetById(int id)
        {
            return _patients.FirstOrDefault(p => p.Id == id);
        }

        public List<Patient> GetAll()
        {
            return new List<Patient>(_patients);
        }

        public void Update(Patient entity)
        {
            var existingPatient = GetById(entity.Id);
            if (existingPatient != null)
            {
                int index = _patients.IndexOf(existingPatient);
                _patients[index] = entity;
                HospitalLogger.Instance.Log($"Patient updated: {entity.Name}");
            }
        }

        public void Delete(int id)
        {
            var patient = GetById(id);
            if (patient != null)
            {
                _patients.Remove(patient);
                HospitalLogger.Instance.Log($"Patient deleted: {patient.Name}");
            }
        }
    }

    public class DoctorRepository : IRepository<Doctor>
    {
        private List<Doctor> _doctors = new List<Doctor>();
        private int _nextId = 1;

        public void Add(Doctor entity)
        {
            entity.Id = _nextId++;
            _doctors.Add(entity);
            HospitalLogger.Instance.Log($"Doctor added: {entity.Name}");
        }

        public Doctor GetById(int id)
        {
            return _doctors.FirstOrDefault(d => d.Id == id);
        }

        public List<Doctor> GetAll()
        {
            return new List<Doctor>(_doctors);
        }

        public void Update(Doctor entity)
        {
            var existingDoctor = GetById(entity.Id);
            if (existingDoctor != null)
            {
                int index = _doctors.IndexOf(existingDoctor);
                _doctors[index] = entity;
                HospitalLogger.Instance.Log($"Doctor updated: {entity.Name}");
            }
        }

        public void Delete(int id)
        {
            var doctor = GetById(id);
            if (doctor != null)
            {
                _doctors.Remove(doctor);
                HospitalLogger.Instance.Log($"Doctor deleted: {doctor.Name}");
            }
        }

        public List<Doctor> GetBySpecialization(string specialization)
        {
            return _doctors.Where(d => d.Specialization.Equals(specialization, StringComparison.OrdinalIgnoreCase)).ToList();
        }
    }

    public class AppointmentRepository : IRepository<Appointment>
    {
        private List<Appointment> _appointments = new List<Appointment>();
        private int _nextId = 1;

        public void Add(Appointment entity)
        {
            entity.Id = _nextId++;
            _appointments.Add(entity);
            HospitalLogger.Instance.Log($"Appointment created: ID {entity.Id}");
        }

        public Appointment GetById(int id)
        {
            return _appointments.FirstOrDefault(a => a.Id == id);
        }

        public List<Appointment> GetAll()
        {
            return new List<Appointment>(_appointments);
        }

        public void Update(Appointment entity)
        {
            var existingAppointment = GetById(entity.Id);
            if (existingAppointment != null)
            {
                int index = _appointments.IndexOf(existingAppointment);
                _appointments[index] = entity;
                HospitalLogger.Instance.Log($"Appointment updated: ID {entity.Id}");
            }
        }

        public void Delete(int id)
        {
            var appointment = GetById(id);
            if (appointment != null)
            {
                _appointments.Remove(appointment);
                HospitalLogger.Instance.Log($"Appointment deleted: ID {id}");
            }
        }
    }

    // ==================== DESIGN PATTERN: STRATEGY ====================
    // Different billing strategies for different patient types
    
    public class StandardBillingStrategy : IBillingStrategy
    {
        public decimal CalculateBill(int durationMinutes, PatientType patientType)
        {
            decimal baseRate = 500; // Base consultation fee
            decimal perMinuteRate = 10;
            return baseRate + (durationMinutes * perMinuteRate);
        }
    }

    public class PremiumBillingStrategy : IBillingStrategy
    {
        public decimal CalculateBill(int durationMinutes, PatientType patientType)
        {
            decimal baseRate = 1000; // Higher base for premium patients
            decimal perMinuteRate = 15;
            decimal discount = 0.1m; // 10% discount
            decimal totalBill = baseRate + (durationMinutes * perMinuteRate);
            return totalBill - (totalBill * discount);
        }
    }

    public class EmergencyBillingStrategy : IBillingStrategy
    {
        public decimal CalculateBill(int durationMinutes, PatientType patientType)
        {
            decimal baseRate = 2000; // Emergency consultation
            decimal perMinuteRate = 20;
            decimal emergencySurcharge = 500;
            return baseRate + (durationMinutes * perMinuteRate) + emergencySurcharge;
        }
    }

    // SOLID: Open/Closed Principle - Billing context open for extension
    public class BillingContext
    {
        private IBillingStrategy _strategy;

        public void SetStrategy(IBillingStrategy strategy)
        {
            _strategy = strategy;
        }

        public decimal CalculateBill(int durationMinutes, PatientType patientType)
        {
            if (_strategy == null)
            {
                throw new InvalidOperationException("Billing strategy not set");
            }
            return _strategy.CalculateBill(durationMinutes, patientType);
        }
    }

    // ==================== DESIGN PATTERN: FACTORY ====================
    // Factory for creating notification services
    
    public class EmailNotificationService : INotificationService
    {
        public void SendNotification(string recipient, string message)
        {
            Console.WriteLine($"[EMAIL] To: {recipient} - {message}");
            HospitalLogger.Instance.Log($"Email sent to {recipient}");
        }
    }

    public class SmsNotificationService : INotificationService
    {
        public void SendNotification(string recipient, string message)
        {
            Console.WriteLine($"[SMS] To: {recipient} - {message}");
            HospitalLogger.Instance.Log($"SMS sent to {recipient}");
        }
    }

    public enum NotificationType
    {
        Email,
        Sms
    }

    public class NotificationFactory
    {
        public static INotificationService CreateNotificationService(NotificationType type)
        {
            switch (type)
            {
                case NotificationType.Email:
                    return new EmailNotificationService();
                case NotificationType.Sms:
                    return new SmsNotificationService();
                default:
                    throw new ArgumentException("Invalid notification type");
            }
        }
    }

    // ==================== DESIGN PATTERN: OBSERVER ====================
    // Observers get notified when appointment status changes
    
    public class PatientNotificationObserver : IAppointmentObserver
    {
        private INotificationService _notificationService;

        public PatientNotificationObserver(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        public void Update(Appointment appointment)
        {
            string message = $"Your appointment (ID: {appointment.Id}) status has been changed to {appointment.Status}";
            _notificationService.SendNotification(appointment.Patient.ContactNumber, message);
        }
    }

    public class DoctorNotificationObserver : IAppointmentObserver
    {
        private INotificationService _notificationService;

        public DoctorNotificationObserver(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        public void Update(Appointment appointment)
        {
            string message = $"Appointment (ID: {appointment.Id}) with {appointment.Patient.Name} - Status: {appointment.Status}";
            _notificationService.SendNotification("doctor@hospital.com", message);
        }
    }

    // Subject that manages observers
    public class AppointmentSubject
    {
        private List<IAppointmentObserver> _observers = new List<IAppointmentObserver>();

        public void Attach(IAppointmentObserver observer)
        {
            _observers.Add(observer);
        }

        public void Detach(IAppointmentObserver observer)
        {
            _observers.Remove(observer);
        }

        public void Notify(Appointment appointment)
        {
            foreach (var observer in _observers)
            {
                observer.Update(appointment);
            }
        }
    }

    // ==================== SERVICE LAYER (SOLID - Single Responsibility) ====================
    
    public class AppointmentService
    {
        private readonly AppointmentRepository _appointmentRepo;
        private readonly PatientRepository _patientRepo;
        private readonly DoctorRepository _doctorRepo;
        private readonly AppointmentSubject _appointmentSubject;

        public AppointmentService(
            AppointmentRepository appointmentRepo,
            PatientRepository patientRepo,
            DoctorRepository doctorRepo,
            AppointmentSubject appointmentSubject)
        {
            _appointmentRepo = appointmentRepo;
            _patientRepo = patientRepo;
            _doctorRepo = doctorRepo;
            _appointmentSubject = appointmentSubject;
        }

        public Appointment BookAppointment(int patientId, int doctorId, DateTime date, string timeSlot)
        {
            var patient = _patientRepo.GetById(patientId);
            var doctor = _doctorRepo.GetById(doctorId);

            if (patient == null || doctor == null)
            {
                throw new InvalidOperationException("Patient or Doctor not found");
            }

            if (!doctor.IsAvailable)
            {
                throw new InvalidOperationException("Doctor is not available");
            }

            var appointment = new Appointment(0, patient, doctor, date, timeSlot);
            _appointmentRepo.Add(appointment);
            _appointmentSubject.Notify(appointment);

            return appointment;
        }

        public void CancelAppointment(int appointmentId)
        {
            var appointment = _appointmentRepo.GetById(appointmentId);
            if (appointment == null)
            {
                throw new InvalidOperationException("Appointment not found");
            }

            appointment.Status = AppointmentStatus.Cancelled;
            _appointmentRepo.Update(appointment);
            _appointmentSubject.Notify(appointment);
        }

        public void CompleteAppointment(int appointmentId)
        {
            var appointment = _appointmentRepo.GetById(appointmentId);
            if (appointment == null)
            {
                throw new InvalidOperationException("Appointment not found");
            }

            appointment.Status = AppointmentStatus.Completed;
            _appointmentRepo.Update(appointment);
            _appointmentSubject.Notify(appointment);
        }
    }

    public class BillingService
    {
        private readonly BillingContext _billingContext;

        public BillingService()
        {
            _billingContext = new BillingContext();
        }

        public decimal GenerateBill(PatientType patientType, int durationMinutes)
        {
            IBillingStrategy strategy;

            switch (patientType)
            {
                case PatientType.Premium:
                    strategy = new PremiumBillingStrategy();
                    break;
                case PatientType.Emergency:
                    strategy = new EmergencyBillingStrategy();
                    break;
                default:
                    strategy = new StandardBillingStrategy();
                    break;
            }

            _billingContext.SetStrategy(strategy);
            decimal bill = _billingContext.CalculateBill(durationMinutes, patientType);
            
            HospitalLogger.Instance.Log($"Bill generated: ${bill} for {patientType} patient");
            return bill;
        }
    }

    // ==================== MAIN PROGRAM ====================
    
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("========================================");
            Console.WriteLine("   HOSPITAL MANAGEMENT SYSTEM");
            Console.WriteLine("========================================\n");

            // Initialize repositories
            var patientRepo = new PatientRepository();
            var doctorRepo = new DoctorRepository();
            var appointmentRepo = new AppointmentRepository();

            // Setup Observer pattern
            var appointmentSubject = new AppointmentSubject();
            var emailService = NotificationFactory.CreateNotificationService(NotificationType.Email);
            var smsService = NotificationFactory.CreateNotificationService(NotificationType.Sms);
            
            appointmentSubject.Attach(new PatientNotificationObserver(smsService));
            appointmentSubject.Attach(new DoctorNotificationObserver(emailService));

            // Initialize services
            var appointmentService = new AppointmentService(appointmentRepo, patientRepo, doctorRepo, appointmentSubject);
            var billingService = new BillingService();

            // Add sample data
            Console.WriteLine("1. Adding Patients...\n");
            var patient1 = new Patient(0, "John Doe", 35, "+1234567890", PatientType.General);
            var patient2 = new Patient(0, "Jane Smith", 42, "+0987654321", PatientType.Premium);
            var patient3 = new Patient(0, "Bob Johnson", 28, "+1122334455", PatientType.Emergency);
            
            patientRepo.Add(patient1);
            patientRepo.Add(patient2);
            patientRepo.Add(patient3);

            Console.WriteLine("\n2. Adding Doctors...\n");
            var doctor1 = new Doctor(0, "Dr. Sarah Williams", "Cardiology");
            var doctor2 = new Doctor(0, "Dr. Michael Brown", "Neurology");
            
            doctorRepo.Add(doctor1);
            doctorRepo.Add(doctor2);

            Console.WriteLine("\n3. Booking Appointments...\n");
            var appointment1 = appointmentService.BookAppointment(1, 1, DateTime.Now.AddDays(1), "10:00 AM");
            var appointment2 = appointmentService.BookAppointment(2, 2, DateTime.Now.AddDays(2), "2:00 PM");

            Console.WriteLine("\n4. Completing Appointment...\n");
            appointmentService.CompleteAppointment(appointment1.Id);

            Console.WriteLine("\n5. Generating Bills (Strategy Pattern)...\n");
            decimal bill1 = billingService.GenerateBill(PatientType.General, 30);
            Console.WriteLine($"General Patient Bill (30 min): ${bill1}\n");

            decimal bill2 = billingService.GenerateBill(PatientType.Premium, 30);
            Console.WriteLine($"Premium Patient Bill (30 min): ${bill2}\n");

            decimal bill3 = billingService.GenerateBill(PatientType.Emergency, 30);
            Console.WriteLine($"Emergency Patient Bill (30 min): ${bill3}\n");

            Console.WriteLine("\n6. Cancelling Appointment...\n");
            appointmentService.CancelAppointment(appointment2.Id);

            Console.WriteLine("\n7. Displaying All Appointments...\n");
            var allAppointments = appointmentRepo.GetAll();
            foreach (var apt in allAppointments)
            {
                Console.WriteLine($"Appointment ID: {apt.Id}");
                Console.WriteLine($"Patient: {apt.Patient.Name}");
                Console.WriteLine($"Doctor: {apt.Doctor.Name}");
                Console.WriteLine($"Date: {apt.AppointmentDate:yyyy-MM-dd}");
                Console.WriteLine($"Time: {apt.TimeSlot}");
                Console.WriteLine($"Status: {apt.Status}");
                Console.WriteLine("---");
            }

            Console.WriteLine("\n8. System Logs (Singleton Pattern)...\n");
            var logs = HospitalLogger.Instance.GetLogs();
            Console.WriteLine($"Total logs captured: {logs.Count}");

            Console.WriteLine("\n========================================");
            Console.WriteLine("   DEMONSTRATION COMPLETE");
            Console.WriteLine("========================================");
        }
    }
}
