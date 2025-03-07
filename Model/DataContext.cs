using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace TARpe23RESTapi.Model
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<Exercise>? ExerciseList { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Exercise>().HasData(
                new Exercise
                {
                    Id = 1,
                    Title = "kätekõverdused",
                    Description = "tavalised kätekõverdused kordamööda jalga tõstes",
                    Intensity = Exercise.ExerciseIntensity.Medium,
                    RecommendedDurationInSeconds = 40,
                    RecommendedTimeInSecondsBeforeExercise = 10,
                    RecommendedTimeInSecondsAfterExercise = 10,
                },
                new Exercise
                {
                    Id = 2,
                    Title = "slaalomhüpped",
                    Description = "kükist hüpped",
                    Intensity = Exercise.ExerciseIntensity.High,
                    RecommendedDurationInSeconds = 40,
                    RecommendedTimeInSecondsBeforeExercise = 10,
                    RecommendedTimeInSecondsAfterExercise = 10,
                    RestTimeInstructions = "venita reie esikülge"
                },
                new Exercise
                {
                    Id = 3,
                    Title = "alt läbi jooks",
                    Description = "toenglamangus jooksmine",
                    Intensity = Exercise.ExerciseIntensity.Medium,
                    RecommendedDurationInSeconds = 40,
                    RecommendedTimeInSecondsBeforeExercise = 10,
                    RecommendedTimeInSecondsAfterExercise = 10,
                }
            );
        }
    }
}