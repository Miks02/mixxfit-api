using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MixxFit.API.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ModifyExerciseEntryRelatedTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_SetEntrys_Reps_Positive",
                table: "SetEntries");

            migrationBuilder.DropCheckConstraint(
                name: "CK_SetEntrys_WeightKg_Positive",
                table: "SetEntries");

            migrationBuilder.DropCheckConstraint(
                name: "CK_ExerciseEntrys_AvgHeartRate_Positive",
                table: "ExerciseEntries");

            migrationBuilder.DropCheckConstraint(
                name: "CK_ExerciseEntrys_IntervalsCount_Positive",
                table: "ExerciseEntries");

            migrationBuilder.DropCheckConstraint(
                name: "CK_ExerciseEntrys_MaxHeartRate_GreaterThan_AvgHeartRate",
                table: "ExerciseEntries");

            migrationBuilder.DropCheckConstraint(
                name: "CK_ExerciseEntrys_MaxHeartRate_Positive",
                table: "ExerciseEntries");

            migrationBuilder.DropCheckConstraint(
                name: "CK_ExerciseEntrys_RestIntervalSec_Positive",
                table: "ExerciseEntries");

            migrationBuilder.DropCheckConstraint(
                name: "CK_ExerciseEntrys_WorkIntervalSec_Positive",
                table: "ExerciseEntries");

            migrationBuilder.DropColumn(
                name: "WeightKg",
                table: "SetEntries");

            migrationBuilder.DropColumn(
                name: "AvgHeartRate",
                table: "ExerciseEntries");

            migrationBuilder.DropColumn(
                name: "CaloriesBurned",
                table: "ExerciseEntries");

            migrationBuilder.DropColumn(
                name: "CardioType",
                table: "ExerciseEntries");

            migrationBuilder.DropColumn(
                name: "DistanceKm",
                table: "ExerciseEntries");

            migrationBuilder.DropColumn(
                name: "Duration",
                table: "ExerciseEntries");

            migrationBuilder.DropColumn(
                name: "IntervalsCount",
                table: "ExerciseEntries");

            migrationBuilder.DropColumn(
                name: "MaxHeartRate",
                table: "ExerciseEntries");

            migrationBuilder.DropColumn(
                name: "PaceMinPerKm",
                table: "ExerciseEntries");

            migrationBuilder.DropColumn(
                name: "RestIntervalSec",
                table: "ExerciseEntries");

            migrationBuilder.DropColumn(
                name: "WorkIntervalSec",
                table: "ExerciseEntries");

            migrationBuilder.AlterColumn<int>(
                name: "Reps",
                table: "SetEntries",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<decimal>(
                name: "Distance",
                table: "SetEntries",
                type: "numeric(6,2)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DurationSeconds",
                table: "SetEntries",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Weight",
                table: "SetEntries",
                type: "numeric(6,2)",
                nullable: true);

            migrationBuilder.AddCheckConstraint(
                name: "CK_SetEntries_Distance_Positive",
                table: "SetEntries",
                sql: "\"Distance\" IS NULL OR \"Distance\" > 0");

            migrationBuilder.AddCheckConstraint(
                name: "CK_SetEntries_DurationSeconds_Positive",
                table: "SetEntries",
                sql: "\"DurationSeconds\" IS NULL OR \"DurationSeconds\" BETWEEN 0 AND 59");

            migrationBuilder.AddCheckConstraint(
                name: "CK_SetEntries_Reps_Positive",
                table: "SetEntries",
                sql: "\"Reps\" IS NULL OR \"Reps\" > 0");

            migrationBuilder.AddCheckConstraint(
                name: "CK_SetEntries_Weight_Positive",
                table: "SetEntries",
                sql: "\"Weight\" IS NULL OR \"Weight\" > 0");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_SetEntries_Distance_Positive",
                table: "SetEntries");

            migrationBuilder.DropCheckConstraint(
                name: "CK_SetEntries_DurationSeconds_Positive",
                table: "SetEntries");

            migrationBuilder.DropCheckConstraint(
                name: "CK_SetEntries_Reps_Positive",
                table: "SetEntries");

            migrationBuilder.DropCheckConstraint(
                name: "CK_SetEntries_Weight_Positive",
                table: "SetEntries");

            migrationBuilder.DropColumn(
                name: "Distance",
                table: "SetEntries");

            migrationBuilder.DropColumn(
                name: "DurationSeconds",
                table: "SetEntries");

            migrationBuilder.DropColumn(
                name: "Weight",
                table: "SetEntries");

            migrationBuilder.AlterColumn<int>(
                name: "Reps",
                table: "SetEntries",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddColumn<double>(
                name: "WeightKg",
                table: "SetEntries",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "AvgHeartRate",
                table: "ExerciseEntries",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "CaloriesBurned",
                table: "ExerciseEntries",
                type: "double precision",
                precision: 7,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CardioType",
                table: "ExerciseEntries",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "DistanceKm",
                table: "ExerciseEntries",
                type: "double precision",
                precision: 5,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "Duration",
                table: "ExerciseEntries",
                type: "interval",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IntervalsCount",
                table: "ExerciseEntries",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MaxHeartRate",
                table: "ExerciseEntries",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "PaceMinPerKm",
                table: "ExerciseEntries",
                type: "double precision",
                precision: 5,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RestIntervalSec",
                table: "ExerciseEntries",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WorkIntervalSec",
                table: "ExerciseEntries",
                type: "integer",
                nullable: true);

            migrationBuilder.AddCheckConstraint(
                name: "CK_SetEntrys_Reps_Positive",
                table: "SetEntries",
                sql: "\"Reps\" > 0");

            migrationBuilder.AddCheckConstraint(
                name: "CK_SetEntrys_WeightKg_Positive",
                table: "SetEntries",
                sql: "\"WeightKg\" > 0");

            migrationBuilder.AddCheckConstraint(
                name: "CK_ExerciseEntrys_AvgHeartRate_Positive",
                table: "ExerciseEntries",
                sql: "\"AvgHeartRate\" > 0");

            migrationBuilder.AddCheckConstraint(
                name: "CK_ExerciseEntrys_IntervalsCount_Positive",
                table: "ExerciseEntries",
                sql: "\"IntervalsCount\" > 0");

            migrationBuilder.AddCheckConstraint(
                name: "CK_ExerciseEntrys_MaxHeartRate_GreaterThan_AvgHeartRate",
                table: "ExerciseEntries",
                sql: "\"MaxHeartRate\" >= \"AvgHeartRate\"");

            migrationBuilder.AddCheckConstraint(
                name: "CK_ExerciseEntrys_MaxHeartRate_Positive",
                table: "ExerciseEntries",
                sql: "\"MaxHeartRate\" > 0");

            migrationBuilder.AddCheckConstraint(
                name: "CK_ExerciseEntrys_RestIntervalSec_Positive",
                table: "ExerciseEntries",
                sql: "\"RestIntervalSec\" > 0");

            migrationBuilder.AddCheckConstraint(
                name: "CK_ExerciseEntrys_WorkIntervalSec_Positive",
                table: "ExerciseEntries",
                sql: "\"WorkIntervalSec\" > 0");
        }
    }
}
