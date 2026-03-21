using System.ComponentModel;
using Microsoft.AspNetCore.Mvc;

namespace MixxFit.API.Features.Exercises.GetExercises;

public record GetExercisesRequest
{
    [FromQuery(Name = "onlyUserDefined")] 
    [DefaultValue(false)]
    public bool? OnlyUserDefined { get; init; }
    
    [FromQuery(Name = "searchTerm")]
    public string? SearchTerm { get; init; }
    
    [FromQuery(Name = "categoryId")]
    public int? CategoryId { get; set; }
    
    [FromQuery(Name = "muscleGroupId")]
    public int? MuscleGroupId { get; set; }
    
}