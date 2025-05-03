using System;
using System.Collections.Generic;  // Para List<T> e outras coleções

namespace User.Models
{
    public class Run
    {
        public Guid Id { get; set; } = Guid.NewGuid(); 
        public double Distance { get; set; } 
        public TimeSpan Duration { get; set; } 
        public DateTime Date { get; set; }
        public string? City { get; set; } 
        public string? TrainingType { get; set; } 
        public string? Weather { get; set; } 
        public double? Temperature { get; set; } 
        public string? Notes { get; set; }

        // Lista de pontos de localização (latitude, longitude) ao longo do percurso
        public List<LocationPoint> Locations { get; set; } = new List<LocationPoint>();
        
        // Relationship with User
        public Guid UserId { get; set; } 
        public required User User { get; set; } 

         // Status da corrida (para saber se está em andamento ou já finalizada)
        public RunStatus Status { get; set; } = RunStatus.InProgress;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;  // Quando foi criado
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;  // Quando foi atualizado
    }

    public enum RunStatus
    {
        InProgress,
        Completed,
        Paused
    }
}
